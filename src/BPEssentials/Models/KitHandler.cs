using BPEssentials.FileHandler;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using System.Collections.Generic;

namespace BPEssentials
{
    public class KitHandler : ExpandableFileHandler<KitHandler.JsonModel>
    {
        public KitHandler()
        {
            Name = "kit";
            FileExtension = "json";
            FilesDirectory = Core.Instance.Paths.KitsFolder;
        }
        public class JsonModel : IExpandableFile, IExpandableFileDelayable, IExpandableFileHasPrice
        {
            public string Name { get; set; }
            public bool Disabled { get; set; }
            public int Price { get; set; }
            public int Delay { get; set; }
            public List<KitsItem> Items { get; set; } = new List<KitsItem>();

            public void GiveItems(ShPlayer player)
            {
                foreach (var item in Items)
                {
                    player.TransferItem(DeltaInv.AddToMe, item.Id, item.Amount, true);
                }
            }
        }
        public class KitsItem
        {
            public int Id { get; set; }
            public int Amount { get; set; }
        }
    }
}
