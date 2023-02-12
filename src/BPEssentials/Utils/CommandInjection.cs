using BPCoreLib.Interfaces;
using BPCoreLib.PlayerFactory;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.ExtendedPlayer;
using System;
using System.Linq;
using System.Linq.Expressions;


namespace BPEssentials.Utils
{
    public static class CommandInjection
    {
        public static bool TryInstantiateAndInjectDependencies(string typeName, out Abstractions.BpeCommand instance, out Type type)
        {
            try
            {
                type = Type.GetType(typeName);
                instance = (Abstractions.BpeCommand)Activator.CreateInstance(type);
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

        public static Abstractions.BpeCommand InjectDependenciesIntoCommand(Abstractions.BpeCommand bpeCommand, ILogger logger, Settings settings, ExtendedPlayerFactory<PlayerItem> extendedPlayerFactory)
        {
            bpeCommand.Logger = logger;
            bpeCommand.Settings = settings;
            bpeCommand.PlayerFactory = extendedPlayerFactory;
            return bpeCommand;
        }

        public static bool TryGetCommandMethodDelegateByTypeName(string typeName, out Delegate del, out Abstractions.BpeCommand instance)
        {
            instance = null;
            del = null;
            try
            {
                if (!TryInstantiateAndInjectDependencies($"BPEssentials.Commands.{typeName}", out instance, out var type))
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
