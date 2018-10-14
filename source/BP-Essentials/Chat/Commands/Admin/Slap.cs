using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Slap
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (String.IsNullOrEmpty(arg1))
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
            else
            {
                bool playerfound = false;
                foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                    if (shPlayer.username == arg1 && !shPlayer.svPlayer.IsServerside() || shPlayer.ID.ToString() == arg1 && !shPlayer.svPlayer.IsServerside())
                    {
                        foreach (var shPlayer2 in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                            if (shPlayer2.svPlayer == player && !shPlayer2.svPlayer.IsServerside())
                            {
                                int amount = new System.Random().Next(4, 15);
                                shPlayer.svPlayer.Damage(DamageIndex.Null, amount, null, null);
                                shPlayer.svPlayer.SvForce(new Vector3(500f, 0f, 500f));
                                shPlayer.svPlayer.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={warningColor}>You got slapped by </color><color={argColor}>{shPlayer2.username}</color><color={warningColor}>! [-{amount} HP]</color>");
                                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>You've slapped </color><color={argColor}>{shPlayer.username}</color><color={infoColor}>. [-{amount} HP]</color>");
                                playerfound = true;
                            }
                    }
                if (!playerfound)
                    player.Send(SvSendType.Self, Channel.Reliable, ClPacket.GameMessage, NotFoundOnline);

            }
        }
    }
}
