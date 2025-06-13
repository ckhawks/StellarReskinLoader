﻿// ReskinProfileManager.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ToasterReskinLoader.swappers;
using UnityEngine;

namespace ToasterReskinLoader;

public static class ReskinProfileManager
{
    // TODO make this inside of a dictionary or profile setting or something
    public static Profile currentProfile { get; private set; } = new Profile();

    private static string ProfilePath = Path.Combine(Path.GetFullPath(Path.Combine(Application.dataPath, "..")), "reskinprofiles", "ReskinProfile.json");

    public static void SetSelectedReskinInCurrentProfile(ReskinRegistry.ReskinEntry reskinEntry, string type, string slot)
    {
        if (type == "stick_attacker")
        {
            switch (slot)
            {
                case "blue_personal":
                    currentProfile.stickAttackerBluePersonal = reskinEntry;
                    SwapperManager.OnPersonalStickChanged();
                    break;
                case "red_personal":
                    currentProfile.stickAttackerRedPersonal = reskinEntry;
                    SwapperManager.OnPersonalStickChanged();
                    break;
                case "blue_team":
                    currentProfile.stickAttackerBlue = reskinEntry;
                    SwapperManager.OnBlueTeamStickChanged();
                    break;
                case "red_team":
                    currentProfile.stickAttackerRed = reskinEntry;
                    SwapperManager.OnRedTeamStickChanged();
                    break;
            }
        } 
        else if (type == "stick_goalie")
        {
            switch (slot)
            {
                case "blue_personal":
                    currentProfile.stickGoalieBluePersonal = reskinEntry;
                    SwapperManager.OnPersonalStickChanged();
                    break;
                case "red_personal":
                    currentProfile.stickGoalieRedPersonal = reskinEntry;
                    SwapperManager.OnPersonalStickChanged();
                    break;
                case "blue_team":
                    currentProfile.stickGoalieBlue = reskinEntry;
                    SwapperManager.OnBlueTeamStickChanged();
                    break;
                case "red_team":
                    currentProfile.stickGoalieRed = reskinEntry;
                    SwapperManager.OnRedTeamStickChanged();
                    break;
            }
        } else if (type == "jersey_torso")
        {
            switch (slot)
            {
                case "blue_skater":
                    currentProfile.blueSkaterTorso = reskinEntry;
                    SwapperManager.OnBlueJerseyChanged();
                    break;
                case "red_skater":
                    currentProfile.redSkaterTorso = reskinEntry;
                    SwapperManager.OnRedJerseyChanged();
                    break;
                case "blue_goalie":
                    currentProfile.blueGoalieTorso = reskinEntry;
                    SwapperManager.OnBlueJerseyChanged();
                    break;
                case "red_goalie":
                    currentProfile.redGoalieTorso = reskinEntry;
                    SwapperManager.OnRedJerseyChanged();
                    break;
            }
        } else if (type == "jersey_groin")
        {
            switch (slot)
            {
                case "blue_skater":
                    currentProfile.blueSkaterGroin = reskinEntry;
                    SwapperManager.OnBlueJerseyChanged();
                    break;
                case "red_skater":
                    currentProfile.redSkaterGroin = reskinEntry;
                    SwapperManager.OnRedJerseyChanged();
                    break;
                case "blue_goalie":
                    currentProfile.blueGoalieGroin = reskinEntry;
                    SwapperManager.OnBlueJerseyChanged();
                    break;
                case "red_goalie":
                    currentProfile.redGoalieGroin = reskinEntry;
                    SwapperManager.OnRedJerseyChanged();
                    break;
            }
        }
        else if (type == "rink_ice")
        {
            // We aren't using slot here
            currentProfile.ice = reskinEntry;
            IceSwapper.SetIceTexture();
        }
        else if (type == "puck")
        {
            currentProfile.puck = reskinEntry;
            PuckSwapper.SetAllPucksTextures();
        } else if (type == "net")
        {
            currentProfile.net = reskinEntry;
            ArenaSwapper.SetNetTexture();
        }
        
        SaveProfile();
    }

