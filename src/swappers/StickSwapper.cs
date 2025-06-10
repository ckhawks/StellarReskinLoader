using System.Collections.Generic;
using System.Reflection;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace ToasterReskinLoader.swappers;

public static class StickSwapper
{
    static readonly FieldInfo _stickMeshRendererField = typeof(StickMesh)
        .GetField("stickMeshRenderer", 
            BindingFlags.Instance | BindingFlags.NonPublic);
    
    static readonly FieldInfo _stickMaterialMapField = typeof(StickMesh)
        .GetField("stickMaterialMap", 
            BindingFlags.Instance | BindingFlags.NonPublic);
    
    public static bool SetStickTexture(Stick stick, ReskinRegistry.ReskinEntry reskin)
    {
        Plugin.Log($"Trying to replace stick texture, postspawn");
        StickMesh stickMesh = stick.StickMesh;
        if (stickMesh == null)
        {
            Plugin.LogError($"stickMesh is null!");
            return false;
        }
        
        MeshRenderer stickMeshRenderer = (MeshRenderer) _stickMeshRendererField.GetValue(stickMesh);
        if (stickMeshRenderer == null)
        {
            Plugin.LogError($"stickMeshRenderer is null!");
            return false;
        }

        // Reset to normal skin
        if (reskin.Path == null)
        {
            stickMesh.SetSkin(stick.Player.Team.Value, stick.Player.GetPlayerStickSkin().ToString());
            return false;
        }
        
        // Plugin.Log($"file path: {textureFilePath}");
        Texture2D texture2D = TextureManager.loadedTextures[reskin.Path];
        if (texture2D == null)
        {
            Plugin.LogError($"texture2D is null!");
            return false;
        }

        // Debugging: Log material and shader
        Plugin.Log($"Material: {stickMeshRenderer.material.name}");
        Plugin.Log($"Shader: {stickMeshRenderer.material.shader.name}");

        SerializedDictionary<string, Material> stickMaterialMap = (SerializedDictionary<string, Material>) _stickMaterialMapField.GetValue(stickMesh);

        string search = "red_beta_attacker";
        foreach (KeyValuePair<string, Material> pair in stickMaterialMap)
        {
            Plugin.Log($"Material key: {pair.Key}");
            Plugin.Log($"Material value: {pair.Value.name} - {pair.Value}");
            if (pair.Key.Contains(search))
                stickMeshRenderer.material = pair.Value;
        }
        
        // // This worked but only when someone had the stick in the world already
        // Material redBetaMaterial = FindMaterialByRendererScan("Stick Red Beta Attacker");
        // if (redBetaMaterial != null)
        // {
        //     Debug.Log("Found material: " + redBetaMaterial.name);
        //     // Use the material
        // }
        // else
        // {
        //     Debug.LogError("Material 'Stick Attacker Red Beta' not found in Resources!");
        //     return false;
        // }
        //
        // stickMeshRenderer.material = redBetaMaterial;
        
        // Dynamically find the texture property
        string texturePropertyName = SwapperUtils.FindTextureProperty(stickMeshRenderer.material);
        if (texturePropertyName == null)
        {
            Plugin.LogError("No texture property found in the shader.");
            return false;
        }

        // Apply the texture to the found property
        stickMeshRenderer.material.SetTexture(texturePropertyName, texture2D);
        Plugin.Log($"Texture applied to property: {texturePropertyName}");

        // Ensure the renderer is enabled
        if (!stickMeshRenderer.enabled)
        {
            Plugin.LogError("stickMeshRenderer is disabled. Enabling it.");
            stickMeshRenderer.enabled = true;
        }

        Plugin.Log("Texture applied to stick GameObject!");
        return true;
    }
    
    public static Material FindMaterialByRendererScan(string materialNamePartial)
    {
        // Get all active Renderers in the current scene
        Renderer[] allRenderers = GameObject.FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in allRenderers)
        {
            // Iterate through all shared materials on this renderer
            // An object might have multiple sub-meshes, each with a different material
            foreach (Material mat in renderer.sharedMaterials)
            {
                if (mat != null && mat.name.Contains(materialNamePartial))
                {
                    Debug.Log($"Found material '{mat.name}' on object '{renderer.gameObject.name}'");
                    return mat; // Return the first match
                }
            }
        }

        Debug.LogWarning($"Material '{materialNamePartial}' not found on any active renderer in the scene.");
        return null;
    }
}