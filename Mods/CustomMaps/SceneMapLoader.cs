/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */


using UnityEngine.SceneManagement;

namespace iiMenu.Mods.CustomMaps
{
    public static class SceneMapLoader
    {
        public static void Init()
        {
            SceneMapRegistry.FillRegistry();
            SceneManager.activeSceneChanged += OnSceneChanged;
            CheckSceneForMap(SceneManager.GetActiveScene().name);
        }

        public static void OnSceneChanged(Scene oldScene, Scene newScene)
        {
            CheckSceneForMap(newScene.name);
        }

        public static void CheckSceneForMap(string sceneName)
        {
            var map = SceneMapRegistry.GetMapForScene(sceneName);
            if (map != null)
                Manager.UpdateCustomMapsTab(map.MapID);
            else
                Manager.UpdateCustomMapsTab();
        }
    }
}
