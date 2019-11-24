﻿using BPEssentials.Abstractions;
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
                player.TS("expFileHandler_error_notFound", player.T(Core.Instance.WarpHandler.Name), warp);
                return;
            }
            var obj = Core.Instance.WarpHandler.List.FirstOrDefault(x => x.Name == warp);
            if (!player.svPlayer.HasPermission($"{Core.Instance.Info.GroupNamespace}.{Core.Instance.WarpHandler.Name}.{warp}"))
            {
                player.TS("expFileHandler_error_noPermission", player.T(Core.Instance.WarpHandler.Name), warp);
                return;
            }
            if (obj.Disabled)
            {
                player.TS("expFileHandler_error_disabled", player.T(Core.Instance.WarpHandler.Name), warp);
                return;
            }
            if (player.IsCooldown(Core.Instance.WarpHandler.Name, warp))
            {
                player.TS("expFileHandler_error_cooldown", player.T(Core.Instance.WarpHandler.Name), player.GetCooldown(Core.Instance.WarpHandler.Name, warp).ToString());
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
            player.GetExtendedPlayer().ResetAndSavePosition(new Vector3(obj.Position.X, obj.Position.Y, obj.Position.Z), new Quaternion(obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z, obj.Rotation.W), obj.Position.PlaceIndex);
            if (obj.Delay > 0)
            {
                player.AddCooldown(Core.Instance.WarpHandler.Name, warp, obj.Delay);
            }
            player.SendChatMessage(
                player.T(Core.Instance.WarpHandler.Name + "_teleported", warp) + 
                (obj.Price > 0 ? player.T(Core.Instance.WarpHandler.Name + "_telported_Price", obj.Price.ToString()) : "") + 
                (obj.Delay > 0 ? player.T(Core.Instance.WarpHandler.Name + "_telported_Delay", obj.Delay.ToString()) : ""));
        }
    }
}
