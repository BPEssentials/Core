using BPCoreLib.Interfaces;
using BPCoreLib.PlayerFactory;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.ExtendedPlayer;
using BPEssentials.Interfaces;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Promote : ICommand
    {
        public bool LastArgSpaces { get; }

        public ILogger Logger { get; set; }

        public Settings Settings { get; set; }

        public ExtendedPlayerFactory<PlayerItem> PlayerFactory { get; set; }

        public void Invoke(ShPlayer player, ShPlayer target = null)
        {
            target = target ?? player;
            if (target.job.info.rankItems.Length == 0) // Check if Target's Job even has Ranks
            {
                player.SendChatMessage($"{target.username.SanitizeString()}'s Job does not have ranks!");
                return;
            }
            if (target.rank == target.job.info.rankItems.Length - 1) // Check if Target is max rank
            {
                player.SendChatMessage($"{target.username.SanitizeString()} already has the highest rank!");
                return;
            }
            target.svPlayer.Reward(target.maxExperience - target.experience + 1, 0);
            player.SendChatMessage($"Promoted {target.username.SanitizeString()} to rank {target.rank + 1}.");
        }
    }
}
