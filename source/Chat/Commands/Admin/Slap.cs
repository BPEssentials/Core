using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Slap : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (AdminsListPlayers.Contains(player.playerData.username) && CmdSlapExecutableBy == "admins" || CmdSlapExecutableBy == "everyone")
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
                                foreach (var shPlayer2 in FindObjectsOfType<ShPlayer>())
                                    if (shPlayer2.svPlayer == player && shPlayer2.IsRealPlayer())
                                    {
                                        int amount = new System.Random().Next(4, 15);
                                        shPlayer.svPlayer.Damage(DamageIndex.Null, amount, null, null);
                                        shPlayer.svPlayer.SvForce(new Vector3(500f, 0f, 500f));
                                        shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, 10, $"<color={warningColor}>You got slapped by </color><color={argColor}>{shPlayer2.username}</color><color={warningColor}>! [-{amount} HP]</color>");
                                        player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>You've slapped </color><color={argColor}>{shPlayer.username}</color><color={infoColor}>. [-{amount} HP]</color>");
                                        playerfound = true;
                                    }
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
