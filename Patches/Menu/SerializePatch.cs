/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using iiMenu.Managers;
using Photon.Pun;
﻿using System;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(PhotonNetwork), nameof(PhotonNetwork.RunViewUpdate))]
    public class SerializePatch
    {
        /// <summary>
        /// Occurs when a serialization process is initiated.
        /// </summary>
        public static event Action OnSerialize;

        /// <summary>
        /// Delegate that determines whether serialization should be overridden.
        /// </summary>
        public static Func<bool> OverrideSerialization;

        public static bool Prefix()
        {
            if (!PhotonNetwork.InRoom)
                return true;

            try
            {
                OnSerialize?.Invoke();
            } catch (Exception e)
            {
                LogManager.LogError($"Error in SerializePatch.OnSerialize: {e}");
            }

            if (OverrideSerialization == null)
                return true;

            try
            {
                return OverrideSerialization();
            } catch (Exception e)
            {
                LogManager.LogError($"Error in SerializePatch.OverrideSerialization: {e}");
                return false;
            }
        }
    }
}
