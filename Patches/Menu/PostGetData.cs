/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

﻿using GorillaNetworking;
using GorillaNetworking.Store;
using HarmonyLib;
using iiMenu.Managers;
using static iiMenu.Menu.Main;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(BundleManager), nameof(BundleManager.CheckIfBundlesOwned))]
    public class PostGetData
    {
        public static bool CosmeticsInitialized;
        private static void Postfix()
        {
            CosmeticsController cosmetics = CosmeticsController.instance;

            if (cosmetics == null)
            {
                LogManager.Log("Bundle check ran before the cosmetics were initialized");
                return;
            }

            CosmeticsInitialized = true;
            CosmeticsOwned = cosmetics.concatStringCosmeticsAllowed;
        }
    }
}
