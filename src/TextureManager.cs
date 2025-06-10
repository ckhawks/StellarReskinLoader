// TextureManager.cs

using System.Collections.Generic;
using UnityEngine;

namespace ToasterReskinLoader;

public static class TextureManager
{
    public static Dictionary<string, Texture2D> loadedTextures = new Dictionary<string, Texture2D>();

    // TODO optimize this so that we are only loading the textures for selected reskins
    public static void LoadTexturesForReskinPacks()
    {
        string[] texture2DTypes = new[] { "stick_attacker", "stick_goalie", "rink_ice", "puck" };

        // TODO this looping is not efficient because it's n*n because of GetReskinEntriesByType
        foreach (string texture2DType in texture2DTypes)
        {
            foreach(ReskinRegistry.ReskinEntry reskinEntry in ReskinRegistry.GetReskinEntriesByType(texture2DType))
            {
                Texture2D texture2D = LoadTexture(reskinEntry.Path);
                if (texture2D == null)
                {
                    Plugin.LogError($"ERROR LOADING RESKIN {reskinEntry.Name} TEXTURE: {reskinEntry.Path} is null!");
                    // return;
                }
                
                loadedTextures.Add(reskinEntry.Path, texture2D);
            }
        }
        
    }

    private static Texture2D LoadTexture(string filePath)
    {
        // TODO make all reskin's textures load into a dictionary or something at setup
        Plugin.Log($"Loading texture from {filePath}...");
        
        if (!System.IO.File.Exists(filePath))
        {
            Plugin.LogError($"File not found: {filePath}");
            return null;
        }

        try
        {
            // Read the file as a byte array
            byte[] fileData = System.IO.File.ReadAllBytes(filePath);

            // Create a new Texture2D
            Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);

            // Use ImageConversion.LoadImage to load the texture
            if (texture.LoadImage(fileData))
            {
                Plugin.Log("Texture loaded successfully!");
                return texture;
            }
            else
            {
                Plugin.LogError("Failed to load texture.");
                return null;
            }
        }
        catch (System.Exception ex)
        {
            Plugin.LogError($"Exception while loading texture: {ex.Message}");
            return null;
        }
    }
}