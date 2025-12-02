using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls;
using System;
using System.Linq;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace Giaohangbot
{
    public class PaymentPage : ContentPage
    {
        private Label statusLabel;
        private Entry qrContentEntry;
        private CameraBarcodeReaderView cameraView;
        private bool hasScannedOnce = false;
        private readonly AppService service;
        private readonly AppService.Product product;
        private readonly DateTime startTime;

        public PaymentPage(AppService.Product product)
        {
            Title = "Payment";
            var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            service = new AppService(config);

            this.product = product;
            startTime = DateTime.Now;

            cameraView = new CameraBarcodeReaderView
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };
            cameraView.BarcodesDetected += HandleQrCode;

            statusLabel = new Label
            {
                Text = "Waiting for QR scan...",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 16,
                TextColor = Colors.Gray
            };

            qrContentEntry = new Entry
            {
                Placeholder = "QR Content (EMVCo)",
                IsReadOnly = true,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 14
            };

            var retryButton = new Button
            {
                Text = "Retry",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            retryButton.Clicked += RetryScan;

            var copyButton = new Button
            {
                Text = "Copy QR",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            copyButton.Clicked += CopyQrContent;

            var gridLayout = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(3, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                }
            };

            gridLayout.Add(cameraView, 0, 0);
            gridLayout.Add(statusLabel, 0, 1);
            gridLayout.Add(qrContentEntry, 0, 2);
            gridLayout.Add(retryButton, 0, 3);
            gridLayout.Add(copyButton, 0, 4);

            Content = gridLayout;
        }

        private async void HandleQrCode(object sender, BarcodeDetectionEventArgs e)
        {
            if (hasScannedOnce) return;

            var qrValue = e.Results?.FirstOrDefault()?.Value;

            if (!string.IsNullOrEmpty(qrValue))
            {
                hasScannedOnce = true;
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    statusLabel.Text = "Scan successful!";
                    statusLabel.TextColor = Colors.Green;
                    qrContentEntry.Text = qrValue;
                });

                if (DateTime.Now - startTime > TimeSpan.FromMinutes(5))
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        statusLabel.Text = "Order expired!";
                        statusLabel.TextColor = Colors.Red;
                    });
                    return;
                }

                var orderId = ExtractOrderIdFromEmvco(qrValue);
                if (!string.IsNullOrEmpty(orderId) && orderId.Replace("_", "").Equals(product.Cargo_Id.Replace("_", ""), StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        await service.UpdatePaymentStatusAsync(product.Cargo_Id, true);

                        // Publish servo_on ngay khi xác nhận
                        await service.PublishMqttCommandAsync("servo_on");
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            statusLabel.Text = $"Order {product.Cargo_Id} confirmed as paid!\nCargo hatch OPENED";
                            statusLabel.TextColor = Colors.Green;
                        });

                        // Sau 15 giây tự động servo_off
                        _ = Task.Run(async () =>
                        {
                            await Task.Delay(TimeSpan.FromSeconds(15));
                            await service.PublishMqttCommandAsync("servo_off");
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                statusLabel.Text = $"Cargo {product.Cargo_Id} CLOSED hatch";
                                statusLabel.TextColor = Colors.Gray;
                            });
                        });
                    }
                    catch
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            statusLabel.Text = "Error updating payment!";
                            statusLabel.TextColor = Colors.Red;
                        });
                    }
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        statusLabel.Text = "QR does not match this order!";
                        statusLabel.TextColor = Colors.Red;
                    });
                }
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    statusLabel.Text = "Scan failed!";
                    statusLabel.TextColor = Colors.Red;
                });
            }
        }



        private void RetryScan(object sender, EventArgs e)
        {
            hasScannedOnce = false;
            statusLabel.Text = "Waiting for QR scan...";
            statusLabel.TextColor = Colors.Gray;
            qrContentEntry.Text = string.Empty;
            cameraView.IsDetecting = true;
        }

        private async void CopyQrContent(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(qrContentEntry.Text))
            {
                await Clipboard.SetTextAsync(qrContentEntry.Text);
                await DisplayAlert("Info", "QR content copied to clipboard", "OK");
            }
            else
            {
                await DisplayAlert("Info", "No QR content to copy", "OK");
            }
        }

        private string ExtractOrderIdFromEmvco(string payload)
        {
            int index = 0;
            while (index + 4 <= payload.Length)
            {
                string tag = payload.Substring(index, 2);
                int length = int.Parse(payload.Substring(index + 2, 2));
                if (index + 4 + length > payload.Length) break;
                string value = payload.Substring(index + 4, length);

                if (tag == "62")
                {
                    if (value.Contains("DH"))
                    {
                        int dhIndex = value.IndexOf("DH");
                        return value.Substring(dhIndex);
                    }
                }

                index += 4 + length;
            }
            return string.Empty;
        }
    }
}