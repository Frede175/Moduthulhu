﻿using Lomztein.AdvDiscordCommands.Framework.Interfaces;
using Lomztein.Moduthulhu.Core.Plugins.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lomztein.Moduthulhu.Plugins.Standard.Utilities
{
    [Descriptor("Lomztein", "Utilities", "General miscellaneous utility commands such as Roll the Dice and Coinflip. Pull requests that add new ones are welcome! See Plugin URL for source repository.")]
    [Source("https://github.com/Lomztein", "https://github.com/Lomztein/Moduthulhu/blob/master/Plugins/Utilities")]
    [Dependency("Lomztein-Command Root")]
    public class UtilitiesPlugin : PluginBase
    {
        private ICommand[] _commands =
        {
            new RollTheDice (),
            new FlipCoin (),
            new Embolden (),
            new Fizzfyr13 (),
        };

        public override void Initialize()
        {
            SendMessage("Lomztein-Command Root", "AddCommands", _commands);
        }

        public override void Shutdown()
        {
            SendMessage("Lomztein-Command Root", "RemoveCommands", _commands);
        }
    }
}
