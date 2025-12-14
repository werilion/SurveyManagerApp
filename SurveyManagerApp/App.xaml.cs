using Microsoft.Extensions.DependencyInjection;
using SurveyManagerApp.Services;
using SurveyManagerApp.ViewModels;
using SurveyManagerApp.Views;
using System;
using System.Windows;

namespace SurveyManagerApp
{
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Зарегистрируйте сервисы
            services.AddSingleton<SurveyService>(provider => new SurveyService());
            // Зарегистрируйте ViewModel
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<MainWindow>(); // Зарегистрируйте Window как зависимость
            services.AddTransient<EditSurveyWindow>();
            services.AddTransient<EditSurveyViewModel>();
            services.AddTransient<TakeSurveyWindow>();
            services.AddTransient<TakeSurveyViewModel>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Загружаем MainWindow через DI
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}