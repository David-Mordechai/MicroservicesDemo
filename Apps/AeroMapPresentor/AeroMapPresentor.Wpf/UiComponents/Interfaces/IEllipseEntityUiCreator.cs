using System.Windows.Controls;
using AeroMapPresentor.Core.Models;

namespace AeroMapPresentor.Wpf.UiComponents.Interfaces;

public interface IEllipseEntityUiCreator
{
    (StackPanel stackPanel, double top, double left) Create(MapEntity mapEntity, double windowHeight, double windowWidth);
}