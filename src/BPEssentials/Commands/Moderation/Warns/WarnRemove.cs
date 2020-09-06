using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.ExtensionMethods.Warns;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using System.Collections.Generic;

namespace BPEssentials.Commands
{
    public class WarnRemove : Command
    {
        public void Invoke(ShPlayer player, string target, int warnId)
        {
            if (warnId < 1)
            {
                player.TS("warn_remove_error_null_or_negative", warnId.ToString());
                return;
            }

            if (EntityCollections.TryGetPlayerByNameOrID(target, out var shTarget))
            {
                if (CheckWarnCount(player, warnId, shTarget.GetWarns()))
                {
                    return;
                }
                shTarget.RemoveWarn(warnId - 1);
                return;
            }

            if (Core.Instance.SvManager.TryGetUserData(target, out var user))
            {
                if (CheckWarnCount(player, warnId, user.GetWarns()))
                {
                    return;
                }
                user.RemoveWarn(warnId - 1);
                return;
            }

            player.TS("user_not_found", target.CleanerMessage());
        }

        private bool CheckWarnCount(ShPlayer player, int warnId, List<ExtensionPlayerWarns.SerializableWarn> warns)
        {
            if (warns.Count < warnId)
            {
                player.TS("warn_remove_error_notExistent", warnId.ToString());
                return true;
            }
            player.TS("player_warn_removed", warns[warnId - 1].ToString(player));
            return false;
        }
    }
}
