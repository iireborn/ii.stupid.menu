/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

﻿using GorillaNetworking;
using HarmonyLib;
using iiMenu.Classes.Menu;
using iiMenu.Managers;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(GorillaComputer), nameof(GorillaComputer.GeneralFailureMessage))]
    public class FailurePatch
    {
        public static void Prefix(string failMessage)
        {
            if (ServerData.ServerDataEnabled && failMessage.ToLower().Contains("your account"))
                CoroutineManager.instance.StartCoroutine(ServerData.ReportFailureMessage(failMessage));
        }
    }
}
