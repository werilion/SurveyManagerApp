using SurveyManagerApp.ViewModels;
using System.Windows;

namespace SurveyManagerApp.Views
{
    public partial class EditSurveyWindow : Window
    {
        private readonly EditSurveyViewModel _viewModel;

        // УДАЛЕН параметр AnswerService
        public EditSurveyWindow(EditSurveyViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = viewModel;
        }

        // УДАЛЕН метод TakeSurveyButton_Click

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveCommand.Execute(null);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}