/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

﻿using HarmonyLib;
using UnityEngine;
using UnityEngine.Rendering;
using static iiMenu.Menu.Main;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(GameObject), nameof(GameObject.CreatePrimitive))]
    public class ShaderFix
    {
        private static void Postfix(GameObject __result)
        {
            if (crystallizeMenu && CrystalMaterial != null)
                __result.GetComponent<Renderer>().material = CrystalMaterial;
            else if (transparentMenu)
            {
                Material material = __result.GetComponent<Renderer>().material;
                material.shader = Shader.Find(shinyMenu ? "Universal Render Pipeline/Lit" : "Universal Render Pipeline/Unlit");

                material.SetFloat("_Surface", 1);
                material.SetFloat("_Blend", 0);
                material.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
                material.SetFloat("_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
                material.SetFloat("_ZWrite", 0);
                material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                material.renderQueue = (int)RenderQueue.Transparent;
            } else
                __result.GetComponent<Renderer>().material.shader = Shader.Find(shinyMenu ? "Universal Render Pipeline/Lit" : "GorillaTag/UberShader");
            
            __result.GetComponent<Renderer>().material.color = backgroundColor.GetColor(0);
        }
    }
}