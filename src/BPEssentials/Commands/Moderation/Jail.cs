using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using BrokeProtocol.Utility.Networking;

namespace BPEssentials.Commands
{
    public class Jail : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target, float timeInSeconds)
        {
            var jail = Core.Instance.SvManager.jails.GetRandom();
            if (jail == null)
            {
                return;
            }
            if (target.IsDead || target.svPlayer.job.info.shared.jobIndex == BPAPI.Instance.PrisonerIndex)
            {
                return;
            }
            var getPositionT = jail.mainT;
            target.svPlayer.SvTrySetJob(BPAPI.Instance.PrisonerIndex, true, false);
            target.GetExtendedPlayer().ResetAndSavePosition(getPositionT.position, getPositionT.rotation, 0);
            target.svPlayer.SvClearCrimes();
            target.svPlayer.RemoveItemsJail();
            target.StartCoroutine(target.svPlayer.JailTimer(timeInSeconds));
            target.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowTimer, timeInSeconds);
            player.TS("player_jail", target.username.CleanerMessage(), timeInSeconds);
            target.TS("target_jail", player.username.CleanerMessage(), timeInSeconds);
        }
    }
}