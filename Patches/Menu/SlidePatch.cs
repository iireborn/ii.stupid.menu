/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

﻿using GorillaLocomotion;
using HarmonyLib;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(GTPlayer), nameof(GTPlayer.GetSlidePercentage))]
    public class SlidePatch
    {
        public static bool everythingSlippery;
        public static bool everythingGrippy;
        public static bool minimalSlip;

        public static void Postfix(GTPlayer __instance, ref float __result)
        {
            if (everythingSlippery)
                __result = 1;

            if (everythingGrippy)
                __result = 0;

            if (minimalSlip)
                __result *= 0.75f;
        }
    }
}