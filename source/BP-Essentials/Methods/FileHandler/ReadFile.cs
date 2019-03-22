using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;
using System.IO;
using Newtonsoft.Json;
using BP_Essentials.Chat;

/* The biggest nonsense method is here. */




namespace BP_Essentials
{
    [Serializable]
    public class _General
    {
        public string Version { get; set; }
        public string CommandCharacter { get; set; }
        public bool DownloadIDList { get; set; }
        public string TimestapFormat { get; set; }
        public string MsgSayColor { get; set; }
        public bool DisplayUnknownCommandMessage { get; set; }
        public bool VoteKickDisabled { get; set; }
        public bool ShowDMGMessage { get; set; }
        public int DebugLevel { get; set; }
        public string DiscordWebhook_Ban { get; set; }
        public bool EnableDiscordWebhook_Ban { get; set; }
        public string DiscordWebhook_Report { get; set; }
        public bool EnableDiscordWebhook_Report { get; set; }
        public bool BlockBanButtonTabMenu { get; set; }
        public bool BlockLicenseRemoved { get; set; }
        public int MessagesAllowedPerSecond { get; set; }
		public int TimeBetweenDelay { get; set; }
		public string WipePassword { get; set; }
    }
	[Serializable]
	public class _Messages
	{
		public string NoPerm { get; set; }
		public string DisabledCommand { get; set; }
		public string MsgSayPrefix { get; set; }
		public string DiscordLink { get; set; }
		public string PlayerIsAFK { get; set; }
		public string SelfIsMuted { get; set; }
		public string ArgRequired { get; set; }
		public string NotFoundOnline { get; set; }
		public string NotFoundOnlineIdOnly { get; set; }
		public string AdminSearchingInv { get; set; }
		public string PlayerMessage { get; set; }
		public string AdminMessage { get; set; }
		public string AdminChatMessage { get; set; }
		public string MsgNoPermJob { get; set; }
		public string BlockedItem { get; set; }
		public string MsgNoWantedAllowed { get; set; }
		public string MsgNoCuffedAllowed { get; set; }
		public string MsgNoJailAllowed { get; set; }
		public string MeMessage { get; set; }
    }
    [Serializable]
    public class MessageColors
    {
        public string Info { get; set; }
        public string Error { get; set; }
        public string Warning { get; set; }
        public string Arg { get; set; }
    }
    [Serializable]
    public class FunctionUI
    {
        public string AccessMoneyMenu { get; set; }
        public string AccessItemMenu { get; set; }
        public string AccessCWMenu { get; set; }
        public string AccessSetHPMenu { get; set; }
        public string AccessSetStatsMenu { get; set; }
    }
    [Serializable]
    public class ReportOptions
    {
        public string F2 { get; set; }
        public string F3 { get; set; }
        public string F4 { get; set; }
        public string F5 { get; set; }
        public string F6 { get; set; }
        public string F7 { get; set; }
        public string F8 { get; set; }
        public string F9 { get; set; }
        public string F10 { get; set; }
    }
    [Serializable]
    public class JobIndexArray
    {
        public string Citizen { get; set; }
        public string Criminal { get; set; }
        public string Prisoner { get; set; }
        public string Police { get; set; }
        public string Paramedic { get; set; }
        public string Firefighter { get; set; }
        public string Rojo_Loco { get; set; }
        public string Green_St_Fam { get; set; }
        public string Borgata_Blue { get; set; }
        public string Mayor { get; set; }
        public string DeliveryDriver { get; set; }
        public string TaxiDriver { get; set; }
        public string SpecOps { get; set; }
    }
    [Serializable]
    public class _Misc
    {
        public bool EnableChatBlock { get; set; }
        public bool EnableLanguageBlock { get; set; }
        public bool CheckBannedEnabled { get; set; }
        public int TimeBetweenAnnounce { get; set; }
        public string BlockSpawnBot { get; set; }
        public bool EnableBlockSpawnBot { get; set; }
        public int GodModeLevel { get; set; }
        public bool ShowJailMessage { get; set; }
        public bool BlockSuicide { get; set; }
        public bool BlockMissions { get; set; }
        public bool ProximityChat { get; set; }
        public bool LocalChatMute { get; set; }
		public bool LocalChatMe { get; set; }
		public bool TimescaleDisabled { get; set; }
	}
    [Serializable]
    public class WhitelistedJob
    {
        public int JobIndex { get; set; }
        public string Whitelisted { get; set; }
    }
    [Serializable]
    public class _Command
    {
        public string CommandName { get; set; }
        public List<string> Commands { get; set; }
        public string ExecutableBy { get; set; }
        public bool? Disabled { get; set; }
        public bool? AllowWithCrimes { get; set; }
        public bool? AllowWhileCuffed { get; set; }
		public bool? AllowWhileJailed { get; set; }
	}
    [Serializable]
    public class __RootObject
    {
        public _General General { get; set; }
        public _Messages Messages { get; set; }
        public MessageColors MessageColors { get; set; }
        public FunctionUI FunctionUI { get; set; }
        public ReportOptions ReportOptions { get; set; }
        public JobIndexArray JobIndexArray { get; set; }
        public _Misc Misc { get; set; }
        public List<int> BlockedItems { get; set; }
        public List<WhitelistedJob> WhitelistedJobs { get; set; }
        public List<_Command> Commands { get; set; }
    }
    [Serializable]
    public class Item
    {
        public string name { get; set; }
        public int id { get; set; }
        public int gameid { get; set; }
    }
    [Serializable]
    public class IdListObject
    {
        public List<Item> items { get; set; }
    }
    class ReadFile
    {

