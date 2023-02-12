using BPCoreLib.ExtensionMethods;
using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Promote : BpeCommand
    {
        public void Invoke(ShPlayer player, ShPlayer target = null)
        {
            target = target ?? player;
            if (target.svPlayer.job.info.shared.upgrades.Length == 0) // Check if Target's Job even has Ranks
            {
                player.TS("promoted_no_ranks", target.username.CleanerMessage());
                return;
            }
            if (target.rank == target.svPlayer.job.info.shared.upgrades.Length - 1) // Check if Target is max rank
            {
                player.TS("promoted_max_rank", target.username.CleanerMessage());
                return;
            }

            target.svPlayer.Reward(target.GetMaxExperience() - target.experience + 1, 0);
            player.TS("promoted", target.username.CleanerMessage(), (target.rank + 1).ToString());
        }
    }
}
