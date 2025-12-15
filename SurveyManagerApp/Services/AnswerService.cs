using Newtonsoft.Json;
using SurveyManagerApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SurveyManagerApp.Services
{
    public class AnswerService
    {
        private readonly string _answersDataFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SurveyManager",
            "answers.json"
        );

        public AnswerService()
        {
            var dir = Path.GetDirectoryName(_answersDataFilePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public List<Answer> GetAllAnswers()
        {
            if (!File.Exists(_answersDataFilePath))
            {
                return new List<Answer>();
            }

            var json = File.ReadAllText(_answersDataFilePath);
            var deserializedAnswers = JsonConvert.DeserializeObject<List<Answer>>(json);
            return deserializedAnswers ?? new List<Answer>();
        }

        public void SaveAnswer(Answer answer)
        {
            var answers = GetAllAnswers();
            // Генерируем ID для нового ответа
            int newId = answers.Count > 0 ? answers.Max(a => a.Id) + 1 : 1;
            answer.Id = newId;
            answers.Add(answer);
            var json = JsonConvert.SerializeObject(answers, Formatting.Indented);
            File.WriteAllText(_answersDataFilePath, json);
        }

        // Опционально: метод для удаления ответа
        public void DeleteAnswer(int id)
        {
            var answers = GetAllAnswers().Where(a => a.Id != id).ToList();
            var json = JsonConvert.SerializeObject(answers, Formatting.Indented);
            File.WriteAllText(_answersDataFilePath, json);
        }
    }
}