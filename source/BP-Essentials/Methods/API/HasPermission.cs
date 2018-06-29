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
        public static bool Run(SvPlayer player, string ExeBy, bool ShowNoPermMessage = false, byte? jobIndex = null)
        {
            if (AdminsListPlayers.Contains(player.playerData.username) && ExeBy.Contains("admins") || ExeBy.Contains("everyone"))
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
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>{MsgNoPerm}</color>");
            return false;
        }
    }
}
