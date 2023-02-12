using BPCoreLib.ExtensionMethods;
using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Heal : BpeCommand
    {
        public void Invoke(ShPlayer player, ShPlayer target = null)
        {
            target = target ?? player;
            target.svPlayer.HealFull();
            target.svPlayer.UpdateStatsDelta(1f, 1f, 1f);
            target.svPlayer.SvClearInjuries();
            player.TS("healed", target.username.CleanerMessage());
        }
    }
}
