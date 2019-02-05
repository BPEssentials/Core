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
    class SendChatMessageToAdmins
    {
        public static void Run(string message)
        {
            foreach (var player in PlayerList.Where(x =>x.Value.ReceiveStaffChat && HasPermission.Run(x.Value.ShPlayer.svPlayer, CmdStaffChatExecutableBy)))
                player.Value.ShPlayer.svPlayer.SendChatMessage(message);
        }
    }
}
