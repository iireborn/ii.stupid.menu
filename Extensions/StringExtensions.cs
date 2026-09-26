/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using static iiMenu.Menu.Main;
using static iiMenu.Utilities.RandomUtilities;

namespace iiMenu.Extensions
{
    public static class StringExtensions
    {
        public static string ClearTags(this string input) =>
            NoRichtextTags(input);

        public static string ToTitleCase(this string input) =>
            Menu.Main.ToTitleCase(input);

        public static string Hash(this string input) =>
            GetSHA256(input);

        public static string EnforceLength(this string str, int maxLength) =>
            str.Length > maxLength ? str[..maxLength] : str;

        public static string Random(this string _, int length) =>
            RandomString(length);
    }
}
