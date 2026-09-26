/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using iiMenu.Mods;
﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(VRRig), nameof(VRRig.PostTick))]
    public class TorsoPatch
    {
        public static event Action VRRigLateUpdate;
        public static bool enabled;
        public static int mode = 0;

        public static void Postfix(VRRig __instance)
        {
            if (__instance.isLocal)
            {
                if (enabled)
                {
                    Quaternion rotation = Quaternion.identity;
                    switch (mode)
                    {
                        case 0:
                            rotation = Quaternion.Euler(0f, Time.time * 180f % 360, 0f);
                            break;
                        case 1:
                            rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                            break;
                        case 2:
                            rotation = Quaternion.Euler(0f, GorillaTagger.Instance.headCollider.transform.rotation.eulerAngles.y + 180f, 0f);
                            break;
                        case 3:
                            rotation = Quaternion.Euler(0f, Movement.recBodyRotary.transform.rotation.eulerAngles.y, 0f);
                            break;
                    }

                    __instance.transform.rotation = rotation;
                    __instance.head.MapMine(__instance.scaleFactor, __instance.playerOffsetTransform);
                    __instance.leftHand.MapMine(__instance.scaleFactor, __instance.playerOffsetTransform);
                    __instance.rightHand.MapMine(__instance.scaleFactor, __instance.playerOffsetTransform);
                }

                VRRigLateUpdate?.Invoke();
            }
        }
    }
}
