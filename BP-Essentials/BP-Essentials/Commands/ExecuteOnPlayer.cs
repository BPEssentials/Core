using static BP_Essentials.EssentialsVariablesPlugin;
using UnityEngine;
namespace BP_Essentials.Commands {
    public class ExecuteOnPlayer : EssentialsChatPlugin {
        public static bool Run(object oPlayer, string message, string arg1)
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
                            if (message.StartsWith("/tp")) // CHANGE THIS TO SOFTCODED ONE
                            {
                                // TODO:
                                // - Rotation doesn't work all the time
                                // - Doesn't always TP
                                if (message.StartsWith(CmdTpHere) || message.StartsWith("/tphere")) // CHANGE THIS TO SOFTCODED ONE
                                {
                                    player.Save();
                                    Debug.Log("1 " +shPlayer.svPlayer.playerData.rotation);
                                    Debug.Log("2 " + player.playerData.rotation);
                                    shPlayer.SetPosition(player.playerData.position);
                                    shPlayer.SetRotation(player.playerData.rotation);
                                    Debug.Log("3 " + shPlayer.svPlayer.playerData.rotation);
                                    Debug.Log("4 " + player.playerData.rotation);
                                    //  shPlayer.svPlayer.playerData.position = player.playerData.position;
                                    // shPlayer.svPlayer.playerData.rotation = player.playerData.rotation;
                                    // Debug.Log("5 " + shPlayer.svPlayer.playerData.rotation);
                                    // Debug.Log("6 " + player.playerData.rotation);
                                    shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, (byte)10, player.playerData.username + " Teleported you to him.");
                                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Teleported " + shPlayer.svPlayer.playerData.username + " to you.");
                                }
                                else if (message.StartsWith("/tp")) // CHANGE THIS TO SOFTCODED ONE
                                {
                                    player.SvTeleport(shPlayer.ID);
                                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Teleported to " + shPlayer.svPlayer.playerData.username + ".");
                                }
                            }
                            else if (message.Contains("/ban")) // CHANGE THIS TO SOFTCODED ONE
                            {
                                player.SvBan(shPlayer.ID);
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Banned " + shPlayer.svPlayer.playerData.username + ".");
                            }
                            else if (message.Contains("/kick")) // CHANGE THIS TO SOFTCODED ONE
                            {
                                player.SvKick(shPlayer.ID);
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Kicked " + shPlayer.svPlayer.playerData.username + ".");
                            }
                            else if (message.Contains("/arrest")) // CHANGE THIS TO SOFTCODED ONE
                            {
                                player.SvArrest(shPlayer.ID);
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Arrested " + shPlayer.svPlayer.playerData.username + ".");
                            }
                            else if (message.Contains("/restrain")) // CHANGE THIS TO SOFTCODED ONE
                            {
                                player.SvArrest(shPlayer.ID);
                                player.SvRestrain(shPlayer.ID);
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Restrained " + shPlayer.svPlayer.playerData.username + ".");
                            }
                            else if (message.Contains("/kill")) // CHANGE THIS TO SOFTCODED ONE
                            {
                                shPlayer.svPlayer.SvSuicide();
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Killed " + shPlayer.svPlayer.playerData.username + ".");
                            }
                            else if (message.Contains("/free")) // CHANGE THIS TO SOFTCODED ONE
                            {
                                shPlayer.svPlayer.Unhandcuff();
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Freed " + shPlayer.svPlayer.playerData.username + ".");
                            }
                            found = true;
                        }
                        else
                            player.SendToSelf(Channel.Unsequenced, (byte)10, arg1 + " Is not a real player.");
                if (!(found))
                    player.SendToSelf(Channel.Unsequenced, (byte)10, arg1 + " Is not online.");
            }
            else
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
            }
            return true;
        }

    }
}