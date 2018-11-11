using UnityEngine;
using static BP_Essentials.EssentialsMethodsPlugin;
using static BP_Essentials.EssentialsVariablesPlugin;
using System;
using System.Reflection;
using System.Text;
using System.IO;

namespace BP_Essentials.Commands
{
    public class DebugCommands
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
                if (string.IsNullOrEmpty(arg2))
                {

                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, "Invalid arguments. /debug get(player)hash [username]");
                    return;
                }
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, "Hash of " + arg2 + " : " + Animator.StringToHash(arg2));
            }
            else if (arg == "spaceindex" || arg == "getspaceindex")
            {
                string arg2 = GetArgument.Run(2, false, true, message);
                if (string.IsNullOrEmpty(arg2))
                {
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, "Your SpaceIndex: " + shPlayer.GetPlaceIndex());
                    return;
                }
                var argshPlayer = GetShByStr.Run(arg2);
                if (argshPlayer == null)
                {
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, NotFoundOnline);
                    return;
                }
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, "SpaceIndex of '" + argshPlayer.svPlayer.playerData.username + "': " + argshPlayer.GetPlaceIndex());
            }
            else if (arg == "createidlist")
            {
                string arg2 = GetArgument.Run(2, false, true, message);
                if (string.IsNullOrEmpty(arg2))
                {
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, "Please select idlist type: /debug createidlist item/vehicle/skinid");
                    return;
                }
                arg2 = arg2.Trim().ToLower();
                if (arg2 != "item" && arg2 != "vehicle" && arg2 != "skinid")
                {
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, "Please select idlist type: /debug createidlist item/vehicle/skinid (Invalid value given as second arument)");
                    return;
                }
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, "Creating ID list.. please wait");
                var location = $"{FileDirectory}IDLists/{arg2}/IDLIST_{DateTime.Now.ToString("yyyy_mm_dd_hh_mm_ss")}.txt";
                if (!Directory.Exists($"{FileDirectory}IDLists/{arg2}"))
                    Directory.CreateDirectory($"{FileDirectory}IDLists/{arg2}");
                var sb = new StringBuilder();
                int currIndex = 1;
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
                    foreach (var entity in shPlayer.manager.entities)
                    {
                        switch (arg2)
                        {
                            case "item":
                                if (entity != null && entity is ShItem)
                                    sb.Append($"{{\"name\": \"{entity.name}\",\"id\": {++currIndex},\"gameid\": {entity.index}}},\n");
                                break;
                            case "vehicle":
                                if (entity != null && entity is ShVehicle)
                                    sb.Append($"{{\"name\": \"{entity.name}\",\"id\": {++currIndex},\"gameid\": {entity.index}}},\n");
                                break;
                            default:
                                break;
                        }
                    }
                }
                File.WriteAllText(location, $"{sb.Remove(sb.Length - 2, 1)}]}}");

                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"Success! ID List has been saved in {location}. ({currIndex} entries.)");
            }
            else if (arg == "jobarray")
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"Jobs array: (Length: {Jobs.Length}) {string.Join(", ", Jobs)}");
            }
            else if (arg == "tpall")
            {
                foreach (var currPlayer in UnityEngine.Object.FindObjectsOfType<SvPlayer>())
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