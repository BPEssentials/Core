using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.ExtensionMethods.Warns;
using BrokeProtocol.Collections;
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
            if (target != null && !player.svPlayer.HasPermission($"{Core.Instance.Info.GroupNamespace}.{nameof(WarnList)}.viewotherplayers"))
            {
                player.TS("warns_noPermission_viewOtherPlayers");
                return;
            }

            if (EntityCollections.TryGetPlayerByNameOrID(target, out var shTarget))
            {
                SendWarnList(player, shTarget.username, shTarget.GetWarns());
                return;
            }

            if (Core.Instance.SvManager.TryGetUserData(target, out var user))
            {
                SendWarnList(player, user.ID, user.GetWarns());
                return;
            }

            player.TS("user_not_found", target.CleanerMessage());

        }

        public void SendWarnList(ShPlayer player, string username, List<SerializableWarn> warnList)
        {
            var warns = warnList.Select(warn => warn.ToString(player)).ToArray();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(player.T("warns_for", username.CleanerMessage()));
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
            player.svPlayer.SendTextPanel(player.T("warn_list_title"), stringBuilder.ToString());
        }
    }
}
