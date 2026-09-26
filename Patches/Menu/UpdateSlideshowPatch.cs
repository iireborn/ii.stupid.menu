/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using iiMenu.Classes.Mods;
using TMPro;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(NewMapsDisplay), nameof(NewMapsDisplay.UpdateSlideshow))]
    public static class UpdateSlideshowPatch
    {
        public static bool Prefix(NewMapsDisplay __instance)
        {
            if (VirtualStumpAd.Instance == null)
                return true;

            __instance.mapImage = VirtualStumpAd.SpriteRenderer;
            __instance.mapInfoTMP = VirtualStumpAd.MapInfoText.GetComponent<TextMeshPro>();
            return __instance.mapImage != null && __instance.mapImage.gameObject != null;
        }
    }
}
