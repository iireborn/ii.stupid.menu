/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using System;
using UnityEngine;
using static iiMenu.Utilities.RandomUtilities;

namespace iiMenu.Extensions
{
    public static class VectorExtensions
    {
        public static float Distance(this Vector3 point, Vector3 to) =>
            Vector3.Distance(point, to);

        public static Vector3 Lerp(this Vector3 a, Vector3 b, float t) =>
            Vector3.Lerp(a, b, t);

        public static Vector3 XyZ(this Vector3 a) =>
            new Vector3(a.x, Mathf.Max(a.y, 0f), a.z);

        public static long Pack(this Vector3 vec) =>
            BitPackUtils.PackWorldPosForNetwork(vec);

        public static Vector3 Random(this Vector3 _, float power = 1) =>
            RandomVector3(power);

        public static Vector3 ClampMagnitude(this Vector3 vec, float magnitude) =>
            Vector3.ClampMagnitude(vec, magnitude);

        public static Vector3 ClampSqrMagnitude(this Vector3 vec, float sqrMagnitude)
        {
            float currentSqrMag = vec.sqrMagnitude;

            if (!(currentSqrMag > sqrMagnitude) || !(currentSqrMag > 0f)) return vec;
            float scale = MathF.Sqrt(sqrMagnitude / currentSqrMag);
            vec *= scale;

            return vec;
        }
    }
}
