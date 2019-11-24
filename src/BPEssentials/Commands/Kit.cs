using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.ExtensionMethods.Cooldowns;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using System.Linq;

namespace BPEssentials.Commands
{
    public class Kit : Command
    {
        public void Invoke(ShPlayer player, string kit)
        {
            var obj = Core.Instance.KitHandler.List.FirstOrDefault(x => x.Name == kit);
            if (obj == null)
            {
                player.TS("expFileHandler_error_notFound", player.T(Core.Instance.KitHandler.Name), kit);
                return;
            }
            if (!player.svPlayer.HasPermission($"{Core.Instance.Info.GroupNamespace}.{Core.Instance.KitHandler.Name}.{kit}"))
            {
                player.TS("expFileHandler_error_noPermission", player.T(Core.Instance.KitHandler.Name), kit);
                return;
            }
            if (obj.Disabled)
            {
                player.TS("expFileHandler_error_disabled", player.T(Core.Instance.KitHandler.Name), kit);
                return;
            }
            if (player.IsCooldown(Core.Instance.KitHandler.Name, kit))
            {
                player.TS("expFileHandler_error_cooldown", player.T(Core.Instance.KitHandler.Name), player.GetCooldown(Core.Instance.KitHandler.Name, kit).ToString());
                return;
            }
            if (obj.Price > 0)
            {
                if (player.MyMoneyCount() < obj.Price)
                {
                    player.TS("expFileHandler_error_price", player.T(Core.Instance.KitHandler.Name), obj.Price.ToString(), player.MyMoneyCount().ToString());
                    return;
                }
                player.TransferMoney(DeltaInv.RemoveFromMe, obj.Price, true);
            }
            obj.GiveItems(player);
            if (obj.Delay > 0)
            {
                player.AddCooldown(Core.Instance.KitHandler.Name, kit, obj.Delay);
            }
            player.SendChatMessage(
                player.T(Core.Instance.KitHandler.Name + "_received", kit) + 
                (obj.Price > 0 ? player.T(Core.Instance.KitHandler.Name + "_received_Price", obj.Price.ToString()) : "") + 
                (obj.Delay > 0 ? player.T(Core.Instance.KitHandler.Name + "_received_Delay", obj.Delay.ToString()) : ""));
        }
    }
}

