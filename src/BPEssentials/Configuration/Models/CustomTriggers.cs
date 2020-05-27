using System.Collections.Generic;

namespace BPEssentials.Configuration.Models
{
    public class CustomTriggers
    {
        public string Trigger { get; set; }

        public string PrivateResponse { get; set; }

        public string SendInChat { get; set; }

        public List<string> Commands { get; set; }
    }
}