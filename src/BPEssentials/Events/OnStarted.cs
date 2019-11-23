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

            Core.Instance.CooldownHandler.Load();
        }
    }
}
