using ZXing.Net.Maui.Controls;  


namespace Giaohangbot;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.Services.AddSingleton<AppService>();
        builder.Services.AddTransient<AuthPage>();

        builder.UseMauiApp<App>();
        builder.UseBarcodeReader();

        return builder.Build();
    }
}