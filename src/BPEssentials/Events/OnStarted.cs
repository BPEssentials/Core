using BPEssentials.ExtensionMethods;
using BrokeProtocol;
using BrokeProtocol.API;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BPEssentials.RegisteredEvents
{
    public class OnStarted : IScript
    {
        public OnStarted()
        {
            GameSourceHandler.Add(BrokeProtocol.API.Events.Manager.OnStarted, new Action<SvManager>(OnEvent));
        }

        public void OnEvent(SvManager svManager)
        {
            Core.Instance.SvManager = svManager;

            foreach (var player in svManager.Database.Users.FindMany(x => x.Character.CustomData.Data.ContainsKey("bpe:cooldowns")).ToDictionary(x => x.ID, x => x.Character.CustomData))
            {
                Core.Instance.Logger.Log("Reading " + player.Key.ToString());
                var cooldownsObj = player.Value.FetchCustomData<Dictionary<string, Dictionary<string, int>>>("bpe:cooldowns");
                Core.Instance.Cooldowns.Add(player.Key, cooldownsObj);
            }
        }
    }
}
