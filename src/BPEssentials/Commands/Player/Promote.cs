using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Promote : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target = null)
        {
            target = target ?? player;
            if (target.job.info.rankItems.Length == 0) // Check if Target's Job even has Ranks
            {
                player.TS("promoted_no_ranks", target.username.SanitizeString());
                return;
            }
            if (target.rank == target.job.info.rankItems.Length - 1) // Check if Target is max rank
            {
                player.TS("promoted_max_rank", target.username.SanitizeString());
                return;
            }
            target.svPlayer.Reward(target.maxExperience - target.experience + 1, 0);
            player.TS("promoted", target.username.SanitizeString(), (target.rank + 1).ToString());
        }
    }
}
