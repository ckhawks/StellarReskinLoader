using System;
using System.Reflection;
using HarmonyLib;
using ToasterReskinLoader.swappers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ToasterReskinLoader;

public static class ChangingRoomHelper
{
    private static ChangingRoomStick changingRoomStickAttacker;
    private static ChangingRoomStick changingRoomStickGoalie;
    private static ChangingRoomManager changingRoomManager;
    private static ChangingRoomPlayer changingRoomPlayer;

    public static bool IsInMainMenu()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "changing_room";
    }
    
    public static void ShowStick()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "changing_room") return;
        Scan();
        
        if (changingRoomManager != null)
        {
            changingRoomManager.Client_MoveCameraToAppearanceDefaultPosition();
        }

        if (changingRoomPlayer != null)
        {
            changingRoomPlayer.RotateWithMouse = false;
            changingRoomPlayer.Client_MovePlayerToDefaultPosition();
        }
        
        // TODO temporarily disabled this until I get it showing skins
        if (changingRoomStickAttacker != null)
        {
            changingRoomStickAttacker.Client_MoveStickToAppearanceStickPosition();
            changingRoomStickAttacker.RotateWithMouse = true;
        }

        if (changingRoomStickGoalie != null)
        {
            changingRoomStickGoalie.Client_MoveStickToAppearanceStickPosition();
            changingRoomStickGoalie.RotateWithMouse = true;
        }
    }

    public static void ShowBody()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "changing_room") return;
        Scan();
        try
        {
            if (changingRoomManager != null)
            {
                changingRoomManager.Client_MoveCameraToAppearanceDefaultPosition();
            }

            if (changingRoomPlayer != null)
            {
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
        
        if (changingRoomPlayer != null)
        {
            changingRoomPlayer.RotateWithMouse = false;
            changingRoomPlayer.Client_MovePlayerToDefaultPosition();
        }
        
        if (changingRoomStickAttacker != null)
        {
            changingRoomStickAttacker.Client_MoveStickToDefaultPosition();
            changingRoomStickAttacker.RotateWithMouse = false;
        }

        if (changingRoomStickGoalie != null)
        {
            changingRoomStickGoalie.Client_MoveStickToDefaultPosition();
            changingRoomStickGoalie.RotateWithMouse = false;
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
        
        if (changingRoomPlayer != null)
        {
            changingRoomPlayer.RotateWithMouse = false;
            changingRoomPlayer.Client_MovePlayerToDefaultPosition();
        }
        
        if (changingRoomStickAttacker != null)
        {
            changingRoomStickAttacker.Client_MoveStickToDefaultPosition();
            changingRoomStickAttacker.RotateWithMouse = false;
        }

        if (changingRoomStickGoalie != null)
        {
            changingRoomStickGoalie.Client_MoveStickToDefaultPosition();
            changingRoomStickGoalie.RotateWithMouse = false;
        }
    }
    
    public static void Scan()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "changing_room") return;
        if (changingRoomStickGoalie == null || changingRoomStickAttacker == null)
        {
            ChangingRoomStick[] crss = Object.FindObjectsByType<ChangingRoomStick>(FindObjectsSortMode.None);
            Plugin.Log($"crss length: {crss.Length}");
            if (crss.Length == 0)
            {
                Plugin.LogError($"Could not locate changing room stick.");
            }
            else
            {
                foreach (ChangingRoomStick crs in crss)
                {
                    if (crs.Role == PlayerRole.Goalie)
                    {
                        changingRoomStickGoalie = crs;
                    } else if (crs.Role == PlayerRole.Attacker)
                    {
                        changingRoomStickAttacker = crs;
                    }
                }
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

    // TODO one small problem is that when we are in the vanilla  PLAYER screen choosing sticks this is being called and 
    [HarmonyPatch(typeof(ChangingRoomStick), nameof(ChangingRoomStick.UpdateStickMesh))]
    public static class ChangingRoomStickUpdateStickMesh
    {
        [HarmonyPrefix]
        public static bool Prefix(ChangingRoomStick __instance)
        {
            if (__instance.Role == PlayerRole.Attacker)
            {
                changingRoomStickAttacker = __instance;
                // Plugin.Log($"Set attacker stick!");
            }

            if (__instance.Role == PlayerRole.Goalie)
            {
                changingRoomStickGoalie = __instance;
                // Plugin.Log($"Set goalie stick!");
            }
            
            // this.StickMesh.SetSkin(this.Team, MonoBehaviourSingleton<SettingsManager>.Instance.GetStickSkin(this.Team, this.Role));
            __instance.StickMesh.SetShaftTape(MonoBehaviourSingleton<SettingsManager>.Instance.GetStickShaftSkin(__instance.Team, __instance.Role));
            __instance.StickMesh.SetBladeTape(MonoBehaviourSingleton<SettingsManager>.Instance.GetStickBladeSkin(__instance.Team, __instance.Role));

            switch (__instance.Team)
            {
                case PlayerTeam.Blue:
                    switch (__instance.Role)
                    {
                        case PlayerRole.Attacker:
                            if (ReskinProfileManager.currentProfile.stickAttackerBluePersonal != null)
                            {
                                StickSwapper.SetStickMeshTexture(__instance.StickMesh, ReskinProfileManager.currentProfile.stickAttackerBluePersonal, PlayerRole.Attacker);
                            }
                            break;
                        case PlayerRole.Goalie: 
                            if (ReskinProfileManager.currentProfile.stickGoalieBluePersonal != null)
                            {
                                StickSwapper.SetStickMeshTexture(__instance.StickMesh, ReskinProfileManager.currentProfile.stickGoalieBluePersonal, PlayerRole.Goalie);
                            }
                            break;
                        default:
                            __instance.StickMesh.SetSkin(__instance.Team, MonoBehaviourSingleton<SettingsManager>.Instance.GetStickSkin(__instance.Team, __instance.Role));
                            break;
                    }
                    break;
                case PlayerTeam.Red:
                    switch (__instance.Role)
                    {
                        case PlayerRole.Attacker:
                            if (ReskinProfileManager.currentProfile.stickAttackerRedPersonal != null)
                            {
                                StickSwapper.SetStickMeshTexture(__instance.StickMesh, ReskinProfileManager.currentProfile.stickAttackerRedPersonal, PlayerRole.Attacker);
                            }
                            break;
                        case PlayerRole.Goalie: 
                            if (ReskinProfileManager.currentProfile.stickGoalieRedPersonal != null)
                            {
                                StickSwapper.SetStickMeshTexture(__instance.StickMesh, ReskinProfileManager.currentProfile.stickGoalieRedPersonal, PlayerRole.Goalie);
                            }
                            break;
                        default:
                            __instance.StickMesh.SetSkin(__instance.Team, MonoBehaviourSingleton<SettingsManager>.Instance.GetStickSkin(__instance.Team, __instance.Role));
                            break;
                    }
                    break;
            }

            return false;
        }
    }

    public static void UpdateStickDisplayToReskin(ReskinRegistry.ReskinEntry reskinEntry, PlayerRole role)
    {
        if (role == PlayerRole.Attacker)
        {
            if (changingRoomStickAttacker != null)
            {
                if (reskinEntry == null || reskinEntry.Path == null)
                {
                    changingRoomStickAttacker.StickMesh.SetSkin(changingRoomStickAttacker.Team, MonoBehaviourSingleton<SettingsManager>.Instance.GetStickSkin(changingRoomStickAttacker.Team, changingRoomStickAttacker.Role));
                    changingRoomStickAttacker.StickMesh.SetShaftTape(MonoBehaviourSingleton<SettingsManager>.Instance.GetStickShaftSkin(changingRoomStickAttacker.Team, changingRoomStickAttacker.Role));
                    changingRoomStickAttacker.StickMesh.SetBladeTape(MonoBehaviourSingleton<SettingsManager>.Instance.GetStickBladeSkin(changingRoomStickAttacker.Team, changingRoomStickAttacker.Role));
                }
                else
                {
                    changingRoomStickAttacker.Show();
                    StickSwapper.SetStickMeshTexture(changingRoomStickAttacker.StickMesh, reskinEntry, role);
                }
            }

            if (changingRoomStickGoalie != null)
            {
                changingRoomStickGoalie.Hide();
            }
        } 
        else if (role == PlayerRole.Goalie)
        {
            if (changingRoomStickGoalie != null)
            {
                if (reskinEntry == null || reskinEntry.Path == null)
                {
                    changingRoomStickGoalie.StickMesh.SetSkin(changingRoomStickGoalie.Team, MonoBehaviourSingleton<SettingsManager>.Instance.GetStickSkin(changingRoomStickGoalie.Team, changingRoomStickGoalie.Role));
                    changingRoomStickGoalie.StickMesh.SetShaftTape(MonoBehaviourSingleton<SettingsManager>.Instance.GetStickShaftSkin(changingRoomStickGoalie.Team, changingRoomStickGoalie.Role));
                    changingRoomStickGoalie.StickMesh.SetBladeTape(MonoBehaviourSingleton<SettingsManager>.Instance.GetStickBladeSkin(changingRoomStickGoalie.Team, changingRoomStickGoalie.Role));
                }
                else
                {
                    changingRoomStickGoalie.Show();
                    StickSwapper.SetStickMeshTexture(changingRoomStickGoalie.StickMesh, reskinEntry, role);
                }
            }

            if (changingRoomStickAttacker != null)
            {
                changingRoomStickAttacker.Hide();
            }
        }
    }
    
    // TODO i might want to have this have a reskin argument so that we can display skins used on non-personal configuration fields
    public static void UpdateStickToPersonalSelected()
    {
        if (changingRoomManager != null)
        {
            if (changingRoomManager.Role == PlayerRole.Attacker)
            {
                if (changingRoomStickAttacker != null)
                {
                    changingRoomStickAttacker.Show();
                    changingRoomStickAttacker.UpdateStickMesh();
                }

                if (changingRoomStickGoalie != null)
                {
                    changingRoomStickGoalie.Hide();
                }
            }
            
            if (changingRoomManager.Role == PlayerRole.Goalie)
            {
                if (changingRoomStickGoalie != null)
                {
                    changingRoomStickGoalie.Show();
                    changingRoomStickGoalie.UpdateStickMesh();
                }

                if (changingRoomStickAttacker != null)
                {
                    changingRoomStickAttacker.Hide();
                }
            }
        } 
    }

    public static void UpdateBodyToPersonalSelected()
    {
        changingRoomPlayer.UpdatePlayerMesh();
    }

    static readonly FieldInfo _changingRoomPlayerRoleField = typeof(ChangingRoomPlayer)
        .GetField("role", 
            BindingFlags.Instance | BindingFlags.NonPublic);
    static readonly FieldInfo _changingRoomPlayerTeamField = typeof(ChangingRoomPlayer)
        .GetField("team", 
            BindingFlags.Instance | BindingFlags.NonPublic);
    
    // public static void UpdateBodyToShowTorso(ReskinRegistry.ReskinEntry torsoReskin, ReskinRegistry.ReskinEntry groinReskin, PlayerRole role, PlayerTeam team)
    // {
    //     if (reskinEntry == null || reskinEntry.Path == null)
    //     {
    //         changingRoomPlayer.PlayerMesh.SetJersey(team, MonoBehaviourSingleton<SettingsManager>.Instance.GetJerseySkin(team, role));
    //         changingRoomPlayer.PlayerMesh.SetRole(role);
    //         return;
    //     }
    //
    //     
    //     // 
    //     _changingRoomPlayerRoleField.SetValue(changingRoomPlayer, role);
    //     changingRoomPlayer.PlayerMesh.SetRole(role); // this shows/hides the leg pads and helmet cage
    //     _changingRoomPlayerTeamField.SetValue(changingRoomPlayer, team);
    //     // changing these two updates the player mesh, which would override stuff
    //     // changingRoomPlayer.Role = role;
    //     // changingRoomPlayer.Team = team;
    //     
    //     
    // }
}