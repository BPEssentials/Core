﻿using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Launch : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target = null)
        {
            target = target ?? player;
            target.svPlayer.SvForce(new UnityEngine.Vector3(0f, 6500f, 0f));
            target.TS("launched_target");
            player.TS("launched", target.username.SanitizeString());
        }
    }
}
