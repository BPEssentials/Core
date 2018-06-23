using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
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
        public static void Run(string json)
        {
            try
            {
                if (!EnableDiscordWebhook)
                    return;
                if (DebugLevel >= 2)
                    Debug.Log($"{SetTimeStamp.Run()}[INFO] Creating POST request to {DiscordWebhook}");
                var formData = Encoding.UTF8.GetBytes(json);
                var www = new WWW(DiscordWebhook, formData);
                SvMan.StartCoroutine(WaitForRequest(www));
                if (DebugLevel >= 2)
                    Debug.Log($"{SetTimeStamp.Run()}[INFO] Post request sent!");
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
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
            Run(@"{ ""username"":""BP-Essentials Ban Log"", ""avatar_url"":""https://i.imgur.com/ckuPXOH.jpg"", ""embeds"":[ {""title"":""New ban AutoLogger"",""description"":""\u200B"",""color"":3837400,""timestamp"":""2018-06-23T12:26:09.635Z"",""footer"":{""text"":""BP-Essentials""},""fields"":[{""name"":""Username"",""value"":" + fUsername + @"},{""name"":""Issuer"",""value"":" + fIssuer + @"},{""name"":""Reason"",""value"":" + fReason + @"},{""name"":""Date (UTC)"",""value"":""" + DateTime.UtcNow + @"""}]}] }");
        }
        static IEnumerator WaitForRequest(WWW www)
        {
            yield return www;
            if (DebugLevel >= 2)
                Debug.Log($"{SetTimeStamp.Run()}[INFO] Post request response received: {(www.error ?? www.text)}");
            www.Dispose();
        }
    }
}
