using BPCoreLib.ExtensionMethods;
using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Required;
using UnityEngine;

namespace BPEssentials.Commands
{
    public class Slap : BpeCommand
    {
        public void Invoke(ShPlayer player, ShPlayer target)
        {
            int amount = Random.Range(4, 15);
            target.svPlayer.Damage(DamageIndex.Null, amount);
            target.svPlayer.SvForce(new Vector3(500f, 0f, 500f));
            target.TS("target_slap", player.username.CleanerMessage(), amount.ToString());
            player.TS("player_slap", target.username.CleanerMessage(), amount.ToString());
        }
    }
}
