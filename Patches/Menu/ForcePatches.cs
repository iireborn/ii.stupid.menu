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
    public class ForcePatches
    {
        public static bool enabled;

        [HarmonyPatch(typeof(ForceVolume), nameof(ForceVolume.OnTriggerEnter))]
        public class OnTriggerEnter
        {
            public static bool Prefix() =>
                !enabled;
        }

        [HarmonyPatch(typeof(ForceVolume), nameof(ForceVolume.OnTriggerExit))]
        public class OnTriggerExit
        {
            public static bool Prefix() =>
                !enabled;
        }

        [HarmonyPatch(typeof(ForceVolume), nameof(ForceVolume.OnTriggerStay))]
        public class OnTriggerStay
        {
            public static bool Prefix() =>
                !enabled;
        }
    }
}
