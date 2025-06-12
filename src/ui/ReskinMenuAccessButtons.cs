using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace ToasterReskinLoader.ui;

public static class ReskinMenuAccessButtons
{
    public static UIMainMenu mainMenu;
    public static Button mainMenuSettingsButton;
    public static Button pauseMenuSettingsButton;
    
    static readonly FieldInfo _mainMenuSettingsButtonField = typeof(UIMainMenu)
        .GetField("settingsButton", 
            BindingFlags.Instance | BindingFlags.NonPublic);
    
    static readonly FieldInfo _pauseMenuSettingsButtonField = typeof(UIPauseMenu)
        .GetField("settingsButton", 
            BindingFlags.Instance | BindingFlags.NonPublic);
    
    private static void AddReskinMenuButtonToPauseMenu(UIPauseMenu pauseMenu)
    {
        VisualElement containerVisualElement = pauseMenuSettingsButton.parent;
        // containerVisualElement.style.height = new StyleLength(new Length(1000, LengthUnit.Pixel));
        
        if (containerVisualElement == null)
        {
            Plugin.LogError("Container VisualElement not found (parent of settingsButton missing)!");
            return;
        }

        Button reskinMenuButton = CreateReskinMenuButton(pauseMenuSettingsButton);
        containerVisualElement.Insert(1, reskinMenuButton);
    }

    private static void AddReskinMenuButtonToMainMenu(UIMainMenu mainMenu)
    { 
        VisualElement containerVisualElement = mainMenuSettingsButton.parent;
        // containerVisualElement.style.height = new StyleLength(new Length(1000, LengthUnit.Pixel));
        
        if (containerVisualElement == null)
        {
            Plugin.LogError("Container VisualElement not found (parent of settingsButton missing)!");
            return;
        }

        Button reskinMenuButton = CreateReskinMenuButton(mainMenuSettingsButton);
        containerVisualElement.Insert(4, reskinMenuButton);
    }

    private static Button CreateReskinMenuButton(Button referenceButton)
    {
        Button button = new Button
        {
            text = "RESKIN MANAGER",
            style =
            {
                backgroundColor = new StyleColor(new Color(0.25f, 0.25f, 0.25f)),
                unityTextAlign = TextAnchor.MiddleLeft,
                width = new StyleLength(new Length(100, LengthUnit.Percent)),
                minWidth = new StyleLength(new Length(100, LengthUnit.Percent)),
                maxWidth = new StyleLength(new Length(100, LengthUnit.Percent)),
                // width = referenceButton.style.width,
                // minWidth = referenceButton.style.minWidth,
                // maxWidth = referenceButton.style.maxWidth,
                height = referenceButton.style.height,
                minHeight = referenceButton.style.minHeight,
                maxHeight = referenceButton.style.maxHeight,
                marginTop = 8,
                paddingTop = 8,
                paddingBottom = 8,
                paddingLeft = 15
            }
        };
        UITools.AddHoverEffectsForButton(button);
        button.RegisterCallback<ClickEvent>(MainMenuOpenReskinManagerClickHandler);

        return button;

        void MainMenuOpenReskinManagerClickHandler(ClickEvent evt)
        {
            ReskinMenu.Show();
        }
    }

    public static void Setup()
    {
        Plugin.Log($"ReskinMenuAccessButtons::Setup()");
        LocateReferenceButtons();
        AddReskinMenuButtonToPauseMenu(UIPauseMenu.Instance);
        
        // TODO is this necessary?
        mainMenu = UIMainMenu.Instance;
        ReskinMenu.uiMainMenu = UIMainMenu.Instance;
        AddReskinMenuButtonToMainMenu(UIMainMenu.Instance);
    }

    private static void LocateReferenceButtons()
    {
        mainMenuSettingsButton = (Button) _mainMenuSettingsButtonField.GetValue(UIMainMenu.Instance);
        Plugin.Log($"Located main menu settings button: {mainMenuSettingsButton}");
        pauseMenuSettingsButton = (Button)_pauseMenuSettingsButtonField.GetValue(UIPauseMenu.Instance);
        Plugin.Log($"Located pause menu settings button: {pauseMenuSettingsButton}");
    }
}