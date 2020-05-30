using BPCoreLib.Interfaces;
using BPCoreLib.Util;
using BPEssentials.Configuration.Models;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.Cooldowns;
using BPEssentials.ExtendedPlayer;
using BPEssentials.Modules;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using BrokeProtocol.Utility.Jobs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using BrokeProtocol.Utility;

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

        public IReader<List<CustomTriggers>> CustomEventReader { get; } = new Reader<List<CustomTriggers>>();

        public Announcer Announcer { get; set; }

        public I18n I18n { get; set; }

        public SvManager SvManager { get; set; }

        public CooldownHandler CooldownHandler { get; set; }

        public WarpHandler WarpHandler { get; set; }

        public KitHandler KitHandler { get; set; }

        public EntityHandler EntityHandler { get; set; }

        public ModuleHandler ModuleHandler { get; set; }

        public Core()
        {
            Instance = this;
            Info = new PluginInfo("BPEssentials", "bpe")
            {
                Description = "Basic commands for powerful moderation.",
                Website = "https://bpessentials.github.io/Docs/"
            };
            CooldownHandler = new CooldownHandler();

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
            new Commands.Save().StartSaveTimer();
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
                if (!CommandInjection.TryGetCommandMethodDelegateByTypeName(command.CommandName, out var del, out var instance))
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
                        if (!command.AllowWhileJailed && player.job is Prisoner)
                        {
                            player.TS("command_failed_jail", command.CommandName);
                            return false;
                        }
                        if (!command.AllowWithCrimes && player.wantedLevel != 0)
                        {
                            player.TS("command_failed_crimes", command.CommandName);
                            return false;
                        }
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

        public void RegisterCustomTriggers()
        {
            var allPermGroup = "bpe_all_perms";
            if (!GroupManager.Groups.ContainsKey(allPermGroup))
            {
                Group newGroup = new Group();
                newGroup.Permissions.Add("*");
                GroupManager.Groups.Add("bpe_all_perms", newGroup);
            }
            else if (!GroupManager.Groups["bpe_all_perms"].Permissions.Contains("*"))
            {
                Core.Instance.Logger.LogWarning($"Bpe can not create group named {allPermGroup} because one such group may already exit and it does not have * perms");
            }

            foreach (var customEvent in CustomEventReader.Parsed)
            {
                Logger.LogInfo($"[CC] Registering custom trigger(s) {string.Join(", ", customEvent.Trigger)}");
                EventsHandler.Add(customEvent.Trigger, new Action<ShPlayer, string>((player, eventName) =>
                {
                    if (customEvent.PrivateResponse != null && customEvent.PrivateResponse != "")
                    {
                        player.SendChatMessage(customEvent.PrivateResponse, true);
                    }
                    if (customEvent.SendInChat != null && customEvent.SendInChat != "")
                    {
                        player.svPlayer.SvGlobalChatMessage(customEvent.SendInChat);
                    }
                    GroupManager.Groups[allPermGroup].AddMember(player);
                    foreach (var command in customEvent.Commands)
                    {
                        string[] array = command.Split('"').Select((string element, int index) => (index % 2 != 0) ? new string[1]
                        {
                            element
                        } : element.Split(Util.commandSeparators, StringSplitOptions.RemoveEmptyEntries)).SelectMany((string[] element) => element).ToArray();
                        string item = array[0].ToLowerInvariant();
                        int num = array.Length;
                        while (true)
                        {
                            if (CommandHandler.Commands.TryGetValue((item, num).GetHashCode(), out BrokeProtocol.API.Types.Command value))
                            {
                                value.Invoke(player, array, num);
                                return;
                            }
                            if (num < 2)
                            {
                                break;
                            }
                            array[num - 2] = string.Join(" ", array[num - 2], array[num - 1]);
                            num--;
                        }
                        if (CommandHandler.Commands.TryGetValue((item, 0).GetHashCode(), out BrokeProtocol.API.Types.Command value2))
                        {
                            value2.SendUsage(player);
                        }
                        else
                        {
                            player.svPlayer.SendGameMessage("Command Not Found, Please Make sure you configured triggers.josn correctly");
                        }
                    }
                    GroupManager.Groups[allPermGroup].RemoveMember(player);
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
            CustomEventReader.Path = Paths.TriggersFile;
        }

        public void ReadConfigurationFiles()
        {
            SettingsReader.ReadAndParse();
            CustomCommandsReader.ReadAndParse();
            CustomEventReader.ReadAndParse();
        }

        public async void OnReloadRequestAsync()
        {
            SetConfigurationFilePaths();
            await FileChecker.CheckFiles();
            ReadConfigurationFiles();
            RegisterCustomCommands();
            RegisterCustomTriggers();
            RegisterCommands();
            SetupAnnouncer();
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