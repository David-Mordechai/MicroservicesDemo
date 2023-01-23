using AeroMapPresentor.Core.Models;

namespace AeroMapPresenter.Mvvm.Wpf.ViewModels
{
    public class MapEntityViewModel : BaseViewModel
    {
        private double _xPosition;
        private double _yPosition;
        private string _title;

        public MapEntityViewModel(MapEntity model)
        {
            XPosition = model.XPosition;
            YPosition = model.YPosition;
            Title = model.Title;
        }

        public double XPosition
        {
            get => _xPosition;
            set => SetField(ref _xPosition, value);
        }

        public double YPosition
        {
            get => _yPosition;
            set => SetField(ref _yPosition, value);
        }

        public string Title
        {
            get => _title;
            set => SetField(ref _title, value);
        }
    }
}
