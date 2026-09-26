/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using UnityEngine;

namespace iiMenu.Extensions
{
    public static class SlingshotExtensions
    {
        public static Vector3 GetTrueLaunchPosition(this Slingshot slingshot) =>
            slingshot.drawingHand.transform.position + 
            (slingshot.centerOrigin.position - slingshot.drawingHand.transform.position).normalized * 
            (EquipmentInteractor.instance.grabRadius - slingshot.dummyProjectileColliderRadius) * 
            (slingshot.dummyProjectileInitialScale * Mathf.Abs(slingshot.transform.lossyScale.x));

        public static Vector3 GetNetworkedLaunchVelocity(this Slingshot slingshot)
        {
            float projectileScale = Mathf.Abs(slingshot.transform.lossyScale.x);

            Vector3 baseDirection = slingshot.centerOrigin.position - slingshot.center.position;
            baseDirection /= projectileScale;

            Vector3 fixedDirection = Mathf.Min(slingshot.springConstant * slingshot.maxDraw, baseDirection.magnitude * slingshot.springConstant) * baseDirection.normalized * projectileScale;
            Vector3 averagedVelocity = slingshot.myRig.LatestVelocity();

            return fixedDirection + averagedVelocity;
        }
    }
}
