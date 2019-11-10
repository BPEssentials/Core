using System.Linq;
using System.Text;
using BPCoreLib.Interfaces;
using BPCoreLib.PlayerFactory;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.ExtendedPlayer;
using BPEssentials.Interfaces;
using BrokeProtocol.API;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Info : ICommand
    {
        public bool LastArgSpaces { get; } = true;

        public ILogger Logger { get; set; }

        public Settings Settings { get; set; }

        public ExtendedPlayerFactory<PlayerItem> PlayerFactory { get; set; }

        public void Invoke(ShPlayer player, string targetStr)
        {
            StringBuilder sb;
            if (Collections.TryGetPlayerByNameOrId(targetStr, out ShPlayer target))
            {
                sb = GetOnlineInfo(target);
            }
            else
            {
                sb = GetOfflineInfo(player, targetStr);
            }
            if (sb == null)
            {
                return;
            }

            // TODO: send as server info or CEF (pref server info to limit footprint)
        }

        // TODO: There might be a better way to do this, for example using reflection.
        private StringBuilder GetOfflineInfo(ShPlayer player, string targetStr)
        {
            if (!ulong.TryParse(targetStr, out var targetUlong))
            {
                player.SendChatMessage($"Could not parse '{targetStr}' to a ulong.");
                return null;
            }
            var target = player.manager.svManager.Database.Users.FindSingle(x => x.ID == targetUlong);
            if (target == null)
            {
                player.SendChatMessage($"No account found with the id '{targetUlong}'.");
                return null;
            }
            var sb = new StringBuilder();
            sb
            .Append("SteamID64: ").AppendLine(target.ID.ToString())
            .Append("Last Updated: ").AppendLine(target.LastUpdated.ToString())
            .Append("Join Date: ").AppendLine(target.JoinDate.ToString())

            .AppendLine("BanInfo: ")
              .Append("  - Is banned: ").AppendLine(target.BanInfo?.IsBanned.ToString())
              .Append("  - Reason: ").AppendLine(target.BanInfo?.Reason)
              .Append("  - Date: ").AppendLine(target.BanInfo?.Date.ToString())

            .AppendLine("Character:")
              .Append("  - Username: ").Append(target.Character.Username.SanitizeString()).AppendLine(" (Sanitized)")
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

              .Append("  - CustomData: ").AppendLine(string.Join("\n    - ", target.Character.CustomData.Data.Select(x => x.Key + ": " + x.Value)));

            return sb;
        }

        private StringBuilder GetOnlineInfo(ShPlayer target)
        {
            var sb = new StringBuilder();
            sb
            .Append("ID: ").AppendLine(target.ID.ToString())
            .Append("SteamID64: ").AppendLine(target.svPlayer.steamID.ToString())
            .Append("Username: ").Append(target.username.SanitizeString()).AppendLine(" (Sanitized)")
            .Append("Health: ").AppendLine(target.health.ToString())
            .Append("BankBalance: ").AppendLine(target.svPlayer.bankBalance.ToString())
            .Append("Position: ").AppendLine(target.GetPosition().ToString())
            .Append("Rotation: ").AppendLine(target.GetRotation().ToString())
            .Append("Stats: ").AppendLine(string.Join("\n    - ", target.stats))
            .Append("Expecting more info? Type '/info ").Append(target.svPlayer.steamID.ToString()).AppendLine("'.");
            return sb;
        }
    }
}
