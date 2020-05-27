using System.Collections.Generic;

namespace BPEssentials.Configuration.Models
{
    public class WordBlacklist
    {
        public List<string> AutoWarn { get; set; }

        public List<string> AutoKick { get; set; }

        public List<string> AutoBan { get; set; }
    }
}