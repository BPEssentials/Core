using BPEssentials.ExtensionMethods;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Required;
using UnityEngine;

namespace BPEssentials.RegisteredEvents
{
    public class OnDamge : IScript
    {
        [Target(GameSourceEvent.PlayerDamage, ExecutionMode.Event)]
        public void OnDamage(ShPlayer player, DamageIndex damageIndex, float amount, ShPlayer attacker, Collider collider, float hitY)
        {
            if (player.svPlayer.godMode)
            {
                if (attacker)
                {
                    player.TS("god_damage_blocked", amount, attacker.ID, attacker.username.CleanerMessage());
                }
                else
                {
                    player.TS("god_damage_blocked_minimal", amount);
                }
            }
        }
    }
}
