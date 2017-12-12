using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.Threading;

namespace BP_Essentials
{
    class SavePeriodically : EssentialsChatPlugin
    {
        public static void Run()
        {
            while (true)
            {
                Debug.Log(SetTimeStamp() + "[INFO] Saving game..");
                foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                    if (shPlayer.IsRealPlayer())
                    {
                        if (shPlayer.GetSpaceIndex() >= 13) continue;
                        shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, (byte)10, "Saving game.. This can take up to 5 seconds.");
                        shPlayer.svPlayer.Save();
                    }
                Thread.Sleep(SaveTime * 1000);
            }
        }
    }
}
