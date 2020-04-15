using BrokeProtocol.API;

namespace BPEssentials.Modules
{

    public class ModuleInfo
    {
        public ModuleInfo()
        {
        }

        public ModuleInfo(Plugin plugin)
        {
            Plugin = plugin;
        }

        public Plugin Plugin { get; set; }
    }
}
