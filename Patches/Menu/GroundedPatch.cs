/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

﻿using HarmonyLib;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(TakeMyHand_HandLink), nameof(TakeMyHand_HandLink.LocalUpdate))]
    public class GroundedPatch
    {
        public static bool enabled;

        public static void Postfix(TakeMyHand_HandLink __instance, bool isGroundedHand, bool isGroundedButt, bool isGripPressed, bool isReadyForGrabbing)
        {
            if (enabled)
                __instance.isGroundedHand = true;
        }
    }
}
