/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

﻿using GorillaLocomotion;
using HarmonyLib;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(GTPlayer), nameof(GTPlayer.TakeMyHand_ProcessMovement))]
    public class HandLinkPatch
    {
        public static bool enabled;

        public static bool Prefix(GTPlayer __instance) =>
            !enabled;
    }
}
