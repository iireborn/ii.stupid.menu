/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using UnityEngine;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(VRRig), nameof(VRRig.GrabbedByPlayer))]
    public class GrabPatch
    {
        public static bool enabled;

        public static bool Prefix(VRRig __instance, VRRig grabbedByRig, bool grabbedBody, bool grabbedLeftHand, bool grabbedWithLeftHand) =>
            !enabled;
    }

    [HarmonyPatch(typeof(VRRig), nameof(VRRig.DroppedByPlayer))]
    public class DropPatch
    {
        public static bool Prefix(VRRig __instance, VRRig grabbedByRig, Vector3 throwVelocity) =>
            !GrabPatch.enabled;
    }

    [HarmonyPatch(typeof(GuardianRPCs), nameof(GuardianRPCs.GuardianLaunchPlayer))]
    public class LaunchPatch
    {
        public static bool Prefix(Vector3 velocity) =>
            !GrabPatch.enabled;
    }
}
