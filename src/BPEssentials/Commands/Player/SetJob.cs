using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using BrokeProtocol.Required;

namespace BPEssentials.Commands
{
    public class SetJob : Command
    {
        public void Invoke(ShPlayer player, JobIndex job, ShPlayer target = null)
        {
            
            target = target ?? player;
            target.svPlayer.SvTrySetJob(job, true, false);
            player.TS("job_set", target.username.CleanerMessage(), job.ToString());
            if (target != player)
                target.TS("new_job", player.username.CleanerMessage(), job.ToString());
        }

    }
}
