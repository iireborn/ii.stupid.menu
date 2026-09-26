/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
﻿using System.Collections.Generic;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(RankedProgressionManager), nameof(RankedProgressionManager.SetLocalProgressionData))]
    public class SetRankedPatch
    {
        public static bool enabled;

        public static bool Prefix(RankedProgressionManager __instance, GorillaTagCompetitiveServerApi.RankedModePlayerProgressionData data)
        {
            if (enabled)
            {
                Dictionary<int, int[]> tierData = new Dictionary<int, int[]> 
                {
                    { 0, new[] { 0, 0 } },
                    { 1, new[] { 0, 1 } },

                    { 2, new[] { 1, 0 } },
                    { 3, new[] { 1, 1 } },
                    { 4, new[] { 1, 2 } },

                    { 5, new[] { 2, 0 } },
                    { 6, new[] { 2, 1 } },
                    { 7, new[] { 2, 2 } },
                };

                foreach (GorillaTagCompetitiveServerApi.RankedModeProgressionPlatformData platformData in data.platformData)
                {
                    platformData.elo = Mods.Safety.targetElo;
                    platformData.majorTier = tierData[Mods.Safety.targetBadge][0];
                    platformData.minorTier = tierData[Mods.Safety.targetBadge][1];
                }
                __instance.ProgressionData = data;
                return false;
            }

            return true;
        }
    }
}
