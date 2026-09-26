/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using GorillaGameModes;
using HarmonyLib;
using iiMenu.Extensions;
using iiMenu.Menu;
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using static iiMenu.Utilities.AssetUtilities;
using static iiMenu.Utilities.GameModeUtilities;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(GameMode), nameof(GameMode.ReportTag))]
    public class TagPatch
    {
        public static readonly List<NetPlayer> taggedPlayers = new List<NetPlayer>();

        public static bool enabled;
        public static float tagDelay;
        public static int tagCount;

        // Internal inconsistency fixed in commit c9b3048f36b61ca9c60ebba720d33c4194da4fbd. Remote ServerResourcePath only serves .ogg, never .wav!
        private static void PlaySound(string name) =>
            LoadSoundFromURL($"{PluginInfo.ServerResourcePath}/Audio/Mods/Fun/TagSounds/{name}.ogg", $"Audio/Mods/Fun/TagSounds/{name}.ogg").Play(Main.buttonClickVolume / 10f);

        public static void Postfix(NetPlayer player)
        {
            if (enabled && PhotonNetwork.InRoom)
            {
                if (Time.time > tagDelay)
                {
                    taggedPlayers.Clear();
                    tagCount = 0;
                }

                if (!taggedPlayers.Contains(player))
                {
                    taggedPlayers.Add(player);
                    tagCount = Math.Min(tagCount + 1, 7);
                    tagDelay = Time.time + 10f;

                    switch (tagCount)
                    {
                        case 1:
                            if (InfectedList().Count <= 1)
                                PlaySound("firstblood");

                            break;
                        case 2:
                            PlaySound("doublekill");
                            break;
                        case 3:
                            PlaySound("triplekill");
                            break;
                        case 4:
                            PlaySound("killingspree");
                            break;
                        case 5:
                            PlaySound("wickedsick");
                            break;
                        case 6:
                            PlaySound("monsterkill");
                            break;
                        case 7:
                            PlaySound("rampage");
                            break;
                    }
                }
            }
        }
    }
}
