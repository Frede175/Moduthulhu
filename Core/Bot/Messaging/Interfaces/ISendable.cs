﻿using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lomztein.Moduthulhu.Core.Bot.Messaging.Advanced
{
    public interface ISendable {
        Task SendAsync(IMessageChannel channel);
    }

    public interface ISendable<TResult> : ISendable {
        TResult Result { get; }
    }
}
