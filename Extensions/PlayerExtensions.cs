/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using ExitGames.Client.Photon;
using GorillaLocomotion;
using iiMenu.Utilities;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine;

namespace iiMenu.Extensions
{
    public static class PlayerExtensions
    {
        #region NetPlayer
        public static Player GetPlayer(this NetPlayer self) =>
            RigUtilities.NetPlayerToPlayer(self);

        public static VRRig VRRig(this NetPlayer self) =>
            RigUtilities.GetVRRigFromPlayer(self);

        public static bool InRoom(this NetPlayer self) =>
            NetworkSystem.Instance.AllNetPlayers.Contains(self);

        public static Hashtable GetCustomProperties(this NetPlayer self) =>
            self.GetPlayer().CustomProperties;
        #endregion

        #region Player
        public static VRRig VRRig(this Player self) =>
            RigUtilities.GetVRRigFromPlayer(self);

        public static bool InRoom(this Player self) =>
            PhotonNetwork.PlayerList.Contains(self);

        #endregion

        #region GorillaTagger
        public static bool IsGrounded(this GorillaTagger tagger, float maxDistance = 0.15f)
        {
            return
                Physics.Raycast(tagger.bodyCollider.transform.position - new Vector3(0f, 0.2f, 0f), Vector3.down, maxDistance, GTPlayer.Instance.locomotionEnabledLayers);
        }
        #endregion
    }
}
