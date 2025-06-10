using System.Collections.Generic;
using System.Reflection;
using ToasterReskinLoader.swappers;
using UnityEngine.UIElements;

namespace ToasterReskinLoader.ui.sections;

public static class ArenaSection
{
    static readonly FieldInfo _showPlayerUsernamesToggleField = typeof(UISettings)
        .GetField("showPlayerUsernamesToggle", 
            BindingFlags.Instance | BindingFlags.NonPublic);
    
    public static void CreateSection(VisualElement contentScrollViewContent)
    {
        // Toggle showPlayerUsernamesToggle = (Toggle) _showPlayerUsernamesToggleField.GetValue(UISettings.Instance);
        //
        // if (showPlayerUsernamesToggle == null)
        // {
        //     Plugin.Log($"Could not find showPlayerUsernamesToggle :(");
        // }
        // List<ReskinRegistry.ReskinEntry> attackerStickReskins = ReskinRegistry.GetReskinEntriesByType("rink_ice");
        // ReskinRegistry.ReskinEntry unchangedEntry = new ReskinRegistry.ReskinEntry();
        // unchangedEntry.Name = "Unchanged";
        // unchangedEntry.Path = null;
        // unchangedEntry.Type = "rink_ice";
        // attackerStickReskins.Insert(0, unchangedEntry);
        
        VisualElement crowdRow = UITools.CreateConfigurationRow();
        crowdRow.Add(UITools.CreateConfigurationLabel("Crowd"));

        Toggle crowdToggle = UITools.CreateConfigurationCheckbox(ReskinProfileManager.profile.crowdEnabled);
        // crowdToggle.style.backgroundColor = showPlayerUsernamesToggle.style.backgroundColor;
        // crowdToggle.style.width = showPlayerUsernamesToggle.style.width;
        // crowdToggle.style.height = showPlayerUsernamesToggle.style.height;
        // crowdToggle.style.color = showPlayerUsernamesToggle.style.color;
        crowdToggle.RegisterCallback<ChangeEvent<bool>>(
            new EventCallback<ChangeEvent<bool>>(evt =>
            {
                bool crowdState = evt.newValue;
                Plugin.Log($"User picked crowd: {crowdState}");
                ReskinProfileManager.profile.crowdEnabled = crowdState;
                ArenaSwapper.UpdateCrowdState();
            })
        );
        crowdRow.Add(crowdToggle);
        contentScrollViewContent.Add(crowdRow);
        
        VisualElement hangarRow = UITools.CreateConfigurationRow();
        hangarRow.Add(UITools.CreateConfigurationLabel("Hangar"));

        Toggle hangarToggle = UITools.CreateConfigurationCheckbox(ReskinProfileManager.profile.hangarEnabled);
        hangarToggle.RegisterCallback<ChangeEvent<bool>>(
            new EventCallback<ChangeEvent<bool>>(evt =>
            {
                bool hangarState = evt.newValue;
                Plugin.Log($"User picked hangar: {hangarState}");
                ReskinProfileManager.profile.hangarEnabled = hangarState;
                ArenaSwapper.UpdateHangarState();
            })
        );
        hangarRow.Add(hangarToggle);
        contentScrollViewContent.Add(hangarRow);
    }
}
