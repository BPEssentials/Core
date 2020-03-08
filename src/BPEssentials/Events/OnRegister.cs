using BrokeProtocol.API;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using BrokeProtocol.Server.LiteDB.Models;
using BrokeProtocol.Utility;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BPEssentials.RegisteredEvents
{
    public class OnRegiser : IScript
    {
        [Target(GameSourceEvent.ManagerTryRegister, ExecutionMode.Override)]
        public void OnTryRegister(SvManager svManager, AuthData authData, ConnectData connectData)
        {
            if (ValidateUser(svManager, authData))
            {
                if (svManager.TryGetUserData(authData.accountID, out User playerData))
                {
                    if (playerData.BanInfo.IsBanned)
                    {
                        svManager.RegisterFail(authData.connection, $"Account banned: {playerData.BanInfo.Reason}");
                        return;
                    }

                    if (!svManager.settings.auth.steam && playerData.PasswordHash != connectData.passwordHash)
                    {
                        svManager.RegisterFail(authData.connection, $"Invalid credentials");
                        return;
                    }
                }


                if (!connectData.username.ValidCredential())
                {
                    svManager.RegisterFail(authData.connection, $"Name cannot be registered (min: {Util.minCredential}, max: {Util.maxCredential})");
                    return;
                }
                if (!Regex.IsMatch(connectData.username, @"^[a-zA-Z0-9_]+$"))
                {
                    svManager.RegisterFail(authData.connection, $"Your Username can only contain letters: A-Z a-z 0-9 _ -");
                    return;
                }

                svManager.AddNewPlayer(authData, connectData);
            }
        }

        public bool ValidateUser(SvManager svManager, AuthData authData)
        {
            if (!svManager.HandleWhitelist(authData.accountID))
            {
                svManager.RegisterFail(authData.connection, "Account not whitelisted");
                return false;
            }

            // Don't allow multi-boxing, WebAPI doesn't prevent this
            foreach (ShPlayer p in EntityCollections.Humans)
            {
                if (p.accountID == authData.accountID)
                {
                    svManager.RegisterFail(authData.connection, "Account still logged in");
                    return false;
                }
            }

            return true;
        }
    }
}
