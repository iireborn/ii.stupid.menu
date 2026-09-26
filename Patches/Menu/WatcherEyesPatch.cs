/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using GorillaLocomotion;
using HarmonyLib;
using UnityEngine;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(HalloweenWatcherEyes), nameof(HalloweenWatcherEyes.Update))]
    public class WatcherEyesPatch
    {
        public static bool enabled;

        public static bool Prefix(HalloweenWatcherEyes __instance)
        {
            if (enabled)
            {
                Quaternion normalized = Quaternion.LookRotation((GTPlayer.Instance.headCollider.transform.position - __instance.transform.position).normalized);
                __instance.leftEye.transform.rotation = normalized;
                __instance.rightEye.transform.rotation = normalized;
                return false;
            }
            return true;
        }
    }
}
