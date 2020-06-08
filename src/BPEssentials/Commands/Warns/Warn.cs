using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.ExtensionMethods.Warns;
using BPEssentials.Utils;
using BrokeProtocol.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using static BPEssentials.ExtensionMethods.Warns.ExtensionPlayerWarns;

namespace BPEssentials.Commands
{
    public class Warn : Command
    {
        public override bool LastArgSpaces { get; } = true;

        public void Invoke(ShPlayer player, string target, string reason, int length = -1)
        {
            var shTarget = Core.Instance.SvManager.connectedPlayers.FirstOrDefault(x => x.Value.accountID.Equals(target) || x.Value.ID.Equals(int.Parse(target))).Value;
            if (shTarget != null)
            {
                shTarget.AddWarn(player, reason, length);
                ChatUtils.SendToAllEnabledChatT("all_warned", player.username.CleanerMessage(), shTarget.username.CleanerMessage(), reason.CleanerMessage());
                player.TS("player_warn", shTarget.username.CleanerMessage(), reason.CleanerMessage());
                shTarget.TS("target_warn", shTarget.username.CleanerMessage(), reason.CleanerMessage());
                return;
            }
            if (Core.Instance.SvManager.TryGetUserData(target, out var user))
            {
                if (!user.Character.CustomData.TryFetchCustomData<List<SerializableWarn>>(CustomDataKey, out var warns))
                {
                    warns = new List<SerializableWarn>();
                }
                warns.Add(new SerializableWarn(player.accountID, reason, DateTime.Now, length));
                user.Character.CustomData.AddOrUpdate(CustomDataKey, warns);
                ChatUtils.SendToAllEnabledChatT("all_warned", player.username.CleanerMessage(), target.CleanerMessage(), reason.CleanerMessage());
                player.TS("player_warn", target.CleanerMessage(), reason.CleanerMessage());
                shTarget.TS("target_warn", target.CleanerMessage(), reason.CleanerMessage());
                return;
            }
            player.TS("user_not_found", target.CleanerMessage());
        }
    }
}