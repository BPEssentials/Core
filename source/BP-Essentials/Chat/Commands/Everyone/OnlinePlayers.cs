using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class OnlinePlayers
    {
        public static void Run(SvPlayer player, string message)
        {
            var realPlayers = UnityEngine.Object.FindObjectsOfType<ShPlayer>().Count(shPlayer => !shPlayer.svPlayer.serverside);
            switch (realPlayers)
            {
                case 1:
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>There is </color><color={argColor}>{realPlayers}</color><color={infoColor}> player online</color>");
                    break;
                default:
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>There are </color><color={argColor}>{realPlayers}</color><color={infoColor}> players online</color>");
                    break;
            }
        }
    }
}
