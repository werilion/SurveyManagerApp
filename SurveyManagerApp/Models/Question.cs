using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace SurveyManagerApp.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty; // Это свойство с инициализацией, но не использует target-typed new
        public QuestionType Type { get; set; }
        // Заменяем target-typed new на явное
        public ObservableCollection<string> Options { get; set; } = new ObservableCollection<string>(); // <-- Явно указан тип
    }

    public enum QuestionType
    {
        Text,
        SingleChoice,
        MultipleChoice
    }
}
