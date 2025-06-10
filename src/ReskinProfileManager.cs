// ReskinProfileManager.cs

using System;
using ToasterReskinLoader.swappers;

namespace ToasterReskinLoader;

public static class ReskinProfileManager
{
    // TODO make this inside of a dictionary or profile setting or something
    public static Profile profile = new Profile();
    // Make serialized JSON profiles which refer to skins by ReskinPack.UniqueId / ReskinEntry.Type / ReskinEntry.Name

    public static void SetSelectedReskinInCurrentProfile(ReskinRegistry.ReskinEntry reskinEntry, string type, string slot)
    {
        if (type == "attacker_stick")
        {
            if (slot == "blue_personal")
            {
                profile.stickAttackerBluePersonal = reskinEntry;
                SwapperManager.OnPersonalStickChanged();
            }
            
            if (slot == "red_personal")
            {
                profile.stickAttackerRedPersonal = reskinEntry;
                SwapperManager.OnPersonalStickChanged();
            }

            if (slot == "blue_team")
            {
                profile.stickAttackerBlue = reskinEntry;
                SwapperManager.OnBlueTeamStickChanged();
            }
            
            if (slot == "red_team") {
                profile.stickAttackerRed = reskinEntry;
                SwapperManager.OnRedTeamStickChanged();
            }
        } 
        else if (type == "rink_ice")
        {
            // We aren't using slot here
            profile.ice = reskinEntry;
            IceSwapper.SetIceTexture();
        }
        else if (type == "puck")
        {
            profile.puck = reskinEntry;
            PuckSwapper.SetAllPucksTextures();
        }
        
        SaveProfileToFilesystem();
    }

    public static void LoadProfileFromFilesystem()
    {
        
    }

    public static void SaveProfileToFilesystem()
    {
        
    }

    [Serializable]
    public class Profile
    {
        // Sticks section
        // public ReskinRegistry.ReskinEntry stickAttackerPersonal;
        public ReskinRegistry.ReskinEntry stickAttackerBlue;
        public ReskinRegistry.ReskinEntry stickAttackerBluePersonal;
        public ReskinRegistry.ReskinEntry stickAttackerRed;
        public ReskinRegistry.ReskinEntry stickAttackerRedPersonal;
        public ReskinRegistry.ReskinEntry stickGoaliePersonal;
        public ReskinRegistry.ReskinEntry stickGoalieBlue;
        public ReskinRegistry.ReskinEntry stickGoalieRed;
        
        // Ice section
        public ReskinRegistry.ReskinEntry ice;
        public float                      iceSmoothness;
        
        // Puck section
        public ReskinRegistry.ReskinEntry puck;
        
        // Arena section
        public bool crowdEnabled = true;
        public bool hangarEnabled = true;
    } 
}