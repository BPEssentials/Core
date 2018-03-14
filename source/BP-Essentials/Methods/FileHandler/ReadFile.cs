﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;
using Newtonsoft.Json;

/* The biggest nonsense method is here. */




namespace BP_Essentials
{
    [Serializable]
    public class _General
    {
        public string version { get; set; }
        public string CommandCharacter { get; set; }
        public string TimestapFormat { get; set; }
        public string MsgSayColor { get; set; }
        public bool DisplayUnknownCommandMessage { get; set; }
        public bool VoteKickDisabled { get; set; }
        public bool ShowDMGMessage { get; set; }
    }
    [Serializable]
    public class _Messages
    {
        public string noperm { get; set; }
        public string DisabledCommand { get; set; }
        public string msgSayPrefix { get; set; }
        public string DiscordLink { get; set; }
        public string PlayerIsAFK { get; set; }
        public string SelfIsMuted { get; set; }
        public string ArgRequired { get; set; }
        public string NotFoundOnline { get; set; }
        public string AdminSearchingInv { get; set; }
    }
    [Serializable]
    public class MessageColors
    {
        public string info { get; set; }
        public string error { get; set; }
        public string warning { get; set; }
        public string arg { get; set; }
    }

    [Serializable]
    public class _Misc
    {
        public bool enableChatBlock { get; set; }
        public bool enableLanguageBlock { get; set; }
        public bool CheckForAlts { get; set; }
        public int TimeBetweenAnnounce { get; set; }
        public string BlockSpawnBot { get; set; }
        public bool EnableBlockSpawnBot { get; set; }
    }
    [Serializable]
    public class _Command
    {
        public string CommandName { get; set; }
        public string Command { get; set; }
        public string Command2 { get; set; }
        public string ExecutableBy { get; set; }
        public bool? Disabled { get; set; }
        public string c { get; set; }
    }
    [Serializable]
    public class RootObject
    {
        public _General General { get; set; }
        public _Messages Messages { get; set; }
        public MessageColors MessageColors { get; set; }
        public _Misc Misc { get; set; }
        public List<_Command> Commands { get; set; }
    }
    class ReadFile : EssentialsChatPlugin
    {

        public static void Run(string fileName)
        {
            try
            {
                switch (fileName)
                {
                    case SettingsFile:
                        RootObject m = JsonConvert.DeserializeObject<RootObject>(FilterComments.Run(SettingsFile));
                        LocalVersion = m.General.version;
                        CmdCommandCharacter = m.General.CommandCharacter;
                        TimestampFormat = m.General.TimestapFormat;
                        MsgSayColor = m.General.MsgSayColor;
                        MsgUnknownCommand = m.General.DisplayUnknownCommandMessage;
                        VoteKickDisabled = m.General.VoteKickDisabled;
                        ShowDMGMessage = m.General.ShowDMGMessage;

                        infoColor = m.MessageColors.info;
                        errorColor = m.MessageColors.error;
                        warningColor = m.MessageColors.warning;
                        argColor = m.MessageColors.arg;

                        MsgNoPerm = m.Messages.noperm;
                        MsgDiscord = m.Messages.DiscordLink;
                        MsgSayPrefix = m.Messages.msgSayPrefix;
                        DisabledCommand = $"<color={errorColor}>{m.Messages.DisabledCommand}</color>";
                        PlayerIsAFK = $"<color={warningColor}>{m.Messages.PlayerIsAFK}</color>";
                        SelfIsMuted = $"<color={errorColor}>{m.Messages.SelfIsMuted}</color>";
                        ArgRequired = $"<color={errorColor}>{m.Messages.ArgRequired}</color>";
                        NotFoundOnline = $"<color={errorColor}>{m.Messages.NotFoundOnline}</color>";
                        AdminSearchingInv = $"<color={errorColor}>{m.Messages.AdminSearchingInv}</color>";

                        EnableBlockSpawnBot = m.Misc.EnableBlockSpawnBot;
                        LanguageBlock = m.Misc.enableLanguageBlock;
                        ChatBlock = m.Misc.enableChatBlock;
                        CheckAlt = m.Misc.CheckForAlts;
                        TimeBetweenAnnounce = m.Misc.TimeBetweenAnnounce;
                        BlockedSpawnIds = m.Misc.BlockSpawnBot.Split(',').Select(int.Parse).ToArray();
                        foreach (var command in m.Commands)
                            StringToVar.Run(command.CommandName, command.Command, command.Command2, command.ExecutableBy, command.Disabled);
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
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
