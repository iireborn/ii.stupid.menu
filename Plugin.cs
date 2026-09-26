/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using BepInEx;
using BepInEx.Logging;
using iiMenu.Classes.Menu;
using iiMenu.Managers;
using iiMenu.Menu;
using iiMenu.Patches;
using iiMenu.Patches.Menu;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UnityEngine;
using Console = System.Console;

namespace iiMenu
{
    [Description(PluginInfo.Description)]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        public static ManualLogSource PluginLogger => instance.Logger;
        public static bool FirstLaunch;

        /// <summary>
        /// One-time data migration: renames the legacy directory ("iisStupidMenu") to the
        /// current <see cref="PluginInfo.BaseDirectory"/>. Rename-only by design — if BOTH
        /// directories exist (e.g. someone alternated legacy and Reborn builds), nothing is
        /// merged or overwritten; both are left untouched and Reborn keeps its own.
        /// </summary>
        private static void MigrateLegacyBaseDirectory()
        {
            try
            {
                if (!Directory.Exists(PluginInfo.LegacyBaseDirectory))
                    return;

                if (Directory.Exists(PluginInfo.BaseDirectory))
                {
                    LogManager.Log($"[Migrate] Both '{PluginInfo.LegacyBaseDirectory}' and '{PluginInfo.BaseDirectory}' exist.");
                    return;
                }

                Directory.Move(PluginInfo.LegacyBaseDirectory, PluginInfo.BaseDirectory);
                LogManager.Log($"[Migrate] Renamed data directory '{PluginInfo.LegacyBaseDirectory}' -> '{PluginInfo.BaseDirectory}'.");
            }
            catch (System.Exception e)
            {
                LogManager.Log($"[Migrate] Failed to migrate '{PluginInfo.LegacyBaseDirectory}': {e.Message}");
            }
        }

        private void Awake()
        {
            // Set console title
            Console.Title = $"ii Reborn // Build {PluginInfo.Version}";
            instance = this;
            Application.quitting += OnApplicationQuitting;

            string logoLines = PluginInfo.Logo.Split(@"
")
                .Aggregate("", (current, line) => current + (System.Environment.NewLine + "     " + line));

            LogManager.Log($@"
{logoLines}
    ii Reborn  {(PluginInfo.BetaBuild ? "Beta " : "Build")} {PluginInfo.Version}
    Compiled {PluginInfo.BuildTimestamp}
    
    This program comes with ABSOLUTELY NO WARRANTY;
    for details see `https://github.com/iireborn/menu/GPL/WARRANTY`
    
    This is free software, and you are welcome to redistribute it under certain conditions;
    see `https://github.com/iireborn/menu/GPL/REDISTRIBUTION` for details.
");

            MigrateLegacyBaseDirectory();

            FirstLaunch = !Directory.Exists(PluginInfo.BaseDirectory);

            string[] ExistingDirectories = {
                "",
                "/Sounds",
                "/Plugins",
                "/Backups",
                "/Macros",
                "/TTS",
                "/PlayerInfo",
                "/CustomScripts",
                "/Friends",
                "/Friends/Messages",
                "/Achievements"
            };

            foreach (string DirectoryString in ExistingDirectories)
            {
                string DirectoryTarget = $"{PluginInfo.BaseDirectory}{DirectoryString}";
                if (!Directory.Exists(DirectoryTarget))
                    Directory.CreateDirectory(DirectoryTarget);
            }

            FirstLaunch = false;

            PatchHandler.PatchAll(true);

            // Ugily hard-coded but works so well
            if (File.Exists($"{PluginInfo.BaseDirectory}/iiMenu_Preferences.txt"))
            {
                if (File.ReadAllLines($"{PluginInfo.BaseDirectory}/iiMenu_Preferences.txt")[0].Split(";;").Contains("Accept TOS"))
                    TOSPatches.enabled = true;
            }

            if (File.Exists($"{PluginInfo.BaseDirectory}/iiMenu_DisableTelemetry.txt"))
                ServerData.DisableTelemetry = true;
            
            GorillaTagger.OnPlayerSpawned(LoadMenu);
        }

        private void OnApplicationQuitting()
        {
            try { IiServersManager.ShutdownForGameExit(); } catch { }
        }

        private void OnDestroy()
        {
            Application.quitting -= OnApplicationQuitting;
            try { IiServersManager.ShutdownForGameExit(); } catch { }
            Main.UnloadMenu();
            try { Utilities.AssetUtilities.ReleaseAll(); } catch { }
        }

        private static void LoadMenu()
        {
            PatchHandler.PatchAll();

            GameObject Loader = new GameObject("iiMenu_Loader");
            Loader.AddComponent<CoroutineManager>();
            Loader.AddComponent<NotificationManager>();
            Loader.AddComponent<CustomBoardManager>();
            Loader.AddComponent<UI>();

            DontDestroyOnLoad(Loader);

            if (CoroutineManager.instance != null)
                CoroutineManager.instance.StartCoroutine(iiMenu.Mods.Important.MapStateReport());
        }

        // For SharpMonoInjector usage
        // Don't merge these methods, it just doesn't work
        public static void Inject()
        {
            GameObject iiMenu = new GameObject("iiMenu");
            iiMenu.AddComponent<Plugin>();
        }

        public static void InjectDontDestroy()
        {
            GameObject iiMenu = new GameObject("iiMenu");
            iiMenu.AddComponent<Plugin>();
            DontDestroyOnLoad(iiMenu);
        }
    }
}
