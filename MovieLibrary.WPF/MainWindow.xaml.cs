using MovieLibrary.WPF.Utility;
using MovieLibrary.WPF.ViewModels;
using System.Windows;

namespace MovieLibrary.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel(new DialogService());
        }
    }
}
