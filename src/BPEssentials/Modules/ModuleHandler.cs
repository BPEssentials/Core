using BPCoreLib.Util;
using BrokeProtocol.API;
using System.Collections.Generic;

namespace BPEssentials.Modules
{
    public class ModuleHandler
    {
        public Logger Logger { get; set; }

        public List<Module> Modules { get; } = new List<Module>();

        public void Initialize()
        {
            FindAll();
        }

        public void FindAll()
        {
            foreach (var plugin in BPAPI.Instance.Plugins)
            {
                if (!plugin.Value.Plugin.CustomData.TryFetchCustomData("bpe:isModule", out bool val) || !val)
                {
                    continue;
                }
                if (!plugin.Value.Plugin.CustomData.TryFetchCustomData("bpe:module", out Module module) || module == null)
                {
                    continue;
                }
                Logger.LogInfo($"[BPE] [MODULE] Loading module '{module.GetName()}'..");
                Modules.Add(module);
                module.OnLoad();
                Logger.LogInfo($"[BPE] [MODULE] Module named '{module.GetName()}' loaded in correctly!");
            }
        }

        public void LoadAll()
        {
            foreach (var module in Modules)
            {
                module?.OnLoad();
            }
        }

        public void UnloadAll()
        {
            foreach (var module in Modules)
            {
                module?.OnUnload();
            }
        }
    }
}