using static BP_Essentials.EssentialsVariablesPlugin;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

namespace BP_Essentials.Commands {
    public class ExecuteOnPlayer : EssentialsChatPlugin {
        public static bool Run(object oPlayer, string message, string arg1)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (AdminsListPlayers.Contains(player.playerData.username))
                {
                    var found = false;
                    foreach (var shPlayer1 in GameObject.FindObjectsOfType<ShPlayer>())
                        if (shPlayer1.svPlayer == player)
                            foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                                if (shPlayer.username == arg1 || shPlayer.ID.ToString() == arg1.ToString())
                                    if (shPlayer.IsRealPlayer())
                                    {
                                        // Improve at some day
                                        if (message.StartsWith(CmdKnockout) || message.StartsWith(CmdKnockout2))
                                        {
                                            shPlayer.svPlayer.SvForceStance(StanceIndex.KnockedOut);
                                            player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>Knocked out</color> <color={argColor}>{shPlayer.username}</color><color={infoColor}>.</color>");
                                        }
                                        else if (message.StartsWith(CmdJail) || message.StartsWith(CmdJail2))
                                        {
                                            float t;
                                            if (float.TryParse(message.Split(' ').Last().Trim(), out t))
                                            {
                                                if (SendToJail.Run(shPlayer, t))
                                                {
                                                    shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, 10, $"<color={argColor}>{shPlayer1.username}</color> <color={infoColor}>Send you to jail.</color>");
                                                    player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>Send</color> <color={argColor}>{shPlayer.username}</color> <color={infoColor}>To jail.</color>");
                                                }
                                                else
                                                    player.SendToSelf(Channel.Unsequenced, 10, $"<color={errorColor}>Cannot send </color> <color={argColor}>{shPlayer.username}</color> <color={errorColor}>To jail.</color>");
                                            }
                                            else
                                                player.SendToSelf(Channel.Unsequenced, 10, ArgRequired);
                                        }
                                        else if (message.StartsWith(CmdTpHere) || message.StartsWith(CmdTpHere2))
                                        {
                                            shPlayer.svPlayer.SvReset(shPlayer1.GetPosition(), shPlayer1.GetRotation(), shPlayer1.GetPlaceIndex());
                                            shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, 10, $"<color={argColor}>" + shPlayer1.username + $"</color><color={infoColor}> Teleported you to him.</color>");
                                            player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>Teleported</color> <color={argColor}>" + shPlayer.username + $"</color><color={infoColor}> To you.</color>");
                                        }
                                        else if (message.StartsWith(CmdTp))
                                        {
                                            player.SvTeleport(shPlayer.ID);
                                            player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>Teleported to</color> <color={argColor}>" + shPlayer.username + $"</color><color={infoColor}>.</color>");
                                        }
                                        else if (message.Contains(CmdBan))
                                        {
                                            player.SvBan(shPlayer.ID);
                                            player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>Banned</color> <color={argColor}>" + shPlayer.username + $"</color><color={infoColor}>.</color>");
                                        }
                                        else if (message.Contains(CmdKick))
                                        {
                                            player.SvKick(shPlayer.ID);
                                            player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>Kicked</color> <color={argColor}>" + shPlayer.username + $"</color><color={infoColor}>.</color>");
                                        }
                                        else if (message.Contains(CmdArrest))
                                        {
                                            player.SvArrest(shPlayer.ID);
                                            player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>Arrested</color> <color={argColor}>" + shPlayer.username + $"</color><color={infoColor}>.</color>");
                                        }
                                        else if (message.Contains(CmdRestrain))
                                        {
                                            player.SvArrest(shPlayer.ID);
                                            player.SvRestrain(shPlayer.ID);
                                            player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>Restrained</color> <color={argColor}>" + shPlayer.username + $"</color><color={infoColor}>.</color>");
                                        }
                                        else if (message.Contains(CmdKill))
                                        {
                                            shPlayer.svPlayer.SvSuicide();
                                            player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>Killed</color> <color={argColor}>" + shPlayer.username + $"</color><color={infoColor}>.</color>");
                                        }
                                        else if (message.Contains(CmdFree))
                                        {
                                            shPlayer.svPlayer.Unhandcuff();
                                            player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>Freed</color> <color={argColor}>" + shPlayer.username + $"</color><color={infoColor}>.</color>");
                                        }
                                        found = true;
                                    }
                                    else
                                        player.SendToSelf(Channel.Unsequenced, 10, $"<color={argColor}>" + arg1 + $"</color><color={errorColor}> Is not a real player.</color>");
                    if (!(found))
                        player.SendToSelf(Channel.Unsequenced, 10, $"<color={argColor}>" + arg1 + $"</color><color={errorColor}> Is not online.</color>");
                }
                else
                    player.SendToSelf(Channel.Unsequenced, 10, MsgNoPerm);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }

    }
}