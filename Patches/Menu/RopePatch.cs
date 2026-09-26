/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

﻿using GorillaLocomotion.Gameplay;
using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(GorillaRopeSwing), nameof(GorillaRopeSwing.AttachLocalPlayer))]
    public class RopePatch
    {
        public static bool enabled;
        public static float amplifier = 5f;

        public static void Prefix(XRNode xrNode, Transform grabbedBone, Vector3 offset, ref Vector3 velocity)
        {
            if (enabled)
                velocity *= amplifier;
        }
    }
}
