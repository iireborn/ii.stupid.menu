/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using GorillaTagScripts;
using HarmonyLib;
using System;

namespace iiMenu.Patches.Menu
{
    public class SubscriptionPatches
    {
        public static bool enabled;

        [HarmonyPatch(typeof(SubscriptionManager), nameof(SubscriptionManager.IsLocalSubscribed))]
        public class IsLocalSubscribed
        {
            private static bool Prefix(ref bool __result)
            {
                if (!enabled)
                    return true;

                __result = true;
                return false;
            }
        }

        [HarmonyPatch(typeof(SubscriptionManager), nameof(SubscriptionManager.LocalSubscriptionStatus))]
        public class LocalSubscriptionStatus
        {
            private static bool Prefix(ref SubscriptionManager.SubscriptionStatus __result)
            {
                if (!enabled)
                    return true;

                __result = SubscriptionManager.SubscriptionStatus.Active;
                return false;
            }
        }

        [HarmonyPatch(typeof(SubscriptionManager), nameof(SubscriptionManager.LocalSubscriptionDetails))]
        public class LocalSubscriptionDetails
        {
            private static bool Prefix(ref SubscriptionManager.SubscriptionDetails __result)
            {
                if (!enabled)
                    return true;

                __result = new SubscriptionManager.SubscriptionDetails
                {
                    active = true,
                    daysAccrued = int.MaxValue,
                    subscriptionFeatureSettings = new[] {true, true},
                    tier = int.MaxValue,
                    subscriptionActiveUntilDate = DateTime.MaxValue,
                    autoRenew = true,
                    autoRenewMonths = int.MaxValue
                };
                return false;
            }
        }
    }
}
