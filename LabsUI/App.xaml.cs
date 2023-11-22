using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using LabsUI.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace LabsUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;
        public App()
        {
            ConfigureHost();

            // Hook up the event handlers for unhandled exceptions
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception != null)
            {
                LogException(e.Exception); // Log the exception
                e.Handled = true; // Mark the exception as handled to prevent application termination
                MessageBox.Show(e.Exception.Message);
            }
        }
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            LogException(e.Exception);
            e.SetObserved(); // Mark the exception as observed to prevent the runtime from terminating the process
            MessageBox.Show(e.Exception.Message);
        }
        private void LogException(Exception ex)
        {
            var nLogService = _host.Services.GetRequiredService<INLogService>();
            nLogService.LogError(ex, ex.Message); // Log the exception using your preferred logging mechanism (NLog, log4net, etc.)
        }

        private void ConfigureHost()
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(System.IO.Directory.GetCurrentDirectory())
               //.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .Build();

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Configure NLog
                    services.AddLogging(builder =>
                    {
                        builder.ClearProviders();
                        builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                        builder.AddNLog(configuration);
                    });

                    // Register NLogService
                    services.AddSingleton<INLogService, NLogService>();
                    services.AddTransient<MainWindow>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            // Instead of StartupUri="Views/MainWindow.xaml", use OnStartup to prepare for Dependency Injection..
            //MainWindow win = new MainWindow();
            //win.Show();

            //base.OnStartup(e);

            await _host.StartAsync();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }

            base.OnExit(e);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
        }
    }
}
