/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
﻿using System.Threading.Tasks;

namespace iiMenu.Patches.Menu
{
    public class TOSPatches
    {
        public static bool enabled;

        [HarmonyPatch(typeof(LegalAgreements), nameof(LegalAgreements.Update))]
        public class Update
        {
            private static bool Prefix(LegalAgreements __instance)
            {
                if (enabled)
                {
                    ControllerInputPoller.instance.leftControllerPrimary2DAxis.y = -1f;
                    __instance.scrollSpeed = 10f;
                    __instance._maxScrollSpeed = 10f;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(ModIOTermsOfUse_v1), nameof(ModIOTermsOfUse_v1.PostUpdate))]
        public class PostUpdateModIO
        {
            private static bool Prefix(ModIOTermsOfUse_v1 __instance)
            {
                if (enabled)
                {
                    __instance.TurnPage(999);
                    ControllerInputPoller.instance.leftControllerPrimary2DAxis.y = -1f;
                    __instance.holdTime = 0.1f;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(AgeSlider), nameof(AgeSlider.PostUpdate))]
        public class PostUpdateAgeSlider
        {
            private static bool Prefix(AgeSlider __instance)
            {
                if (enabled)
                {
                    __instance._currentAge = 21;
                    __instance.holdTime = 0.1f;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PrivateUIRoom), nameof(PrivateUIRoom.StartOverlay))]
        public class StartOverlay
        {
            private static bool Prefix() =>
                !enabled;
        }

        [HarmonyPatch(typeof(KIDManager), nameof(KIDManager.UseKID))]
        public class UseKID
        {
            private static bool Prefix(ref Task<bool> __result)
            {
                if (!enabled)
                    return true;

                __result = Task.FromResult(false);
                return false;
            }
        }
    }
}
