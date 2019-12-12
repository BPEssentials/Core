using BrokeProtocol.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPEssentials.Modules
{
    public abstract class Module
    {
        public abstract ModuleInfo ModuleInfo { get; set; }

        public virtual ModuleInfo GetModuleInfo()
        {
            return ModuleInfo;
        }

        public virtual void OnLoad()
        {
        }

        public virtual void OnUnload()
        {
        }

        public string GetName()
        {
            return ModuleInfo?.Plugin?.GetPluginName();
        }

        public string GetDescription()
        {
            return ModuleInfo?.Plugin?.GetPluginDescription();
        }
    }

    public class Test : Module
    {
        public override ModuleInfo ModuleInfo { get; set; } = new ModuleInfo(Core.Instance);
    }
}
