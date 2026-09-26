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
    [HarmonyPatch(typeof(PhotonNetworkController), nameof(PhotonNetworkController.AttemptToJoinRankedPublicRoom))]
    public class RankedPatch
    {
        public static bool enabled;
        public static string targetPlatform;
        public static string targetTier;

        public static bool Prefix(GorillaNetworkJoinTrigger triggeredTrigger, JoinType roomJoinType = JoinType.Solo)
        {
            if (enabled)
            {
                PhotonNetworkController.Instance.AttemptToJoinRankedPublicRoomAsync(
                    triggeredTrigger,
                    targetTier ?? RankedProgressionManager.Instance.GetRankedMatchmakingTier().ToString(),
                    targetPlatform ?? "PC",
                    roomJoinType
                );

                return false;
            }
            return true;
        }
    }
}
