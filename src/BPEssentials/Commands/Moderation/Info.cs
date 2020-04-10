using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using BrokeProtocol.Server.LiteDB.Models;
using BrokeProtocol.Utility.Networking;
using System.Linq;
using System.Text;


namespace BPEssentials.Commands
{
    public class Info : Command
    {
        public override bool LastArgSpaces => true;

        public void Invoke(ShPlayer player, string targetStr)
        {
            StringBuilder sb;
            var target = Core.Instance.SvManager.database.Users.FindById(targetStr);
            if (target != null)
            {
                sb = GetOfflineInfo(target);
            }else if(EntityCollections.TryGetPlayerByNameOrID(targetStr, out ShPlayer shPlayer))
            {
                sb = GetOnlineInfo(shPlayer);
            }else
            {
                player.SendChatMessage($"No account found with the id '{targetStr}'.");
                return;
            }
            player.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.ServerInfo, sb.ToString());
        }

        // TODO: Add i18n for this
        // TODO: There might be a better way to do this, for example using reflection.
        private StringBuilder GetOfflineInfo(User target)
        {
            var sb = new StringBuilder();
            sb
            .Append("accountID64: ").AppendLine(target.ID.ToString())
            .Append("Last Updated: ").AppendLine(target.LastUpdated.ToString())
            .Append("Join Date: ").AppendLine(target.JoinDate.ToString())

            .AppendLine("BanInfo: ")
              .Append("  - Is banned: ").AppendLine(target.BanInfo?.IsBanned.ToString())
              .Append("  - Reason: ").AppendLine(target.BanInfo?.Reason)
              .Append("  - Date: ").AppendLine(target.BanInfo?.Date.ToString())

            .AppendLine("Character:")
              .Append("  - Username: ").Append(target.Character.Username.CleanerMessage()).AppendLine(" (Sanitized)")
              .Append("  - Health: ").AppendLine(target.Character.Health.ToString())
              .Append("  - BankBalance: ").AppendLine(target.Character.BankBalance.ToString())
              .Append("  - Position: ").AppendLine(target.Character.Position.ToString())
              .Append("  - Rotation: ").AppendLine(target.Character.Rotation.ToString())
              .Append("  - Stats: ").AppendLine(string.Join("\n    - ", target.Character.Stats))

              .Append("  - EquipableIndex: ").AppendLine(target.Character.EquipableIndex.ToString())
              .Append("  - PlaceIndex: ").AppendLine(target.Character.PlaceIndex.ToString())
              .Append("  - SkinIndex: ").AppendLine(target.Character.SkinIndex.ToString())

              .Append("  - JailTime: ").AppendLine(target.Character.JailTime.ToString())
              .Append("  - MapName: ").AppendLine(target.Character.MapName)

              .Append("  - Apartments: ").Append(target.Character.Apartments.Count).Append(", Indexes: ").Append(string.Join(", ", target.Character.Apartments.Select(x => x.Index)))
              .Append("  - Wearables: ").AppendLine(string.Join("\n    - ", target.Character.Wearables))
              .Append("  - Items: ").AppendLine(string.Join("\n    - ", target.Character.Items.Select(x => x.Key + ": " + x.Value)))

              .AppendLine("  - Job: ")
                .Append("    - Index: ").AppendLine(target.Character.Job.Index.ToString())
                .Append("    - Rank: ").AppendLine(target.Character.Job.Rank.ToString())
                .Append("    - Experience: ").AppendLine(target.Character.Job.Experience.ToString())

              .Append("  - CustomData: ").AppendLine(string.Join("\n    - ", target.Character.CustomData.Data.Select(x => x.Key + ": " + Newtonsoft.Json.JsonConvert.SerializeObject(x.Value))));

            return sb;
        }

        private StringBuilder GetOnlineInfo(ShPlayer target)
        {
            var sb = new StringBuilder();
            sb
            .Append("ID: ").AppendLine(target.ID.ToString())
            .Append("accountID64: ").AppendLine(target.accountID.ToString())
            .Append("Username: ").Append(target.username.CleanerMessage()).AppendLine(" (Sanitized)")
            .Append("Health: ").AppendLine(target.health.ToString())
            .Append("BankBalance: ").AppendLine(target.svPlayer.bankBalance.ToString())
            .Append("Position: ").AppendLine(target.GetPosition.ToString())
            .Append("Rotation: ").AppendLine(target.GetRotation.ToString())
            .Append("Stats: ").AppendLine(string.Join("\n    - ", target.stats))
            .Append("Expecting more info? Type '/info ").Append(target.accountID.ToString()).AppendLine("'.");
            return sb;
        }
    }
}