        public static void Run(string fileName)
        {
            try
            {
                IdListObject idlist;
				switch (fileName)
				{
					case SettingsFile:
						var m = JsonConvert.DeserializeObject<__RootObject>(FilterComments.Run(SettingsFile));


						LocalVersion = m.General.Version;
						CmdCommandCharacter = m.General.CommandCharacter;
						DownloadIdList = m.General.DownloadIDList;
						TimestampFormat = m.General.TimestapFormat;
						MsgSayColor = m.General.MsgSayColor;
						MsgUnknownCommand = m.General.DisplayUnknownCommandMessage;
						VoteKickDisabled = m.General.VoteKickDisabled;
						ShowDMGMessage = m.General.ShowDMGMessage;
						DebugLevel = m.General.DebugLevel;
						EnableDiscordWebhook_Ban = m.General.EnableDiscordWebhook_Ban;
						if (EnableDiscordWebhook_Ban && string.IsNullOrEmpty(m.General.DiscordWebhook_Ban.Trim()))
						{
							Debug.Log("[ERROR] Discord webhook_Ban is empty but EnableDiscordWebhook_Ban is true! Disabling webhook_Ban.");
							EnableDiscordWebhook_Ban = false;
						}
						else
							DiscordWebhook_Ban = m.General.DiscordWebhook_Ban;
						EnableDiscordWebhook_Report = m.General.EnableDiscordWebhook_Report;
						if (EnableDiscordWebhook_Report && string.IsNullOrEmpty(m.General.DiscordWebhook_Report.Trim()))
						{
							Debug.Log("[ERROR] Discord webhook_Report is empty but EnableDiscordWebhook_Report is true! Disabling webhook_Report.");
							EnableDiscordWebhook_Report = false;
						}
						else
							DiscordWebhook_Report = m.General.DiscordWebhook_Report;
						BlockBanButtonTabMenu = m.General.BlockBanButtonTabMenu;
						blockLicenseRemoved = m.General.BlockLicenseRemoved;
						MessagesAllowedPerSecond = m.General.MessagesAllowedPerSecond;
						TimeBetweenDelay = m.General.TimeBetweenDelay;
						WipePassword = m.General.WipePassword;

						infoColor = m.MessageColors.Info;
						errorColor = m.MessageColors.Error;
						warningColor = m.MessageColors.Warning;
						argColor = m.MessageColors.Arg;

						MsgNoPerm = m.Messages.NoPerm;
						MsgDiscord = m.Messages.DiscordLink;
						MsgSayPrefix = m.Messages.MsgSayPrefix;
						DisabledCommand = $"<color={errorColor}>{m.Messages.DisabledCommand}</color>";
						PlayerIsAFK = $"<color={warningColor}>{m.Messages.PlayerIsAFK}</color>";
						SelfIsMuted = $"<color={errorColor}>{m.Messages.SelfIsMuted}</color>";
						ArgRequired = $"<color={errorColor}>{m.Messages.ArgRequired}</color>";
						NotFoundOnline = $"<color={errorColor}>{m.Messages.NotFoundOnline}</color>";
						NotFoundOnlineIdOnly = $"<color={errorColor}>{m.Messages.NotFoundOnlineIdOnly}</color>";
						AdminSearchingInv = $"<color={errorColor}>{m.Messages.AdminSearchingInv}</color>";
						PlayerMessage = m.Messages.PlayerMessage;
						AdminMessage = m.Messages.AdminMessage;
						AdminChatMessage = m.Messages.AdminChatMessage;
						MsgNoPermJob = $"<color={errorColor}>{m.Messages.MsgNoPermJob}</color>";
						BlockedItemMessage = $"<color={errorColor}>{m.Messages.BlockedItem}</color>";
						MsgNoWantedAllowed = $"<color={errorColor}>{m.Messages.MsgNoWantedAllowed}</color>";
						MsgNoCuffedAllowed = $"<color={errorColor}>{m.Messages.MsgNoCuffedAllowed}</color>";
						MsgNoJailAllowed = $"<color={errorColor}>{m.Messages.MsgNoJailAllowed}</color>";
						MeMessage = m.Messages.MeMessage;

						AccessMoneyMenu = m.FunctionUI.AccessMoneyMenu;
						AccessItemMenu = m.FunctionUI.AccessItemMenu;
						AccessSetHPMenu = m.FunctionUI.AccessSetHPMenu;
						AccessSetStatsMenu = m.FunctionUI.AccessSetStatsMenu;
						AccessCWMenu = m.FunctionUI.AccessCWMenu;

						ReportReasons = new string[] { m.ReportOptions.F2, m.ReportOptions.F3, m.ReportOptions.F4, m.ReportOptions.F5, m.ReportOptions.F6, m.ReportOptions.F7, m.ReportOptions.F8, m.ReportOptions.F9, m.ReportOptions.F10 };

						// Softcode this someday
						Jobs = new string[] { m.JobIndexArray.Citizen, m.JobIndexArray.Criminal, m.JobIndexArray.Prisoner, m.JobIndexArray.Police, m.JobIndexArray.Paramedic, m.JobIndexArray.Firefighter, m.JobIndexArray.Rojo_Loco, m.JobIndexArray.Green_St_Fam, m.JobIndexArray.Borgata_Blue, m.JobIndexArray.Mayor, m.JobIndexArray.DeliveryDriver, m.JobIndexArray.TaxiDriver, m.JobIndexArray.SpecOps };

						BlockedItems = m.BlockedItems;

						EnableBlockSpawnBot = m.Misc.EnableBlockSpawnBot;
						LanguageBlock = m.Misc.EnableLanguageBlock;
						ChatBlock = m.Misc.EnableChatBlock;
						CheckBannedEnabled = m.Misc.CheckBannedEnabled;
						TimeBetweenAnnounce = m.Misc.TimeBetweenAnnounce;
						Variables.Announcer.Interval = TimeBetweenAnnounce;
						TimescaleDisabled = m.Misc.TimescaleDisabled;

						if (m.Misc.EnableBlockSpawnBot)
							BlockedSpawnIds = m.Misc.BlockSpawnBot.Split(',').Select(int.Parse).ToArray();
						GodModeLevel = m.Misc.GodModeLevel;
						ShowJailMessage = m.Misc.ShowJailMessage;
						BlockSuicide = m.Misc.BlockSuicide;
						BlockMissions = m.Misc.BlockMissions;
						ProximityChat = m.Misc.ProximityChat;
						LocalChatMute = m.Misc.LocalChatMute;
						LocalChatMe = m.Misc.LocalChatMe;

						foreach (var currJob in m.WhitelistedJobs)
						{
							if (WhitelistedJobs.ContainsKey(currJob.JobIndex))
							{
								Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [WARNING] WhitelistedJobs already contains a item with the key '{currJob.JobIndex}'! (Did you make two objects with the same JobIndex?)");
								continue;
							}
							WhitelistedJobs.Add(currJob.JobIndex, currJob.Whitelisted);
						}
						RegisterCommands.Run(m.Commands);
						break;
					case IdListItemsFile:
						idlist = JsonConvert.DeserializeObject<IdListObject>(FilterComments.Run(IdListItemsFile));
						IDs_Items = idlist.items.Select(x => x.gameid).ToArray();
						break;
					case IdListVehicleFile:
						idlist = JsonConvert.DeserializeObject<IdListObject>(FilterComments.Run(IdListVehicleFile));
						IDs_Vehicles = idlist.items.Select(x => x.gameid).ToArray();
						break;
					case AnnouncementsFile:
						Variables.Announcer.Announcements = File.ReadAllLines(fileName).ToList();
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
					default:
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
