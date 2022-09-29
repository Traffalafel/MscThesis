using Microsoft.Extensions.DependencyInjection;
using MscThesis.Runner;
using MscThesis.UI.Pages;
using MscThesis.UI.ViewModels;
using SkiaSharp.Views.Maui.Controls.Hosting;

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

            builder.Services.AddSingleton<TestProvider>();
            builder.Services.AddTransient<ResultVM>();
            builder.Services.AddTransient<ResultPage>();

            return builder.Build();
        }
    }
}