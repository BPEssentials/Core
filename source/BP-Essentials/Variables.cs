using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace BP_Essentials
{
    public class Variables : Core
    {
		public static string Version { get; private set; } = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
		public static bool IsPreRelease => Version.Contains("pre");

        // Generic Constants
        public const string FileDirectory = "Essentials/";

        public static string LogDirectory = Path.Combine(FileDirectory, "logs/");
        public static string KitDirectory = Path.Combine(FileDirectory, "kits/");
        public static string WarpDirectory = Path.Combine(FileDirectory, "warps/");

        public const string SettingsFile = FileDirectory + "settings.json";
        public const string LanguageBlockFile = FileDirectory + "languageblock.txt";
        public const string ChatBlockFile = FileDirectory + "chatblock.txt";
        public const string AnnouncementsFile = FileDirectory + "announcements.txt";
        public const string IpListFile = FileDirectory + "ip_list.txt";
        public const string GodListFile = FileDirectory + "godlist.txt";
        public const string AfkListFile = FileDirectory + "afklist.txt";
        public const string MuteListFile = FileDirectory + "mutelist.txt";
        public const string ExceptionFile = FileDirectory + "exceptions.txt";
        public const string CustomCommandsFile = FileDirectory + "CustomCommands.json";
        public const string CustomGroupsFile = FileDirectory + "CustomGroups.json";
        public const string IdListItemsFile = FileDirectory + "ID_list-Items.txt";
        public const string IdListVehicleFile = FileDirectory + "ID_list-Vehicles.txt";

        public const string AutoReloader = FileDirectory + "autoReloader.txt";

        public const string AdminListFile = "admin_list.txt";
        public const string RulesFile = "server_info.txt";
        public const string BansFile = "ban_list.txt";
        public static readonly string LogFile = Path.Combine(LogDirectory, "all.txt");
        public static readonly string ChatLogFile = Path.Combine(LogDirectory, "chat.txt");
        public static readonly string CommandLogFile = Path.Combine(LogDirectory, "commands.txt");

		// Singletons
		public static WarpHandler WarpHandler { get; set; }
		public static KitsHandler KitsHandler { get; set; }
		public static Announcer Announcer { get; set; } = new Announcer();

		// Bools
		public static bool MsgUnknownCommand;

        public static bool ChatBlock;
        public static bool LanguageBlock;
        public static bool All;
        public static bool Unmute;
        public static bool MessageToLower;
        public static bool EnableAtmCommand;
        public static bool Confirmed;
        public static bool? EnableBlockSpawnBot;
        public static bool ShowDMGMessage;
        public static bool VoteKickDisabled;
        public static bool DownloadIdList;
        public static bool EnableDiscordWebhook_Ban;
        public static bool EnableDiscordWebhook_Report;
        public static bool BlockBanButtonTabMenu;
        public static bool CheckBannedEnabled;
        public static bool blockLicenseRemoved;
        public static bool ShowJailMessage;
        public static bool BlockSuicide;
        public static bool BlockMissions;
        public static bool ProximityChat;
        public static bool LocalChatMute;
		public static bool LocalChatMe;
		public static bool TimescaleDisabled;

		// Lists
		public static List<CustomCommand> CustomCommands = new List<CustomCommand>();

        public static List<string> Responses = new List<string>();
        public static List<string> ChatBlockWords = new List<string>();
        public static List<string> LanguageBlockWords = new List<string>();
        public static List<string> AdminsListPlayers = new List<string>();
        public static List<string> GodListPlayers = new List<string>();
        public static List<string> AfkPlayers = new List<string>();
        public static List<string> MutePlayers = new List<string>();
        public static List<string> LatestVotePeople = new List<string>();

        public static List<int> BlockedItems = new List<int>();

		// Dictionary
		public static Dictionary<int, PlayerListItem> PlayerList { get; set; } = new Dictionary<int, PlayerListItem>();
		public static Dictionary<string, _Group> Groups { get; set; } = new Dictionary<string, _Group>();
		public static Dictionary<int, _CommandList> CommandList { get; set; } = new Dictionary<int, _CommandList>();
		public static Dictionary<int, string> WhitelistedJobs { get; set; } = new Dictionary<int, string>();

		// MultiDictonary
		public static Methods.Utils.MultiDictionary<byte, CurrentMenu, Func<SvPlayer, CurrentMenu>> FunctionMenuKeys { get; set; } = new Methods.Utils.MultiDictionary<byte, CurrentMenu, Func<SvPlayer, CurrentMenu>>();

		// Arrays
        public static string[] Jobs = {
            // default values
            "Citizen",
            "Criminal",
            "Prisoner",
            "Police",
            "Paramedic",
            "Firefighter",
            "Gangster: Red",
            "Gangster: Green",
            "Gangster: Blue",
            "Mayor",
            "DeliveryDriver",
            "TaxiDriver",
            "Special Forces"
        };

        // Messages
        public static string MsgSayPrefix;

        public static string MsgNoPerm;
        public static string MsgNoPermJob;
        public static string MsgDiscord;
        public static string DisabledCommand;
        public static string ArgRequired;
        public static string PlayerIsAFK;
        public static string SelfIsMuted;
        public static string NotFoundOnline;
        public static string NotFoundOnlineIdOnly;
        public static string AdminSearchingInv;
        public static string PlayerMessage;
        public static string AdminMessage;
        public static string AdminChatMessage;
        public static string BlockedItemMessage;
        public static string MsgNoWantedAllowed;
        public static string MsgNoCuffedAllowed;
		public static string MsgNoJailAllowed;
		public static string MeMessage;

		public static string infoColor, errorColor, warningColor, argColor;

        // Strings
        public static string Rules;

        public static string DisabledSpawnBots;
        public static string LocalVersion;
        public static string MsgSayColor;
        public static string AccessMoneyMenu;
        public static string AccessItemMenu;
        public static string AccessCWMenu;
        public static string AccessSetHPMenu;
        public static string AccessSetStatsMenu;
        public static string CmdCommandCharacter;

        // Commands, still a few needed for easy access
        public static string CmdStaffChatExecutableBy;
        public static string CmdConfirm;
        public static string CmdToggleChat;
        public static string CmdTpaaccept;


        public static string DiscordWebhook_Ban;
        public static string DiscordWebhook_Report;
        public static string WipePassword;

		public static string NotValidArg = $"<color={errorColor}>Error: Is that a valid number you provided as argument?</color>";
		// Ints
		public const int SaveTime = 5 * 60;

        public static int AnnounceIndex;
        public static int TimeBetweenAnnounce;
        public static int[] BlockedSpawnIds;
        public static int DebugLevel;
        public static int GodModeLevel;
        public static int MessagesAllowedPerSecond;
		public static int TimeBetweenDelay;

        // Misc.
        public static string _msg;

        public static string username;
        public static string TimestampFormat;
        public const string CensoredText = "******";
        public const string PatternTemplate = @"\b({0})(s?)\b";

        public static SvManager SvMan;

        [Obsolete("Maps can be custom now, so should be removed in future update or manually added by server owners.")]
        public static Dictionary<string[], Vector3> PlaceDictionary = new Dictionary<string[], Vector3>
        {
            { new[] { "1", "PoliceStation", "Police Station" }, new Vector3(-17.0F, 0.0F, 46.0F) },
            { new[] { "2", "FireStation", "Fire Station" }, new Vector3(173.0F, 0.0F,  237.0F)},
            { new[] { "3", "Hospital", "Ambulance Station" }, new Vector3(111.0F, 0.0F, 148.0F)},
            { new[] { "4", "GunShop", "Gun Shop", "ammunition" }, new Vector3(55.0F, 0.0F, 112.0F) },
            { new[] { "5", "ElectronicsShop", "Electronics Shop" }, new Vector3(567.0F, 0.0F, -92.0F) },
            { new[] { "6", "PawnShop", "Pawn Shop" }, new Vector3(448.0F, 0.0F, -203.0F) },
            { new[] { "7", "FastFoodShop", "Fast Food Shop" }, new Vector3(645.0F, 0.0F, -168.0F) },
            { new[] { "8", "CoffeeShop", "Coffee Shop" }, new Vector3(618.0F, 0.0F, 148.0F) },
            { new[] { "9", "ClothingShop", "Clothing Shop" }, new Vector3(595.0F, 0.0F, 81.0F) },
            { new[] { "10", "GreenGang", "Green St. Fam Boss" }, new Vector3(32.0F, 0.0F, -202.0F) },
            { new[] { "11", "BlueGang", "Borgata Blue Boss" }, new Vector3(504.0F, 0.0F, 23.0F) },
            { new[] { "12", "RedGang", "Rojo Loco Boss" }, new Vector3(521.0F, 0.0F, 199.0F) },
            { new[] { "13", "10k", "Large Apartment" }, new Vector3(2.0F, 0.0F, -92.0F) },
            { new[] { "14", "5k", "Medium Apartment" }, new Vector3(519.0F, 0.0F, -125.0F) },
            { new[] { "15", "1.2k", "Small Apartment" }, new Vector3(643.0F, 0.0F, -61.0F) },
            { new[] { "16", "DeliveryJob", "Delivery Job" }, new Vector3(64.0F, 0.0F, -92.0F) },
            { new[] { "17", "TaxiJob", "Taxi Job" }, new Vector3(-232.0F, 0.0F, 87.0F) },
            { new[] { "18", "TownHall", "Town Hall", "Mayor", "MayorOffice" }, new Vector3(127.0F, 0.0F, -56.0F) },
            { new[] { "19", "Bank" }, new Vector3(447.0F, 0.0F, -23.0F) },
            { new[] { "20", "DrugDealer", "Drug Dealer" }, new Vector3(288.0F, 0.0F, -236.0F) },
            { new[] { "21", "SpecOps", "Spec Ops Job", "MilitaryBase", "Military Base" }, new Vector3(654.0F, 0.0F, 278.0F) },
            { new[] { "22", "Bomb", "BombLocation", "Bomb Location" }, new Vector3(1010.0F, 0.0F, 401.0F) }
        };

        public static string[] ReportReasons =
        {
            // default values
            "Random Vote Kick",
            "Committing suicide/Disconnecting while arrested",
            "Hacks/Exploits/Cheats",
            "Pretending to be an admin",
            "RDM as cop",
            "RDA as cop",
            "Bad username",
            "Bullying, Harrasing, or Discriminating someone",
            "Alternative account's (alts)"
        };

        public static int[] CommonIDs =
        {
            // need a better way of doing this
            493970259, // Pistol Ammo
            -479434394, // Handcuffs
            -906852676, // Taser Ammo
            -700261193, //License Boating
            1695812550, //License Drivers
            499504400, //License Gun
            607710552 //License Pilots
        };
		public static int[] IDs_Vehicles;
        public static int[] IDs_Items;
    }
	public class LastLocation
	{
		public Vector3 Position { get; private set; }
		public Quaternion Rotation { get; private set; }
		public int PlaceIndex { get; private set; }
		public void Update(Vector3 position, Quaternion rotation, int index)
		{
			Position = position;
			Rotation = rotation;
			PlaceIndex = index;
		}
		public bool HasPositionSet()
		{
			return Position != default(Vector3) && Rotation != default(Quaternion);
		}
		public LastLocation(Vector3 position, Quaternion rotation, int index)
		{
			Update(position, rotation, index);
		}
		public LastLocation()
		{

		}
	}
    public class _CommandList
    {
        public Action<SvPlayer, string> RunMethod;
        public List<string> commandCmds;
        public bool commandDisabled;
        public string commandGroup;
        public string commandName;
        public bool commandWantedAllowed;
        public bool commandHandcuffedAllowed;
		public bool commandWhileJailedAllowed;
    }

    public class PlayerListItem
    {
		public PlayerListItem(ShPlayer player)
		{
			ShPlayer = player;
		}
        public ShPlayer ShPlayer { get; set; }
        public CurrentMenu LastMenu { get; set; }
        public ShPlayer ReportedPlayer { get; set; }
        public string ReportedReason { get; set; }
        public bool ChatEnabled { get; set; } = true;
        public bool StaffChatEnabled { get; set; }
        public bool ReceiveStaffChat { get; set; } = true;
        public bool SpyEnabled { get; set; }
        public int MessagesSent { get; set; }
        public bool IsCurrentlyAwaiting { get; set; }
        public ShPlayer ReplyToUser { get; set; }
        public ShPlayer TpaUser { get; set; }
		public LastLocation LastLocation { get; set; } = new LastLocation();
    }

    public class _Group
    {
        public string Name;
        public string Message;
        public List<string> Users = new List<string>();
    }

    public enum CurrentMenu
    {
		None,
        Main,
        Help,
        Staff,
        GiveMoney,
        GiveItems,
        ServerInfo,
        Report,
        AdminReport
    }
}