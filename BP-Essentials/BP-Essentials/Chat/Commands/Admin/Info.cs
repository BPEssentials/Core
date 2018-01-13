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
                if (AdminsListPlayers.Contains(player.playerData.username) && CmdInfoExecutableBy == "admin" || CmdInfoExecutableBy == "everyone")
                {
                    var arg1 = GetArgument.Run(1, false, true, message);
                    var found = false;
                    if (!String.IsNullOrWhiteSpace(arg1))
                    {
                        foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                            if (shPlayer.svPlayer.playerData.username == arg1 || shPlayer.ID.ToString() == arg1.ToString())
                                if (shPlayer.IsRealPlayer())
                                {
                                    shPlayer.svPlayer.Save();
                                    player.SendToSelf(Channel.Unsequenced, 10, "Info about: '" + shPlayer.svPlayer.playerData.username + "'.");
                                    string[] contentarray = {
                                    "Username:              " +  shPlayer.svPlayer.playerData.username,
                                    "",
                                    "",
                                    "Job:                         " + Jobs[shPlayer.svPlayer.playerData.jobIndex],
                                    "Health:                    " + shPlayer.svPlayer.playerData.health,
                                    "OwnsApartment:   " + shPlayer.svPlayer.playerData.ownedApartment,
                                    "Position:                 " + shPlayer.svPlayer.playerData.position,
                                    "WantedLevel:         " + shPlayer.wantedLevel,
                                    "IsAdmin:                 " + shPlayer.admin,
                                    "IP:                            " + shPlayer.svPlayer.netMan.GetAddress(shPlayer.svPlayer.connection)
                                };

                                    var content = string.Join("\r\n", contentarray);

                                    player.SendToSelf(Channel.Reliable, 50, content);

                                    found = true;
                                }
                        if (!(found))
                            player.SendToSelf(Channel.Unsequenced, 10, "'" + arg1 + "' Not found/online.");
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
