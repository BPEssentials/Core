using static BP_Essentials.Variables;
namespace BP_Essentials.Commands
{
    public class Spy
    {
        public static void Run(SvPlayer player, string message)
        {
            PlayerList[player.player.ID].SpyEnabled = !PlayerList[player.player.ID].SpyEnabled;
            player.SendChatMessage($"<color={infoColor}>SpyChat</color> <color={argColor}>{(PlayerList[player.player.ID].SpyEnabled ? "Enabled" : "Disabled")}</color><color={infoColor}>.</color>");
        }
    }
}
