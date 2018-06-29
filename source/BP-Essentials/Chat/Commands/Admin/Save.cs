using static BP_Essentials.EssentialsVariablesPlugin;
using System;
using System.Threading;
using static BP_Essentials.EssentialsCorePlugin;

namespace BP_Essentials.Commands {
    public class Save : EssentialsChatPlugin {
        public static void Run()
        {
            var thread = new Thread(SaveNow.Run);
            thread.Start();
        }
    }
}