// UITools.cs

using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace ToasterReskinLoader.ui;

public static class UITools
{
    public static VisualElement CreateRow()
    {
        VisualElement row = new VisualElement();
        row.style.flexDirection = FlexDirection.Row;
        row.style.alignItems = Align.Center;
        return row;
    }
    
    public static VisualElement CreateConfigurationRow()
    {
        VisualElement row = new VisualElement();
        row.style.flexDirection = FlexDirection.Row;
        row.style.alignItems = Align.Center;
        row.style.justifyContent = Justify.SpaceBetween;
        row.style.marginTop = 4;
        row.style.marginBottom = 4;
        return row;
    }

    public static PopupField<ReskinRegistry.ReskinEntry> CreateConfigurationDropdownField()
    {
        // Plugin.Log($"Creating configuration dropdown field");
        PopupField<ReskinRegistry.ReskinEntry> popupField = new PopupField<ReskinRegistry.ReskinEntry>();
        
        // string formatItemCallback(ReskinRegistry.ReskinEntry i)
        // {
        //     return i.Name;
        // }
        // string formatItemCallback2(ReskinRegistry.ReskinEntry i)
        // {
        //     return i.Name;
        // }
        popupField.index = 0; // If you don't do this, there is no selected value, and the formatSelectedValueCallback DIES
        popupField.formatSelectedValueCallback = e => (e == null) ? "None" : e.Name; // formatItemCallback2; // TODO NullReferenceException: Object reference not set to an instance of an object
        popupField.formatListItemCallback = e => e.Name;
            
        // DropdownField dropdownField = new DropdownField();
        popupField.style.minWidth = 400;
        popupField.style.maxWidth = 400;
        popupField.style.width = 400;
        popupField.style.height = 60;
        popupField.style.minHeight = 30;
        popupField.style.maxHeight = 30;
        popupField.style.fontSize = 16;
        // dropdownField.style.marginRight = 10;
        popupField.style.overflow = Overflow.Hidden;
        return popupField;
    }
    
    public static Label CreateConfigurationLabel(string text)
    {
        Label label = new Label();
        label.text = text;
        label.style.fontSize = 16;
        label.style.color = Color.white;
        return label;
    }

    public static Toggle CreateConfigurationCheckbox(bool defaultValue)
    {
        Toggle toggle = new Toggle();
        toggle.value = defaultValue;
        return toggle;
    }

    public static Slider CreateConfigurationSlider(float lowValue, float highValue, float value, float width)
    {
        Slider slider = new Slider();
        slider.lowValue = lowValue;
        slider.highValue = highValue;
        slider.value = value;
        slider.direction = SliderDirection.Horizontal;
        slider.showInputField = true;
        slider.style.width = width;
        slider.style.minWidth = width;
        slider.style.maxWidth = width;
        slider.style.fontSize = 14;
        return slider;
    }

    public static void AddHoverEffectsForButton(Button button)
    {
        button.RegisterCallback<MouseEnterEvent>(new EventCallback<MouseEnterEvent>((evt) =>
        {
            button.style.backgroundColor = Color.white;
            button.style.color = Color.black;
        }));
        button.RegisterCallback<MouseLeaveEvent>(new EventCallback<MouseLeaveEvent>((evt) =>
        {
            button.style.backgroundColor = new StyleColor(new Color(0.25f, 0.25f, 0.25f));
            button.style.color = Color.white;
        }));
    }
    
    /// <summary>
    /// Creates a full configuration section for editing a Color value.
    /// Includes a label, a color preview box, and sliders for R, G, B, and optionally A.
    /// </summary>
    /// <param name="label">The text for the main label of the section.</param>
    /// <param name="initialColor">The starting color to display.</param>
    /// <param name="includeAlpha">If true, an 'A' (alpha) slider will be included.</param>
    /// <param name="onValueChanged">Callback that fires continuously as the color changes. Good for live previews.</param>
    /// <param name="onSave">Callback that fires when the user releases the mouse on any slider. Good for saving the final value.</param>
    /// <returns>A VisualElement containing the entire color configuration UI.</returns>
    public static VisualElement CreateColorConfigurationRow(
        string label,
        Color initialColor,
        bool includeAlpha,
        Action<Color> onValueChanged,
        Action onSave
    )
    {
        // This will hold the current color state as the user interacts with the sliders
        var currentColor = initialColor;

        // 1. Main container for the whole component
        var mainContainer = new VisualElement();
        mainContainer.style.flexDirection = FlexDirection.Column;
        mainContainer.style.marginBottom = 10;

        // 2. Top row for the main label and the color preview
        var topRow = CreateConfigurationRow();
        topRow.Add(CreateConfigurationLabel(label));

        var colorPreview = new VisualElement
        {
            style =
            {
                width = 300,
                height = 30,
                backgroundColor = initialColor,
                // borderTopWidth = 1,
                // borderBottomWidth = 1,
                // borderLeftWidth = 1,
                // borderRightWidth = 1,
                // borderTopColor = Color.white,
                // borderBottomColor = Color.white,
                // borderLeftColor = Color.white,
                // borderRightColor = Color.white,
            },
        };
        topRow.Add(colorPreview);
        mainContainer.Add(topRow);

        // 3. Container for the individual R, G, B, A sliders
        var slidersContainer = new VisualElement();
        slidersContainer.style.marginLeft = 20; // Indent sliders slightly
        mainContainer.Add(slidersContainer);

        // 4. Local helper function to avoid repeating slider creation code
        void CreateAndRegisterSliderRow(
            string componentLabel,
            float initialComponentValue,
            Func<float, float> updateComponentAction
        )
        {
            var row = CreateConfigurationRow();
            row.Add(CreateConfigurationLabel(componentLabel));
            var slider = CreateConfigurationSlider(
                0,
                1,
                initialComponentValue,
                300
            );
            

            // Register callback for continuous changes (live preview)
            slider.RegisterCallback<ChangeEvent<float>>(evt =>
            {
                // Update the specific color component (r, g, b, or a)
                updateComponentAction(evt.newValue);
                // Update the preview box color
                colorPreview.style.backgroundColor = currentColor;
                // Fire the external callback for live updates
                onValueChanged?.Invoke(currentColor);
            });

            // Register callback for when the user is done dragging (save)
            slider.RegisterCallback<PointerUpEvent>(evt =>
            {
                onSave?.Invoke();
            });

            row.Add(slider);
            slidersContainer.Add(row);
        }

        // 5. Create the sliders using the local helper
        CreateAndRegisterSliderRow(
            "Red",
            initialColor.r,
            (val) => currentColor.r = val
        );
        CreateAndRegisterSliderRow(
            "Green",
            initialColor.g,
            (val) => currentColor.g = val
        );
        CreateAndRegisterSliderRow(
            "Blue",
            initialColor.b,
            (val) => currentColor.b = val
        );

        if (includeAlpha)
        {
            CreateAndRegisterSliderRow(
                "Alpha",
                initialColor.a,
                (val) => currentColor.a = val
            );
        }

        return mainContainer;
    }
}