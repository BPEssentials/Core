using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials
{
    class HasPermission
    {
        public static bool Run(SvPlayer player, string ExeBy, bool ShowNoPermMessage = false, byte? jobIndex = null)
        {
            if ((player.player.admin && ExeBy.Contains("admins")) || ExeBy.Contains("everyone") || ExeBy.Contains($"username:{player.player.username}"))
                return true;
            string[] GroupsSplit = ExeBy.Split(',');
            foreach (string name in GroupsSplit)
                if (jobIndex != null)
                {
                    if (Groups.Any(curr => $"group:{curr.Value.Name}".Equals(name.Trim()) && curr.Value.Users.Contains(player.playerData.username)) || name == $"jobindex:{jobIndex}")
                        return true;
                }
                else
                    if (Groups.Any(curr => $"group:{curr.Value.Name}".Equals(name.Trim()) && curr.Value.Users.Contains(player.playerData.username)))
                        return true;
            if (ShowNoPermMessage)
                player.SendChatMessage($"<color={errorColor}>{MsgNoPerm}</color>");
            return false;
        }
    }
}
