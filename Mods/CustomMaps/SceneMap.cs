/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */


namespace iiMenu.Mods.CustomMaps
{
    public class SceneMap
    {
        public long MapID { get; }
        public string SceneName { get; }

        public SceneMap(long mapID, string sceneName)
        {
            MapID = mapID;
            SceneName = sceneName;
        }
    }
}
