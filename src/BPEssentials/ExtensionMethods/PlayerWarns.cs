using BrokeProtocol.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace BPEssentials.ExtensionMethods.Warns
{
    public static class ExtensionPlayerWarns
    {
        public static string CustomDataKey { get; } = "bpe:warns";

        public class SerializableWarn
        {
            public string Reason { get; set; }

            public string IssueraccountID { get; set; }

            public DateTime Date { get; set; }

            public SerializableWarn(string issuer, string reason, DateTime dateTime)
            {
                IssueraccountID = issuer;
                Reason = reason;
                Date = dateTime;
            }

            public string ToString(ShPlayer player)
            {
                var issuer = Core.Instance.SvManager.database.Users.FindById(IssueraccountID);
                return player.T("warn_toString", Reason, issuer != null ? issuer.Character.Username : IssueraccountID, Date.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static void AddWarn(this ShPlayer player, ShPlayer issuer, string reason)
        {
            var warns = GetWarns(player);
            warns.Add(new SerializableWarn(issuer.accountID, reason, DateTime.Now));
            player.svPlayer.CustomData.AddOrUpdate(CustomDataKey, warns);
        }

        public static void RemoveWarn(this ShPlayer player, int warnId)
        {
            var warns = GetWarns(player);
            warns.Remove(warns[warnId]);
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
