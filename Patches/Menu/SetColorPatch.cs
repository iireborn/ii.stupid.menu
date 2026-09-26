/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using iiMenu.Menu;
using iiMenu.Utilities;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(VRRig), nameof(VRRig.InitializeNoobMaterial))]
    public class InitializeNoobMaterial
    {
        public static bool Prefix(VRRig __instance, float red, float green, float blue, PhotonMessageInfoWrapped info)
        {
            NetPlayer player = RigUtilities.GetPlayerFromVRRig(__instance) ?? null;
            if (player != null && Main.ShouldBypassChecks(player))
            {
                if (info.senderID == NetworkSystem.Instance.GetOwningPlayerID(__instance.rigSerializer.gameObject))
                    __instance.InitializeNoobMaterialLocal(red, green, blue);

                return false;
            }

            return true;
        }
    }
}
