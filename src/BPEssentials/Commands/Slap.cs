using BPCoreLib.Interfaces;
using BPCoreLib.PlayerFactory;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.ExtendedPlayer;
using BPEssentials.Interfaces;
using BrokeProtocol;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility.Networking;
using System.Linq;

namespace BPEssentials.Commands
{
    public class Slap : ICommand
    {
        public bool LastArgSpaces { get; }

        public ILogger Logger { get; set; }

        public Settings Settings { get; set; }

        public ExtendedPlayerFactory<PlayerItem> PlayerFactory { get; set; }

        public void Invoke(ShPlayer player, ShPlayer target)
        {
            int amount = new System.Random().Next(4, 15);
            target.svPlayer.Damage(DamageIndex.Null, amount, null, null);
            target.svPlayer.SvForce(new UnityEngine.Vector3(500f, 0f, 500f));
            target.SendChatMessage($"You got slapped by {player.username.SanitizeString()}! [-{amount} HP]");
            player.SendChatMessage($"You've slapped {target.username.SanitizeString()}. [-{amount} HP]");
        }
    }
}
