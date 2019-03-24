using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using static BP_Essentials.Variables;

namespace BP_Essentials
{
    public static class ExtensionMethods
    {
        public static void SavePosition(this SvPlayer player)
        {
            player.SavePosition(player.player.GetPosition(), player.player.GetRotation(), player.player.GetPlaceIndex());
        }
        public static void SavePosition(this SvPlayer player, Vector3 position, Quaternion rotation, int index)
        {
            if (!PlayerList.TryGetValue(player.player.ID, out var playerValue))
                return;
            playerValue.LastLocation.Update(position, rotation, index);
        }
        public static void ResetAndSavePosition(this SvPlayer player, Vector3 position, Quaternion rotation, int index)
        {
            player.SavePosition();
            player.SvReset(position, rotation, index);
        }
        public static void ResetAndSavePosition(this SvPlayer player, SvPlayer targetPlayer)
        {
            player.SavePosition();
            player.SvReset(targetPlayer.player.GetPosition(), targetPlayer.player.GetRotation(), targetPlayer.player.GetPlaceIndex());
        }

    public static void SendChatMessage(this ShPlayer player, string message) => player.svPlayer.SendChatMessage(message);
        public static void SendChatMessage(this SvPlayer player, string message) => player.SendGameMessage(SvSendType.Self, message);
        public static void SendChatMessageToAll(this SvPlayer player, string message) => player.SendGameMessage(SvSendType.All, message);
        public static void SendGameMessage(this SvPlayer player, SvSendType type, string message) => player.Send(type, Channel.Unsequenced, ClPacket.GameMessage, message);

        public static bool WillDieByDamage(this SvPlayer player, float damage) => (player.player.health - damage) <= 0;

        public static string FilterString(this string str) => new Regex("(<)").Replace(str, "<<b></b>");

        public static void CloseFunctionMenu(this SvPlayer player) => player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.CloseFunctionMenu);
        public static void CloseAndResetFunctionMenu(this SvPlayer player)
        {
            player.CloseFunctionMenu();
            if (PlayerList.TryGetValue(player.player.ID, out var playerListItem))
                return;
            playerListItem.LastMenu = CurrentMenu.None;
        }

    }
}
