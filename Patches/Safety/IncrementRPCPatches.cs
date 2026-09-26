/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace iiMenu.Patches.Safety
{
    public class IncrementRPCPatches
    {
        [HarmonyPatch(typeof(VRRig), nameof(VRRig.IncrementRPC), typeof(PhotonMessageInfoWrapped), typeof(string))]
        public class NoIncrementRPC
        {
            private static bool Prefix(PhotonMessageInfoWrapped info, string sourceCall) =>
                false;
        }

        [HarmonyPatch(typeof(MonkeAgent), nameof(MonkeAgent.IncrementRPCCall), typeof(PhotonMessageInfo), typeof(string))]
        public class NoIncrementRPCCall
        {
            private static bool Prefix(PhotonMessageInfo info, string callingMethod = "") =>
                false;
        }

        [HarmonyPatch(typeof(MonkeAgent), nameof(MonkeAgent.IncrementRPCCallLocal))]
        public class NoIncrementRPCCallLocal
        {
            private static bool Prefix(PhotonMessageInfoWrapped infoWrapped, string rpcFunction) =>
                false;
        }

        [HarmonyPatch]
        public class NoIncrementRPCTracker
        {
            private static IEnumerable<MethodBase> TargetMethods()
            {
                var trackerMethods = typeof(MonkeAgent).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                    .Where(m => m.Name == nameof(MonkeAgent.IncrementRPCTracker) && m.GetParameters().Length == 3);

                foreach (var method in trackerMethods)
                    yield return method;
            }

            private static bool Prefix(ref bool __result)
            {
                __result = true;
                return false;
            }
        }
    }
}
