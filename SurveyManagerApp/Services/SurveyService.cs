using Newtonsoft.Json;
using SurveyManagerApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SurveyManagerApp.Services
{
    public class SurveyService
    {
        private readonly string _dataFilePath;

        // Конструктор принимает путь к файлу данных
        public SurveyService(string dataFilePath = null)
        {
            _dataFilePath = dataFilePath ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "SurveyManager",
                "surveys.json"
            );

            var dir = Path.GetDirectoryName(_dataFilePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public List<Survey> GetAllSurveys()
        {
            if (!File.Exists(_dataFilePath))
            {
                return new List<Survey>();
            }

            var json = File.ReadAllText(_dataFilePath);
            var deserializedSurveys = JsonConvert.DeserializeObject<List<Survey>>(json);
            return deserializedSurveys ?? new List<Survey>();
        }

        public void SaveSurvey(Survey survey)
        {
            var surveys = GetAllSurveys();
            var existingSurvey = surveys.FirstOrDefault(s => s.Id == survey.Id);
            if (existingSurvey != null)
            {
                existingSurvey.Title = survey.Title;
                existingSurvey.Description = survey.Description;
                existingSurvey.Questions.Clear();
                foreach (var question in survey.Questions)
                {
                    existingSurvey.Questions.Add(question);
                }
            }
            else
            {
                int newId = surveys.Count > 0 ? surveys.Max(s => s.Id) + 1 : 1;
                survey.Id = newId;
                surveys.Add(survey);
            }
            var json = JsonConvert.SerializeObject(surveys, Formatting.Indented);
            File.WriteAllText(_dataFilePath, json);
        }

        public void DeleteSurvey(int id)
        {
            var surveys = GetAllSurveys().Where(s => s.Id != id).ToList();
            var json = JsonConvert.SerializeObject(surveys, Formatting.Indented);
            File.WriteAllText(_dataFilePath, json);
        }
    }
}