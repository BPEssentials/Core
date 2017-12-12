using UnityEngine;
using static BP_Essentials.EssentialsMethodsPlugin;
using static BP_Essentials.EssentialsVariablesPlugin;
using System;
namespace BP_Essentials.Commands {
    public class DebugCommands : EssentialsChatPlugin {
        public static bool Run(object oPlayer, string message)
        {
            var player = (SvPlayer)oPlayer;
            if (AdminsListPlayers.Contains(player.playerData.username))
            {
                // TODO: Add Commands like
                // - Ram usage and CPU usage on PC as well as date and other useful information about it

                foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                    if (shPlayer.svPlayer == player)
                        if (shPlayer.IsRealPlayer())
                        {
                            string arg = GetArgument(1, false, false, message).Trim().ToLower();
                            if (arg == "location" || arg == "getlocation")
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Your location: " + shPlayer.GetPosition());
                            else if (arg == "getplayerhash" || arg == "gethash")
                            {
                                string TempMSG = GetArgument(0, false, false, message).Trim().ToLower() + arg;
                                string arg2 = TempMSG.Substring(TempMSG.Length + 1);
                                if (!String.IsNullOrWhiteSpace(arg2))
                                {
                                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Hash of " + arg2 + " :" + Animator.StringToHash(arg2).ToString());
                                }
                                else
                                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Invalid arguments. /debug get(player)hash [username]");
                            }
                            else if (arg == "spaceindex" || arg == "getspaceindex")
                            {
                                string TempMSG = GetArgument(0, false, false, message).Trim().ToLower() + arg;
                                string arg2 = TempMSG.Substring(TempMSG.Length + 1);
                                if (!String.IsNullOrWhiteSpace(arg2))
                                {
                                    bool found = false;
                                    foreach (var shPlayer2 in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                                        if (shPlayer2.svPlayer.playerData.username == arg2 || shPlayer2.ID.ToString() == arg2)
                                            if (shPlayer2.IsRealPlayer())
                                            {
                                                found = true;
                                                player.SendToSelf(Channel.Unsequenced, (byte)10, "SpaceIndex of '" + shPlayer2.svPlayer.playerData.username + "': " + shPlayer2.GetSpaceIndex());
                                            }
                                    if (!found)
                                        player.SendToSelf(Channel.Unsequenced, (byte)10, "Invalid arguments (Or user is not found online.) /debug (get)spaceindex [username] ");
                                }
                                else
                                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Your SpaceIndex: " + shPlayer.GetSpaceIndex());
                            }
                        }
            }
            else
                player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
            return true;
        }

    }
}