    public static void LoadProfile()
    {
        string profilesFolder = Path.Combine(Path.GetFullPath(Path.Combine(Application.dataPath, "..")), "reskinprofiles");
        if (!Directory.Exists(profilesFolder))
        {
            Plugin.LogError($"Local reskin profiles folder not found: {profilesFolder}, creating it...");
            Directory.CreateDirectory(profilesFolder);
        }
        
        if (!File.Exists(ProfilePath))
        {
            Plugin.Log("No reskin profile found. Creating a default profile.");
            currentProfile = new Profile();
            SaveProfile(); // Save the new default profile
            return;
        }
        
        // Create a new profile instance that holds all the correct default values.
        // This is our safety net.
        var defaultProfile = new Profile();
        
        try
        {
            Plugin.Log($"Loading reskin profile from: {ProfilePath}");
            string json = File.ReadAllText(ProfilePath);
            var serializableProfile =
                JsonConvert.DeserializeObject<SerializableProfile>(json);

            // If the JSON file is empty or invalid, serializableProfile could be null.
            if (serializableProfile == null)
            {
                Plugin.LogError(
                    "Failed to deserialize profile (file might be empty or corrupt). Loading default profile."
                );
                currentProfile = defaultProfile;
                return;
            }

            Plugin.Log($"Deserialized {ProfilePath}, loading values...");
            // "Hydrate" the profile: convert references back to live ReskinEntry objects
            currentProfile = new Profile
            {
                // Sticks
                stickAttackerBluePersonal = FindEntryFromReference(serializableProfile?.StickAttackerBluePersonalRef),
                stickAttackerRedPersonal = FindEntryFromReference(serializableProfile?.StickAttackerRedPersonalRef),
                stickAttackerBlue = FindEntryFromReference(serializableProfile?.StickAttackerBlueRef),
                stickAttackerRed = FindEntryFromReference(serializableProfile?.StickAttackerRedRef),
                stickGoalieBluePersonal = FindEntryFromReference(serializableProfile?.StickGoalieBluePersonalRef),
                stickGoalieRedPersonal = FindEntryFromReference(serializableProfile?.StickGoalieRedPersonalRef),
                stickGoalieBlue = FindEntryFromReference(serializableProfile?.StickGoalieBlueRef),
                stickGoalieRed = FindEntryFromReference(serializableProfile?.StickGoalieRedRef),
                
                // Jerseys
                blueSkaterTorso = FindEntryFromReference(serializableProfile?.BlueSkaterTorsoRef),
                blueSkaterGroin = FindEntryFromReference(serializableProfile?.BlueSkaterGroinRef),
                blueGoalieTorso = FindEntryFromReference(serializableProfile?.BlueGoalieTorsoRef),
                blueGoalieGroin = FindEntryFromReference(serializableProfile?.BlueGoalieGroinRef),
                redSkaterTorso = FindEntryFromReference(serializableProfile?.RedSkaterTorsoRef),
                redSkaterGroin = FindEntryFromReference(serializableProfile?.RedSkaterGroinRef),
                redGoalieTorso = FindEntryFromReference(serializableProfile?.RedGoalieTorsoRef),
                redGoalieGroin = FindEntryFromReference(serializableProfile?.RedGoalieGroinRef),
                
                // Puck
                puck = FindEntryFromReference(serializableProfile?.PuckRef),

                // Arena
                // Use the ?? (null-coalescing) operator. If the loaded value is null, use the default.
                crowdEnabled = serializableProfile.CrowdEnabled
                    ?? defaultProfile.crowdEnabled,
                hangarEnabled = serializableProfile.HangarEnabled
                    ?? defaultProfile.hangarEnabled,
                glassEnabled = serializableProfile.GlassEnabled
                                ?? defaultProfile.glassEnabled,
                scoreboardEnabled = serializableProfile.ScoreboardEnabled
                                ?? defaultProfile.scoreboardEnabled,
                ice = FindEntryFromReference(serializableProfile.IceRef),
                iceSmoothness = serializableProfile.IceSmoothness
                    ?? defaultProfile.iceSmoothness,

                // For colors, we check if the SerializableColor object is null.
                boardsBorderTopColor =
                    serializableProfile.BoardsBorderTopColor != null
                        ? (Color)serializableProfile.BoardsBorderTopColor
                        : defaultProfile.boardsBorderTopColor,
                boardsMiddleColor =
                    serializableProfile.BoardsMiddleColor != null
                        ? (Color)serializableProfile.BoardsMiddleColor
                        : defaultProfile.boardsMiddleColor,
                boardsBorderBottomColor =
                    serializableProfile.BoardsBorderBottomColor != null
                        ? (Color)serializableProfile.BoardsBorderBottomColor
                        : defaultProfile.boardsBorderBottomColor,
                glassSmoothness = serializableProfile.GlassSmoothness ?? defaultProfile.glassSmoothness,
                pillarsColor = serializableProfile.PillarsColor != null
                    ? (Color)serializableProfile.PillarsColor
                    : defaultProfile.pillarsColor,
                spectatorDensity =  serializableProfile.SpectatorDensity ?? defaultProfile.spectatorDensity,
                net = FindEntryFromReference(serializableProfile?.NetRef),

                // Skybox
                skyboxAtmosphereThickness =
                    serializableProfile.SkyboxAtmosphereThickness
                    ?? defaultProfile.skyboxAtmosphereThickness,
                skyboxExposure = serializableProfile.SkyboxExposure
                    ?? defaultProfile.skyboxExposure,
                skyboxSunDisk = serializableProfile.SkyboxSunDisk
                    ?? defaultProfile.skyboxSunDisk,
                skyboxSunSize = serializableProfile.SkyboxSunSize
                    ?? defaultProfile.skyboxSunSize,
                skyboxSunSizeConvergence =
                    serializableProfile.SkyboxSunSizeConvergence
                    ?? defaultProfile.skyboxSunSizeConvergence,
                skyboxGroundColor =
                    serializableProfile.SkyboxGroundColor != null
                        ? (Color)serializableProfile.SkyboxGroundColor
                        : defaultProfile.skyboxGroundColor,
                skyboxSkyTint =
                    serializableProfile.SkyboxSkyTint != null
                        ? (Color)serializableProfile.SkyboxSkyTint
                        : defaultProfile.skyboxSkyTint,
            };

            Plugin.Log("Reskin profile loaded successfully.");
        }
        catch (Exception ex)
        {
            Plugin.LogError($"Failed to load reskin profile: {ex.Message}. Creating a new default profile.");
            currentProfile = new Profile(); // Fallback to a default profile on error
        }
    }

