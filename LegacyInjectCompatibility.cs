/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using iiMenu;

#pragma warning disable IDE0130 // Namespace does not match folder structure [for legacy compatibility with default SMI settings]
// ReSharper disable once CheckNamespace
namespace Loading
#pragma warning restore IDE0130 // Namespace does not match folder structure [for legacy compatibility with default SMI settings]
{
    public static class Loader 
    {
        public static void Load() =>
            Plugin.InjectDontDestroy();
    }
}
