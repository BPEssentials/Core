using BPEssentials.ExtensionMethods;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Required;
using BrokeProtocol.Utility;
using BrokeProtocol.Utility.AI;
using System;
using UnityEngine;


namespace BPEssentials.RegisteredEvents
{
    public class OnDamage : IScript
    {
        [Target(GameSourceEvent.PlayerDamage, ExecutionMode.Override)]
        public void OnEvent(ShPlayer player, DamageIndex damageIndex, float amount, ShPlayer attacker, Collider collider)
        {
            if (player.IsDead || player.IsShielded(damageIndex, collider))
            {
                return;
            }

            if (player.IsBlocking(damageIndex))
            {
                amount *= 0.3f;
            }
            else if (collider == player.headCollider) // Headshot
            {
                amount *= 2f;
            }

            if (!player.isHuman)
            {
                amount /= player.svPlayer.svManager.settings.difficulty;
            }

            amount -= amount * (player.armorLevel / player.maxStat * 0.5f);

            // -- BPE EXTEND
            if (player.isHuman && player.GetExtendedPlayer().HasGodmode)
            {
                player.SendChatMessage($"Damage blocked: {amount}HP. (Attacker: {attacker.ID}: {attacker.username.CleanerMessage()})");
                return;
            }
            // BPE EXTEND --
            new BrokeProtocol.GameSource.Types.Movable().OnDamage(player, damageIndex, amount, attacker, collider);

            if (player.IsDead)
            {
                return;
            }

            // Still alive, do knockdown and Assault crimes
            
            if (player.stance.setable)
            {
                if (player.isHuman && player.health < 15f)
                {
                    player.svPlayer.SvForceStance(StanceIndex.KnockedOut);
                    // If knockout AI, set AI state Null
                }
                else if (UnityEngine.Random.value < player.manager.damageTypes[(int)damageIndex].fallChance)
                {
                    player.StartCoroutine(player.svPlayer.KnockedDown());
                }
            }

            if (attacker && attacker != player)
            {
                if (player.wantedLevel == 0)
                {
                    if (attacker.curEquipable is ShGun)
                    {
                        attacker.svPlayer.SvAddCrime(CrimeIndex.ArmedAssault, player);
                    }
                    else
                    {
                        attacker.svPlayer.SvAddCrime(CrimeIndex.Assault, player);
                    }
                }

                if (!player.isHuman)
                {
                    player.svPlayer.targetEntity = attacker;
                    player.svPlayer.SetState(StateIndex.Attack);
                }
            }

            
        }
    }
}
