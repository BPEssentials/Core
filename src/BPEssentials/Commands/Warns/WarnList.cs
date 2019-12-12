using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.ExtensionMethods.Warns;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;
using System.Linq;

namespace BPEssentials.Commands
{
    public class WarnList : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target = null)
        {
            if(target != null && !player.svPlayer.HasPermission($"{Core.Instance.Info.GroupNamespace}.{nameof(WarnList)}.viewotherplayers"))
            {
                player.TS("warns_noPermission_viewOtherPlayers");
                return;
            }
            target = target ?? player;
            var warns = target.GetWarns().Select(warn => warn.ToString()).ToArray();
            player.TS("warns", target.username.SanitizeString(), warns.Length.ToString(), (warns.Length == 0 ? "none" : string.Join(", ", warns)));
        }
    }
}
