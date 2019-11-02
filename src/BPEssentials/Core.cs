using BPCoreLib.Interfaces;
using BPCoreLib.Util;
using BPEssentials.Configuration.Models;
using BPEssentials.Configuration.Models.SettingsModel;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace BPEssentials
{
    public class Core : Resource
    {
        public static string Version { get; } = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        // TODO: This can get confusing real fast, need a new name for this.
        public BPCoreLib.PlayerFactory.ExtendedPlayerFactory PlayerHandler { get; internal set; } = new ExtendedPlayer.ExtendedPlayerFactory();

        public ILogger Logger { get; } = new Logger();

        public Paths Paths { get; } = new Paths();

        public static Core Instance { get; internal set; }

        public IReader<Settings> SettingsReader { get; } = new Reader<Settings>();

        public Settings Settings => SettingsReader.Parsed;

        public IReader<List<CustomCommand>> CustomCommandsReader { get; } = new Reader<List<CustomCommand>>();

        public Announcer Announcer { get; set; }

        public Core()
        {
            Instance = this;
            Info = new ResourceInfo("BPEssentials", "BPE", new List<ResourceAuthor>() {new ResourceAuthor("UserR00t"), new ResourceAuthor("PLASMA_chicken")})
            {
                Description = "Basic commands for powerful moderation.",
                Github = "https://github.com/UserR00T/BP-Essentials/",
                Website = "https://userr00t.github.io/BP-Essentials/"
            };

            OnReloadRequest();

            EventsHandler.Add("bpe:reload", new Action(OnReloadRequest));
            EventsHandler.Add("bpe:version", new Action<string>(OnVersionRequest));
            Logger.LogInfo($"BP Essentials {(IsDevelopmentBuild() ? "[DEVELOPMENT-BUILD] " : "")}v{Version} loaded in successfully!");
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
                var del = Util.GetCommandMethodDelegateByTypeName(command.CommandName, out var instance);
                if (del == null)
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
            // TODO:
            // Announcer = new Announcer(Settings.General.AnnounceInterval * 1000, Settings.Announcements);
            Announcer = new Announcer(10 * 1000, new List<string> { "test", "test2" });
            Logger.LogInfo("Announcer started!");
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

        public void OnReloadRequest()
        {
            SetConfigurationFilePaths();
            ReadConfigurationFiles();
            RegisterCustomCommands();
            RegisterCommands();
            SetupAnnouncer();
        }

        public void OnVersionRequest(string callback)
        {
            if (callback.StartsWith("bpe:"))
                return;
            EventsHandler.Call(callback, Version, IsDevelopmentBuild());
        }
    }
}
