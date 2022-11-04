using BPEssentials.ExtensionMethods;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Required;
using UnityEngine;

namespace BPEssentials.RegisteredEvents
{
    public class OnDamge : IScript
    {
        [Target(GameSourceEvent.PlayerDamage, ExecutionMode.PreEvent)]
        public void OnDamage(ShDestroyable destroyable, DamageIndex damageIndex, float amount, ShPlayer attacker, Collider collider, Vector3 source, Vector3 hitPoint)
        {
            var player = destroyable.Player;
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
