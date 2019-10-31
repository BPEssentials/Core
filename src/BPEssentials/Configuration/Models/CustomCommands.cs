using System.Collections.Generic;

namespace BPEssentials.Configuration.Models
{
    public class CustomCommand
    {
        public string Name { get; set; }

        public List<string> Commands { get; set; }

        public string Response { get; set; }
    }
}
