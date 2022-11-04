using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility.Jobs;
using System.Collections.Generic;
using System.Linq;

namespace BPEssentials.Commands
{
    public class SetJob : BpeCommand
    {
        public void Invoke(ShPlayer player, string jobName, ShPlayer target = null)
        {
            target = target ?? player;
            var jobs = new Dictionary<string, JobInfo>();
            foreach (var jobType in BPAPI.Jobs)
            {
                jobs.Add(jobType.shared.jobName, jobType);
            }

            var wantedJob = jobs.FirstOrDefault(x => x.Key == jobName);
            if (wantedJob.Key == null)
            {
                if (Core.Instance.Settings.Levenshtein.SetJobMode == Configuration.Models.SettingsModel.LevenshteinMode.None)
                {
                    player.TS("job_notFound", jobName);
                    return;
                }
                wantedJob = jobs.OrderByDescending(x => LevenshteinDistance.CalculateSimilarity(x.Key, jobName)).FirstOrDefault();

                if (Core.Instance.Settings.Levenshtein.SetJobMode == Configuration.Models.SettingsModel.LevenshteinMode.Suggest)
                {
                    player.TS("job_notFound", jobName);
                    player.TS("levenshteinSuggest", wantedJob.Key);
                    return;
                }
            }
            target.svPlayer.SvSetJob(wantedJob.Value, true, false);
            player.TS("job_set", target.username.CleanerMessage(), jobName);
            if (target != player)
            {
                target.TS("new_job", player.username.CleanerMessage(), wantedJob.Key);
            }
        }
    }
}
