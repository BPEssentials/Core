using System.Text.RegularExpressions;
using BrokeProtocol.API;
using BrokeProtocol.LiteDB;
using BrokeProtocol.Managers;
using BrokeProtocol.Utility;

namespace BPEssentials.Events
{
    public class OnRegister : IScript
    {
        [Target(GameSourceEvent.ManagerTryRegister, ExecutionMode.PreEvent)]
        public bool OnTryRegister(ConnectData connectionData)
        {
            if (Core.Instance.Settings.General.LimitNames && !Regex.IsMatch(connectionData.username, @"^[a-zA-Z0-9_-]+$"))
            {
                SvManager.Instance.RegisterFail(connectionData.connection, "Your username can only contain the following characters: A-Z a-z 0-9 _ -");
                return false;
            }
            if (SvManager.Instance.TryGetUserData(connectionData.username, out User user) && Core.Instance.Settings.General.DisableAccountOverwrite)
            {
                SvManager.Instance.RegisterFail(connectionData.connection, "This name has already been registered and this server has disabled overwriting accounts!");
                return false;
            }

            return true;
        }
    }
}
