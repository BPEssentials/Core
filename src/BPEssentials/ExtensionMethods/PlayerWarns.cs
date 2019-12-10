using BrokeProtocol.Entities;
using System.Collections.Generic;

namespace BPEssentials.ExtensionMethods.Warns
{
    public static class ExtensionPlayerWarns
    {
        public static string CustomDataKey { get; } = "bpe:warns";

        public class SerializableWarn
        {
            public string Reason { get; set; }

            public string IssuerSteamID { get; set; }

            public SerializableWarn(string issuer, string reason)
            {
                IssuerSteamID = issuer;
                Reason = reason;
            }

            public override string ToString()
            {
                return $"{Reason} by {IssuerSteamID}";
            }
        }

        public static void AddWarn(this ShPlayer player, ShPlayer issuer, string reason)
        {
            var warns = GetWarns(player);
            warns.Add(new SerializableWarn(issuer.steamID, reason));
            player.svPlayer.CustomData.AddOrUpdate(CustomDataKey, warns);
        }

        public static List<SerializableWarn> GetWarns(this ShPlayer player)
        {
            player.svPlayer.CustomData.TryFetchCustomData<List<SerializableWarn>>(CustomDataKey, out var warns);
            if (warns == null)
            {
                warns = new List<SerializableWarn>();
            }
            return warns;
        }
    }
}
