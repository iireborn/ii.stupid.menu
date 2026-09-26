/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;
using iiMenu.Extensions;
using iiMenu.Menu;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(GorillaPlayerScoreboardLine), nameof(GorillaPlayerScoreboardLine.UpdatePlayerText))]
    public class UpdatePatch
    {
        public static bool enabled;

        private static int GetPing(VRRig rig)
        {
            int ping = rig.GetPing();
            return ping <= 150 ? 5 : ping <= 300 ? 4 : ping <= 450 ? 3 : ping <= 600 ? 2 : 1;
        }

        public static void Postfix(GorillaPlayerScoreboardLine __instance)
        {
            if (enabled)
            {
                string targetName = Main.CleanPlayerName(__instance.linePlayer.NickName) + " ERR";
                try
                {
                    VRRig rig = __instance.linePlayer.VRRig();
                    targetName = $"{Main.CleanPlayerName(__instance.linePlayer.NickName)}<size=50> <sprite name=\"{rig.GetPlatform()}\"> <sprite name=\"Ping{GetPing(rig)}\">{rig.fps}</size>";
                } catch { }
                __instance.playerNameVisible = targetName;
            }
        }
    }
}
