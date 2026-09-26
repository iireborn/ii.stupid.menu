/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
﻿using System.Collections.Generic;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(GorillaTagManager), nameof(GorillaTagManager.ReportTag))]
    public class ReportTagPatch
    {
        public static readonly List<NetPlayer> blacklistedPlayers = new List<NetPlayer>();
        public static readonly List<NetPlayer> invinciblePlayers = new List<NetPlayer>();

        public static bool Prefix(NetPlayer taggedPlayer, NetPlayer taggingPlayer)
        {
            return !blacklistedPlayers.Contains(taggingPlayer) && !invinciblePlayers.Contains(taggedPlayer);
        }
    }
}
