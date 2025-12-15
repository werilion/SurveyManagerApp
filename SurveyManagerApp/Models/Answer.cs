using System;
using System.Collections.Generic;

namespace SurveyManagerApp.Models
{
    // Модель для хранения ответа на один вопрос
    public class QuestionAnswer
    {
        public int QuestionId { get; set; }
        // Для текста и одиночного выбора
        public string TextAnswer { get; set; } = string.Empty;
        // Для множественного выбора
        public List<string> SelectedOptions { get; set; } = new List<string>();
    }

    // Модель для хранения всех ответов на один опрос от одного пользователя (или сеанса)
    public class Answer
    {
        public int Id { get; set; } // Уникальный ID для ответа
        public int SurveyId { get; set; } // Ссылка на опрос, на который отвечали
        public DateTime SubmissionTime { get; set; } = DateTime.Now; // Время отправки (для статистики)
        public List<QuestionAnswer> QuestionAnswers { get; set; } = new List<QuestionAnswer>(); // Коллекция ответов на вопросы
        
    }
}