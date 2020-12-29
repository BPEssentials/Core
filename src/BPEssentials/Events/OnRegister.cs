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
                svManager.RegisterFail(connectionData.connection, $"Your username can only contain the following characters: A-Z a-z 0-9 _ -");
                return false;
            }
            if (Core.Instance.Settings.General.DisableAccountOverwrite)
            {
                svManager.RegisterFail(connectionData.connection, "This name has already been registered and this server has disabled overwriting accounts!");
                return false;
            }
            return true;
        }
    }
}
