﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace ToasterReskinLoader.swappers;

public static class PuckSwapper
{
    private static Texture originalTexture;
    private static Texture originalBumpMap;
    private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

    public static string puckBumpMapPath = "";

    public static void SetPuckTexture(Puck puck, ReskinRegistry.ReskinEntry reskinEntry)
    {
        try
        {
            // TODO fix this
            MeshRenderer puckMeshRenderer =
                puck.gameObject.transform.Find("puck").Find("Puck").GetComponent<MeshRenderer>();

            if (puckMeshRenderer == null)
            {
                Debug.LogError("No MeshRenderer found on GameObject Puck.");
            }

            // string texturePropertyName = SwapperUtils.FindTextureProperty(puckMeshRenderer.material);
            // if (texturePropertyName == null)
            // {
            //     Plugin.LogError("No texture property found in the shader.");
            //     return;
            // }

            if (originalTexture == null)
            {
                originalTexture = puckMeshRenderer.material.GetTexture("_BaseMap");
            }

            if (originalBumpMap == null)
            {
                originalBumpMap = puckMeshRenderer.material.GetTexture("_BumpMap");
            }

            // If setting to unchanged,
            if (reskinEntry.Path == null)
            {
                puckMeshRenderer.material.SetTexture(BaseMap, originalTexture);
                puckMeshRenderer.material.SetTexture("_BumpMap", originalBumpMap);
                // Plugin.Log("Original texture applied to property: _BaseMap");
            }
            else
            {
                puckMeshRenderer.material.SetTexture(BaseMap, TextureManager.GetTexture(reskinEntry));
                puckMeshRenderer.material.SetTexture("_BumpMap", TextureManager.GetTextureFromFilePath(puckBumpMapPath));
                // Plugin.Log("Texture applied to property: _BaseMap");
            }

            // Plugin.Log($"Set the puck texture to {reskinEntry.Name} {reskinEntry.Path}");
        }
        catch (Exception ex)
        {
            Plugin.LogError($"Error while setting puck texture: {ex.Message}");
        }
    }

    public static void GetBumpMapPathAndLoad()
    {
        // string workshopModsRoot = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(execPath)!, ".."));
        puckBumpMapPath = Path.Combine(Path.GetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)), "puck_normal.png");
        TextureManager.GetTextureFromFilePath(puckBumpMapPath);
    }

    public static void SetAllPucksTextures()
    {
        List<Puck> pucks = PuckManager.Instance.GetPucks();
        foreach (Puck puck in pucks)
        {
            SetPuckTexture(puck, ReskinProfileManager.currentProfile.puck);
        }
        Plugin.LogDebug($"Updated all pucks to have correct texture.");
    }
    
    [HarmonyPatch(typeof(Puck), "OnNetworkPostSpawn")]
    public static class PuckOnNetworkPostSpawn
    {
        [HarmonyPostfix]
        public static void Postfix(Puck __instance)
        {
            SetPuckTexture(__instance, ReskinProfileManager.currentProfile.puck);
        }
    }
}