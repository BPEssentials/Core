using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;


namespace BPEssentials.Commands
{
    public class TpaAccept : BpeCommand
    {
        public void Invoke(ShPlayer player)
        {
            var ePlayer = player.GetExtendedPlayer();
            if (ePlayer.TpaUser == null || ePlayer.TpaUser.Player == null)
            {
                player.TS("no_tpa_requests");
                return;
            }
            ePlayer.TpaUser.Player.TS("TpaUser_tpa_accepted", player.username.CleanerMessage());
            player.TS("player_tpa_accepted", ePlayer.TpaUser.Player.username.CleanerMessage());
            if (ePlayer.TpaUser.TpHere)
            {
                player.GetExtendedPlayer().ResetAndSavePosition(ePlayer.TpaUser.Player);
            }
            else
            {
                ePlayer.TpaUser.Player.GetExtendedPlayer().ResetAndSavePosition(player);
            }
            ePlayer.TpaUser = null;
        }
    }
}
