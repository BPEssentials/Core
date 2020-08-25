using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.ExtensionMethods.Warns;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility.Networking;
using System.Linq;
using System.Text;


namespace BPEssentials.Commands
{
    public class WarnList : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target = null)
        {
            if (target != null && !player.svPlayer.HasPermission($"{Core.Instance.Info.GroupNamespace}.{nameof(WarnList)}.viewotherplayers"))
            {
                player.TS("warns_noPermission_viewOtherPlayers");
                return;
            }
            target = target ?? player;
            var warns = target.GetWarns().Select(warn => warn.ToString(player)).ToArray();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(player.T("warns_for", target.username.CleanerMessage()));
            if (warns.Length == 0)
            {
                stringBuilder.AppendLine(player.T("none"));
            }
            else
            {
                stringBuilder.AppendLine(player.T("warns_count", warns.Length.ToString()));
                stringBuilder.AppendLine();
                for (int i = 0; i < warns.Length; i++)
                {
                    stringBuilder.AppendLine($"{i + 1} - {warns[i]}");
                }
            }
            player.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.ServerInfo, stringBuilder.ToString());
        }
    }
}
