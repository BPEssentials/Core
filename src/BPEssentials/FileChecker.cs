using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BPEssentials
{
    public static class FileChecker
    {
        public static Dictionary<string, string> RequiredDirectories { get; }= new Dictionary<string, string>
        {
            {"Essentials", Paths.EssentialsFolder},
            {"Warps", Core.Instance.Paths.KitsFolder},
            {"Kits", Core.Instance.Paths.WarpsFolder}
        };

        public static Dictionary<string, string> RequiredFiles { get; } = new Dictionary<string, string>
        {
            {"settings.json", Core.Instance.Paths.SettingsFile},
            {"CustomCommands.json", Core.Instance.Paths.CustomCommandsFile},
            {"localization.json", Core.Instance.Paths.LocalizationFile}
        };

        public static HttpClient Client { get; } = new HttpClient();

        public static async Task CheckFiles()
        {
            foreach (var directory in RequiredDirectories)
            {
                if (Directory.Exists(directory.Value))
                {
                    continue;
                }
                Core.Instance.Logger.LogWarning($"{directory.Key} directory was not found; creating");
                Directory.CreateDirectory(directory.Value);
                Core.Instance.Logger.LogInfo($"{directory.Key} directory was created.");
            }

            foreach (var file in RequiredFiles)
            {
                if (File.Exists(file.Value))
                {
                    continue;
                }
                try
                {
                    Core.Instance.Logger.LogWarning($"{file.Key} was not found; downloading.");
                    var content = await Client.GetStringAsync($"{Core.Instance.Info.Git}/raw/{(Core.IsDevelopmentBuild() ? "master" : Core.Version)}/dist/{file.Key}");
                    File.WriteAllText(file.Value, content);
                    Core.Instance.Logger.LogInfo($"{file.Key} was downloaded.");
                }
                catch (HttpRequestException e)
                {
                    Core.Instance.Logger.LogError($"{file.Key} could not be donwloaded because of: {e.Message}.");
                }
            }
        }
    }
}
