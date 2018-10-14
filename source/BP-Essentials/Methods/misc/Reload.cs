using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.Threading;

namespace BP_Essentials
{
    class Reload
    {
        public static void Run(bool silentExecution, SvPlayer player = null, bool IsFirstReload = false)
        {
            try
            {
                if (!silentExecution)
                {
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, "[WAIT] Reloading all files..");
                    CheckFiles.Run();
                    ReadFile.Run(SettingsFile);
                    ReadCustomCommands.Run();
                    ReadGroups.Run();
                    Kits.LoadAllKits();
                    ReadStream.Run(LanguageBlockFile, LanguageBlockWords);
                    ReadStream.Run(ChatBlockFile, ChatBlockWords);
                    ReadStream.Run(AdminListFile, AdminsListPlayers);
                    LanguageBlockWords = LanguageBlockWords.ConvertAll(d => d.ToLower());
                    ChatBlockWords = ChatBlockWords.ConvertAll(d => d.ToLower());
                    ReadFile.Run(IdListItemsFile);
                    ReadFile.Run(IdListVehicleFile);
                    ReadFile.Run(AnnouncementsFile);
                    ReadFile.Run(GodListFile);
                    ReadFile.Run(MuteListFile);
                    ReadFile.Run(AfkListFile);
                    ReadFile.Run(RulesFile);
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, "[OK] Critical .txt files reloaded");
                }
                else
                {
                    CheckFiles.Run();
                    ReadFile.Run(SettingsFile);
                    ReadStream.Run(LanguageBlockFile, LanguageBlockWords);
                    ReadStream.Run(ChatBlockFile, ChatBlockWords);
                    ReadStream.Run(AdminListFile, AdminsListPlayers);
                    ReadCustomCommands.Run();
                    ReadGroups.Run();
                    Kits.LoadAllKits(IsFirstReload);
                    LanguageBlockWords = LanguageBlockWords.ConvertAll(d => d.ToLower());
                    ChatBlockWords = ChatBlockWords.ConvertAll(d => d.ToLower());
                    if (DownloadIdList)
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
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
