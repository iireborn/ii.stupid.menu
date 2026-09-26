/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

﻿using GorillaLocomotion;
using HarmonyLib;
using iiMenu.Extensions;
using UnityEngine;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(TakeMyHand_HandLink), nameof(TakeMyHand_HandLink.OnRelease))]
    public class ReleasePatch
    {
        public static bool enabled;

        public static bool Prefix(TakeMyHand_HandLink __instance, bool __result, DropZone zoneReleased, GameObject releasingHand)
        {
            if (enabled)
            {
                if (!__instance.myRig.isOfflineVRRig)
                {
                    bool grounded = false;

                    TakeMyHand_HandLink handLink = releasingHand == EquipmentInteractor.instance.leftHand ? VRRig.LocalRig.leftHandLink : VRRig.LocalRig.rightHandLink;
                   
                    HandLinkAuthorityStatus selfHandLinkAuthority = GTPlayer.Instance.TakeMyHand_GetSelfHandLinkAuthority();
                    HandLinkAuthorityStatus selfChainAuthority = handLink.GetChainAuthority(out _);

                    if (selfHandLinkAuthority.type >= HandLinkAuthorityType.ButtGrounded && selfChainAuthority.type < selfHandLinkAuthority.type)
                        grounded = true;
                    else if (handLink.myOtherHandLink.grabbedLink != null)
                    {
                        HandLinkAuthorityStatus otherChainAuthority = handLink.myOtherHandLink.GetChainAuthority(out _);
                        if (otherChainAuthority.type >= HandLinkAuthorityType.ButtGrounded && selfChainAuthority.type < otherChainAuthority.type)
                            grounded = true;
                    }

                    if (grounded)
                    {
                        Vector3 averageVelocity = (handLink.isLeftHand ? GTPlayer.Instance.LeftHand.velocityTracker : GTPlayer.Instance.RightHand.velocityTracker).GetAverageVelocity(true).normalized * 20f;
                        __instance.myRig.netView.SendRPC("DroppedByPlayer", __instance.myRig.GetPlayer(), averageVelocity);
                        __instance.myRig.ApplyLocalTrajectoryOverride(averageVelocity);
                    }

                    handLink.BreakLink();
                }
                return false;
            }
            
            return true;
        }
    }
}
