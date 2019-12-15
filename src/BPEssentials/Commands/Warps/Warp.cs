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
    public class Warp : Command
    {
        public void Invoke(ShPlayer player, string warp)
        {
            var obj = Core.Instance.WarpHandler.List.FirstOrDefault(x => x.Name.Equals(warp, StringComparison.OrdinalIgnoreCase)));
            if (obj == null)
            {
                if (Core.Instance.Settings.Levenshtein.WarpMode == Configuration.Models.SettingsModel.LevenshteinMode.None)
                {
                    player.TS("expFileHandler_error_notFound", player.T(Core.Instance.WarpHandler.Name), warp);
                    return;
                }
                obj = Core.Instance.WarpHandler.List.OrderByDescending(x => LevenshteinDistance.CalculateSimilarity(x.Name, warp)).FirstOrDefault();

                if (Core.Instance.Settings.Levenshtein.WarpMode == Configuration.Models.SettingsModel.LevenshteinMode.Suggest)
                {
                    player.TS("expFileHandler_error_notFound", player.T(Core.Instance.WarpHandler.Name), warp);
                    player.TS("levenshteinSuggest", obj.Name);
                    return;
                }
            }
            if (!player.svPlayer.HasPermission($"{Core.Instance.Info.GroupNamespace}.{Core.Instance.WarpHandler.Name}.{obj.Name}"))
            {
                player.TS("expFileHandler_error_noPermission", player.T(Core.Instance.WarpHandler.Name), obj.Name);
                return;
            }
            if (obj.Disabled)
            {
                player.TS("expFileHandler_error_disabled", player.T(Core.Instance.WarpHandler.Name), obj.Name);
                return;
            }
            if (player.HasCooldown(Core.Instance.WarpHandler.Name, obj.Name))
            {
                player.TS("expFileHandler_error_cooldown", player.T(Core.Instance.WarpHandler.Name), player.GetCooldown(Core.Instance.WarpHandler.Name, obj.Name).ToString());
                return;
            }
            if (obj.Price > 0)
            {
                if (player.MyMoneyCount() < obj.Price)
                {
                    player.TS("expFileHandler_error_price", player.T(Core.Instance.WarpHandler.Name), obj.Price.ToString(), player.MyMoneyCount().ToString());
                    return;
                }
                player.TransferMoney(DeltaInv.RemoveFromMe, obj.Price, true);
            }
            player.GetExtendedPlayer().ResetAndSavePosition(obj.Position.SerializableVector3.ToVector3(), obj.SerializableQuaternion.ToQuaternion(), obj.Position.PlaceIndex);
            if (obj.Delay > 0)
            {
                player.AddCooldown(Core.Instance.WarpHandler.Name, obj.Name, obj.Delay);
            }
            player.SendChatMessage(
                player.T(Core.Instance.WarpHandler.Name + "_teleported", obj.Name) + 
                (obj.Price > 0 ? player.T(Core.Instance.WarpHandler.Name + "_telported_Price", obj.Price.ToString()) : "") + 
                (obj.Delay > 0 ? player.T(Core.Instance.WarpHandler.Name + "_telported_Delay", obj.Delay.ToString()) : ""));
        }
    }
}
