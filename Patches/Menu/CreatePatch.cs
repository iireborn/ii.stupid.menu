/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

﻿using GorillaTagScripts;
using HarmonyLib;
using iiMenu.Mods;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(BuilderTableNetworking), nameof(BuilderTableNetworking.PieceCreatedByShelfRPC))]
    public class CreatePatch
    {
        public static bool enabled;
        public static int pieceTypeSearch;

        private static void Postfix(int pieceType, int pieceId)
        {
            if (enabled)
            {
                if (pieceTypeSearch == pieceType)
                {
                    Fun.pieceId = pieceId;
                    enabled = false;
                }
            }
        }
    }
}
