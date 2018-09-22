﻿using Discord.WebSocket;
using Lomztein.AdvDiscordCommands.Extensions;
using Lomztein.AdvDiscordCommands.Framework;
using Lomztein.Moduthulhu.Core.Configuration;
using Lomztein.Moduthulhu.Core.Module.Framework;
using Lomztein.Moduthulhu.Modules.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lomztein.Moduthulhu.Core.Extensions;
using Lomztein.AdvDiscordCommands.Framework.Categories;

namespace Lomztein.Moduthulhu.Modules.Misc.Color
{
    public class ColourModule : ModuleBase, IConfigurable<MultiConfig> {

        public const string PREFIX = "cl_";

        public override string Name => "COLOURS";
        public override string Description => "This module is scientifically proven to improve funkyness by about 4%.";
        public override string Author => "Lomztein";

        public override string [ ] RequiredModules => new string [ ] { "Lomztein_Command Root" };

        public override bool Multiserver => true;

        public MultiConfig Configuration { get; set; } = new MultiConfig ();

        [AutoConfig] private MultiEntry<bool, SocketGuild> autocolourNewJoins = new MultiEntry<bool, SocketGuild> (x => false, "AutoColourNewMembers");
        [AutoConfig] private MultiEntry<Dictionary<ulong, string>, SocketGuild> colourIdentification = new MultiEntry<Dictionary<ulong, string>, SocketGuild> (x => x.Roles.Where (y => y.Name.StartsWith (PREFIX)).ToDictionary (z => z.Id, z => z.Name.Substring (PREFIX.Length)), "ColourRoleIDs");

        private SetColour colorCommand = new SetColour ();

        public override void Initialize() {
            colorCommand.ParentModule = this;
            ParentModuleHandler.GetModule<CommandRootModule> ().commandRoot.AddCommands (colorCommand);
            ParentBotClient.discordClient.UserJoined += OnUserJoined;
        }

        private Task OnUserJoined(SocketGuildUser guildUser) {
            if (this.IsConfigured (guildUser.Guild.Id))
                GiveRandomColourAsync (guildUser);
            return Task.CompletedTask;
        }

        private async void GiveRandomColourAsync (SocketGuildUser guildUser) {
            Dictionary<ulong, string> localColours = colourIdentification.GetEntry (guildUser.Guild);
            SocketRole randomRole = ParentBotClient.GetRole (guildUser.Guild.Id, localColours.ElementAt (new Random ().Next (localColours.Count)).Key);
            await guildUser.AsyncSecureAddRole (randomRole);
        }

        public override void Shutdown() {
            ParentModuleHandler.GetModule<CommandRootModule> ().commandRoot.RemoveCommands (colorCommand);
            ParentBotClient.discordClient.UserJoined -= OnUserJoined;
        }

        public class SetColour : ModuleCommand<ColourModule> {

            public SetColour () {
                Name = "setcolour";
                Description = "Set your personal color to something funky.";
                Category = StandardCategories.Utility;
            }

            [Overload (typeof (void), "Set your colour to something cool!")]
            public async Task<Result> Execute(CommandMetadata data, string colorName) {

                if (ParentModule.IsConfigured (data.Message.GetGuild ().Id)) {

                    SocketGuildUser guildUser = data.Message.Author as SocketGuildUser;
                    IEnumerable<SocketRole> currentRoles = guildUser.Roles.Where (x => ParentModule.colourIdentification.GetEntry (guildUser.Guild).ContainsKey (x.Id));

                    SocketRole role = null;
                    string name = "";

                    foreach (var entry in ParentModule.colourIdentification.GetEntry (data.Message.GetGuild ())) {
                        if (entry.Value.ToUpper () == colorName.ToUpper ()) {
                            role = ParentModule.ParentBotClient.GetRole (data.Message.GetGuild ().Id, entry.Key);
                            name = entry.Value;
                            break;
                        }
                    }

                    await guildUser.RemoveRolesAsync (currentRoles);
                    if (role != null)
                        await guildUser.AsyncSecureAddRole (role);

                    return new Result (null, role == null ? "Failed to colour you, colour not found." : $"You've been succesfully coloured **{name}**!");
                }

                return new Result (null, null);
            }
        }
    }
}
