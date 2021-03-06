﻿using Lomztein.Moduthulhu.Core.Plugins.Framework;
using Lomztein.AdvDiscordCommands.ExampleCommands;
using Lomztein.AdvDiscordCommands.Framework.Interfaces;

namespace Lomztein.Moduthulhu.Plugins.Standard
{
    [Dependency ("Moduthulhu-Command Root")]
    [Descriptor("Moduthulhu", "Standard Commands", "Implements all the default commands from the command framework.")]
    [Source("https://github.com/Lomztein", "https://github.com/Lomztein/Moduthulhu/blob/master/Core/Plugin/Standard%20Plugins/Command%20Root/StandardCommandsPlugin.cs")]
    public class StandardCommandsPlugin : PluginBase {

        private readonly ICommand [ ] _commands = new ICommand[] {
                new DiscordCommandSet (),
                new FlowCommandSet (),
                new MathCommandSet (),
                new VariableCommandSet (),
                new CallstackCommand (),
                new PrintCommand (),
        };

        public override void Initialize() {
            SendMessage ("Moduthulhu-Command Root", "AddCommands", _commands);
        }

        public override void Shutdown() {
            SendMessage ("Moduthulhu-Command Root", "RemoveCommands", _commands);
        }
    }
}
