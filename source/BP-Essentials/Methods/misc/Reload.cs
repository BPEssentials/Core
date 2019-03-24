using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;
using System.Threading;

namespace BP_Essentials
{
    class Reload
    {
        public static void Run(bool silentExecution, SvPlayer player = null, bool IsFirstReload = false)
        {
            try
            {
                if (!silentExecution && player != null)
                    player.SendChatMessage("[WAIT] Reloading all files..");
                CheckFiles.Run();
                ReadFile.Run(SettingsFile);
                ReadStream.Run(LanguageBlockFile, LanguageBlockWords);
                ReadStream.Run(ChatBlockFile, ChatBlockWords);
                ReadStream.Run(AdminListFile, AdminsListPlayers);
                ReadCustomCommands.Run();
                ReadGroups.Run();
                LanguageBlockWords = LanguageBlockWords.ConvertAll(d => d.ToLower());
                ChatBlockWords = ChatBlockWords.ConvertAll(d => d.ToLower());
                if (DownloadIdList && player == null) // do not download every time a player /reloads
                    GetIdList.Run(false);
                else
                {
                    ReadFile.Run(IdListItemsFile);
                    ReadFile.Run(IdListVehicleFile);
                }
                ReadFile.Run(AnnouncementsFile);
                ReadFile.Run(GodListFile);
                ReadFile.Run(MuteListFile);
                ReadFile.Run(AfkListFile);
                ReadFile.Run(RulesFile);
                if (!silentExecution && player != null)
                    player.SendChatMessage("[OK] Critical config files reloaded");
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
