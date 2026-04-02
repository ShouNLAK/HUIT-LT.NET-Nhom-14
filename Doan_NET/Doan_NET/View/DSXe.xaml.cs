using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
    public class Moto
    {
        public string TenDongXe { get; set; } // Đây nên là Tên xe cụ thể (VD: Wave Alpha)
        public string LoaiXe { get; set; }    // THÊM MỚI: Loại xe (VD: Xe số)
        public string GiaXe { get; set; }
        public string HinhAnhFullPath { get; set; }
        public string MoTa { get; set; }
    }
    public partial class DSXe : UserControl
    {
        public DSXe()
        {
            InitializeComponent();
        }
        public ObservableCollection<Moto> DSmoto { get; set; }
        public DSXe(string tenHang, string quocGia) : this()
        {

            if (TitleTextBlock != null)
            {
                TitleTextBlock.Text = "DANH SÁCH XE HÃNG " + tenHang.ToUpper();
            }
            DSmoto = new ObservableCollection<Moto>()
            {
                new Moto {
                    TenDongXe = "Honda Wave RSX",
                    LoaiXe = "Xe số",
                    GiaXe = "22.000.000 VNĐ",
                    HinhAnhFullPath = "https://via.placeholder.com/100",
                    MoTa = "Xe số bền bỉ, tiết kiệm xăng."
                },
                new Moto {
                    TenDongXe = "Honda Vision",
                    LoaiXe = "Xe ga",
                    GiaXe = "33.000.000 VNĐ",
                    HinhAnhFullPath = "https://via.placeholder.com/100",
                    MoTa = "Xe ga quốc dân, thiết kế thời trang."
                },
                new Moto {
                    TenDongXe = "Honda Winner X",
                    LoaiXe = "Xe tay côn",
                    GiaXe = "46.000.000 VNĐ",
                    HinhAnhFullPath = "https://via.placeholder.com/100",
                    MoTa = "Sức mạnh vượt trội, phong cách thể thao."
                }
            };

            CarListControl.ItemsSource = DSmoto;
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is Moto moto)
            {
                DisplayCarDetail(moto);
            }
        }

        private void DisplayCarDetail(Moto moto)
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
                Text = "Dòng xe: " + moto.LoaiXe,
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
