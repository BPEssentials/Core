using System;
using System.Threading;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands {
    public class ClearChat : EssentialsChatPlugin {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, false, message).Trim();
            if (arg1 == "all" || arg1 == "everyone")
            {
                if (AdminsListPlayers.Contains(player.playerData.username))
                {
                    for (var i = 0; i < 6; i++)
                        player.SendToAll(Channel.Unsequenced, 10, " ");
                    player.SendToAll(Channel.Unsequenced, 10, $"<color={argColor}>{player.playerData.username}</color><color={warningColor}> Cleared the chat for everyone.</color>");
                }
                else
                    player.SendToSelf(Channel.Unsequenced, 10, MsgNoPerm);
            }
            else
            {

                for (var i = 0; i < 6; i++)
                    player.SendToSelf(Channel.Unsequenced, 10, " ");
                player.SendToSelf(Channel.Unsequenced, 10, $"<color={warningColor}>Cleared the chat for yourself.</color>");
            }
        }
    }
}
