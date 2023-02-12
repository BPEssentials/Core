using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility.Jobs;
using System.Collections.Generic;
using System.Linq;
using BPCoreLib.ExtensionMethods;

namespace BPEssentials.Commands
{
    public class SetJob : BpeCommand
    {
        public void Invoke(ShPlayer player, string jobName, ShPlayer target = null)
        {
            target = target ?? player;
            JobInfo wantedJob = BPAPI.Jobs.FirstOrDefault(x => x.shared.jobName == jobName);
            if (wantedJob == null)
            {
                if (Core.Instance.Settings.Levenshtein.SetJobMode == Configuration.Models.SettingsModel.LevenshteinMode.None)
                {
                    player.TS("job_notFound", jobName);
                    return;
                }

                wantedJob = BPAPI.Jobs.OrderByDescending(x => LevenshteinDistance.CalculateSimilarity(x.shared.jobName, jobName)).FirstOrDefault();
                if (Core.Instance.Settings.Levenshtein.SetJobMode == Configuration.Models.SettingsModel.LevenshteinMode.Suggest)
                {
                    player.TS("job_notFound", jobName);
                    player.TS("levenshteinSuggest", wantedJob.shared.jobName);
                    return;
                }
            }

            target.svPlayer.SvSetJob(wantedJob, true, false);
            player.TS("job_set", target.username.CleanerMessage(), jobName);
            if (target != player)
            {
                target.TS("new_job", player.username.CleanerMessage(), wantedJob.shared.jobName);
            }
        }
    }
}
