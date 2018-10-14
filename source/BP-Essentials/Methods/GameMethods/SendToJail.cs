using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials
{
    class SendToJail
    {
        public static bool Run(ShPlayer shPlayer, float time)
        {
            if (shPlayer.IsDead())
                return false;
            var jailSpawn = shPlayer.manager.jail.transform;
            SetJob.Run(shPlayer, 2, true, false);
            shPlayer.svEntity.SvReset(jailSpawn.position, jailSpawn.rotation, 0);
            shPlayer.StartCoroutine(shPlayer.svPlayer.JailTimer(time));
            shPlayer.svPlayer.SvClearCrimes();
            shPlayer.RemoveItemsJail();
            shPlayer.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowTimer, time);
            return true;
        }
    }
}
