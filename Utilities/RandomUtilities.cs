/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using UnityEngine;

namespace iiMenu.Utilities
{
    public class RandomUtilities
    {
        /// <summary>
        /// Generates a Vector3 with each component set to a random value between -range and range.
        /// </summary>
        /// <param name="range">The maximum absolute value for each component of the generated vector.</param>
        /// <returns>A Vector3 with random x, y, and z components within the specified range.</returns>
        public static Vector3 RandomVector3(float range = 1f) =>
            new Vector3(Random.Range(-range, range),
                        Random.Range(-range, range),
                        Random.Range(-range, range));

        /// <summary>
        /// Generates a random Quaternion with each Euler angle component in the range [0, range).
        /// </summary>
        /// <param name="range">The upper bound (exclusive) for each Euler angle in degrees. Defaults to 360.</param>
        /// <returns>A Quaternion with random rotation based on the specified range.</returns>
        public static Quaternion RandomQuaternion(float range = 360f) =>
            Quaternion.Euler(Random.Range(0f, range),
                        Random.Range(0f, range),
                        Random.Range(0f, range));

        /// <summary>
        /// Generates a random color with each RGB component in the range [0, range) and the specified alpha value.
        /// </summary>
        /// <param name="range">The exclusive upper bound for the random RGB component values.</param>
        /// <param name="alpha">The alpha (transparency) value of the generated color.</param>
        /// <returns>A randomly generated Color with the specified range and alpha.</returns>
        public static Color RandomColor(byte range = 255, byte alpha = 255) =>
            new Color32((byte)Random.Range(0, range),
                        (byte)Random.Range(0, range),
                        (byte)Random.Range(0, range),
                        alpha);

        /// <summary>
        /// Generates a random alphanumeric string of the specified length using uppercase letters and digits.
        /// </summary>
        /// <param name="length">The length of the generated string. Defaults to 4.</param>
        /// <returns>A random string consisting of uppercase letters and digits.</returns>
        public static string RandomString(int length = 4)
        {
            string random = "";
            for (int i = 0; i < length; i++)
            {
                int rand = Random.Range(0, 36);
                char c = rand < 26
                    ? (char)('A' + rand)
                    : (char)('0' + (rand - 26));
                random += c;
            }

            return random;
        }
    }
}
