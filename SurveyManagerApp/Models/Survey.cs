using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace SurveyManagerApp.Models
{
    public class Survey
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        // Заменяем target-typed new на явное
        public ObservableCollection<Question> Questions { get; set; } = new ObservableCollection<Question>(); // <-- Явно указан тип
    }
}