/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
﻿using System.Collections.Generic;
using UnityEngine;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(GameEntityManager), nameof(GameEntityManager.TryGrabLocal))]
    public class EntityGrabPatch
    {
        public static bool enabled;

        public static bool Prefix(GameEntityManager __instance, Vector3 handPosition, bool isLeftHand, Vector3 closestPointOnBoundingBox, ref GameEntityId __result)
        {
            if (enabled)
            {
                List<GameEntity> entities = __instance.entities;

                GameEntityId gameEntityId = GameEntityId.Invalid;
                float closestDist = float.MaxValue;
                for (int i = 0; i < entities.Count; i++)
                {
                    GameEntity entity = entities[i];
                    if (entity != null && __instance.ValidateGrab(entity, NetworkSystem.Instance.LocalPlayer.ActorNumber, isLeftHand))
                    {
                        double reach = 16;

                        float distance = (handPosition - entity.transform.position).sqrMagnitude;
                        if (distance < reach && distance < closestDist)
                        {
                            gameEntityId = entity.id;
                            closestDist = distance;
                        }
                    }
                }

                __result = gameEntityId;

                return false;
            }
            
            return true;
        }
    }
}
