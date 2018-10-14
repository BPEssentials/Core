using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static BP_Essentials.EssentialsMethodsPlugin;
using static BP_Essentials.EssentialsVariablesPlugin;
namespace BP_Essentials
{
    public class EssentialsChatPlugin : MonoBehaviour
    {
        #region Event: ChatMessage | Global
        //Chat Events
        [Hook("SvPlayer.SvGlobalChatMessage")]
        public static bool SvGlobalChatMessage(SvPlayer player, ref string message)
        {
            try
            {
                var tempMessage = message;
                if (MessagesAllowedPerSecond != -1 && MessagesAllowedPerSecond < 50)
                {
                    if (playerList.TryGetValue(player.player.ID, out var currObj))
                    {
                        if (currObj.messagesSent >= MessagesAllowedPerSecond)
                        {
                            Debug.Log($"{SetTimeStamp.Run()}[WARNING] {player.player.username} got kicked for spamming! {currObj.messagesSent}/s (max: {MessagesAllowedPerSecond}) messages sent.");
                            player.svManager.Kick(player.connection);
                            return true;
                        }
                        else
                        {
                            playerList[player.player.ID].messagesSent++;
                            if (!currObj.isCurrentlyAwaiting)
                            {
                                playerList[player.player.ID].isCurrentlyAwaiting = true;
                                Task.Factory.StartNew(async () =>
                                {
                                    await Task.Delay(1000);
                                    if (playerList.ContainsKey(player.player.ID))
                                    {
                                        playerList[player.player.ID].messagesSent = 0;
                                        playerList[player.player.ID].isCurrentlyAwaiting = false;
                                    }
                                });
                            }
                        }
                    }
                }
                //Message Logging
                if (!(MutePlayers.Contains(player.playerData.username)))
                    LogMessage.Run(player, message);
                var command = GetArgument.Run(0, false, false, message);
                if (message.StartsWith(CmdCommandCharacter))
                {
                    // CustomCommands
                    var customCommand = CustomCommands.FirstOrDefault(x => tempMessage.StartsWith(x.Command));
                    if (customCommand != null)
                    {
                        foreach (string line in customCommand.Response.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
                            player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, GetPlaceHolders.Run(line, player));
                        return true;
                    }
                    // Go through all registered commands and check if the command that the user entered matches
                    foreach (var cmd in CommandList.Values)
                        if (cmd.commandCmds.Contains(command))
                        {
                            if (cmd.commandDisabled)
                            {
                                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, DisabledCommand);
                                return true;
                            }
                            if (HasPermission.Run(player, cmd.commandGroup, true, player.player.job.jobIndex) && HasWantedLevel.Run(player, cmd.commandWantedAllowed))
                            {
                                playerList.Where(x => x.Value.spyEnabled && x.Value.Shplayer.svPlayer != player).ToList().ForEach(x => x.Value.Shplayer.svPlayer.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color=#f4c242>[SPYCHAT]</color> {player.playerData.username}: {tempMessage}"));
                                cmd.RunMethod.Invoke(player, message);
                            }
                            return true;
                        }
                    if (AfkPlayers.Contains(player.playerData.username))
                        Commands.Afk.Run(player, message);
                    if (MsgUnknownCommand)
                    {
                        player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>Unknown command. Type</color><color={argColor}> {CmdCommandCharacter}essentials cmds </color><color={errorColor}>for more info.</color>");
                        return true;
                    }
                }
                //Checks if the player is muted.
                if (MutePlayers.Contains(player.playerData.username))
                {
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, SelfIsMuted);
                    return true;
                }
                //Checks if the message contains a username that is AFK.
                if (AfkPlayers.Any(message.Contains))
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, PlayerIsAFK);

                var shplayer = player.player;
                if (!playerList[shplayer.ID].chatEnabled)
                {
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={warningColor}>Please enable your chat again by typing</color> <color={argColor}>{CmdCommandCharacter}{CmdToggleChat}</color><color={warningColor}>.</color>");
                    return true;
                }
                if (playerList[shplayer.ID].staffChatEnabled)
                {
                    SendChatMessageToAdmins.Run(FillPlaceholders.Run(shplayer, AdminChatMessage, message));
                    return true;
                }
                foreach (var curr in Groups)
                    if (curr.Value.Users.Contains(player.playerData.username))
                    {
                        SendChatMessage.Run(FillPlaceholders.Run(shplayer, curr.Value.Message, message));
                        return true;
                    }
                if (player.player.admin)
                {
                    SendChatMessage.Run(FillPlaceholders.Run(shplayer, AdminMessage, message));
                    return true;
                }
                SendChatMessage.Run(FillPlaceholders.Run(shplayer, PlayerMessage, message));
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
        #endregion

        #region Event: ChatMessage | Local
        [Hook("SvPlayer.SvLocalChatMessage")]
        public static bool SvLocalChatMessage(SvPlayer player, ref string message)
        {
            if (LocalChatMute && MutePlayers.Contains(player.playerData.username))
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, SelfIsMuted);
                return true;
            }
            LogMessage.LocalMessage(player, message);
            if (!ProximityChat)
                return false;
            player.Send(SvSendType.LocalOthers, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>[Local-Chat]</color> {new Regex("(<)").Replace(player.player.username, "<<b></b>")}: {new Regex("(<)").Replace(message, "<<b></b>")}");
            return true;
        }
        #endregion
    }
}