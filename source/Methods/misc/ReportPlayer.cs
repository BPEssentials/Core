using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BP_Essentials
{
    class ReportPlayer : EssentialsVariablesPlugin
    {
        public static void Run(string Reporter, string ReportReason, object ReportedPlayer)
        {
            try
            {
                var reportedPlayer = (ShPlayer)ReportedPlayer;
                foreach (KeyValuePair<_PlayerList, int> item in playerList)
                {
                    if (item.Key.shplayer.admin)
                    {
                        if (item.Key.LastMenu == CurrentMenu.Main)
                        {
                            item.Key.shplayer.svPlayer.SendToSelf(Channel.Reliable, 76);
                            item.Key.shplayer.svPlayer.SendToSelf(Channel.Reliable, 75, "<color=#00ffffff>New report:</color>\n<color=#00ffffff>Username: </color>" + reportedPlayer.username + "\n<color=#00ffffff>Reporter: </color>" + Reporter + "\n<color=#00ffffff>Reason: </color>" + ReportReason + "\n\n<color=#00ffffff>F2: </color>Teleport to player<color=#00ffffff>\nF3-11: </color>Close menu\n\n<color=#00ffffff>Press</color><color=#ea8220> F11 </color><color=#00ffffff>To close this (G)UI</color>");
                            item.Key.LastMenu = CurrentMenu.AdminReport;
                            item.Key.reportedPlayer = reportedPlayer;
                        }
                    }
                    continue;
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
