using Config.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SettingsManager;
using System;
using System.IO;

namespace FileDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            // create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // entry to run app
            serviceProvider.GetService<App>().Run();
        }

        /// <summary>
        /// Configuring the services for the application
        /// </summary>
        /// <param name="serviceCollection"></param>
        private static void ConfigureServices(IServiceCollection serviceCollection)
        {

            string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            ISettings settings = new ConfigurationBuilder<ISettings>()
                       .UseJsonFile(Path.Combine(basePath, "appsettings.json"))
                       .Build();
            settings = new ConfigurationBuilder<ISettings>()
                .UseJsonFile(Path.Combine(basePath, "appsettings.json"))
                .UseJsonFile(Path.Combine(basePath, $"appsettings.{settings.Environment}.json"))
                .Build();

            serviceCollection.AddSingleton(settings);

            var logConfiguration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(basePath, string.Format("appsettings.{0}.json", settings.Environment)))
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(logConfiguration)
                .Enrich.FromLogContext()
                .CreateLogger();

            // add app
            serviceCollection.AddTransient<App>();

        }

    }
}
