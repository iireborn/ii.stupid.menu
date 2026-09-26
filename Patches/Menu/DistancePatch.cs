/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using iiMenu.Menu;
using iiMenu.Utilities;
using UnityEngine;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(VRRig), nameof(VRRig.IsPositionInRange))]
    public class DistancePatch
    {
        public static bool enabled;

        public static void Postfix(VRRig __instance, ref bool __result, Vector3 position, float range)
        {
            NetPlayer player = RigUtilities.GetPlayerFromVRRig(__instance) ?? null;
            if ((enabled && __instance.isLocal) || (player != null && Main.ShouldBypassChecks(player)))
                __result = true;
        }
    }
}
