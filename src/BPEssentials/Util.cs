using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BPCoreLib.Interfaces;
using BPCoreLib.PlayerFactory;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.ExtensionMethods;
using BPEssentials.Interfaces;
using BrokeProtocol.API;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.API.Types;
using BrokeProtocol.Entities;
using static BPEssentials.ExtendedPlayer.PlayerItem;

namespace BPEssentials
{
    public static class Util
    {
        public static void SendStaffChatMessage(ShPlayer player, string message)
        {
            foreach (var currPlayer in PoolHandler.Players.All())
            {
                if (currPlayer == player || !currPlayer.GetExtendedPlayer().CanRecieveStaffChat)
                    continue;
                currPlayer.SendChatMessage($"[STAFFCHAT] {player.username.SanitizeString()}: {message}"); // to Username
            }
        }

        public static void SendToAllEnabledChat(string message)
        {
            foreach (var currPlayer in PoolHandler.Players.All())
            {
                if (currPlayer.GetExtendedPlayer().CurrentChat == Chat.Disabled)
                    continue;
                currPlayer.SendChatMessage(message);
            }
        }

        public static void SendToAllEnabledChat(string[] messages)
        {
            foreach (var currPlayer in PoolHandler.Players.All())
            {
                if (currPlayer.GetExtendedPlayer().CurrentChat == Chat.Disabled)
                    continue;
                foreach (var message in messages)
                {
                    currPlayer.SendChatMessage(message);
                }
            }
        }

        public static void SendToAllEnabledChat(List<string> messages) => SendToAllEnabledChat(messages.ToArray());

        public static void ReloadFiles()
        {
            Core.Instance.ReadConfigurationFiles();
            Core.Instance.RegisterCustomCommands();
        }

        public static bool TryInstanciateAndInjectDependencies(string typeName, out ICommand instance, out Type type)
        {
            try
            {
                type = Type.GetType($"BPEssentials.Commands.{typeName}");
                instance = (ICommand)Activator.CreateInstance(type);
                instance = InjectDependenciesIntoCommand(instance, Core.Instance.Logger, Core.Instance.Settings, Core.Instance.PlayerHandler);
                return true;
            }
            catch (Exception ex)
            {
                Core.Instance.Logger.LogException(ex);
                instance = null;
                type = null;
                return false;
            }
        }

        public static ICommand InjectDependenciesIntoCommand(ICommand command, ILogger logger, Settings settings, ExtendedPlayerFactory extendedPlayerFactory)
        {
            command.Logger = logger;
            command.Settings = settings;
            command.PlayerFactory = extendedPlayerFactory;
            return command;
        }

        public static Delegate GetCommandMethodDelegateByTypeName(string typeName, out ICommand instance)
        {
            instance = null;
            try
            {
                var success = TryInstanciateAndInjectDependencies(typeName, out instance, out var type);
                if (!success)
                {
                    return null;
                }
                var method = type.GetMethod("Invoke");
                var types = method.GetParameters().Select(p => p.ParameterType);
                return Delegate.CreateDelegate(Expression.GetActionType(types.ToArray()), instance, method.Name);
            }
            catch (Exception ex)
            {
                Core.Instance.Logger.LogException(ex);
                return null;
            }
        }
    }
}
