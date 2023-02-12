using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.ExtensionMethods.Warns;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPCoreLib.ExtensionMethods;
using BrokeProtocol.LiteDB;
using BrokeProtocol.Managers;

namespace BPEssentials.Commands
{
    public class WarnList : BpeCommand
    {
        public void Invoke(ShPlayer player, string target = null)
        {
            if (target != null && !player.svPlayer.HasPermission($"{Core.Instance.Info.GroupNamespace}.{nameof(WarnList)}.viewotherplayers"))
            {
                player.TS("warns_noPermission_viewOtherPlayers");
                return;
            }
            if (target == null)
            {
                SendWarnList(player, player.username, player.GetWarns());
                return;
            }
            if (EntityCollections.TryGetPlayerByNameOrID(target, out ShPlayer shTarget))
            {
                SendWarnList(player, shTarget.username, shTarget.GetWarns());
                return;
            }
            if (SvManager.Instance.TryGetUserData(target, out User user))
            {
                SendWarnList(player, user.ID, user.GetWarns());
                return;
            }

            player.TS("user_not_found", target.CleanerMessage());
        }

        public void SendWarnList(ShPlayer player, string username, List<ExtensionPlayerWarns.SerializableWarn> warnList)
        {
            string[] warns = warnList.Select(warn => warn.ToString(player)).ToArray();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(player.T("warns_for", username.CleanerMessage()));
            if (warns.Length == 0)
            {
                stringBuilder.AppendLine(player.T("none"));
            }
            else
            {
                stringBuilder.AppendLine(player.T("active_warns_count", warnList.Count(x => !x.Expired)));
                if (!Core.Instance.Settings.Warns.DeleteExpiredWarns)
                {
                    stringBuilder.AppendLine(player.T("expired_warns_count", warnList.Count(x => x.Expired)));
                }
                stringBuilder.AppendLine();
                for (int i = 0; i < warns.Length; i++)
                {
                    stringBuilder.AppendLine($"{i + 1} - {warns[i]}");
                }
            }

            player.svPlayer.SendTextMenu(player.T("warn_list_title"), stringBuilder.ToString());
        }
    }
}
