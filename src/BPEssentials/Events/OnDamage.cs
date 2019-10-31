using BPEssentials.ExtensionMethods;
using BrokeProtocol.API;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;
using System;

namespace BPEssentials.RegisteredEvents
{
    public class OnDamage : IScript
    {
        public OnDamage()
        {
            GameSourceHandler.Add(BrokeProtocol.API.Events.Player.OnDamage, new Action<ShPlayer, ShPlayer, float>(OnEvent));
        }

        public void OnEvent(ShPlayer player, ShPlayer attacker, float amount)
        {
            if (player.GetExtendedPlayer().HasGodmode)
            {
                player.SendChatMessage($"Damage blocked: {amount}HP. (Attacker: {attacker.ID}: {attacker.username.SanitizeString()})");
                // TODO: Override gamesource
                return;
            }
        }
    }
}
