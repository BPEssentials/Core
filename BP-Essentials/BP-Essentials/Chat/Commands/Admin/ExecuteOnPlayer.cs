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
                    foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                        if (shPlayer.svPlayer.playerData.username == arg1 || shPlayer.ID.ToString() == arg1.ToString())
                            if (shPlayer.IsRealPlayer())
                            {
                                shPlayer.svPlayer.Save();
                                if (message.StartsWith(CmdTp))
                                {
                                    player.SvTeleport(shPlayer.ID);
                                    player.SendToSelf(Channel.Unsequenced, 10, "Teleported to " + shPlayer.svPlayer.playerData.username + ".");
                                }
                                else if (message.StartsWith(CmdTpHere) || message.StartsWith(CmdTpHere2))
                                {
                                    // TODO:
                                    // - Rotation doesn't work all the time
                                    // - Doesn't always TP
                                    player.Save();
                                    shPlayer.SetPosition(player.playerData.position); //TODO
                                    shPlayer.SetRotation(player.playerData.rotation);
                                    shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, 10, player.playerData.username + " Teleported you to him.");
                                    player.SendToSelf(Channel.Unsequenced, 10, "Teleported " + shPlayer.svPlayer.playerData.username + " to you.");
                                }
                                else if (message.Contains(CmdBan))
                                {
                                    player.SvBan(shPlayer.ID);
                                    player.SendToSelf(Channel.Unsequenced, 10, "Banned " + shPlayer.svPlayer.playerData.username + ".");
                                }
                                else if (message.Contains(CmdKick))
                                {
                                    player.SvKick(shPlayer.ID);
                                    player.SendToSelf(Channel.Unsequenced, 10, "Kicked " + shPlayer.svPlayer.playerData.username + ".");
                                }
                                else if (message.Contains(CmdArrest))
                                {
                                    player.SvArrest(shPlayer.ID);
                                    player.SendToSelf(Channel.Unsequenced, 10, "Arrested " + shPlayer.svPlayer.playerData.username + ".");
                                }
                                else if (message.Contains(CmdRestrain))
                                {
                                    player.SvArrest(shPlayer.ID);
                                    player.SvRestrain(shPlayer.ID);
                                    player.SendToSelf(Channel.Unsequenced, 10, "Restrained " + shPlayer.svPlayer.playerData.username + ".");
                                }
                                else if (message.Contains(CmdKill))
                                {
                                    shPlayer.svPlayer.SvSuicide();
                                    player.SendToSelf(Channel.Unsequenced, 10, "Killed " + shPlayer.svPlayer.playerData.username + ".");
                                }
                                else if (message.Contains(CmdFree))
                                {
                                    shPlayer.svPlayer.Unhandcuff();
                                    player.SendToSelf(Channel.Unsequenced, 10, "Freed " + shPlayer.svPlayer.playerData.username + ".");
                                }
                                found = true;
                            }
                            else
                                player.SendToSelf(Channel.Unsequenced, 10, arg1 + " Is not a real player.");
                    if (!(found))
                        player.SendToSelf(Channel.Unsequenced, 10, arg1 + " Is not online.");
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