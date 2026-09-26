/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using GorillaNetworking;
using iiMenu.Classes.Menu;
using iiMenu.Managers;
using iiMenu.Menu;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace iiMenu.Mods
{
    public static class PublicRoomGuard
    {
        public static bool PublicRoomGuardEnabled;

        private static bool firing;

        public static void OnJoinRoom()
        {
            if (!PublicRoomGuardEnabled || firing || PhotonNetwork.InRoom == false || NetworkSystem.Instance.SessionIsPrivate)
                return;

            firing = true;
            CoroutineManager.instance.StartCoroutine(DisableDetectedMods());
        }

        private static IEnumerator DisableDetectedMods()
        {
            yield return null;

            try
            {
                var detectedButtons = Buttons.buttons
                    .SelectMany(category => category)
                    .Where(button => button.detected)
                    .ToArray();

                var disabled = new List<string>();

                foreach (var button in detectedButtons)
                {
                    if (button == null || !button.enabled)
                        continue;

                    Main.Toggle(button.buttonText);
                    disabled.Add(button.overlapText ?? button.buttonText);
                }

                Settings.SavePreferences();

                if (disabled.Count > 0)
                {
                    string list = string.Join(", ", disabled.Take(5));
                    if (disabled.Count > 5)
                        list += $" (+{disabled.Count - 5} more)";

                    NotificationManager.SendNotification(
                        $"<color=grey>[</color><color=yellow>GUARD</color><color=grey>]</color> Public room detected — disabled {disabled.Count} detected mod(s): {list}");
                }
            }
            finally
            {
                firing = false;
            }
        }
    }
}
