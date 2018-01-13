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
                if (AdminsListPlayers.Contains(player.playerData.username) && CmdMoneyExecutableBy == "admin" || CmdMoneyExecutableBy == "everyone")
                {
                    string arg1 = null;
                    string arg2 = null;
                    try
                    {
                        if (message.StartsWith(CmdMoney))
                            arg1 = message.Substring(CmdMoney.Length + 1).Trim();
                        else if (message.StartsWith(CmdMoney2))
                            arg1 = message.Substring(CmdMoney2.Length + 1).Trim();
                        arg2 = message.Split(' ').Last().Trim();
                        arg1 = arg1.Substring(0, arg1.Length - arg2.Length).Trim();
                        if (String.IsNullOrEmpty(arg1))
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, GetArgument.Run(0, false, false, message) + " [Player] [Amount]");
                            return true;
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, GetArgument.Run(0, false, false, message) + " [Player] [Amount]");
                        return true;
                    }
                    if (!(String.IsNullOrEmpty(arg2)))
                    {
                        int arg2int;
                        bool isNumeric = int.TryParse(arg2, out arg2int);

                        if (isNumeric)
                        {
                            bool found = false;
                            foreach (var shPlayer in FindObjectsOfType<ShPlayer>())
                                if (shPlayer.svPlayer.playerData.username == arg1 || shPlayer.ID.ToString() == arg1.ToString())
                                    if (shPlayer.IsRealPlayer())
                                    {
                                        shPlayer.playerInventory.TransferMoney(1, arg2int, true);
                                        player.SendToSelf(Channel.Unsequenced, (byte)10, "Succesfully gave " + shPlayer.svPlayer.playerData.username + " " + arg2int + "$");
                                        shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, (byte)10, player.playerData.username + " gave you " + arg2int + "$!");
                                        found = true;
                                    }
                            if (!(found))
                                player.SendToSelf(Channel.Unsequenced, (byte)10, arg1 + @" Not found/online.");

                        }
                        else
                            player.SendToSelf(Channel.Unsequenced, (byte)10, GetArgument.Run(0, false, false, message) + " [Player] [Amount] (incorrect argument!)");
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, GetArgument.Run(0, false, false, message) + " [Player] [Amount]");
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
