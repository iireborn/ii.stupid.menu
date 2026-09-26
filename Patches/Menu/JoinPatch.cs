/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using System.Collections.Generic;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(GameEntityManager), nameof(GameEntityManager.JoinWithItems))]
    public class JoinPatch
    {
        public static bool enabled;
        public static bool Prefix(List<GameEntity> entities) =>
            !enabled;
    }
}
