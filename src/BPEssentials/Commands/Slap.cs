using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Slap : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target)
        {
            var amount = new System.Random().Next(4, 15);
            target.svPlayer.Damage(DamageIndex.Null, amount, null, null);
            target.svPlayer.SvForce(new UnityEngine.Vector3(500f, 0f, 500f));
            target.TS("target_slap", player.username.SanitizeString(), amount.ToString());
            player.TS("player_slap", player.username.SanitizeString(), amount.ToString());
        }
    }
}
