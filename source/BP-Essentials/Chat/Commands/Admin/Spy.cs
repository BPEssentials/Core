using static BP_Essentials.EssentialsVariablesPlugin;
namespace BP_Essentials.Commands
{
    public class Spy
    {
        public static void Run(SvPlayer player, string message)
        {
            var shPlayer = player.player;
            playerList[shPlayer.ID].spyEnabled = !playerList[shPlayer.ID].spyEnabled;
            player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>SpyChat</color> <color={argColor}>{(playerList[shPlayer.ID].spyEnabled ? "Enabled" : "Disabled")}</color><color={infoColor}>.</color>");
        }
    }
}
