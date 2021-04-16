using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Discord : Command
    {
        public void Invoke(ShPlayer player)
        {
            player.TS("discord", Core.Instance.Settings.Messages.DiscordLink);
            player.svPlayer.SvOpenURL(Core.Instance.Settings.Messages.DiscordLink, player.T("discord"));
        }
    }
}
