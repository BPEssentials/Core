using BPEssentials.FileHandler;
using BPCoreLib.Serializable;

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

            public SerializableQuaternion SerializableQuaternion { get; set; }
        }

        public class Position
        {
            public SerializableVector3 SerializableVector3 { get; set; }

            public int PlaceIndex { get; set; }
        }
    }
}