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
    public static class QuaternionExtensions
    {
        public static Quaternion Lerp(this Quaternion a, Quaternion b, float t) =>
            Quaternion.Lerp(a, b, t);
    }
}
