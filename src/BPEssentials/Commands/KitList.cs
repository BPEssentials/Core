using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using System.Linq;

namespace BPEssentials.Commands
{
    public class KitList : Command
    {
        public void Invoke(ShPlayer player)
        {
            var kits = Core.Instance.KitHandler.List.Where(x => !x.Disabled && player.svPlayer.HasPermission($"{Core.Instance.Info.GroupNamespace}.{Core.Instance.KitHandler.Name}.{x.Name}")).Select(n => n.Name + $"{(n.Price != 0 ? $" ({n.Price})" : "")}").ToArray();
            player.TS("kits", kits.Length.ToString(), (kits == null || kits.Length == 0 ? "none" : string.Join(", ", kits)));
        }
    }
}
