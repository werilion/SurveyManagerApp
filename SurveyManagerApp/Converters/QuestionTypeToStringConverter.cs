using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurveyManagerApp.Models;
using System.Globalization;
using System.Windows.Data;

namespace SurveyManagerApp.Converters
{
    public class QuestionTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is QuestionType type)
            {
                switch (type)
                {
                    case QuestionType.Text:
                        return "Текст";
                    case QuestionType.SingleChoice:
                        return "Один выбор";
                    case QuestionType.MultipleChoice:
                        return "Множественный выбор";
                    default:
                        return type.ToString(); // Возвратить строку, если тип неизвестен
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
