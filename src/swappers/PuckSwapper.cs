using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace ToasterReskinLoader.swappers;

public static class PuckSwapper
{
    private static Texture originalTexture;
    
    public static void SetPuckTexture(Puck puck, ReskinRegistry.ReskinEntry reskinEntry)
    {
        // TODO fix this
        MeshRenderer puckMeshRenderer = puck.gameObject.transform.Find("puck").Find("Puck").GetComponent<MeshRenderer>();
        
        if (puckMeshRenderer == null)
        {
            Debug.LogError("No MeshRenderer found on GameObject Puck.");
        }
        
        string texturePropertyName = SwapperUtils.FindTextureProperty(puckMeshRenderer.material);
        if (texturePropertyName == null)
        {
            Plugin.LogError("No texture property found in the shader.");
            return;
        }
        
        if (originalTexture == null)
        {
            originalTexture = puckMeshRenderer.material.GetTexture(texturePropertyName);
        }
        
        // If setting to unchanged,
        if (reskinEntry.Path == null)
        {
            puckMeshRenderer.material.SetTexture(texturePropertyName, originalTexture);
            Plugin.Log($"Texture applied to property: {texturePropertyName}");
        }
        else
        {
            puckMeshRenderer.material.SetTexture(texturePropertyName, TextureManager.loadedTextures[reskinEntry.Path]);
            Plugin.Log($"Texture applied to property: {texturePropertyName}");
        }
        
        Plugin.Log($"Set the puck texture to {reskinEntry.Name} {reskinEntry.Path}");
    }

    public static void SetAllPucksTextures()
    {
        List<Puck> pucks = PuckManager.Instance.GetPucks();
        foreach (Puck puck in pucks)
        {
            SetPuckTexture(puck, ReskinProfileManager.profile.puck);
        }
    }
    
    [HarmonyPatch(typeof(Puck), "OnNetworkPostSpawn")]
    public static class PuckOnNetworkPostSpawn
    {
        [HarmonyPostfix]
        public static void Postfix(Puck __instance)
        {
            SetPuckTexture(__instance, ReskinProfileManager.profile.puck);
        }
    }
}