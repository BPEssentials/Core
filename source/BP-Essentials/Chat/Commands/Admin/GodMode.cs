using static BP_Essentials.HookMethods;
using static BP_Essentials.Variables;
using System;
using System.IO;
using UnityEngine;

namespace BP_Essentials.Commands
{
    public class GodMode
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1))
                arg1 = player.player.username;
            var currPlayer = GetShByStr.Run(arg1);
            if (currPlayer == null)
            {
                player.SendChatMessage(NotFoundOnline);
                return;
            }
            ReadFile.Run(GodListFile);
            var msg = $"<color={infoColor}>Godmode</color> <color={argColor}>{{0}}</color> <color={infoColor}>for</color> <color={argColor}>'{arg1}'</color><color={infoColor}>.</color>";
            if (GodListPlayers.Contains(currPlayer.username))
            {
                RemoveStringFromFile.Run(GodListFile, currPlayer.username);
                ReadFile.Run(GodListFile);
                player.SendChatMessage(string.Format(msg, "disabled"));
                return;
            }
            File.AppendAllText(GodListFile, currPlayer.username + Environment.NewLine);
            GodListPlayers.Add(currPlayer.username);
            player.SendChatMessage(string.Format(msg, "enabled"));
        }
    }
}