using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for HangxeView.xaml
    /// </summary>
    public class HangXe
    {
        public string TenHang { get; set; }
        public string QuocGia { get; set; }
        public string LogoFullPath { get; set; }

    }
    public partial class HangxeView : UserControl
    {
        public ObservableCollection<HangXe> HangXeList { get; set; }
        
        public HangxeView()
        {
            InitializeComponent();
            this.DataContext = this;
            HangXeList = new ObservableCollection<HangXe>()
        {
            new HangXe { TenHang="Toyota", QuocGia="Japan", LogoFullPath = "https://via.placeholder.com/80" },
            new HangXe { TenHang="BMW", QuocGia="Germany", LogoFullPath = "https://via.placeholder.com/80"},
            new HangXe { TenHang="Mercedes", QuocGia="Germany",LogoFullPath = "https://via.placeholder.com/80"},
            new HangXe { TenHang="Honda", QuocGia="Japan", LogoFullPath = "https://via.placeholder.com/80"}
        };

            this.DataContext = this; // QUAN TRỌNG
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is HangXe brand)
            {
                DSXe carControl = new DSXe(brand.TenHang, brand.QuocGia);

                if (Window.GetWindow(this) is MainWindow mainWindow)
                {
                    mainWindow.MainContentControl.Content = carControl;
                }
            }
        }
    }
}
