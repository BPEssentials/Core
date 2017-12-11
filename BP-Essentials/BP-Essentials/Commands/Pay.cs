using static BP_Essentials.EssentialsVariablesPlugin;
using System;
using System.Linq;
using UnityEngine;

namespace BP_Essentials.Commands {
    public class Pay : EssentialsChatPlugin {
        public static bool Run(object oPlayer, string message) {
            string arg1 = null;
            string arg2 = null;
            var player = (SvPlayer)oPlayer;

                    try
                    {
                        if (message.StartsWith(CmdPay))
                        {
                            arg1 = message.Substring(CmdPay.Length + 1).Trim();
                        }
                        else if (message.StartsWith(CmdPay2))
                        {
                            arg1 = message.Substring(CmdPay2.Length + 1).Trim();
                        }
                        arg2 = message.Split(' ').Last().Trim();
                        arg1 = arg1.Substring(0, arg1.Length - arg2.Length).Trim();
                        if (string.IsNullOrEmpty(arg1))
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, CmdPay + " / " + CmdPay2 + " [Player] [Amount]");
                            return true;
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, CmdPay + " / " + CmdPay2 + " [Player] [Amount]");
                        return true;
                    }
                    if (!(String.IsNullOrEmpty(arg2)))
                    {
                        int arg2Int;
                        var isNumeric = int.TryParse(arg2, out arg2Int);

                        if (isNumeric)
                        {
                            var error = false;
                            var found = false;
                            if (arg2Int == 0)
                            {
                                error = true;
                            }
                            if (!(error))
                            {
                                foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                                {
                                    if (shPlayer.svPlayer.playerData.username == arg1 || shPlayer.ID.ToString() == arg1.ToString())
                                    {
                                        if (shPlayer.IsRealPlayer())
                                        {
                                            foreach (var _shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                                            {
                                                if (_shPlayer.svPlayer == player)
                                                {
                                                    if (_shPlayer.IsRealPlayer())
                                                    {
                                                        Debug.Log(_shPlayer.playerInventory.MyMoneyCount());
                                                        if (_shPlayer.playerInventory.MyMoneyCount() >= arg2Int)
                                                        {
                                                            _shPlayer.playerInventory.TransferMoney(2, arg2Int, true);
                                                        }
                                                        else
                                                        {
                                                            error = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            if (!(error))
                                            {
                                                shPlayer.playerInventory.TransferMoney(1, arg2Int, true);
                                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Succesfully transfered " + arg2Int + "$ to " + shPlayer.svPlayer.playerData.username + "!");
                                                shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, (byte)10, player.playerData.username + " gave you " + arg2Int + "$!");
                                                found = true;
                                            }
                                            else
                                            {
                                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Cannot transfer money, do you have " + arg2Int + "$ in your inventory?");
                                                found = true;
                                            }
                                        }
                                    }
                                }
                                if (!(found))
                                {
                                    player.SendToSelf(Channel.Unsequenced, (byte)10, arg1 + @" Not found/online.");
                                }
                            }
                            else
                            {
                                if (arg2Int == 0)
                                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Cannot transfer 0$.");
                                else
                                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Cannot transfer money, do you have " + arg2Int + "$ in your inventory?");

                            }
                        }
                        else
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, CmdPay + " / " + CmdPay2 + " [Player] [Amount] (incorrect argument!)");
                        }
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, CmdPay + " / " + CmdPay2 + " [Player] [Amount]");
                    }
                    return true;
                }
        }
    }
