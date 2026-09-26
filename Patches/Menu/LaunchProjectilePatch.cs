/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using iiMenu.Extensions;
using static iiMenu.Menu.Main;
using static iiMenu.Utilities.AssetUtilities;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(ProjectileWeapon), nameof(ProjectileWeapon.LaunchProjectile))]
    public class LaunchProjectilePatch
    {
        public static bool enabled;

        public static void Prefix(ProjectileWeapon __instance)
        {
            if (enabled)
            {
                GorillaTagger.Instance.rigidbody.linearVelocity = __instance.GetLaunchVelocity();

                if (dynamicSounds)
                    LoadSoundFromURL($"{PluginInfo.ServerResourcePath}/Audio/Mods/Fun/AngryBirds/launch.ogg", "Audio/Mods/Fun/AngryBirds/launch.ogg").Play(buttonClickVolume / 10f);
            }
        }
    }
}
