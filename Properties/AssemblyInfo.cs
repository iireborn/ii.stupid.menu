/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using iiMenu;
using System.Reflection;

[assembly: AssemblyCompany("Goldentrophy Software")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyFileVersion(PluginInfo.Version + ".0")]
[assembly: AssemblyInformationalVersion(PluginInfo.Version)]
[assembly: AssemblyVersion(PluginInfo.Version + ".0")]

#if DEBUG
[assembly: AssemblyProduct(PluginInfo.Name + " [Debug]")]
[assembly: AssemblyTitle(PluginInfo.Name + " [Debug]")]
#else
[assembly: AssemblyProduct(PluginInfo.Name)]
[assembly: AssemblyTitle(PluginInfo.Name)]
#endif

[assembly: AssemblyDescription(PluginInfo.Description + " [Debug]")]
