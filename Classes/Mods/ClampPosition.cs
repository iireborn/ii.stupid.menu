/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

﻿using UnityEngine;

namespace iiMenu.Classes.Mods
{
    public class ClampPosition : MonoBehaviour
    {
        public void Start() =>
            Update();

        public void Update()
        {
            if (targetTransform == null || targetTransform.gameObject == null)
                Destroy(this);

            transform.position = targetTransform.position;
            transform.rotation = targetTransform.rotation;
        }

        public Transform targetTransform;
    }
}
