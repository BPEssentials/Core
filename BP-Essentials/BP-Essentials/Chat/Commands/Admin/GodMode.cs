using static BP_Essentials.EssentialsConfigPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using static BP_Essentials.EssentialsVariablesPlugin;
using System;
using System.IO;
namespace BP_Essentials.Commands {
    public class GodMode : EssentialsChatPlugin{
        public static bool Run(object oPlayer, string message) {
            {
                try
                {
                        var player = (SvPlayer)oPlayer;
                        if (AdminsListPlayers.Contains(player.playerData.username))
                        {
                            ReadFile(GodListFile);
                            string name = GetArgument.Run(1, false, true, message);
                            string msg = "Godmode {0} for '" + name + "'.";
                            if (name == String.Empty)
                            {
                                name = player.playerData.username;
                                msg = "Godmode {0}.";
                            }
                            if (GodListPlayers.Contains(name))
                            {
                                RemoveStringFromFile.Run(GodListFile, name);
                                ReadFile(GodListFile);
                                player.SendToSelf(Channel.Unsequenced, (byte)10, String.Format(msg, "disabled"));
                            }
                            else
                            {
                                File.AppendAllText(GodListFile, name + Environment.NewLine);
                                GodListPlayers.Add(name);
                                player.SendToSelf(Channel.Unsequenced, (byte)10, String.Format(msg, "enabled"));
                            }
                        }
                        else
                            player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);

                }
                catch (Exception ex)
                {
                    ErrorLogging.Run(ex);
                }
                return true;

            }
        }
    }
}