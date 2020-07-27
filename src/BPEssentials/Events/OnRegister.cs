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
        [Target(GameSourceEvent.ManagerTryRegister, ExecutionMode.Override)]
        public void OnTryRegister(SvManager svManager, ConnectionData connectData)
        {
            if (ValidateUser(svManager, connectData))
            {
                if (svManager.TryGetUserData(connectData.username, out User playerData))
                {
                    if (playerData.PasswordHash != connectData.passwordHash)
                    {
                        svManager.RegisterFail(connectData.connection, "Invalid credentials");
                        return;
                    }
                }

                if (!connectData.username.ValidCredential())
                {
                    svManager.RegisterFail(connectData.connection, $"Name cannot be registered (min: {Util.minCredential}, max: {Util.maxCredential})");
                    return;
                }
                // -- BPE EXTEND
                if (Core.Instance.Settings.General.LimitNames && !Regex.IsMatch(connectData.username, @"^[a-zA-Z0-9_]+$"))
                {
                    svManager.RegisterFail(connectData.connection, $"Your Username can only contain letters: A-Z a-z 0-9 _ -");
                    return;
                }
                // BPE EXTEND --
                svManager.AddNewPlayer(connectData);
            }
        }

        public bool ValidateUser(SvManager svManager, ConnectionData authData)
        {
            if (!svManager.HandleWhitelist(authData.username))
            {
                svManager.RegisterFail(authData.connection, "Account not whitelisted");
                return false;
            }

            // Don't allow multi-boxing, WebAPI doesn't prevent this
            foreach (ShPlayer p in EntityCollections.Humans)
            {
                if (p.username == authData.username)
                {
                    svManager.RegisterFail(authData.connection, "Account still logged in");
                    return false;
                }
            }

            return true;
        }
    }
}