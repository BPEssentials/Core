using BrokeProtocol.Entities;
using BPEssentials.Utils;
using BPEssentials.Abstractions;

namespace BPEssentials.Commands
{
    public class Sudo : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target, string message)
        {
            Core.Instance.Logger.LogInfo($"[GLOBAL] {target.username}: {message}");
            ChatUtils.SendToAllEnabledChat(ChatUtils.FormatMessage(target, message), false);
        }
    }
}
