using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ToasterReskinLoader;

public static class ChangingRoomHelper
{
    private static ChangingRoomStick changingRoomStick;
    private static ChangingRoomManager changingRoomManager;
    private static ChangingRoomPlayer changingRoomPlayer;

    public static bool IsInMainMenu()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "changing_room";
    }
    
    public static void ShowStickSkin(PlayerRole role, ReskinRegistry.ReskinEntry reskinEntry)
    {
        
    }
    
    public static void ShowStick()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "changing_room") return;
        Scan();
        
        if (changingRoomManager != null)
        {
            changingRoomManager.Client_MoveCameraToAppearanceDefaultPosition();
        }

        // TODO temporarily disabled this until I get it showing skins
        // if (changingRoomStick != null)
        // {
        //     changingRoomStick.Client_MoveStickToAppearanceStickPosition();
        //     changingRoomStick.RotateWithMouse = true;
        // }
    }

    public static void ShowBody()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "changing_room") return;
        Scan();
        try
        {
            if (changingRoomManager != null)
            {
                changingRoomManager.Client_MoveCameraToAppearanceJerseyPosition();
            }
            
            Plugin.Log($"halfway");

            if (changingRoomPlayer != null)
            {
                Plugin.Log($"test");
                Plugin.Log($"changing room player {changingRoomPlayer.name}");
                changingRoomPlayer.RotateWithMouse = true;
            }
        }
        catch (Exception e)
        {
            Plugin.LogError($"Error ShowBody(): {e.Message}");
        }
        
    }

    public static void ShowBaseFocus()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "changing_room") return;
        Scan();
        
        if (changingRoomManager != null)
        {
            changingRoomManager.Client_MoveCameraToAppearanceDefaultPosition();
        }
        
        if (changingRoomStick != null)
        {
            changingRoomStick.Client_MoveStickToDefaultPosition();
            changingRoomStick.RotateWithMouse = false;
        }
    }

    public static void Unfocus()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "changing_room") return;
        Scan();
        
        if (changingRoomManager != null)
        {
            changingRoomManager.Client_MoveCameraToDefaultPosition();
        }
        
        if (changingRoomStick != null)
        {
            changingRoomStick.Client_MoveStickToDefaultPosition();
            changingRoomStick.RotateWithMouse = false;
        }
    }
    
    public static void Scan()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "changing_room") return;
        if (changingRoomStick == null)
        {
            ChangingRoomStick[] crss = Object.FindObjectsByType<ChangingRoomStick>(FindObjectsSortMode.None);
            if (crss.Length == 0)
            {
                Plugin.LogError($"Could not locate changing room stick.");
            }
            else
            {
                changingRoomStick = crss[0];
            }
        }

        if (changingRoomManager == null)
        {
            ChangingRoomManager[] crms = Object.FindObjectsByType<ChangingRoomManager>(FindObjectsSortMode.None);
            if (crms.Length == 0)
            {
                Plugin.LogError($"Could not locate changing room manager.");
            }
            else
            {
                changingRoomManager = crms[0];
            }
        }
        
        if (changingRoomPlayer == null)
        {
            ChangingRoomPlayer[] crps = Object.FindObjectsByType<ChangingRoomPlayer>(FindObjectsSortMode.None);
            if (crps.Length == 0)
            {
                Plugin.LogError($"Could not locate changing room player.");
            }
            else
            {
                changingRoomPlayer = crps[0];
            }
        }
    }
}