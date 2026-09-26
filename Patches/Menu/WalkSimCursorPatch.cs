/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using iiMenu.Managers;
using iiMenu.Menu;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

namespace iiMenu.Patches.Menu
{
    public static class WalkSimCursorPatch
    {
        private static readonly string[] HeadDriverTypeNames =
        {
            "WalkSimulator.Rigging.HeadDriver",
            "WalkSim.WalkSim.Rigging.HeadDriver"
        };

        private static bool installed;
        private static int installAttempts;
        private const int MaxInstallAttempts = 5;
        private const float RetryInterval = 5f;
        private static float nextRetryTime;

        public static void Install() => EnsureInstalled(force: true);

        public static void EnsureInstalled(bool force = false)
        {
            if (installed)
                return;

            if (!force)
            {
                if (installAttempts >= MaxInstallAttempts || Time.time < nextRetryTime)
                    return;
            }

            installAttempts++;
            nextRetryTime = Time.time + RetryInterval;

            try
            {
                Type headDriver = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(TryGetTypes)
                    .FirstOrDefault(type => HeadDriverTypeNames.Contains(type.FullName));

                if (headDriver == null)
                    return;

                PatchHandler.ApplyPatch(headDriver, "set_LockCursor",
                    prefix: AccessTools.Method(typeof(WalkSimCursorPatch), nameof(LockCursorPrefix)));

                installed = true;
                LogManager.Log("WalkSimulator cursor lock neutralized while the desktop menu is open");
            }
            catch (Exception ex)
            {
                LogManager.LogError($"Failed to patch WalkSimulator cursor lock: {ex.Message}");
            }
        }

        private static Type[] TryGetTypes(System.Reflection.Assembly assembly)
        {
            try { return assembly.GetTypes(); }
            catch { return Array.Empty<Type>(); }
        }

        private static void LockCursorPrefix(ref bool value)
        {
            if (value && Main.MenuWantsCursor)
                value = false;
        }

        public static void Uninstall()
        {
            if (!installed) return;
            try
            {
                Type headDriver = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(TryGetTypes)
                    .FirstOrDefault(type => HeadDriverTypeNames.Contains(type.FullName));
                if (headDriver != null)
                    PatchHandler.RemovePatch(headDriver, "set_LockCursor");
            } catch { }
            installed = false;
        }
    }
}
