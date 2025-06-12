using System;
using System.IO;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using Object = System.Object;

namespace ToasterReskinLoader;

public static class PatchClientChat
{
    [HarmonyPatch(typeof(UIChat), nameof(UIChat.Client_SendClientChatMessage))]
    class PatchUIChatClientSendClientChatMessage
    {
        [HarmonyPrefix]
        static bool Prefix(UIChat __instance, string message, bool useTeamChat)
        {
            Plugin.Log($"Patch: UIChat.Client_SendClientChatMessage (Prefix) was called.");
            string[] messageParts = message.Split(' ');
            
            // if (messageParts[0].Equals("/reskin", StringComparison.OrdinalIgnoreCase))
            // {
            //     __instance.AddChatMessage($"Reskin here");
            //     Stick stick = PlayerManager.Instance.GetLocalPlayer().Stick;
            //     // TestStick.SetStickTexture(stick);
            //     __instance.AddChatMessage($"Reskin there");
            //     
            //     return false;
            // }
            //
            // if (messageParts[0].Equals("/levell", StringComparison.OrdinalIgnoreCase))
            // {
            //     TestArena.ChangeLevel();
            //     __instance.AddChatMessage($"Level swap");
            //     return false;
            // }

            if (messageParts[0].Equals($"/hierarchy"))
            {
                PrintHierarchyToFile("hierarchy.txt");
                __instance.AddChatMessage($"Wrote <b>hierarchy.txt</b> to your Puck directory.");
                return false;
            }
            
            return true;
        }
    }
    
    private static void PrintHierarchyToFile(string filePath)
    {
        Debug.Log($"Writing hierarchy of the current scene to file: {filePath}");

        // Create or overwrite the file
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {

            // Find all GameObjects in the scene
            Object[] allObjects = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));

            // Iterate through all objects
            foreach (var obj in allObjects)
            {
                // Try to cast the object to a GameObject
                GameObject gameObject = obj as GameObject;
                if (gameObject == null || gameObject.transform == null)
                {
                    continue;
                }

                // If it's a root object, write its hierarchy
                if (gameObject.transform.parent == null)
                {
                    WriteGameObjectHierarchyToFile(gameObject, 0, writer);
                }
            }
        }

        Debug.Log("Hierarchy successfully written to file.");
    }

    private static void WriteGameObjectHierarchyToFile(GameObject obj, int depth, StreamWriter writer)
    {
        if (obj == null || obj.transform == null)
        {
            return;
        }

        // Exclude objects with "Spectator" in their name (except "Spectator Manager")
        if (obj.name.Contains("Spectator") && !obj.name.Contains("Manager") && !obj.name.Contains("Camera") &&
            !obj.name.Contains("Controller"))
        {
            return;
        }

        // Exclude objects if their parent contains "Spectator" (except "Spectator Manager")
        Transform parent = obj.transform.parent;
        while (parent != null)
        {
            if (parent.name.Contains("Spectator") && !parent.name.Contains("Manager") &&
                !parent.name.Contains("Controller") && !parent.name.Contains("Camera"))
            {
                return;
            }

            parent = parent.parent;
        }

        // Get all components attached to the GameObject
        Component[] components = obj.GetComponents<Component>();

// Collect the names of the component types
        string componentTypes = string.Join(", ", components.Select(c => c.GetType().Name));

// Write the GameObject information with its components
        writer.WriteLine($"{new string('-', depth * 2)}{obj.name} " +
                         $"[Active: {obj.activeSelf}, Layer: {obj.layer}" +
                         $"Position: {obj.transform.position}, Components: {componentTypes}]");

        // Recursively write all children
        foreach (var childObj in obj.transform)
        {
            if (childObj is Transform childTransform && childTransform.gameObject != null)
            {
                WriteGameObjectHierarchyToFile(childTransform.gameObject, depth + 1, writer);
            }
        }
    }
}