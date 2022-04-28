using System.Threading.Tasks;
using System.Windows.Controls;

namespace AeroMapPresentor.Wpf.ViewModels;

public interface IMainWindowViewModel
{
    Task SetImageSource();
    void CreateMapEntity(string message, Canvas canvasEntities);
}
