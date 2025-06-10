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
        Plugin.Log($"Creating configuration dropdown field");
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
        Plugin.Log($"here1");
        popupField.formatSelectedValueCallback = e => (e ==null) ? "None" : e.Name; // formatItemCallback2; // TODO NullReferenceException: Object reference not set to an instance of an object
        Plugin.Log($"here1.1");
        popupField.formatListItemCallback = e => e.Name;
        Plugin.Log($"here2");
            
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
        Plugin.Log($"here3");
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
}