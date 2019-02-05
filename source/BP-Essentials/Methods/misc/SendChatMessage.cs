using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;
using System.IO;
using System.Threading;
using System.Reflection;

namespace BP_Essentials
{
    class SendChatMessage
    {
        public static void Run(string message)
        {
            foreach (var player in PlayerList.Where(x => x.Value.ChatEnabled))
                player.Value.ShPlayer.svPlayer.SendChatMessage(message);
        }
    }
}
