using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
namespace BP_Essentials
{
    class Reload : EssentialsChatPlugin
    {
        public static void Run(bool silentExecution, object oPlayer = null)
        {
            try
            {
                if (!silentExecution)
                {
                    var player = (SvPlayer)oPlayer;
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        player.SendToSelf(Channel.Unsequenced, 10, "Checking if file's exist...");
                        CheckFiles.Run("all");
                        player.SendToSelf(Channel.Unsequenced, 10, "Reloading config files...");
                        ReadFile.Run(SettingsFile);
                        player.SendToSelf(Channel.Unsequenced, 10, "[OK] Config file reloaded");
                        player.SendToSelf(Channel.Unsequenced, 10, "Reloading critical .txt files...");
                        ReadCustomCommands.Run();
                        ReadStream.Run(LanguageBlockFile, LanguageBlockWords);
                        ReadStream.Run(ChatBlockFile, ChatBlockWords);
                        ReadStream.Run(AdminListFile, AdminsListPlayers);
                        LanguageBlockWords = LanguageBlockWords.ConvertAll(d => d.ToLower());
                        ChatBlockWords = ChatBlockWords.ConvertAll(d => d.ToLower());
                        ReadFile.Run(AnnouncementsFile);
                        ReadFile.Run(GodListFile);
                        ReadFile.Run(MuteListFile);
                        ReadFile.Run(AfkListFile);
                        ReadFile.Run(RulesFile);
                        player.SendToSelf(Channel.Unsequenced, 10, "[OK] Critical .txt files reloaded");
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, 10, MsgNoPerm);
                    }
                }
                else
                {
                    CheckFiles.Run("all");
                    ReadFile.Run(SettingsFile);
                    ReadStream.Run(LanguageBlockFile, LanguageBlockWords);
                    ReadStream.Run(ChatBlockFile, ChatBlockWords);
                    ReadStream.Run(AdminListFile, AdminsListPlayers);
                    ReadCustomCommands.Run();
                    LanguageBlockWords = LanguageBlockWords.ConvertAll(d => d.ToLower());
                    ChatBlockWords = ChatBlockWords.ConvertAll(d => d.ToLower());
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
