/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

﻿using HarmonyLib;
using UnityEngine;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(PropHuntHandFollower), nameof(PropHuntHandFollower.GeoCollisionPoint))]
    public class PropPatch
    {
        public static bool enabled;

        public static void Postfix(ref Vector3 __result, Vector3 sourcePos, Vector3 targetPos)
        {
            if (enabled)
                __result = targetPos;
        }
    }
}
