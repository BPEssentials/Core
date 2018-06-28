﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;

namespace BP_Essentials
{
    class CreateFile : EssentialsChatPlugin
    {
        public static void Run(string fileName)
        {
            try
            {
                switch (fileName)
                {
                    case SettingsFile:
                        {
                            if (isPreRelease)
#pragma warning disable CS0162 // Unreachable code detected / *shrugs*
                                File.WriteAllText(SettingsFile, DownloadFile.Run("http://www.UserR00T.com/dev/BPEssentials/settings_test.txt"));
#pragma warning restore CS0162 // Unreachable code detected
                            else
#pragma warning disable CS0162 // Unreachable code detected / *shrugs*
                                File.WriteAllText(SettingsFile, DownloadFile.Run("http://www.UserR00T.com/dev/BPEssentials/settings.txt"));
#pragma warning restore CS0162 // Unreachable code detected
                            break;
                        }
                    case ChatBlockFile:
                        {
                            string[] content = { "nigger", "nigga", "nigg3r", "NIGGER", "NI99ER", "ni99er", "nigger.", "nigga.", "nigg3r.", "N199ER", "n1gger", "N1GGER", "NIGGA", "NIGGA." };
                            File.WriteAllLines(ChatBlockFile, content);
                            break;
                        }
                    case LanguageBlockFile:
                        {
                            string[] content = { "bombas", "hola", "alguien", "habla", "espanol", "español", "estoy", "banco", "voy", "consegi", "donde", "quedamos", "banko", "afuera", "estas", "alguem", "donde", "nos", "vemos", "soy ", "vueno", "como", "carro", "cabros", "miren", "hacha", "laar", "corri", "sacame", "aqui", "policia", "trajo", "encerro", "bomba", "beuno", "pantalones", "dinero", "porque", "tengo", "escopetaa", "escopeta" };
                            File.WriteAllLines(LanguageBlockFile, content);
                            break;
                        }
                    case CustomCommandsFile:
                        {
                            File.WriteAllText(CustomCommandsFile, DownloadFile.Run("http://www.UserR00T.com/dev/BPEssentials/customcommands.txt"));
                            break;
                        }
                    case CustomGroupsFile:
                        {
                            File.WriteAllText(CustomGroupsFile, DownloadFile.Run("http://www.UserR00T.com/dev/BPEssentials/customgroups.txt"));
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
