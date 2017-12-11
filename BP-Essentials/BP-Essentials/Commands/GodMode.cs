using static BP_Essentials.EssentialsConfigPlugin;
using static BP_Essentials.EssentialsVariablesPlugin;
using System;
using System.IO;
namespace BP_Essentials.Commands {
    public class GodMode : EssentialsCorePlugin{
        public static bool Run(object oPlayer, string mesage) {
            {
                try
                {
                    var player = (SvPlayer)oPlayer;
                    ReadFile(GodListFile);

                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        if (GodListPlayers.Contains(player.playerData.username))
                        {
                            RemoveStringFromFile(GodListFile, player.playerData.username);
                            ReadFile(GodListFile);
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "Godmode disabled.");
                            return true;
                        }
                        else
                        {
                            File.AppendAllText(GodListFile, player.playerData.username + Environment.NewLine);
                            GodListPlayers.Add(player.playerData.username);
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "Godmode enabled.");
                            return true;
                        }
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                        return false;
                    }

                }
                catch (Exception ex)
                {

                    ErrorLogging(ex);

                    return true;
                }

            }
        }
    }
}