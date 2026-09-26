/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using iiMenu.Managers;
using System;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(VRRig), nameof(VRRig.SerializeReadShared))]
    public class PlayerSerializePatch
    {
        public static bool stopSerialization;
        public static float? delay;

        public static event Action<VRRig> OnPlayerSerialize;
        public static bool Prefix(VRRig __instance, InputStruct data)
        {
            if (stopSerialization)
                return false;

            if (delay != null)
            {
                CoroutineManager.instance.StartCoroutine(
                    iiMenu.Menu.Main.SerializationDelay(() =>
                    {
                        float oldDelay = delay.Value;
                        delay = null;
                        try
                        {
                            __instance.SerializeReadShared(data);
                        } catch { }
                        delay = oldDelay;
                    }, delay.Value)
                );

                return false;
            }

            return true;
        }

        public static void Postfix(VRRig __instance, InputStruct data) =>
            OnPlayerSerialize?.Invoke(__instance);
    }
}
