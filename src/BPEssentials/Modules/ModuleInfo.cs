using BrokeProtocol.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
