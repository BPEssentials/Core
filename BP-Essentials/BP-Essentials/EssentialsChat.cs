using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Threading;
using UnityEngine;
using static BP_Essentials.EssentialsConfigPlugin;
using static BP_Essentials.EssentialsCmdPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using static BP_Essentials.EssentialsVariablesPlugin;
namespace BP_Essentials
{
    public class EssentialsChatPlugin : EssentialsCorePlugin{
        #region Event: ChatMessage
        //Chat Events
        [Hook("SvPlayer.SvGlobalChatMessage")]
        public static bool SvGlobalChatMessage(SvPlayer player, ref string message)
        {
            //Message Logging
            if (!(MutePlayers.Contains(player.playerData.username)))
            {
                MessageLog(message, player);
            }
            
            
            //AFK Handling
            if (AfkPlayers.Contains(player.playerData.username))
            {
                Commands.Afk.Run(player,message);
            }
            
            
            
            if (message.StartsWith(CmdClearChat) || message.StartsWith(CmdClearChat2))
                return Commands.ClearChat.Run(player, message);
            else if (message.StartsWith(CmdAfk) || message.StartsWith(CmdAfk2))
                return Commands.Afk.Run(player, message);
            else if (message.StartsWith("/essentials") || message.StartsWith("/ess"))
                return Commands.Essentials.Run(player, message);
            else if (message.StartsWith(CmdDbug))
                return Commands.DebugCommands.Run(player, message);
            else if (message.StartsWith(CmdGodmode) || message.StartsWith(CmdGodmode2))
                return Commands.GodMode.Run(player, message);
            else if (message.StartsWith(CmdPay) || message.StartsWith(CmdPay2))
                return Commands.Pay.Run(player, message);
            else if (message.StartsWith(CmdSave))
                return Commands.Save.Run(player, message);
            else if (message.StartsWith(CmdTpHere) || message.StartsWith(CmdTpHere2))
                return Commands.Tp.TpHere(player, message);
            else if (message.StartsWith(CmdTp))
                return Commands.Tp.Run(player, message);
            else if (message.StartsWith(CmdBan))
                return Commands.Ban.Run(player, message);
                
            return false;
        }

        #endregion
    }
}