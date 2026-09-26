/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using HarmonyLib;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(FriendCard), nameof(FriendCard.Populate), new[] { typeof(FriendBackendController.Friend), typeof(bool) })]
    public class PopulatePatch
    {
        public static bool enabled;

        public static void Postfix(FriendCard __instance, FriendBackendController.Friend friend)
        {
            if (enabled)
            {
                bool custom = friend.Presence.RoomId[0] == '@';

                __instance.SetRoom((custom ? friend.Presence.RoomId[1..] : friend.Presence.RoomId).ToUpper());
                __instance.SetZone((custom ? "CUSTOM" : friend.Presence.Zone).ToUpper());
                __instance.joinable = true;

                __instance.UpdateComponentStates();
            }
        }
    }
}
