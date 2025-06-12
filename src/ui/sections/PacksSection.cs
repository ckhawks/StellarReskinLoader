using UnityEngine;
using UnityEngine.UIElements;

namespace ToasterReskinLoader.ui.sections;

public static class PacksSection
{
    public static void CreateSection(VisualElement contentScrollViewContent)
    {
        ChangingRoomHelper.ShowBaseFocus();
            
        Label packsNumberLabel =
            UITools.CreateConfigurationLabel($"{ReskinRegistry.reskinPacks.Count} pack{(ReskinRegistry.reskinPacks.Count == 1 ? "" : "s")} loaded");
        
        packsNumberLabel.style.marginBottom = 16;
        contentScrollViewContent.Add(packsNumberLabel);
        
        // https://steamcommunity.com/workshop/browse/?appid=2994020&requiredtags[]=Resource+Pack
            
        // For each loaded pack,
        foreach (var pack in ReskinRegistry.reskinPacks)
        {
            VisualElement row = UITools.CreateConfigurationRow();
                
            Label packLabel = UITools.CreateConfigurationLabel(pack.Name);
            row.Add(packLabel);
            VisualElement rightSide = new VisualElement();
            rightSide.style.flexDirection = FlexDirection.Row;
            rightSide.style.alignItems = Align.Center;
            // rightSide.style.justifyContent = Justify.SpaceBetween;
            if (pack.WorkshopId != 0)
            {
                // Label workshopLabel = UITools.CreateConfigurationLabel($"Workshop {pack.WorkshopId}");
                // row.Add(workshopLabel);
                Button workshopPackButton = new Button
                {
                    text = "View on Workshop",
                    style =
                    {
                        backgroundColor = new StyleColor(new Color(0.25f, 0.25f, 0.25f)),
                        unityTextAlign = TextAnchor.MiddleCenter,
                        // width = new StyleLength(new Length(80, LengthUnit.Pixel)),
                        // minWidth = new StyleLength(new Length(80, LengthUnit.Pixel)),
                        // maxWidth = new StyleLength(new Length(80, LengthUnit.Pixel)),
                        fontSize = 10,
                        // height = 24,
                        // minHeight = 24,
                        // maxHeight = 24,
                        // marginTop = 2,
                        paddingTop = 2,
                        paddingBottom = 2,
                        paddingLeft = 8,
                        paddingRight = 8,
                        marginRight = 8,
                    }
                };
                workshopPackButton.RegisterCallback<MouseEnterEvent>(new EventCallback<MouseEnterEvent>((evt) =>
                {
                    workshopPackButton.style.backgroundColor = Color.white;
                    workshopPackButton.style.color = Color.black;
                }));
                workshopPackButton.RegisterCallback<MouseLeaveEvent>(new EventCallback<MouseLeaveEvent>((evt) =>
                {
                    workshopPackButton.style.backgroundColor = new StyleColor(new Color(0.25f, 0.25f, 0.25f));
                    workshopPackButton.style.color = Color.white;
                }));
                workshopPackButton.RegisterCallback<ClickEvent>(WorkshopPackButtonClickHandler);
        
                void WorkshopPackButtonClickHandler(ClickEvent evt)
                {
                    Application.OpenURL($"https://steamcommunity.com/sharedfiles/filedetails/?id={pack.WorkshopId}");
                }
                
                rightSide.Add(workshopPackButton);
            }
            else
            {
                Label workshopLabel = UITools.CreateConfigurationLabel($"Local pack");
                workshopLabel.style.fontSize = 12;
                workshopLabel.style.marginRight = 12;
                rightSide.Add(workshopLabel);
            }
            Label packDetailsLabel = UITools.CreateConfigurationLabel($"{pack.Reskins.Count} reskin{(pack.Reskins.Count == 1 ? "" : "s")}");
            packDetailsLabel.style.width = 80;
            packDetailsLabel.style.unityTextAlign = TextAnchor.MiddleRight;
            rightSide.Add(packDetailsLabel);
            row.Add(rightSide);
            
            contentScrollViewContent.Add(row);
        }
    }
}