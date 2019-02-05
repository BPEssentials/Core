using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static BP_Essentials.HookMethods;
using static BP_Essentials.Variables;
namespace BP_Essentials
{
    public class HookChat
    {
        #region Event: ChatMessage | Global
        //Chat Events
        [Hook("SvPlayer.SvGlobalChatMessage")]
        public static bool SvGlobalChatMessage(SvPlayer player, ref string message)
        {
            try
            {
                var tempMessage = message;
				if (HandleSpam.Run(player, tempMessage))
					return true;
                //Message Logging
                if (!MutePlayers.Contains(player.playerData.username))
                    LogMessage.Run(player, message);

                if (message.StartsWith(CmdCommandCharacter, StringComparison.CurrentCulture))
					if (OnCommand(player, ref message))
						return true;

                //Checks if the player is muted.
                if (MutePlayers.Contains(player.playerData.username))
                {
                    player.SendChatMessage(SelfIsMuted);
                    return true;
                }
                //Checks if the message contains a username that is AFK.
                if (AfkPlayers.Any(message.Contains))
                    player.SendChatMessage(PlayerIsAFK);

                var shPlayer = player.player;
                if (!PlayerList[shPlayer.ID].ChatEnabled)
                {
                    player.SendChatMessage($"<color={warningColor}>Please enable your chat again by typing</color> <color={argColor}>{CmdCommandCharacter}{CmdToggleChat}</color><color={warningColor}>.</color>");
                    return true;
                }
                if (PlayerList[shPlayer.ID].StaffChatEnabled)
                {
                    SendChatMessageToAdmins.Run(PlaceholderParser.ParseUserMessage(shPlayer, AdminChatMessage, message));
                    return true;
                }
                foreach (var curr in Groups)
                    if (curr.Value.Users.Contains(player.playerData.username))
                    {
                        SendChatMessage.Run(PlaceholderParser.ParseUserMessage(shPlayer, curr.Value.Message, message));
                        return true;
                    }
                if (player.player.admin)
                {
                    SendChatMessage.Run(PlaceholderParser.ParseUserMessage(shPlayer, AdminMessage, message));
                    return true;
                }
                SendChatMessage.Run(PlaceholderParser.ParseUserMessage(shPlayer, PlayerMessage, message));
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
		#endregion

		#region Event OnCommand | Global
		public static bool OnCommand(SvPlayer player, ref string message)
		{
			var tempMessage = message;
			var command = GetArgument.Run(0, false, false, message);
			// CustomCommands
			var customCommand = CustomCommands.FirstOrDefault(x => tempMessage.StartsWith(CmdCommandCharacter + x.Command, StringComparison.CurrentCulture));
			if (customCommand != null)
			{
				foreach (string line in customCommand.Response.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
					player.SendChatMessage(PlaceholderParser.ParseUserMessage(player.player, line, message));
				PlayerList.Where(x => x.Value.SpyEnabled && x.Value.ShPlayer.svPlayer != player).ToList().ForEach(x => x.Value.ShPlayer.svPlayer.SendChatMessage($"<color=#f4c242>[SPYCHAT]</color> {player.playerData.username}: {tempMessage}"));
				return true;
			}
			// Go through all registered commands and check if the command that the user entered matches
			foreach (var cmd in CommandList.Values)
				if (cmd.commandCmds.Contains(command))
				{
					if (cmd.commandDisabled)
					{
						player.SendChatMessage(DisabledCommand);
						return true;
					}
					if (HasPermission.Run(player, cmd.commandGroup, true, player.player.job.jobIndex)
						&& HasWantedLevel.Run(player, cmd.commandWantedAllowed)
						&& IsCuffed.Run(player, cmd.commandHandcuffedAllowed)
						&& IsJailed.Run(player, cmd.commandWhileJailedAllowed))
					{
						PlayerList.Where(x => x.Value.SpyEnabled && x.Value.ShPlayer.svPlayer != player).ToList().ForEach(x => x.Value.ShPlayer.svPlayer.SendChatMessage($"<color=#f4c242>[SPYCHAT]</color> {player.playerData.username}: {tempMessage}"));
						cmd.RunMethod.Invoke(player, message);
					}
					return true;
				}
			if (AfkPlayers.Contains(player.playerData.username))
				Commands.Afk.Run(player, message);
			if (MsgUnknownCommand)
			{
				player.SendChatMessage($"<color={errorColor}>Unknown command. Type</color><color={argColor}> {CmdCommandCharacter}essentials cmds </color><color={errorColor}>for more info.</color>");
				return true;
			}
			return false;
		}
		#endregion

		#region Event: ChatMessage | Local
		[Hook("SvPlayer.SvLocalChatMessage")]
        public static bool SvLocalChatMessage(SvPlayer player, ref string message)
        {
            if (LocalChatMute && MutePlayers.Contains(player.playerData.username))
            {
                player.SendChatMessage(SelfIsMuted);
                return true;
            }
            LogMessage.LocalMessage(player, message);
            if (!ProximityChat)
                return false;
            player.Send(SvSendType.LocalOthers, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>[Local-Chat]</color> {new Regex("(<)").Replace(player.player.username, "<<b></b>")}: {new Regex("(<)").Replace(message, "<<b></b>")}");
            return true;
        }
        #endregion
    }
}