using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static BP_Essentials.Variables;

namespace BP_Essentials.Methods.Utils
{
	public static class FunctionMenu
	{
		public static void RegisterMenus()
		{
			FunctionMenuKeys = new MultiDictionary<byte, CurrentMenu, Func<SvPlayer, CurrentMenu>>
			{
				[1, CurrentMenu.None] = MainMenu,

				[2, CurrentMenu.ServerInfo] = ServerInfo,
				[2, CurrentMenu.Staff] = GiveMoneyMenu,
				[2, CurrentMenu.GiveMoney] = Give1kMoney,
				[2, CurrentMenu.GiveItems] = GivePistolAmmo,
				[2, CurrentMenu.AdminReport] = AdminReportWindowTeleport,

				[3, CurrentMenu.Main] = ServerInfoMenu,
				[3, CurrentMenu.ServerInfo] = ServerAdmins,
				[3, CurrentMenu.Staff] = GiveItemsMenu,
				[3, CurrentMenu.GiveMoney] = Give10kMoney,
				[3, CurrentMenu.GiveItems] = Give20Handcuffs,
				[3, CurrentMenu.AdminReport] = ResetWindow,

				[4, CurrentMenu.GiveMoney] = Give100kMoney,
				[4, CurrentMenu.GiveItems] = Give10TaserAmmo,
				[4, CurrentMenu.Staff] = Heal,

				[5, CurrentMenu.GiveItems] = GiveAllLicenses,
				[5, CurrentMenu.Staff] = Feed,

				[6, CurrentMenu.Staff] = ClearWanted,

				[10, CurrentMenu.Main] = StaffMenu
			};
		}
		public static CurrentMenu ResetWindow(SvPlayer player)
		{
			player.CloseFunctionMenu();
			return CurrentMenu.None;
		}

		#region Key | Any
		public static CurrentMenu ReportMenu(SvPlayer player, byte key)
		{
			player.CloseFunctionMenu();
			if (!PlayerList.TryGetValue(player.player.ID, out var playerListItem))
				return CurrentMenu.None;
			player.SendChatMessage($"<color={infoColor}>Reported \"</color><color={warningColor}>{playerListItem.ReportedPlayer.username}</color><color={infoColor}>\" With the reason \"</color><color={warningColor}>{ReportReasons[key - 2]}</color><color={infoColor}>\".</color>");
			playerListItem.ReportedReason = ReportReasons[key - 2];
			SendDiscordMessage.ReportMessage(playerListItem.ReportedPlayer.username, player.player.username, ReportReasons[key - 2]);
			ReportPlayer.Run(player.player.username, ReportReasons[key - 2], playerListItem.ReportedPlayer);
			return CurrentMenu.None;
		}
		#endregion

