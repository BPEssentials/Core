using System;
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
            if (message.StartsWith(CmdClearChat) || message.StartsWith(CmdClearChat2))
                return Commands.ClearChat.Run(player, message);
            return false;
        }

        #endregion
    }
}