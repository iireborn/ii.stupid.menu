/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using Photon.Pun;
﻿using System.Collections.Generic;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(RequestableOwnershipGuard), nameof(RequestableOwnershipGuard.OwnershipRequested))]
    public class OwnershipPatch
    {
        public static bool enabled;
        public static readonly List<RequestableOwnershipGuard> blacklistedGuards = new List<RequestableOwnershipGuard>();

        public static bool Prefix(RequestableOwnershipGuard __instance, string nonce, PhotonMessageInfo info) =>
            !enabled || (__instance.photonView.IsMine && !blacklistedGuards.Contains(__instance));
    }
}
