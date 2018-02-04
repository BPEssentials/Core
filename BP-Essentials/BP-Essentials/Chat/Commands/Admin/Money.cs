using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Money : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (AdminsListPlayers.Contains(player.playerData.username) && CmdMoneyExecutableBy == "admins" || CmdMoneyExecutableBy == "everyone")
                {
                    string CorrSyntax = $"<color={argColor}>" + GetArgument.Run(0, false, false, message) + $"</color><color={errorColor}> [Player] [Amount]</color><color={warningColor}> (Incorrect or missing argument(s).)</color>";
                    string arg1 = GetArgument.Run(1, false, true, message);
                    string arg2 = message.Split(' ').Last().Trim();
                    if (String.IsNullOrEmpty(GetArgument.Run(1, false, false, message)) || String.IsNullOrEmpty(arg2))
                    {
                        player.SendToSelf(Channel.Unsequenced, 10, CorrSyntax);
                        return true;
                    }
                    else
                    {
                        int lastIndex = arg1.LastIndexOf(" ");
                        if (lastIndex != -1)
                            arg1 = arg1.Remove(lastIndex).Trim();
                    }
                    int arg2int;
                    bool isNumeric = int.TryParse(arg2, out arg2int);
                    if (isNumeric)
                    {
                        bool found = false;
                        foreach (var shPlayer in FindObjectsOfType<ShPlayer>())
                            if (shPlayer.username == arg1 && shPlayer.IsRealPlayer() || shPlayer.ID.ToString() == arg1 && shPlayer.IsRealPlayer())
                            {
                                shPlayer.playerInventory.TransferMoney(1, arg2int, true);
                                player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>Successfully gave</color><color={argColor}> " + shPlayer.username + " " + arg2int + $"</color><color={infoColor}>$</color>");
                                shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, 10, $"<color={argColor}>" + player.playerData.username + $"</color><color={infoColor}> gave you </color><color={argColor}>" + arg2int + $"</color><color={infoColor}>$!</color>");
                                found = true;
                            }
                        if (!found)
                            player.SendToSelf(Channel.Unsequenced, 10, NotFoundOnline);
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, 10, CorrSyntax);
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
