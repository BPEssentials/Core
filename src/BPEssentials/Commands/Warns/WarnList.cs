using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.ExtensionMethods.Warns;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility.Networking;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static BPEssentials.ExtensionMethods.Warns.ExtensionPlayerWarns;

namespace BPEssentials.Commands
{
    public class WarnList : Command
    {
        public void Invoke(ShPlayer player, string target = null)
        {
            if (target != null && !player.HasPermission($"{Core.Instance.Info.GroupNamespace}.{nameof(WarnList)}.viewotherplayers"))
            {
                player.TS("warns_noPermission_viewOtherPlayers");
                return;
            }
            ShPlayer shTarget;
            if (target == null)
            {
                shTarget = player;
            }
            else
            {
                shTarget = Core.Instance.SvManager.connectedPlayers.FirstOrDefault(x => x.Value.accountID.Equals(target) || x.Value.ID.Equals(int.Parse(target))).Value;
            }
            StringBuilder stringBuilder = new StringBuilder();
            List<SerializableWarn> warns;
            if (shTarget != null)
            {
                warns = shTarget.GetWarns();
                stringBuilder.AppendLine(player.T("warns_for", shTarget.username.CleanerMessage()));
            }
            else
            {
                if (!Core.Instance.SvManager.TryGetUserData(target, out var user))
                {
                    player.TS("user_not_found", target.CleanerMessage());
                    return;
                }
                if (!user.Character.CustomData.TryFetchCustomData(CustomDataKey, out warns))
                {
                    warns = new List<SerializableWarn>();
                }
                stringBuilder.AppendLine(player.T("warns_for", user.Character.Username.CleanerMessage()));
            }
            if (warns.Count == 0)
            {
                stringBuilder.AppendLine(player.T("none"));
            }
            else
            {
                stringBuilder.AppendLine(player.T("warns_count", warns.Count.ToString()));
                stringBuilder.AppendLine();
                for (int i = 0; i < warns.Count; i++)
                {
                    stringBuilder.AppendLine($"{i + 1} - {warns[i]}");
                }
            }
            player.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.ServerInfo, stringBuilder.ToString());
        }
    }
}