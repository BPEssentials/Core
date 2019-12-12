using BPCoreLib.Util;
using BrokeProtocol.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            foreach (var plugin in (IEnumerable<Plugin>)Plugins.GetPlugin("please replace me later"))
            {
                if (!plugin.CustomData.TryFetchCustomData("bpe:isModule", out bool val) || !val)
                {
                    continue;
                }
                if (!plugin.CustomData.TryFetchCustomData("bpe:module", out Module module) || module == null)
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
