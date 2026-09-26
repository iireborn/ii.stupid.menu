/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

﻿using iiMenu.Classes.Menu;

namespace iiMenu.Mods.CustomMaps
{
    public abstract class CustomMap
    {
        public abstract long MapID { get; }
        public abstract ButtonInfo[] Buttons { get; }
    }
}
