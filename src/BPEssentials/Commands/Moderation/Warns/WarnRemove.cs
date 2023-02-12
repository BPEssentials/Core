using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.ExtensionMethods.Warns;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using System.Collections.Generic;
using BPCoreLib.ExtensionMethods;
using BrokeProtocol.LiteDB;
using BrokeProtocol.Managers;

namespace BPEssentials.Commands
{
    public class WarnRemove : BpeCommand
    {
        public void Invoke(ShPlayer player, string target, int warnId)
        {
            if (warnId < 1)
            {
                player.TS("warn_remove_error_null_or_negative", warnId.ToString());
                return;
            }
            if (EntityCollections.TryGetPlayerByNameOrID(target, out ShPlayer shTarget))
            {
                if (CheckWarnCount(player, warnId, shTarget.GetWarns()))
                {
                    return;
                }

                shTarget.RemoveWarn(warnId - 1);
                return;
            }
            if (SvManager.Instance.TryGetUserData(target, out User user))
            {
                if (CheckWarnCount(player, warnId, user.GetWarns()))
                {
                    return;
                }

                user.RemoveWarn(warnId - 1);
                SvManager.Instance.database.Users.Upsert(user);
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
