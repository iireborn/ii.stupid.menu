/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */


using System.Collections.Generic;

namespace iiMenu.Mods.CustomMaps
{
    public static class SceneMapRegistry
    {
        public static readonly Dictionary<string, SceneMap> sceneMapLookup = new Dictionary<string, SceneMap>();

        public static void RegisterMap(long mapID, string sceneName)
        {
            if (!sceneMapLookup.ContainsKey(sceneName))
                sceneMapLookup.Add(sceneName, new SceneMap(mapID, sceneName));
        }

        public static SceneMap GetMapForScene(string sceneName)
        {
            sceneMapLookup.TryGetValue(sceneName, out var map);
            return map;
        }

        public static void FillRegistry()
        {
            RegisterMap(5107228, "monke-magic-halloween-alt"); 
            RegisterMap(5135423, "Guns");          
            RegisterMap(5024157, "Flight-Simulator");          
            RegisterMap(4977315, "MiningSimulator");          
        }
    }
}
