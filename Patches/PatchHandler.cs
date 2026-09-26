/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using iiMenu.Managers;
using iiMenu.Patches.Safety;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace iiMenu.Patches
{
    public class PatchHandler
    {
        public static bool IsPatched { get; internal set; }
        public static int PatchErrors { get; internal set; }

        public static bool CriticalPatchFailed { get; internal set; }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class SecurityPatch : Attribute { }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class PatchOnAwake : Attribute { }

        public static void PatchAll(bool awake = false)
        {
            if (IsPatched) return;
            instance ??= new Harmony(PluginInfo.GUID);

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes()
                         .Where(t => t.IsClass && t.GetCustomAttribute<HarmonyPatch>() != null && t.GetCustomAttribute<PatchOnAwake>() != null == awake))
            {
                try
                {
                    instance.CreateClassProcessor(type).Patch();
                }
                catch (Exception ex)
                {
                    PatchErrors++;
                    if (type.GetCustomAttribute<SecurityPatch>() != null)
                        CriticalPatchFailed = true;
                    LogManager.LogError($"Failed to patch {type.FullName}: {ex}");
                }
            }

            LogManager.Log($"Patched with {PatchErrors} errors");

            IsPatched = !awake;
        }

        public static void UnpatchAll()
        {
            if (instance == null || !IsPatched) return;
            instance.UnpatchSelf();
            IsPatched = false;
            instance = null;
        }

        public static void ApplyPatch(Type targetClass, string methodName, MethodInfo prefix = null, MethodInfo postfix = null, Type[] parameterTypes = null)
        {
            var original =
                (parameterTypes == null ?
                targetClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static) :
                targetClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null, parameterTypes, null)) ?? throw new Exception($"Method '{methodName}' not found on {targetClass.FullName}");
            instance.Patch(original,
                prefix: prefix != null ? new HarmonyMethod(prefix) : null,
                postfix: postfix != null ? new HarmonyMethod(postfix) : null);
        }

        public static void RemovePatch(Type targetClass, string methodName, Type[] parameterTypes = null)
        {
            var original =
                (parameterTypes == null ?
                targetClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static) :
                targetClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null, parameterTypes, null)) ?? throw new Exception($"Method '{methodName}' not found on {targetClass.FullName}");
            instance.Unpatch(original, HarmonyPatchType.All, instance.Id);
        }

        private static Harmony instance;
        public const string InstanceId = PluginInfo.GUID;
    }
}