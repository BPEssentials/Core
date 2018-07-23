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
        public static void LoadAllKits()
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
                if (DebugLevel >= 1)
                    Debug.Log($"{SetTimeStamp.Run()}[INFO] Loaded kit: {obj.Name}");
            }
            if (DebugLevel >= 1)
                Debug.Log($"{SetTimeStamp.Run()}[INFO] Loaded in {listKits.Count} kit(s).");
        }
        public static void CreateKit(SvPlayer player, string name, int delay, string fileName)
        {
            var obj = new Kits_Json.Kits_RootObj
            {
                Name = name,
                Delay = delay,
                Disabled = false,
                ExecutableBy = "everyone"
            };
            foreach (var item in player.player.myItems.Values)
                obj.Items.Add(new Kits_Json.Kits_Item { Amount = item.count, Id = item.item.index });
            File.WriteAllText(fileName, JsonConvert.SerializeObject(obj, Formatting.Indented));
            listKits.Add(obj);
        }

        public static IEnumerator KitCooldown(SvPlayer player, Kits_Json.Kits_RootObj kit)
        {
            if (!kit.CurrentlyInCooldown.ContainsKey(player.player.username))
                kit.CurrentlyInCooldown.Add(player.player.username, kit.Delay);
            var passedTime = 0f;
            while (passedTime < kit.Delay)
            {
                yield return new WaitForSeconds(1);
                ++passedTime;
                --kit.CurrentlyInCooldown[player.player.username];
            }
            if (kit.CurrentlyInCooldown.ContainsKey(player.player.username))
                kit.CurrentlyInCooldown.Remove(player.player.username);
        }
    }
}
