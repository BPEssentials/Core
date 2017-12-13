using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class OnlinePlayers : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer)
        {
            var player = (SvPlayer)oPlayer;
            var realPlayers = UnityEngine.Object.FindObjectsOfType<ShPlayer>().Count(shPlayer => shPlayer.IsRealPlayer());
            switch (realPlayers)
            {
                case 1:
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "There is " + realPlayers + " player online");
                    break;
                default:
                    if (realPlayers < 1)
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "There are " + realPlayers + " play- wait, how is that possible");
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "There are " + realPlayers + " player(s) online");
                    break;
            }
            return true;
        }
    }
}
