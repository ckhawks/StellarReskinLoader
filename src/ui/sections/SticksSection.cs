using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ToasterReskinLoader.ui.sections;

public static class SticksSection
{
    public static void CreateSection(VisualElement contentScrollViewContent)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "ChangingRoom")
        {
            ReskinMenu.changingRoomManager.Client_MoveCameraToAppearanceDefaultPosition();
            ReskinMenu.changingRoomStick.Client_MoveStickToAppearanceStickPosition(); // TODO this might cause glitchy because we are doing it right after the other one fires
        }
            
        // Attacker section
        List<ReskinRegistry.ReskinEntry> attackerStickReskins = ReskinRegistry.GetReskinEntriesByType("stick_attacker");
        ReskinRegistry.ReskinEntry unchangedEntry = new ReskinRegistry.ReskinEntry();
        unchangedEntry.Name = "Unchanged";
        unchangedEntry.Path = null;
        unchangedEntry.Type = "stick_attacker";
        attackerStickReskins.Insert(0, unchangedEntry);
        // List<string> attackerStickReskinsStrings = new List<string>();
        // attackerStickReskinsStrings.Add("Unchanged");
        // foreach (ReskinRegistry.ReskinEntry reskinEntry in attackerStickReskins)
        // {
        //     Plugin.Log($"Adding {reskinEntry.Name} to attackerSticks");
        //     attackerStickReskinsStrings.Add(reskinEntry.Name);
        //     // TODO this might have a problem where two reskins have the same name and then can't be selected correctly in the dropdown
        // }
        // Plugin.Log($"attackerStickReskinsStrings length: {attackerStickReskinsStrings.Count}");
        Label attackerSticksTitle = new Label("Skater");
        attackerSticksTitle.style.fontSize = 24;
        attackerSticksTitle.style.color = Color.white;
        contentScrollViewContent.Add(attackerSticksTitle);
            
        VisualElement attackerBluePersonalStickRow = UITools.CreateConfigurationRow();
        attackerBluePersonalStickRow.Add(UITools.CreateConfigurationLabel("Blue personal"));
            
        PopupField<ReskinRegistry.ReskinEntry> attackerBluePersonalStickDropdown = UITools.CreateConfigurationDropdownField();
        attackerBluePersonalStickDropdown.RegisterCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(
            new EventCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(evt =>
            {
                ReskinRegistry.ReskinEntry chosen = evt.newValue;
                Plugin.Log($"User picked ID={chosen.Path}, Name={chosen.Name}");
                ReskinProfileManager.SetSelectedReskinInCurrentProfile(chosen, "attacker_stick", "blue_personal");
            })
        );
        // attackerPersonalStickDropdown.index = 0;
        attackerBluePersonalStickDropdown.choices = attackerStickReskins;
        attackerBluePersonalStickRow.Add(attackerBluePersonalStickDropdown);
        contentScrollViewContent.Add(attackerBluePersonalStickRow);
        
        VisualElement attackerRedPersonalStickRow = UITools.CreateConfigurationRow();
        attackerRedPersonalStickRow.Add(UITools.CreateConfigurationLabel("Red personal"));
            
        PopupField<ReskinRegistry.ReskinEntry> attackerRedPersonalStickDropdown = UITools.CreateConfigurationDropdownField();
        attackerRedPersonalStickDropdown.RegisterCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(
            new EventCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(evt =>
            {
                ReskinRegistry.ReskinEntry chosen = evt.newValue;
                Plugin.Log($"User picked ID={chosen.Path}, Name={chosen.Name}");
                ReskinProfileManager.SetSelectedReskinInCurrentProfile(chosen, "attacker_stick", "red_personal");
            })
        );
        // attackerPersonalStickDropdown.index = 0;
        attackerRedPersonalStickDropdown.choices = attackerStickReskins;
        attackerRedPersonalStickRow.Add(attackerRedPersonalStickDropdown);
        contentScrollViewContent.Add(attackerRedPersonalStickRow);
        
            
        VisualElement attackerBlueStickRow = UITools.CreateConfigurationRow();
        attackerBlueStickRow.Add(UITools.CreateConfigurationLabel("Blue team"));
        PopupField<ReskinRegistry.ReskinEntry> attackerBlueStickDropdown = UITools.CreateConfigurationDropdownField();
        attackerBlueStickDropdown.RegisterCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(
            new EventCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(evt =>
            {
                ReskinRegistry.ReskinEntry chosen = evt.newValue;
                Plugin.Log($"User picked ID={chosen.Path}, Name={chosen.Name}");
                ReskinProfileManager.SetSelectedReskinInCurrentProfile(chosen, "attacker_stick", "blue_team");
            })
        );
        attackerBlueStickDropdown.choices = attackerStickReskins;
        attackerBlueStickDropdown.index = 0;
        attackerBlueStickRow.Add(attackerBlueStickDropdown);
        contentScrollViewContent.Add(attackerBlueStickRow);
            
        VisualElement attackerRedStickRow = UITools.CreateConfigurationRow();
        attackerRedStickRow.Add(UITools.CreateConfigurationLabel("Red team"));
        PopupField<ReskinRegistry.ReskinEntry> attackerRedStickDropdown = UITools.CreateConfigurationDropdownField();
        attackerRedStickDropdown.RegisterCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(
            new EventCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(evt =>
            {
                ReskinRegistry.ReskinEntry chosen = evt.newValue;
                Plugin.Log($"User picked ID={chosen.Path}, Name={chosen.Name}");
                ReskinProfileManager.SetSelectedReskinInCurrentProfile(chosen, "attacker_stick", "red_team");
            })
        );
        attackerRedStickDropdown.choices = attackerStickReskins;
        attackerRedStickDropdown.index = 0;
        attackerRedStickRow.Add(attackerRedStickDropdown);
        contentScrollViewContent.Add(attackerRedStickRow);
            
        // Goalie section
        List<ReskinRegistry.ReskinEntry> goalie_stick_reskins = ReskinRegistry.GetReskinEntriesByType("stick_goalie");
        Label goalieSticksTitle = new Label("Goalie");
        goalieSticksTitle.style.fontSize = 24;
        goalieSticksTitle.style.color = Color.white;
        // contentSectionTitle.style.marginTop = 20;
        contentScrollViewContent.Add(goalieSticksTitle);
    }
    
}