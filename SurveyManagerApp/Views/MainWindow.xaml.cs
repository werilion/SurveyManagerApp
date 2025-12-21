using SurveyManagerApp.Services;
using SurveyManagerApp.ViewModels;
using System.Windows;

namespace SurveyManagerApp
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;
        private readonly SurveyService _surveyService;
        private readonly AnswerService _answerService;

        public MainWindow(MainWindowViewModel viewModel, SurveyService surveyService, AnswerService answerService)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _surveyService = surveyService;
            _answerService = answerService;
            DataContext = viewModel;
        }

        private void OpenChildWindow(Window childWindow)
        {
            this.Hide();
            childWindow.ShowDialog();
            this.Show();
            _viewModel.LoadSurveys();
        }

        private void NewSurveyButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_viewModel.NewSurveyTitle))
            {
                var newSurvey = new Models.Survey
                {
                    Title = _viewModel.NewSurveyTitle,
                    Description = _viewModel.NewSurveyDescription,
                    Questions = new System.Collections.ObjectModel.ObservableCollection<Models.Question>()
                };
                var editWindow = new Views.EditSurveyWindow(new EditSurveyViewModel(newSurvey, _surveyService));
                OpenChildWindow(editWindow);
                _viewModel.NewSurveyTitle = string.Empty;
                _viewModel.NewSurveyDescription = string.Empty;
            }
        }

        private void EditSurveyButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedSurvey != null)
            {
                var editWindow = new Views.EditSurveyWindow(new EditSurveyViewModel(_viewModel.SelectedSurvey, _surveyService));
                OpenChildWindow(editWindow);
            }
        }

        private void TakeSurveyButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedSurvey != null)
            {
                var takeWindow = new Views.TakeSurveyWindow(new TakeSurveyViewModel(_viewModel.SelectedSurvey, _answerService));
                OpenChildWindow(takeWindow);
            }
        }

        private void DeleteSurveyButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedSurvey != null)
            {
                _viewModel.DeleteSurvey();
            }
        }
    }
}