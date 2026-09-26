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
    public class FlightSimulator : CustomMap
    {
        public override long MapID => 5024157;
        public override ButtonInfo[] Buttons => new[]
        {
            new ButtonInfo { buttonText = "Steal Pilot", enableMethod = StealPilot, disableMethod = DisableStealPilot, toolTip = "Allows you to steal the pilot position from other people's planes."},
        };

        public static void StealPilot()
        {
            ModifyCustomScript(new Dictionary<int, string>
                    {
                        { 373, "if isButtonPressed(pilotClaimButtonJet) then" }
                    });
        }
        public static void DisableStealPilot() =>
            RevertCustomScript(373);
    }
}
