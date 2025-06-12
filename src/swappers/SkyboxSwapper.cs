using System;
using UnityEngine;

namespace ToasterReskinLoader.swappers;

public static class SkyboxSwapper
{
    public static void UpdateSkybox()
    {
        try
        {
            // Check if there is a skybox material assigned
            if (RenderSettings.skybox != null)
            {
                // Create a new instance of the skybox material to avoid
                // modifying the original asset.
                Material skyboxInstance = new Material(RenderSettings.skybox);

                // Set the new instance as the current skybox
                RenderSettings.skybox = skyboxInstance;

                // Now, modify the properties of our new instance.
                // Let's change the tint color to a reddish hue.
                // The property name "_SkyTint" is specific to the default
                // procedural skybox.
                skyboxInstance.SetFloat("_AtmosphereThickness", ReskinProfileManager.currentProfile.skyboxAtmosphereThickness);
                skyboxInstance.SetFloat("_Exposure", ReskinProfileManager.currentProfile.skyboxExposure);
                skyboxInstance.SetFloat("_SunDisk", ReskinProfileManager.currentProfile.skyboxSunDisk);
                skyboxInstance.SetFloat("_SunSize", ReskinProfileManager.currentProfile.skyboxSunSize);
                skyboxInstance.SetFloat("_SunSizeConvergence", ReskinProfileManager.currentProfile.skyboxSunSizeConvergence);
            
                skyboxInstance.SetColor("_GroundColor", ReskinProfileManager.currentProfile.skyboxGroundColor);
                skyboxInstance.SetColor("_SkyTint", ReskinProfileManager.currentProfile.skyboxSkyTint);
                // skyboxInstance.SetColor()
            }
            else
            {
                Debug.LogWarning("No skybox material found in RenderSettings.");
            }
        }
        catch (Exception e)
        {
            Plugin.LogError($"Error when updating skybox: {e.Message}");
        }
    }
}