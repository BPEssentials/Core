using BPEssentials.Abstractions;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using System;

namespace BPEssentials.Commands
{
    public class Save : BpeCommand
    {
        public void Invoke(ShPlayer player)
        {
            Run();
        }

        public static void Run()
        {
            Core.Instance.Logger.LogWithTimestamp("Saving game status");
            try
            {
                SvManager.Instance.SaveAll();
            }
            catch (Exception ex)
            {
                Core.Instance.Logger.LogError($"Error while saving game status: {ex}");
            }
        }
    }
}