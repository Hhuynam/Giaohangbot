namespace Giaohangbot;

public partial class App : Application
{
    public App()
    {
        
        MainPage = new AppShell();

        Shell.Current.GoToAsync("Auth");
    }
}
