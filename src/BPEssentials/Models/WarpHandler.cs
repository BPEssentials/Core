using BPEssentials.FileHandler;

namespace BPEssentials
{
    public class WarpHandler : ExpandableFileHandler<WarpHandler.JsonModel>
    {
        public WarpHandler()
        {
            Name = "warp";
            FileExtension = "json";
            FilesDirectory = Core.Instance.Paths.WarpsFolder;
        }
        public class JsonModel : IExpandableFile, IExpandableFileDelayable, IExpandableFileHasPrice
        {
            public string Name { get; set; }
            public bool Disabled { get; set; }
            public int Price { get; set; }
            public int Delay { get; set; }

            public Position Position { get; set; }
            public Rotation Rotation { get; set; }
        }
        public class Position
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
            public int PlaceIndex { get; set; }
        }

        public class Rotation
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
            public float W { get; set; }
        }
    }
}