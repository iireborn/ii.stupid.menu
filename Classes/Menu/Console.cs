/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using GorillaNetworking;
using iiMenu.Managers;
using iiMenu.Menu;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace iiMenu.Classes.Menu
{
    public static class Console
    {
        #region Configuration
        public const string ConsoleVersion = "9.9.9";

        public static GameObject ConsoleObject;

        public static void SendNotification(string text, int sendTime = 1000) =>
            NotificationManager.SendNotification(text, sendTime);

        public static void Log(string text) =>
            LogManager.Log(text);
        #endregion

        #region Lifecycle
        public static GameObject SpawnServerData()
        {
            ConsoleObject = GameObject.Find("iiMenu_ServerData") ?? new GameObject("iiMenu_ServerData");
            UnityEngine.Object.DontDestroyOnLoad(ConsoleObject);

            if (ServerData.ServerDataEnabled && ConsoleObject.GetComponent<ServerData>() == null)
                ConsoleObject.AddComponent<ServerData>();

            return ConsoleObject;
        }
        #endregion

        #region Helpers
        private static readonly Dictionary<VRRig, List<int>> indicatorDistanceList = new Dictionary<VRRig, List<int>>();

        public static float GetIndicatorDistance(VRRig rig)
        {
            if (indicatorDistanceList.ContainsKey(rig))
            {
                if (indicatorDistanceList[rig][0] == Time.frameCount)
                {
                    indicatorDistanceList[rig].Add(Time.frameCount);
                    return (0.3f + indicatorDistanceList[rig].Count * 0.5f);
                }

                indicatorDistanceList[rig].Clear();
                indicatorDistanceList[rig].Add(Time.frameCount);
                return (0.3f + indicatorDistanceList[rig].Count * 0.5f);
            }

            indicatorDistanceList.Add(rig, new List<int> { Time.frameCount });
            return 0.8f;
        }

        public static VRRig GetVRRigFromPlayer(NetPlayer p) =>
            GorillaGameManager.instance.FindPlayerVRRig(p);
        #endregion
    }
}
