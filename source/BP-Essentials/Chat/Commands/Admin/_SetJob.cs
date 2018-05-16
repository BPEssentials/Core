using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.Reflection;

namespace BP_Essentials.Commands
{
    class _SetJob : SvPlayer
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (HasPermission.Run(player, CmdSetjobExecutableBy))
                {
                    string NotValidArg = $"<color={errorColor}>Error: Is that a valid number you provided as argument?</color>";
                    string arg1 = GetArgument.Run(1, false, false, message);
                    string arg2 = GetArgument.Run(2, false, true, message + " ");
                    byte arg1int = 10;
                    string msg = $"<color={infoColor}>Set </color><color={argColor}>{{0}}</color><color={infoColor}>'s Job to</color> <color={argColor}>{{1}}</color><color={infoColor}>.</color>";
                    if (String.IsNullOrEmpty(arg2))
                        arg2 = player.playerData.username;
                    if (!String.IsNullOrEmpty(arg1))
                    {
                        bool Parsed = true;
                        if (Jobs.Contains(arg1))
                            arg1int = Convert.ToByte((Array.IndexOf(Jobs, arg1)));
                        else
                            Parsed = byte.TryParse(arg1, out arg1int);
                        if (Parsed)
                        {
                            foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                                if (shPlayer.username == arg2 || shPlayer.ID.ToString() == arg2.ToString())
                                    if (arg1int <= Jobs.Length && arg1int >= 0)
                                    {
                                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, String.Format(msg, arg2, Jobs[arg1int]));
                                        SetJob.Run(shPlayer, arg1int, true, false);

                                    }
                                    else
                                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>Error: The value must be between 0 and {Jobs.Length}.</color>");
                        }
                        else
                            player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, NotValidArg);
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, NotValidArg);
                }
                else
                    player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, MsgNoPerm);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}
