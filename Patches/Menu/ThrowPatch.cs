/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using UnityEngine;
using static iiMenu.Utilities.RandomUtilities;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(GrowingSnowballThrowable), nameof(GrowingSnowballThrowable.PerformSnowballThrowAuthority))]
    public class ThrowPatch
    {
        public static bool enabled;
        public static readonly int extraCount = 5;

        public static bool Prefix(GrowingSnowballThrowable __instance)
        {
            if (enabled)
            {
                enabled = false;

                Vector3 originalLinearVelocity = __instance.velocityEstimator.linearVelocity;
                for (int i = 0; i < extraCount; i++)
                {
                    __instance.velocityEstimator.linearVelocity = originalLinearVelocity + RandomVector3(2f);
                    __instance.PerformSnowballThrowAuthority();
                }

                enabled = true;

                return false;
            }

            return true;
        }
    }
}
