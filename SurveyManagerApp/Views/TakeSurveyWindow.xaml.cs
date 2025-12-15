using SurveyManagerApp.Services; // Добавлено
using SurveyManagerApp.ViewModels;
using System.Windows;

namespace SurveyManagerApp.Views
{
    public partial class TakeSurveyWindow : Window
    {
        // Принимаем AnswerService через конструктор
        public TakeSurveyWindow(TakeSurveyViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is TakeSurveyViewModel viewModel)
            {
                viewModel.SubmitCommand.Execute(null);
                // Окно закрывается после выполнения команды
                // this.Close(); // Можно вызвать здесь, если VM не управляет этим
            }
        }
    }
}