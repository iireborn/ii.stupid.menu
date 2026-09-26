/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

﻿using GorillaLocomotion.Climbing;
using HarmonyLib;
using UnityEngine;

namespace iiMenu.Patches.Menu
{
    public class VelocityPatches
    {
        public static bool enabled;
        public static float multipleFactor;

        [HarmonyPatch(typeof(GorillaVelocityTracker), nameof(GorillaVelocityTracker.GetAverageVelocity))]
        public class VelocityPatch
        {
            public static void Postfix(GorillaVelocityTracker __instance, ref Vector3 __result, bool worldSpace = false, float maxTimeFromPast = 0.15f, bool doMagnitudeCheck = false)
            {
                if (enabled)
                    __result *= multipleFactor;
            }
        }

        [HarmonyPatch(typeof(GorillaVelocityTracker), nameof(GorillaVelocityTracker.GetLatestVelocity))]
        public class VelocityPatch2
        {
            public static void Postfix(GorillaVelocityTracker __instance, ref Vector3 __result, bool worldSpace = false)
            {
                if (enabled)
                    __result *= multipleFactor;
            }
        }

        [HarmonyPatch(typeof(GorillaVelocityEstimator), nameof(GorillaVelocityEstimator.TriggeredLateUpdate))]
        public class VelocityPatch3
        {
            public static void Postfix(GorillaVelocityEstimator __instance)
            {
                if (enabled)
                {
                    __instance.linearVelocity *= multipleFactor;
                    __instance.angularVelocity *= multipleFactor;
                    __instance.handPos *= multipleFactor;
                }
            }
        }
    }
}
