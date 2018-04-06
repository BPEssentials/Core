using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using static BP_Essentials.EssentialsMethodsPlugin;
using static BP_Essentials.EssentialsVariablesPlugin;
namespace BP_Essentials
{
    public class EssentialsChatPlugin : MonoBehaviour {
        #region Event: ChatMessage
        //Chat Events
        [Hook("SvPlayer.SvGlobalChatMessage")]
        public static bool SvGlobalChatMessage(SvPlayer player, ref string message)
        {
            try
            {

                //Message Logging
                if (!(MutePlayers.Contains(player.playerData.username)))
                    LogMessage.Run(player, message);
                //AFK Handling
                if (message.StartsWith(CmdAfk) || message.StartsWith(CmdAfk2))
                {
                    if (!CmdAfkDisabled)
                        return Commands.Afk.Run(player);
                    else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }
                }
                else if (AfkPlayers.Contains(player.playerData.username))
                    Commands.Afk.Run(player);
                if (message.StartsWith(CmdCommandCharacter))
                {
                    // CustomCommands
                    if (CustomCommands.Any(message.Contains))
                    {
                        var i = 0;
                        foreach (var command in CustomCommands)
                            if (message.StartsWith(command))
                                i = CustomCommands.IndexOf(command);
                        string[] lines = Responses[i].Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                        foreach (string line in lines)
                            player.SendToSelf(Channel.Unsequenced, 10, GetPlaceHolders.Run(line, player));
                        return true;
                    }

                    else if (message.StartsWith(CmdClearChat) || message.StartsWith(CmdClearChat2))
                        if (!CmdClearChatDisabled)
                            return Commands.ClearChat.Run(player, message);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }
                    else if (message.StartsWith("/essentials") || message.StartsWith("/ess"))
                        return Commands.Essentials.Run(player, message);
                    else if (message.StartsWith(CmdDebug) || (message.StartsWith(CmdDebug2)))
                        return Commands.DebugCommands.Run(player, message);
                    else if (message.StartsWith(CmdGodmode) || message.StartsWith(CmdGodmode2))
                        if (!CmdGodmodeDisabled)
                            return Commands.GodMode.Run(player, message);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }
                    else if (message.StartsWith(CmdPay) || message.StartsWith(CmdPay2))
                        if (!CmdPayDisabled)
                            return Commands.Pay.Run(player, message);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }
                    else if (message.StartsWith(CmdSave))
                        return Commands.Save.Run(player);
                    else if (message.StartsWith(CmdBan))
                        return Commands.Ban.Run(player, message);
                    else if (message.StartsWith(CmdKick))
                        return Commands.Kick.Run(player, message);
                    else if (message.StartsWith(CmdRestrain))
                        return Commands.Restrain.Run(player, message);
                    else if (message.StartsWith(CmdArrest))
                        return Commands.Arrest.Run(player, message);
                    else if (message.StartsWith(CmdFree))
                        return Commands.Free.Run(player, message);
                    else if (message.StartsWith(CmdTpHere) || message.StartsWith(CmdTpHere2))
                        return Commands.Tp.TpHere(player, message);
                    else if (message.StartsWith(CmdTp))
                        return Commands.Tp.Run(player, message);
                    else if (message.StartsWith(CmdKill))
                        return Commands.Kill.Run(player, message);
                    else if (message.StartsWith(CmdReload) || message.StartsWith(CmdReload2))
                        return Commands.Reload.Run(player);
                    else if (message.StartsWith(CmdMute) || message.StartsWith(CmdMute2))
                        if (!CmdMuteDisabled)
                            return Commands.Mute.Run(player, message);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }
                    else if (message.StartsWith(CmdSay) || message.StartsWith(CmdSay2))
                        if (!CmdSayDisabled)
                            return Commands.Say.Run(player, message);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }
                    else if (message.StartsWith(CmdInfo) || message.StartsWith(CmdInfo2))
                        if (!CmdInfoDisabled)
                            return Commands.Info.Run(player, message);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }

                    else if (message.StartsWith(CmdLogs))
                        return Commands.GetLogs.Run(player, ChatLogFile);

                    else if (message.StartsWith(CmdPlayers) || message.StartsWith(CmdPlayers2))
                        if (!CmdPlayersDisabled)
                            return Commands.OnlinePlayers.Run(player);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }
                    else if (message.StartsWith(CmdGive) || (message.StartsWith(CmdGive2)))
                        if (!CmdGiveDisabled)
                            return Commands.Give.Run(player, message);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }
                    else if (message.StartsWith(CmdFakeJoin) || (message.StartsWith(CmdFakeJoin2)))
                        if (!CmdFakeJoinDisabled)
                            return Commands.FakeJoin.Run(player, message);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }
                    else if (message.StartsWith(CmdFakeLeave) || (message.StartsWith(CmdFakeLeave2)))
                        if (!CmdFakeLeaveDisabled)
                            return Commands.FakeLeave.Run(player, message);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }
                    else if (message.StartsWith(CmdAtm) || (message.StartsWith(CmdAtm2)))
                        if (!CmdAtmDisabled)
                            return Commands.Atm.Run(player);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }
                    else if (message.StartsWith(CmdCheckAlts) || (message.StartsWith(CmdCheckAlts2)))
                        if (!CmdCheckAltsDisabled)
                            return Commands.CheckAlts.Run(player, message);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }
                    else if (message.StartsWith(CmdHeal) || message.StartsWith(CmdHeal2))
                        return Commands.Heal.Run(player, message);
                    else if (message.StartsWith(CmdFeed) || message.StartsWith(CmdFeed2))
                        return Commands.Feed.Run(player, message);
                    else if (message.StartsWith(CmdLatestVoteResults) || message.StartsWith(CmdLatestVoteResults2))
                        if (!CmdLatestVoteResultsDisabled)
                            return Commands.LatestVoteResults.Run(player);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }
                    else if (message.StartsWith(CmdMoney) || message.StartsWith(CmdMoney2))
                        if (!CmdMoneyDisabled)
                            return Commands.Money.Run(player, message);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }
                    else if (message.StartsWith(CmdSetjob) || message.StartsWith(CmdSetjob2))
                        if (!CmdSetjobDisabled)
                            return Commands.SetJob.Run(player, message);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }
                    else if (message.StartsWith(CmdClearWanted) || message.StartsWith(CmdClearWanted2))
                        if (!CmdClearWantedDisabled)
                            return Commands.ClearWanted.Run(player, message);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }

                    else if (message.StartsWith(CmdReport) || message.StartsWith(CmdReport2))
                        if (!CmdReportDisabled)
                            return Commands.Report.Run(player, message);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }
                    else if (message.StartsWith(CmdLaunch) || message.StartsWith(CmdLaunch2))
                        if (!CmdLaunchDisabled)
                            return Commands.Launch.Run(player, message);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }
                    else if (message.StartsWith(CmdStrip) || message.StartsWith(CmdStrip2))
                        if (!CmdStripDisabled)
                            return Commands.Strip.Run(player, message);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }
                    else if (message.StartsWith(CmdSlap) || message.StartsWith(CmdSlap2))
                        if (!CmdSlapDisabled)
                            return Commands.Slap.Run(player, message);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }

                    else if (message.StartsWith(CmdSearch) || message.StartsWith(CmdSearch2))
                        if (!CmdSearchDisabled)
                            return Commands.Search.Run(player, message);
                        else
                        { player.SendToSelf(Channel.Unsequenced, 10, DisabledCommand); return true; }
                    else if (message.StartsWith(CmdConfirm) || message.StartsWith(CmdConfirm2))
                        return Commands.Confirm.Run(player);
                    if (MsgUnknownCommand)
                    {
                        player.SendToSelf(Channel.Unsequenced, 10, $"<color={errorColor}>Unknown command. Type</color><color={argColor}> {CmdCommandCharacter}essentials cmds </color><color={errorColor}>for more info.</color>");
                        return true;
                    }
                }
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

                // Will improve this someday..
                foreach (KeyValuePair<string, _Group> curr in Groups)
                {
                    if (curr.Value.Users.Contains(player.playerData.username))
                    {
                        _msg = curr.Value.Message;
                        _msg = _msg.Replace("{username}", new Regex("(<)").Replace(player.playerData.username, "<<b></b>"));
                        _msg = _msg.Replace("{message}", new Regex("(<)").Replace(Chat.LangAndChatBlock.Run(player, message), "<<b></b>"));
                        player.SendToAll(Channel.Unsequenced, 10, _msg);
                        return true;
                    }
                }
                if (AdminsListPlayers.Contains(player.playerData.username))
                {
                    _msg = AdminMessage;
                    _msg = _msg.Replace("{username}", new Regex("(<)").Replace(player.playerData.username, "<<b></b>"));
                    _msg = _msg.Replace("{message}", new Regex("(<)").Replace(Chat.LangAndChatBlock.Run(player, message), "<<b></b>"));
                    player.SendToAll(Channel.Unsequenced, 10, _msg);
                    return true;
                }
                _msg = PlayerMessage;
                _msg = _msg.Replace("{username}", new Regex("(<)").Replace(player.playerData.username, "<<b></b>"));
                _msg = _msg.Replace("{message}", new Regex("(<)").Replace(Chat.LangAndChatBlock.Run(player, message), "<<b></b>"));
                player.SendToAll(Channel.Unsequenced, 10, _msg);
                return true;
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
                return true;
            }
        }
        #endregion
    }
}