using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.ExtensionMethods.Warns;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility.Networking;
using System.Linq;
using System.Text;

namespace BPEssentials.Commands
{
    public class WarnRemove : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target, int warnId)
        {
            if (warnId < 1)
            {
                player.TS("warn_remove_error_null_or_negative", warnId.ToString());
                return;
            }
            var warns = target.GetWarns();
            if (warns.Count < warnId)
            {
                player.TS("warn_remove_error_notExistant", warnId.ToString());
                return;
            }
            player.TS("player_warn_removed", warns[warnId - 1].ToString(player));
            target.RemoveWarn(warnId - 1);
        }
    }
}