		#region Key | 1
		public static CurrentMenu MainMenu(SvPlayer player)
		{
			if (HasPermission.Run(player, AccessMoneyMenu) || HasPermission.Run(player, AccessItemMenu) || HasPermission.Run(player, AccessSetHPMenu) || HasPermission.Run(player, AccessSetStatsMenu) || HasPermission.Run(player, AccessCWMenu))
			{
				player.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowFunctionMenu, "<color=#00ffffff>Main menu:</color>\n\n<color=#00ffffff>F3:</color> Server info menu\n<color=#00ffffff>F10:</color> Extras menu\n\n<color=#00ffffff>Press</color> <color=#ea8220>F11</color> <color=#00ffffff>To close this (G)UI</color>");
				return CurrentMenu.Main;
			}
			player.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowFunctionMenu, "<color=#00ffffff>Main menu:</color>\n\n<color=#00ffffff>F3:</color> Server info menu\n\n<color=#00ffffff>Press</color> <color=#ea8220>F11</color> <color=#00ffffff>To close this (G)UI</color>");
			return CurrentMenu.Main;
		}
		#endregion

		#region Key | 2
		public static CurrentMenu ServerInfo(SvPlayer player)
		{
			player.CloseFunctionMenu();
			player.Send(SvSendType.Self, Channel.Fragmented, ClPacket.ServerInfo, File.ReadAllText("server_info.txt"));
			return CurrentMenu.None;
		}
		public static CurrentMenu GiveMoneyMenu(SvPlayer player)
		{
			if (!HasPermission.Run(player, AccessMoneyMenu))
				return CurrentMenu.None;
			player.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowFunctionMenu, "<color=#00ffffff>Give Money menu:</color>\n\n<color=#00ffffff>F2:</color> Give <color=#ea8220>1.000 dollars (1k)</color>\n<color=#00ffffff>F3:</color> Give <color=#ea8220>10.000 dollars (10k)</color>\n<color=#00ffffff>F4:</color> Give <color=#ea8220>100.000 dollars (100k)</color>\n\n<color=#00ffffff>Press</color><color=#ea8220> F11 </color><color=#00ffffff>To close this (G)UI</color>");
			return CurrentMenu.GiveMoney;
		}
		public static CurrentMenu Give1kMoney(SvPlayer player)
		{
			player.player.TransferMoney(DeltaInv.AddToMe, 1000, true);
			player.SendChatMessage($"<color={infoColor}>You have given yourself 1.000 dollars.</color>");
			Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] {player.player.username} Spawned in 1.000 dollars through the functionUI");
			return CurrentMenu.GiveMoney;
		}
		public static CurrentMenu GivePistolAmmo(SvPlayer player)
		{
			player.player.TransferItem(DeltaInv.AddToMe, CommonIDs[0], 500, true);
			player.Send(SvSendType.Self, Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>You have given yourself 500 pistol ammo.</color>");
			Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] {player.player.username} Spawned in 500 pistol ammo through the functionUI");
			return CurrentMenu.GiveItems;
		}
		public static CurrentMenu AdminReportWindowTeleport(SvPlayer player)
		{
			player.CloseFunctionMenu();
			if (!PlayerList.TryGetValue(player.player.ID, out var playerItem))
				return CurrentMenu.None;
			playerItem.ReportedPlayer = null;
			if (!IsOnline.Run(playerItem.ReportedPlayer))
			{
				player.SendChatMessage("<color=#ff0000ff>Player not online anymore.</color>");
				return CurrentMenu.None;
			}
			player.ResetAndSavePosition(playerItem.ReportedPlayer.GetPosition(), playerItem.ReportedPlayer.GetRotation(), playerItem.ReportedPlayer.GetPlaceIndex());
			player.SendChatMessage($"<color={infoColor}>Teleported to \"</color><color=#ea8220>{playerItem.ReportedPlayer.username}</color><color={infoColor}>\".</color>");
			return CurrentMenu.None;
		}
		#endregion

		#region Key | 3
		public static CurrentMenu GiveItemsMenu(SvPlayer player)
		{
			if (!HasPermission.Run(player, AccessItemMenu))
				return CurrentMenu.None;
			player.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowFunctionMenu, "<color=#00ffffff>Give Items menu:</color>\n\n<color=#00ffffff>F2:</color> Give <color=#ea8220>500</color> Pistol Ammo\n<color=#00ffffff>F3:</color> Give <color=#ea8220>20</color> Handcuffs\n<color=#00ffffff>F4:</color> Give <color=#ea8220>10</color> Taser ammo\n<color=#00ffffff>F5:</color> Give <color=#ea8220>all</color> Licenses\n\n<color=#00ffffff>Press</color><color=#ea8220> F11 </color><color=#00ffffff>To close this (G)UI</color>");
			return CurrentMenu.GiveItems;
		}
		public static CurrentMenu Give10kMoney(SvPlayer player)
		{
			player.player.TransferMoney(DeltaInv.AddToMe, 10000, true);
			player.SendChatMessage($"<color={infoColor}>You have given yourself 10.000 dollars.</color>");
			Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] {player.player.username} Spawned in 10.000 dollars through the functionUI");
			return CurrentMenu.GiveMoney;
		}
		public static CurrentMenu Give20Handcuffs(SvPlayer player)
		{
			player.player.TransferItem(1, CommonIDs[1], 20, true);
			player.SendChatMessage($"<color={infoColor}>You have given yourself 20 handcuffs.</color>");
			Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] {player.player.username} Spawned in 20 handcuffs through the functionUI");
			return CurrentMenu.GiveItems;
		}
		public static CurrentMenu ServerInfoMenu(SvPlayer player)
		{
			player.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowFunctionMenu, "<color=#00ffffff>Server info menu:</color>\n\n<color=#00ffffff>F2:</color> Show rules\n<color=#00ffffff>F3:</color> Show admins\n\n<color=#00ffffff>Press</color><color=#ea8220> F11 </color><color=#00ffffff>To close this (G)UI</color>");
			return CurrentMenu.ServerInfo;
		}
		public static CurrentMenu ServerAdmins(SvPlayer player)
		{
			player.CloseFunctionMenu();
			var builder = new StringBuilder("All admins on this server:\n\n");
			foreach (var line in File.ReadAllLines("admin_list.txt"))
				if (line.Trim() != null && !line.Trim().StartsWith("#", StringComparison.OrdinalIgnoreCase))
					builder.AppendLine(line);
			player.Send(SvSendType.Self, Channel.Fragmented, ClPacket.ServerInfo, builder.ToString());
			return CurrentMenu.None;
		}
		#endregion

		#region Key | 4
		public static CurrentMenu Give100kMoney(SvPlayer player)
		{
			player.player.TransferMoney(DeltaInv.AddToMe, 100000, true);
			player.SendChatMessage($"<color={infoColor}>You have given yourself 100.000 dollars.</color>");
			Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] {player.player.username} Spawned in 100.000 dollars through the functionUI");
			return CurrentMenu.GiveMoney;
		}
		public static CurrentMenu Heal(SvPlayer player)
		{
			if (!HasPermission.Run(player, AccessSetHPMenu))
				return CurrentMenu.None;
			player.Heal(100);
			player.SendChatMessage($"<color={infoColor}>You've been healed.</color>");
			Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] {player.player.username} healed himself through the functionUI");
			return CurrentMenu.Staff;
		}
		public static CurrentMenu Give10TaserAmmo(SvPlayer player)
		{
			player.player.TransferItem(1, CommonIDs[2], 10, true);
			player.SendChatMessage($"<color={infoColor}>You have given yourself 10 Taser ammo.</color>");
			Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] {player.player.username} Spawned in 10 taser ammo through the functionUI");
			return CurrentMenu.GiveItems;
		}
		#endregion

		#region Key | 5
		public static CurrentMenu Feed(SvPlayer player)
		{
			if (!HasPermission.Run(player, AccessSetStatsMenu))
				return CurrentMenu.None;
			player.UpdateStats(100F, 100F, 100F, 100F);
			player.SendChatMessage($"<color={infoColor}>Maxed out stats for yourself.</color>");
			Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] {player.player.username} Maxed out stats through the functionUI");
			return CurrentMenu.Staff;
		}
		public static CurrentMenu GiveAllLicenses(SvPlayer player)
		{
			for (int i = 3; i < 7; i++)
				player.player.TransferItem(DeltaInv.AddToMe, CommonIDs[i], 1, true);
			player.SendChatMessage($"<color={infoColor}>You have given yourself all licenses.</color>");
			Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] {player.player.username} Spawned in all licenses through the functionUI");
			return CurrentMenu.GiveItems;
		}
		#endregion

		#region Key | 6
		public static CurrentMenu ClearWanted(SvPlayer player)
		{
			player.player.ClearCrimes();
			player.Send(SvSendType.Self, Channel.Reliable, ClPacket.ClearCrimes, player.player.ID);
			player.Send(SvSendType.Self, Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>Cleared wanted level.</color>");
			Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] {player.player.username} Removed his wantedlevel through the functionUI");
			return CurrentMenu.Staff;
		}
		#endregion

		#region Key | 10
		public static CurrentMenu StaffMenu(SvPlayer player)
		{
			var sb = new StringBuilder("<color=#00ffffff>Staff menu:</color>\n\n");
			if (HasPermission.Run(player, AccessMoneyMenu))
				sb.Append("<color=#00ffffff>F2:</color> Give Money\n");
			if (HasPermission.Run(player, AccessItemMenu))
				sb.Append("<color=#00ffffff>F3:</color> Give Items\n");
			if (HasPermission.Run(player, AccessSetHPMenu))
				sb.Append("<color=#00ffffff>F4:</color> Set HP to full\n");
			if (HasPermission.Run(player, AccessSetStatsMenu))
				sb.Append("<color=#00ffffff>F5:</color> Set Stats to full\n");
			if (HasPermission.Run(player, AccessCWMenu))
				sb.Append("<color=#00ffffff>F6:</color> Clear wanted level\n\n");
			player.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowFunctionMenu, $"{sb}<color=#00ffffff>Press</color><color=#ea8220> F11 </color><color=#00ffffff>To close this (G)UI</color>");
			return CurrentMenu.Staff;
		}
		#endregion
	}
}
