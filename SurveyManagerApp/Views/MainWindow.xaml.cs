using SurveyManagerApp.ViewModels;
using System.Windows;

namespace SurveyManagerApp
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}