using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using static BP_Essentials.EssentialsConfigPlugin;
namespace BP_Essentials
{
    class Reload : EssentialsChatPlugin
    {
        public static void Run(bool silentExecution, object oPlayer = null)
        {
            if (!silentExecution)
            {
                var player = (SvPlayer)oPlayer;
                if (AdminsListPlayers.Contains(player.playerData.username))
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Checking if file's exist...");
                    CheckFiles("all");
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Reloading config files...");
                    ReadFile(SettingsFile);
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "[OK] Config file reloaded");
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Reloading critical .txt files...");
                    ReadCustomCommands();
                    ReadFileStream(LanguageBlockFile, LanguageBlockWords);
                    ReadFileStream(ChatBlockFile, ChatBlockWords);
                    ReadFileStream(AdminListFile, AdminsListPlayers);
                    LanguageBlockWords = LanguageBlockWords.ConvertAll(d => d.ToLower());
                    ChatBlockWords = ChatBlockWords.ConvertAll(d => d.ToLower());
                    ReadFile(AnnouncementsFile);
                    ReadFile(GodListFile);
                    ReadFile(MuteListFile);
                    ReadFile(AfkListFile);
                    ReadFile(RulesFile);
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "[OK] Critical .txt files reloaded");
                }
                else
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                }
            }
            else
            {
                CheckFiles("all");
                ReadFile(SettingsFile);
                ReadFileStream(LanguageBlockFile, LanguageBlockWords);
                ReadFileStream(ChatBlockFile, ChatBlockWords);
                ReadFileStream(AdminListFile, AdminsListPlayers);
                ReadCustomCommands();
                LanguageBlockWords = LanguageBlockWords.ConvertAll(d => d.ToLower());
                ChatBlockWords = ChatBlockWords.ConvertAll(d => d.ToLower());
                ReadFile(AnnouncementsFile);
                ReadFile(GodListFile);
                ReadFile(MuteListFile);
                ReadFile(AfkListFile);
                ReadFile(RulesFile);
            }
        }
    }
}
