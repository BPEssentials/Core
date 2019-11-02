using BPCoreLib.Interfaces;
using BPCoreLib.PlayerFactory;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.Interfaces;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility.Networking;
using System.Linq;

namespace BPEssentials.Commands
{
    public class Restrain : ICommand
    {
        public bool LastArgSpaces { get; }

        public ILogger Logger { get; set; }

        public Settings Settings { get; set; }

        public ExtendedPlayerFactory PlayerFactory { get; set; }

        public void Invoke(ShPlayer player, ShPlayer target)
        {
            target.svPlayer.Restrain(target.manager.handcuffed);
            var shRetained = target.curEquipable as ShRestrained;
            target.svPlayer.SvSetEquipable(shRetained.otherRestrained.index);
            target.SendChatMessage("You've been restrained");
            player.SendChatMessage($"Restrained {target.username.SanitizeString()}.");

        }
    }
}
