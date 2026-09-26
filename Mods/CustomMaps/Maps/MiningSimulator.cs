/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using iiMenu.Classes.Menu;
﻿using System.Collections.Generic;
using static iiMenu.Mods.CustomMaps.Manager;

namespace iiMenu.Mods.CustomMaps.Maps
{
    public class MiningSimulator : CustomMap
    {
        public override long MapID => 4977315;
        public override ButtonInfo[] Buttons => new[]
        {
            new ButtonInfo { buttonText = "Instant Mine", enableMethod = InstantMine, disableMethod = DisableInstantMine, toolTip = "Instantly mines any blocks with your pickaxe."},
            new ButtonInfo { buttonText = "Mine Anything", enableMethod = MineAnything, disableMethod = DisableMineAnything, toolTip = "Lets you mine any block."},
            new ButtonInfo { buttonText = "Infinite Backpack", enableMethod = InfiniteBackpack, disableMethod = DisableInfiniteBackpack, toolTip = "Lets you mine more blocks even if your inventory is full."},
        };

        public static void InstantMine()
        {
            ModifyCustomScript(new Dictionary<int, string>
                    {
                        { 974, "lastMinedTime = 0" }
                    });
        }
        public static void DisableInstantMine() =>
            RevertCustomScript(974);

        public static void MineAnything()
        {
            ModifyCustomScript(new Dictionary<int, string>
                    {
                        { 965, "if true then" }
                    });
        }
        public static void DisableMineAnything() =>
            RevertCustomScript(965);

        public static void InfiniteBackpack()
        {
            ModifyCustomScript(new Dictionary<int, string>
                    {
                        { 981, "if false then" }
                    });
        }
        public static void DisableInfiniteBackpack() =>
            RevertCustomScript(981);
    }
}
