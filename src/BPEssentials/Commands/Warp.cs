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
        private string Type = "warp";

        public void Invoke(ShPlayer player, string warpName)
        {
            if (!Core.Instance.WarpHandler.List.Any(x => x.Name == warpName))
            {
                player.TS("expFileHandler_error_notFound", player.T(Type), warpName);
                return;
            }
            var obj = Core.Instance.WarpHandler.List.FirstOrDefault(x => x.Name == warpName);
            if (!player.svPlayer.HasPermission($"{Core.Instance.Info.GroupNamespace}.{Type}.{warpName}"))
            {
                player.TS("expFileHandler_error_noPermission", player.T(Type), warpName);
                return;
            }
            if (obj.Disabled)
            {
                player.TS("expFileHandler_error_disabled", player.T(Type), warpName);
                return;
            }
            if (player.IsCooldown(Type, warpName))
            {
                player.TS("expFileHandler_error_cooldown", player.T(Type), player.GetCooldown(Type, warpName).ToString());
                return;
            }
            if (obj.Price > 0)
            {
                if (player.MyMoneyCount() < obj.Price)
                {
                    player.TS("expFileHandler_error_price", player.T(Type), obj.Price.ToString(), player.MyMoneyCount().ToString());
                    return;
                }
                player.TransferMoney(DeltaInv.RemoveFromMe, obj.Price, true);
            }
            player.GetExtendedPlayer().ResetAndSavePosition(new Vector3(obj.Position.X, obj.Position.Y, obj.Position.Z), new Quaternion(obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z, obj.Rotation.W), obj.Position.PlaceIndex);
            if (obj.Delay > 0)
            {
                player.AddCooldown(Type, warpName, obj.Delay);
            }
            player.SendChatMessage(player.T(Type + "_teleported", warpName) + (obj.Delay > 0 ? player.T(Type + "_telported_Price", obj.Price.ToString()) : "") + (obj.Delay > 0 ? player.T(Type + "_telported_Delay", obj.Delay.ToString()) : ""));
        }
    }
}
