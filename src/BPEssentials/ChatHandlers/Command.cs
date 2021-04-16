using BPEssentials.ExtensionMethods;
using BrokeProtocol.API;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;

namespace BPEssentials.ChatHandlers
{
    public class Command : IScript
    {
        [Target(GameSourceEvent.PlayerCommand, ExecutionMode.Event)]
        public void OnEvent(ShPlayer player, string message)
        {
            Core.Instance.Logger.LogInfo($"[COMMAND] {player.username}: {message}");
            foreach (var currPlayer in EntityCollections.Humans)
            {
                ExtendedPlayer.PlayerItem extendedPlayer = currPlayer.GetExtendedPlayer();
                if (currPlayer == player || !extendedPlayer.EnabledSpychat)
                {
                    continue;
                }
                extendedPlayer.SendSpyChatMessage(player, message);
            }
        }
    }
}
