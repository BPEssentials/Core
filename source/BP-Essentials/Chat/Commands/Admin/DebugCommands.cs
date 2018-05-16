using UnityEngine;
using static BP_Essentials.EssentialsMethodsPlugin;
using static BP_Essentials.EssentialsVariablesPlugin;
using System;
namespace BP_Essentials.Commands {
    public class DebugCommands : EssentialsChatPlugin {
        public static bool Run(object oPlayer, string message)
        {
            try
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
                                string arg = GetArgument.Run(1, false, false, message).Trim().ToLower();
                                if (arg == "location" || arg == "getlocation")
                                    player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "Your location: " + shPlayer.GetPosition());
                                else if (arg == "getplayerhash" || arg == "gethash")
                                {
                                    string arg2 = GetArgument.Run(2, false, true, message);
                                    if (!String.IsNullOrEmpty(arg2))
                                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "Hash of " + arg2 + " : " + Animator.StringToHash(arg2).ToString());
                                    else
                                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "Invalid arguments. /debug get(player)hash [username]");
                                }
                                else if (arg == "spaceindex" || arg == "getspaceindex")
                                {
                                    string arg2 = GetArgument.Run(2, false, true, message);
                                    if (!String.IsNullOrEmpty(arg2))
                                    {
                                        bool found = false;
                                        foreach (var shPlayer2 in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                                            if (shPlayer2.svPlayer.playerData.username == arg2 || shPlayer2.ID.ToString() == arg2)
                                                if (shPlayer2.IsRealPlayer())
                                                {
                                                    found = true;
                                                    player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "SpaceIndex of '" + shPlayer2.svPlayer.playerData.username + "': " + shPlayer2.GetPlaceIndex());
                                                }
                                        if (!found)
                                            player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "Invalid arguments (Or user is not found online.) /debug (get)spaceindex [username] ");
                                    }
                                    else
                                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "Your SpaceIndex: " + shPlayer.GetPlaceIndex());
                                }
                            }
                }
                else
                    player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, MsgNoPerm);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }

    }
}