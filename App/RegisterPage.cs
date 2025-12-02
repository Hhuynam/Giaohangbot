using Microsoft.Extensions.Configuration;

namespace Giaohangbot;

public class RegisterPage : ContentPage
{
    private readonly AppService _appService;
    private Entry _usernameEntry;
    private Entry _passwordEntry;
    private Button _registerButton;

    // Constructor mặc định cho Shell
    public RegisterPage()
    {
        var config = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

        _appService = new AppService(config);
        BuildUI();
    }

    // Constructor có tham số nếu muốn inject AppService
    public RegisterPage(AppService appService)
    {
        _appService = appService;
        BuildUI();
    }

    private void BuildUI()
    {
        _usernameEntry = new Entry { Placeholder = "Username" };
        _passwordEntry = new Entry { Placeholder = "Password", IsPassword = true };

        _registerButton = new Button { Text = "Register" };
        _registerButton.Clicked += OnRegisterClicked;

        Content = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 10,
            Children =
            {
                _usernameEntry,
                _passwordEntry,
                _registerButton
            }
        };
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string username = _usernameEntry.Text;
        string password = _passwordEntry.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Please enter username and password", "OK");
            return;
        }

        // Luôn gán role = "User"
        string role = "User";

        await _appService.CreateAccountAsync(username, password, role);
        await DisplayAlert("Success", $"Account {username} created with role {role}", "OK");

        // Điều hướng về Auth bằng Shell
        await Shell.Current.GoToAsync("Auth");
    }
}
