using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials
{
    class SaveNow : EssentialsChatPlugin
    {
        public static void Run()
        {
            Debug.Log(SetTimeStamp() +"[INFO] Saving game..");
            foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                if (shPlayer.IsRealPlayer())
                {
                    if (shPlayer.GetSpaceIndex() >= 13) continue;
                    shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, (byte)10, "Saving game.. This can take up to 5 seconds.");
                    shPlayer.svPlayer.Save();
                }
        }
    }
}
