using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Chat
{
    class LangAndChatBlock : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            message = message.ToLower();
            var player = (SvPlayer)oPlayer;
            if (ChatBlock)
            {
                if (ChatBlockWords.Any(s => message.Contains(s)))
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Please don't say a blacklisted word, the message has been blocked.");
                    Debug.Log(SetTimeStamp.Run() + "[INFO] "+ player.playerData.username + " Said a word that is blocked.");
                    return true;
                }
            }
            if (LanguageBlock)
            {
                if (LanguageBlockWords.Any(s => message.Contains(s)))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "Because you are staff, your message has NOT been blocked.");
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "--------------------------------------------------------------------------------------------");
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "             ?olo ingl�s! Tu mensaje ha sido bloqueado.");
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "             Only English! Your message has been blocked.");
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "--------------------------------------------------------------------------------------------");
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
