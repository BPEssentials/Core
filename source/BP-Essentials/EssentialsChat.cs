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
                var command = GetArgument.Run(0, false, false, message);
                if (message.StartsWith(CmdCommandCharacter))
                {
                    // CustomCommands
                    if (CustomCommands.Any(message.Contains))
                    {
                        var i = 0;
                        foreach (var command2 in CustomCommands)
                            if (message.StartsWith(command2))
                                i = CustomCommands.IndexOf(command2);
                        foreach (string line in Responses[i].Split(new[] { "\\r\\n", "\\r", "\\n" }, StringSplitOptions.None))
                            player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, GetPlaceHolders.Run(line, player));
                        return true;
                    }
                    // Go through all registered commands and check if the command that the user entered matches
                    foreach (var cmd in CommandList.Values)
                        if (cmd.commandCmds.Contains(command))
                        {
                            if (cmd.commandDisabled == true)
                            {
                                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, DisabledCommand);
                                return true;
                            }
                            if (HasPermission.Run(player, cmd.commandGroup, true))
                            {
                                // Improve (use LINQ, not that familiar with it though
                                foreach (var currPlayer in playerList.Values)
                                    if (currPlayer.spyEnabled && currPlayer.shplayer.svPlayer != player)
                                        currPlayer.shplayer.svPlayer.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color=#f4c242>[SPYCHAT]</color> {player.playerData.username}: {message}");
                                switch (cmd.commandName)
                                {
                                    // If anyone knows a way to improve this, let me know :)
                                    case nameof(Reload):
                                        Commands.Reload.Run(player);
                                        break;
                                    case "Atm":
                                        Commands.Atm.Run(player);
                                        break;
                                    case "Confirm":
                                        Commands.Confirm.Run(player);
                                        break;
                                    case "OnlinePlayers":
                                        Commands.OnlinePlayers.Run(player);
                                        break;
                                    case "ToggleChat":
                                        Commands.ToggleChat.Run(player);
                                        break;
                                    case "Afk":
                                        Commands.Afk.Run(player);
                                        break;
                                    case "LatestVoteResults":
                                        Commands.LatestVoteResults.Run(player);
                                        break;
                                    case "ToggleReceiveStaffChat":
                                        Commands.ToggleReceiveStaffChat.Run(player);
                                        break;
                                    case "Spy":
                                        Commands.Spy.Run(player);
                                        break;
                                    case "Save":
                                        Commands.Save.Run();
                                        break;
                                    case "GetLogs":
                                        Commands.GetLogs.Run(player, ChatLogFile);
                                        break;
                                    default:
                                        cmd.RunMethod.Invoke(player, message);
                                        break;
                                }
                            }
                            return true;
                        }
                    if (AfkPlayers.Contains(player.playerData.username))
                        Commands.Afk.Run(player);
                    if (MsgUnknownCommand)
                    {
                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>Unknown command. Type</color><color={argColor}> {CmdCommandCharacter}essentials cmds </color><color={errorColor}>for more info.</color>");
                        return true;
                    }
                }
                //Checks if the player is muted.
                if (MutePlayers.Contains(player.playerData.username))
                {
                    player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, SelfIsMuted);
                    return true;
                }
                //Checks if the message contains a username that is AFK.
                if (AfkPlayers.Any(message.Contains))
                    player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, PlayerIsAFK);

                var shplayer = player.player;
                if (!playerList[shplayer.ID].chatEnabled)
                {
                    player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={warningColor}>Please enable your chat again by typing</color> <color={argColor}>{CmdCommandCharacter}{CmdToggleChat}</color><color={warningColor}>.</color>");
                    return true;
                }
                if (playerList[shplayer.ID].staffChatEnabled)
                {
                    _msg = AdminChatMessage;
                    _msg = _msg.Replace("{username}", new Regex("(<)").Replace(shplayer.username, "<<b></b>"));
                    _msg = _msg.Replace("{id}", $"{shplayer.ID}");
                    _msg = _msg.Replace("{jobindex}", $"{shplayer.job.jobIndex}");
                    _msg = _msg.Replace("{jobname}", $"{shplayer.job.info.jobName}");
                    _msg = _msg.Replace("{message}", new Regex("(<)").Replace(Chat.LangAndChatBlock.Run(message), "<<b></b>"));
                    SendChatMessageToAdmins.Run(_msg);
                    return true;
                }
                // Will improve this someday..
                foreach (KeyValuePair<string, _Group> curr in Groups)
                {
                    if (curr.Value.Users.Contains(player.playerData.username))
                    {
                        _msg = curr.Value.Message;
                        _msg = _msg.Replace("{username}", new Regex("(<)").Replace(player.playerData.username, "<<b></b>"));
                        _msg = _msg.Replace("{id}", $"{shplayer.ID}");
                        _msg = _msg.Replace("{jobindex}", $"{shplayer.job.jobIndex}");
                        _msg = _msg.Replace("{jobname}", $"{shplayer.job.info.jobName}");
                        _msg = _msg.Replace("{message}", new Regex("(<)").Replace(Chat.LangAndChatBlock.Run(message), "<<b></b>"));
                        SendChatMessage.Run(_msg);
                        return true;
                    }
                }
                if (AdminsListPlayers.Contains(player.playerData.username))
                {
                    _msg = AdminMessage;
                    _msg = _msg.Replace("{username}", new Regex("(<)").Replace(player.playerData.username, "<<b></b>"));
                    _msg = _msg.Replace("{id}", $"{shplayer.ID}");
                    _msg = _msg.Replace("{jobindex}", $"{shplayer.job.jobIndex}");
                    _msg = _msg.Replace("{jobname}", $"{shplayer.job.info.jobName}");
                    _msg = _msg.Replace("{message}", new Regex("(<)").Replace(Chat.LangAndChatBlock.Run(message), "<<b></b>"));
                    SendChatMessage.Run(_msg);
                    return true;
                }
                _msg = PlayerMessage;
                _msg = _msg.Replace("{username}", new Regex("(<)").Replace(player.playerData.username, "<<b></b>"));
                _msg = _msg.Replace("{id}", $"{shplayer.ID}");
                _msg = _msg.Replace("{jobindex}", $"{shplayer.job.jobIndex}");
                _msg = _msg.Replace("{jobname}", $"{shplayer.job.info.jobName}");
                _msg = _msg.Replace("{message}", new Regex("(<)").Replace(Chat.LangAndChatBlock.Run(message), "<<b></b>"));
                SendChatMessage.Run(_msg);
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