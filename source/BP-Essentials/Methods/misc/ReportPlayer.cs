using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BP_Essentials
{
    class ReportPlayer : EssentialsVariablesPlugin
    {
        public static void Run(string Reporter, string ReportReason, ShPlayer ReportedPlayer)
        {
            try
            {
                foreach (var currItem in playerList.Values)
                {
                    if (currItem.Shplayer.admin)
                    {
                        if (currItem.LastMenu == CurrentMenu.Main)
                        {
                            currItem.Shplayer.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.CloseFunctionMenu);
                            currItem.Shplayer.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowFunctionMenu, "<color=#00ffffff>New report:</color>\n<color=#00ffffff>Username: </color>" + ReportedPlayer.username + "\n<color=#00ffffff>Reporter: </color>" + Reporter + "\n<color=#00ffffff>Reason: </color>" + ReportReason + "\n\n<color=#00ffffff>F2: </color>Teleport to player<color=#00ffffff>\nF3-11: </color>Close menu\n\n<color=#00ffffff>Press</color><color=#ea8220> F11 </color><color=#00ffffff>To close this (G)UI</color>");
                            currItem.LastMenu = CurrentMenu.AdminReport;
                            currItem.ReportedPlayer = ReportedPlayer;
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
