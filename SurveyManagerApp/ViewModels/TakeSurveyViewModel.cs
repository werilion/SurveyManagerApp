using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SurveyManagerApp.Models;
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
        public ObservableCollection<UserControl> QuestionControls { get; set; }
        public ICommand SubmitCommand { get; }

        public TakeSurveyViewModel(Survey survey)
        {
            Survey = survey;
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
                QuestionControls.Add(control);
            }
        }

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

            foreach (var option in q.Options)
            {
                var cb = new CheckBox { Content = option, Tag = q.Id };
                stackPanel.Children.Add(cb);
            }
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
            // Логика сбора ответов...
            MessageBox.Show("Ответы успешно отправлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            // ViewModel не вызывает Close(). Это делает View.
        }
    }
}