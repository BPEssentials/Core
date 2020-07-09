using BrokeProtocol.Entities;

namespace BPEssentials.ExtensionMethods.Cooldowns
{
    public static class ExtensionPlayerCooldowns
    {
        public static bool HasCooldown(this ShPlayer player, string type, string key)
        {
            return Core.Instance.CooldownHandler.IsCooldown(player.username, type, key);
        }

        public static void AddCooldown(this ShPlayer player, string type, string key, int time)
        {
            Core.Instance.CooldownHandler.AddCooldown(player.username, type, key, time);
        }

        public static int GetCooldown(this ShPlayer player, string type, string key)
        {
            return Core.Instance.CooldownHandler.GetCooldown(player.username, type, key);
        }
    }
}