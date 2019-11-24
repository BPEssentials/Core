using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.ExtensionMethods.Cooldowns;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using System.Linq;
using UnityEngine;

namespace BPEssentials.Commands
{
    public class Warp : Command
    {
        public void Invoke(ShPlayer player, string warp)
        {
            if (!Core.Instance.WarpHandler.List.Any(x => x.Name == warp))
            {
                player.TS("expFileHandler_error_notFound", player.T(nameof(warp)), warp);
                return;
            }
            var obj = Core.Instance.WarpHandler.List.FirstOrDefault(x => x.Name == warp);
            if (!player.svPlayer.HasPermission($"{Core.Instance.Info.GroupNamespace}.{nameof(warp)}.{warp}"))
            {
                player.TS("expFileHandler_error_noPermission", player.T(nameof(warp)), warp);
                return;
            }
            if (obj.Disabled)
            {
                player.TS("expFileHandler_error_disabled", player.T(nameof(warp)), warp);
                return;
            }
            if (player.IsCooldown(nameof(warp), warp))
            {
                player.TS("expFileHandler_error_cooldown", player.T(nameof(warp)), player.GetCooldown(nameof(warp), warp).ToString());
                return;
            }
            if (obj.Price > 0)
            {
                if (player.MyMoneyCount() < obj.Price)
                {
                    player.TS("expFileHandler_error_price", player.T(nameof(warp)), obj.Price.ToString(), player.MyMoneyCount().ToString());
                    return;
                }
                player.TransferMoney(DeltaInv.RemoveFromMe, obj.Price, true);
            }
            player.GetExtendedPlayer().ResetAndSavePosition(new Vector3(obj.Position.X, obj.Position.Y, obj.Position.Z), new Quaternion(obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z, obj.Rotation.W), obj.Position.PlaceIndex);
            if (obj.Delay > 0)
            {
                player.AddCooldown(nameof(warp), warp, obj.Delay);
            }
            player.SendChatMessage(player.T(nameof(warp) + "_teleported", warp) + (obj.Delay > 0 ? player.T(nameof(warp) + "_telported_Price", obj.Price.ToString()) : "") + (obj.Delay > 0 ? player.T(nameof(warp) + "_telported_Delay", obj.Delay.ToString()) : ""));
        }
    }
}
