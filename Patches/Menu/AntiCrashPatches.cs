/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using ExitGames.Client.Photon;
using GorillaExtensions;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
﻿using System.Collections.Generic;
using UnityEngine;

namespace iiMenu.Patches.Menu
{
    public class AntiCrashPatches
    {
        public static bool enabled;

        [HarmonyPatch(typeof(VRRig), nameof(VRRig.DroppedByPlayer))]
        public class DroppedByPlayer
        {
            public static bool enabled;

            public static bool Prefix(VRRig __instance, VRRig grabbedByRig, Vector3 throwVelocity)
            {
                return !enabled || !__instance.isLocal || throwVelocity.IsValid();
            }
        }

        [HarmonyPatch(typeof(VRRig), nameof(VRRig.RequestCosmetics))]
        public class RequestCosmetics
        {
            private static readonly List<float> callTimestamps = new List<float>();
            public static bool Prefix(VRRig __instance)
            {
                if (enabled && __instance.isLocal)
                {
                    callTimestamps.Add(Time.time);
                    callTimestamps.RemoveAll(t => Time.time - t > 1);

                    return callTimestamps.Count < 15;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(VRRig), nameof(VRRig.RequestMaterialColor))]
        public class RequestMaterialColor
        {
            private static readonly List<float> callTimestamps = new List<float>();
            public static bool Prefix(VRRig __instance)
            {
                if (enabled && __instance.isLocal)
                {
                    callTimestamps.Add(Time.time);
                    callTimestamps.RemoveAll(t => Time.time - t > 1);

                    return callTimestamps.Count < 15;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(DeployedChild), nameof(DeployedChild.Deploy))]
        public class Deploy
        {
            public static void Postfix(DeployedChild __instance, DeployableObject parent, Vector3 launchPos, Quaternion launchRot, Vector3 releaseVel, bool isRemote = false)
            {
                if (enabled)
                    __instance._rigidbody.linearVelocity = __instance._rigidbody.linearVelocity.ClampMagnitudeSafe(100f);
            }
        }

        [HarmonyPatch(typeof(LuauVm), nameof(LuauVm.OnEvent))]
        public class OnEvent
        {
            public static bool Prefix(EventData eventData)
            {
                if (enabled)
                {
                    if (eventData.Code != 180) return false;

                    Player sender = PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(eventData.Sender);

                    object[] args = eventData.CustomData == null ? new object[] { } : (object[])eventData.CustomData;
                    string command = args.Length > 0 ? (string)args[0] : "";

                    if (sender != PhotonNetwork.LocalPlayer && args[1] is double v && v == PhotonNetwork.LocalPlayer.ActorNumber && command == "leaveGame")
                        return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(RoomSystem), nameof(RoomSystem.SearchForShuttle))]
        public class SearchForShuttle
        {
            public static bool Prefix(object[] shuffleData, PhotonMessageInfoWrapped info) =>
                !enabled;
        }

        [HarmonyPatch(typeof(RoomInfo), nameof(RoomInfo.InternalCacheProperties))]
        public class InternalCacheProperties
        {
            public static bool Prefix(RoomInfo __instance, Hashtable propertiesToCache)
            {
                return __instance.masterClientId != PhotonNetwork.LocalPlayer.ActorNumber || propertiesToCache.Count != 1 || !propertiesToCache.ContainsKey(248) || !enabled;
            }
        }

        [HarmonyPatch(typeof(GameEntityManager), nameof(GameEntityManager.JoinWithItemsRPC))]
        public class JoinWithItemsRPC
        {
            public static bool Prefix(GameEntityManager __instance, byte[] stateData, int[] netIds, int joiningActorNum, PhotonMessageInfo info)
            {
                return stateData.Length <= 255;
            }
        }

        [HarmonyPatch(typeof(GorillaWrappedSerializer), nameof(GorillaWrappedSerializer.FailedToSpawn))]
        public class FailedToSpawn
        {
            public static bool Prefix(GorillaWrappedSerializer __instance)
            {
                if (enabled)
                {
                    __instance.gameObject.SetActive(false);
                    return false;
                }
                return true;
            }
        }


    }
}
