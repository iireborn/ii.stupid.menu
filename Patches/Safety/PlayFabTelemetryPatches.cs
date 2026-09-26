/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Internal;
using System;
using System.Collections.Generic;
using static iiMenu.Utilities.RandomUtilities;
using Random = UnityEngine.Random;
using static iiMenu.Patches.PatchHandler;

namespace iiMenu.Patches.Safety
{
    public class PlayFabTelemetryPatches
    {
        [PatchOnAwake]
        [HarmonyPatch(typeof(PlayFabDeviceUtil), nameof(PlayFabDeviceUtil.SendDeviceInfoToPlayFab))]
        public class PlayfabUtil01
        {
            private static bool Prefix() =>
                false;
        }

        [PatchOnAwake]
        [HarmonyPatch(typeof(PlayFabClientInstanceAPI), nameof(PlayFabClientInstanceAPI.ReportDeviceInfo))]
        public class PlayfabUtil02
        {
            private static bool Prefix() =>
                false;
        }

        [PatchOnAwake]
        [HarmonyPatch(typeof(PlayFabClientAPI), nameof(PlayFabClientAPI.ReportDeviceInfo))]
        public class PlayfabUtil03
        {
            private static bool Prefix() =>
                false;
        }

        [HarmonyPatch(typeof(PlayFabDeviceUtil), nameof(PlayFabDeviceUtil.GetAdvertIdFromUnity))]
        public class PlayfabUtil04
        {
            private static bool Prefix() =>
                false;
        }

        [PatchOnAwake]
        [HarmonyPatch(typeof(PlayFabClientAPI), nameof(PlayFabClientAPI.AttributeInstall))]
        public class PlayfabUtil05
        {
            private static bool Prefix() =>
                false;
        }

        [PatchOnAwake]
        [HarmonyPatch(typeof(PlayFabHttp), nameof(PlayFabHttp.InitializeScreenTimeTracker))]
        public class PlayfabUtil06
        {
            private static bool Prefix() =>
                false;
        }

        [PatchOnAwake]
        [HarmonyPatch(typeof(PlayFabClientAPI), nameof(PlayFabClientAPI.UpdateUserTitleDisplayName))] // Credits to Shiny for letting me use this
        public class DisplayNamePatch
        {
            public static void Prefix(ref UpdateUserTitleDisplayNameRequest request, Action<UpdateUserTitleDisplayNameResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null) =>
                request.DisplayName = RandomString(Random.Range(3, 12)); // Min and max is 3 and 12, do not modify this
        }
    }
}
