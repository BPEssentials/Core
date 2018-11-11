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
    public class Kits
    {
        public static void LoadAllKits(bool firstLoad = false)
        {
            if (DebugLevel >= 1)
                Debug.Log($"{SetTimeStamp.Run()}[INFO] Loading kits..");
            listKits.Clear();
            foreach (string file in Directory.EnumerateFiles(KitDirectory, "*.json", SearchOption.AllDirectories))
            {
                var obj = JsonConvert.DeserializeObject<Kits_Json.Kits_RootObj>(FilterComments.Run(file));
                if (listKits.Any(x => x.Name == obj.Name))
                {
                    Debug.Log($"{SetTimeStamp.Run()}[ERROR] Cannot add kit {obj.Name} because it already exists in the list!");
                    continue;
                }
                listKits.Add(obj);
                if (firstLoad && obj.CurrentlyInCooldown != null)
                    foreach (var player in obj.CurrentlyInCooldown.ToList())
                        SvMan.StartCoroutine(KitCooldown(player.Key, obj, player.Value));
                if (DebugLevel >= 1)
                    Debug.Log($"{SetTimeStamp.Run()}[INFO] Loaded kit: {obj.Name}");
            }
            if (DebugLevel >= 1)
                Debug.Log($"{SetTimeStamp.Run()}[INFO] Loaded in {listKits.Count} kit(s).");
        }
        public static void CreateKit(SvPlayer player, string name, int delay, int price, string fileName)
        {
            var obj = new Kits_Json.Kits_RootObj
            {
                Name = name,
                Delay = delay,
                Price = price < 0 ? 0 : price,
                Disabled = false,
                ExecutableBy = "everyone"
            };
            foreach (var item in player.player.myItems.Values)
                obj.Items.Add(new Kits_Json.Kits_Item { Amount = item.count, Id = item.item.index });
            File.WriteAllText(fileName, JsonConvert.SerializeObject(obj, Formatting.Indented));
            listKits.Add(obj);
        }
        public static void DeleteKit(SvPlayer player, string fileName, string name)
        {
            File.Delete(fileName);
            listKits = listKits.Where(x => x.Name != name).ToList();
        }
        public static void StartKitTimer()
        {
            try
            {
                _Timer.Elapsed += (sender, e) => {
                    foreach (var kit in listKits)
                    {
                        var path = Path.Combine(KitDirectory, $"{kit.Name}.json");
                        if (File.Exists(path))
                            File.WriteAllText(path, JsonConvert.SerializeObject(kit, Formatting.Indented));
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
        public static IEnumerator KitCooldown(string username, Kits_Json.Kits_RootObj kit, int PassedTime = 0)
        {
            if (!kit.CurrentlyInCooldown.ContainsKey(username))
                kit.CurrentlyInCooldown.Add(username, kit.Delay);
            var path = Path.Combine(KitDirectory, $"{kit.Name}.json");
            File.WriteAllText(path, JsonConvert.SerializeObject(kit, Formatting.Indented));
            var passedTime = PassedTime;
            while (passedTime <= kit.Delay)
            {
                ++passedTime;
                --kit.CurrentlyInCooldown[username];
                yield return new WaitForSeconds(1);
            }
            if (kit.CurrentlyInCooldown.ContainsKey(username))
                kit.CurrentlyInCooldown.Remove(username);
            if (File.Exists(path))
                File.WriteAllText(path, JsonConvert.SerializeObject(kit, Formatting.Indented));
        }
    }
}
