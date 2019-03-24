using UnityEngine;
using static BP_Essentials.HookMethods;
using static BP_Essentials.Variables;
using System;
using System.Reflection;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace BP_Essentials.Commands
{
    public class DebugCommands
    {
        public static void Run(SvPlayer player, string message)
        {
            var shPlayer = player.player;
            string arg = GetArgument.Run(1, false, false, message).Trim().ToLower();

            switch (arg)
            {
                case "location":
                case "getlocation":
                    player.SendChatMessage("Your location: " + shPlayer.GetPosition());
                    break;
                case "getplayerhash":
                case "gethash":
                    {
                        string arg2 = GetArgument.Run(2, false, true, message);
                        if (string.IsNullOrEmpty(arg2))
                        {

                            player.SendChatMessage("Invalid arguments. /debug get(player)hash [username]");
                            return;
                        }
                        player.SendChatMessage("Hash of " + arg2 + " : " + Animator.StringToHash(arg2));
                        break;
                    }

                case "spaceindex":
                case "getspaceindex":
                    {
                        string arg2 = GetArgument.Run(2, false, true, message);
                        if (string.IsNullOrEmpty(arg2))
                        {
                            player.SendChatMessage("Your SpaceIndex: " + shPlayer.GetPlaceIndex());
                            return;
                        }
                        var argshPlayer = GetShByStr.Run(arg2);
                        if (argshPlayer == null)
                        {
                            player.SendChatMessage(NotFoundOnline);
                            return;
                        }
                        player.SendChatMessage("SpaceIndex of '" + argshPlayer.svPlayer.playerData.username + "': " + argshPlayer.GetPlaceIndex());
                        break;
                    }

                case "createidlist":
                    {
                        string arg2 = GetArgument.Run(2, false, true, message);
                        if (string.IsNullOrEmpty(arg2))
                        {
                            player.SendChatMessage("Please select idlist type: /debug createidlist item/vehicle/skinid");
                            return;
                        }
                        arg2 = arg2.Trim().ToLower();
                        if (arg2 != "item" && arg2 != "vehicle" && arg2 != "skinid")
                        {
                            player.SendChatMessage("Please select idlist type: /debug createidlist item/vehicle/skinid (Invalid value given as second arument)");
                            return;
                        }
                        player.SendChatMessage("Creating ID list.. please wait");
                        var location = $"{FileDirectory}IDLists/{arg2}/IDLIST_{DateTime.Now.ToString("yyyy_mm_dd_hh_mm_ss")}.txt";
                        if (!Directory.Exists($"{FileDirectory}IDLists/{arg2}"))
                            Directory.CreateDirectory($"{FileDirectory}IDLists/{arg2}");
                        var sb = new StringBuilder();
                        int currIndex = 0;
                        sb.Append("{\"items\": [");
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
                            var itemsAdded = new List<int>();
                            foreach (var entity in Builder.pb_Scene.instance.entityCollection)
                            {
                                if (entity == null)
                                    continue;
                                if (((arg2 == "item" && entity is ShItem) || (arg2 == "vehicle" && entity is ShTransport && !(entity is ShParachute))) && !itemsAdded.Contains(entity.index))
                                {
                                    sb.Append($"{{\"name\": \"{entity.name.Replace("(Clone)", "")}\",\"id\": {++currIndex},\"gameid\": {entity.index}}},\n");
                                    itemsAdded.Add(entity.index);
                                }
                            }
                        }
                        File.WriteAllText(location, $"{sb.Remove(sb.Length - 2, 1)}]}}");

                        player.SendChatMessage($"Success! ID List has been saved in {location}. ({currIndex} entries.)");
                        break;
                    }

                case "jobarray":
                    player.SendChatMessage($"Jobs array: (Length: {Jobs.Length}) {string.Join(", ", Jobs)}");
                    break;
                case "tpall":
                    {
                        foreach (var currPlayer in UnityEngine.Object.FindObjectsOfType<SvPlayer>())
                            currPlayer.ResetAndSavePosition(shPlayer.GetPosition(), shPlayer.GetRotation(), shPlayer.GetPlaceIndex());
                        break;
                    }

                case "addcrimes":
                    player.SvAddCrime(CrimeIndex.Murder, null);
                    break;

                case "disconnecttest":
                    player.Send(SvSendType.AllOthers, Channel.Unsequenced, ClPacket.DestroyEntity, player.player.ID);
                    break;
                case "setmaxapartmententities":
                    player.entity.manager.apartmentLimit = 300;
                    player.SendChatMessage("success");
                    break;
                case "getmaxapartmententities":
                    player.SendChatMessage($"Max entities: {player.entity.manager.apartmentLimit}");
                    break;
                default:
                    player.SendChatMessage("/debug location/getplayerhash/getspaceindex/createidlist/jobarray");
                    break;
            }
        }

    }
}