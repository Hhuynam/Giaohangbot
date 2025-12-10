using Giaohangbot;

public partial class AppShell : Shell
{
    public AppShell()
    {

        Items.Add(new FlyoutItem
        {
            Title = "Home",
            Items =
            {
                new ShellContent { Title = "Home", Content = new HomePage() }
            }
        });


        Items.Add(new FlyoutItem
        {
            Title = "Seller",
            Items =
            {
                new ShellContent { Title = "Seller", Content = new SellerPage() }
            }
        });

        Items.Add(new FlyoutItem
        {
            Title = "Admin",
            Items =
            {
                new ShellContent { Title = "Admin", Content = new AdminPage() }
            }
        });
    }
}