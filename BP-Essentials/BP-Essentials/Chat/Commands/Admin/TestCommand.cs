
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
                        else if (message.StartsWith("/tryvalue"))
                        {
                            ShPlayer shplyr = (ShPlayer)typeof(SvPlayer).GetField("player", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(shPlayer.svPlayer);
                            Debug.Log("Your ID:" + shplyr.ID);
                        }
                        else if (message.StartsWith("/usetheforce"))
                        {
                            player.SvForce(new Vector3(0f, 5000f, 0f));
                            player.ExplosionDamage(20f, 20f, shPlayer);
                            shPlayer.StartEffect(EffectIndex.Flashed);
                            player.SendToSelf(Channel.Unsequenced, 10, "Off he goes!");
                        }
                        else if (message.StartsWith("/sendtoself"))
                        {
                            string arg1 = GetArgument.Run(1, false, false, message);
                            string arg2 = GetArgument.Run(2, false, false, message);
                            byte barg1 = Convert.ToByte(arg1);
                            player.SendToSelf(Channel.Unsequenced, barg1, arg2);
                        }
                        else if (message.StartsWith("/addnew"))
                        {
                            GameMan gm = shPlayer.gameMan;
                            Vector3 position = shPlayer.GetPosition();
                            position[1] += 15f;

                            ShEntity she = new ShEntity();
                            if (shPlayer.GetPlaceIndex() == 0)
                            {
                                she = player.netMan.AddNewEntity(gm.GetEntity(884127623).gameObject, gm.places[0], position, player.playerData.rotation, false, false);
                                she.Spawn(position, player.playerData.rotation, gm.places[0]);
                            }
                            else
                                player.SendToSelf(Channel.Unsequenced, 10, "Cannot spawn inside a place");


                            //int size = 0;
                            //IndexCollection<ShEntity> ECol = (IndexCollection<ShEntity>)typeof(GameMan).GetField("entityCollection", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(gm);
                            //foreach (ShEntity v in ECol)
                            //{
                            //    ++size;
                            //    Debug.Log(v.index);
                            //}
                            //Debug.Log("Size of IndexCollection: " + size);
                        }
                        else if (message.StartsWith("/gl"))
                        {
                            shPlayer.playerInventory.TransferItem(1, -700261193, 1, true);
                            shPlayer.playerInventory.TransferItem(1, 1695812550, 1, true);
                            shPlayer.playerInventory.TransferItem(1, 499504400, 1, true);
                            shPlayer.playerInventory.TransferItem(1, 607710552, 1, true);
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
