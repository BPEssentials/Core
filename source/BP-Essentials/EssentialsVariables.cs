using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BP_Essentials
{
    public class EssentialsVariablesPlugin : EssentialsCorePlugin
    {
        public const string Version = "2.4.0";

        // Generic Constants
        public const string FileDirectory = "Essentials/";
        public const string LogDirectory = FileDirectory + "logs/";

        public const string SettingsFile = FileDirectory + "essentials_settings.txt";
        public const string LanguageBlockFile = FileDirectory + "languageblock.txt";
        public const string ChatBlockFile = FileDirectory + "chatblock.txt";
        public const string AnnouncementsFile = FileDirectory + "announcements.txt";
        public const string IpListFile = FileDirectory + "ip_list.txt";
        public const string GodListFile = FileDirectory + "godlist.txt";
        public const string AfkListFile = FileDirectory + "afklist.txt";
        public const string MuteListFile = FileDirectory + "mutelist.txt";
        public const string ExceptionFile = FileDirectory + "exceptions.txt";
        public const string CustomCommandsFile = FileDirectory + "CustomCommands.txt";
        public const string CustomGroupsFile = FileDirectory + "CustomGroups.txt";
        public const string IdListFile = FileDirectory + "ID_list.txt";

        public const string AdminListFile = "admin_list.txt";
        public const string RulesFile = "server_info.txt";
        public const string BansFile = "ban_list.txt";

        public const string LogFile = LogDirectory + "all.txt";
        public const string ChatLogFile = LogDirectory + "chat.txt";
        public const string CommandLogFile = LogDirectory + "commands.txt";

        // Bools
        public static bool MsgUnknownCommand;
        public static bool ChatBlock;
        public static bool LanguageBlock;
        public static bool CheckAlt;
        public static bool All;
        public static bool Unmute;
        public static bool MessageToLower;
        public static bool EnableAtmCommand;
        public static bool Confirmed;
        public static bool? EnableBlockSpawnBot;
        public static bool ShowDMGMessage;
        public static bool VoteKickDisabled;
        public static bool DownloadIdList;

        // Lists
        public static List<string> CustomCommands = new List<string>();
        public static List<string> Responses = new List<string>();
        public static List<string> ChatBlockWords = new List<string>();
        public static List<string> LanguageBlockWords = new List<string>();
        public static List<string> AdminsListPlayers = new List<string>();
        public static List<string> GodListPlayers = new List<string>();
        public static List<string> AfkPlayers = new List<string>();
        public static List<string> MutePlayers = new List<string>();
        public static List<string> LatestVotePeople = new List<string>();

        // Arrays
        public static string[] Announcements;
        public static readonly string[] Jobs = { "Citizen", "Criminal", "Prisoner", "Police", "Paramedic", "Firefighter", "Gangster: Red", "Gangster: Green", "Gangster: Blue", "Mayor", "DeliveryDriver", "TaxiDriver", "Special Forces" };

        // Messages
        public static string MsgSayPrefix;
        public static string MsgNoPerm;
        public static string MsgDiscord;
        public static string DisabledCommand;
        public static string ArgRequired;
        public static string PlayerIsAFK;
        public static string SelfIsMuted;
        public static string NotFoundOnline;
        public static string AdminSearchingInv;
        public static string PlayerMessage;
        public static string AdminMessage;
        public static string AdminChatMessage;

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

        // Commands
        #region Commands

        public static string CmdSay, CmdSay2, CmdSayExecutableBy;
        public static string CmdGodmode, CmdGodmode2, CmdGodmodeExecutableBy;
        public static string CmdMute, CmdMute2, CmdMuteExecutableBy;
        public static string CmdAfk, CmdAfk2, CmdAfkExecutableBy;
        public static string CmdFakeJoin, CmdFakeJoin2, CmdFakeJoinExecutableBy;
        public static string CmdFakeLeave, CmdFakeLeave2, CmdFakeLeaveExecutableBy;
        public static string CmdPlayers, CmdPlayers2, CmdPlayersExecutableBy;
        public static string CmdInfo, CmdInfo2, CmdInfoExecutableBy;
        public static string CmdMoney, CmdMoney2, CmdMoneyExecutableBy;
        public static string CmdAtm, CmdAtm2, CmdAtmExecutableBy;
        public static string CmdPay, CmdPay2, CmdPayExecutableBy;
        public static string CmdTpHere, CmdTpHere2, CmdTpHereExecutableBy;
        public static string CmdHeal, CmdHeal2, CmdHealExecutableBy;
        public static string CmdFeed, CmdFeed2, CmdFeedExecutableBy;
        public static string CmdCheckAlts, CmdCheckAlts2, CmdCheckAltsExecutableBy;
        public static string CmdGive, CmdGive2, CmdGiveExecutableBy;
        public static string CmdSetjob, CmdSetjob2, CmdSetjobExecutableBy;
        public static string CmdLaunch, CmdLaunch2, CmdLaunchExecutableBy;
        public static string CmdStrip, CmdStrip2, CmdStripExecutableBy;
        public static string CmdSlap, CmdSlap2, CmdSlapExecutableBy;
        public static string CmdSearch, CmdSearch2, CmdSearchExecutableBy;
        public static string CmdJail, CmdJail2, CmdJailExecutableBy;
        public static string CmdKnockout, CmdKnockout2, CmdKnockoutExecutableBy;
        public static string CmdKill, CmdKillExecutableBy;
        public static string CmdBan, CmdBanExecutableBy;
        public static string CmdKick, CmdKickExecutableBy;
        public static string CmdLogs, CmdLogsExecutableBy;
        public static string CmdArrest, CmdArrestExecutableBy;
        public static string CmdRestrain, CmdRestrainExecutableBy;
        public static string CmdFree, CmdFreeExecutableBy;
        public static string CmdTp, CmdTpExecutableBy;
        public static string CmdSave, CmdSaveExecutableBy;
        public static string CmdLatestVoteResults, CmdLatestVoteResults2, CmdLatestVoteResultsExecutableBy;
        public static string CmdClearWanted, CmdClearWanted2, CmdClearWantedExecutableBy;
        public static string CmdToggleChat, CmdToggleChat2, CmdToggleChatExecutableBy;
        public static string CmdDebug, CmdDebug2;
        public static string CmdConfirm, CmdConfirm2;
        public static string CmdReload, CmdReload2;
        public static string CmdClearChat, CmdClearChat2;
        public static string CmdReport, CmdReport2;
        public static string CmdStaffChat, CmdStaffChat2, CmdStaffChatExecutableBy;
        public static string CmdStaffChatMessages, CmdStaffChatMessages2, CmdStaffChatMessagesExecutableBy;
        public static string CmdHelp;
        public static string CmdCommandCharacter;
        public static bool CmdClearChatDisabled;
        public static bool CmdSayDisabled;
        public static bool CmdGodmodeDisabled;
        public static bool CmdMuteDisabled;
        public static bool CmdAfkDisabled;
        public static bool CmdFakeJoinDisabled;
        public static bool CmdFakeLeaveDisabled;
        public static bool CmdPlayersDisabled;
        public static bool CmdInfoDisabled;
        public static bool CmdMoneyDisabled;
        public static bool CmdAtmDisabled;
        public static bool CmdPayDisabled;
        public static bool CmdHealDisabled;
        public static bool CmdFeedDisabled;
        public static bool CmdCheckAltsDisabled;
        public static bool CmdGiveDisabled;
        public static bool CmdLatestVoteResultsDisabled;
        public static bool CmdSetjobDisabled;
        public static bool CmdClearWantedDisabled;
        public static bool CmdReportDisabled;
        public static bool CmdLaunchDisabled;
        public static bool CmdStripDisabled;
        public static bool CmdSlapDisabled;
        public static bool CmdSearchDisabled;
        public static bool CmdJailDisabled;
        public static bool CmdKnockoutDisabled;
        public static bool CmdToggleChatDisabled;
        public static bool CmdStaffChatDisabled;
        public static bool CmdStaffChatMessagesDisabled;
        #endregion

        // Ints
        public const int SaveTime = 60 * 5;
        public static int AnnounceIndex;
        public static int TimeBetweenAnnounce;
        public static int[] BlockedSpawnIds;

        // Misc.
        public static string _msg;
        public static string username;
        public static string TimestampFormat;
        public const string CensoredText = "******";
        public const string PatternTemplate = @"\b({0})(s?)\b";
        public static Dictionary<int, _PlayerList> playerList = new Dictionary<int, _PlayerList>();
        public static Dictionary<string, _Group> Groups = new Dictionary<string, _Group>();

        public static string[] ReportReasons =
        {
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
        #region ID LIST
        public static int[] IDs = {
0, // you don't want to use ID 0
-2081117539, //FireExtinguisher
-1490613521, //FireHose
1097271049, //Bandage
2048164192, //Defibrillator
1783530721, //Morphine
-479434394, //Handcuffs
1411864780, //Axe
1953054383, //BatMetal
2082248681, //Baton
1021921521, //BatWood
162623668, //BrassKnuckles
304757094, //Chainsaw
-1291023120, //Cleaver
-900648623, //Crowbar
1613601012, //Drill
-1312754449, //FireAxe
539449839, //FryPan1
-1188165547, //FryPan2
1800940813, //GolfClub
2109002909, //AK47
1968059740, //Colt
-1975896234, //Glock
633001730, //M4
794733957, //Mac
-1384309123, //MachineGun
1875626018, //MP5SD
554920573, //Shotgun
-162370066, //Sig
1053582733, //Springfield
2015188875, //Taser
238752591, //Winchester
-648442112, //Hammer
-1477501700, //Hands
-484090981, //Kabar
2072098604, //Knife
1670374823, //Machete
1216282502, //MetalPipe
-406179965, //Plank
-65303750, //PlankSpiked
807974021, //Sledgehammer
-1477281543, //Spade
1529145355, //Sword
1616058966, //Wrench
869043502, //AmmoMG
493970259, //AmmoPistol
-872807194, //AmmoRifle
1086364132, //AmmoShotgun
168336396, //AmmoSMG
-906852676, //AmmoTaser
-1108084005, //Ashtray
-242327999, //Binder1
1753681915, //Binder2
528498541, //Binder3
-2115720498, //Binder4
-261377924, //Bloodbag
540923433, //Bomb
-1098973167, //Bong1
662065579, //Bong2
572982516, //Bonzai
319300906, //Book1
-1979616112, //Book2
-49921018, //Book3
1667420581, //Book4
-1429135829, //BoomBox
1932783552, //Bucket1
-365248902, //Bucket2
-1656910100, //Bucket3
1797559058, //Calculator
1018227507, //Camera
-1309120383, //Candle1
686897467, //Candle2
1609959853, //Candle3
1812188878, //CDCase
313689893, //CDHolder
981834931, //Chest
2053867013, //Chiller1
-480061505, //Chiller2
-1805261015, //Chiller3
253737406, //Cigar1
-1776883708, //Cigar2
-518924142, //Cigar3
2138393905, //Cigar4
2028024241, //Cigarette1
-504864757, //Cigarette2
220874293, //Clipboard
1469219949, //Coin1
-828647977, //Coin2
-1775946983, //ComputerCaseModern
-1310377196, //ComputerCaseOffice
-816841057, //ComputerCaseOld
-1979328202, //ComputerDisplayModern
846906178, //ComputerDisplayOffice1
-1418489096, //ComputerDisplayOffice2
708496214, //ComputerDisplayOld
-237671921, //ComputerKeyboardModern
-703241726, //ComputerKeyboardOffice
-78488252, //ComputerKeyboardOld
532683176, //ComputerMouseModern
-1179427236, //ComputerMouseOld
-366810752, //Beer1
1932245050, //Beer2
70027436, //Beer3
1202587699, //CoffeeCup1
-559457911, //CoffeeCup2
-1449105121, //CoffeeCup3
935656636, //CoffeeCup4
-82690173, //CoffeeCupSmall
-1953356567, //CupWater
1734556914, //EnergyCan1
-26579640, //EnergyCan2
-1495334556, //MilkBottle1
1071010014, //MilkBottle2
1414592914, //MilkCarton1
-849762264, //MilkCarton2
312215426, //PaperBottle1
-1953147336, //PaperBottle2
-755725543, //PlasticCoffeeCup1
1274927779, //PlasticCoffeeCup2
-1638525914, //SodaCan1
123684252, //SodaCan2
1884845322, //SodaCan3
707466586, //SoftDrinkLarge1
-1289599776, //SoftDrinkLarge2
1736080705, //SoftDrinkSmall
1877954167, //WaterBottle1
-152690739, //WaterBottle2
19015101, //Wine1
-1741982713, //Wine2
-282311535, //Wine3
-846875713, //Cocaine
-1617712860, //Heroin
236716975, //Meth
1314879085, //Moonshine
1809308527, //Weed
-273508955, //Apple1
1992016927, //Apple2
1279649806, //AppleBig1
-716392012, //AppleBig2
-1572107998, //AppleBig3
1009720449, //AppleBig4
310612836, //AppleHalf1
-1953840418, //AppleHalf2
1702175260, //Bacon
69690105, //Banana
1148264960, //BananaBunch1
-579308614, //BananaBunch2
971070930, //BananaSmall
295609557, //BigMac
-577045850, //BlueberryCream
375734610, //BunBottom
-1068735117, //BunTop
-397451461, //Burger
-1489105393, //BurgerMeal
1793713626, //BurgerWrapped
-2039755664, //Burrito
1463680763, //Cake1
-835408063, //Cake2
-1187799081, //Cake3
399745542, //CakeSliceChocolate
1599774388, //CakeSliceVanilla1
-967586034, //CakeSliceVanilla2
-1089302024, //Carrot
407176456, //CarrotSliced
-1245471453, //CarrotSmall
-362654475, //Cheese
923427815, //Cheesecake
1201374976, //Chicken
1585611432, //ChickenLeg
1963643076, //ChickenLegSmall
-1598837947, //ChickenMeal
74472777, //ChocolateBar1
-1653011213, //ChocolateBar2
-360825755, //ChocolateBar3
-897720728, //CinnamonRoll
-1924350128, //Cookie
-420017340, //CookieBig1
2147375870, //CookieBig2
1205979388, //Corn1
-555181754, //Corn2
-1321228377, //CreamRoll1
674690589, //CreamRoll2
1941924400, //CreamSlice
-392093828, //CreamTube
-1617094551, //Croissant
-162406301, //Cucumber1
1868083673, //Cucumber2
-1505527222, //CucumberSliced
664587976, //Cupcake1
-1097490574, //Cupcake2
-913133596, //Cupcake3
-649681443, //CupcakeSmall1
1078981735, //CupcakeSmall2
927515889, //CupcakeSmall3
-1389912314, //Donut1
875482812, //Donut2
1126825514, //Donut3
-582128759, //Donut4
-302939084, //DonutBig1
1962464654, //DonutBig2
1887084450, //Egg
-34417183, //Fish1
1694205019, //Fish2
1176993064, //Fries
1775022893, //Gerkin
-1928640708, //HamLeg
-338274182, //HotDog
-209488711, //IceBlock1
1787438339, //IceBlock2
495785365, //IceBlock3
-580117133, //IceCream1
1147489481, //IceCream2
-1773123705, //IceCreamCone1
257496637, //IceCreamCone2
2019436203, //IceCreamCone3
242189893, //IceCreamSundae
460359885, //Kiwi1
-2105943689, //Kiwi2
1167664423, //KiwiSliced
404542308, //Leak
-1883035562, //Lettuce
113691451, //Muffin1
-1613784447, //Muffin2
-389502441, //Muffin3
516242109, //Mushroom
-1363098368, //Mushrooms
-938380499, //Onion
443135054, //Orange
-587460087, //OrangeBig1
1173701555, //OrangeBig2
-415825014, //Pancakes
1289297238, //Patty
-150239810, //Pear1
1845679108, //Pear2
419816594, //Pear3
-467943479, //Pie1
2098392691, //Pie2
169205477, //Pie3
-1220321196, //Pineapple
236760427, //Pizza
26382251, //PizzaThin
1674766418, //Popcorn
-1719549639, //Ribs
1129488573, //Salami
-61088408, //SalamiSliced
1120707446, //Sandwich1
-607784244, //Sandwich2
-243156885, //Sausage1
1753803217, //Sausage2
-378704046, //SausageBread
1973375733, //SausageRope
390308133, //ShishKebab
-1795417320, //Shrimp
-1378573660, //ShrimpTray
-1515157115, //Steak1
1018812479, //Steak2
1270802601, //Steak3
-1178999815, //SteakSmall
-800722565, //Sushi1
1229758657, //Sushi2
1045139543, //Sushi3
-1075600813, //Taco
-1906967086, //TakeawayBag
1922778948, //Toast
733496620, //Tomato
-374771862, //TomatoSlice
-1233064414, //TomatoSlices
-1626110389, //Turkey1
102422513, //Turkey2
1897785191, //Turkey3
-1566807776, //Waffle
811305019, //WaterMelon1
-1454188159, //WaterMelon2
467765943, //MedicBox1
-2098668787, //MedicBox2
1789815627, //DeskLamp
1324844400, //Cannabis
-1626616456, //Coca
-1605962214, //Ephedrin
-719529300, //Ethanol
1374943456, //Opium
-1343270306, //FlowerVase
-351400709, //Folder
613358412, //FoodBowl1
-1115272458, //FoodBowl2
-897484192, //FoodBowl3
1860288822, //FruitBowl
1699387113, //FuelCan
-2070052131, //Armchair1
496292711, //Armchair2
1788077041, //Armchair3
1202142496, //ArmchairBig
1331416699, //BarCounter
464889268, //BarStool
-621624305, //BedDouble1
1140561333, //BedDouble2
888972579, //BedDouble3
-1908616205, //BedSingle1
389382729, //BedSingle2
1613935327, //BedSingle3
-820845019, //Bench1
1444657055, //Bench2
555517705, //Bench3
1134099175, //BunkBed1
-627987619, //BunkBed2
-1316498584, //Cabinet1
680428242, //Cabinet2
1602859588, //Cabinet3
-1041422361, //Cabinet4
-1226172559, //Cabinet5
1690463105, //Chair
419929706, //Chair1
-2146512944, //Chair2
-150339770, //Chair3
1768778469, //Chair4
510286451, //Chair5
-2023552055, //Chair6
-261866657, //Chair7
1624975054, //Chair8
400299608, //Chair9
-613832092, //ChairOffice
-28765381, //ChairSwivel1
1732264577, //ChairSwivel2
-2144444209, //ChestOld
11133431, //CopyMachine
-907608641, //Couch1
1357884421, //Couch2
669555859, //Couch3
-1182004944, //Couch4
-829736538, //Couch5
1468164124, //Couch6
545609866, //Couch7
-1338358501, //Couch8
-952273523, //Couch9
91428285, //CounterWood
-423007494, //Desk1
2143304512, //Desk2
-533773879, //DeskChair
772328317, //Dresser1
-1224631609, //Dresser2
-1073305007, //Dresser3
1583497202, //Dresser4
694488932, //Dresser5
-1335075106, //Dresser6
647466325, //DresserMirror1
-1080107793, //DresserMirror2
-929436551, //DresserMirror3
203711829, //DrumKit
1472642478, //Dryer
-1391042892, //FootRest1
874450702, //FootRest2
1152478689, //HighChair
2047009475, //Keyboard
-430574708, //Lamp1
2136777270, //Lamp2
140219040, //Lamp3
-1774254333, //Lamp4
-516409451, //Lamp5
2016503343, //Lamp6
-1234597859, //Oven1
794876327, //Oven2
1483204913, //Oven3
541897676, //BathMat1
-1186602378, //BathMat2
-1085122469, //BooksFew
1289059060, //BooksMany
-1746943216, //Calendar
-1493150307, //Certificate
2142933398, //Clock
-1888650310, //Corkboard
-141437809, //Cushion1
1855489333, //Cushion2
1908819108, //Flag
-244484924, //FloorMat1
1751393662, //FloorMat2
144897887, //FullLengthMirror
848466005, //Guitar1
-1415855633, //Guitar2
-185936085, //GuitarElectric1
1843627665, //GuitarElectric2
451196423, //GuitarElectric3
1235636787, //InsectFrame1
-793836663, //InsectFrame2
-1482149089, //InsectFrame3
-1053045356, //MatRubber
2075660469, //Placemat1
-491699953, //Placemat2
-1783205479, //Placemat3
1014361465, //PlantVaseShort
977778144, //PlantVaseTall
762453193, //PlateDryer
1763200980, //Rug1
-267313554, //Rug2
-2028589320, //Rug3
426949467, //Rug4
366971451, //Saxophone
1274143987, //ToiletBrush
-92602784, //ToyBear
1926903954, //ToyCat
-672477378, //ToyChick
1839743559, //ToyDog
-1397850393, //ToyLamb
551415405, //ToyPig
1706266935, //ToyRabbit
2046762882, //ToyTurtle
-1356689746, //TrashCanSmall
1520699428, //WallArt1
-1012049506, //WallArt2
-1263900408, //WallArt3
718208171, //WallArt4
-2013623785, //WallDeco1
519297965, //WallDeco2
1777658683, //WallDeco3
-141525352, //WallDeco4
-2137567730, //WallDeco5
429793204, //WallDeco6
1855532834, //WallDeco7
-31051085, //WallDeco8
-439271429, //RoomDivider
1936595972, //SafeSmall
1114775266, //Shelf1Big
1060332178, //Shelf1Medium
-1933729649, //Shelf1Small
-595780254, //Shelf1Thin
288187477, //Shelf1Tiny
1355264268, //Shelf2Big
249264143, //Shelf2Medium
170512929, //Shelf2Small
-1679988814, //Shelf2Thin
1452088965, //Shelf2Tiny
-1692189096, //ShelfMetal
2118980676, //ShelfOffice1
-414956034, //ShelfOffice2
-1874627224, //ShelfOffice3
237494475, //ShelfOffice4
-1079661818, //ShopShelf1
648870588, //ShopShelf2
-2037103436, //ShopTall
-305205626, //Showcase
-562139751, //CabinetFiling
-51935546, //CabinetFlat
693498836, //Cabinets
-667273670, //Case
1591617113, //ClockGrandfather
-129617024, //CoffeeMachine
446585989, //Cupboards1
-2087252673, //Cupboards2
-191873623, //Cupboards3
-180008769, //Fridge
-1930958485, //RecordPlayer
-1367046112, //RecordShelf1
932043162, //RecordShelf2
1561328110, //RubbishBin1
-1006196652, //RubbishBin2
-1147890278, //SoundMonitor2
-1451178543, //Stereo
1009707106, //TrashBinHome
874269822, //WallShelf1
-1391125052, //WallShelf2
-615675499, //SimpleRecycle
-1330575044, //SimpleTrash
580764704, //SoundMonitor1
-1205427253, //Stool
456730539, //Table1
-2110753263, //Table2
-180926841, //Table3
1800655652, //Table4
475325362, //Table5
1416725174, //TableCoffee
499900372, //TableOffice1
-2067624338, //TableOffice2
-205144328, //TableOffice3
1839278939, //TableOffice4
447093709, //TableOffice5
583490363, //TableShop1
-1144124799, //TableShop2
14893190, //TableSmall1
-1712713412, //TableSmall2
-286449238, //TableSmall3
1888072713, //TableSmall4
126780575, //TableSmall5
-1635265243, //TableSmall6
-377428557, //TableSmall7
2034235426, //TableSmall8
781101585, //WallExtinguisher
-424034282, //Wardrobe1
2142278060, //Wardrobe2
-1646994167, //Washer
1546856892, //WaterCooler
1312283616, //Whiteboard
-1506794359, //Game1
1060730163, //Game2
-81513546, //GameConsole
-390648404, //GamePad
1334811906, //Handbag1
-695808840, //Handbag2
-1585316818, //Handbag3
1071999373, //Handbag4
1222793499, //Handbag5
-773125983, //Handbag6
119353383, //HealthPack
494261122, //Jam1
-2072214984, //Jam2
-209997138, //Jam3
1830758157, //Jam4
87407750, //Joystick
-502195999, //Ketchup
-1939907396, //KeyPrison
-1127724957, //KitchenFork
-567536648, //KitchenKnife
2095575396, //KitchenSpoon
1677000883, //Ladle
-408071411, //Laptop
-700261193, //LicenseBoating
1695812550, //LicenseDrivers
499504400, //LicenseGun
607710552, //LicensePilots
-1092231192, //Lockpick
-1252471357, //Longboard1
743545977, //Longboard2
-896858339, //Magazines1
1401050791, //Magazines2
451290069, //Microwave
-328781560, //MissileBig
-2080560987, //MissileSmall
1981693152, //Money
1108257615, //Mugshot
-1481382589, //Mustard
1822567310, //Necklace1
-173311436, //Necklace2
1241849122, //NecklaceDisplay1
-754168680, //NecklaceDisplay2
-1542751218, //NecklaceDisplay3
980412845, //NecklaceDisplay4
1298716987, //NecklaceDisplay5
-731805567, //NecklaceDisplay6
1142121509, //Newspaper
311999004, //Notepad
-1881452782, //Paper
-109265115, //PaperTray
561849375, //Pen
-1915921987, //Pencil
-1945929627, //PhoneOffice
-1899151937, //PictureFrame
1285549208, //Pills1
-711541470, //Pills2
-3861386, //PlantSmall1
1724663244, //PlantSmall2
298546522, //PlantSmall3
-1884299015, //PlantSmall4
1163839965, //PlateLong
-835689360, //PlateSmall
-1067629520, //Plunger
1123106929, //Printer
-1500569022, //Register
-961292868, //RegisterDigital
-1714478603, //RegisterElectronic1
13004879, //RegisterElectronic2
-1287877920, //RingGold
-1125222960, //RingSilver
398080430, //RiotShield
-921321944, //Rocket
1438178225, //RolledPaper
1642696866, //Scoop
-49005880, //ScotchTape
1517652904, //Screwdriver1
-1015227886, //Screwdriver2
-1266947452, //Screwdriver3
-1561514462, //CannabisSeed
1343067718, //CocaSeed
-1428095884, //OpiumSeed
-1380688662, //Skateboard
-318449686, //SoapDish
1719938366, //SoapDispenser
-709313329, //SoundSystem
521180800, //Spanner
1706792829, //Spatula
1080318452, //Speaker1
-647165874, //Speaker2
-1368770344, //Speaker3
513670350, //SportsBag1
-2020299404, //SportsBag2
-258761246, //SportsBag3
935534676, //Stapler
-4527003, //TableLamp
1338841008, //Tablet1
-691780086, //Tablet2
-1581025636, //Tablet3
-1678397020, //Toaster
-1468082443, //ToolBox1
829917007, //ToolBox2
1181792217, //ToolBox3
2115192440, //Toolkit
701212054, //TV1
-1329400788, //TV2
-943340358, //TV3
1503744281, //TV4
782655887, //TV5
-1213255627, //TV6
203418146, //VaseSmall
-918444274, //VCR
2046665206, //WalkieTalkie1
-520818612, //WalkieTalkie2
-1745633062, //WalkieTalkie3
-33192302, //WalkingStick
1578700508, //Watches Display 1
-955228314, //Watches Display 2
1304259192, //Weight1
-726385726, //Weight2
-1733366847, //Basketball
-1572858027, //Bazooka
161824251, //Flare
895677224, //Flashbang
-1520575955, //Grenade
-2004367746, //Smoke
-552154137, //BackpackBrown
-544534799, //BackpackCombat
-1201692785, //BackpackPurple
334614070, //BackpackRed
-409267593, //BackwardsCapBlue
-762392333, //BackwardsCapGreen
-1962854266, //BackwardsCapRed
360985946, //BeardBlack
973831052, //BeardBlond
-250078426, //BeardBrown
-1221739260, //BlouseBlue
-904946077, //BlouseNavy
-1694703303, //BlouseTeal
-843762116, //CapBlue
1667534522, //CapChauffeur
113858252, //CapFlat
866805455, //CapGreen
753732225, //CapPizza
-1557289298, //CapPolice
-913660239, //CapRacer
-904992899, //CapRed
-951990666, //CapSheriff
-1376032294, //FaceScarfDark
667274109, //FaceScarfLight
-1067924423, //GangJacketBlue
1137541042, //GangJacketGreen
1251400951, //GangJacketRed
-544630730, //GlovesDark
1076848752, //GlovesFingerlessDark
288052618, //GlovesFingerlessLight
-390235841, //GlovesFingerlessMedium
956220633, //GlovesFingerlessWhite
-1911875617, //GlovesLight
-880225031, //GlovesMedical
-215745530, //GlovesMedium
-1478588788, //GlovesWhite
-657754262, //GoateeBlack
-146523716, //GoateeBlond
1012357398, //GoateeBrown
-1849094355, //HardHatDark
69246628, //HardHatLight
880705339, //HardHatMedium
973238883, //HatBoonieDark
-1779484338, //HatBoonieLight
-544909805, //HatFedora
-52820099, //HatFire
-633509750, //HatHazard
-1310874884, //HelmetCombat
-766353867, //HelmetRiot
-622606519, //JacketBusinessBlack
-798800555, //JacketBusinessPurple
1953581485, //JacketBusinessRed
2065462452, //JacketFire
-1867909606, //JacketFireBlack
1992507847, //JacketHippieBlue
-1526906535, //JacketHippieBrown
-1795298674, //JacketHippiePurple
-623640896, //JacketHipsterBlue
1998765530, //JacketHipsterOrange
1753630114, //JacketHipsterPink
-313637738, //JacketRacer
-1574869922, //JacketVarsityBlue
-1871056151, //JacketVarsityGreen
-299146069, //JacketVarsityRed
112674745, //KevlarVest
-1626497894, //NullArmor
673780802, //NullBack
-1638932793, //NullBody
1089711634, //NullFace
2064679354, //NullFeet
1174688158, //NullHands
-501996567, //NullHead
-1191209217, //NullLegs
1645391131, //PantsBlue
730598986, //PantsBrown
-1793758716, //PantsBrownPatched
-1888685880, //PantsChauffeur
1998080865, //PantsCombat
-312606938, //PantsConstructionBlue
134344360, //PantsConstructionOrange
1296190984, //PantsConstructionRed
-443982957, //PantsDoctor
-1527091616, //PantsFire
2025319866, //PantsFireBlack
2044418121, //PantsGray
-1777372097, //PantsGreen
-107652805, //PantsLightBlue
276408694, //PantsLightBrown
1066947391, //PantsLightPurple
1775808134, //PantsMerchant
595581678, //PantsParamedic
506851744, //PantsPolice
-1175302500, //PantsPrisoner
1815065665, //PantsRacer
-295837114, //PantsRiot
541389606, //PantsSheriff
-757862723, //PunkTopBlue
1985284270, //PunkTopRed
996118564, //PunkTopWhite
1372071833, //ShirtBusinessBlue
-476209925, //ShirtBusinessPink
781576011, //ShirtBusinessYellow
-1755425193, //ShirtStripedBlue
-376425089, //ShirtStripedGreen
1718551758, //ShirtStripedPurple
-1161119954, //ShirtTouristOrange
1300919472, //ShirtTouristPurple
1997269705, //ShirtTouristYellow
-2054702454, //ShoesBlack
1629197558, //ShoesBrown
837068077, //ShoesGray
-471928380, //ShoesHipster
363817674, //ShortsBlue
-634433073, //ShortsBrown
1803652245, //SkirtBlue
379826162, //SkirtNavy
1179701416, //SkirtTeal
1860177600, //SlacksBlue
1965685650, //SlacksGray
-2029875776, //SlacksGreen
-40633710, //SneakersBlue
-800938321, //SneakersTeal
-1868436119, //SneakersWhite
889404594, //SneakersYellow
-300071980, //SuspendersBrown
1402878369, //SuspendersGreen
129955089, //SuspendersPurple
-1369391530, //SuspendersShortPurple
1946500812, //SuspendersShortRed
-1797185489, //SuspendersShortYellow
-1691035721, //TeeApple
-1779870671, //TeeLove
-37255800, //TeePizza
1234346447, //TopChauffeur
388702693, //TopCombat
1258964252, //TopConstructionBlue
102550548, //TopConstructionOrange
-1498475603, //TopConstructionRed
-2051395305, //TopDoctor
-1689248836, //TopMerchant
-444494871, //TopParamedic
2114362148, //TopPolice
1266351014, //TopPrisoner
-645285295, //TopRiot
602133489, //TopSafetyVest
-890140811, //TopSheriff
};

        #endregion
    }

    public class _PlayerList
    {
        public ShPlayer shplayer { get; set; }
        public int LastMenu;
        public ShPlayer reportedPlayer { get; set; }
        public string reportedReason;
        public bool chatEnabled = true;
        public bool staffChatEnabled;
        public bool receiveStaffChat = true;
    }

    public class _Group
    {
        public string Name;
        public string Message;
        public List<string> Users = new List<string>();
    }

    public static class CurrentMenu //Todo: convert to enum
    {
        public static readonly int Main;
        public static readonly int Help = 1;
        public static readonly int Staff = 2;
        public static readonly int GiveMoney = 3;
        public static readonly int GiveItems = 4;
        public static readonly int ServerInfo = 5;
        public static readonly int Report = 6;
        public static readonly int AdminReport = 7;
    }
}