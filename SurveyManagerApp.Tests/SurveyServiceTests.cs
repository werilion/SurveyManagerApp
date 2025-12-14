using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using SurveyManagerApp.Models;
using SurveyManagerApp.Services;
using System.Collections.Generic;
using System.IO;

namespace SurveyManagerApp.Tests
{
    [TestClass]
    public class SurveyServiceTests
    {
        private string _testFilePath;
        private SurveyService _surveyService;

        [TestInitialize]
        public void Setup()
        {
            // Используем уникальный путь для тестов
            _testFilePath = Path.Combine(Path.GetTempPath(), "SurveyManagerTest", "surveys_test.json");
            var directory = Path.GetDirectoryName(_testFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Передаём тестовый путь в конструктор SurveyService
            _surveyService = new SurveyService(_testFilePath);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
            var directory = Path.GetDirectoryName(_testFilePath);
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetAllSurveys_WhenFileDoesNotExist_ReturnsEmptyList()
        {
            // Arrange: файл не существует (мы его удалили в Cleanup)

            // Act
            var result = _surveyService.GetAllSurveys();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetAllSurveys_WhenFileExists_ReturnsCorrectSurveys()
        {
            // Arrange
            var surveysToSave = new List<Survey>
            {
                new Survey { Id = 1, Title = "Test Survey 1", Description = "Desc 1" },
                new Survey { Id = 2, Title = "Test Survey 2", Description = "Desc 2" }
            };
            // ЯВНО указываем Newtonsoft.Json.Formatting
            var json = JsonConvert.SerializeObject(surveysToSave, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(_testFilePath, json);

            // Act
            var result = _surveyService.GetAllSurveys();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Test Survey 1", result[0].Title);
            Assert.AreEqual("Test Survey 2", result[1].Title);
        }

        [TestMethod]
        public void SaveSurvey_NewSurvey_AddsSurveyWithCorrectId()
        {
            // Arrange
            var survey = new Survey { Title = "New Survey", Description = "New Desc", Questions = new System.Collections.ObjectModel.ObservableCollection<Question>() };

            // Act
            _surveyService.SaveSurvey(survey);

            // Assert
            var savedSurveys = _surveyService.GetAllSurveys();
            Assert.AreEqual(1, savedSurveys.Count);
            Assert.AreEqual("New Survey", savedSurveys[0].Title);
            Assert.AreEqual(1, savedSurveys[0].Id); // Предполагаем, что ID генерируется как Max + 1, начиная с 1
        }

        [TestMethod]
        public void SaveSurvey_ExistingSurvey_UpdatesSurvey()
        {
            // Arrange
            var survey = new Survey { Id = 1, Title = "Old Title", Description = "Old Desc", Questions = new System.Collections.ObjectModel.ObservableCollection<Question>() };
            _surveyService.SaveSurvey(survey); // Сохраняем первый раз

            // Изменяем
            survey.Title = "Updated Title";
            survey.Description = "Updated Desc";

            // Act
            _surveyService.SaveSurvey(survey); // Сохраняем второй раз

            // Assert
            var savedSurveys = _surveyService.GetAllSurveys();
            Assert.AreEqual(1, savedSurveys.Count);
            Assert.AreEqual("Updated Title", savedSurveys[0].Title);
            Assert.AreEqual("Updated Desc", savedSurveys[0].Description);
        }

        [TestMethod]
        public void DeleteSurvey_RemovesSurveyFromList()
        {
            // Arrange
            var survey1 = new Survey { Id = 1, Title = "Survey 1", Description = "Desc 1", Questions = new System.Collections.ObjectModel.ObservableCollection<Question>() };
            var survey2 = new Survey { Id = 2, Title = "Survey 2", Description = "Desc 2", Questions = new System.Collections.ObjectModel.ObservableCollection<Question>() };
            _surveyService.SaveSurvey(survey1);
            _surveyService.SaveSurvey(survey2);

            // Act
            _surveyService.DeleteSurvey(1);

            // Assert
            var savedSurveys = _surveyService.GetAllSurveys();
            Assert.AreEqual(1, savedSurveys.Count);
            Assert.AreEqual(2, savedSurveys[0].Id);
        }
    }
}