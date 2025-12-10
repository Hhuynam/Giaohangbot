using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls;
    using System.Collections.Generic;
    using System.IO;

    namespace Giaohangbot
    {
        public class ProductUI
        {
            public string Cargo_Id { get; set; }
            public string Name { get; set; }
            public string Price { get; set; }
            public ImageSource ProductImage { get; set; }
            public DateTime TimeUpload { get; set; }
        }

        public class HomePage : ContentPage
        {
            private readonly AppService _service;
            private readonly CollectionView _productList;

            public HomePage()
            {
                Title = "Home";


            _service = new AppService();


            ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Logout",
                Command = new Command(async () =>
                {
                    Application.Current.MainPage = new NavigationPage(new AuthPage(new AppService()));
                })
            });

            _productList = new CollectionView
                {
                    ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical),
                    ItemTemplate = new DataTemplate(() =>
                    {
                        var image = new Image { HeightRequest = 120, WidthRequest = 120, Aspect = Aspect.AspectFill };
                        image.SetBinding(Image.SourceProperty, "ProductImage");

                        var cargoLabel = new Label { FontSize = 14, TextColor = Colors.DarkGray, HorizontalTextAlignment = TextAlignment.Center };
                        cargoLabel.SetBinding(Label.TextProperty, "Cargo_Id");

                        var nameLabel = new Label { FontSize = 18, FontAttributes = FontAttributes.Bold, HorizontalTextAlignment = TextAlignment.Center };
                        nameLabel.SetBinding(Label.TextProperty, "Name");

                        var priceLabel = new Label { FontSize = 16, TextColor = Colors.Gray, HorizontalTextAlignment = TextAlignment.Center };
                        priceLabel.SetBinding(Label.TextProperty, "Price");

                        var payButton = new Button { Text = "Thanh toan" };
                        payButton.SetBinding(Button.CommandParameterProperty, ".");
                        payButton.Clicked += PayButton_Clicked;

                        return new Frame
                        {
                            Margin = 10,
                            Padding = 10,
                            CornerRadius = 8,
                            BorderColor = Colors.LightGray,
                            Content = new VerticalStackLayout
                            {
                                Spacing = 5,
                                Children = { image, cargoLabel, nameLabel, priceLabel, payButton }
                            }
                        };
                    })
                };

                Content = new ScrollView { Content = _productList };
                Appearing += HomePage_Appearing;
            }

            private async void PayButton_Clicked(object sender, EventArgs e)
            {
                var vm = (ProductUI)((Button)sender).CommandParameter;

                var product = new AppService.Product
                {
                    Cargo_Id = vm.Cargo_Id,
                    Name = vm.Name,
                    Price = double.Parse(vm.Price.Replace(" VND", "").Replace(",", "")),
                    TimeUpload = vm.TimeUpload
                };

                await _service.PublishOrderToMqttAsync(product);
                await Navigation.PushAsync(new PaymentPage(product));
            }

            private async void HomePage_Appearing(object sender, EventArgs e)
            {
                var products = await _service.GetAllProductsAsync();
                var viewModels = new List<ProductUI>();

                foreach (var p in products)
                {
                    var bytes = await _service.GetImageAsync(p.ImageId);
                    var imgSource = ImageSource.FromStream(() => new MemoryStream(bytes));

                    viewModels.Add(new ProductUI
                    {
                        Cargo_Id = p.Cargo_Id,
                        Name = p.Name,
                        Price = $"{p.Price:N0} VND",
                        ProductImage = imgSource,
                        TimeUpload = p.TimeUpload
                    });
                }

                _productList.ItemsSource = viewModels;
            }
        }
    }
