using System.Collections.Generic;
using UnityEngine.UIElements;

namespace ToasterReskinLoader.ui.sections;

public class PucksSection
{
    public static void CreateSection(VisualElement contentScrollViewContent)
    {
        List<ReskinRegistry.ReskinEntry> attackerStickReskins = ReskinRegistry.GetReskinEntriesByType("puck");
        ReskinRegistry.ReskinEntry unchangedEntry = new ReskinRegistry.ReskinEntry();
        unchangedEntry.Name = "Default";
        unchangedEntry.Path = null;
        unchangedEntry.Type = "puck";
        attackerStickReskins.Insert(0, unchangedEntry);
        
        VisualElement puckRow = UITools.CreateConfigurationRow();
        puckRow.Add(UITools.CreateConfigurationLabel("Ice"));
            
        PopupField<ReskinRegistry.ReskinEntry> puckDropdown = UITools.CreateConfigurationDropdownField();
        puckDropdown.RegisterCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(
            new EventCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(evt =>
            {
                ReskinRegistry.ReskinEntry chosen = evt.newValue;
                Plugin.Log($"User picked ID={chosen.Path}, Name={chosen.Name}");
                ReskinProfileManager.SetSelectedReskinInCurrentProfile(chosen, "puck", null);
            })
        );
        // attackerPersonalStickDropdown.index = 0;
        puckDropdown.choices = attackerStickReskins;
        puckRow.Add(puckDropdown);
        contentScrollViewContent.Add(puckRow);
    }
}