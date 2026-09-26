/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using GorillaTagScripts;
using HarmonyLib;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(LurkerGhost), nameof(LurkerGhost.ChangeState))]
    public class LurkerPatch
    {
        public static bool enabled;

        public static bool Prefix(LurkerGhost __instance, LurkerGhost.ghostState newState) =>
            !(enabled &&
                newState == LurkerGhost.ghostState.possess &&
                __instance.targetPlayer == NetworkSystem.Instance.LocalPlayer);

    }
}
