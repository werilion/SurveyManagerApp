using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyManagerApp.Models
{
    public class Answer
    {
        public int SurveyId { get; set; }
        public int QuestionId { get; set; }
        // Заменяем target-typed new на явное
        public List<string> TextAnswers { get; set; } = new List<string>(); // <-- Явно указан тип
        public string SelectedOption { get; set; } = string.Empty;
    }
}