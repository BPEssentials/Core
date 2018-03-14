using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Info : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (AdminsListPlayers.Contains(player.playerData.username) && CmdInfoExecutableBy == "admins" || CmdInfoExecutableBy == "everyone")
                {
                    var arg1 = GetArgument.Run(1, false, true, message);
                    var found = false;
                    if (!String.IsNullOrEmpty(arg1))
                    {
                        foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                            if (shPlayer.username == arg1 || shPlayer.ID.ToString() == arg1.ToString())
                                if (shPlayer.IsRealPlayer())
                                {
                                    player.SendToSelf(Channel.Unsequenced, 10, "Info about: '" + shPlayer.username + "'.");
                                    string[] contentarray = {
                                    "Username:              " +  shPlayer.username,
                                    "",
                                    "",
                                    "Job:                         " + Jobs[shPlayer.job.jobIndex],
                                    "Health:                    " + shPlayer.health,
                                    "OwnsApartment:   " + shPlayer.ownedApartment,
                                    "Position:                 " + shPlayer.GetPosition().ToString(),
                                    "WantedLevel:         " + shPlayer.wantedLevel,
                                    "IsAdmin:                 " + shPlayer.admin,
                                    "BankBalance:       " + shPlayer.svPlayer.bankBalance,
                                    "IP:                            " + shPlayer.svPlayer.svManager.GetAddress(shPlayer.svPlayer.connection)
                                };

                                    var content = string.Join("\r\n", contentarray);

                                    player.SendToSelf(Channel.Reliable, 50, content);

                                    found = true;
                                }
                        if (!(found))
                            player.SendToSelf(Channel.Unsequenced, 10, NotFoundOnline);
                    }
                    else
                        player.SendToSelf(Channel.Reliable, 10, ArgRequired);
                }
                else
                    player.SendToSelf(Channel.Unsequenced, 10, MsgNoPerm);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}
