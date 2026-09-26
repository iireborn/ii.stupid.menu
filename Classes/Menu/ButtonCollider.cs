/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using iiMenu.Managers;
using UnityEngine;
using static iiMenu.Menu.Main;

namespace iiMenu.Classes.Menu
{
    public class ButtonCollider : MonoBehaviour
	{
		public string relatedText;

		public bool incremental;
		public bool positive;
		public bool interactable = true;

		public void OnTriggerEnter(Collider collider)
		{
			if (!(Time.time > buttonCooldown) ||
			    (collider != buttonCollider && collider != lKeyCollider && collider != rKeyCollider) || joystickMenu ||
			    menu == null) return;

			PressFromMouse();
		}

		public void PressFromMouse()
		{
			if (!interactable || !(Time.time > buttonCooldown) || joystickMenu || menu == null) return;
			buttonCooldown = Time.time + 0.2f;
			PlayButtonSound(relatedText);

			if (annoyingMode)
			{
				if (Random.Range(1, 5) == 2)
				{
					NotificationManager.SendNotification("Try again loser >:3");
					return;
				}
			}

			if (incremental)
				ToggleIncremental(relatedText, positive);
			else
				Toggle(relatedText, true);
		}
	}
}
