namespace Giaohangbot;

public class AuthPage : ContentPage
{
    private readonly AppService _appService;
    private Entry _usernameEntry;
    private Entry _passwordEntry;
    private Button _loginButton;

    public AuthPage(AppService appService)
    {
        _appService = appService;

        _usernameEntry = new Entry { Placeholder = "Username" };
        _passwordEntry = new Entry { Placeholder = "Password", IsPassword = true };
        _loginButton = new Button { Text = "Login" };
        _loginButton.Clicked += OnLoginClicked;

        var registerLabel = new Label
        {
            Text = "Don't have an account? Register here",
            TextColor = Colors.Blue,
            HorizontalOptions = LayoutOptions.Center
        };

        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnRegisterTapped;
        registerLabel.GestureRecognizers.Add(tapGesture);

        Content = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 10,
            Children =
            {
                _usernameEntry,
                _passwordEntry,
                _loginButton,
                registerLabel
            }
        };
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = _usernameEntry.Text;
        string password = _passwordEntry.Text;

        var account = await _appService.LoginAsync(username, password);

        if (account != null)
        {
            if (_appService.IsAdmin(account))
            {
                await DisplayAlert("Login", "Welcome Admin!", "OK");

                // Bật menu Admin trong AppShell
                (Application.Current.MainPage as AppShell)?.SetAdminVisibility(true);

                // Điều hướng về Home, Admin sẽ hiện trong menu
                await Shell.Current.GoToAsync("//Home");
            }
            else
            {
                await DisplayAlert("Login", "Welcome User!", "OK");

                // Ẩn menu Admin nếu không phải admin
                (Application.Current.MainPage as AppShell)?.SetAdminVisibility(false);

                await Shell.Current.GoToAsync("//Home");
            }
        }
        else
        {
            await DisplayAlert("Login Failed", "Invalid username or password", "OK");
        }
    }


    private async void OnRegisterTapped(object sender, EventArgs e)
    {
        // Điều hướng sang trang đăng ký bằng Shell
        await Shell.Current.GoToAsync("Register");
    }
}
