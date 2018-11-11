using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;

namespace BP_Essentials
{
    // Almost a 1 : 1 copy of the Kits class.. oh well
    public class Warps
    {
        public static void LoadAllWarps(bool firstLoad = false)
        {
            if (DebugLevel >= 1)
                Debug.Log($"{SetTimeStamp.Run()}[INFO] Loading warps..");
            listWarps.Clear();
            foreach (string file in Directory.EnumerateFiles(WarpDirectory, "*.json", SearchOption.AllDirectories))
            {
                var obj = JsonConvert.DeserializeObject<Warps_Json.Warps_RootObj>(FilterComments.Run(file));
                if (listWarps.Any(x => x.Name == obj.Name))
                {
                    Debug.Log($"{SetTimeStamp.Run()}[ERROR] Cannot add warp {obj.Name} because it already exists in the list!");
                    continue;
                }
                listWarps.Add(obj);
                if (firstLoad && obj.CurrentlyInCooldown != null)
                    foreach (var player in obj.CurrentlyInCooldown.ToList())
                        SvMan.StartCoroutine(WarpCooldown(player.Key, obj, player.Value));
                if (DebugLevel >= 1)
                    Debug.Log($"{SetTimeStamp.Run()}[INFO] Loaded warp: {obj.Name}");
            }
            if (DebugLevel >= 1)
                Debug.Log($"{SetTimeStamp.Run()}[INFO] Loaded in {listWarps.Count} warp(s).");
        }
        public static void CreateWarp(SvPlayer player, string name, int delay, int price, string fileName)
        {
            var obj = new Warps_Json.Warps_RootObj
            {
                Name = name,
                Delay = delay,
                Price = price < 0 ? 0 : price,
                Disabled = false,
                ExecutableBy = "everyone",
                Position = new Warps_Json.Position { X = player.player.GetPosition().x, Y = player.player.GetPosition().y, Z = player.player.GetPosition().z},
                Rotation = new Warps_Json.Rotation { X = player.player.GetRotation().x, Y = player.player.GetRotation().y, Z = player.player.GetRotation().z, W = player.player.GetRotation().w}
            };
            File.WriteAllText(fileName, JsonConvert.SerializeObject(obj, Formatting.Indented));
            listWarps.Add(obj);
        }
        public static void DeleteWarp(SvPlayer player, string fileName, string name)
        {
            File.Delete(fileName);
            listWarps = listWarps.Where(x => x.Name != name).ToList();
        }
        public static void StartWarpTimer()
        {
            try
            {
                _Timer.Elapsed += (sender, e) => {
                    foreach (var warp in listWarps)
                    {
                        var path = Path.Combine(KitDirectory, $"{warp.Name}.json");
                        if (File.Exists(path))
                            File.WriteAllText(path, JsonConvert.SerializeObject(warp, Formatting.Indented));
                    }
                };
                _Timer.Interval = 30 * 60 * 1000; // Save every 30 minutes
                _Timer.Enabled = true;
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
        public static IEnumerator WarpCooldown(string username, Warps_Json.Warps_RootObj warp, int PassedTime = 0)
        {
            if (!warp.CurrentlyInCooldown.ContainsKey(username))
                warp.CurrentlyInCooldown.Add(username, warp.Delay);
            var path = Path.Combine(WarpDirectory, $"{warp.Name}.json");
            File.WriteAllText(path, JsonConvert.SerializeObject(warp, Formatting.Indented));
            var passedTime = PassedTime;
            while (passedTime <= warp.Delay)
            {
                ++passedTime;
                --warp.CurrentlyInCooldown[username];
                yield return new WaitForSeconds(1);
            }
            if (warp.CurrentlyInCooldown.ContainsKey(username))
                warp.CurrentlyInCooldown.Remove(username);
            if (File.Exists(path))
                File.WriteAllText(path, JsonConvert.SerializeObject(warp, Formatting.Indented));
        }
    }
}
