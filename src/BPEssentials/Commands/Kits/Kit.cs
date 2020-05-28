using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.ExtensionMethods.Cooldowns;
using BPEssentials.Utils;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using System;
using System.Linq;

namespace BPEssentials.Commands
{
    public class Kit : Command
    {
        public void Invoke(ShPlayer player, string kit)
        {
            var obj = Core.Instance.KitHandler.List.FirstOrDefault(x => x.Name.Equals(kit, StringComparison.OrdinalIgnoreCase));
            if (obj == null)
            {
                if (Core.Instance.Settings.Levenshtein.KitMode == Configuration.Models.SettingsModel.LevenshteinMode.None)
                {
                    player.TS("expFileHandler_error_notFound", player.T(Core.Instance.KitHandler.Name), kit);
                    return;
                }
                obj = Core.Instance.KitHandler.List.OrderByDescending(x => LevenshteinDistance.CalculateSimilarity(x.Name, kit)).FirstOrDefault();

                if (Core.Instance.Settings.Levenshtein.KitMode == Configuration.Models.SettingsModel.LevenshteinMode.Suggest)
                {
                    player.TS("expFileHandler_error_notFound", player.T(Core.Instance.KitHandler.Name), kit);
                    player.TS("levenshteinSuggest", obj.Name);
                    return;
                }
            }
            if (!player.HasPermission($"{Core.Instance.Info.GroupNamespace}.{Core.Instance.KitHandler.Name}.{obj.Name}"))
            {
                player.TS("expFileHandler_error_noPermission", player.T(Core.Instance.KitHandler.Name), obj.Name);
                return;
            }
            if (obj.Disabled)
            {
                player.TS("expFileHandler_error_disabled", player.T(Core.Instance.KitHandler.Name), obj.Name);
                return;
            }
            if (player.HasCooldown(Core.Instance.KitHandler.Name, obj.Name))
            {
                player.TS("expFileHandler_error_cooldown", player.T(Core.Instance.KitHandler.Name), player.GetCooldown(Core.Instance.KitHandler.Name, obj.Name).ToString());
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
                player.AddCooldown(Core.Instance.KitHandler.Name, obj.Name, obj.Delay);
            }
            player.SendChatMessage(
                player.TC(Core.Instance.KitHandler.Name + "_received", obj.Name) +
                (obj.Price > 0 ? player.TC(Core.Instance.KitHandler.Name + "_received_Price", obj.Price.ToString()) : "") +
                (obj.Delay > 0 ? player.TC(Core.Instance.KitHandler.Name + "_received_Delay", obj.Delay.ToString()) : ""));
        }
    }
}

