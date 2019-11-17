using BPCoreLib.Interfaces;
using BPCoreLib.Util;
using BPEssentials.Configuration.Models;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.ExtendedPlayer;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace BPEssentials
{
    public class Core : Plugin
    {
        public static Core Instance { get; internal set; }

        public static string Version { get; } = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        // TODO: This can get confusing real fast, need a new name for this.
        public BPCoreLib.PlayerFactory.ExtendedPlayerFactory<PlayerItem> PlayerHandler { get; internal set; } = new ExtendedPlayerFactory();

        public ILogger Logger { get; } = new Logger();

        public Paths Paths { get; } = new Paths();

        public IReader<Settings> SettingsReader { get; } = new Reader<Settings>();

        public Settings Settings => SettingsReader.Parsed;

        public IReader<List<CustomCommand>> CustomCommandsReader { get; } = new Reader<List<CustomCommand>>();

        public Announcer Announcer { get; set; }

        public I18n I18n { get; set; }
        
        public SvManager SvManager { get; set; }

        public Dictionary<ulong, Dictionary<string, Dictionary<string, int>>> Cooldowns { get; set; } = new Dictionary<ulong, Dictionary<string, Dictionary<string, int>>>();

        public WarpHandler WarpHandler { get; set; }

        public Core()
        {
            Instance = this;
            Info = new PluginInfo("BPEssentials", "BPE", new List<PluginAuthor> { new PluginAuthor("UserR00T"), new PluginAuthor("PLASMA_chicken") })
            {
                Description = "Basic commands for powerful moderation.",
                Git = "https://github.com/UserR00T/BP-Essentials/",
                Website = "https://userr00t.github.io/BP-Essentials/"
            };

            OnReloadRequestAsync();
            SetCutsomData();

            WarpHandler = new WarpHandler();
            WarpHandler.LoadAll(true);

            EventsHandler.Add("bpe:reload", new Action(OnReloadRequestAsync));
            EventsHandler.Add("bpe:version", new Action<string>(OnVersionRequest));
            Logger.LogInfo($"BP Essentials {(IsDevelopmentBuild() ? "[DEVELOPMENT-BUILD] " : "")}v{Version} loaded in successfully!");
        }

        private void SetCutsomData()
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
                var name = "bpe:c:" + command.CommandName;
                if (!Util.TryGetCommandMethodDelegateByTypeName(command.CommandName, out var del, out var instance))
                {
                    Logger.LogError($"[C] Cannot register command {command.CommandName}. Delegate was null.");
                    continue;
                }
                CommandHandler.RemoveCommand(name);
                CommandHandler.RegisterCommand(name, command.Commands, del, (player, apiCommand) =>
                {
                    if (command.Disabled)
                    {
                        player.SendChatMessage("Command disabled");
                        return false;
                    }
                    // TODO: implement allowwhileX here
                    return true;
                }, instance.LastArgSpaces);
            }
        }

        public void RegisterCustomCommands()
        {
            foreach (var customCommand in CustomCommandsReader.Parsed)
            {
                Logger.LogInfo($"[CC] Registering custom command(s) {string.Join(", ", customCommand.Commands)}..");
                var name = "bpe:cc:" + customCommand.Name;
                CommandHandler.RemoveCommand(name);
                CommandHandler.RegisterCommand(name, customCommand.Commands, new Action<ShPlayer>((player) =>
                {
                    player.SendChatMessage(customCommand.Response);
                }));
            }
        }

        public void SetupAnnouncer()
        {
            Announcer = new Announcer(Settings.General.AnnounceInterval * 1000, Settings.Announcements);
            Logger.LogInfo("Announcer started!");
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
            SetupAnnouncer();
            SetupI18n();
        }

        public void OnVersionRequest(string callback)
        {
            if (callback.StartsWith("bpe:"))
            {
                return;
            }
            EventsHandler.Call(callback, Version, IsDevelopmentBuild());
        }
    }
}
