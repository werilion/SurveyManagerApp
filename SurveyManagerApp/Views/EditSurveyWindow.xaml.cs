using SurveyManagerApp.ViewModels;
using System.Windows;

namespace SurveyManagerApp.Views
{
    public partial class EditSurveyWindow : Window
    {
        public EditSurveyWindow(EditSurveyViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is EditSurveyViewModel viewModel)
            {
                // Вызываем команду сохранения
                viewModel.SaveCommand.Execute(null);
                // Окно закрывается автоматически после выполнения команды Save в ViewModel
                // Но если Close не вызывается из VM, можно закрыть здесь:
                // this.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}