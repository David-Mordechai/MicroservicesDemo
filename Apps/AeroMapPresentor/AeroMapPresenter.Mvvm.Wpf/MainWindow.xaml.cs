using AeroMapPresenter.Mvvm.Wpf.ViewModels;
using System.Windows;

namespace AeroMapPresenter.Mvvm.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            DataContext = mainWindowViewModel;
            InitializeComponent();
        }
    }
}
