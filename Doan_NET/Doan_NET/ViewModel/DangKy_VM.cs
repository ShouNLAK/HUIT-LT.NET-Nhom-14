using Doan_NET.Helper;
using Doan_NET.View;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Doan_NET.ViewModel
{
    public class DangKy_VM : BaseViewModel
    {
        public ICommand LenhDangKy { get; }
        public ICommand LenhMoDangNhap { get; }

        public DangKy_VM()
        {
            LenhDangKy = new RelayCommand(thamSo => DangKy(thamSo as Window));
            LenhMoDangNhap = new RelayCommand(thamSo => MoDangNhap(thamSo as Window));
        }

        private void DangKy(Window cuaSoDangKy)
        {
            MessageBox.Show("Dang ky thanh cong (mo phong).", "Thong bao", MessageBoxButton.OK, MessageBoxImage.Information);
            var cuaSoDangNhap = new W_DangNhap();
            cuaSoDangNhap.Show();

            if (cuaSoDangKy == null)
            {
                cuaSoDangKy = Application.Current.Windows.OfType<W_DangKy>().FirstOrDefault();
            }

            cuaSoDangKy?.Close();
        }

        private void MoDangNhap(Window cuaSoDangKy)
        {
            var cuaSoDangNhap = new W_DangNhap();
            cuaSoDangNhap.Show();

            if (cuaSoDangKy == null)
            {
                cuaSoDangKy = Application.Current.Windows.OfType<W_DangKy>().FirstOrDefault();
            }

            cuaSoDangKy?.Close();
        }
    }
}
