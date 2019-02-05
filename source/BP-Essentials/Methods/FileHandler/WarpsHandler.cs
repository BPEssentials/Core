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
	public class WarpHandler : ExpandableFileHandler<WarpHandler.JsonModel>
	{
		public WarpHandler()
		{
			Name = "warp";
			FileExtension = "json";
			FilesDirectory = WarpDirectory;
		}
		public class JsonModel : IExpandableFile, IExpandableFileDelayable, IExpandableFileHasPrice
		{
			public string Name { get; set; }
			public string ExecutableBy { get; set; }
			public bool Disabled { get; set; }
			public int Price { get; set; }
			public int Delay { get; set; }
			public Dictionary<string, int> CurrentlyInCooldown { get; set; } = new Dictionary<string, int>();

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
