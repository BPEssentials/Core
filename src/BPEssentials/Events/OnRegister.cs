using BrokeProtocol.API;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using BrokeProtocol.LiteDB;
using BrokeProtocol.Managers;
using BrokeProtocol.Utility;
using System.Text.RegularExpressions;

namespace BPEssentials.RegisteredEvents
{
    public class OnRegiser : IScript
    {

        [Target(GameSourceEvent.ManagerTryRegister, ExecutionMode.Test)]
        public bool OnTryRegister(SvManager svManager, ConnectionData connectionData)
        {
            if (Core.Instance.Settings.General.LimitNames && !Regex.IsMatch(connectionData.username, @"^[a-zA-Z0-9_-]+$"))
            {
                svManager.RegisterFail(connectionData.connection, $"Your Username can only contain letters: A-Z a-z 0-9 _ -");
                return false;
            }
            return true;
        }
    }
}