/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using GorillaLocomotion;
using UnityEngine;
using static iiMenu.Menu.Main;
using static iiMenu.Utilities.RandomUtilities;

namespace iiMenu.Classes.Mods
{
    public class CustomParticle : MonoBehaviour
	{
		public float spawnTime;
        public float startScale;

        public Renderer renderer;
		public Vector3 velocity;

		public void Awake()
		{
			spawnTime = Time.time;

			startScale = transform.localScale.x;

            renderer = gameObject.GetComponent<Renderer>() ?? null;
			velocity = RandomVector3(scaleWithPlayer ? GTPlayer.Instance.scale : 1f);

            Update();
		}

		public void Update()
		{
			if (renderer != null)
				renderer.material.color = buttonColors[1].GetCurrentColor();

			if (Time.time > spawnTime + 1f)
			{
				Destroy(gameObject);
				return;
			}

			transform.position += velocity * Time.unscaledDeltaTime;
			transform.localScale = Vector3.one * Mathf.Lerp(startScale, 0f, Time.time - spawnTime);
		}
	}
}