    public static void SaveProfile()
    {
        try
        {
            // Convert the live profile into its serializable representation
            var serializableProfile = new SerializableProfile
            {
                // Sticks
                StickAttackerBluePersonalRef = CreateReferenceFromEntry(currentProfile.stickAttackerBluePersonal),
                StickAttackerRedPersonalRef = CreateReferenceFromEntry(currentProfile.stickAttackerRedPersonal),
                StickAttackerBlueRef = CreateReferenceFromEntry(currentProfile.stickAttackerBlue),
                StickAttackerRedRef = CreateReferenceFromEntry(currentProfile.stickAttackerRed),
                StickGoalieBluePersonalRef = CreateReferenceFromEntry(currentProfile.stickGoalieBluePersonal),
                StickGoalieRedPersonalRef = CreateReferenceFromEntry(currentProfile.stickGoalieRedPersonal),
                StickGoalieBlueRef = CreateReferenceFromEntry(currentProfile.stickGoalieBlue),
                StickGoalieRedRef = CreateReferenceFromEntry(currentProfile.stickGoalieRed),

                // Jerseys
                BlueSkaterTorsoRef = CreateReferenceFromEntry(currentProfile.blueSkaterTorso),
                BlueSkaterGroinRef = CreateReferenceFromEntry(currentProfile.blueSkaterGroin),
                BlueGoalieTorsoRef = CreateReferenceFromEntry(currentProfile.blueGoalieTorso),
                BlueGoalieGroinRef = CreateReferenceFromEntry(currentProfile.blueGoalieGroin),
                RedSkaterTorsoRef = CreateReferenceFromEntry(currentProfile.redSkaterTorso),
                RedSkaterGroinRef = CreateReferenceFromEntry(currentProfile.redSkaterGroin),
                RedGoalieTorsoRef = CreateReferenceFromEntry(currentProfile.redGoalieTorso),
                RedGoalieGroinRef = CreateReferenceFromEntry(currentProfile.redGoalieGroin),
                
                // Puck
                PuckRef = CreateReferenceFromEntry(currentProfile.puck),

                // Arena
                CrowdEnabled = currentProfile.crowdEnabled,
                HangarEnabled = currentProfile.hangarEnabled,
                ScoreboardEnabled = currentProfile.scoreboardEnabled,
                GlassEnabled = currentProfile.glassEnabled,
                IceRef = CreateReferenceFromEntry(currentProfile.ice),
                IceSmoothness = currentProfile.iceSmoothness,
                BoardsBorderTopColor = new SerializableColor(currentProfile.boardsBorderTopColor),
                BoardsMiddleColor = new SerializableColor(currentProfile.boardsMiddleColor),
                BoardsBorderBottomColor = new SerializableColor(currentProfile.boardsBorderBottomColor),
                GlassSmoothness = currentProfile.glassSmoothness,
                PillarsColor = new SerializableColor(currentProfile.pillarsColor),
                SpectatorDensity = currentProfile.spectatorDensity,
                NetRef = CreateReferenceFromEntry(currentProfile.net),
                
                // Skybox
                SkyboxAtmosphereThickness = currentProfile.skyboxAtmosphereThickness,
                SkyboxExposure = currentProfile.skyboxExposure,
                SkyboxSunDisk = currentProfile.skyboxSunDisk,
                SkyboxSunSize = currentProfile.skyboxSunSize,
                SkyboxSunSizeConvergence = currentProfile.skyboxSunSizeConvergence,
                SkyboxGroundColor = new SerializableColor(currentProfile.skyboxGroundColor),
                SkyboxSkyTint = new SerializableColor(currentProfile.skyboxSkyTint)
            };

            string json = JsonConvert.SerializeObject(serializableProfile, Formatting.Indented);
            File.WriteAllText(ProfilePath, json);
            Plugin.LogDebug($"Reskin profile saved to: {ProfilePath}");
        }
        catch (Exception ex)
        {
            Plugin.LogError($"Failed to save reskin profile: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Applies all settings from the CurrentProfile to the game via the SwapperManager.
    /// Call this after loading a profile or making a change.
    /// </summary>
    public static void LoadTexturesForActiveReskins()
    {
        // 1. Get a list of all reskins that are currently active in the profile.
        var activeReskins = GetAllActiveReskinEntries();

        // 2. Tell the TextureManager to unload anything not on this list.
        TextureManager.UnloadUnusedTextures(activeReskins);
        
        // This function centralizes applying the loaded settings
        // SwapperManager.OnPersonalStickChanged();
        // SwapperManager.OnBlueTeamStickChanged();
        // SwapperManager.OnRedTeamStickChanged();
        // IceSwapper.SetIceTexture();
        // PuckSwapper.SetAllPucksTextures();
        // Add calls for other swappers here...
        
        Plugin.Log($"Loading active reskins textures to memory...");
        foreach (ReskinRegistry.ReskinEntry reskinEntry in activeReskins)
        {
            TextureManager.GetTexture(reskinEntry);
        }

        PuckSwapper.GetBumpMapPathAndLoad();
        Plugin.Log($"Loaded active reskins textures to memory!");
    }
    
    /// <summary>
    /// A helper method to gather all non-null ReskinEntry objects from the current profile.
    /// </summary>
    /// <returns>A list of all active reskin entries.</returns>
    private static List<ReskinRegistry.ReskinEntry> GetAllActiveReskinEntries()
    {
        var activeList = new List<ReskinRegistry.ReskinEntry>();

        // Add each property from the profile to the list if it's not null
        if (currentProfile.stickAttackerBlue != null) activeList.Add(currentProfile.stickAttackerBlue);
        if (currentProfile.stickAttackerBluePersonal != null) activeList.Add(currentProfile.stickAttackerBluePersonal);
        if (currentProfile.stickAttackerRed != null) activeList.Add(currentProfile.stickAttackerRed);
        if (currentProfile.stickAttackerRedPersonal != null) activeList.Add(currentProfile.stickAttackerRedPersonal);
        if (currentProfile.stickGoalieBlue != null) activeList.Add(currentProfile.stickGoalieBlue);
        if (currentProfile.stickGoalieBluePersonal != null) activeList.Add(currentProfile.stickGoalieBluePersonal);
        if (currentProfile.stickGoalieRed != null) activeList.Add(currentProfile.stickGoalieRed);
        if (currentProfile.stickGoalieRedPersonal != null) activeList.Add(currentProfile.stickGoalieRedPersonal);
        if (currentProfile.ice != null) activeList.Add(currentProfile.ice);
        if (currentProfile.puck != null) activeList.Add(currentProfile.puck);
        if (currentProfile.net != null) activeList.Add(currentProfile.net);
        
        if (currentProfile.blueSkaterTorso != null) activeList.Add(currentProfile.blueSkaterTorso);
        if (currentProfile.blueSkaterGroin != null) activeList.Add(currentProfile.blueSkaterGroin);
        if (currentProfile.blueGoalieTorso != null) activeList.Add(currentProfile.blueGoalieTorso);
        if (currentProfile.blueGoalieGroin != null) activeList.Add(currentProfile.blueGoalieGroin);
        if (currentProfile.redSkaterTorso != null) activeList.Add(currentProfile.redSkaterTorso);
        if (currentProfile.redSkaterGroin != null) activeList.Add(currentProfile.redSkaterGroin);
        if (currentProfile.redGoalieTorso != null) activeList.Add(currentProfile.redGoalieTorso);
        if (currentProfile.redGoalieGroin != null) activeList.Add(currentProfile.redGoalieGroin);
        
        // Add other profile entries here as you expand

        return activeList;
    }
    
    /// <summary>
    /// Finds a live ReskinEntry from the registry based on a reference.
    /// Returns null if the pack or entry is no longer installed.
    /// </summary>
    private static ReskinRegistry.ReskinEntry FindEntryFromReference(ReskinReference reference)
    {
        if (reference == null || string.IsNullOrEmpty(reference.PackId))
        {
            return null; // No reference to find
        }

        // Find the pack with the matching UniqueId
        var pack = ReskinRegistry.reskinPacks.FirstOrDefault(p => p.UniqueId == reference.PackId);
        if (pack == null)
        {
            string missingPackInfo = $"Could not find reskin pack with ID '{reference.PackId}' for entry '{reference.EntryName}'. The pack may be uninstalled.";
            if (reference.WorkshopId == 0)
            {
                missingPackInfo += " This was a local pack.";
            }
            else
            {
                // You can now provide a direct link to the workshop item!
                missingPackInfo += $" Workshop Link: https://steamcommunity.com/sharedfiles/filedetails/?id={reference.WorkshopId}";
            }
            Plugin.LogWarning(missingPackInfo); return null; // Pack not found
        }

        // Find the entry within that pack with the matching name
        var entry = pack.Reskins.FirstOrDefault(e => e.Name == reference.EntryName);
        if (entry == null)
        {
            Plugin.LogWarning($"Could not find reskin entry named '{reference.EntryName}' in pack '{pack.Name}'. The entry may have been removed from the pack.");
            return null; // Entry not found in pack
        }

        return entry;
    }
    
    /// <summary>
    /// Creates a serializable ReskinReference from a live ReskinEntry.
    /// </summary>
    private static ReskinReference CreateReferenceFromEntry(ReskinRegistry.ReskinEntry entry)
    {
        if (entry?.ParentPack == null)
        {
            return null; // Cannot create a reference for a null entry or an entry without a parent pack
        }

        return new ReskinReference
        {
            PackId = entry.ParentPack.UniqueId,
            EntryName = entry.Name,
            WorkshopId = entry.ParentPack.WorkshopId,
        };
    }
    
    /// <summary>
    /// Resets only the skybox-related properties of the current profile
    /// to their default values without affecting other settings like sticks or ice.
    /// </summary>
    public static void ResetSkyboxToDefault()
    {
        Plugin.Log("Resetting skybox settings to their default values.");

        // Create a temporary new profile just to access its default values.
        var defaultValues = new Profile();
        
        // Apply the default skybox values to the current profile.
        currentProfile.skyboxAtmosphereThickness = defaultValues.skyboxAtmosphereThickness;
        currentProfile.skyboxExposure = defaultValues.skyboxExposure;
        currentProfile.skyboxSunDisk = defaultValues.skyboxSunDisk;
        currentProfile.skyboxSunSize = defaultValues.skyboxSunSize;
        currentProfile.skyboxSunSizeConvergence = defaultValues.skyboxSunSizeConvergence;
        currentProfile.skyboxGroundColor = defaultValues.skyboxGroundColor;
        currentProfile.skyboxSkyTint = defaultValues.skyboxSkyTint;

        // Save the profile with the updated skybox values.
        SaveProfile();

        // Apply the changes to the game world.
        swappers.SkyboxSwapper.UpdateSkybox();
    }
    
    public class Profile
    {
        // Sticks section
        public ReskinRegistry.ReskinEntry stickAttackerBlue;
        public ReskinRegistry.ReskinEntry stickAttackerBluePersonal;
        public ReskinRegistry.ReskinEntry stickAttackerRed;
        public ReskinRegistry.ReskinEntry stickAttackerRedPersonal;
        public ReskinRegistry.ReskinEntry stickGoalieBlue;
        public ReskinRegistry.ReskinEntry stickGoalieBluePersonal;
        public ReskinRegistry.ReskinEntry stickGoalieRed;
        public ReskinRegistry.ReskinEntry stickGoalieRedPersonal;
        
        // Jerseys
        public ReskinRegistry.ReskinEntry blueSkaterTorso;
        public ReskinRegistry.ReskinEntry blueSkaterGroin;
        public ReskinRegistry.ReskinEntry blueGoalieTorso;
        public ReskinRegistry.ReskinEntry blueGoalieGroin;
        public ReskinRegistry.ReskinEntry  redSkaterTorso;
        public ReskinRegistry.ReskinEntry  redSkaterGroin;
        public ReskinRegistry.ReskinEntry  redGoalieTorso;
        public ReskinRegistry.ReskinEntry  redGoalieGroin;
        
        // Puck section
        public ReskinRegistry.ReskinEntry puck;
        
        // Arena section
        public bool crowdEnabled = true;
        public bool hangarEnabled = true;
        public bool glassEnabled = true;
        public bool scoreboardEnabled = true;
        public ReskinRegistry.ReskinEntry ice;
        public float                      iceSmoothness = 0.8f;
        public Color boardsBorderTopColor    = new Color(0, 0.260123f, 1, 1);
        public Color boardsMiddleColor       = new Color(1, 1, 1, 1);
        public Color boardsBorderBottomColor = new Color(1, 0.868332f, 0, 1);
        public Color pillarsColor = new Color(0.7830189f, 0.7830189f, 0.7830189f, 1);
        public float glassSmoothness = 1f;
        public float spectatorDensity = 0.25f;
        public ReskinRegistry.ReskinEntry net;
        
        // Skybox section
        public float skyboxAtmosphereThickness = 1;
        public float skyboxExposure = 1.3f;
        public float skyboxSunDisk = 1;
        public float skyboxSunSize = 0.04f;
        public float skyboxSunSizeConvergence = 5;
        public Color skyboxGroundColor = new Color(0.369f, 0.349f, 0.341f, 1f);
        public Color skyboxSkyTint = new Color(0.5f, 0.5f, 0.5f, 1f);
    } 
    
    /// <summary>
    /// A lightweight, serializable reference to a specific reskin entry.
    /// </summary>
    [Serializable]
    private class ReskinReference
    {
        [JsonProperty("packId")]
        public string PackId { get; set; }

        [JsonProperty("entryName")]
        public string EntryName { get; set; }
        
        [JsonProperty("workshopId")]
        public ulong WorkshopId { get; set; }
    }
    
    /// <summary>
    /// The data structure that is actually saved to and loaded from the JSON file.
    /// </summary>
    [Serializable]
    private class SerializableProfile
    {
        // STICKS
        [JsonProperty("stickAttackerBlueRef")]
        public ReskinReference StickAttackerBlueRef { get; set; }
        [JsonProperty("stickAttackerBluePersonalRef")]
        public ReskinReference StickAttackerBluePersonalRef { get; set; }
        [JsonProperty("stickAttackerRedRef")]
        public ReskinReference StickAttackerRedRef { get; set; }
        [JsonProperty("stickAttackerRedPersonalRef")]
        public ReskinReference StickAttackerRedPersonalRef { get; set; }

        [JsonProperty("stickGoalieBlueRef")]
        public ReskinReference StickGoalieBlueRef { get; set; }
        [JsonProperty("stickGoalieBluePersonalRef")]
        public ReskinReference StickGoalieBluePersonalRef { get; set; }
        [JsonProperty("stickGoalieRedRef")]
        public ReskinReference StickGoalieRedRef { get; set; }
        [JsonProperty("stickGoalieRedPersonalRef")]
        public ReskinReference StickGoalieRedPersonalRef { get; set; }
        
        [JsonProperty("blueSkaterTorsoRef")]
        public ReskinReference BlueSkaterTorsoRef { get; set; }
        [JsonProperty("blueSkaterGroinRef")]
        public ReskinReference BlueSkaterGroinRef { get; set; }
        [JsonProperty("blueGoalieTorsoRef")]
        public ReskinReference BlueGoalieTorsoRef { get; set; }
        [JsonProperty("blueGoalieGroinRef")]
        public ReskinReference BlueGoalieGroinRef { get; set; }
        [JsonProperty("redSkaterTorsoRef")]
        public ReskinReference RedSkaterTorsoRef { get; set; }
        [JsonProperty("redSkaterGroinRef")]
        public ReskinReference RedSkaterGroinRef { get; set; }
        [JsonProperty("redGoalieTorsoRef")]
        public ReskinReference RedGoalieTorsoRef { get; set; }
        [JsonProperty("redGoalieGroinRef")]
        public ReskinReference RedGoalieGroinRef { get; set; }
        
        // ARENA
        [JsonProperty("crowdEnabled")]
        public bool? CrowdEnabled { get; set; }
        [JsonProperty("scoreboardEnabled")]
        public bool? ScoreboardEnabled { get; set; }
        [JsonProperty("glassEnabled")]
        public bool? GlassEnabled { get; set; }
        [JsonProperty("hangarEnabled")]
        public bool? HangarEnabled { get; set; }
        
        [JsonProperty("iceRef")]
        public ReskinReference IceRef { get; set; }
        [JsonProperty("iceSmoothness")]
        public float? IceSmoothness { get; set; }
        [JsonProperty("glassSmoothness")]
        public float? GlassSmoothness { get; set; }
        [JsonProperty("pillarsColor")]
        public SerializableColor PillarsColor { get; set; }
        [JsonProperty("spectatorDensity")]
        public float? SpectatorDensity { get; set; }
        
        
        [JsonProperty("boardsBorderTopColor")]
        public SerializableColor BoardsBorderTopColor { get; set; }
        [JsonProperty("boardsMiddleColor")]
        public SerializableColor BoardsMiddleColor { get; set; }
        [JsonProperty("boardsBorderBottomColor")]
        public SerializableColor BoardsBorderBottomColor { get; set; }
        [JsonProperty("netRef")]
        public ReskinReference NetRef { get; set; }

        // PUCKS
        [JsonProperty("puckRef")]
        public ReskinReference PuckRef { get; set; }
        
        // SKYBOX
        [JsonProperty("skyboxAtmosphereThickness")]
        public float? SkyboxAtmosphereThickness { get; set; }
        [JsonProperty("skyboxExposure")]
        public float? SkyboxExposure { get; set; }
        [JsonProperty("skyboxSunDisk")]
        public float? SkyboxSunDisk { get; set; }
        [JsonProperty("skyboxSunSize")]
        public float? SkyboxSunSize { get; set; }
        [JsonProperty("skyboxSunSizeConvergence")]
        public float? SkyboxSunSizeConvergence { get; set; }
        [JsonProperty("skyboxGroundColor")]
        public SerializableColor SkyboxGroundColor { get; set; }
        [JsonProperty("skyboxSkyTint")]
        public SerializableColor SkyboxSkyTint { get; set; }
    }
}

/// <summary>
/// A simple, serializable representation of a UnityEngine.Color.
/// This avoids the self-referencing loop issue with Newtonsoft.Json.
/// </summary>
[Serializable]
public class SerializableColor
{
    public float r, g, b, a;

    // A default constructor for deserialization
    public SerializableColor() { }

    // A constructor to easily convert from a Unity Color
    public SerializableColor(Color color)
    {
        r = color.r;
        g = color.g;
        b = color.b;
        a = color.a;
    }

    // An explicit conversion operator to easily convert back to a Unity Color
    public static explicit operator Color(SerializableColor sc)
    {
        return new Color(sc.r, sc.g, sc.b, sc.a);
    }
}