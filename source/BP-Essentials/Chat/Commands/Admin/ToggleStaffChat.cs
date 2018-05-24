using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.Text.RegularExpressions;

namespace BP_Essentials.Commands
{
    class ToggleStaffChat : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (HasPermission.Run(player, CmdStaffChatExecutableBy))
                {
                    var arg1 = GetArgument.Run(1, false, true, message);
                    var shplayer = GetShBySv.Run(player);
                    if (string.IsNullOrEmpty(arg1))
                    {
                        if (playerList[shplayer.ID].staffChatEnabled)
                        {
                            playerList[shplayer.ID].staffChatEnabled = false;
                            player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Staff chat disabled.</color>");
                        }
                        else
                        {
                            playerList[shplayer.ID].staffChatEnabled = true;
                            player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Staff chat enabled.</color>");
                        }
                    }
                    else
                    {
                        if (playerList[shplayer.ID].staffChatEnabled)
                        {
                            _msg = AdminChatMessage;
                            _msg = _msg.Replace("{username}", new Regex("(<)").Replace(player.playerData.username, "<<b></b>"));
                            _msg = _msg.Replace("{message}", new Regex("(<)").Replace(Chat.LangAndChatBlock.Run(player, arg1), "<<b></b>"));
                            SendChatMessageToAdmins.Run(_msg);
                        }
                        else
                            player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={warningColor}>You must enable staff chat first before you can send a message.</color>");
                    }
                }
                else
                    player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, MsgNoPerm);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}
