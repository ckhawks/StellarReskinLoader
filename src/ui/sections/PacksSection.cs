using UnityEngine.UIElements;

namespace ToasterReskinLoader.ui.sections;

public static class PacksSection
{
    public static void CreateSection(VisualElement contentScrollViewContent)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "ChangingRoom")
        {
            ReskinMenu.changingRoomManager.Client_MoveCameraToAppearanceDefaultPosition();
        }
            
            
        Label packsNumberLabel =
            UITools.CreateConfigurationLabel($"{ReskinRegistry.reskinPacks.Count} packs loaded");
        packsNumberLabel.style.marginBottom = 16;
        contentScrollViewContent.Add(packsNumberLabel);
            
        // For each loaded pack,
        foreach (var pack in ReskinRegistry.reskinPacks)
        {
            VisualElement row = UITools.CreateConfigurationRow();
                
            Label packLabel = UITools.CreateConfigurationLabel(pack.Name);
            row.Add(packLabel);
            Label packDetailsLabel = UITools.CreateConfigurationLabel($"{pack.Reskins.Count} reskins");
            row.Add(packDetailsLabel);
            contentScrollViewContent.Add(row);
        }
    }
}