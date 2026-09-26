/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using ExitGames.Client.Photon;
using HarmonyLib;
using Photon.Realtime;
﻿using System.Linq;

namespace iiMenu.Patches.Menu
{
    public class PropertiesPatches
    {
        public static bool enabled;

        [HarmonyPatch(typeof(Player), nameof(Player.SetCustomProperties))]
        public class SetCustomPropertiesMethod
        {
            public static bool Prefix(Player __instance, ref Hashtable propertiesToSet)
            {
                if (__instance.IsLocal && enabled)
                {
                    if (propertiesToSet.Any(prop => prop.Key.ToString() != "didTutorial"))
                        return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.CustomProperties), MethodType.Setter)]
        public class SetCustomPropertiesField
        {
            public static bool Prefix(Player __instance, ref Hashtable value)
            {
                if (__instance.IsLocal && enabled)
                {
                    if (value.Any(prop => prop.Key.ToString() != "didTutorial"))
                        return false;
                }

                return true;
            }
        }
    }
}