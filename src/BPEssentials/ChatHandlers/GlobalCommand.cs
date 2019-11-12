using BPEssentials.ExtensionMethods;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using System;
using BrokeProtocol.Collections;

namespace BPEssentials.ChatHandlers
{
    public class GlobalCommand : IScript
    {
        public GlobalCommand()
        {
            GameSourceHandler.Add(BrokeProtocol.API.Events.Player.OnCommand, new Action<ShPlayer, string>(OnEvent));
        }

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
