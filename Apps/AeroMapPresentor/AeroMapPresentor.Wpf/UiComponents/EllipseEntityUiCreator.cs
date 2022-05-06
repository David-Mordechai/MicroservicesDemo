using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AeroMapPresentor.Core.Models;
using AeroMapPresentor.Wpf.UiComponents.Interfaces;

namespace AeroMapPresentor.Wpf.UiComponents;

internal class EllipseEntityUiCreator : IEllipseEntityUiCreator
{
    public StackPanel Create(MapEntity mapEntity)
    {
        const int ellipseSize = 30;
        var ellipse = new Ellipse
        {
            Fill = Brushes.Purple,
            Opacity = 0.5d,
            Width = ellipseSize,
            Height = ellipseSize
        };
        var textBlock = new TextBlock
        {
            Foreground = Brushes.Purple,
            Opacity = 0.8d,
            FontSize = 12,
            Text = mapEntity.Title,
            TextAlignment = TextAlignment.Center,
            Width = ellipseSize,
            TextWrapping = TextWrapping.WrapWithOverflow
        };

        var stackPanel = new StackPanel();
        stackPanel.Children.Add(ellipse);
        stackPanel.Children.Add(textBlock);
        return stackPanel;
    }
}