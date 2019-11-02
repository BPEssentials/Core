using BPCoreLib.Interfaces;
using BPCoreLib.PlayerFactory;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.ExtendedPlayer;

namespace BPEssentials.Interfaces
{
    public interface ICommand
    {
        bool LastArgSpaces { get; }

        ILogger Logger { get; set; }

        Settings Settings { get; set; }

        ExtendedPlayerFactory<PlayerItem> PlayerFactory { get; set; }
    }
}
