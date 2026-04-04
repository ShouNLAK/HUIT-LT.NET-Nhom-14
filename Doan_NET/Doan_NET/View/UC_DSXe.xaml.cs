using Doan_NET.Model;
using Doan_NET.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Doan_NET.View
{
    /// <summary>
    /// Interaction logic for UC_DSXe.xaml
    /// </summary>
    public partial class UC_DSXe : UserControl
    {
        public UC_DSXe()
        {
            InitializeComponent();
            if (DataContext == null)
            {
                DataContext = new Xe_VM();
            }
        }

        public UC_DSXe(string brandName, string brandCountry) : this()
        {
            if (!string.IsNullOrEmpty(brandName))
            {
                TitleTextBlock.Text = $"DANH SÁCH XE - {brandName}";
            }
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is MoTo moto)
            {
                DisplayCarDetail(moto);
            }
        }

        private void DisplayCarDetail(MoTo moto)
        {
            DetailPanel.Children.Clear();
            DetailPanel.Children.Add(new Image
            {
                Source = new BitmapImage(new Uri(moto.HinhAnhFullPath, UriKind.RelativeOrAbsolute)),
                Height = 180,
                Stretch = Stretch.Uniform,
                Margin = new Thickness(0, 0, 0, 20)
            });

            DetailPanel.Children.Add(new TextBlock
            {
                Text = moto.TenDongXe,
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            });

            DetailPanel.Children.Add(new TextBlock
            {
                Text = "Loại xe: " + moto.LoaiXe,
                FontSize = 14,
                Foreground = Brushes.DimGray,
                Margin = new Thickness(0, 0, 0, 15)
            });
            DetailPanel.Children.Add(new TextBlock
            {
                Text = "Năm sản xuất: " + moto.NamSX,
                FontSize = 14,
                Foreground = Brushes.DimGray,
                Margin = new Thickness(0, 0, 0, 15)
            });
            DetailPanel.Children.Add(new TextBlock
            {
                Text = "MÔ TẢ CHI TIẾT",
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Gray,
                Margin = new Thickness(0, 5, 0, 5)
            });

            DetailPanel.Children.Add(new TextBlock
            {
                Text = moto.MoTa,
                FontSize = 13,
                TextWrapping = TextWrapping.Wrap,
                Foreground = new SolidColorBrush(Color.FromRgb(52, 73, 94)),
                FontStyle = FontStyles.Italic,
                Margin = new Thickness(0, 0, 0, 15)
            });

            DetailPanel.Children.Add(new TextBlock
            {
                Text = "Giá: " + moto.GiaXe,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Red,
                Margin = new Thickness(0, 0, 0, 20)
            });

            DetailPanel.Children.Add(new TextBlock
            {
                Text = "CHỌN MÀU SẮC",
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            });

            WrapPanel colorPanel = new WrapPanel();

            colorPanel.Children.Add(CreateColorBox(Colors.Gray));
            colorPanel.Children.Add(CreateColorBox(Colors.Black));
            colorPanel.Children.Add(CreateColorBox(Colors.White));

            DetailPanel.Children.Add(colorPanel);

            DetailPanel.Children.Add(new TextBlock
            {
                Text = "Tồn kho: 5 chiếc",
                Foreground = Brushes.Green,
                Margin = new Thickness(0, 10, 0, 10)
            });

            Button buyButton = new Button
            {
                Content = "BÁN NGAY",
                Background = new SolidColorBrush(Color.FromRgb(231, 76, 60)),
                Foreground = Brushes.White,
                Padding = new Thickness(10),
                BorderThickness = new Thickness(0)
            };

            DetailPanel.Children.Add(buyButton);
        }

        private Button CreateColorBox(Color color)
        {
            return new Button
            {
                Width = 40,
                Height = 40,
                Background = new SolidColorBrush(color),
                Margin = new Thickness(5),
                BorderThickness = new Thickness(1)
            };
        }
    }
}
