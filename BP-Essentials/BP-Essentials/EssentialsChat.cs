using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using static BP_Essentials.EssentialsConfig;
using static BP_Essentials.EssentialsCore;
using static BP_Essentials.EssentialsChat;
using static BP_Essentials.EssentialsCmd;
using static BP_Essentials.EssentialsMethods;
namespace BP_Essentials {
    public static class EssentialsChat {
        #region Event: ChatMessage
        //Chat Events
        [Hook("SvPlayer.SvGlobalChatMessage")]
        public static bool SvGlobalChatMessage(SvPlayer player, ref string message)
        {
            // TODO: Improve ;)
            if (!(MutePlayers.Contains(player.playerData.username)))
            {
                MessageLog(message, player);
            }
            // If player is afk, unafk him
            if (AfkPlayers.Contains(player.playerData.username))
            {
                Afk(message, player);
            }
            if (message.Trim().StartsWith(CmdCommandCharacter))
            {
                if (message.StartsWith("/arg")) // TODO
                {
                    try {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, GetArgument(1, message));
                       // GetArgument(Convert.ToByte(GetArgument(1, message)), message);
                    }
                    catch (Exception ex)
                    {
                        if (ex is FormatException || ex is OverflowException)
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "ERROR: " + ex.Message);
                        }
                    }
               
