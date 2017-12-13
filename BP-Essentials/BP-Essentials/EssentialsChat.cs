using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Threading;
using UnityEngine;
using static BP_Essentials.EssentialsMethodsPlugin;
using static BP_Essentials.EssentialsVariablesPlugin;
namespace BP_Essentials
{
    public class EssentialsChatPlugin : EssentialsCorePlugin{
        #region Event: ChatMessage
        //Chat Events
        [Hook("SvPlayer.SvGlobalChatMessage")]
        public static bool SvGlobalChatMessage(SvPlayer player, ref string message)
        {
            //Message Logging
            if (!(MutePlayers.Contains(player.playerData.username)))
                LogMessage.Run(player, message);
            //AFK Handling
            if (AfkPlayers.Contains(player.playerData.username))
                Commands.Afk.Run(player);

            // CustomCommands
            if (CustomCommands.Any(message.Contains))
            {
                var i = 0;
                foreach (var command in CustomCommands)
                    if (message.StartsWith(command))
                        i = CustomCommands.IndexOf(command);
                player.SendToSelf(Channel.Unsequenced, (byte)10, GetPlaceHolders.Run(i, player));
                return true;
            }
            else if (message.StartsWith(CmdClearChat) || message.StartsWith(CmdClearChat2))
                return Commands.ClearChat.Run(player, message);
            else if (message.StartsWith(CmdAfk) || message.StartsWith(CmdAfk2))
                return Commands.Afk.Run(player);
            else if (message.StartsWith("/essentials") || message.StartsWith("/ess"))
                return Commands.Essentials.Run(player, message);
            else if (message.StartsWith(CmdDbug))
                return Commands.DebugCommands.Run(player, message);
            else if (message.StartsWith(CmdGodmode) || message.StartsWith(CmdGodmode2))
                return Commands.GodMode.Run(player, message);
            else if (message.StartsWith(CmdPay) || message.StartsWith(CmdPay2))
                return Commands.Pay.Run(player, message);
            else if (message.StartsWith(CmdSave))
                return Commands.Save.Run(player);
            else if (message.StartsWith(CmdTpHere) || message.StartsWith(CmdTpHere2))
                return Commands.Tp.TpHere(player, message);
            else if (message.StartsWith(CmdTp))
                return Commands.Tp.Run(player, message);
            else if (message.StartsWith(CmdBan))
                return Commands.Ban.Run(player, message);
            else if (message.StartsWith(CmdReload) || message.StartsWith(CmdReload2))
                return Commands.Reload.Run(player);
            else if (message.StartsWith(CmdMute) || message.StartsWith(CmdUnMute))
                return Commands.Mute.Run(player, message);
            else if (message.StartsWith(CmdSay) || message.StartsWith(CmdSay2))
                return Commands.Say.Run(player, message);
            else if (message.StartsWith(CmdInfo) || message.StartsWith(CmdInfo2))
                return Commands.Info.Run(player, message);
            else if (message.StartsWith(CmdBan))
                return Commands.Ban.Run(player, message);
            else if (message.StartsWith(CmdCheckIp))
                return Commands.CheckIp.Run(player, message);
            else if (message.StartsWith(CmdCheckPlayer))
                return Commands.CheckPlayer.Run(player, message);
            else if (message.StartsWith(CmdLogs))
                return Commands.GetLogs.Run(player, ChatLogFile);
            else if (message.StartsWith(CmdPlayers) || message.StartsWith(CmdPlayers2))
                return Commands.OnlinePlayers.Run(player);
            else if (message.StartsWith(CmdRules))
                return Commands.Rules.Run(player);
            else if (message.StartsWith(CmdDiscord))
                return Commands.Discord.Run(player);
            else if (message.StartsWith(CmdFakeJoin))
                return Commands.FakeJoin.Run(player, message);
            else if (message.StartsWith(CmdFakeLeave))
                return Commands.FakeLeave.Run(player, message);
            //Checks if the player is muted.
            if (MutePlayers.Contains(player.playerData.username))
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, "You are muted.");
                return true;
            }
            //Checks if the message contains a username that is AFK.
            if (AfkPlayers.Any(message.Contains))
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, "That player is AFK.");
                return true;
            }
            // Checks if Chatblock AND LanguageBlock are disabled, if so, return false.
            if (!ChatBlock && !LanguageBlock) return false;
            //Checks if the message is a blocked one, if it is, block it.
            return Chat.LangAndChatBlock.Run(player, message);
        }

        #endregion
    }
}