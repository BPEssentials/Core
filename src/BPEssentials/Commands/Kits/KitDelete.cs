﻿using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.Entities;
using System.IO;
using System.Linq;

namespace BPEssentials.Commands
{
    public class KitDelete : BpeCommand
    {
        public void Invoke(ShPlayer player, string kit)
        {
            string file = Path.Combine(Paths.KitsFolder, $"{kit}.json");
            if (!File.Exists(file))
            {
                player.TS("expFileHandler_error_notFound", player.T(Core.Instance.KitHandler.Name), kit);
                player.TS("levenshteinSuggest", Core.Instance.KitHandler.List.OrderByDescending(x => LevenshteinDistance.CalculateSimilarity(x.Name, kit)).FirstOrDefault().Name);
                return;
            }

            Core.Instance.KitHandler.DeleteExisting(kit);
            player.TS("expFileHandler_deleted", player.T(Core.Instance.KitHandler.Name), kit);
        }
    }
}
