/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using GorillaLocomotion;
using iiMenu.Managers;
using iiMenu.Mods;
using System;
using System.Linq;
using UnityEngine;

namespace iiMenu.Classes.Mods
{
    public class PortalTrigger : MonoBehaviour
    {
        private static readonly Type[] allowedTypes = { typeof(ThrowableBug), typeof(SlingshotProjectile) };

        static bool HasAllowedComponent(Collider col) =>
            allowedTypes.Any(t => col.GetComponent(t) != null);

        public GameObject destination;
        public void OnTriggerEnter(Collider other)
        {
            if (other == GTPlayer.Instance.bodyCollider || other == GTPlayer.Instance.headCollider)
                CoroutineManager.instance.StartCoroutine(Movement.TeleportPortal(destination));
            else if (HasAllowedComponent(other))
                CoroutineManager.instance.StartCoroutine(Movement.TeleportObject(other.gameObject, destination));
        }
    }
}
