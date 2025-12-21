using Microsoft.Extensions.DependencyInjection;
using SurveyManagerApp.Services;
using SurveyManagerApp.ViewModels;
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
            services.AddSingleton<SurveyService>();
            services.AddSingleton<AnswerService>();
            services.AddTransient<MainWindow>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<Views.EditSurveyWindow>();
            services.AddTransient<EditSurveyViewModel>();
            services.AddTransient<Views.TakeSurveyWindow>();
            services.AddTransient<TakeSurveyViewModel>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}