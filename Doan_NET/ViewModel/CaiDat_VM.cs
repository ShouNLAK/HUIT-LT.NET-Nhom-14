using Doan_NET.Helper;
using Doan_NET.Model;
using System.Linq;
using System.Windows;

namespace Doan_NET.ViewModel
{
    // Tab Cài đặt: quản lý tài khoản admin đang đăng nhập (đổi mật khẩu).
    public class CaiDat_VM : BaseViewModel
    {
        private string tenDangNhap;
        public string TenDangNhap
        {
            get { return tenDangNhap; }
            set { tenDangNhap = value; OnPropertyChanged(); }
        }

        private string vaiTro;
        public string VaiTro
        {
            get { return vaiTro; }
            set { vaiTro = value; OnPropertyChanged(); }
        }

        private string tenNhanVien;
        public string TenNhanVien
        {
            get { return tenNhanVien; }
            set { tenNhanVien = value; OnPropertyChanged(); }
        }

        public System.Windows.Input.ICommand LenhDoiMatKhau { get; }

        public CaiDat_VM()
        {
            LenhDoiMatKhau = new RelayCommand(p => ThucHienDoiMatKhau(p as System.Windows.Controls.UserControl));
            TaiThongTinTaiKhoan();
        }

        private void ThucHienDoiMatKhau(System.Windows.Controls.UserControl uc)
        {
            if (uc == null) return;
            var mkCu = uc.FindName("MatKhauCuBox") as System.Windows.Controls.PasswordBox;
            var mkMoi = uc.FindName("MatKhauMoiBox") as System.Windows.Controls.PasswordBox;
            var xacNhan = uc.FindName("XacNhanBox") as System.Windows.Controls.PasswordBox;

            if (mkCu != null && mkMoi != null && xacNhan != null)
            {
                if (DoiMatKhau(mkCu.Password, mkMoi.Password, xacNhan.Password))
                {
                    mkCu.Clear();
                    mkMoi.Clear();
                    xacNhan.Clear();
                }
            }
        }

        private void TaiThongTinTaiKhoan()
        {
            string username = PhienDangNhap.TenDangNhap ?? string.Empty;
            TenDangNhap = username;
            VaiTro = PhienDangNhap.Role;
            TenNhanVien = "(không có)";

            using (var ctx = new QuanLyBanXeMayEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var tk = ctx.TaiKhoans.FirstOrDefault(t => t.Username == username);
                if (tk != null)
                {
                    VaiTro = tk.Role;
                    if (!string.IsNullOrEmpty(tk.MaNV))
                    {
                        var nv = ctx.NhanViens.FirstOrDefault(n => n.MaNV == tk.MaNV);
                        if (nv != null)
                        {
                            TenNhanVien = nv.HoTen;
                        }
                    }
                }
            }
        }

        // Trả về true nếu đổi mật khẩu thành công.
        public bool DoiMatKhau(string matKhauCu, string matKhauMoi, string xacNhan)
        {
            string username = PhienDangNhap.TenDangNhap ?? string.Empty;
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Không xác định được tài khoản đang đăng nhập.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(matKhauMoi) || matKhauMoi.Length < 3)
            {
                MessageBox.Show("Mật khẩu mới phải có ít nhất 3 ký tự.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (matKhauMoi != xacNhan)
            {
                MessageBox.Show("Xác nhận mật khẩu không khớp.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            using (var ctx = new QuanLyBanXeMayEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var tk = ctx.TaiKhoans.FirstOrDefault(t => t.Username == username);
                if (tk == null)
                {
                    MessageBox.Show("Không tìm thấy tài khoản trong hệ thống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                // Lối tắt admin/123 vẫn được chấp nhận như mật khẩu cũ hợp lệ.
                bool matKhauCuDung = tk.Password == matKhauCu ||
                    (string.Equals(username, "admin", System.StringComparison.OrdinalIgnoreCase) && matKhauCu == "123");
                if (!matKhauCuDung)
                {
                    MessageBox.Show("Mật khẩu cũ không đúng.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                tk.Password = matKhauMoi;
                ctx.SaveChanges();
            }

            MessageBox.Show("Đổi mật khẩu thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }
    }
}
