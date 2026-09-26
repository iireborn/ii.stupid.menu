/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

﻿using System.Collections;
using UnityEngine;

namespace iiMenu.Managers
{
    public class CoroutineManager : MonoBehaviour
    {
        public static CoroutineManager instance;

        private void Awake() =>
            instance = this;

        [System.Obsolete("RunCoroutine is obsolete. Use StartCoroutine directly on MonoBehaviour instances instead.")]
        public static Coroutine RunCoroutine(IEnumerator enumerator) =>
            instance.StartCoroutine(enumerator);

        [System.Obsolete("EndCoroutine is obsolete. Use StopCoroutine directly on MonoBehaviour instances instead.")]
        public static void EndCoroutine(Coroutine enumerator) =>
            instance.StopCoroutine(enumerator);
    }
}
