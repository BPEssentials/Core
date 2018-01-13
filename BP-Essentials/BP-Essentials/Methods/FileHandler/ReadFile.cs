using System;
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
                        RootObject m = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(SettingsFile));
                        LocalVersion = m.General.version;
                        CmdCommandCharacter = m.General.CommandCharacter;
                        TimestampFormat = m.General.TimestapFormat;
                        MsgUnknownCommand = m.General.DisplayUnknownCommandMessage;
                        VoteKickDisabled = m.General.VoteKickDisabled;
                        ShowDMGMessage = m.General.ShowDMGMessage;

                        MsgNoPerm = m.Messages.noperm;
                        MsgDiscord = m.Messages.DiscordLink;
                        MsgSayPrefix = m.Messages.msgSayPrefix;
                        DisabledCommand = m.Messages.DisabledCommand;
                        PlayerIsAFK = m.Messages.PlayerIsAFK;
                        SelfIsMuted = m.Messages.SelfIsMuted;
                        ArgRequired = m.Messages.ArgRequired;

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
