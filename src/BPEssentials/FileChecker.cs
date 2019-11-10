using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BPEssentials
{
    public static class FileChecker
    {
        public static Dictionary<string, string> RequiredFiles { get; } = new Dictionary<string, string>
        {
            {"settings.json", Core.Instance.Paths.SettingsFile},
            {"CustomCommands.json", Core.Instance.Paths.CustomCommandsFile}
        };

        public static HttpClient Client { get; } = new HttpClient();

        public static async Task CheckFiles()
        {
            if (!Directory.Exists(Paths.EssentialsFolder))
            {
                Core.Instance.Logger.LogWarning("Essentials Folder not found.");
                Directory.CreateDirectory(Paths.EssentialsFolder);
                Core.Instance.Logger.LogInfo("Created Essentials Folder.");
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
                    var content = await Client.GetStringAsync($"{Core.Instance.Info.Github}/raw/{(Core.IsDevelopmentBuild() ? "development" : "master")}/dist/{file.Key}");
                    File.WriteAllText(file.Value, content);
                    Core.Instance.Logger.Log($"{file.Key} was downloaded.");
                }
                catch (HttpRequestException e)
                {
                    Core.Instance.Logger.LogError($"{file.Key} could not be donwloaded because of: {e.Message}.");
                }
            }
        }
    }
}
