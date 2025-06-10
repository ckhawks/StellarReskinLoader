// ReskinRegistry.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace ToasterReskinLoader;

public static class ReskinRegistry
{
    public static List<ReskinPack> reskinPacks = new List<ReskinPack>();
    public readonly static List<string> ReskinTypes =
        new List<string>{"stick_attacker", "stick_goalie", "netting", "puck", "rink_ice", "arena"};

    public static void LoadPacks()
    {
        Plugin.Log($"Loading packs...");
        // Assembly.GetExecutingAssembly().Location
        // This is done because for local development I need it to search a different spot
        string execPath = Assembly.GetExecutingAssembly().Location;
        Plugin.Log($"execPath: {execPath}");
        if (execPath.Contains($"common"))
        {
            execPath = @"C:\Program Files (x86)\Steam\steamapps\workshop\content\2994020\3493628417\ToasterCrispyShadows.dll";
        }
        string workshopModsRoot = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(execPath)!, ".."));
        
        Plugin.Log($"workshopModsRoot: {workshopModsRoot}");
        
        Plugin.Log($"Application.dataPath: {Application.dataPath}");

        string gameRootFolder = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string localReskinFolder = Path.Combine(gameRootFolder, "reskinpacks");
        
        if (!Directory.Exists(localReskinFolder))
        {
            Plugin.LogError($"Local reskin packs folder not found: {localReskinFolder}, creating it...");
            Directory.CreateDirectory(localReskinFolder);
        }

        // for each pack in the workshop mods directory
        Plugin.Log($"Looking for reskin packs at {workshopModsRoot}...");
        foreach (var dir in Directory.GetDirectories(workshopModsRoot))
        {
            LoadPackDirectory(dir);
        }
        
        Plugin.Log($"Looking for reskin packs at {localReskinFolder}...");
        foreach (var dir in Directory.GetDirectories(localReskinFolder))
        {
            LoadPackDirectory(dir);
        }
        Plugin.Log($"Loaded {reskinPacks.Count} packs");
    }

    public static void LoadPackDirectory(string dir)
    {
        string manifestPath = Path.Combine(dir, "reskinpack.json");
        if (!File.Exists(manifestPath))
        {
            Plugin.Log($" - Missing reskinpack.json in {dir}");
            return;
        }

        try
        {
            string json = File.ReadAllText(manifestPath);
            var pack = JsonConvert.DeserializeObject<ReskinPack>(json);
            if (pack != null)
            {
                // make paths absolute
                foreach (var skin in pack.Reskins)
                {
                    if (!ReskinTypes.Contains(skin.Type))
                    {
                        Plugin.Log($"   - Unknown reskin type: {skin.Type}");
                        continue;
                    };
                    skin.Path = Path.GetFullPath(Path.Combine(dir, skin.Path));
                }
                reskinPacks.Add(pack);
                Plugin.Log($" - Loaded pack: {pack.Name} v{pack.Version} with {pack.Reskins.Count} reskins.");
            }
        }
        catch (Exception ex)
        {
            Plugin.LogError($" - Failed to load reskinpack.json in {dir}: {ex}");
        }
    }
    
    public static List<ReskinEntry> GetReskinEntriesByType(string reskinType)
    {
        if (string.IsNullOrEmpty(reskinType))
            return new List<ReskinEntry>();

        return reskinPacks
            .Where(pack => pack.Reskins != null)
            .SelectMany(pack => pack.Reskins)
            .Where(entry => string.Equals(entry.Type, reskinType,
                StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
    
    public class ReskinPack
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("unique-id")]
        public string UniqueId { get; set; }

        [JsonProperty("pack-version")]
        public string PackVersion { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("reskins")]
        public List<ReskinEntry> Reskins { get; set; } = new List<ReskinEntry>();
    }

    public class ReskinEntry
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        // this is relative in JSON; we'll make it absolute in LoadPacks()
        [JsonProperty("path")]
        public string Path { get; set; }
    }
}