/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

﻿using UnityEngine;

namespace iiMenu.Classes.Mods
{
    public class DestroyOnRest : MonoBehaviour
    {
        public void Start()
        {
            rigidbody = gameObject.GetComponent<Rigidbody>();
            Update();
        }

        public void Update()
        {
            if (rigidbody.linearVelocity.magnitude < 0.01f)
                Destroy(gameObject);
        }

        public Rigidbody rigidbody;
    }
}
