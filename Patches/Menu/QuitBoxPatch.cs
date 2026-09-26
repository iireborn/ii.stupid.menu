/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

﻿using HarmonyLib;
using iiMenu.Mods;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(GorillaQuitBox), nameof(GorillaQuitBox.OnBoxTriggered))]
    public class QuitBoxPatch
    {
        public static bool enabled = true;
        public static bool teleportToStump;

        public static bool Prefix()
        {
            if (teleportToStump)
            {
                Movement.TeleportToMap(Movement.mapData[0][1], Movement.mapData[0][2]);
                return false;
            }

            return enabled;
        }
    }
}
