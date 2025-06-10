using UnityEngine;

namespace ToasterReskinLoader.swappers;

public class IceSwapper
{
    private static Texture originalTexture;

    public static bool SetIceTexture()
    {
        ReskinRegistry.ReskinEntry reskinEntry = ReskinProfileManager.profile.ice;

        var (go, renderer, material, originalTexture2) = FindUsageOfTexture();
        Plugin.Log($"go: {go.name}, renderer: {renderer.name}, material: {material.name}, originalTexture: {originalTexture2.name}");
        
        GameObject iceBottomGameObject = GameObject.Find("Ice Bottom");

        if (iceBottomGameObject == null)
        {
            Plugin.LogError($"Could not locate Ice Top GameObject.");
            return false;
        }
        
        MeshRenderer iceBottomMeshRenderer = iceBottomGameObject.GetComponent<MeshRenderer>();

        if (iceBottomMeshRenderer == null)
        {
            Debug.LogError("No MeshRenderer found on GameObject Ice Top.");
        }
        
        string texturePropertyName = SwapperUtils.FindTextureProperty(iceBottomMeshRenderer.material);
        if (texturePropertyName == null)
        {
            Plugin.LogError("No texture property found in the shader.");
            return false;
        }
        
        if (originalTexture == null)
        {
            originalTexture = iceBottomMeshRenderer.material.GetTexture(texturePropertyName);
        }
        
        // If setting to unchanged,
        if (reskinEntry.Path == null)
        {
            iceBottomMeshRenderer.material.SetTexture(texturePropertyName, originalTexture);
            Plugin.Log($"Texture applied to property: {texturePropertyName}");
        }
        else
        {
            iceBottomMeshRenderer.material.SetTexture(texturePropertyName, TextureManager.loadedTextures[reskinEntry.Path]);
            Plugin.Log($"Texture applied to property: {texturePropertyName}");
        }

        // iceBottomMeshRenderer.enabled = false;
        
        // TODO find the Ice Top material
        // TODO 
        Plugin.Log($"Set the Ice Top texture to {reskinEntry.Name} {reskinEntry.Path}");
        return true;
    }
    
    private const string TargetTextureName = "hockey_rink";
    
    /// <summary>
    /// Scans the scene to find a GameObject, MeshRenderer, and Material that uses the
    /// specified texture. This should be called sparingly due to performance.
    /// </summary>
    /// <returns>
    /// A tuple containing:
    /// - GameObject: The GameObject found, or null.
    /// - MeshRenderer (or other Renderer): The renderer found, or null.
    /// - Material: The material found, or null.
    /// - Texture2D: The original texture that was found, or null.
    /// </returns>
    public static (GameObject go, Renderer renderer, Material material, Texture2D texture)
        FindUsageOfTexture()
    {
        // Find all active Renderers in the current scene.
        // This is a slow operation, avoid calling in Update.
        Renderer[] allRenderers = GameObject.FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in allRenderers)
        {
            if (renderer == null || renderer.sharedMaterials == null)
            {
                continue;
            }

            foreach (Material mat in renderer.sharedMaterials)
            {
                if (mat == null)
                {
                    continue;
                }

                // Check the mainTexture property first (common for many shaders)
                Texture mainTex = mat.mainTexture;
                if (mainTex != null && mainTex.name == TargetTextureName)
                {
                    Debug.Log($"Found '{TargetTextureName}' on material '{mat.name}' " +
                              $"on renderer '{renderer.gameObject.name}'. (Main Texture)");
                    return (renderer.gameObject, renderer, mat, mainTex as Texture2D);
                }

                // --- ADVANCED: Check other common texture properties by name ---
                // You might need to know the specific shader properties if mainTexture isn't it.
                // Examples: _BaseMap (URP), _Albedo (Standard), _MainTex (Legacy/Custom)
                foreach (string propName in new string[] { "_MainTex", "_BaseMap", "_Albedo" }) // Add more as needed
                {
                    if (mat.HasProperty(propName))
                    {
                        Texture otherTex = mat.GetTexture(propName);
                        if (otherTex != null && otherTex.name == TargetTextureName)
                        {
                            Debug.Log($"Found '{TargetTextureName}' on material '{mat.name}' " +
                                      $"on renderer '{renderer.gameObject.name}'. (Property: {propName})");
                            return (renderer.gameObject, renderer, mat, otherTex as Texture2D);
                        }
                    }
                }
                // --- END ADVANCED ---
            }
        }

        Debug.LogWarning($"Texture '{TargetTextureName}' not found on any active renderer in the scene.");
        return (null, null, null, null); // Not found
    }
}