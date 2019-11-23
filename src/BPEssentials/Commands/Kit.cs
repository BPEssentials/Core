using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.ExtensionMethods.Cooldowns;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using System.Linq;
using UnityEngine;

namespace BPEssentials.Commands
{
    public class Kit : Command
    {
        private string Type = "kit";

        public void Invoke(ShPlayer player, string kit)
        {
            if (!Core.Instance.KitHandler.List.Any(x => x.Name == kit))
            {
                player.TS("expFileHandler_error_notFound", player.T(Type), kit);
                return;
            }
            var obj = Core.Instance.KitHandler.List.FirstOrDefault(x => x.Name == kit);
            if (!player.svPlayer.HasPermission($"{Core.Instance.Info.GroupNamespace}.{Type}.{kit}"))
            {
                player.TS("expFileHandler_error_noPermission", player.T(Type), kit);
                return;
            }
            if (obj.Disabled)
            {
                player.TS("expFileHandler_error_disabled", player.T(Type), kit);
                return;
            }
            if (player.IsCooldown(Type, kit))
            {
                player.TS("expFileHandler_error_cooldown", player.T(Type), player.GetCooldown(Type, kit).ToString());
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
            foreach (var item in obj.Items)
            {
                player.TransferItem(DeltaInv.AddToMe, item.Id, item.Amount, true);
            }
            if (obj.Delay > 0)
            {
                player.AddCooldown(Type, kit, obj.Delay);
            }
            player.SendChatMessage(player.T(Type + "_received", kit) + (obj.Delay > 0 ? player.T(Type + "_received_Price", obj.Price.ToString()) : "") + (obj.Delay > 0 ? player.T(Type + "_received_Delay", obj.Delay.ToString()) : ""));
        }
    }
}

