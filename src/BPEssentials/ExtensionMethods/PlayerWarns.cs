using BrokeProtocol.Entities;
using System;
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

            public DateTime Date { get; set; }

            public SerializableWarn(string issuer, string reason, DateTime dateTime)
            {
                IssuerSteamID = issuer;
                Reason = reason;
                Date = dateTime;
            }

            public override string ToString()
            {
                var issuer = Core.Instance.SvManager.Database.Users.FindById(IssuerSteamID);
                if (issuer == null)
                {
                    return $"{Reason} by {IssuerSteamID}";
                }
                return $"{Reason} by {issuer.Character.Username} on {Date.ToString()}";
            }
        }

        public static void AddWarn(this ShPlayer player, ShPlayer issuer, string reason)
        {
            var warns = GetWarns(player);
            warns.Add(new SerializableWarn(issuer.steamID, reason, DateTime.Now));
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
