using Foundation;

namespace ios
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => Giaohangbot.MauiProgram.CreateMauiApp();
    }
}
