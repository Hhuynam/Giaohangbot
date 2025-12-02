namespace Giaohangbot;

public partial class AppShell : Shell
{
    private FlyoutItem _adminItem;

    public AppShell()
    {
        Routing.RegisterRoute("Auth", typeof(AuthPage));
        Routing.RegisterRoute("Register", typeof(RegisterPage));

        Items.Add(new FlyoutItem
        {
            Title = "Home",
            Items =
            {
                new ShellContent { Title = "Home", Route = "Home", Content = new HomePage() }
            }
        });

        Items.Add(new FlyoutItem
        {
            Title = "Seller",
            Items =
            {
                new ShellContent { Title = "Seller", Route = "Seller", Content = new SellerPage() }
            }
        });

        // Admin mặc định ẩn
        _adminItem = new FlyoutItem
        {
            Title = "Admin",
            IsVisible = false,
            Items =
            {
                new ShellContent { Title = "Admin", Route = "Admin", Content = new AdminPage() }
            }
        };

        Items.Add(_adminItem);
    }

    // Hàm bật/tắt menu Admin
    public void SetAdminVisibility(bool isVisible)
    {
        _adminItem.IsVisible = isVisible;
    }
}
