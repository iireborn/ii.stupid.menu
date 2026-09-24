/*
 * ii's Stupid Menu  Patches/Menu/SerializeWritePatch.cs
 * A mod menu for Gorilla Tag with over 1000+ mods
 *
 * Copyright (C) 2026  Goldentrophy Software
 * https://github.com/iireborn/iis.Stupid.Menu
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
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
