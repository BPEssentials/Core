using UnityEngine;
using static BP_Essentials.EssentialsMethodsPlugin;
using static BP_Essentials.EssentialsVariablesPlugin;
using System;
using System.Reflection;
using System.Text;
using System.IO;

namespace BP_Essentials.Commands
{
    public class DebugCommands : EssentialsChatPlugin
    {
        public static void Run(SvPlayer player, string message)
        {
            var shPlayer = player.player;
            string arg = GetArgument.Run(1, false, false, message).Trim().ToLower();

            if (arg == "location" || arg == "getlocation")
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, "Your location: " + shPlayer.GetPosition());
            else if (arg == "getplayerhash" || arg == "gethash")
            {
                string arg2 = GetArgument.Run(2, false, true, message);
                if (!String.IsNullOrEmpty(arg2))
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, "Hash of " + arg2 + " : " + Animator.StringToHash(arg2));
                else
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, "Invalid arguments. /debug get(player)hash [username]");
            }
            else if (arg == "spaceindex" || arg == "getspaceindex")
            {
                string arg2 = GetArgument.Run(2, false, true, message);
                if (!String.IsNullOrEmpty(arg2))
                {
                    bool found = false;
                    foreach (var shPlayer2 in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                        if (shPlayer2.svPlayer.playerData.username == arg2 || shPlayer2.ID.ToString() == arg2)
                            if (!shPlayer2.svPlayer.IsServerside())
                            {
                                found = true;
                                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, "SpaceIndex of '" + shPlayer2.svPlayer.playerData.username + "': " + shPlayer2.GetPlaceIndex());
                            }
                    if (!found)
                        player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, "Invalid arguments (Or user is not found online.) /debug (get)spaceindex [username] ");
                }
                else
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, "Your SpaceIndex: " + shPlayer.GetPlaceIndex());
            }
            else if (arg == "createidlist")
            {
                string arg2 = GetArgument.Run(2, false, true, message).Trim().ToLower();
                if (!String.IsNullOrEmpty(arg2) && (arg2 == "item" || arg2 == "vehicle" || arg2 == "skinid"))
                {
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, "Creating ID list.. please wait");
                    var location = $"{FileDirectory}IDLists/{arg2}/IDLIST_{DateTime.Now.ToString("yyyy_mm_dd_hh_mm_ss")}.txt";
                    if (!Directory.Exists($"{FileDirectory}IDLists/{arg2}"))
                        Directory.CreateDirectory($"{FileDirectory}IDLists/{arg2}");
                    var sb = new StringBuilder();
                    int currIndex = 1;
                    sb.Append("{\"items\": [");
                    IndexCollection<ShEntity> ECol = (IndexCollection<ShEntity>)typeof(ShManager).GetField("entityCollection", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(shPlayer.manager);
                    if (arg2 == "skinid")
                    {
                        for (int i = 0; i < shPlayer.manager.skinPrefabs.Length; i++)
                        {
                            sb.Append($"{{\"name\": \"{shPlayer.manager.skinPrefabs[i].name}\",\"id\": {currIndex},\"gameid\": {Animator.StringToHash(shPlayer.manager.skinPrefabs[i].name)}}},\n");
                            ++currIndex;
                        }
                    }
                    else
                    {
                        foreach (var v in ECol)
                        {
                            // dry coding, improve
                            switch (arg2)
                            {
                                case "item":
                                    // Jesus o.O
                                    if (
                                           v.GetType() == typeof(ShPlaceable)
                                        || v.GetType() == typeof(ShGun)
                                        || v.GetType() == typeof(ShWeapon)
                                        || v.GetType() == typeof(ShFurniture)
                                        || v.GetType() == typeof(ShWearable)
                                        || v.GetType() == typeof(ShConsumable)
                                        || v.GetType() == typeof(ShDrugMaterial)
                                        || v.GetType() == typeof(ShExtinguisher)
                                        || v.GetType() == typeof(ShHealer)
                                        || v.GetType() == typeof(ShRestraint)
                                        || v.GetType() == typeof(ShSeed)
                                        || v.GetType() == typeof(ShProjectile)
                                        || v.GetType() == typeof(ShDetonator)
                                        || v.GetType() == typeof(ShShield))
                                    {
                                        sb.Append($"{{\"name\": \"{v.name}\",\"id\": {currIndex},\"gameid\": {v.index}}},\n");
                                        ++currIndex;
                                    }
                                    break;
                                case "vehicle":
                                    if (v.GetType() == typeof(ShVehicle) || v.GetType() == typeof(ShBoat) || v.GetType() == typeof(ShHelo) || v.GetType() == typeof(ShTransport))
                                    {
                                        sb.Append($"{{\"name\": \"{v.name}\",\"id\": {currIndex},\"gameid\": {v.index}}},\n");
                                        ++currIndex;
                                    }
                                    break;
                            }
                        }
                    }
                    File.WriteAllText(location, $"{sb.Remove(sb.Length - 2, 1)}]}}");

                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"Success! ID List has been saved in {location}. ({currIndex} entries.)");
                }
                else
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, "Please select idlist type: /debug createidlist item/vehicle");
            }
            else if (arg == "jobarray")
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"Jobs array: (Length: {Jobs.Length}) {string.Join(", ", Jobs)}");
            }
            else if (arg == "tpall")
            {
                foreach (var currPlayer in FindObjectsOfType<SvPlayer>())
                    currPlayer.SvReset(shPlayer.GetPosition(), shPlayer.GetRotation(), shPlayer.GetPlaceIndex());
            }
            else if (arg == "addcrimes")
            {
                player.SvAddCrime(CrimeIndex.Murder, null);
            }
            else
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, "/debug location/getplayerhash/getspaceindex/createidlist/jobarray");
        }

    }
}