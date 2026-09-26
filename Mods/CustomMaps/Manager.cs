/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using GorillaTagScripts.VirtualStumpCustomMaps;
using iiMenu.Classes.Menu;
using iiMenu.Managers;
using iiMenu.Menu;
using iiMenu.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace iiMenu.Mods.CustomMaps
{
    public static class Manager
    {
        public static readonly Dictionary<long, string> mapScriptArchives = new Dictionary<long, string>();
        public static readonly Dictionary<long, CustomMap> mapCache = new Dictionary<long, CustomMap>();

        public static long? currentMapId;

        public static void UpdateCustomMapsTab(long? overwriteId = null)
        {
            currentMapId = overwriteId;

            int category = Buttons.GetCategory("Custom Maps");
            List<ButtonInfo> buttons = new List<ButtonInfo> { new ButtonInfo { buttonText = "Exit Custom Maps", method = () => Buttons.CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns you back to the main page." } };

            if (overwriteId != null)
            {
                long mapID = overwriteId.Value;
                if (!mapScriptArchives.ContainsKey(mapID))
                    mapScriptArchives.Add(mapID, CustomGameMode.LuaScript);

                CustomMap map = GetMapByID(mapID);
                if (map == null)
                    buttons.Add(new ButtonInfo { buttonText = "This map is not supported yet.", label = true });
                else
                    buttons.AddRange(map.Buttons);

                buttons.AddRange(new[]
                {
                    new ButtonInfo { buttonText = " ", label = true },
                    new ButtonInfo { buttonText = "Edit Custom Script", method = EditUserScript, isTogglable = false, toolTip = "Opens your custom script for this map." },
                    new ButtonInfo { buttonText = "Delete Custom Script", method = DeleteUserScript, isTogglable = false, toolTip = "Deletes your custom script for this map." },
                    new ButtonInfo { buttonText = "Run Custom Script", enableMethod = StartUserScript, disableMethod = StopUserScript, toolTip = "Runs your custom script for this map." }
                });
            }
            else
                buttons.Add(new ButtonInfo { buttonText = "You have not loaded a map.", label = true });

            Buttons.buttons[category] = buttons.ToArray();
        }

        public static void ModifyCustomScript(Dictionary<int, string> replacements)
        {
            string input = CustomGameMode.LuaScript;
            string[] lines = input.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            foreach (var (lineIndex, value) in replacements)
            {
                if (lineIndex < 0 || lineIndex >= lines.Length) continue;
                LogManager.Log("Replacing " + lines[lineIndex] + " with " + value);
                lines[lineIndex] = value;
            }

            CustomGameMode.LuaScript = string.Join(Environment.NewLine, lines);

            if (NetworkSystem.Instance.InRoom)
                LuauHud.Instance.RestartLuauScript();

            CustomMapManager.ReturnToVirtualStump();
        }

        public static void EditUserScript()
        {
            string DirectoryTarget = FileUtilities.GetGamePath() + $"/{PluginInfo.BaseDirectory}/CustomScripts/{CustomMapLoader.LoadedMapModId}.luau";
            if (!File.Exists(DirectoryTarget))
                File.WriteAllText(DirectoryTarget, mapScriptArchives[CustomMapManager.currentRoomMapModId]);
            Process.Start(DirectoryTarget);
        }

        public static void DeleteUserScript()
        {
            string DirectoryTarget = FileUtilities.GetGamePath() + $"/{PluginInfo.BaseDirectory}/CustomScripts/{CustomMapLoader.LoadedMapModId}.luau";
            if (File.Exists(DirectoryTarget))
                File.Delete(DirectoryTarget);
        }

        public static void StartUserScript()
        {
            string DirectoryTarget = FileUtilities.GetGamePath() + $"/{PluginInfo.BaseDirectory}/CustomScripts/{CustomMapLoader.LoadedMapModId}.luau";
            if (File.Exists(DirectoryTarget))
                CustomGameMode.LuaScript = File.ReadAllText(DirectoryTarget);

            if (NetworkSystem.Instance.InRoom)
                LuauHud.Instance.RestartLuauScript();

            CustomMapManager.ReturnToVirtualStump();
        }

        public static void StopUserScript()
        {
            CustomGameMode.LuaScript = mapScriptArchives[CustomMapManager.currentRoomMapModId];

            if (NetworkSystem.Instance.InRoom)
                LuauHud.Instance.RestartLuauScript();

            CustomMapManager.ReturnToVirtualStump();
        }

        public static void RevertCustomScript(int[] lines)
        {
            Dictionary<int, string> replacements = lines.ToDictionary(line => line, line => mapScriptArchives[CustomMapManager.currentRoomMapModId].Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)[line]);
            ModifyCustomScript(replacements);
        }

        public static void RevertCustomScript(int line) =>
            RevertCustomScript(new[] { line });

        public static CustomMap GetMapByID(long id)
        {
            if (mapCache.TryGetValue(id, out var instance)) return instance;
            var mapTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(CustomMap)));

            foreach (var type in mapTypes)
            {
                CustomMap mapInstance = (CustomMap)Activator.CreateInstance(type);
                if (mapInstance.MapID != id) continue;
                instance = mapInstance;
                break;
            }

            mapCache[id] = instance;

            return instance;
        }
    }
}
