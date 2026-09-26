/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

﻿using HarmonyLib;
using iiMenu.Menu;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(FXSystem), nameof(FXSystem.PlayFXForRig), typeof(FXType), typeof(IFXContext), typeof(PhotonMessageInfoWrapped))]
    public class FXPatch
    {
        public static bool Prefix(FXType fxType, IFXContext context, PhotonMessageInfoWrapped info = default(PhotonMessageInfoWrapped))
        {
            NetPlayer player = info.Sender;
            if (player != null && Main.ShouldBypassChecks(player))
            {
                context.OnPlayFX();
                return false;
            }

            return true;
        }
    }
}
