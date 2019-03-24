using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BP_Essentials.Variables;

namespace BP_Essentials.Commands
{
    class Me
    {
        public static void Run(SvPlayer player, string message)
        {
            var arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrWhiteSpace(arg1))
            {
                player.SendChatMessage(ArgRequired);
                return;
            }
            player.Send(LocalChatMe ? SvSendType.Local : SvSendType.All, Channel.Unsequenced, ClPacket.GameMessage, PlaceholderParser.ParseUserMessage(player.player, MeMessage, arg1));
        }
    }
}
