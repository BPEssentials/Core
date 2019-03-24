using BP_Essentials.Methods.FileHandler;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;

namespace BP_Essentials
{
    public class KitsHandler : ExpandableFileHandler<KitsHandler.JsonModel>
    {
        public KitsHandler()
        {
            Name = "kit";
            FileExtension = "json";
            FilesDirectory = KitDirectory;
        }
        public class JsonModel : IExpandableFile, IExpandableFileDelayable, IExpandableFileHasPrice
        {
            public string Name { get; set; }
            public string ExecutableBy { get; set; }
            public bool Disabled { get; set; }
            public int Price { get; set; }
            public int Delay { get; set; }
            public List<Kits_Item> Items { get; set; } = new List<Kits_Item>();
            public Dictionary<string, int> CurrentlyInCooldown { get; set; } = new Dictionary<string, int>();
        }
        public class Kits_Item
        {
            public int Id { get; set; }
            public int Amount { get; set; }
        }
    }
}
