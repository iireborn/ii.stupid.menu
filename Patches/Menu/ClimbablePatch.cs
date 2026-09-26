/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using GorillaLocomotion.Climbing;
using HarmonyLib;
using UnityEngine;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(GorillaHandClimber), nameof(GorillaHandClimber.GetClosestClimbable))]
    public class ClimbablePatch
    {
        public static bool enabled;
        private static void Postfix(GorillaHandClimber __instance, ref GorillaClimbable __result)
        {
            if (enabled && __result == null)
            {
                switch (__instance.potentialClimbables.Count)
                {
                    case 0:
                    case 1:
                        return;
                }

                Vector3 position = __instance.transform.position;
                Bounds bounds = __instance.col.bounds;

                float closestDistance = (__instance.col as SphereCollider).radius + 0.05f;

                GorillaClimbable gorillaClimbable = null;
                foreach (GorillaClimbable potentialClimbable in __instance.potentialClimbables)
                {
                    float distance;
                    if (potentialClimbable.colliderCache)
                    {
                        if (!bounds.Intersects(potentialClimbable.colliderCache.bounds))
                            continue;
                        
                        Vector3 vector = potentialClimbable.colliderCache.ClosestPoint(position);
                        distance = Vector3.Distance(position, vector);
                    }
                    else
                        distance = Vector3.Distance(position, potentialClimbable.transform.position);

                    if (distance < closestDistance)
                    {
                        gorillaClimbable = potentialClimbable;
                        closestDistance = distance;
                    }
                }

                __result = gorillaClimbable;
            }
        }
    }
}
