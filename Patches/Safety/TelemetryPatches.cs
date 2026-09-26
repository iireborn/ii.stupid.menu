/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using JetBrains.Annotations;
using Liv.Lck.Telemetry;
using PlayFab;
using PlayFab.EventsModels;
using System.Collections.Generic;
using static iiMenu.Patches.PatchHandler;

namespace iiMenu.Patches.Safety
{
    // Gorilla Tag's one weakness -- tracking data to get players banned. This is how they did it over the years.
    public class TelemetryPatches
    {
        public static bool enabled = true;

        [PatchOnAwake]
        [HarmonyPatch(typeof(GorillaTelemetry), nameof(GorillaTelemetry.EnqueueTelemetryEvent))]
        public class EnqueueTelemetryEvent
        {
            private static bool Prefix(string eventName, object content, [CanBeNull] string[] customTags = null) =>
                !enabled;
        }

        [PatchOnAwake]
        [HarmonyPatch(typeof(GorillaTelemetry), nameof(GorillaTelemetry.FlushMothershipTelemetry))]
        public class FlushMothershipTelemetry
        {
            private static bool Prefix() =>
                !enabled;
        }

        [PatchOnAwake]
        [HarmonyPatch(typeof(LckTelemetryClient), nameof(LckTelemetryClient.SendTelemetry))]
        public class SendTelemetry
        {
            private static bool Prefix(LckTelemetryEvent lckTelemetryEvent) =>
                !enabled;
        }

        [PatchOnAwake]
        [HarmonyPatch(typeof(PlayFabEventsAPI), nameof(PlayFabEventsAPI.WriteTelemetryEvents))]
        public class WriteTelemetryEvents
        {
            private static bool Prefix(WriteEventsRequest request, System.Action<WriteEventsResponse> resultCallback, System.Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null) =>
                !enabled;
        }
    }
}
