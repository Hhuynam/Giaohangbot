using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Threading.Tasks;

namespace Giaohangbot
{
    public class AdminPage : ContentPage
    {
        private readonly IConfiguration _config;

        private readonly WebView _videoStream;
        private readonly Editor _logBox;
        private readonly MqttService _mqttService;

        public AdminPage()
        {
            Title = "Admin Control";


            var streamUrl = "http://192.168.0.220/stream";


            _videoStream = new WebView
            {
                Source = streamUrl,
                HeightRequest = 240,
                WidthRequest = 320
            };

            var videoFrame = new Frame
            {
                Content = _videoStream,
                BorderColor = Colors.Gray,
                CornerRadius = 8,
                Padding = 2,
            };

            _logBox = new Editor
            {
                IsReadOnly = true,
                HeightRequest = 120,
                BackgroundColor = Colors.Black,
                TextColor = Colors.Lime,
                FontSize = 14
            };

            // Khởi tạo MQTT service
            _mqttService = new MqttService();


            // Các nút điều khiển
            var forwardBtn = CreateHoverButton("Forward", "forward");
            var backwardBtn = CreateHoverButton("Backward", "backward");
            var leftBtn = CreateHoverButton("Left", "left");
            var rightBtn = CreateHoverButton("Right", "right");
            var stopBtn = CreateHoverButton("Stop", "stop", Colors.Red, Colors.White);

            var buzzerBtn = CreateHoverButton("Buzzer Beep", "buzzer_beep");
            var relayOnBtn = CreateHoverButton("Relay ON", "relay_on");
            var relayOffBtn = CreateHoverButton("Relay OFF", "relay_off");
            var servoOnBtn = CreateHoverButton("Servo ON", "servo_on");
            var servoOffBtn = CreateHoverButton("Servo OFF", "servo_off");

            // Grid cho điều hướng
            var grid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }
                },
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                }
            };

            grid.Children.Add(forwardBtn); Grid.SetColumn(forwardBtn, 1); Grid.SetRow(forwardBtn, 0);
            grid.Children.Add(leftBtn); Grid.SetColumn(leftBtn, 0); Grid.SetRow(leftBtn, 1);
            grid.Children.Add(stopBtn); Grid.SetColumn(stopBtn, 1); Grid.SetRow(stopBtn, 1);
            grid.Children.Add(rightBtn); Grid.SetColumn(rightBtn, 2); Grid.SetRow(rightBtn, 1);
            grid.Children.Add(backwardBtn); Grid.SetColumn(backwardBtn, 1); Grid.SetRow(backwardBtn, 2);

            // Layout chính
            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = 20,
                    Spacing = 10,
                    Children =
                    {
                        new Label { Text = "ESP32-CAM Stream", FontSize = 20, HorizontalOptions = LayoutOptions.Center },
                        videoFrame,
                        new Label { Text = "IoT Control Panel", FontSize = 20, HorizontalOptions = LayoutOptions.Center },
                        grid,
                        buzzerBtn,
                        relayOnBtn,
                        relayOffBtn,
                        servoOnBtn,
                        servoOffBtn,
                        new Label { Text = "Command Log", FontSize = 18, HorizontalOptions = LayoutOptions.Center },
                        _logBox
                    }
                }
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                await _mqttService.ConnectAsync();
                _logBox.Text += $"[{DateTime.Now:HH:mm:ss}] MQTT connected.\n";
            }
            catch (Exception ex)
            {
                _logBox.Text += $"[{DateTime.Now:HH:mm:ss}] MQTT connect failed: {ex.Message}\n";
            }
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            await _mqttService.DisconnectAsync();
            _logBox.Text += $"[{DateTime.Now:HH:mm:ss}] MQTT disconnected.\n";
        }

        private Button CreateHoverButton(string text, string command, Color? bg = null, Color? fg = null)
        {
            var btn = new Button
            {
                Text = text,
                BackgroundColor = bg ?? Colors.LightGray,
                TextColor = fg ?? Colors.Black
            };

            btn.Clicked += async (s, e) => await SendCommand(command);

            // Visual states
            VisualStateManager.SetVisualStateGroups(btn, new VisualStateGroupList
            {
                new VisualStateGroup
                {
                    Name = "CommonStates",
                    States =
                    {
                        new VisualState
                        {
                            Name = "Normal",
                            Setters =
                            {
                                new Setter { Property = Button.BackgroundColorProperty, Value = bg ?? Colors.LightGray },
                                new Setter { Property = Button.TextColorProperty, Value = fg ?? Colors.Black }
                            }
                        },
                        new VisualState
                        {
                            Name = "PointerOver",
                            Setters =
                            {
                                new Setter { Property = Button.BackgroundColorProperty, Value = Colors.DarkGray },
                                new Setter { Property = Button.TextColorProperty, Value = Colors.White }
                            }
                        },
                        new VisualState
                        {
                            Name = "Pressed",
                            Setters =
                            {
                                new Setter { Property = Button.BackgroundColorProperty, Value = Colors.Black },
                                new Setter { Property = Button.TextColorProperty, Value = Colors.Yellow }
                            }
                        },
                        new VisualState
                        {
                            Name = "Disabled",
                            Setters =
                            {
                                new Setter { Property = Button.BackgroundColorProperty, Value = Colors.LightGray },
                                new Setter { Property = Button.TextColorProperty, Value = Colors.Gray }
                            }
                        }
                    }
                }
            });

            return btn;
        }

        private async Task SendCommand(string cmd)
        {
            _logBox.Text += $"[{DateTime.Now:HH:mm:ss}] Sent: {cmd}\n";

            if (_mqttService.IsConnected)
            {
                await _mqttService.PublishAsync(cmd);
            }
            else
            {
                _logBox.Text += $"[{DateTime.Now:HH:mm:ss}] MQTT not connected!\n";
            }
        }
    }
}
