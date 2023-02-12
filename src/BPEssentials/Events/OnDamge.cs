using BPCoreLib.ExtensionMethods;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Required;
using UnityEngine;

namespace BPEssentials.Events
{
    public class OnDamage : IScript
    {
        [Target(GameSourceEvent.PlayerDamage, ExecutionMode.PreEvent)]
        public void OnEvent(ShDestroyable destroyable, DamageIndex damageIndex, float amount, ShPlayer attacker, Collider collider, Vector3 source, Vector3 hitPoint)
        {
            ShPlayer player = destroyable.Player;
            if (!player.svPlayer.godMode)
            {
                return;
            }

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
