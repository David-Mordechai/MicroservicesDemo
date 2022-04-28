using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AeroMapPresentor.Wpf.UiComponents.Interfaces;

namespace AeroMapPresentor.Wpf.UiComponents
{
    internal class EllipseEntityUiCreator : IEllipseEntityUiCreator
    {
        public StackPanel Create(MainWindow.MapEntity mapEntity)
        {
            var ellipse = new Ellipse
            {
                Fill = Brushes.Purple,
                Stroke = Brushes.MediumPurple,
                Width = 20,
                Height = 20,
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
            return stackPanel;
        }
    }
}
