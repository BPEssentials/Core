using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Collections;
using System.Threading;
using Newtonsoft.Json;

namespace BP_Essentials
{
    class SendDiscordMessage : MonoBehaviour
    {
        public static void Run(string json, string link, bool enabled)
        {
            try
            {
                if (!enabled)
                    return;
                if (DebugLevel >= 2)
                    Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] Creating POST request to {link}");
                var formData = Encoding.UTF8.GetBytes(json);
                var www = new WWW(link, formData);
                SvMan.StartCoroutine(WaitForRequest(www));
                if (DebugLevel >= 2)
                    Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] Post request sent!");
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
        public static void ReportMessage(string username, string issuer, string reason)
        {
            var fUsername = JsonConvert.ToString(username);
            var fIssuer = JsonConvert.ToString(issuer);
            var fReason = JsonConvert.ToString(reason);
            Run(@"{ ""username"":""BP-Essentials Report Log"", ""avatar_url"":""https://i.imgur.com/ckuPXOH.jpg"", ""embeds"":[ {""title"":""New report AutoLogger"",""description"":""\u200B"",""color"":3837400,""timestamp"":""2018-06-23T12:26:09.635Z"",""footer"":{""text"":""BP-Essentials""},""fields"":[{""name"":""Username"",""value"":" + fUsername + @"},{""name"":""Issuer"",""value"":" + fIssuer + @"},{""name"":""Reason"",""value"":" + fReason + @"},{""name"":""Date (UTC)"",""value"":""" + DateTime.UtcNow + @"""}]}] }", DiscordWebhook_Report, EnableDiscordWebhook_Report);
        }
        public static void BanMessage(string username, string issuer)
        {
            BanMessage(username, issuer, "No reason provided. (Used tab menu)");
        }
        public static void BanMessage(string username, string issuer, string banReason)
        {
            var fUsername = JsonConvert.ToString(username);
            var fIssuer = JsonConvert.ToString(issuer);
            var fReason = JsonConvert.ToString(banReason);
            Run(@"{ ""username"":""BP-Essentials Ban Log"", ""avatar_url"":""https://i.imgur.com/ckuPXOH.jpg"", ""embeds"":[ {""title"":""New ban AutoLogger"",""description"":""\u200B"",""color"":3837400,""timestamp"":""2018-06-23T12:26:09.635Z"",""footer"":{""text"":""BP-Essentials""},""fields"":[{""name"":""Username"",""value"":" + fUsername + @"},{""name"":""Issuer"",""value"":" + fIssuer + @"},{""name"":""Reason"",""value"":" + fReason + @"},{""name"":""Date (UTC)"",""value"":""" + DateTime.UtcNow + @"""}]}] }", DiscordWebhook_Ban, EnableDiscordWebhook_Ban);
        }
        static IEnumerator WaitForRequest(WWW www)
        {
            yield return www;
            if (DebugLevel >= 2)
                Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] Post request response received: {(www.text ?? $"[HTTP-ERROR] {www.error}")}");
            www.Dispose();
        }
    }
}
