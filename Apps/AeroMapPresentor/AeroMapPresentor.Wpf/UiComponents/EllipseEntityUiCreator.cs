using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AeroMapPresentor.Core.Models;
using AeroMapPresentor.Wpf.UiComponents.Interfaces;

namespace AeroMapPresentor.Wpf.UiComponents;

internal class EllipseEntityUiCreator : IEllipseEntityUiCreator
{
    public (StackPanel stackPanel, double top, double left) Create(MapEntity mapEntity, double windowHeight, double windowWidth)
    {
        const int ellipseSize = 20;
        var top = windowHeight / mapEntity.MapHeight * mapEntity.XPosition - ellipseSize;
        var left = windowWidth / mapEntity.MapWidth * mapEntity.YPosition - ellipseSize;
            
        var ellipse = new Ellipse
        {
            Fill = Brushes.Purple,
            Stroke = Brushes.MediumPurple,
            Width = ellipseSize,
            Height = ellipseSize,
            StrokeThickness = 2
        };

        var textBlock = new TextBlock
        {
            Foreground = Brushes.Purple,
            FontSize = 12,
            Text = mapEntity.Title
        };

        var stackPanel = new StackPanel();
        stackPanel.Children.Add(ellipse);
        stackPanel.Children.Add(textBlock);
        return (stackPanel, top, left);
    }
}