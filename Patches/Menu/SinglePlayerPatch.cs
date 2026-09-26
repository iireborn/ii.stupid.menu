/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using Photon.Pun;
﻿using System.Threading.Tasks;
using UnityEngine;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(NetworkSystemPUN), nameof(NetworkSystemPUN.InternalDisconnect))]
    public class SinglePlayerPatch
    {
        public static bool enabled;

        private static bool Prefix(NetworkSystemPUN __instance, ref Task __result)
        {
            if (!enabled)
                return true;

            __instance.internalState = NetworkSystemPUN.InternalState.Internal_Disconnecting;
            PhotonNetwork.Disconnect();

            Object.Destroy(__instance.VoiceNetworkObject);
            __instance.UpdatePlayers();
            __instance.SinglePlayerStarted();

            __result = InternalDisconnect(__instance);

            return false;
        }

        private static async Task InternalDisconnect(NetworkSystemPUN instance)
        {
            await instance.WaitForStateCheck(NetworkSystemPUN.InternalState.Internal_Disconnected);
            instance.internalState = NetworkSystemPUN.InternalState.Idle;
        }
    }
}
