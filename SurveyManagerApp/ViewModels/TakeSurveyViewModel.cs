using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SurveyManagerApp.Models;
using SurveyManagerApp.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using System.Linq;
using System.Windows.Input;

namespace SurveyManagerApp.ViewModels
{
    public partial class TakeSurveyViewModel : ObservableObject
    {
        public Survey Survey { get; }
        private readonly List<(UserControl Control, Question Question)> _questionControlsAndQuestions = new List<(UserControl Control, Question Question)>();
        public ObservableCollection<UserControl> QuestionControls { get; set; }
        public ICommand SubmitCommand { get; }

        // Внедряем AnswerService через конструктор
        public TakeSurveyViewModel(Survey survey, AnswerService answerService)
        {
            Survey = survey;
            _answerService = answerService; // Сохраняем сервис
            QuestionControls = new ObservableCollection<UserControl>();
            SubmitCommand = new RelayCommand(Submit);

            foreach (var question in survey.Questions)
            {
                UserControl control;
                switch (question.Type)
                {
                    case QuestionType.Text:
                        control = CreateTextQuestionControl(question);
                        break;
                    case QuestionType.SingleChoice:
                        control = CreateSingleChoiceQuestionControl(question);
                        break;
                    case QuestionType.MultipleChoice:
                        control = CreateMultipleChoiceQuestionControl(question);
                        break;
                    default:
                        control = CreateDefaultQuestionControl(question);
                        break;
                }
                _questionControlsAndQuestions.Add((control, question)); // Сохраняем пару
                QuestionControls.Add(control);
            }
        }

        private readonly AnswerService _answerService; // Поле для сервиса

        private UserControl CreateTextQuestionControl(Question q)
        {
            var stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(0, 0, 0, 20);
            var label = new Label { Content = q.Text, FontWeight = FontWeights.Bold };
            var textBox = new TextBox { Tag = q.Id, AcceptsReturn = true, TextWrapping = TextWrapping.Wrap, Height = 60 };
            stackPanel.Children.Add(label);
            stackPanel.Children.Add(textBox);
            return new UserControl { Content = stackPanel };
        }

        private UserControl CreateSingleChoiceQuestionControl(Question q)
        {
            var stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(0, 0, 0, 20);
            var label = new Label { Content = q.Text, FontWeight = FontWeights.Bold };
            stackPanel.Children.Add(label);

            var group = new StackPanel();
            foreach (var option in q.Options)
            {
                var rb = new RadioButton { Content = option, Tag = q.Id };
                group.Children.Add(rb);
            }
            stackPanel.Children.Add(group);
            return new UserControl { Content = stackPanel };
        }

        private UserControl CreateMultipleChoiceQuestionControl(Question q)
        {
            var stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(0, 0, 0, 20);
            var label = new Label { Content = q.Text, FontWeight = FontWeights.Bold };
            stackPanel.Children.Add(label);

            var group = new StackPanel();
            foreach (var option in q.Options)
            {
                var cb = new CheckBox { Content = option, Tag = q.Id };
                group.Children.Add(cb);
            }
            stackPanel.Children.Add(group);
            return new UserControl { Content = stackPanel };
        }

        private UserControl CreateDefaultQuestionControl(Question q)
        {
            var stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(0, 0, 0, 20);
            var label = new Label { Content = $"[Неподдерживаемый тип вопроса: {q.Type}] {q.Text}", FontStyle = FontStyles.Italic };
            stackPanel.Children.Add(label);
            return new UserControl { Content = stackPanel };
        }

        private void Submit()
        {
            // СОБИРАЕМ ответы
            var collectedAnswers = new List<QuestionAnswer>();

            foreach (var (control, question) in _questionControlsAndQuestions)
            {
                var contentPanel = control.Content as StackPanel;
                if (contentPanel != null)
                {
                    var qAnswer = new QuestionAnswer { QuestionId = question.Id };

                    switch (question.Type)
                    {
                        case QuestionType.Text:
                            var textBox = contentPanel.Children.OfType<TextBox>().FirstOrDefault();
                            if (textBox != null)
                            {
                                qAnswer.TextAnswer = textBox.Text;
                            }
                            break;
                        case QuestionType.SingleChoice:
                            var radioButtonGroup = contentPanel.Children.OfType<StackPanel>().LastOrDefault(); // Второй StackPanel - группа RadioButton
                            if (radioButtonGroup != null)
                            {
                                var selectedRadioButton = radioButtonGroup.Children.OfType<RadioButton>().FirstOrDefault(rb => rb.IsChecked == true);
                                if (selectedRadioButton != null)
                                {
                                    qAnswer.TextAnswer = selectedRadioButton.Content.ToString();
                                }
                            }
                            break;
                        case QuestionType.MultipleChoice:
                            var checkBoxGroup = contentPanel.Children.OfType<StackPanel>().LastOrDefault(); // Второй StackPanel - группа CheckBox
                            if (checkBoxGroup != null)
                            {
                                var selectedCheckBoxes = checkBoxGroup.Children.OfType<CheckBox>().Where(cb => cb.IsChecked == true);
                                qAnswer.SelectedOptions = selectedCheckBoxes.Select(cb => cb.Content.ToString()).ToList();
                            }
                            break;
                    }
                    collectedAnswers.Add(qAnswer);
                }
            }

            // СОЗДАЁМ объект Answer
            var finalAnswer = new Answer
            {
                SurveyId = Survey.Id,
                QuestionAnswers = collectedAnswers
                // SubmissionTime будет установлен в AnswerService
            };

            // СОХРАНЯЕМ ответ
            _answerService.SaveAnswer(finalAnswer);

            // ПОКАЗЫВАЕМ сообщение
            MessageBox.Show("Ответы успешно отправлены и сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            // Окно закрывается в View (TakeSurveyWindow.xaml.cs)
        }
    }
}