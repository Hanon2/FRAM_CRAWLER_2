using Elcometer.Demo.Xamarin.Forms.PageModels;

namespace Elcometer.Demo.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("opensansregular.ttf", "OpenSansRegular");
                    fonts.AddFont("opensanssemibold.ttf", "OpenSansSemibold");
                });
            return builder.Build();
        }
    }
}