/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using iiMenu.Menu;
using UnityEngine;

namespace iiMenu.Classes.Menu
{
    public class ScrollMaterial : MonoBehaviour
    {
        private Renderer renderer;
        private Material mat;

        void Awake()
        {
            renderer = GetComponent<Renderer>();
            mat = renderer.material;

            Update();
        }

        void Update()
        {
            float offset = Main.slowFadeColors ? Time.time / 10f : Time.time;
            Vector4 st = new Vector4(1, 1, offset, offset);
            mat.SetVector("_BaseMap_ST", st);
        }
    }
}