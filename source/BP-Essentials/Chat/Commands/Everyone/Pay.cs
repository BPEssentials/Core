using static BP_Essentials.EssentialsVariablesPlugin;
using System;
using System.Linq;
using UnityEngine;

namespace BP_Essentials.Commands
{
    public class Pay : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (HasPermission.Run(player, CmdPayExecutableBy))
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
                    int arg2Int;
                    var isNumeric = int.TryParse(arg2, out arg2Int);
                    if (isNumeric)
                    {
                        if (arg2Int <= 0)
                        {
                            player.SendToSelf(Channel.Unsequenced, 10, $"<color={errorColor}>Cannot transfer 0$.</color>");
                            return true;
                        }
                        foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                            if (shPlayer.username == arg1 && shPlayer.IsRealPlayer() || shPlayer.ID.ToString() == arg1.ToString() && shPlayer.IsRealPlayer())
                                foreach (var _shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                                    if (_shPlayer.svPlayer == player && _shPlayer.IsRealPlayer())
                                    {
                                        if (_shPlayer.MyMoneyCount() >= arg2Int)
                                        {
                                            _shPlayer.TransferMoney(2, arg2Int, true);
                                            shPlayer.TransferMoney(1, arg2Int, true);
                                            player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>Successfully transfered</color> <color={argColor}>{arg2Int}</color><color={infoColor}>$ to </color><color={argColor}>{shPlayer.username}</color><color={infoColor}>!</color>");
                                            shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, 10, $"<color={argColor}>{player.playerData.username}</color><color={infoColor}> gave you </color><color={argColor}>{arg2Int}</color><color={infoColor}>$!</color>");
                                        }
                                        else
                                            player.SendToSelf(Channel.Unsequenced, 10, $"<color={errorColor}>Cannot transfer money, do you have</color><color={argColor}> " + arg2Int + $"</color><color={errorColor}>$ in your inventory?</color>");
                                        return true;
                                    }

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
