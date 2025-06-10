using System;
using HarmonyLib;

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
            
            if (messageParts[0].Equals("/reskin", StringComparison.OrdinalIgnoreCase))
            {
                __instance.AddChatMessage($"Reskin here");
                Stick stick = PlayerManager.Instance.GetLocalPlayer().Stick;
                // TestStick.SetStickTexture(stick);
                __instance.AddChatMessage($"Reskin there");
                
                return false;
            }

            if (messageParts[0].Equals("/levell", StringComparison.OrdinalIgnoreCase))
            {
                TestArena.ChangeLevel();
                __instance.AddChatMessage($"Level swap");
                return false;
            }
            
            return true;
        }
    }
}