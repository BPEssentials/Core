using System;
using System.Threading;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    public class ClearChat
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, false, message);
            if (arg1 == "all" || arg1 == "everyone")
            {
                if (!player.player.admin)
                {
                    player.SendChatMessage(MsgNoPerm);
                    return;
                }
                for (var i = 0; i < 6; i++)
                    player.SendChatMessage(" ");
                player.SendChatMessage($"<color={argColor}>{player.playerData.username}</color><color={warningColor}> Cleared the chat for everyone.</color>");
            }
            else
            {
                for (var i = 0; i < 6; i++)
                    player.SendChatMessage(" ");
                player.SendChatMessage($"<color={warningColor}>Cleared the chat for yourself.</color>");
            }
        }
    }
}
