/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using System.Collections.Generic;
using System.Text;

namespace iiMenu.Utilities
{
    /// <summary>
    /// Helpers for the dot-delimited cosmetics format used by telemetry.
    /// A cosmetic id such as "LBAAK" is serialized as "LBAAK.", so an entire inventory
    /// looks like "LBAAK.LBAGS.LBANI.LMAPY." and every '.' terminates one id.
    /// </summary>
    public static class CosmeticsUtilities
    {
        public const char CosmeticsDelimiter = '.';

        public static string NormalizeCosmeticId(string cosmetic)
        {
            if (string.IsNullOrEmpty(cosmetic))
                return string.Empty;

            // The game already terminates most ids with a '.', so strip it here to avoid "LBAAK..".
            return cosmetic.Trim().TrimEnd(CosmeticsDelimiter);
        }

        public static string SerializeOwnedCosmetics(IEnumerable<string> ownedCosmetics)
        {
            if (ownedCosmetics == null)
                return string.Empty;

            StringBuilder serialized = new StringBuilder();

            foreach (string cosmetic in ownedCosmetics)
            {
                string id = NormalizeCosmeticId(cosmetic);
                if (id.Length == 0)
                    continue;

                serialized.Append(id).Append(CosmeticsDelimiter);
            }

            return serialized.ToString();
        }

        public static List<string> ParseCosmeticsString(string cosmeticsString)
        {
            List<string> cosmetics = new List<string>();

            if (string.IsNullOrEmpty(cosmeticsString))
                return cosmetics;

            foreach (string cosmetic in cosmeticsString.Split(CosmeticsDelimiter))
            {
                string id = NormalizeCosmeticId(cosmetic);
                if (id.Length > 0)
                    cosmetics.Add(id);
            }

            return cosmetics;
        }
    }
}
