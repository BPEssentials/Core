using static BP_Essentials.Variables;
using System;
using static BP_Essentials.HookMethods;
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
                player.SendChatMessage(ArgRequired);
                return;
            }
            if (WipePassword == "default")
            {
                player.SendChatMessage($"<color={errorColor}>Password was not changed yet, cannot use this command.</color>");
                return;
            }
            if (arg1 != WipePassword)
            {
                player.SendChatMessage($"<color={errorColor}>Invalid password.</color>");
                return;
            }
            Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] Wipe command ran by {player.player.username}, password matched, deleting all save files.");
            foreach (var currPlayer in PlayerList.Values.ToList())
                player.svManager.Disconnect(currPlayer.ShPlayer.svPlayer.connection, DisconnectTypes.Problem);
            Thread.Sleep(500);
            var files = Directory.GetFiles(Path.Combine(Application.persistentDataPath, "PlayerData/"), "*.json").Where(item => item.EndsWith(".json", StringComparison.CurrentCulture));
            foreach (var file in files.ToList())
                File.Delete(file);
            Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] All user data deleted!");
        }
    }
}