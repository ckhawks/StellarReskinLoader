using UnityEngine;

namespace ToasterReskinLoader.swappers;

public static class SwapperUtils
{
    public static string FindTextureProperty(Material material)
    {
        Shader shader = material.shader;
        int propertyCount = shader.GetPropertyCount();

        bool foundOnce = false;
        for (int i = 0; i < propertyCount; i++)
        {
            // Check if the property is a texture
            if (shader.GetPropertyType(i) == UnityEngine.Rendering.ShaderPropertyType.Texture)
            {
                string propertyName = shader.GetPropertyName(i);
                Plugin.Log($"Found texture property: {propertyName}");
                if (!foundOnce)
                {
                    foundOnce = true;
                }
                else
                {
                    return propertyName; // Return the first texture property found
                }
            }
        }

        return null; // No texture property found
    }
    
    public static string FindTextureProperties(Material material)
    {
        Shader shader = material.shader;
        int propertyCount = shader.GetPropertyCount();
        
        for (int i = 0; i < propertyCount; i++)
        {
            // Check if the property is a texture
            if (shader.GetPropertyType(i) == UnityEngine.Rendering.ShaderPropertyType.Texture)
            {
                string propertyName = shader.GetPropertyName(i);
                Plugin.Log($"Found texture property: {propertyName}");
            }
        }

        return null; // No texture property found
    }
}