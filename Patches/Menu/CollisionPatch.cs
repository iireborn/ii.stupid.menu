/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using System;
using UnityEngine;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(SlingshotProjectile), nameof(SlingshotProjectile.OnCollisionEnter))]
    public class CollisionPatch
    {
        public static event Action<SlingshotProjectile, Collision> OnCollisionEnterEvent;

        private static void Prefix(SlingshotProjectile __instance, Collision collision)
        {
            if (__instance != null && !__instance.dontDestroyOnHit && __instance.particleLaunched)
                OnCollisionEnterEvent?.Invoke(__instance, collision);
        }
    }
}
