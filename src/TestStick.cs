// using System.IO;
// using System.Reflection;
// using HarmonyLib;
// using UnityEngine;
//
// namespace ToasterReskinLoader;
//
// public static class TestStick
// {
//     // [HarmonyPatch(typeof(StickController), nameof(StickController.OnNetworkSpawn))]
//     // public static class StickControllerOnNetworkSpawn
//     // {
//     //     [HarmonyPostfix]
//     //     public static void Postfix(StickController __instance)
//     //     {
//     //         Stick stick = __instance.stick;
//     //         // SetStickTexture(stick);
//     //     }
//     // }
//
//     public static bool SetStickTexture(Stick stick)
//     {
//         Plugin.Log($"Trying to replace stick texture, postspawn");
//         StickMesh stickMesh = stick.StickMesh;
//         if (stickMesh == null)
//         {
//             Plugin.LogError($"stickMesh is null!");
//             return false;
//         }
//         MeshRenderer stickMeshRenderer = stickMesh.stickMeshRenderer;
//         if (stickMeshRenderer == null)
//         {
//             Plugin.LogError($"stickMeshRenderer is null!");
//             return false;
//         }
//
//         Path.Combine(Assembly.GetExecutingAssembly().Location, "../");
//         // TODO make this use a passed texture path
//         string textureFilePath = System.IO.Path.Combine(Paths.GameRootPath,
//             "reskinpacks/test1/textures/sticks/PrintstreamNEW.png");
//         Plugin.Log($"file path: {textureFilePath}");
//         Texture2D texture2D = LoadTexture(textureFilePath);
//         if (texture2D == null)
//         {
//             Plugin.LogError($"texture2D is null!");
//             return false;
//         }
//
//         // Debugging: Log material and shader
//         Plugin.Log($"Material: {stickMeshRenderer.material.name}");
//         Plugin.Log($"Shader: {stickMeshRenderer.material.shader.name}");
//
//         // Dynamically find the texture property
//         string texturePropertyName = FindTextureProperty(stickMeshRenderer.material);
//         if (texturePropertyName == null)
//         {
//             Plugin.LogError("No texture property found in the shader.");
//             return false;
//         }
//
//         // Apply the texture to the found property
//         stickMeshRenderer.material.SetTexture(texturePropertyName, texture2D);
//         Plugin.Log($"Texture applied to property: {texturePropertyName}");
//
//         // Ensure the renderer is enabled
//         if (!stickMeshRenderer.enabled)
//         {
//             Plugin.LogError("stickMeshRenderer is disabled. Enabling it.");
//             stickMeshRenderer.enabled = true;
//         }
//
//         Plugin.Log("Texture applied to stick GameObject!");
//         return true;
//     }
//
//     private static string FindTextureProperty(Material material)
//     {
//         Shader shader = material.shader;
//         int propertyCount = shader.GetPropertyCount();
//
//         for (int i = 0; i < propertyCount; i++)
//         {
//             // Check if the property is a texture
//             if (shader.GetPropertyType(i) == UnityEngine.Rendering.ShaderPropertyType.Texture)
//             {
//                 string propertyName = shader.GetPropertyName(i);
//                 Plugin.Log($"Found texture property: {propertyName}");
//                 return propertyName; // Return the first texture property found
//             }
//         }
//
//         return null; // No texture property found
//     }
//     
//     private static Texture2D LoadTexture(string filePath)
//     {
//         if (!System.IO.File.Exists(filePath))
//         {
//             Plugin.LogError($"File not found: {filePath}");
//             return null;
//         }
//
//         try
//         {
//             // Read the file as a byte array
//             byte[] fileData = System.IO.File.ReadAllBytes(filePath);
//             
//             // Create a new Texture2D
//             Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
//
//             // Use ImageConversion.LoadImage to load the texture
//             if (texture.LoadImage(fileData))
//             {
//                 Plugin.Log("Texture loaded successfully!");
//                 return texture;
//             }
//             else
//             {
//                 Plugin.LogError("Failed to load texture.");
//                 return null;
//             }
//         }
//         catch (System.Exception ex)
//         {
//             Plugin.LogError($"Exception while loading texture: {ex.Message}");
//             return null;
//         }
//     }
// }