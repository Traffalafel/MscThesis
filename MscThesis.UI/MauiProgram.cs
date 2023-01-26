using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MscThesis.Framework;
using MscThesis.UI.Pages;
using MscThesis.UI.ViewModels;
using SkiaSharp.Views.Maui.Controls.Hosting;
using System.Reflection;

namespace MscThesis.UI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseSkiaSharp(true)
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            builder.Configuration.AddConfiguration(config);

            builder.Services.AddSingleton<TestProvider>();
            builder.Services.AddTransient<ResultVM>();
            builder.Services.AddTransient<ResultPage>();
            builder.Services.AddTransient<SetupPage>();

            return builder.Build();
        }
    }
}