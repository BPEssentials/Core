
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Reflection;

namespace BP_Essentials.Commands
{
    class TestCommand : EssentialsChatPlugin
    {
        [Serializable]
        public class _General
        {
            public string version { get; set; }
            public string CommandCharacter { get; set; }
            public string TimestapFormat { get; set; }
            public bool DisplayUnknownCommandMessage { get; set; }
        }
        [Serializable]
        public class _Messages
        {
            public string noperm { get; set; }
            public string DisabledCommand { get; set; }
            public string msgSayPrefix { get; set; }
            public string DiscordLink { get; set; }
        }
        [Serializable]
        public class _Misc
        {
            public bool enableChatBlock { get; set; }
            public bool enableLanguageBlock { get; set; }
            public bool CheckForAlts { get; set; }
            public int TimeBetweenAnnounce { get; set; }
            public int BlockSpawnBot { get; set; }
            public bool EnableBlockSpawnBot { get; set; }
        }
        [Serializable]
        public class _Command
        {
            public string CommandName { get; set; }
            public string Command { get; set; }
            public string Command2 { get; set; }
            public string ExecutableBy { get; set; }
            public bool Disabled { get; set; }
            public string c { get; set; }
        }
        [Serializable]
        public class RootObject
        {
            public _General General { get; set; }
            public _Messages Messages { get; set; }
            public _Misc Misc { get; set; }
            public List<_Command> Commands { get; set; }
        }


        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                    if (shPlayer.svPlayer == player && shPlayer.IsRealPlayer())
                    {
                        if (message.StartsWith("/jsontest"))
                        {
                            Debug.Log("...1");
                            RootObject m = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(FileDirectory + "test.txt"));
                            Debug.Log("...2");
                            Debug.Log("RecievedDataCommandChar " + m.General.CommandCharacter);
                            Debug.Log("...4");
                        }
                        else if (message.StartsWith("/createexception"))
                        {
                            throw new ArgumentNullException("message", "boi you fucked up");
                        }
                    }
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;

        }

    }
}
