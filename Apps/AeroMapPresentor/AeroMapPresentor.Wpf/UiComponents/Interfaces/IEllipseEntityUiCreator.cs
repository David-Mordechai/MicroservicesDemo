using System.Windows.Controls;
using AeroMapPresentor.Core.Models;

namespace AeroMapPresentor.Wpf.UiComponents.Interfaces;

public interface IEllipseEntityUiCreator
{
    StackPanel Create(MapEntity mapEntity);
}