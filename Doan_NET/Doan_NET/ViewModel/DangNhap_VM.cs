using Doan_NET.Helper;
using Doan_NET.View;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Doan_NET.ViewModel
{
    public class DangNhap_VM : BaseViewModel
    {
        public ICommand LenhDangNhap { get; }
        public ICommand LenhMoDangKy { get; }

        public DangNhap_VM()
        {
            LenhDangNhap = new RelayCommand(thamSo => DangNhap(thamSo as Window));
            LenhMoDangKy = new RelayCommand(thamSo => MoDangKy(thamSo as Window));
        }

        private void DangNhap(Window cuaSoDangNhap)
        {
            var cuaSoChinh = new Doan_NET.View.MainWindow();
            cuaSoChinh.Show();

            if (cuaSoDangNhap == null)
            {
                cuaSoDangNhap = Application.Current.Windows.OfType<W_DangNhap>().FirstOrDefault();
            }

            cuaSoDangNhap?.Close();
        }

        private void MoDangKy(Window cuaSoDangNhap)
        {
            var cuaSoDangKy = new W_DangKy();
            cuaSoDangKy.Show();

            if (cuaSoDangNhap == null)
            {
                cuaSoDangNhap = Application.Current.Windows.OfType<W_DangNhap>().FirstOrDefault();
            }

            cuaSoDangNhap?.Close();
        }
    }
}
