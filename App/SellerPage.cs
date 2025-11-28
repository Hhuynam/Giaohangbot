using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace Giaohangbot
{
    public class SellerPage : ContentPage
    {
        private readonly AppService _service;

        public SellerPage()
        {
            Title = "Người bán";

            _service = new AppService();

            var nameEntry = new Entry { Placeholder = "Tên sản phẩm" };
            var priceEntry = new Entry { Placeholder = "Giá sản phẩm", Keyboard = Keyboard.Numeric };
            var chooseImageButton = new Button { Text = "Chọn ảnh (PNG/JPG/JPEG)" };
            var postProductButton = new Button { Text = "Đăng sản phẩm" };

            string? selectedImagePath = null;

            var imageFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".png", ".jpg", ".jpeg" } },
                    { DevicePlatform.Android, new[] { "image/png", "image/jpeg" } },
                    { DevicePlatform.iOS, new[] { "public.png", "public.jpeg" } }
                });

            // Chọn ảnh
            chooseImageButton.Clicked += async (s, e) =>
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = imageFileType,
                    PickerTitle = "Chọn ảnh sản phẩm (PNG/JPG/JPEG)"
                });
                if (result != null)
                    selectedImagePath = result.FullPath;
            };

            // Đăng sản phẩm
            postProductButton.Clicked += async (s, e) =>
            {
                if (selectedImagePath == null)
                {
                    await DisplayAlert("Lỗi", "Chưa chọn ảnh", "OK");
                    return;
                }

                var imageId = await _service.SaveImageAsync(selectedImagePath);

                var product = new AppService.Product
                {
                    Name = nameEntry.Text,
                    Price = double.TryParse(priceEntry.Text, out var p) ? p : 0,
                    ImageId = imageId
                };

                await _service.SaveProductAsync(product);

                await DisplayAlert("Thành công", $"Đã lưu sản phẩm {product.Name} với mã {product.Cargo_Id}", "OK");

                // Reset form
                nameEntry.Text = string.Empty;
                priceEntry.Text = string.Empty;
                selectedImagePath = null;
            };

            // Layout
            Content = new VerticalStackLayout
            {
                Padding = 20,
                Spacing = 15,
                Children =
                {
                    nameEntry,
                    priceEntry,
                    chooseImageButton,
                    postProductButton
                }
            };
        }
    }
}
