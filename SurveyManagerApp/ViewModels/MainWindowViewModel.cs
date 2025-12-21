using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SurveyManagerApp.Models;
using SurveyManagerApp.Services;
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
        public SurveyService SurveyService { get; }
        public AnswerService AnswerService { get; }
        public ObservableCollection<Survey> Surveys { get; set; }

        private Survey _selectedSurvey;
        public Survey SelectedSurvey
        {
            get => _selectedSurvey;
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
            get => _newSurveyTitle;
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
            get => _newSurveyDescription;
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
        public ICommand ExportSurveyCommand { get; }

        public MainWindowViewModel(SurveyService surveyService, AnswerService answerService)
        {
            SurveyService = surveyService;
            AnswerService = answerService;
            Surveys = new ObservableCollection<Survey>(); // ЯВНОЕ указание типа
            LoadSurveys();

            AddSurveyCommand = new RelayCommand(AddSurvey);
            EditSurveyCommand = new RelayCommand(EditSurvey, CanEditOrTakeSurvey);
            TakeSurveyCommand = new RelayCommand(TakeSurvey, CanEditOrTakeSurvey);
            DeleteSurveyCommand = new RelayCommand(DeleteSurvey, CanDeleteSurvey);
            ExportSurveyCommand = new RelayCommand(ExportSurvey, CanExportSurvey);
        }

        public void LoadSurveys()
        {
            var loadedSurveys = SurveyService.GetAllSurveys();
            Surveys.Clear();
            foreach (var survey in loadedSurveys)
            {
                Surveys.Add(survey);
            }
        }

        private void AddSurvey() { }
        private void EditSurvey() { }
        private void TakeSurvey() { }

        // ИСПРАВЛЕНО: метод теперь PUBLIC
        public void DeleteSurvey()
        {
            if (SelectedSurvey != null)
            {
                SurveyService.DeleteSurvey(SelectedSurvey.Id);
                Surveys.Remove(SelectedSurvey);
                SelectedSurvey = null;
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