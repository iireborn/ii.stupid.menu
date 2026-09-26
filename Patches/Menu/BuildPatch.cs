/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

﻿using HarmonyLib;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(BuilderPieceInteractor), nameof(BuilderPieceInteractor.UpdateHandState))]
    public class BuildPatch
    {
        public static bool enabled;
        public static float previous;
        public static float previous2;

        private static void Prefix()
        {
            if (enabled)
            {
                previous = VRRig.LocalRig.NativeScale;
                previous2 = VRRig.LocalRig.ScaleMultiplier;
                VRRig.LocalRig.NativeScale = 1f;
                VRRig.LocalRig.ScaleMultiplier = 1f;
            }
        }

        private static void Postfix()
        {
            if (enabled)
            {
                VRRig.LocalRig.NativeScale = previous;
                VRRig.LocalRig.ScaleMultiplier = previous2;
            }
        }
    }
}
