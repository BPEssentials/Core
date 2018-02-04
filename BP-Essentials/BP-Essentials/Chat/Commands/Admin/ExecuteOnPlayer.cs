using static BP_Essentials.EssentialsVariablesPlugin;
using UnityEngine;
using System;

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
                                        if (message.StartsWith(CmdTpHere) || message.StartsWith(CmdTpHere2))
                                        {
                                            // TODO:
                                            // - Rotation doesn't work all the time
                                            // - Doesn't always TP
                                            shPlayer.SetPosition(shPlayer1.GetPosition());
                                            shPlayer.SetRotation(shPlayer1.GetRotation());
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