/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using iiMenu.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static iiMenu.Utilities.GameModeUtilities;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(SIGadgetChargeBlaster), nameof(SIGadgetChargeBlaster.FireProjectile))]
    public class FirePatch
    {
        public static bool enabled;

        public static void Prefix(SIGadgetChargeBlaster __instance, float firedAtChargeLevel, int fireId, Vector3 position, Quaternion rotation)
        {
            if (enabled && __instance.blaster.LocalEquippedOrActivated)
            {
                List<NetPlayer> infected = InfectedList();
                List<VRRig> rigs = VRRigCache.ActiveRigs
                    .Where(rig => !rig.isLocal)
                    .Where(rig => !infected.Contains(rig.GetPlayer()))
                    .ToList();

                Transform head = GorillaTagger.Instance.headCollider.transform;
                VRRig targetRig = rigs
                    .Where(rig => rig != null)
                    .Select(rig => new {
                        Rig = rig,
                        ToRig = (rig.transform.position - head.position).normalized,
                        Distance = Vector3.Distance(head.position, rig.transform.position)
                    })
                    .OrderBy(x => Vector3.Angle(head.forward, x.ToRig) + x.Distance * 0.1f)
                    .Select(x => x.Rig)
                    .FirstOrDefault();

                rotation = Quaternion.LookRotation((targetRig.headMesh.transform.position - position).normalized);
            }
        }
    }
}
