using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SurveyManagerApp.Models;
using SurveyManagerApp.Services;
using SurveyManagerApp.Views;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace SurveyManagerApp.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly SurveyService _surveyService;
        private readonly AnswerService _answerService; // Добавлено

        public ObservableCollection<Survey> Surveys { get; set; }

        private Survey _selectedSurvey;
        public Survey SelectedSurvey
        {
            get { return _selectedSurvey; }
            set
            {
                if (_selectedSurvey != value)
                {
                    _selectedSurvey = value;
                    OnPropertyChanged(nameof(SelectedSurvey));
                    UpdateCommandsCanExecute();
                }
            }
        }

        private string _newSurveyTitle = string.Empty;
        public string NewSurveyTitle
        {
            get { return _newSurveyTitle; }
            set
            {
                if (_newSurveyTitle != value)
                {
                    _newSurveyTitle = value;
                    OnPropertyChanged(nameof(NewSurveyTitle));
                }
            }
        }

        private string _newSurveyDescription = string.Empty;
        public string NewSurveyDescription
        {
            get { return _newSurveyDescription; }
            set
            {
                if (_newSurveyDescription != value)
                {
                    _newSurveyDescription = value;
                    OnPropertyChanged(nameof(NewSurveyDescription));
                }
            }
        }

        public ICommand AddSurveyCommand { get; }
        public ICommand EditSurveyCommand { get; }
        public ICommand TakeSurveyCommand { get; }
        public ICommand DeleteSurveyCommand { get; }
        public ICommand NewSurveyCommand { get; }
        public ICommand ExportSurveyCommand { get; }

        // Принимаем оба сервиса через DI
        public MainWindowViewModel(SurveyService surveyService, AnswerService answerService)
        {
            _surveyService = surveyService;
            _answerService = answerService; // Сохраняем
            Surveys = new ObservableCollection<Survey>();
            LoadSurveys();

            AddSurveyCommand = new RelayCommand(AddSurvey);
            EditSurveyCommand = new RelayCommand(EditSurvey, CanEditOrTakeSurvey);
            TakeSurveyCommand = new RelayCommand(TakeSurvey, CanEditOrTakeSurvey);
            DeleteSurveyCommand = new RelayCommand(DeleteSurvey, CanDeleteSurvey);
            NewSurveyCommand = new RelayCommand(NewSurvey);
            ExportSurveyCommand = new RelayCommand(ExportSurvey, CanExportSurvey);
        }

        private void LoadSurveys()
        {
            var loadedSurveys = _surveyService.GetAllSurveys();
            Surveys.Clear();
            foreach (var survey in loadedSurveys)
            {
                Surveys.Add(survey);
            }
        }

        private void AddSurvey()
        {
            NewSurvey();
        }

        private void NewSurvey()
        {
            if (!string.IsNullOrWhiteSpace(NewSurveyTitle))
            {
                var newSurvey = new Survey();
                newSurvey.Title = NewSurveyTitle;
                newSurvey.Description = NewSurveyDescription;
                newSurvey.Questions = new System.Collections.ObjectModel.ObservableCollection<Question>();

                var editWindow = new EditSurveyWindow(new EditSurveyViewModel(newSurvey, _surveyService));
                editWindow.ShowDialog();
                LoadSurveys();
                NewSurveyTitle = string.Empty;
                NewSurveyDescription = string.Empty;
            }
        }

        private void EditSurvey()
        {
            if (SelectedSurvey != null)
            {
                var editWindow = new EditSurveyWindow(new EditSurveyViewModel(SelectedSurvey, _surveyService));
                editWindow.ShowDialog();
                LoadSurveys();
            }
        }

        private void TakeSurvey()
        {
            if (SelectedSurvey != null)
            {
                // Передаём AnswerService в конструктор TakeSurveyViewModel
                var takeViewModel = new TakeSurveyViewModel(SelectedSurvey, _answerService);
                var takeWindow = new TakeSurveyWindow(takeViewModel);
                takeWindow.ShowDialog();
            }
        }

        private void DeleteSurvey()
        {
            var surveys = Surveys;
            var selectedSurvey = SelectedSurvey;

            if (surveys != null && selectedSurvey != null)
            {
                if (surveys.Contains(selectedSurvey))
                {
                    surveys.Remove(selectedSurvey);
                    _surveyService.DeleteSurvey(selectedSurvey.Id);
                    SelectedSurvey = null;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Попытка удалить элемент, которого нет в коллекции Surveys.");
                }
            }
        }

        private bool CanEditOrTakeSurvey() => SelectedSurvey != null;
        private bool CanDeleteSurvey() => SelectedSurvey != null;
        private bool CanExportSurvey() => SelectedSurvey != null;

        private void ExportSurvey()
        {
            if (SelectedSurvey != null)
            {
                var dialog = new SaveFileDialog
                {
                    FileName = $"{SelectedSurvey.Title}.json",
                    DefaultExt = ".json",
                    Filter = "JSON files (.json)|*.json|All files (.)|."
                };

                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        var json = JsonConvert.SerializeObject(SelectedSurvey, Newtonsoft.Json.Formatting.Indented);
                        File.WriteAllText(dialog.FileName, json);
                        MessageBox.Show($"Опрос '{SelectedSurvey.Title}' экспортирован в {dialog.FileName}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void UpdateCommandsCanExecute()
        {
            ((RelayCommand)EditSurveyCommand).NotifyCanExecuteChanged();
            ((RelayCommand)TakeSurveyCommand).NotifyCanExecuteChanged();
            ((RelayCommand)DeleteSurveyCommand).NotifyCanExecuteChanged();
            ((RelayCommand)ExportSurveyCommand).NotifyCanExecuteChanged();
        }
    }
}