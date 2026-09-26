/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using GorillaNetworking;
using HarmonyLib;
using PlayFab;
using System;
using System.Collections.Generic;

namespace iiMenu.Patches.Menu
{
    public class ErrorPatches
    {
        public static bool enabled;
        public static bool ErrorCall(PlayFabError error)
        {
            if (!enabled)
                return true;

            if (!error.ErrorMessage.Contains("is currently banned"))
                return true;

            using Dictionary<string, List<string>>.Enumerator enumerator = error.ErrorDetails.GetEnumerator();
            if (!enumerator.MoveNext())
                return false;

            KeyValuePair<string, List<string>> keyValuePair = enumerator.Current;

            bool isIndefinite = keyValuePair.Value[0] == "Indefinite";

            DateTime banEnd = isIndefinite ? DateTime.MaxValue : DateTime.Parse(keyValuePair.Value[0]);
            TimeSpan remaining = banEnd - DateTime.UtcNow;

            string banMessage = @$"Your account {PlayFabAuthenticator.instance.GetPlayFabPlayerId()} has been banned.
Ban Reason: {keyValuePair.Key}
Time Left: {(isIndefinite ? "Indefinite" : FormatTimeLeft(remaining))}
Unban Date: {(isIndefinite ? "Never" : banEnd.ToString("MMMM dd, yyyy h:mm tt"))}";

            GorillaComputer.instance.GeneralFailureMessage(banMessage);
            return false;
        }

        private static string FormatTimeLeft(TimeSpan time)
        {
            if (time <= TimeSpan.Zero)
                return "Expired";

            var parts = new List<string>();

            int months = time.Days / 30;
            int weeks = (time.Days % 30) / 7;
            int days = (time.Days % 30) % 7;

            if (months > 0) parts.Add($"{months} months");
            if (weeks > 0) parts.Add($"{weeks} weeks");
            if (days > 0) parts.Add($"{days} days");
            if (time.Hours > 0) parts.Add($"{time.Hours} hours");
            if (time.Minutes > 0) parts.Add($"{time.Minutes} minutes");
            if (time.Seconds > 0) parts.Add($"{time.Seconds} seconds");

            return string.Join(" ", parts);
        }
    }

    [HarmonyPatch(typeof(GorillaComputer), nameof(GorillaComputer.OnErrorShared))]
    public class ComputerErrorShared
    {
        public static bool Prefix(PlayFabError error) => ErrorPatches.ErrorCall(error);
    }

    [HarmonyPatch(typeof(PlayFabAuthenticator), nameof(PlayFabAuthenticator.OnPlayFabError))]
    public class PlayFabErrorShared
    {
        public static bool Prefix(PlayFabError obj) => ErrorPatches.ErrorCall(obj);
    }
}
