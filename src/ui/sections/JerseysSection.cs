using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ToasterReskinLoader.ui.sections;

public static class JerseysSection
{
    public static void CreateSection(VisualElement contentScrollViewContent)
    {
        ChangingRoomHelper.ShowBody();
        
        // Change blue team torso
        // Change blue team groin
        // Change blue goalie torso
        // Change blue goalie groin
        // Change red team torso
        // Change red team groin
        // Change red goalie torso
        // Change red goalie groin
            
        // Attacker section
        List<ReskinRegistry.ReskinEntry> jerseyTorsos = ReskinRegistry.GetReskinEntriesByType("jersey_torso");
        ReskinRegistry.ReskinEntry unchangedJerseyTorsoEntry = new ReskinRegistry.ReskinEntry
        {
            Name = "Unchanged",
            Path = null,
            Type = "jersey_torso"
        };
        jerseyTorsos.Insert(0, unchangedJerseyTorsoEntry);
        
        List<ReskinRegistry.ReskinEntry> jerseyGroins = ReskinRegistry.GetReskinEntriesByType("jersey_groin");
        ReskinRegistry.ReskinEntry unchangedJerseyGroinEntry = new ReskinRegistry.ReskinEntry
        {
            Name = "Unchanged",
            Path = null,
            Type = "jersey_groin"
        };
        jerseyGroins.Insert(0, unchangedJerseyGroinEntry);

        // BLUE TEAM
        Label blueTeamTitle = new Label("Blue");
        blueTeamTitle.style.fontSize = 24;
        blueTeamTitle.style.color = Color.white;
        contentScrollViewContent.Add(blueTeamTitle);
        
        VisualElement blueSkaterTorsoRow = UITools.CreateConfigurationRow();
        blueSkaterTorsoRow.Add(UITools.CreateConfigurationLabel("Skater Torso"));
            
        PopupField<ReskinRegistry.ReskinEntry> blueSkaterTorsoDropdown = UITools.CreateConfigurationDropdownField();
        blueSkaterTorsoDropdown.RegisterCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(
            new EventCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(evt =>
            {
                ReskinRegistry.ReskinEntry chosen = evt.newValue;
                Plugin.Log($"User picked ID={chosen.Path}, Name={chosen.Name}");
                ReskinProfileManager.SetSelectedReskinInCurrentProfile(chosen, "jersey_torso", "blue_skater");
            })
        );
        // attackerPersonalStickDropdown.index = 0;
        blueSkaterTorsoDropdown.choices = jerseyTorsos;
        blueSkaterTorsoDropdown.value = ReskinProfileManager.currentProfile.blueSkaterTorso != null
            ? ReskinProfileManager.currentProfile.blueSkaterTorso
            : unchangedJerseyTorsoEntry;
        blueSkaterTorsoRow.Add(blueSkaterTorsoDropdown);
        contentScrollViewContent.Add(blueSkaterTorsoRow);
        
       
        VisualElement blueSkaterGroinRow = UITools.CreateConfigurationRow();
        blueSkaterGroinRow.Add(UITools.CreateConfigurationLabel("Skater Groin"));
            
        PopupField<ReskinRegistry.ReskinEntry> blueSkaterGroinDropdown = UITools.CreateConfigurationDropdownField();
        blueSkaterGroinDropdown.RegisterCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(
            new EventCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(evt =>
            {
                ReskinRegistry.ReskinEntry chosen = evt.newValue;
                Plugin.Log($"User picked ID={chosen.Path}, Name={chosen.Name}");
                ReskinProfileManager.SetSelectedReskinInCurrentProfile(chosen, "jersey_groin", "blue_skater");
            })
        );
        // attackerPersonalStickDropdown.index = 0;
        blueSkaterGroinDropdown.choices = jerseyGroins;
        blueSkaterGroinDropdown.value = ReskinProfileManager.currentProfile.blueSkaterGroin != null
            ? ReskinProfileManager.currentProfile.blueSkaterGroin
            : unchangedJerseyGroinEntry;
        blueSkaterGroinRow.Add(blueSkaterGroinDropdown);
        contentScrollViewContent.Add(blueSkaterGroinRow);
        
        
        VisualElement blueGoalieTorsoRow = UITools.CreateConfigurationRow();
        blueGoalieTorsoRow.Add(UITools.CreateConfigurationLabel("Goalie Torso"));
            
        PopupField<ReskinRegistry.ReskinEntry> blueGoalieTorsoDropdown = UITools.CreateConfigurationDropdownField();
        blueGoalieTorsoDropdown.RegisterCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(
            new EventCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(evt =>
            {
                ReskinRegistry.ReskinEntry chosen = evt.newValue;
                Plugin.Log($"User picked ID={chosen.Path}, Name={chosen.Name}");
                ReskinProfileManager.SetSelectedReskinInCurrentProfile(chosen, "jersey_torso", "blue_goalie");
            })
        );
        // attackerPersonalStickDropdown.index = 0;
        blueGoalieTorsoDropdown.choices = jerseyTorsos;
        blueGoalieTorsoDropdown.value = ReskinProfileManager.currentProfile.blueGoalieTorso != null
            ? ReskinProfileManager.currentProfile.blueGoalieTorso
            : unchangedJerseyTorsoEntry;
        blueGoalieTorsoRow.Add(blueGoalieTorsoDropdown);
        contentScrollViewContent.Add(blueGoalieTorsoRow);
        
       
        VisualElement blueGoalieGroinRow = UITools.CreateConfigurationRow();
        blueGoalieGroinRow.Add(UITools.CreateConfigurationLabel("Goalie Groin"));
            
        PopupField<ReskinRegistry.ReskinEntry> blueGoalieGroinDropdown = UITools.CreateConfigurationDropdownField();
        blueGoalieGroinDropdown.RegisterCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(
            new EventCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(evt =>
            {
                ReskinRegistry.ReskinEntry chosen = evt.newValue;
                Plugin.Log($"User picked ID={chosen.Path}, Name={chosen.Name}");
                ReskinProfileManager.SetSelectedReskinInCurrentProfile(chosen, "jersey_groin", "blue_goalie");
            })
        );
        // attackerPersonalStickDropdown.index = 0;
        blueGoalieGroinDropdown.choices = jerseyGroins;
        blueGoalieGroinDropdown.value = ReskinProfileManager.currentProfile.blueGoalieGroin != null
            ? ReskinProfileManager.currentProfile.blueGoalieGroin
            : unchangedJerseyGroinEntry;
        blueGoalieGroinRow.Add(blueGoalieGroinDropdown);
        contentScrollViewContent.Add(blueGoalieGroinRow);
        
        
        // RED TEAM
        Label redTeamTitle = new Label("Red");
        redTeamTitle.style.fontSize = 24;
        redTeamTitle.style.color = Color.white;
        contentScrollViewContent.Add(redTeamTitle);
        
        VisualElement redSkaterTorsoRow = UITools.CreateConfigurationRow();
        redSkaterTorsoRow.Add(UITools.CreateConfigurationLabel("Skater Torso"));
            
        PopupField<ReskinRegistry.ReskinEntry> redSkaterTorsoDropdown = UITools.CreateConfigurationDropdownField();
        redSkaterTorsoDropdown.RegisterCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(
            new EventCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(evt =>
            {
                ReskinRegistry.ReskinEntry chosen = evt.newValue;
                Plugin.Log($"User picked ID={chosen.Path}, Name={chosen.Name}");
                ReskinProfileManager.SetSelectedReskinInCurrentProfile(chosen, "jersey_torso", "red_skater");
            })
        );
        // attackerPersonalStickDropdown.index = 0;
        redSkaterTorsoDropdown.choices = jerseyTorsos;
        redSkaterTorsoDropdown.value = ReskinProfileManager.currentProfile.redSkaterTorso != null
            ? ReskinProfileManager.currentProfile.redSkaterTorso
            : unchangedJerseyTorsoEntry;
        redSkaterTorsoRow.Add(redSkaterTorsoDropdown);
        contentScrollViewContent.Add(redSkaterTorsoRow);
        
       
        VisualElement redSkaterGroinRow = UITools.CreateConfigurationRow();
        redSkaterGroinRow.Add(UITools.CreateConfigurationLabel("Skater Groin"));
            
        PopupField<ReskinRegistry.ReskinEntry> redSkaterGroinDropdown = UITools.CreateConfigurationDropdownField();
        redSkaterGroinDropdown.RegisterCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(
            new EventCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(evt =>
            {
                ReskinRegistry.ReskinEntry chosen = evt.newValue;
                Plugin.Log($"User picked ID={chosen.Path}, Name={chosen.Name}");
                ReskinProfileManager.SetSelectedReskinInCurrentProfile(chosen, "jersey_groin", "red_skater");
            })
        );
        // attackerPersonalStickDropdown.index = 0;
        redSkaterGroinDropdown.choices = jerseyGroins;
        redSkaterGroinDropdown.value = ReskinProfileManager.currentProfile.redSkaterGroin != null
            ? ReskinProfileManager.currentProfile.redSkaterGroin
            : unchangedJerseyGroinEntry;
        redSkaterGroinRow.Add(redSkaterGroinDropdown);
        contentScrollViewContent.Add(redSkaterGroinRow);
        
        
        VisualElement redGoalieTorsoRow = UITools.CreateConfigurationRow();
        redGoalieTorsoRow.Add(UITools.CreateConfigurationLabel("Goalie Torso"));
            
        PopupField<ReskinRegistry.ReskinEntry> redGoalieTorsoDropdown = UITools.CreateConfigurationDropdownField();
        redGoalieTorsoDropdown.RegisterCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(
            new EventCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(evt =>
            {
                ReskinRegistry.ReskinEntry chosen = evt.newValue;
                Plugin.Log($"User picked ID={chosen.Path}, Name={chosen.Name}");
                ReskinProfileManager.SetSelectedReskinInCurrentProfile(chosen, "jersey_torso", "red_goalie");
            })
        );
        // attackerPersonalStickDropdown.index = 0;
        redGoalieTorsoDropdown.choices = jerseyTorsos;
        redGoalieTorsoDropdown.value = ReskinProfileManager.currentProfile.redGoalieTorso != null
            ? ReskinProfileManager.currentProfile.redGoalieTorso
            : unchangedJerseyTorsoEntry;
        redGoalieTorsoRow.Add(redGoalieTorsoDropdown);
        contentScrollViewContent.Add(redGoalieTorsoRow);
        
       
        VisualElement redGoalieGroinRow = UITools.CreateConfigurationRow();
        redGoalieGroinRow.Add(UITools.CreateConfigurationLabel("Goalie Groin"));
            
        PopupField<ReskinRegistry.ReskinEntry> redGoalieGroinDropdown = UITools.CreateConfigurationDropdownField();
        redGoalieGroinDropdown.RegisterCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(
            new EventCallback<ChangeEvent<ReskinRegistry.ReskinEntry>>(evt =>
            {
                ReskinRegistry.ReskinEntry chosen = evt.newValue;
                Plugin.Log($"User picked ID={chosen.Path}, Name={chosen.Name}");
                ReskinProfileManager.SetSelectedReskinInCurrentProfile(chosen, "jersey_groin", "red_goalie");
            })
        );
        // attackerPersonalStickDropdown.index = 0;
        redGoalieGroinDropdown.choices = jerseyGroins;
        redGoalieGroinDropdown.value = ReskinProfileManager.currentProfile.redGoalieGroin != null
            ? ReskinProfileManager.currentProfile.redGoalieGroin
            : unchangedJerseyGroinEntry;
        redGoalieGroinRow.Add(redGoalieGroinDropdown);
        contentScrollViewContent.Add(redGoalieGroinRow);
    }
}