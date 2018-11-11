using static BP_Essentials.EssentialsVariablesPlugin;
using System;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Threading;

namespace BP_Essentials.Commands
{
    public class Wipe
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1))
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
                return;
            }
            if (WipePassword == "default")
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>Password was not changed yet, cannot use this command.</color>");
                return;
            }
            if (arg1 != WipePassword)
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>Invalid password.</color>");
                return;
            }
            Debug.Log($"{SetTimeStamp.Run()}[INFO] Wipe command ran by {player.player.username}, password matched, deleting all save files.");
            foreach (var currPlayer in playerList.Values)
                player.svManager.Disconnect(currPlayer.Shplayer.svPlayer.connection);
            Thread.Sleep(500);
            foreach (string file in Directory.GetFiles(Path.Combine(Application.persistentDataPath, "PlayerData/"), "*.json").Where(item => item.EndsWith(".json")))
                File.Delete(file);
            Debug.Log($"{SetTimeStamp.Run()}[INFO] All user data deleted!");
        }
    }
}