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
                                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Knocked out</color> <color={argColor}>{shPlayer.username}</color><color={infoColor}>.</color>");
                                    }
                                    else if (message.StartsWith(CmdJail) || message.StartsWith(CmdJail2))
                                    {
                                        if (float.TryParse(message.Split(' ').Last().Trim(), out float t))
                                        {
                                            if (SendToJail.Run(shPlayer, t))
                                            {
                                                shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={argColor}>{shPlayer1.username}</color> <color={infoColor}>sent you to jail.</color>");
                                                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Sent</color> <color={argColor}>{shPlayer.username}</color> <color={infoColor}>To jail.</color>");
                                            }
                                            else
                                                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>Cannot send </color> <color={argColor}>{shPlayer.username}</color> <color={errorColor}>To jail.</color>");
                                        }
                                        else
                                            player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
                                    }
                                    else if (message.StartsWith(CmdTpHere) || message.StartsWith(CmdTpHere2))
                                    {
                                        shPlayer.svPlayer.SvReset(shPlayer1.GetPosition(), shPlayer1.GetRotation(), shPlayer1.GetPlaceIndex());
                                        shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={argColor}>" + shPlayer1.username + $"</color><color={infoColor}> Teleported you to him.</color>");
                                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Teleported</color> <color={argColor}>" + shPlayer.username + $"</color><color={infoColor}> To you.</color>");
                                    }
                                    else if (message.StartsWith(CmdTp))
                                    {
                                        player.SvReset(shPlayer.GetPosition(), shPlayer.GetRotation(), shPlayer.GetPlaceIndex());
                                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Teleported to</color> <color={argColor}>" + shPlayer.username + $"</color><color={infoColor}>.</color>");
                                    }
                                    else if (message.Contains(CmdBan))
                                    {
                                        player.svManager.AddBanned(shPlayer);
                                        player.svManager.Disconnect(shPlayer.svPlayer.connection);
                                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Banned</color> <color={argColor}>" + shPlayer.username + $"</color><color={infoColor}>.</color>");
                                    }
                                    else if (message.Contains(CmdKick))
                                    {
                                        player.svManager.Kick(shPlayer.svPlayer.connection);
                                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Kicked</color> <color={argColor}>" + shPlayer.username + $"</color><color={infoColor}>.</color>");
                                    }
                                    else if (message.Contains(CmdArrest))
                                    {
                                        shPlayer.svPlayer.Arrest(shPlayer.manager.handcuffed);
                                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Arrested</color> <color={argColor}>" + shPlayer.username + $"</color><color={infoColor}>.</color>");
                                    }
                                    else if (message.Contains(CmdRestrain))
                                    {
                                        shPlayer.svPlayer.Arrest(shPlayer.manager.handcuffed);
                                        ShRetained shRetained = shPlayer.curEquipable as ShRetained;
                                        shPlayer.svPlayer.SvSetEquipable(shRetained.otherRetained.index);
                                        if (!shPlayer.svPlayer.IsServerside())
                                            shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "You've been restrained");
                                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Restrained</color> <color={argColor}>" + shPlayer.username + $"</color><color={infoColor}>.</color>");
                                    }
                                    else if (message.Contains(CmdKill))
                                    {
                                        shPlayer.svPlayer.SvSuicide();
                                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Killed</color> <color={argColor}>" + shPlayer.username + $"</color><color={infoColor}>.</color>");
                                    }
                                    else if (message.Contains(CmdFree))
                                    {
                                        UnRetain.Run(shPlayer.svPlayer);
                                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Freed</color> <color={argColor}>" + shPlayer.username + $"</color><color={infoColor}>.</color>");
                                    }
                                        found = true;
                                    }
                                    else
                                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={argColor}>" + arg1 + $"</color><color={errorColor}> Is not a real player.</color>");
                    if (!(found))
                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={argColor}>" + arg1 + $"</color><color={errorColor}> Is not online.</color>");
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }

    }
}