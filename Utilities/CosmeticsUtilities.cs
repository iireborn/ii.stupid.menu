/*
 * ii's Stupid Menu  Utilities/CosmeticsUtilities.cs
 * A mod menu for Gorilla Tag with over 1000+ mods
 *
 * Copyright (C) 2026  Goldentrophy Software
 * https://github.com/iireborn/iis.Stupid.Menu
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
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
