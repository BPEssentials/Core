using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static BP_Essentials.Variables;

namespace BP_Essentials.Methods.FileHandler
{
	public interface IExpandableFile
	{
		string Name { get; set; }
		string ExecutableBy { get; set; }
		bool Disabled { get; set; }
	}
	public interface IExpandableFileDelayable
	{
		int Delay { get; set; }
		Dictionary<string, int> CurrentlyInCooldown { get; set; }
	}
	public interface IExpandableFileHasPrice
	{
		int Price { get; set; }
	}
	public abstract class ExpandableFileHandler<JsonType> where JsonType : class, IExpandableFile, new()
	{
		protected ExpandableFileHandler()
		{
			if (IsInitialized)
				return;
			StartTimer();
			IsInitialized = true;
		}
		public string FileExtension { get; set; } = "json";
		public string FilesDirectory { get; set; }
		public bool IsInitialized { get; private set; }
		public List<JsonType> List { get; private set; } = new List<JsonType>();
		public string Name { get; set; }
		public System.Timers.Timer Timer { get; private set; } = new System.Timers.Timer();

		public virtual void CreateNew(JsonType obj, string fileName)
		{
			var filePath = Path.Combine(FilesDirectory, $"{fileName}.{FileExtension}");
			File.WriteAllText(filePath, JsonConvert.SerializeObject(obj, Formatting.Indented));
			List.Add(obj);
		}
		public virtual void DeleteExisting(string fileName, string name = null)
		{
			var filePath = Path.Combine(FileDirectory, $"{fileName}.{FileExtension}");
			File.Delete(filePath);
			List = List.Where(x => x.Name != (name ?? fileName)).ToList();
		}
		public virtual void LoadAll(bool initCooldown = false)
		{
			if (DebugLevel >= 1)
				Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] Loading {Name}s..");
			List.Clear();
			foreach (string file in Directory.EnumerateFiles(FilesDirectory, $"*.{FileExtension}", SearchOption.AllDirectories))
			{
				var obj = JsonConvert.DeserializeObject<JsonType>(FilterComments.Run(file));
				if (List.Any(x => x.Name == obj.Name))
				{
					Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [ERROR] Cannot add {Name} {obj.Name} because it already exists in the list!");
					continue;
				}
				List.Add(obj);
				if (initCooldown)
					SetupDelayable(obj);
				if (DebugLevel >= 1)
					Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] Loaded {Name}: {obj.Name}");
			}
			if (DebugLevel >= 1)
				Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] Loaded in {List.Count} {Name}(s).");
		}
		public virtual IEnumerator StartCooldown(string username, JsonType obj, int _passedTime = 0)
		{
			if (!(obj is IExpandableFileDelayable objDelayable))
				yield break;
			if (!objDelayable.CurrentlyInCooldown.ContainsKey(username))
				objDelayable.CurrentlyInCooldown.Add(username, objDelayable.Delay);
			var path = Path.Combine(WarpDirectory, $"{obj.Name}.json");
			var passedTime = _passedTime;
			while (passedTime < objDelayable.Delay)
			{
				++passedTime;
				--objDelayable.CurrentlyInCooldown[username];
				yield return new WaitForSecondsRealtime(1);
			}
			if (objDelayable.CurrentlyInCooldown.ContainsKey(username))
				objDelayable.CurrentlyInCooldown.Remove(username);
			if (File.Exists(path))
				File.WriteAllText(path, JsonConvert.SerializeObject(obj, Formatting.Indented));
		}

		void SetupDelayable(JsonType obj)
		{
			if (!(obj is IExpandableFileDelayable objDelayable))
				return;
			foreach (var player in objDelayable.CurrentlyInCooldown.ToList())
			{
				if (player.Value <= 0)
					continue;
				SvMan.StartCoroutine(StartCooldown(player.Key, obj, player.Value));
			}
		}
		void StartTimer()
		{
			try
			{
				Timer.Elapsed += (sender, e) =>
				{
					foreach (var item in List)
					{
						var path = Path.Combine(FilesDirectory, $"{item.Name}.json");
						if (File.Exists(path))
							File.WriteAllText(path, JsonConvert.SerializeObject(item, Formatting.Indented));
					}
				};
				Timer.Interval = 10 * 60 * 1000; // Save every 10 minutes
				Timer.Enabled = true;
			}
			catch (Exception ex)
			{
				ErrorLogging.Run(ex);
			}
		}
	}
}
