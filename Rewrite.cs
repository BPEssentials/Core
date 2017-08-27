// Essentials created by UserR00T
using UnityEngine;
using System;
using System.Threading;
using System.IO;
using System.Linq;

public class EssentialsPlugin { 
    
    #region Folder Locations
	private static string DirectoryFolder = Directory.GetCurrentDirectory() + " \\Essentials";
	private static string SettingsFile = DirectoryFolder + "settings.txt";
	private static string LanguageBlockFile = DirectoryFolder + "languageblock.txt";
	private static string ChatBlockFile = DirectoryFolder + "chatblock.txt";

	private static string IPListFile = "ip_list.txt";
	private static string AdminListFile = "admin_list.txt";
    #endregion


	#region predefining variables

	// General
	private static string version;
	private static string msgUnknownCommand;
	private static string ChatBlock;
	private static string LanguageBlock;
    private static string Command;
    private static SvPlayer svplayer; 

	// Block arrays
	private static string[] ChatBlockWords;
	private static string[] LanguageBlockWords;

	// Messages
	private static string msgNoPerm;

	// Commands
	private static string cmdCommandCharacter;

	private static string cmdClearChat;
	private static string cmdClearChat2;

	private static string cmdReload;
	private static string cmdReload2;
	#endregion




    /*


			Code below here, Don't edit unless you know what you're doing.
			Information about the api @ https://github.com/deathbykorea/universalunityhooks


	 */



     #region Hooks
	[Hook("SvPlayer.Initialize")]

     #endregion


    //Startup script
    [Hook("SvNetMan.StartServerNetwork")]
	public static void StartServerNetwork(SvNetMan netMan)
	{
		if (!Directory.Exists(DirectoryFolder))
		{
			Directory.CreateDirectory(DirectoryFolder);
			Thread.Sleep(20);
			File.Create(SettingsFile);
			Debug.Log("[WARNING] Essentials - Settings file does not exist! Creating one.");
			DownloadFile("https://UserR00T.com/dev/BPEssentials/settings.txt",SettingsFile);
		}
		if (!File.Exists(SettingsFile))
		{
			File.Create(SettingsFile);
			Debug.Log("[WARNING] Essentials - Settings file does not exist! Creating one.");
			DownloadFile("https://UserR00T.com/dev/BPEssentials/settings.txt", SettingsFile);
		}
		Debug.Log("[INFO] Essentials - version: " + version + " Loaded in successfully!");
	}


    //Chat Events 
	[Hook("SvPlayer.SvGlobalChatMessage")]

	public static bool SvGlobalChatMessage(SvPlayer player, ref string message){

    
        if (message.StartsWith(cmdCommandCharacter){
            command = message
            
        }
        else { 
            MessageLog(message);

        }
        if (LanguageBlock == true) { 
            if (LanguageBlockWords.Any(message.ToLower().Contains)) { 

            }
        }

    }


    
    //Message Logging
    public static bool MessageLog(string message){}{
    	if (!message.StartsWith(cmdCommandCharacter))
		{
			Debug.Log("[MESSAGE]" + player.playerData.username + ": " + message);

			if (ChatBlock == true)
			{

				if (ChatBlockWords.Any(message.ToLower().Contains))
				{
					player.SendToSelf(Channel.Unsequenced, (byte)10, "Please don't say a blocked word, the message has been blocked.");
					Debug.Log(player.playerData.username + " Said a word that is blocked.");
					return true;
				}
			}
        }
        			return false;

    }

    public static void BlockMessage(string message){

    }
    

}

