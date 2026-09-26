/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

﻿using GorillaNetworking;
using HarmonyLib;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(PhotonNetworkController), nameof(PhotonNetworkController.OnJoinedRoom))]
    public class JoinedRoomPatch
    {
        public static bool enabled;

        private static void Prefix()
        {
            if (enabled)
                PhotonNetworkController.Instance.currentJoinType = JoinType.FollowingParty;
        }
    }
}
