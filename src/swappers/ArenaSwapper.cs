using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace ToasterReskinLoader.swappers;

public static class ArenaSwapper
{
    private static List<GameObject> hiddenOutdoorObjects = new List<GameObject>();
    private static List<GameObject> hiddenCrowdObjects = new List<GameObject>();
    public static void UpdateCrowdState()
    {
        if (ReskinProfileManager.profile.crowdEnabled)
        {
            ShowCrowdObjects();
        }
        else
        {
            HideCrowdObjects();
        }
    }

    public static void UpdateHangarState()
    {
        if (ReskinProfileManager.profile.hangarEnabled)
        {
            ShowOutdoorObjects();
        }
        else
        {
            HideOutdoorObjects();
        } 
    }
    
    private static string[] namesOfOutdoorObjects = new[]
    {
        "hangar",
        "Rafter",
        "Rafter Edge",
        "scoreboard",
        "Scoreboard",
        // "Glass",
        "Doors",
        // "Light Row",
        // "Light Row.001",
        // "Light Row.002",
        // "Light Row.003",
        "Small Roof Rafters",
        "Small Side Rafters",
        "Window Borders",
        "Windows",
        // "Pillars",
        "Side Rafter Ties",
        "Hangar"
    };

    private static string[] namesOfCrowdObjects = new[]
    {
        "Spectator",
        "Spectator(Clone)",
        "spectator_booth"
    };

    private static void HideCrowdObjects()
    {
        // Find all GameObjects in the scene
        UnityEngine.Object[] allObjects = UnityEngine.Object.FindObjectsByType(typeof(GameObject), FindObjectsSortMode.None);

        // Iterate through all objects
        foreach (Object obj in allObjects)
        {
            // Try to cast the object to a GameObject
            GameObject gameObject = (GameObject) obj;
            if (gameObject == null || gameObject.transform == null)
            {
                continue;
            }

            if (namesOfCrowdObjects.Contains(gameObject.name))
            {
                hiddenCrowdObjects.Add(gameObject);
                gameObject.SetActive(false);
            }
        }
    }

    private static void ShowCrowdObjects()
    {
        foreach (GameObject obj in hiddenCrowdObjects)
        {
            obj.SetActive(true);
        }
        hiddenCrowdObjects.Clear();
    }
    
    public static void HideOutdoorObjects()
    {
        // Find all GameObjects in the scene
        UnityEngine.Object[] allObjects = UnityEngine.Object.FindObjectsByType(typeof(GameObject), FindObjectsSortMode.None);

        // Iterate through all objects
        foreach (Object obj in allObjects)
        {
            // Try to cast the object to a GameObject
            GameObject gameObject = (GameObject) obj;
            if (gameObject == null || gameObject.transform == null)
            {
                continue;
            }

            if (namesOfOutdoorObjects.Contains(gameObject.name))
            {
                hiddenOutdoorObjects.Add(gameObject);
                gameObject.SetActive(false);

                MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    mr.enabled = false;
                }
            }
        }
    }
    
    public static void ShowOutdoorObjects()
    {
        foreach (GameObject obj in hiddenOutdoorObjects)
        {
            obj.SetActive(true);
            MeshRenderer mr = obj.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                mr.enabled = true;
            }
        }
        hiddenOutdoorObjects.Clear();
    }

    [HarmonyPatch(typeof(SpectatorManager), nameof(SpectatorManager.SpawnSpectators))]
    public static class SpectatorManagerSpawnSpectators
    {
        [HarmonyPostfix]
        public static void Postfix(SpectatorManager __instance)
        {
            UpdateCrowdState();
        }
    }
}