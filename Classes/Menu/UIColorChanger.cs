/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using UnityEngine;
using UnityEngine.UI;

namespace iiMenu.Classes.Menu
{
    public class UIColorChanger : MonoBehaviour
    {
        public void Start()
        {
            if (colors == null)
            {
                Destroy(this);
                return;
            }

            targetGraphic = gameObject.GetComponent<MaskableGraphic>();

            if (colors.IsFlat())
            {
                Update();
                Destroy(this);
                return;
            }

            Update();
        }

        public void Update() =>
            targetGraphic.color = colors.GetCurrentColor();

        public MaskableGraphic targetGraphic;
        public ExtGradient colors;
    }
}
