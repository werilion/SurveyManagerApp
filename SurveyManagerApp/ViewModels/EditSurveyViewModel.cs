using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SurveyManagerApp.Models;
using SurveyManagerApp.Services;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace SurveyManagerApp.ViewModels
{
    public partial class EditSurveyViewModel : ObservableObject
    {
        private readonly SurveyService _surveyService;
        public Survey Survey { get; }
        private readonly List<QuestionType> _questionTypesList;

        public IEnumerable<QuestionType> QuestionTypes
        {
            get { return _questionTypesList; }
        }

        private Question _selectedQuestion;
        public Question SelectedQuestion
        {
            get { return _selectedQuestion; }
            set
            {
                if (_selectedQuestion != value)
                {
                    _selectedQuestion = value;
                    OnPropertyChanged(nameof(SelectedQuestion));
                    ((RelayCommand)RemoveQuestionCommand).NotifyCanExecuteChanged();
                }
            }
        }

        public ICommand AddQuestionCommand { get; }
        public ICommand RemoveQuestionCommand { get; }
        public ICommand AddOptionCommand { get; }
        public ICommand RemoveOptionCommand { get; }
        public ICommand SaveCommand { get; }

        public EditSurveyViewModel(Survey survey, SurveyService surveyService)
        {
            Survey = survey;
            _surveyService = surveyService;

            _questionTypesList = new List<QuestionType>(Enum.GetValues(typeof(QuestionType)).Cast<QuestionType>());

            AddQuestionCommand = new RelayCommand(AddQuestion);
            RemoveQuestionCommand = new RelayCommand(RemoveQuestion, () => SelectedQuestion != null);
            AddOptionCommand = new RelayCommand<string>(AddOption);
            RemoveOptionCommand = new RelayCommand<string>(RemoveOption);
            SaveCommand = new RelayCommand(Save);
        }

        private void AddQuestion()
        {
            var newQuestion = new Question { Text = "Новый вопрос", Type = QuestionType.Text };
            Survey.Questions.Add(newQuestion);
        }

        private void RemoveQuestion()
        {
            if (SelectedQuestion != null)
            {
                Survey.Questions.Remove(SelectedQuestion);
                SelectedQuestion = null;
            }
        }

        private void AddOption(string optionText)
        {
            if (SelectedQuestion != null && !string.IsNullOrWhiteSpace(optionText))
            {
                SelectedQuestion.Options.Add(optionText);
            }
        }

        private void RemoveOption(string optionText)
        {
            if (SelectedQuestion != null && optionText != null)
            {
                SelectedQuestion.Options.Remove(optionText);
            }
        }

        private void Save()
        {
            _surveyService.SaveSurvey(Survey);
            // ViewModel не вызывает Close(). Это делает View.
        }
    }
}