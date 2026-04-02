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
using System.Windows.Shapes;

namespace Doan_NET.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MENU_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string tag = button.Tag?.ToString();
                LoadUserControl(tag);
            }
        }
        private void LoadUserControl(string viewName)
        {
            try
            {
                UserControl userControl = null;

                switch (viewName)
                {
                    
                    case "QuanLyXe":
                        userControl = new View.HangxeView();
                        break;
                    case "KhachHang":
                        //userControl = new KhachHangView();
                        break;
                    case "NhanVien":
                        //userControl = new NhanVienView();
                        break;
                    case "DonHang":
                        //userControl = new DonHangView();
                        break;
                    case "ThanhToan":
                        //userControl = new ThanhToanView();
                        break;
                    case "ThongKe":
                        //userControl = new ThongKeView();
                        break;
                    case "CaiDat":
                        //userControl = new CaiDatView();
                        break;
                    default:
                        //userControl = new TongQuanView();
                        break;
                }

                MainContentControl.Content = userControl;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải view: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
