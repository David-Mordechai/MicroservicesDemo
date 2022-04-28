using System.Windows.Controls;

namespace AeroMapPresentor.Wpf.UiComponents.Interfaces;

public interface IEllipseEntityUiCreator
{
    StackPanel Create(MainWindow.MapEntity mapEntity);
}