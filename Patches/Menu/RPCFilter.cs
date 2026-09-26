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
using Photon.Realtime;
using System;
using System.Collections.Generic;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(PhotonNetwork), nameof(PhotonNetwork.RPC), typeof(PhotonView), typeof(string), typeof(RpcTarget), typeof(Player), typeof(bool), typeof(object[]))]
    public class RPCFilter
    {
        /// <summary>
        /// Stores a mapping of RPC names to filter functions that determine whether each RPC should be sent.
        /// </summary>
        public static Dictionary<string, Func<bool>> FilteredRPCs = new Dictionary<string, Func<bool>>();

        public static bool Prefix(PhotonView view, string methodName, RpcTarget target, Player player, bool encrypt, params object[] parameters)
        {
            if (FilteredRPCs.Count <= 0)
                return true;

            try
            {
                if (FilteredRPCs.TryGetValue(methodName, out var function))
                    return function?.Invoke() ?? true;
            } catch (Exception e)
            {
                LogManager.LogError($"Error in RPCFilter.FilteredRPCs.{methodName}: {e}");
            }

            return true;
        }
    }
}
