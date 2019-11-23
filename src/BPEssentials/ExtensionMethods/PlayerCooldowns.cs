using BPEssentials.ExtendedPlayer;
using BrokeProtocol.Entities;
using UnityEngine;

namespace BPEssentials.ExtensionMethods.Cooldowns
{
    public static class ExtensionPlayerCooldowns
    {
        public static bool IsCooldown(this SvPlayer player, string type, string key)
        {
            return Core.Instance.CooldownHandler.IsCooldown(player.steamID, type, key);
        }

        public static bool IsCooldown(this ShPlayer player, string type, string key)
        {
            return player.svPlayer.IsCooldown(type, key);
        }

        public static void AddCooldown(this SvPlayer player, string type, string key, int time)
        {
            Core.Instance.CooldownHandler.AddCooldown(player.steamID, type, key, time);
        }

        public static void AddCooldown(this ShPlayer player, string type, string key, int time)
        {
            player.svPlayer.AddCooldown(type, key, time);
        }

        public static int GetCooldown(this SvPlayer player, string type, string key)
        {
            return Core.Instance.CooldownHandler.GetCooldown(player.steamID, type, key);
        }

        public static int GetCooldown(this ShPlayer player, string type, string key)
        {
            return player.svPlayer.GetCooldown(type, key);
        }
    }
}