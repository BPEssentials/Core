using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.Entities;
using System.IO;
using System.Linq;

namespace BPEssentials.Commands
{
    public class WarpDelete : BpeCommand
    {
        public void Invoke(ShPlayer player, string warp)
        {

            var file = Path.Combine(Core.Instance.Paths.WarpsFolder, $"{warp}.json");

            if (!File.Exists(file))
            {
                player.TS("expFileHandler_error_notFound", player.T(Core.Instance.WarpHandler.Name), warp);
                player.TS("levenshteinSuggest", Core.Instance.WarpHandler.List.OrderByDescending(x => LevenshteinDistance.CalculateSimilarity(x.Name, warp)).FirstOrDefault().Name);
                return;
            }


            Core.Instance.WarpHandler.DeleteExisting(warp);
            player.TS("expFileHandler_deleted", player.T(Core.Instance.WarpHandler.Name), warp);
        }
    }
}
