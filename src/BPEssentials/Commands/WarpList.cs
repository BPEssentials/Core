using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using System.Linq;

namespace BPEssentials.Commands
{
    public class WarpList : Command
    {
        private string Type = "warp";

        public void Invoke(ShPlayer player)
        {
            var warps = Core.Instance.WarpHandler.List.Where(x => !x.Disabled && player.svPlayer.HasPermission($"{Core.Instance.Info.GroupNamespace}.{Type}.{x.Name}")).Select(n => n.Name + $"{(n.Price != 0 ? $" ({n.Price})" : "")}").ToArray();
            player.TS("warps", warps.Length.ToString(), (warps == null || warps.Length == 0 ? "none" : string.Join(", ", warps)));
        }
    }
}
