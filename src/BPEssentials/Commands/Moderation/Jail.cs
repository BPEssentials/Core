using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using BrokeProtocol.Required;
using BrokeProtocol.Utility.Jobs;
using BrokeProtocol.Utility.Networking;
using System.Linq;

namespace BPEssentials.Commands
{
    public class Jail : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target, float timeInSeconds)
        {
            var jail = SceneManager.Instance.jail;
            if (jail == null)
            {
                return;
            }
            if (target.IsDead || target.job is Prisoner)
            {
                return;
            }
            var getPositionT = jail.mainT;
            target.svPlayer.SvTrySetJob(JobIndex.Prisoner, true, false);
            target.GetExtendedPlayer().ResetAndSavePosition(getPositionT.position, getPositionT.rotation, 0);
            target.svPlayer.SvClearCrimes();
            target.RemoveItemsJail();
            target.StartCoroutine(target.svPlayer.JailTimer(timeInSeconds));
            target.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowTimer, timeInSeconds);
            player.TS("player_jail", target.username.CleanerMessage(), timeInSeconds);
            target.TS("target_jail", player.username.CleanerMessage(), timeInSeconds);
        }
    }
}