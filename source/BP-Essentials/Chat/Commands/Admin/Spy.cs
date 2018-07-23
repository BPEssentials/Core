using static BP_Essentials.EssentialsVariablesPlugin;
namespace BP_Essentials.Commands
{
    public class Spy : EssentialsChatPlugin
    {
        public static void Run(SvPlayer player)
        {
            var shPlayer = player.player;
            playerList[shPlayer.ID].spyEnabled = !playerList[shPlayer.ID].spyEnabled;
            player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>SpyChat</color> <color={argColor}>{(playerList[shPlayer.ID].spyEnabled ? "Enabled" : "Disabled")}</color><color={infoColor}>.</color>");
        }
    }
}
