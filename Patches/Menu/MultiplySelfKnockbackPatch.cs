/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using iiMenu.Extensions;
using iiMenu.Mods;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(SlingshotProjectile), nameof(SlingshotProjectile.CheckForAOEKnockback))]
    public class MultiplySelfKnockbackPatch
    {
        public static bool enabled;

        public static void Prefix(SlingshotProjectile __instance)
        {
            if (enabled && __instance.projectileOwner == VRRig.LocalRig.GetPlayer())
            {
                if (__instance.aoeKnockbackConfig != null)
                {
                    var config = __instance.aoeKnockbackConfig.Value;
                    config.knockbackVelocity = config.knockbackVelocity * Movement.multiplicationAmount / 10;
                    __instance.aoeKnockbackConfig = config;
                }
            }
        }
    }
}