                    return true;
                }
                if (message.StartsWith(CmdPay) || (message.StartsWith(CmdPay2)))
                {
                    string arg1 = null;
                    string arg2 = null;
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
                // TODO ------------------------------------------------------------------------------------------
                if (message.StartsWith("/location") || (message.StartsWith("/loc")))
                {
                    if (message.StartsWith("/location") || (message.StartsWith("/loc")))
                    {
                        if (AdminsListPlayers.Contains(player.playerData.username))
                        {
                            player.Save();
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "Your location: " + player.playerData.position);
                        }
                        else
                            player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                        return true;
                    }
                }
                if (message.StartsWith("/getplayerhash") || (message.StartsWith("/gethash")))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        var TempMSG = message.Trim();
                        if (TempMSG != "/getplayerhash" || TempMSG != "/gethash")
                        {
                            var arg1 = player.playerData.username;
                            if (TempMSG.StartsWith("/gethash"))
                                arg1 = TempMSG.Substring(8 + 1);
                            if (TempMSG.StartsWith("/getplayerhash"))
                                arg1 = TempMSG.Substring(14 + 1);
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "StringToHash: " + Animator.StringToHash(arg1).ToString());
                        }
                        else
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                    return true;
                }
                // Command: SpaceIndex
                if (message.StartsWith("/spaceindex"))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                            if (shPlayer.svPlayer == player)
                                if (shPlayer.IsRealPlayer())
                                {
                                    player.SendToSelf(Channel.Unsequenced, (byte)10, "SpaceIndex: " + shPlayer.GetSpaceIndex());
                                }
                        return true;

                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                        return true;
                    }
                }
                // TODO ------------------------------------------------------------------------------------------
                // Command: Save
                if (message.StartsWith(CmdSave))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        var thread = new Thread(SaveNow);
                        thread.Start();
                        return true;
                    }
                    player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                    return true;
                }
                // Tp
                if (message.StartsWith(CmdTpHere) || message.StartsWith(CmdTpHere2))
                {
                    var tempMsg= message.Trim();
                    if (tempMsg!= CmdTpHere || tempMsg!= CmdTpHere2)
                    {
                        var arg1 = String.Empty;
                        if (tempMsg.StartsWith(CmdTpHere + " "))
                        {
                            arg1 = tempMsg.Substring(CmdTpHere.Length + 1);
                        }
                        else if (tempMsg.StartsWith(CmdTpHere2 + " "))
                        {
                            arg1 = tempMsg.Substring(CmdTpHere2.Length + 1);
                        }
                        ExecuteOnPlayer(player, message, arg1);
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                    return true;
                }
                else if (message.StartsWith("/tp"))
                {
                    var tempMsg= message.Trim();
                    if (tempMsg!= "/tp")
                    {
                        var arg1 = tempMsg.Substring(3 + 1);
                        ExecuteOnPlayer(player, message, arg1);
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                    return true;
                }
                // Ban
                if (message.StartsWith("/ban"))
                {
                    var tempMsg= message.Trim();
                    if (tempMsg!= "/ban")
                    {
                        var arg1 = tempMsg.Substring(4 + 1);
                        ExecuteOnPlayer(player, message, arg1);
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                    return true;
                }

                // Kick
                if (message.StartsWith("/kick"))
                {
                    var tempMsg= message.Trim();
                    if (tempMsg!= "/kick")
                    {
                        var arg1 = tempMsg.Substring(5 + 1);
                        ExecuteOnPlayer(player, message, arg1);
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                    return true;
                }
                // Arrest
                if (message.StartsWith("/arrest"))
                {
                    var tempMsg= message.Trim();
                    if (tempMsg!= "/arrest")
                    {
                        var arg1 = tempMsg.Substring(7 + 1);
                        ExecuteOnPlayer(player, message, arg1);
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");

                    return true;
                }
                // Restrain
                if (message.StartsWith("/restrain"))
                {
                    var tempMsg= message.Trim();
                    if (tempMsg!= "/restrain")
                    {
                        var arg1 = tempMsg.Substring(9 + 1);
                        ExecuteOnPlayer(player, message, arg1);

                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");

                    return true;
                }
                // Kill
                if (message.StartsWith("/kill"))
                {
                    var tempMsg= message.Trim();
                    if (tempMsg!= "/kill")
                    {
                        var arg1 = tempMsg.Substring(5 + 1);
                        ExecuteOnPlayer(player, message, arg1);
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");

                    return true;
                }
                // Free
                if (message.StartsWith("/free"))
                {
                    var tempMsg= message.Trim();
                    if (tempMsg!= "/free")
                    {
                        var arg1 = tempMsg.Substring(5 + 1);
                        ExecuteOnPlayer(player, message, arg1);
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");

                    return true;
                }

                // Command: Confirm
                if (message.ToLower().StartsWith("/confirm"))
                {
                    player.Save();
                    if (player.playerData.ownedApartment)
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "Selling apartment...");
                        Confirmed = true;
                        player.SvSellApartment();
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "You don't have a apartment to sell!");
                    }
                    return true;
                }

                // Command: Logs
                if (message.StartsWith("/logs"))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        GetLogs(player, ChatLogFile);
                        return true;
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                        return true;
                    }
                }
                // Command: ATM
                if (message.ToLower().StartsWith(CmdAtm))
                {
                    if (EnableAtmCommand)
                    {
                        player.Save();
                        foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                        {
                            if (shPlayer.svPlayer == player)
                            {
                                if (shPlayer.IsRealPlayer())
                                {
                                    if (shPlayer.wantedLevel == 0)
                                    {
                                        player.SendToSelf(Channel.Unsequenced, (byte)10, "Opening ATM menu..");
                                        player.SendToSelf(Channel.Reliable, (byte)40, player.playerData.bankBalance);
                                        return true;
                                    }
                                    else if (shPlayer.wantedLevel != 0)
                                    {
                                        player.SendToSelf(Channel.Unsequenced, (byte)10, "Criminal Activity: Account Locked");
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, DisabledCommand);
                    }
                    return true;
                }
                // Command: players
                if (message.StartsWith(CmdPlayers) || message.StartsWith(CmdPlayers2))
                {
                    var realPlayers = GameObject.FindObjectsOfType<ShPlayer>().Count(shPlayer => shPlayer.IsRealPlayer());
                    switch (realPlayers) {
                        case 1:
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "There is " + realPlayers + " player online");
                            break;
                        default:
                            if (realPlayers < 1)
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "There are " + realPlayers + " play- wait, how is that possible");
                            else
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "There are " + realPlayers + " player(s) online");
                            break;
                    }
                    return true;
                }

                // Command: ClearChat
                if (message.StartsWith(CmdClearChat) || message.StartsWith(CmdClearChat2))
                {
                    if (message.Contains("all") || message.Contains("everyone"))
                    {
                        if (AdminsListPlayers.Contains(player.playerData.username))
                        {
                            All = true;
                            ClearChat(message, player, All);
                            return true;
                        }
                        else
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                            return true;
                        }

                    }
                    else
                    {
                        All = false;
                        ClearChat(message, player, All);
                        return true;
                    }
                }
                // Command: Reload
                if (message.StartsWith(CmdReload) || message.StartsWith(CmdReload2))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        Reload(false, message, player);
                        return true;
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                        return true;
                    }
                }
                // Command: Mute
                if (message.StartsWith(CmdUnMute))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        Unmute = true;
                        Mute(message, player, Unmute);
                        return true;
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                        return true;
                    }

                }
                else if (message.StartsWith(CmdMute))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        Unmute = false;
                        Mute(message, player, Unmute);
                        return true;
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                        return true;
                    }

                }
                // Command: Say
                if (message.StartsWith(CmdSay) || (message.StartsWith(CmdSay2)))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        Say(message, player);
                        return true;
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                        return true;
                    }

                }
                // Command: GodMode
                if (message.StartsWith(CmdGodmode) || message.StartsWith(CmdGodmode2))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        GodMode(message, player);
                        return true;
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                        return true;
                    }
                }
                // Command: AFK
                if (message.StartsWith(CmdAfk) || message.StartsWith(CmdAfk2))
                {
                    Afk(message, player);
                    return true;
                }
                // Command: Main/essentials
                if (message.StartsWith("/essentials") || message.StartsWith("/ess"))
                {
                    Essentials(message, player);
                    return true;
                }
                // Command: Rules
                if (message.StartsWith(CmdRules))
                {
                    player.SendToSelf(Channel.Reliable, (byte)50, Rules);
                    return true;
                }
                // Command: CheckIP
                if (message.StartsWith(CmdCheckIp))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        var tempMsg= message.Trim();
                        if (tempMsg!= CmdCheckIp)
                        {
                            var arg1 = tempMsg.Substring(CmdCheckIp.Count() + 1);
                            CheckIp(tempMsg, player, arg1);
                            return true;
                        }
                        else
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                            return true;
                        }
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                        return true;
                    }
                }
                // Command: CheckPlayer
                if (message.StartsWith(CmdCheckPlayer))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        var tempMsg= message.Trim();
                        if (tempMsg!= CmdCheckPlayer)
                        {
                            var arg1 = tempMsg.Substring(CmdCheckPlayer.Count() + 1);
                            CheckPlayer(tempMsg, player, arg1);
                            return true;
                        }
                        else
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                            return true;
                        }
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                        return true;
                    }

                }
                // Command: Fake Join/Leave
                if (message.StartsWith(CmdFakeJoin) || (message.StartsWith(CmdFakeLeave)))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        var tempMsg= message.Trim();
                        if (!(tempMsg== CmdFakeJoin || tempMsg== CmdFakeLeave))
                        {
                            string arg1 = null;
                            if (tempMsg.StartsWith(CmdFakeJoin))
                            {
                                arg1 = tempMsg.Substring(CmdFakeJoin.Length + 1);
                                player.SendToAll(Channel.Unsequenced, (byte)10, arg1 + " connected");
                            }
                            else if (tempMsg.StartsWith(CmdFakeLeave))
                            {
                                arg1 = tempMsg.Substring(CmdFakeLeave.Length + 1);
                                player.SendToAll(Channel.Unsequenced, (byte)10, arg1 + " disconnected");
                            }
                        }
                        else
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                        }

                        return true;
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                        return true;
                    }

                }
                // Command: Discord
                if (message.StartsWith(CmdDiscord))
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Discord: " + MsgDiscord);
                    return true;
                }
                // Command: Help
                if (message.StartsWith("/help"))
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Up to date help can be found at http://bit.do/BPEssentials");
                    return true;
                }
                // CustomCommands
                if (Commands.Any(message.Contains))
                {
                    var i = 0;
                    foreach (var command in Commands)
                    {
                        if (message.StartsWith(command))
                        {
                            i = Commands.IndexOf(command);
                        }
                    }
                    player.SendToSelf(Channel.Unsequenced, (byte)10, GetPlaceHolders(i, player));
                    return true;
                }
                if (message.StartsWith(CmdInfo) || message.StartsWith(CmdInfo2))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        try
                        {
                            var arg1 = "null";
                            if (message.StartsWith(CmdInfo))
                            {
                                arg1 = message.Substring(CmdInfo.Length + 1).Trim();
                            }
                            else if (message.StartsWith(CmdInfo2))
                            {
                                arg1 = message.Substring(CmdInfo2.Length + 1).Trim();
                            }

                            if (!(string.IsNullOrEmpty(arg1)))
                            {
                                new Thread(delegate () { GetPlayerInfo(player, arg1); }).Start();
                            }
                            else
                            {
                                player.SendToSelf(Channel.Unsequenced, (byte)10, CmdInfo + " / " + CmdInfo2 +" [Username]");
                            }
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, CmdInfo + " / " + CmdInfo2 + " [Username]");
                        }
                        return true;
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                        return true;
                    }
                }
                if (message.StartsWith(CmdMoney) || message.StartsWith(CmdMoney2))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        string arg1 = null;
                        string arg2 = null;
                        try
                        {
                            if (message.StartsWith(CmdMoney))
                            {
                                arg1 = message.Substring(CmdMoney.Length + 1).Trim();
                            }
                            else if (message.StartsWith(CmdMoney2))
                            {
                                arg1 = message.Substring(CmdMoney2.Length + 1).Trim();
                            }
                            arg2 = message.Split(' ').Last().Trim();
                            arg1 = arg1.Substring(0, arg1.Length - arg2.Length).Trim();
                            if (string.IsNullOrEmpty(arg1))
                            {
                                player.SendToSelf(Channel.Unsequenced, (byte)10, CmdMoney + "/ " + CmdMoney2 + " [Player] [Amount]");
                                return true;
                            }
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, CmdMoney + "/ " + CmdMoney2 + " [Player] [Amount]");
                            return true;
                        }
                        if (!(string.IsNullOrEmpty(arg2)))
                        {
                            int arg2Int;
                            var isNumeric = int.TryParse(arg2, out arg2Int);

                            if (isNumeric)
                            {
                                var found = false;
                                foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                                {
                                    if (shPlayer.svPlayer.playerData.username == arg1 || shPlayer.ID.ToString() == arg1.ToString())
                                    {
                                        if (shPlayer.IsRealPlayer())
                                        {
                                            shPlayer.playerInventory.TransferMoney(1, arg2Int, true);
                                            player.SendToSelf(Channel.Unsequenced, (byte)10, "Succesfully gave " + shPlayer.svPlayer.playerData.username + " " + arg2Int + "$");
                                            shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, (byte)10, player.playerData.username + " gave you " + arg2Int + "$!");
                                            found = true;
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
                                player.SendToSelf(Channel.Unsequenced, (byte)10, CmdMoney + "/ " + CmdMoney2 + " [Player] [Amount] (incorrect argument!)");
                            }
                        }
                        else
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, CmdMoney + "/ " + CmdMoney2 + " [Player] [Amount]");
                        }
                        return true;
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                        return true;
                    }
                }

                if (MsgUnknownCommand)
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Unknown command");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (MutePlayers.Contains(player.playerData.username))
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "You are muted.");
                    return true;
                }
                // Check if message contains a player that is AFK
                if (AfkPlayers.Any(message.Contains))
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "That player is AFK.");
                    return true;
                }
                //Checks if the message is a blocked one, if it is, block it.
                if (!ChatBlock && !LanguageBlock) return false;
                return BlockMessage(message, player);
            }
            return false;

        }
        #endregion
    }
}