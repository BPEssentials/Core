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
    public class EssentialsChatPlugin : SvPlayer{
        #region Event: ChatMessage
        //Chat Events
        [Hook("SvPlayer.SvGlobalChatMessage")]
        public static bool SvGlobalChatMessage(SvPlayer player, ref string message)
        {
            // ------------------------------------------------------
            //test command, remove when done
            Commands.TestCommand.Run(player, message);
            //test command, remove when done
            // ------------------------------------------------------


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
                string[] lines = Responses[i].Split(new[] { Environment.NewLine },StringSplitOptions.None);
                foreach (string line in lines)
                    player.SendToSelf(Channel.Unsequenced, 10, GetPlaceHolders.Run(line, player));
                return true;
            }

            else if (message.StartsWith(CmdClearChat) || message.StartsWith(CmdClearChat2))
                if (!CmdClearChatDisabled)
                    return Commands.ClearChat.Run(player, message);
                else
                    player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand);
            else if (message.StartsWith(CmdAfk) || message.StartsWith(CmdAfk2))
                if (!CmdAfkDisabled)
                    return Commands.Afk.Run(player);
                else
                    player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand);
            else if (message.StartsWith("/essentials") || message.StartsWith("/ess"))
                return Commands.Essentials.Run(player, message);
            else if (message.StartsWith(CmdDebug) || (message.StartsWith(CmdDebug2)))
                return Commands.DebugCommands.Run(player, message);
            else if (message.StartsWith(CmdGodmode) || message.StartsWith(CmdGodmode2))
                if (!CmdGodmodeDisabled)
                    return Commands.GodMode.Run(player, message);
                else
                    player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand);
            else if (message.StartsWith(CmdPay) || message.StartsWith(CmdPay2))
                if (!CmdPayDisabled)
                    return Commands.Pay.Run(player, message);
                else
                    player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand);
            else if (message.StartsWith(CmdSave))
                return Commands.Save.Run(player);
            else if (message.StartsWith(CmdTpHere) || message.StartsWith(CmdTpHere2))
                if (!CmdTpHereDisabled)
                    return Commands.Tp.TpHere(player, message);
                else
                    player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand);
            else if (message.StartsWith(CmdTp))
                return Commands.Tp.Run(player, message);
            else if (message.StartsWith(CmdBan))
                return Commands.Ban.Run(player, message);
            else if (message.StartsWith(CmdReload) || message.StartsWith(CmdReload2))
                return Commands.Reload.Run(player);
            else if (message.StartsWith(CmdMute) || message.StartsWith(CmdMute2))
                if (!CmdMuteDisabled)
                    return Commands.Mute.Run(player, message);
                else
                    player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand);
            else if (message.StartsWith(CmdSay) || message.StartsWith(CmdSay2))
                if (!CmdSayDisabled)
                    return Commands.Say.Run(player, message);
                else
                    player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand);
            else if (message.StartsWith(CmdInfo) || message.StartsWith(CmdInfo2))
                if (CmdInfoDisabled)
                    return Commands.Info.Run(player, message);
                else
                    player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand);

            else if (message.StartsWith(CmdLogs))
                return Commands.GetLogs.Run(player, ChatLogFile);
            else if (message.StartsWith(CmdPlayers) || message.StartsWith(CmdPlayers2))
                if (!CmdPlayersDisabled)
                    return Commands.OnlinePlayers.Run(player);
                else
                    player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand);
            else if (message.StartsWith(CmdRules) || message.StartsWith(CmdRules2))
                    return Commands.Rules.Run(player);
            else if (message.StartsWith(CmdGive) || (message.StartsWith(CmdGive2)))
                if (!CmdGiveDisabled)
                    return Commands.Give.Run(player, message );
                else
                    player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand);
            else if (message.StartsWith(CmdFakeJoin) || (message.StartsWith(CmdFakeJoin2)))
                if (!CmdFakeJoinDisabled)
                    return Commands.FakeJoin.Run(player, message);
                else
                    player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand);
            else if (message.StartsWith(CmdFakeLeave) || (message.StartsWith(CmdFakeLeave2)))
                if (!CmdFakeLeaveDisabled)
                    return Commands.FakeLeave.Run(player, message);
                else
                    player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand);
            else if (message.StartsWith(CmdAtm) || (message.StartsWith(CmdAtm2)))
                if (!CmdAtmDisabled)
                    return Commands.Atm.Run(player);
                else
                    player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand);
            else if (message.StartsWith(CmdCheckAlts) || (message.StartsWith(CmdCheckAlts2)))
                if (!CmdCheckAltsDisabled)
                    return Commands.CheckAlts.Run(player, message);
                else
                    player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand);
            else if (message.StartsWith(CmdHeal) || message.StartsWith(CmdHeal2))
                return Commands.Heal.Run(player, message);
            else if (message.StartsWith(CmdFeed) || message.StartsWith(CmdFeed2))
                return Commands.Feed.Run(player, message);
            else if (message.StartsWith(CmdLatestVoteResults) || message.StartsWith(CmdLatestVoteResults2))
                if (!CmdLatestVoteResultsDisabled)
                    return Commands.LatestVoteResults.Run(player);
                else
                    player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand);
            else if (message.StartsWith(CmdMoney) || message.StartsWith(CmdMoney2))
                if (!CmdMoneyDisabled)
                    return Commands.Money.Run(player, message);
                else
                    player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand);
            //Checks if the player is muted.
            if (MutePlayers.Contains(player.playerData.username))
            {
                player.SendToSelf(Channel.Unsequenced, 10, SelfIsMuted);
                return true;
            }
            //Checks if the message contains a username that is AFK.
            if (AfkPlayers.Any(message.Contains))
            {
                player.SendToSelf(Channel.Unsequenced, 10, PlayerIsAFK);
                return true;
            }
            // Checks if Chatblock AND LanguageBlock are disabled, if so, return false.
            if (!ChatBlock && !LanguageBlock)
                return false;
            //Checks if the message is a blocked one, if it is, block it.
            return Chat.LangAndChatBlock.Run(player, message);
        }

        #endregion
    }
}