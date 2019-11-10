using BPCoreLib.Interfaces;
using BPCoreLib.PlayerFactory;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.ExtendedPlayer;
using BPEssentials.Interfaces;
using BrokeProtocol.Entities;
using System.Linq;

namespace BPEssentials.Commands
{
    public class Essentials : ICommand
    {
        public bool LastArgSpaces { get; } = true;

        public ILogger Logger { get; set; }

        public Settings Settings { get; set; }

        public ExtendedPlayerFactory<PlayerItem> PlayerFactory { get; set; }

        public void Invoke(ShPlayer player, string arg1 = "")
        {
            player.SendChatMessage($"BP Essentials v{Core.Version}.");
            var info = Core.Instance.Info;
            switch (arg1)
            {
                case "web":
                    player.SendChatMessage($"Website: {info.Website}");
                    break;

                case "info":
                    player.SendChatMessage(info.ToString());
                    player.SendChatMessage($"Authors: {string.Join(", ", info.Authors.Select(x => x.ToString()))}");
                    player.SendChatMessage($"Version: {(Core.IsDevelopmentBuild() ? "[DEVELOPMENT-BUILD] " : "")}v{Core.Version}");
                    break;

                case "reload":
                    if (!player.svPlayer.HasPermission(info.GroupNamespace + ".reload"))
                    {
                        player.SendChatMessage($"No permission! Requires the '{info.GroupNamespace}.reload' permission.");
                        return;
                    }
                    player.SendChatMessage("Reloading..");
                    Core.Instance.OnReloadRequestAsync();
                    player.SendChatMessage("Reloaded.");
                    break;

                default:
                    player.SendChatMessage("Missing argument: reload/help/info/web");
                    break;
            }
        }
    }
}
