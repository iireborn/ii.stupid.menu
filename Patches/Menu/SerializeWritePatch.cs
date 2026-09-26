/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using iiMenu.Extensions;
using Photon.Pun;
using UnityEngine;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(PhotonNetwork), nameof(PhotonNetwork.OnSerializeWrite))]
    public static class SerializeWritePatch
    {
        public static Vector3? positionOverride;

        public static void Prefix(PhotonView view, out Vector3? __state)
        {
            __state = null;

            if (positionOverride == null || VRRig.LocalRig == null)
                return;

            if (GorillaTagger.Instance == null || GorillaTagger.Instance.myVRRig == null || view != GorillaTagger.Instance.myVRRig.GetView)
                return;

            __state = VRRig.LocalRig.transform.position;
            VRRig.LocalRig.transform.position = positionOverride.Value;
        }

        public static void Finalizer(ref Vector3? __state)
        {
            if (__state == null || VRRig.LocalRig == null)
                return;

            VRRig.LocalRig.transform.position = __state.Value;
        }
    }
}
