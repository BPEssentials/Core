﻿using BPEssentials.Abstractions;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Sudo : BpeCommand
    {
        public void Invoke(ShPlayer player, ShPlayer target, string message)
        {
            target.svPlayer.SvChatGlobal(message);
        }
    }
}
