using BPEssentials.ExtensionMethods;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using System;

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
            foreach (var currPlayer in Collections.Humans)
            {
                ExtendedPlayer.PlayerItem extendedPlayer = null;
                if (currPlayer == player || !(extendedPlayer = currPlayer.GetExtendedPlayer()).EnabledSpychat)
                    continue;
                extendedPlayer.SendSpyChatMessage(player, message);
            }
        }
    }
}
