namespace Giaohangbot;

public partial class App : Application
{
    public App()
    {
        MainPage = new NavigationPage(new AuthPage(new AppService()));
    }
}
