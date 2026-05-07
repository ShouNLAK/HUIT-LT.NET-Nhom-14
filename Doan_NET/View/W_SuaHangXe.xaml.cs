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
    /// Interaction logic for W_SuaHangXe.xaml
    /// </summary>
    public partial class W_SuaHangXe : Window
    {
        public W_SuaHangXe()
        {
            InitializeComponent();
        }

        private void ChonLogo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Vui lòng nhập đường dẫn logo vào ô bên cạnh.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LogoTextBox.Focus();
        }
    }
}
