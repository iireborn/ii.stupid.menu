/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using iiMenu.Mods;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(SnowballThrowable), nameof(SnowballThrowable.GetRandomModelIndex))]
    public class IndexPatch
    {
        public static bool enabled;

        public static bool Prefix(SnowballThrowable __instance, ref int __result)
        {
            if (enabled)
            {
                if (__instance.localModels.Count == 0)
                {
                    __result = -1;
                    return false;
                }

                __instance.randModelIndex = Projectiles.targetProjectileIndex % __instance.localModels.Count;
                __result = Projectiles.targetProjectileIndex;

                return false;
            }

            return true;
        }
    }
}
