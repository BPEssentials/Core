using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;
using System.IO;

namespace BP_Essentials
{
    class CreateFile
    {
        public static void Run(string fileName)
        {
            try
            {
                switch (fileName)
                {
                    case SettingsFile:
                        {
                            if (IsPreRelease)
                                GetWebsiteContent.WriteToFile(SettingsFile, "http://www.UserR00T.com/dev/BPEssentials/settings_test.json");
                            else
                                GetWebsiteContent.WriteToFile(SettingsFile, "http://www.UserR00T.com/dev/BPEssentials/settings.json");
                            break;
                        }
                    case ChatBlockFile:
                        {
                            File.WriteAllText(ChatBlockFile, "");
                            break;
                        }
                    case LanguageBlockFile:
                        {
                            File.WriteAllText(LanguageBlockFile, "");
                            break;
                        }
                    case CustomCommandsFile:
                        {
                            GetWebsiteContent.WriteToFile(CustomCommandsFile, "http://www.UserR00T.com/dev/BPEssentials/customcommands.json");
                            break;
                        }
                    case CustomGroupsFile:
                        {
                            GetWebsiteContent.WriteToFile(CustomGroupsFile, "http://www.UserR00T.com/dev/BPEssentials/customgroups.json");
                            break;
                        }

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
