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
                if (HasPermission.Run(player, CmdGiveExecutableBy))
                {
                    string arg1 = GetArgument.Run(1, false, false, message);
                    string arg2 = GetArgument.Run(2, false, false, message);
                    if (String.IsNullOrEmpty(arg1) || String.IsNullOrEmpty(arg2))
                    {
                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
                        return true;
                    }
                    int arg1int, arg2int;
                    bool Parsed = Int32.TryParse(arg1, out arg1int);
                    Parsed = int.TryParse(arg2, out arg2int);
                    if (Parsed)
                    {
                        if (arg1int > 1 && arg1int <= IDs.Length -1)
                        {
                            foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                                if (shPlayer.svPlayer == player && shPlayer.IsRealPlayer())
                                {
                                    if (arg1.Length > 4)
                                        shPlayer.TransferItem(1, arg1int, arg2int, true);
                                    else
                                        shPlayer.TransferItem(1, IDs[arg1int], arg2int, true);
                                    player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Giving you item ID: </color><color={argColor}>{arg1}</color>");
                                }
                        }
                        else
                            player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>Error: The ID must be between 1 and {IDs.Length -1}.</color>");
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>Error: Is that a valid number you provided as argument?</color>");
                }
                else
                    player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, MsgNoPerm);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}
