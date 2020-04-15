using System;
using System.Linq;
using System.Linq.Expressions;
using BPCoreLib.Interfaces;
using BPCoreLib.PlayerFactory;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.ExtendedPlayer;


namespace BPEssentials.Utils
{
    public static class CommandInjection
    {
        public static bool TryInstanciateAndInjectDependencies(string typeName, out Abstractions.Command instance, out Type type)
        {
            try
            {
                type = Type.GetType(typeName);
                instance = (Abstractions.Command)Activator.CreateInstance(type);
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

        public static Abstractions.Command InjectDependenciesIntoCommand(Abstractions.Command command, ILogger logger, Settings settings, ExtendedPlayerFactory<PlayerItem> extendedPlayerFactory)
        {
            command.Logger = logger;
            command.Settings = settings;
            command.PlayerFactory = extendedPlayerFactory;
            return command;
        }

        public static bool TryGetCommandMethodDelegateByTypeName(string typeName, out Delegate del, out Abstractions.Command instance)
        {
            instance = null;
            del = null;
            try
            {
                if (!TryInstanciateAndInjectDependencies($"BPEssentials.Commands.{typeName}", out instance, out var type))
                {
                    return false;
                }
                var method = type.GetMethod("Invoke");
                var types = method.GetParameters().Select(p => p.ParameterType);
                del = Delegate.CreateDelegate(Expression.GetActionType(types.ToArray()), instance, method.Name);
                return true;
            }
            catch (Exception ex)
            {
                Core.Instance.Logger.LogException(ex);
                return false;
            }
        }
    }
}
