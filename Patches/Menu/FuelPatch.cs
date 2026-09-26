/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(SIGadgetWristJet), nameof(SIGadgetWristJet.OnUpdateAuthority))]
    public class FuelPatch
    {
        public static bool enabled;

        public static void Postfix(SIGadgetWristJet __instance, float dt)
        {
            if (enabled)
                __instance.currentFuel = __instance.fuelSize;
        }
    }
}
