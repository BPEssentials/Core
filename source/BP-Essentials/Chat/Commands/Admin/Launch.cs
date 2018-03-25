using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Launch : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (HasPermission.Run(player, CmdLaunchExecutableBy))
                {
                    string arg1 = GetArgument.Run(1, false, true, message);
                    if (String.IsNullOrEmpty(arg1))
                        player.SendToSelf(Channel.Unsequenced, 10, ArgRequired);
                    else
                    {
                        bool playerfound = false;
                        foreach (var shPlayer in FindObjectsOfType<ShPlayer>())
                            if (shPlayer.username == arg1 && shPlayer.IsRealPlayer() || shPlayer.ID.ToString() == arg1 && shPlayer.IsRealPlayer())
                            {
                                shPlayer.svPlayer.SvForce(new Vector3(0f, 6500f, 0f));
                                shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, 10, $"<color={warningColor}>Off you go!</color>");
                                player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>You've launched </color><color={argColor}>{shPlayer.username}</color><color={infoColor}> into space!</color>");
                                playerfound = true;
                            }
                        if (!playerfound)
                            player.SendToSelf(Channel.Reliable, 10, NotFoundOnline);

                    }
                }
                else
                    player.SendToSelf(Channel.Unsequenced, 10, MsgNoPerm);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}
