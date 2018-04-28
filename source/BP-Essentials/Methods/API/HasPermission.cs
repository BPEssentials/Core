using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials
{
    class HasPermission : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string ExeBy, bool ShowNoPermMessage = false)
        {
            var player = (SvPlayer)oPlayer;
            if (AdminsListPlayers.Contains(player.playerData.username) && ExeBy.Contains("admins") || ExeBy.Contains("everyone"))
                return true;
            string[] GroupsSplit = ExeBy.Split(',');
            foreach (string name in GroupsSplit)
                if (Groups.Any(curr => $"group:{curr.Value.Name}".Equals(name.Trim()) && curr.Value.Users.Contains(player.playerData.username)))
                    return true;
            if (ShowNoPermMessage)
                player.SendToSelf(Channel.Unsequenced, 10, $"<color={errorColor}>{MsgNoPerm}</color>");
            return false;
        }
    }
}
