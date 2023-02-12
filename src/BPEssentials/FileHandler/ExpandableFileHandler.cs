using BPCoreLib.Interfaces;
using BPCoreLib.Util;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BPEssentials.FileHandler
{
    public interface IExpandableFile
    {
        string Name { get; set; }

        bool Disabled { get; set; }
    }

    public interface IExpandableFileDelayable
    {
        int Delay { get; set; }
    }

    public interface IExpandableFileHasPrice
    {
        int Price { get; set; }
    }

    public abstract class ExpandableFileHandler<JsonType> where JsonType : class, IExpandableFile, new()
    {
        protected ExpandableFileHandler()
        {
        }

        public string FileExtension { get; set; } = "json";

        public string FilesDirectory { get; set; }

        public List<JsonType> List { get; private set; } = new List<JsonType>();

        public string Name { get; set; }

        public IReader<JsonType> FileReader { get; } = new Reader<JsonType>();

        public virtual void CreateNew(JsonType obj, string fileName)
        {
            var filePath = Path.Combine(FilesDirectory, $"{fileName}.{FileExtension}");
            File.WriteAllText(filePath, JsonConvert.SerializeObject(obj, Formatting.Indented));
            List.Add(obj);
        }

        public virtual void DeleteExisting(string fileName, string name = null)
        {
            var filePath = Path.Combine(FilesDirectory, $"{fileName}.{FileExtension}");
            File.Delete(filePath);
            List = List.Where(x => x.Name != (name ?? fileName)).ToList();
        }

        public virtual void ReloadAll()
        {
            Core.Instance.Logger.LogInfo($"Loading {Name}s..");
            List.Clear();
            foreach (string file in Directory.EnumerateFiles(FilesDirectory, $"*.{FileExtension}", SearchOption.AllDirectories))
            {
                FileReader.Path = file;
                JsonType obj = FileReader.Read();
                if (List.Any(x => x.Name == obj.Name))
                {
                    Core.Instance.Logger.LogError($"Cannot add {Name} {obj.Name} because it already exists in the list!");
                    continue;
                }

                List.Add(obj);
                Core.Instance.Logger.LogInfo($"Loaded {Name}: {obj.Name}");
            }

            Core.Instance.Logger.LogInfo($"Loaded in {List.Count} {Name}(s).");
        }
    }
}
