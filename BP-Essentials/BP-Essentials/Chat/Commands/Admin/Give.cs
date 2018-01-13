using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Give : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (AdminsListPlayers.Contains(player.playerData.username) && CmdGiveExecutableBy == "admin" || CmdGiveExecutableBy == "everyone")
                {
                    string arg1 = GetArgument.Run(1, false, false, message);
                    string arg2 = GetArgument.Run(2, false, false, message);
                    int arg1int, arg2int;
                    bool Parsed = Int32.TryParse(arg1, out arg1int);
                    Parsed = int.TryParse(arg2, out arg2int);
                    if (Parsed)
                        foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                            if (shPlayer.svPlayer == player && shPlayer.IsRealPlayer())
                            {
                                shPlayer.playerInventory.TransferItem(1, IDs[arg1int], arg2int, true);
                                player.SendToSelf(Channel.Unsequenced, 10, "Giving you item ID: " + arg1);
                            }
                            else
                                player.SendToSelf(Channel.Unsequenced, 10, "Error: Is that a valid number you provided as argument?");
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
