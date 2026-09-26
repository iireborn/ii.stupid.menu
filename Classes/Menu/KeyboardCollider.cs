/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using System.Collections.Generic;
using UnityEngine;
using static iiMenu.Menu.Main;

namespace iiMenu.Classes.Menu
{
    public class KeyboardKey : MonoBehaviour
	{
		public static readonly Dictionary<string, KeyboardKey> keyLookupDictionary = new Dictionary<string, KeyboardKey>();
		public string key;
		public static float delay;

		public void Start() =>
            keyLookupDictionary[gameObject.name] = this;
		
		public void OnTriggerEnter(Collider collider)
		{
			if ((collider != lKeyCollider && collider != rKeyCollider) || menu == null || !(Time.time > delay)) return;
			if (!iiMenu.Menu.Buttons.GetIndex("Disable Keyboard Delay").enabled)
				delay = Time.time + 0.1f;

			if (doButtonsVibrate)
				GorillaTagger.Instance.StartVibration(collider == lKeyCollider, GorillaTagger.Instance.tagHapticStrength / 2f, GorillaTagger.Instance.tagHapticDuration / 2f);
				
			VRRig.LocalRig.PlayHandTapLocal(66, collider == lKeyCollider, buttonClickVolume / 10f);
			PressKeyboardKey(key);
		}
	}
}
