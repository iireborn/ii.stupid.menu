/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */
using HarmonyLib;
using GorillaNetworking;
using iiMenu.Managers;
using Photon.Voice.Unity;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(VoiceConnection), "ConnectUsingSettings")]
    public class BlockIiServersVoicePatch
    {
        static bool Prefix() => !IiServersManager.IsOnIiServers;
    }

    [HarmonyPatch(typeof(PhotonNetworkController), nameof(PhotonNetworkController.AttemptToJoinPublicRoom))]
    public class BlockIiServersPublicJoinPatch
    {
        static bool Prefix()
        {
            if (IiServersManager.IsOnIiServers)
                return false;
            return true;
        }
    }

    [HarmonyPatch(typeof(PhotonNetworkController), nameof(PhotonNetworkController.AttemptToJoinPublicRoomAsync))]
    public class BlockIiServersPublicJoinAsyncPatch
    {
        static bool Prefix()
        {
            if (IiServersManager.IsOnIiServers)
                return false;
            return true;
        }
    }

    [HarmonyPatch(typeof(PhotonNetworkController), nameof(PhotonNetworkController.AttemptToJoinRankedPublicRoom))]
    public class BlockIiServersRankedJoinPatch
    {
        static bool Prefix()
        {
            if (IiServersManager.IsOnIiServers)
                return false;
            return true;
        }
    }

    [HarmonyPatch(typeof(PhotonNetworkController), nameof(PhotonNetworkController.AttemptToJoinRankedPublicRoomAsync))]
    public class BlockIiServersRankedJoinAsyncPatch
    {
        static bool Prefix()
        {
            if (IiServersManager.IsOnIiServers)
                return false;
            return true;
        }
    }
}
