using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace ToasterReskinLoader.swappers;

public static class JerseySwapper
{
    private static Texture originalBlueGroin;
    private static Texture originalRedGroin;
    private static Dictionary<ulong, Texture> originalBlueTorsoTextures = new Dictionary<ulong, Texture>(); // TODO clear this when leave server
    private static Dictionary<ulong, Texture> originalRedTorsoTextures = new Dictionary<ulong, Texture>();
    
    static readonly FieldInfo _meshRendererTexturerTorsoField = typeof(PlayerTorso)
        .GetField("meshRendererTexturer", 
            BindingFlags.Instance | BindingFlags.NonPublic);
    static readonly FieldInfo _meshRendererTexturerGroinField = typeof(PlayerGroin)
        .GetField("meshRendererTexturer", 
            BindingFlags.Instance | BindingFlags.NonPublic);
    static readonly FieldInfo _meshRendererField = typeof(MeshRendererTexturer)
        .GetField("meshRenderer", 
            BindingFlags.Instance | BindingFlags.NonPublic);
    public static void SetJerseyForPlayer(Player player)
    {
        Plugin.LogDebug($"Setting jersey for {player.Username.Value} {player.Team.Value} isReplay {player.IsReplay.Value}");
        PlayerTeam team = player.Team.Value;

        if (team is not (PlayerTeam.Blue or PlayerTeam.Red))
        {
            Plugin.LogDebug($"Player {player.Username.Value} is not on red or blue team, not swapping jersey.");
            return;
        }

        if (player == null || player.PlayerBody == null || player.PlayerBody.PlayerMesh == null ||
            player.PlayerBody.PlayerMesh.PlayerTorso == null)
        {
            Plugin.LogError($"Player {player.Username.Value} is missing body parts.");
            return;
        }
        
        MeshRendererTexturer torsoMeshRendererTexturer =
            (MeshRendererTexturer) _meshRendererTexturerTorsoField.GetValue(player.PlayerBody.PlayerMesh.PlayerTorso);
        MeshRendererTexturer groinMeshRendererTexturer =
            (MeshRendererTexturer) _meshRendererTexturerGroinField.GetValue(player.PlayerBody.PlayerMesh.PlayerGroin);
        
        // can call torsoMeshRendererTexturer.SetTexture(Texture);
        MeshRenderer torsoMeshRenderer = (MeshRenderer) _meshRendererField.GetValue(torsoMeshRendererTexturer);
        MeshRenderer groinMeshRenderer = (MeshRenderer) _meshRendererField.GetValue(groinMeshRendererTexturer);

        // SwapperUtils.FindTextureProperties(torsoMeshRenderer.material);
        // Plugin.Log($"Texture torso property: {SwapperUtils.FindTextureProperty(torsoMeshRenderer.material)}");
        // Plugin.Log($"Texture groin property: {SwapperUtils.FindTextureProperty(groinMeshRenderer.material)}");
        
        // player.PlayerBody.PlayerMesh.SetJersey(player.Team.Value, player.GetPlayerJerseySkin().ToString());
        
        if (team == PlayerTeam.Blue)
        {
            if (originalBlueGroin == null)
            {
                originalBlueGroin = groinMeshRenderer.material.mainTexture;
            }

            if (!originalBlueTorsoTextures.ContainsKey(player.OwnerClientId))
            {
                if (torsoMeshRenderer.material.mainTexture.name.Contains("blue_"))
                    originalBlueTorsoTextures.Add(player.OwnerClientId, torsoMeshRenderer.material.mainTexture);
            }

            if (player.Role.Value == PlayerRole.Goalie)
            {
                if (ReskinProfileManager.currentProfile.blueGoalieTorso == null ||
                    ReskinProfileManager.currentProfile.blueGoalieTorso.Path == null)
                {
                    //  Restore original blue torso we saved
                    if (originalBlueTorsoTextures.ContainsKey(player.OwnerClientId))
                        torsoMeshRenderer.material.mainTexture = originalBlueTorsoTextures[player.OwnerClientId];   
                }
                else
                {
                    torsoMeshRenderer.material.SetTexture("_BaseMap",
                        TextureManager.GetTexture(ReskinProfileManager.currentProfile.blueGoalieTorso));
                    torsoMeshRenderer.material.SetTexture("_MainTex",
                        TextureManager.GetTexture(ReskinProfileManager.currentProfile.blueGoalieTorso));
                }

                if (ReskinProfileManager.currentProfile.blueGoalieGroin == null ||
                    ReskinProfileManager.currentProfile.blueGoalieGroin.Path == null)
                {
                    //  Restore original blue groin
                    groinMeshRenderer.material.mainTexture = originalBlueGroin;
                }
                else
                {
                    //  Set groin to custom value
                    groinMeshRenderer.material.SetTexture("_MainTex",
                        TextureManager.GetTexture(ReskinProfileManager.currentProfile.blueGoalieGroin));
                    groinMeshRenderer.material.SetTexture("_BaseMap",
                        TextureManager.GetTexture(ReskinProfileManager.currentProfile.blueGoalieGroin));
                }
            }
            else
            {
                if (ReskinProfileManager.currentProfile.blueSkaterTorso == null ||
                    ReskinProfileManager.currentProfile.blueSkaterTorso.Path == null)
                {
                    //  Restore original blue torso we saved
                    if (originalBlueTorsoTextures.ContainsKey(player.OwnerClientId))
                        torsoMeshRenderer.material.mainTexture = originalBlueTorsoTextures[player.OwnerClientId];   
                }
                else
                {
                    Plugin.LogDebug(
                        $"Setting blue skater torso to {ReskinProfileManager.currentProfile.blueSkaterTorso.Name}");
                    torsoMeshRenderer.material.SetTexture("_MainTex",
                        TextureManager.GetTexture(ReskinProfileManager.currentProfile.blueSkaterTorso));
                    torsoMeshRenderer.material.SetTexture("_BaseMap",
                        TextureManager.GetTexture(ReskinProfileManager.currentProfile.blueSkaterTorso));
                }

                if (ReskinProfileManager.currentProfile.blueSkaterGroin == null ||
                    ReskinProfileManager.currentProfile.blueSkaterGroin.Path == null)
                {
                    //  Restore original blue groin
                    groinMeshRenderer.material.mainTexture = originalBlueGroin;
                }
                else
                {
                    //  Set groin to custom value
                    groinMeshRenderer.material.SetTexture("_MainTex",
                        TextureManager.GetTexture(ReskinProfileManager.currentProfile.blueSkaterGroin));
                    groinMeshRenderer.material.SetTexture("_BaseMap",
                        TextureManager.GetTexture(ReskinProfileManager.currentProfile.blueSkaterGroin));
                }
            }
        } else if (team == PlayerTeam.Red)
        {
            if (originalRedGroin == null)
            {
                originalRedGroin = groinMeshRenderer.material.mainTexture;
            }
            
            if (!originalRedTorsoTextures.ContainsKey(player.OwnerClientId))
            {
                if (torsoMeshRenderer.material.mainTexture.name.Contains("red_"))
                {
                    originalRedTorsoTextures.Add(player.OwnerClientId, torsoMeshRenderer.material.mainTexture);
                }
            }
            
            if (player.Role.Value == PlayerRole.Goalie)
            {
                if (ReskinProfileManager.currentProfile.redGoalieTorso == null ||
                    ReskinProfileManager.currentProfile.redGoalieTorso.Path == null)
                {
                    //  Restore original red torso we saved
                    if (originalRedTorsoTextures.ContainsKey(player.OwnerClientId))
                        torsoMeshRenderer.material.mainTexture = originalRedTorsoTextures[player.OwnerClientId];   
                }
                else
                {
                    torsoMeshRenderer.material.SetTexture("_MainTex",
                        TextureManager.GetTexture(ReskinProfileManager.currentProfile.redGoalieTorso));
                    torsoMeshRenderer.material.SetTexture("_BaseMap",
                        TextureManager.GetTexture(ReskinProfileManager.currentProfile.redGoalieTorso));
                }

                if (ReskinProfileManager.currentProfile.redGoalieGroin == null ||
                    ReskinProfileManager.currentProfile.redGoalieGroin.Path == null)
                {
                    //  Restore original red groin
                    groinMeshRenderer.material.mainTexture = originalRedGroin;
                }
                else
                {
                    //  Set groin to custom value
                    groinMeshRenderer.material.SetTexture("_MainTex",
                        TextureManager.GetTexture(ReskinProfileManager.currentProfile.redGoalieGroin));
                    groinMeshRenderer.material.SetTexture("_BaseMap",
                        TextureManager.GetTexture(ReskinProfileManager.currentProfile.redGoalieGroin));
                }
            }
            else
            {
                if (ReskinProfileManager.currentProfile.redSkaterTorso == null ||
                    ReskinProfileManager.currentProfile.redSkaterTorso.Path == null)
                {
                    //  Restore original red torso we saved
                    if (originalRedTorsoTextures.ContainsKey(player.OwnerClientId))
                    {
                        torsoMeshRenderer.material.mainTexture = originalRedTorsoTextures[player.OwnerClientId];  
                    }
                         
                }
                else
                {
                    torsoMeshRenderer.material.SetTexture("_MainTex",
                        TextureManager.GetTexture(ReskinProfileManager.currentProfile.redSkaterTorso));
                    torsoMeshRenderer.material.SetTexture("_BaseMap",
                        TextureManager.GetTexture(ReskinProfileManager.currentProfile.redSkaterTorso));
                }
                
                if (ReskinProfileManager.currentProfile.redSkaterGroin == null ||
                    ReskinProfileManager.currentProfile.redSkaterGroin.Path == null)
                {
                    //  Restore original red groin
                    groinMeshRenderer.material.mainTexture = originalRedGroin;
                }
                else
                {
                    //  Set groin to custom value
                    groinMeshRenderer.material.SetTexture("_MainTex",
                        TextureManager.GetTexture(ReskinProfileManager.currentProfile.redSkaterGroin));
                    groinMeshRenderer.material.SetTexture("_BaseMap",
                        TextureManager.GetTexture(ReskinProfileManager.currentProfile.redSkaterGroin));
                }
            }
        }
        Plugin.LogDebug($"Set jersey for {player.Username.Value.ToString()}");
    }

    // [HarmonyPatch(typeof(PlayerMesh), nameof(PlayerMesh.SetJersey))]
    // public static class PlayerMeshSetJersey
    // {
    //     [HarmonyPrefix]
    //     public static void Prefix(PlayerMesh __instance)
    //     {
    //         Player player = __instance.
    //     }
    // }
    
}