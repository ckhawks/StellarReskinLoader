using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ToasterReskinLoader.ui.sections;

public static class IceSection
{
    public static void CreateSection(VisualElement contentScrollViewContent)
    {
        List<ReskinRegistry.ReskinEntry> attackerStickReskins = ReskinRegistry.GetReskinEntriesByType("rink_ice");
        ReskinRegistry.ReskinEntry unchangedEntry = new ReskinRegistry.ReskinEntry();
        unchangedEntry.Name = "Unchanged";
        unchangedEntry.Path = null;
        unchangedEntry.Type = "rink_ice";
        attackerStickReskins.Insert(0, unchangedEntry);
        
        VisualElement iceRow = UITools.CreateConfigurationRow();
        iceRow.Add(UITools.CreateConfigurationLabel("Ice"));
            
        PopupField<ReskinRegistry.ReskinEntry> iceDropdown = UITools.CreateConfigurationDropdownField();
        iceDropdown.RegisterCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(
            new EventCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(evt =>
            {
                ReskinRegistry.ReskinEntry chosen = evt.newValue;
                Plugin.Log($"User picked ID={chosen.Path}, Name={chosen.Name}");
                ReskinProfileManager.SetSelectedReskinInCurrentProfile(chosen, "rink_ice", null);
            })
        );
        // attackerPersonalStickDropdown.index = 0;
        iceDropdown.choices = attackerStickReskins;
        iceRow.Add(iceDropdown);
        contentScrollViewContent.Add(iceRow);
    }
}