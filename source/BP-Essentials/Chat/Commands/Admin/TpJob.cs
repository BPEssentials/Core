using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BP_Essentials.Variables;

namespace BP_Essentials.Commands
{
    class TpJob
    {
        public static void Run(SvPlayer player, string message)
        {
            var arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrWhiteSpace(arg1))
            {
                player.SendChatMessage(ArgRequired);
                return;
            }
            var bosses = UnityEngine.Object.FindObjectsOfType<ShPlayer>().Where(x => x.boss || x.svPlayer.serverside);
            var boss = bosses.FirstOrDefault(x => x.ID.ToString() == arg1 || x.username == arg1 || x.username.StartsWith(arg1, StringComparison.CurrentCultureIgnoreCase) || x.job.info.jobName == arg1.ToLower() || x.job.info.jobName.StartsWith(arg1, StringComparison.CurrentCultureIgnoreCase));
            if (boss == default(ShPlayer))
            {
                player.SendChatMessage(NotFoundOnline);
                return;
            }
            player.ResetAndSavePosition(boss.svPlayer);
            player.SendChatMessage($"<color={infoColor}>Teleported to</color> <color={argColor}>{boss.username} ({boss.job.info.jobName})</color>");
        }
    }
}
