using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Search : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (HasPermission.Run(player, CmdSearchExecutableBy))
                {
                    string arg1 = GetArgument.Run(1, false, true, message);
                    if (String.IsNullOrEmpty(arg1))
                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
                    else
                    {
                        foreach (var shPlayer2 in FindObjectsOfType<ShPlayer>())
                            if (shPlayer2.username == arg1 && shPlayer2.IsRealPlayer() || shPlayer2.ID.ToString() == arg1 && shPlayer2.IsRealPlayer())
                            {
                                foreach (var shPlayer in FindObjectsOfType<ShPlayer>())
                                    if (shPlayer.svPlayer == player && shPlayer.IsRealPlayer())
                                    {
                                        if (!shPlayer2.IsDead())
                                        {
                                            if (shPlayer2.otherEntity)
                                                shPlayer2.svPlayer.SvStopInventory();
                                            shPlayer2.viewers.Add(shPlayer);
                                            shPlayer.otherEntity = shPlayer2;
                                            player.SendToSelf(Channel.Fragmented, 13, shPlayer.otherEntity.ID, shPlayer.otherEntity.SerializeMyItems());
                                            if (!shPlayer2.svPlayer.IsServerside() && shPlayer2.viewers.Count == 1)
                                                shPlayer2.svPlayer.SendToSelf(Channel.Reliable, 16, new System.Object[] { });
                                            player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Viewing inventory of</color> <color={argColor}>{shPlayer2.username}</color>");
                                            shPlayer2.svPlayer.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"{AdminSearchingInv}");
                                        }
                                        else
                                            player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>Player is dead.</color>");
                                    }
                            }
                    }
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
