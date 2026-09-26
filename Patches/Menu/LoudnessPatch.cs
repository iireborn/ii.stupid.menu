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
    [HarmonyPatch(typeof(GorillaSpeakerLoudness), nameof(GorillaSpeakerLoudness.UpdateLoudness))]
    public class LoudnessPatch
    {
        public static bool enabled;

        private static bool Prefix(GorillaSpeakerLoudness __instance, ref bool ___isMicEnabled, ref bool ___isSpeaking, ref float ___loudness) =>
            !enabled || __instance.gameObject.name != "Local Gorilla Player";
    }
}
