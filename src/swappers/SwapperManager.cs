using System.Collections.Generic;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace ToasterReskinLoader.swappers;

public static class SwapperManager
{
    // TODO this class should call the various swappers to trigger them to set textures/reskins whenever game events happen.
    // TODO we also need like a SettingsManager or something that handles the state of which reskins are selected, and handles saving/loading profiles
    
    // TODO all selected skins' Texture2D's should be loaded into memory.
    // TODO when someone selects a new skin we can load that one into memory
    
    // TODO we need to save each player's vanilla setup (stick, jersey) before applying anything
    
    // Intended to be called whenever we need to update the local player's stick
    public static void OnPersonalStickChanged()
    {
        SetStickReskinForPlayer(PlayerManager.Instance.GetLocalPlayer());
    }
    
    public static void OnBlueTeamStickChanged()
    {
        List<Player> bluePlayers = PlayerManager.Instance.GetSpawnedPlayersByTeam(PlayerTeam.Blue);
        foreach (Player bluePlayer in bluePlayers)
        {
            if (!bluePlayer.IsLocalPlayer)
                SetStickReskinForPlayer(bluePlayer);
        }
    }
    
    public static void OnRedTeamStickChanged()
    {
        List<Player> redPlayers = PlayerManager.Instance.GetSpawnedPlayersByTeam(PlayerTeam.Red);
        foreach (Player redPlayer in redPlayers)
        {
            if (!redPlayer.IsLocalPlayer)
                SetStickReskinForPlayer(redPlayer);
        }
    }

    public static void SetStickReskinForPlayer(Player player)
    {
        // If we are missing a part of the player, player body, or stick
        if (player == null || player.PlayerBody == null || player.Stick == null)
            return;
        
        if (player.Team.Value == PlayerTeam.Blue)
        {
            if (player.IsLocalPlayer)
            {
                StickSwapper.SetStickTexture(player.Stick, ReskinProfileManager.profile.stickAttackerBluePersonal);
                return;
            }
            
            if (ReskinProfileManager.profile.stickAttackerBlue != null)
            {
                StickSwapper.SetStickTexture(player.Stick, ReskinProfileManager.profile.stickAttackerBlue);
            }
            else
            {
                // TODO reset stick to their own selected stick
            }

            return;
        }
        
        if (player.Team.Value == PlayerTeam.Red)
        {
            if (player.IsLocalPlayer)
            {
                StickSwapper.SetStickTexture(player.Stick, ReskinProfileManager.profile.stickAttackerRedPersonal);
                return;
            }
            
            if (ReskinProfileManager.profile.stickAttackerRed != null)
            {
                StickSwapper.SetStickTexture(player.Stick, ReskinProfileManager.profile.stickAttackerRed);
            }
            else
            {
                // TODO reset stick to their own selected stick
            }

            return;
        }
    }

    [HarmonyPatch(typeof(Stick), "OnNetworkPostSpawn")]
    public static class StickOnNetworkPostSpawn
    {
        [HarmonyPostfix]
        public static void Postfix(Stick __instance)
        {
            Plugin.Log($"Stick.OnNetworkPostSpawn");
            Player player = __instance.PlayerBody.Player;
            
            SetStickReskinForPlayer(player);
        }
    }
    
    public static void Setup()
    {
        global::UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public static void Destroy()
    {
        global::UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Plugin.Log($"OnSceneLoaded: {scene.name}");
        IceSwapper.SetIceTexture();
        ArenaSwapper.UpdateCrowdState();
        ArenaSwapper.UpdateHangarState();
    }
}