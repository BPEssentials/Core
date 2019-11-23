using BrokeProtocol.Collections;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace BPEssentials.Cooldowns
{
    public class CooldownHandler
    {
        public Dictionary<ulong, Dictionary<string, Dictionary<string, int>>> Cooldowns { get; set; } = new Dictionary<ulong, Dictionary<string, Dictionary<string, int>>>();

        private bool ready = false;

        public void Load()
        {
            foreach (var player in Core.Instance.SvManager.Database.Users.FindMany(x => x.Character.CustomData.Data.ContainsKey("bpe:cooldowns")).ToDictionary(x => x.ID, x => x.Character.CustomData))
            {
                Core.Instance.Logger.Log("Reading " + player.Key.ToString());
                var cooldownsObj = player.Value.FetchCustomData<Dictionary<string, Dictionary<string, int>>>("bpe:cooldowns");
                Cooldowns.Add(player.Key, cooldownsObj);
                foreach (var cooldownType in cooldownsObj)
                {
                    Core.Instance.Logger.Log("Reading " + cooldownType.Key);

                    foreach (var cooldown in cooldownType.Value)
                    {
                        Core.Instance.Logger.Log("Reading " + cooldown.Key);

                        AddCooldown(player.Key, cooldownType.Key, cooldown.Key);
                    }
                    Core.Instance.Logger.Log("LOOP CLOSED");

                }
                Core.Instance.Logger.Log("LOOP CLOSED");

            }
            Core.Instance.Logger.Log("LOOP CLOSED");
            ready = true;

        }


        public void SaveCooldowns()
        {
            foreach (var cooldownPlayer in Cooldowns)
            {
                Core.Instance.Logger.Log(cooldownPlayer.Key.ToString());
                Core.Instance.Logger.Log(JsonConvert.SerializeObject(cooldownPlayer.Value));
                var onlinePlayer = EntityCollections.Humans.FirstOrDefault(x => x.svPlayer.steamID == cooldownPlayer.Key);
                if (onlinePlayer)
                {
                    onlinePlayer.svPlayer.CustomData.AddOrUpdate("bpe:cooldowns", cooldownPlayer.Value);
                }
                else
                {
                    var newUser = Core.Instance.SvManager.Database.Users.FindSingle(x => x.ID == cooldownPlayer.Key);
                    newUser.Character.CustomData.AddOrUpdate("bpe:cooldowns", cooldownPlayer.Value);
                    Core.Instance.SvManager.Database.Users.UpdateSingle(newUser);
                }
            }
        }

        private IEnumerator StartCooldown(ulong ID, string type, string key)
        {
            Core.Instance.Logger.Log("Starting Cooldown " + key);
            while (!ready)
            {
                yield return new WaitForSecondsRealtime(1);
            }

            while (Cooldowns[ID][type][key] > 0)
            {
                Core.Instance.Logger.Log(key + "New val " + Cooldowns[ID][type][key].ToString());
                --Cooldowns[ID][type][key];
                yield return new WaitForSecondsRealtime(1);
            }
            if (Cooldowns[ID][type].ContainsKey(key))
            {
                Cooldowns[ID][type].Remove(key);
            }

        }

        public void AddCooldown(ulong ID, string type, string key, int time = 0)
        {
            if (!Cooldowns.ContainsKey(ID))
            {
                Cooldowns.Add(ID, new Dictionary<string, Dictionary<string, int>>());
            }
            if (!Cooldowns[ID].ContainsKey(type))
            {
                Cooldowns[ID].Add(type, new Dictionary<string, int>());
            }
            if (!Cooldowns[ID][type].ContainsKey(key))
            {
                Cooldowns[ID][type].Add(key, time);
            }
            Core.Instance.SvManager.StartCoroutine(StartCooldown(ID, type, key));
        }

        public bool IsCooldown(ulong ID, string type, string key)
        {
            return GetCooldown(ID, type, key) > 0;
        }

        public int GetCooldown(ulong ID, string type, string key)
        {
            if (!Cooldowns.ContainsKey(ID) || !Cooldowns[ID].ContainsKey(type) || !Cooldowns[ID][type].ContainsKey(key)) { return 0; }
            return Cooldowns[ID][type][key];
        }
    }
}
