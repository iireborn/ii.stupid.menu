/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using iiMenu.Managers;
using Photon.Voice;
using Photon.Voice.Unity;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(Recorder))]
    public class RecorderPatch
    {
        public static bool enabled = true;
        [HarmonyPatch(nameof(Recorder.SourceType), MethodType.Getter)]
        public static bool Prefix(ref Recorder.InputSourceType __result)
        {
            if (enabled)
            {
                __result = Recorder.InputSourceType.Factory;
                return false;
            }
            return true;
            
        }

        [HarmonyPatch(nameof(Recorder.InputFactory), MethodType.Getter)]
        public static bool Prefix(ref System.Func<IAudioDesc> __result)
        {
            if (enabled)
            {
                __result = () => VoiceManager.Get();
                return false;
            }
            return true;
        }

        [HarmonyPatch(nameof(Recorder.CreateLocalVoiceAudioAndSource))]
        public static bool Prefix(Recorder __instance)
        {
            if (enabled)
            {
                __instance.SourceType = Recorder.InputSourceType.Factory;
                __instance.InputFactory = () => VoiceManager.Get();
            }
            return true;
        }
    }
}
