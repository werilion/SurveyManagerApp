using SurveyManagerApp.ViewModels;
using System.Windows;

namespace SurveyManagerApp.Views
{
    public partial class TakeSurveyWindow : Window
    {
        public TakeSurveyWindow(TakeSurveyViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.TakeSurveyViewModel viewModel)
            {
                viewModel.SubmitCommand.Execute(null);
                this.Close();
            }
        }
    }
}