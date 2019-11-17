using BPEssentials.FileHandler;
using System.Collections.Generic;

namespace BPEssentials
{
    public class KitsHandler : ExpandableFileHandler<KitsHandler.JsonModel>
    {
        public KitsHandler()
        {
            Name = "kit";
            FileExtension = "json";
            FilesDirectory = Core.Instance.Paths.KitsFolder;
        }
        public class JsonModel : IExpandableFile, IExpandableFileDelayable, IExpandableFileHasPrice
        {
            public string Name { get; set; }
            public string ExecutableBy { get; set; }
            public bool Disabled { get; set; }
            public int Price { get; set; }
            public int Delay { get; set; }
            public List<Kits_Item> Items { get; set; } = new List<Kits_Item>();
        }
        public class Kits_Item
        {
            public int Id { get; set; }
            public int Amount { get; set; }
        }
    }
}