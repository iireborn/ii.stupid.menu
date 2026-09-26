/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using GorillaTag.Cosmetics;
using HarmonyLib;
using Photon.Pun;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(DreidelHoldable), nameof(DreidelHoldable.OnActivate))]
    public class DreidelPatch
    {
        public static bool enabled;
        public static double? time;

        public static void Prefix()
        {
            if (enabled)
            {
                time = PhotonNetwork.frametime;
                PhotonNetwork.frametime = double.MaxValue;
            }
        }

        public static void Postfix()
        {
            if (enabled)
                PhotonNetwork.frametime = time ?? PhotonNetwork.ServerTimestamp / 1000.0;
            
            time = null;
        }
    }
}
