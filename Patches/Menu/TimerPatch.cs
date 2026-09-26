/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using UnityEngine;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(GorillaTagger), nameof(GorillaTagger.LateUpdate))]
    public class TimerPatch
    {
        public static bool enabled;
        private static float oldDeltaTime;

        public static void Prefix(GorillaTagger __instance)
        {
            if (enabled)
            {
                oldDeltaTime = Time.fixedDeltaTime;
                __instance._framerateUpdated = true;
                Time.fixedDeltaTime = 1 / UnityEngine.XR.XRDevice.refreshRate;
            }
        }

        public static void Postfix(GorillaTagger __instance)
        {
            if (enabled)
                Time.fixedDeltaTime = oldDeltaTime;
        }
    }
}