﻿using Lomztein.ModularDiscordBot.Core.Module.Framework;
using Lomztein.AdvDiscordCommands.Framework;
using System;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord;

namespace Lomztein.ModularDiscordBot.Modules.CommandRoot
{
    public class CommandRootModule : ModuleBase {

        public override string Name => "Command Root";
        public override string Description => "A base module for implementing the Advanced Discord Commands framework.";
        public override string Author => "Lomztein";

        public override bool Multiserver => true;

        public AdvDiscordCommands.Framework.CommandRoot commandRoot;

        public override void PreInitialize() {
            commandRoot = new AdvDiscordCommands.Framework.CommandRoot ();
        }

        public override void Initialize() {
            ParentBotClient.discordClient.MessageReceived += OnMessageRecieved;
        }

        private Task OnMessageRecieved(SocketMessage arg) {
            AwaitAndSend (arg);
            return Task.CompletedTask;
        }

        // This is neccesary since awaiting the result in the event would halt the rest of the bot, and we don't really want that.
        private async void AwaitAndSend (SocketMessage arg) {
            var result = await commandRoot.EnterCommand (arg as SocketUserMessage);
            await arg.Channel.SendMessageAsync (result?.message, false, result?.value as Embed);
        }

        public override void Shutdown() {
            ParentBotClient.discordClient.MessageReceived -= OnMessageRecieved;
        }
    }
}