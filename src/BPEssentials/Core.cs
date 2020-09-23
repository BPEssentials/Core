using BPCoreLib.Interfaces;
using BPCoreLib.Util;
using BPEssentials.Configuration.Models;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.ExtendedPlayer;
using BPEssentials.ExtensionMethods;
using BPEssentials.Modules;
using BPEssentials.Utils;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;

namespace BPEssentials
{
    public class Core : Plugin
    {
        public static Core Instance { get; internal set; }

        public static string Version { get; } = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        public static string Git { get; } = "https://github.com/BPEssentials/Core";

        public static string[] Authors { get; } = { "PLASMA_chicken", "UserR00T" };

        // TODO: This can get confusing real fast, need a new name for this.
        public BPCoreLib.PlayerFactory.ExtendedPlayerFactory<PlayerItem> PlayerHandler { get; internal set; } = new ExtendedPlayerFactory();

        public ILogger Logger { get; } = new Logger();

        public Paths Paths { get; } = new Paths();

        public IReader<Settings> SettingsReader { get; } = new Reader<Settings>();

        public Settings Settings => SettingsReader.Parsed;

        public IReader<List<CustomCommand>> CustomCommandsReader { get; } = new Reader<List<CustomCommand>>();

        public I18n I18n { get; set; }

        public SvManager SvManager { get; set; }

        public ICooldownHandler KitsCooldownHandler { get; set; }

        public ICooldownHandler WarpsCooldownHandler { get; set; }

        private ICooldownHandler CommandsCooldownHandler { get; set; }

        public WarpHandler WarpHandler { get; set; }

        public KitHandler KitHandler { get; set; }

        public EntityHandler EntityHandler { get; set; }

        public ModuleHandler ModuleHandler { get; set; }

        // TODO think of a better name
        public readonly string CommandCoolDownType = "command";

        public Core()
        {
            Instance = this;
            Info = new PluginInfo("BPEssentials", "bpe")
            {
                Description = "Basic commands for powerful moderation.",
                Website = "https://bpessentials.github.io/Docs/"
            };
            KitsCooldownHandler = new CooldownHandler(Info.GroupNamespace + ":cooldowns:kits");
            WarpsCooldownHandler = new CooldownHandler(Info.GroupNamespace + ":cooldowns:warps");
            CommandsCooldownHandler = new CooldownHandler(Info.GroupNamespace + ":cooldowns:commands");

            WarpHandler = new WarpHandler();

            KitHandler = new KitHandler();

            OnReloadRequestAsync();
            SetCustomData();

            EntityHandler = new EntityHandler();
            EntityHandler.LoadEntities();

            ModuleHandler = new ModuleHandler();
            ModuleHandler.Initialize();

            EventsHandler.Add("bpe:reload", new Action(OnReloadRequestAsync));
            EventsHandler.Add("bpe:version", new Action<string>(OnVersionRequest));
            Logger.LogInfo($"BP Essentials {(IsDevelopmentBuild() ? "[DEVELOPMENT-BUILD] " : "")}v{Version} loaded in successfully!");
        }

        private void SetCustomData()
        {
            CustomData.AddOrUpdate("version", Version);
            CustomData.AddOrUpdate("devbuild", IsDevelopmentBuild());
        }

        public static bool IsDevelopmentBuild()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

        public void RegisterCommands()
        {
            foreach (var command in Settings.Commands)
            {
                Logger.LogInfo($"[C] Registering command {command.CommandName}..");
                if (!CommandInjection.TryGetCommandMethodDelegateByTypeName(command.CommandName, out var del,
                    out var instance))
                {
                    Logger.LogError($"[C] Cannot register command {command.CommandName}. Delegate was null.");
                    continue;
                }
                CommandHandler.RegisterCommand(command.Commands, del, (player, apiCommand) =>
                    {
                        if (command.Disabled)
                        {
                            player.TS("command_disabled", command.CommandName);
                            return false;
                        }

                        if (!player.GetExtendedPlayer().EnabledBypass)
                        {
                            if (!command.AllowWhileDead && player.IsDead)
                            {
                                player.TS("command_failed_crimes", command.CommandName);
                                return false;
                            }

                            if (!command.AllowWhileCuffed && player.IsRestrained)
                            {
                                player.TS("command_failed_cuffed", command.CommandName);
                                return false;
                            }

                            if (!command.AllowWhileJailed && player.svPlayer.job.info.shared.jobIndex == BPAPI.Instance.PrisonerIndex)
                            {
                                player.TS("command_failed_jail", command.CommandName);
                                return false;
                            }

                            if (!command.AllowWithCrimes && player.wantedLevel != 0)
                            {
                                player.TS("command_failed_crimes", command.CommandName);
                                return false;
                            }

                            if (command.CoolDown > 0 && CommandsCooldownHandler.IsCooldown(player.svPlayer, command.CommandName, command.CoolDown))
                            {
                                //TODO localtion
                                player.TS("command_failed_cooldown", command.CommandName,
                                    CommandsCooldownHandler.GetCooldown(player.svPlayer, command.CommandName, command.CoolDown));
                                return false;
                            }
                        }
                        if (command.CoolDown > 0)
                        {
                            CommandsCooldownHandler.AddCooldown(player.svPlayer, command.CommandName);
                        }
                        return true;
                    }, command.CommandName);
            }
        }

        public void RegisterCustomCommands()
        {
            foreach (var customCommand in CustomCommandsReader.Parsed)
            {
                Logger.LogInfo($"[CC] Registering custom command(s) {string.Join(", ", customCommand.Commands)} by name '{customCommand.Name}'..");
                var permission = "cc." + customCommand.Name;
                CommandHandler.RegisterCommand(customCommand.Commands, new Action<ShPlayer>((player) =>
                {
                    player.SendChatMessage(customCommand.Response);
                }), null, permission);
            }
        }

        public void SetupI18n()
        {
            I18n = new I18n();
            I18n.ParseLocalization(Paths.LocalizationFile);
            Logger.LogInfo("I18n loaded!");
        }

        public void SetConfigurationFilePaths()
        {
            SettingsReader.Path = Paths.SettingsFile;
            CustomCommandsReader.Path = Paths.CustomCommandsFile;
        }

        public void ReadConfigurationFiles()
        {
            SettingsReader.ReadAndParse();
            CustomCommandsReader.ReadAndParse();
        }

        public async void OnReloadRequestAsync()
        {
            SetConfigurationFilePaths();
            await FileChecker.CheckFiles();
            ReadConfigurationFiles();
            RegisterCustomCommands();
            RegisterCommands();
            SetupI18n();
            WarpHandler.ReloadAll();
            KitHandler.ReloadAll();
        }

        public void OnVersionRequest(string callback)
        {
            if (callback.StartsWith("bpe:"))
            {
                return;
            }
            EventsHandler.Exec(callback, Version, IsDevelopmentBuild());
        }
    }
}