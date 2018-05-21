using UnityEngine;
using static BP_Essentials.EssentialsMethodsPlugin;
using static BP_Essentials.EssentialsVariablesPlugin;
using System;
using System.Reflection;
using System.Text;
using System.IO;

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
                                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "Hash of " + arg2 + " : " + Animator.StringToHash(arg2));
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
                                else if (arg == "createidlist")
                                {
                                    string arg2 = GetArgument.Run(2, false, true, message).ToLower();
                                    if (!String.IsNullOrEmpty(arg2) && arg2 == "json" || arg2 == "array")
                                    {
                                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "Creating ID list.. please wait");
                                        var shm = shPlayer.manager;
                                        var location = $"{FileDirectory}IDLists/IDLIST_{DateTime.Now.ToString("yyyy_mm_dd_hh_mm_ss")}.txt";
                                        if (!Directory.Exists($"{FileDirectory}IDLists/"))
                                            Directory.CreateDirectory($"{FileDirectory}IDLists/");
                                        var sb = new StringBuilder();
                                        int currIndex = 1;
                                        if (arg2 == "array")
                                            sb.Append("public static int[] IDs = {\n0, // you don't want to use ID 0\n");
                                        else
                                            sb.Append("{\"items\": [");
                                        IndexCollection<ShEntity> ECol = (IndexCollection<ShEntity>)typeof(ShManager).GetField("entityCollection", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(shm);
                                        foreach (ShEntity v in ECol)
                                        {
                                            if (v.GetType() == typeof(ShPlaceable) || v.GetType() == typeof(ShPlaceable) || v.GetType() == typeof(ShGun) || v.GetType() == typeof(ShWeapon) || v.GetType() == typeof(ShFurniture) || v.GetType() == typeof(ShWearable) || v.GetType() == typeof(ShConsumable) || v.GetType() == typeof(ShDrugMaterial) || v.GetType() == typeof(ShExtinguisher) || v.GetType() == typeof(ShHealer) || v.GetType() == typeof(ShRetainer) || v.GetType() == typeof(ShSeed) || v.GetType() == typeof(ShProjectile))
                                                if (arg2 == "array")
                                                    sb.Append($"{v.index}, //{v.name}\n");
                                                else
                                                {
                                                    sb.Append($"{{\"name\": \"{v.name}\",\"id\": {currIndex},\"gameid\": {v.index}}},\n");
                                                    ++currIndex;
                                                }
                                        }
                                        if (arg2 == "array")
                                            File.WriteAllText(location, $"{sb}}};");
                                        else
                                            File.WriteAllText(location, $"{sb.Remove(sb.Length - 2, 1)}]}}");

                                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"Success! ID List has been saved in {location}. ({currIndex} entries.)");
                                    }
                                    else
                                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "Please select file type /debug createidlist json/array");
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