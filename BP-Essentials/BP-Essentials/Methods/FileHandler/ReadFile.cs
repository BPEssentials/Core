using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;

/* The biggest nonsense method is here. */




namespace BP_Essentials
{
    class ReadFile : EssentialsChatPlugin
    {
        public static void Run(string fileName)
        {

            switch (fileName)
            {
                case SettingsFile:
                    foreach (var line in File.ReadAllLines(SettingsFile)) //TODO: THIS METHOD IS A MESS, THERE IS PROBABLY A BETTER WAY.
                    {
                        if (!line.StartsWith("#"))
                        {
                            if (line.Contains("version: "))
                            {
                                LocalVersion = line.Substring(9);
                            }
                            else if (line.Contains("CommandCharacter: "))
                            {
                                CmdCommandCharacter = line.Substring(18);
                            }
                            else if (line.Contains("noperm: "))
                            {
                                MsgNoPerm = line.Substring(8);
                            }
                            else if (line.Contains("ClearChatCommand: "))
                            {
                                CmdClearChat = CmdCommandCharacter + line.Substring(18);
                            }
                            else if (line.Contains("ClearChatCommand2: "))
                            {
                                CmdClearChat2 = CmdCommandCharacter + line.Substring(19);
                            }
                            else if (line.Contains("SayCommand: "))
                            {
                                CmdSay = CmdCommandCharacter + line.Substring(12);
                            }
                            else if (line.Contains("SayCommand2:"))
                            {
                                CmdSay2 = CmdCommandCharacter + line.Substring(13);
                            }
                            else if (line.Contains("msgSayPrefix: "))
                            {
                                MsgSayPrefix = line.Substring(14);
                            }
                            else if (line.StartsWith("GodmodeCommand: "))
                            {
                                CmdGodmode = CmdCommandCharacter + line.Substring(16);
                            }
                            else if (line.StartsWith("GodmodeCommand2: "))
                            {
                                CmdGodmode2 = CmdCommandCharacter + line.Substring(17);
                            }
                            else if (line.StartsWith("MuteCommand: "))
                            {
                                CmdMute = CmdCommandCharacter + line.Substring(13);
                            }
                            else if (line.StartsWith("UnMuteCommand: "))
                            {
                                CmdUnMute = CmdCommandCharacter + line.Substring(15);
                            }
                            else if (line.Contains("UnknownCommand: "))
                            {
                                MsgUnknownCommand = Convert.ToBoolean(line.Substring(16));
                            }
                            else if (line.Contains("enableChatBlock: "))
                            {
                                ChatBlock = Convert.ToBoolean(line.Substring(17));
                            }
                            else if (line.Contains("enableLanguageBlock: "))
                            {
                                LanguageBlock = Convert.ToBoolean(line.Substring(21));
                            }
                            else if (line.Contains("RulesCommand: "))
                            {
                                CmdRules = CmdCommandCharacter + line.Substring(14);
                            }
                            else if (line.Contains("ReloadCommand: "))
                            {
                                CmdReload = CmdCommandCharacter + line.Substring(15);
                            }
                            else if (line.Contains("ReloadCommand2: "))
                            {
                                CmdReload2 = CmdCommandCharacter + line.Substring(16);
                            }
                            else if (line.Contains("TimeBetweenAnnounce: "))
                            {
                                TimeBetweenAnnounce = Int32.Parse(line.Substring(21));
                            }
                            else if (line.Contains("TimestapFormat: "))
                            {
                                TimestampFormat = line.Substring(16);
                            }
                            else if (line.Contains("AFKCommand: "))
                            {
                                CmdAfk = CmdCommandCharacter + line.Substring(12);
                            }
                            else if (line.Contains("AFKCommand2: "))
                            {
                                CmdAfk2 = CmdCommandCharacter + line.Substring(13);
                            }
                            else if (line.Contains("CheckIPCommand: "))
                            {
                                CmdCheckIp = CmdCommandCharacter + line.Substring(16);
                            }
                            else if (line.Contains("CheckPlayerCommand: "))
                            {
                                CmdCheckPlayer = CmdCommandCharacter + line.Substring(20);
                            }
                            else if (line.Contains("FakeJoinCommand: "))
                            {
                                CmdFakeJoin = CmdCommandCharacter + line.Substring(17);
                            }
                            else if (line.Contains("FakeLeaveCommand: "))
                            {
                                CmdFakeLeave = CmdCommandCharacter + line.Substring(18);
                            }
                            else if (line.Contains("CheckForAlts: "))
                            {
                                CheckAlt = Convert.ToBoolean(line.Substring(14));
                            }
                            else if (line.Contains("DiscordCommand: "))
                            {
                                CmdDiscord = CmdCommandCharacter + line.Substring(16);
                            }
                            else if (line.Contains("PlayersOnlineCommand: "))
                            {
                                CmdPlayers = CmdCommandCharacter + line.Substring(22);
                            }
                            else if (line.Contains("PlayersOnlineCommand2: "))
                            {
                                CmdPlayers2 = CmdCommandCharacter + line.Substring(23);
                            }
                            else if (line.Contains("InfoPlayerCommand: "))
                            {
                                CmdInfo = CmdCommandCharacter + line.Substring(19);
                            }
                            else if (line.Contains("InfoPlayerCommand2: "))
                            {
                                CmdInfo2 = CmdCommandCharacter + line.Substring(20);
                            }
                            else if (line.Contains("MoneyCommand: "))
                            {
                                CmdMoney = CmdCommandCharacter + line.Substring(14);
                            }
                            else if (line.Contains("MoneyCommand2: "))
                            {
                                CmdMoney2 = CmdCommandCharacter + line.Substring(15);
                            }
                            else if (line.Contains("DiscordLink: "))
                            {
                                MsgDiscord = line.Substring(13);
                            }
                            else if (line.StartsWith("ATMCommand: "))
                            {
                                CmdAtm = CmdCommandCharacter + line.Substring(12);
                            }
                            else if (line.StartsWith("EnableATMCommand: "))
                            {
                                EnableAtmCommand = Convert.ToBoolean(line.Substring(18));
                            }
                            else if (line.StartsWith("EnableBlockSpawnBot: "))
                            {
                                EnableBlockSpawnBot = Convert.ToBoolean(line.Substring(21));
                            }
                            else if (line.StartsWith("BlockSpawnBot: "))
                            {
                                if (EnableBlockSpawnBot == null)
                                {
                                    foreach (var line2 in File.ReadAllLines(SettingsFile)) //TODO: Bad way of doing it, but it works i guess
                                    {
                                        if (line2.StartsWith("#"))
                                        { }
                                        else
                                        {
                                            if (line2.StartsWith("EnableBlockSpawnBot: "))
                                            {
                                                EnableBlockSpawnBot = Convert.ToBoolean(line2.Substring(21));
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (EnableBlockSpawnBot == true)
                                {
                                    DisabledSpawnBots = line.Substring(15);
                                    DisabledSpawnBots = DisabledSpawnBots.Replace(" ", String.Empty);
                                    if (DisabledSpawnBots.EndsWith(","))
                                    {
                                        EnableBlockSpawnBot = false;
                                        Debug.Log("[ERROR] BlockSpawnBot Cannot end with a comma!");
                                    }
                                    else
                                    {
                                        try
                                        {
                                            BlockedSpawnIds = DisabledSpawnBots.Split(',').Select(int.Parse).ToArray();
                                        }
                                        catch (Exception ex)
                                        {
                                            EnableBlockSpawnBot = false;
                                            ErrorLogging.Run(ex);
                                        }
                                    }
                                }
                            }
                            else if (line.Contains("HelpCommand: "))
                            {
                                CmdHelp = line.Substring(13);
                            }
                            else if (line.StartsWith("SaveCommand: "))
                            {
                                CmdSave = CmdCommandCharacter + line.Substring(13);
                            }
                            else if (line.StartsWith("FreeCommand: "))
                            {
                                CmdFree = CmdCommandCharacter + line.Substring(13);
                            }
                            else if (line.StartsWith("KillCommand: "))
                            {
                                CmdKill = CmdCommandCharacter + line.Substring(13);
                            }
                            else if (line.StartsWith("KickCommand: "))
                            {
                                CmdKick = CmdCommandCharacter + line.Substring(13);
                            }
                            else if (line.StartsWith("LogsCommand: "))
                            {
                                CmdLogs = CmdCommandCharacter + line.Substring(13);
                            }
                            else if (line.StartsWith("TpCommand: "))
                            {
                                CmdTp = CmdCommandCharacter + line.Substring(11);
                            }
                            else if (line.StartsWith("TpHereCommand: "))
                            {
                                CmdTpHere = CmdCommandCharacter + line.Substring(15);
                            }
                            else if (line.StartsWith("TpHereCommand2: "))
                            {
                                CmdTpHere2 = CmdCommandCharacter + line.Substring(16);
                            }
                            else if (line.StartsWith("PayCommand: "))
                            {
                                CmdPay = CmdCommandCharacter + line.Substring(12);
                            }
                            else if (line.StartsWith("PayCommand2: "))
                            {
                                CmdPay2 = CmdCommandCharacter + line.Substring(13);
                            }
                            else if (line.StartsWith("BanCommand: "))
                            {
                                CmdBan = CmdCommandCharacter + line.Substring(12);
                            }
                            else if (line.StartsWith("ConfirmCommand: "))
                            {
                                CmdConfirm = CmdCommandCharacter + line.Substring(16);
                            }
                            else if (line.StartsWith("ArrestCommand: "))
                            {
                                CmdArrest = CmdCommandCharacter + line.Substring(15);
                            }
                            else if (line.StartsWith("RestrainCommand: "))
                            {
                                CmdRestrain = CmdCommandCharacter + line.Substring(17);
                            }
                        }

                    }

                    break;
                case AnnouncementsFile:
                    Announcements = File.ReadAllLines(fileName);
                    break;
                case RulesFile:
                    Rules = File.ReadAllText(fileName);
                    break;
                case GodListFile:
                    GodListPlayers = File.ReadAllLines(fileName).ToList();
                    break;
                case AfkListFile:
                    AfkPlayers = File.ReadAllLines(fileName).ToList();
                    break;
                case MuteListFile:
                    MutePlayers = File.ReadAllLines(fileName).ToList();
                    break;
            }
        }
    }
}
