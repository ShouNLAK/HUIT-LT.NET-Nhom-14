using Doan_NET.Helper;
using Doan_NET.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Net.Mail;

namespace Doan_NET.ViewModel
{
    public class KhachHang_VM : BaseViewModel
    {
        private ObservableCollection<KhachHang> danhSachKhachHang;
        public ObservableCollection<KhachHang> DanhSachKhachHang
        {
            get { return danhSachKhachHang; }
            set
            {
                danhSachKhachHang = value;
                OnPropertyChanged();
            }
        }

        private KhachHang khachHangDangChon;
        public KhachHang KhachHangDangChon
        {
            get { return khachHangDangChon; }
            set
            {
                khachHangDangChon = value;
                OnPropertyChanged();
                if (khachHangDangChon != null)
                {
                    MaKHNhap = khachHangDangChon.MaKH;
                    HoTenNhap = khachHangDangChon.HoTen;
                    SDTNhap = khachHangDangChon.SDT;
                    CCCDNhap = khachHangDangChon.CCCD;
                    EmailNhap = khachHangDangChon.Email;
                    DiaChiNhap = khachHangDangChon.DiaChi;
                    dangThemMoi = false;
                }
                lenhXoaKhachHang?.RaiseCanExecuteChanged();
            }
        }

        private string tuKhoaTimKiem;
        public string TuKhoaTimKiem
        {
            get { return tuKhoaTimKiem; }
            set
            {
                tuKhoaTimKiem = value;
                OnPropertyChanged();
            }
        }

        private string maKHNhap;
        public string MaKHNhap
        {
            get { return maKHNhap; }
            set
            {
                maKHNhap = value;
                OnPropertyChanged();
            }
        }

        private string hoTenNhap;
        public string HoTenNhap
        {
            get { return hoTenNhap; }
            set
            {
                hoTenNhap = value;
                OnPropertyChanged();
            }
        }

        private string sdtNhap;
        public string SDTNhap
        {
            get { return sdtNhap; }
            set
            {
                sdtNhap = value;
                OnPropertyChanged();
            }
        }

        private string cccdNhap;
        public string CCCDNhap
        {
            get { return cccdNhap; }
            set
            {
                cccdNhap = value;
                OnPropertyChanged();
            }
        }

        private string emailNhap;
        public string EmailNhap
        {
            get { return emailNhap; }
            set
            {
                emailNhap = value;
                OnPropertyChanged();
            }
        }

        private string diaChiNhap;
        public string DiaChiNhap
        {
            get { return diaChiNhap; }
            set
            {
                diaChiNhap = value;
                OnPropertyChanged();
            }
        }

        private bool dangThemMoi;
        private readonly RelayCommand lenhXoaKhachHang;

        public ICommand LenhThemKhachHang { get; }
        public ICommand LenhXoaKhachHang
        {
            get { return lenhXoaKhachHang; }
        }
        public ICommand LenhLuuKhachHang { get; }
        public ICommand LenhTimKiemKhachHang { get; }
        public ICommand LenhLamMoiKhachHang { get; }

        public KhachHang_VM()
        {
            LenhThemKhachHang = new RelayCommand(_ => ThemKhachHang());
            lenhXoaKhachHang = new RelayCommand(_ => XoaKhachHang(), _ => KhachHangDangChon != null);
            LenhLuuKhachHang = new RelayCommand(_ => LuuKhachHang());
            LenhTimKiemKhachHang = new RelayCommand(_ => TaiDanhSachKhachHang());
            LenhLamMoiKhachHang = new RelayCommand(_ => LamMoiNhapKhachHang());

            DanhSachKhachHang = new ObservableCollection<KhachHang>();
            TaiDanhSachKhachHang();
            LamMoiNhapKhachHang();
        }

        private void TaiDanhSachKhachHang()
        {
            IEnumerable<KhachHang> danhSachLoc = DuLieuHeThong.DanhSachKhachHang;

            if (!string.IsNullOrWhiteSpace(TuKhoaTimKiem))
            {
                string tuKhoa = TuKhoaTimKiem.Trim().ToLower();
                danhSachLoc = danhSachLoc.Where(item =>
                    (item.MaKH ?? string.Empty).ToLower().Contains(tuKhoa) ||
                    (item.HoTen ?? string.Empty).ToLower().Contains(tuKhoa) ||
                    (item.SDT ?? string.Empty).ToLower().Contains(tuKhoa));
            }

            DanhSachKhachHang = new ObservableCollection<KhachHang>(danhSachLoc);

            if (KhachHangDangChon != null)
            {
                string maDangChon = KhachHangDangChon.MaKH;
                KhachHangDangChon = DanhSachKhachHang.FirstOrDefault(item =>
                    string.Equals(item.MaKH, maDangChon, StringComparison.OrdinalIgnoreCase));
            }

            lenhXoaKhachHang.RaiseCanExecuteChanged();
        }

        private void ThemKhachHang()
        {
            dangThemMoi = true;
            MaKHNhap = TaoMaKhachHangMoi();
            HoTenNhap = string.Empty;
            SDTNhap = string.Empty;
            CCCDNhap = string.Empty;
            EmailNhap = string.Empty;
            DiaChiNhap = string.Empty;
            KhachHangDangChon = null;
        }

        private void XoaKhachHang()
        {
            try
            {
                if (KhachHangDangChon == null)
                {
                    return;
                }

                MessageBoxResult ketQua = MessageBox.Show("Bạn có chắc muốn xóa khách hàng đang chọn?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (ketQua != MessageBoxResult.Yes)
                {
                    return;
                }

                DuLieuHeThong.DanhSachKhachHang.Remove(KhachHangDangChon);
                KhachHangDangChon = null;
                LamMoiNhapKhachHang();
                TaiDanhSachKhachHang();
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể xóa khách hàng. Vui lòng thử lại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LuuKhachHang()
        {
            try
            {
                if (!KiemTraDuLieuKhachHang())
                {
                    return;
                }

                if (dangThemMoi)
                {
                    KhachHang khachHangTrung = DuLieuHeThong.DanhSachKhachHang.FirstOrDefault(item =>
                        string.Equals(item.MaKH, MaKHNhap, StringComparison.OrdinalIgnoreCase));
                    if (khachHangTrung != null)
                    {
                        MessageBox.Show("Mã khách hàng đã tồn tại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (KiemTraTrungThongTinKhachHang(null))
                    {
                        return;
                    }

                    var khachHangMoi = new KhachHang
                    {
                        MaKH = MaKHNhap.Trim().ToUpper(),
                        HoTen = HoTenNhap.Trim(),
                        SDT = SDTNhap.Trim(),
                        CCCD = CCCDNhap.Trim(),
                        Email = EmailNhap.Trim(),
                        DiaChi = DiaChiNhap.Trim()
                    };

                    DuLieuHeThong.DanhSachKhachHang.Add(khachHangMoi);
                    dangThemMoi = false;
                    TaiDanhSachKhachHang();
                    KhachHangDangChon = DanhSachKhachHang.FirstOrDefault(item => item.MaKH == khachHangMoi.MaKH);
                    return;
                }

                if (KhachHangDangChon == null)
                {
                    MessageBox.Show("Vui lòng chọn khách hàng để cập nhật.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                KhachHang khachHangTrungKhac = DuLieuHeThong.DanhSachKhachHang.FirstOrDefault(item =>
                    item != KhachHangDangChon &&
                    string.Equals(item.MaKH, MaKHNhap, StringComparison.OrdinalIgnoreCase));
                if (khachHangTrungKhac != null)
                {
                    MessageBox.Show("Mã khách hàng đã tồn tại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (KiemTraTrungThongTinKhachHang(KhachHangDangChon))
                {
                    return;
                }

                int viTri = DuLieuHeThong.DanhSachKhachHang.IndexOf(KhachHangDangChon);
                if (viTri < 0)
                {
                    KhachHang khachHangCanSua = DuLieuHeThong.DanhSachKhachHang.FirstOrDefault(item =>
                        string.Equals(item.MaKH, KhachHangDangChon.MaKH, StringComparison.OrdinalIgnoreCase));
                    viTri = khachHangCanSua != null ? DuLieuHeThong.DanhSachKhachHang.IndexOf(khachHangCanSua) : -1;
                }

                if (viTri >= 0)
                {
                    var khachHangMoi = new KhachHang
                    {
                        MaKH = MaKHNhap.Trim().ToUpper(),
                        HoTen = HoTenNhap.Trim(),
                        SDT = SDTNhap.Trim(),
                        CCCD = CCCDNhap.Trim(),
                        Email = EmailNhap.Trim(),
                        DiaChi = DiaChiNhap.Trim()
                    };

                    DuLieuHeThong.DanhSachKhachHang[viTri] = khachHangMoi;
                    TaiDanhSachKhachHang();
                    KhachHangDangChon = DanhSachKhachHang.FirstOrDefault(item => item.MaKH == khachHangMoi.MaKH);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể lưu dữ liệu khách hàng. Vui lòng thử lại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool KiemTraTrungThongTinKhachHang(KhachHang khachHangDangBoQua)
        {
            string sdt = SDTNhap.Trim();
            string cccd = CCCDNhap.Trim();
            string email = EmailNhap.Trim();

            KhachHang trungSDT = DuLieuHeThong.DanhSachKhachHang.FirstOrDefault(item =>
                item != khachHangDangBoQua &&
                string.Equals(item.SDT, sdt, StringComparison.OrdinalIgnoreCase));
            if (trungSDT != null)
            {
                MessageBox.Show("Số điện thoại đã tồn tại trong hệ thống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return true;
            }

            KhachHang trungCCCD = DuLieuHeThong.DanhSachKhachHang.FirstOrDefault(item =>
                item != khachHangDangBoQua &&
                string.Equals(item.CCCD, cccd, StringComparison.OrdinalIgnoreCase));
            if (trungCCCD != null)
            {
                MessageBox.Show("CCCD đã tồn tại trong hệ thống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return true;
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                KhachHang trungEmail = DuLieuHeThong.DanhSachKhachHang.FirstOrDefault(item =>
                    item != khachHangDangBoQua &&
                    string.Equals(item.Email, email, StringComparison.OrdinalIgnoreCase));
                if (trungEmail != null)
                {
                    MessageBox.Show("Email đã tồn tại trong hệ thống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return true;
                }
            }

            return false;
        }

        private bool KiemTraDuLieuKhachHang()
        {
            if (string.IsNullOrWhiteSpace(MaKHNhap))
            {
                MessageBox.Show("Mã khách hàng không được để trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(HoTenNhap))
            {
                MessageBox.Show("Họ tên không được để trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (HoTenNhap.Trim().Length < 2)
            {
                MessageBox.Show("Họ tên phải có ít nhất 2 ký tự.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!KiemTraChuoiSo(SDTNhap, 10, 11))
            {
                MessageBox.Show("Số điện thoại phải là 10 đến 11 chữ số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!KiemTraChuoiSo(CCCDNhap, 9, 12))
            {
                MessageBox.Show("CCCD phải từ 9 đến 12 chữ số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(DiaChiNhap))
            {
                MessageBox.Show("Địa chỉ không được để trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(EmailNhap))
            {
                try
                {
                    MailAddress diaChiMail = new MailAddress(EmailNhap.Trim());
                    if (!string.Equals(diaChiMail.Address, EmailNhap.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Email không hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }
                catch
                {
                    MessageBox.Show("Email không hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            return true;
        }

        private bool KiemTraChuoiSo(string duLieu, int doDaiMin, int doDaiMax)
        {
            if (string.IsNullOrWhiteSpace(duLieu))
            {
                return false;
            }

            string giaTri = duLieu.Trim();
            if (giaTri.Length < doDaiMin || giaTri.Length > doDaiMax)
            {
                return false;
            }

            foreach (char kyTu in giaTri)
            {
                if (kyTu < '0' || kyTu > '9')
                {
                    return false;
                }
            }

            return true;
        }

        private string TaoMaKhachHangMoi()
        {
            int maLonNhat = 0;
            foreach (KhachHang item in DuLieuHeThong.DanhSachKhachHang)
            {
                if (item == null || string.IsNullOrWhiteSpace(item.MaKH))
                {
                    continue;
                }

                string so = item.MaKH.Trim().ToUpper().Replace("KH", string.Empty);
                int maSo;
                if (int.TryParse(so, out maSo) && maSo > maLonNhat)
                {
                    maLonNhat = maSo;
                }
            }

            return "KH" + (maLonNhat + 1).ToString("000");
        }

        private void LamMoiNhapKhachHang()
        {
            dangThemMoi = false;
            MaKHNhap = string.Empty;
            HoTenNhap = string.Empty;
            SDTNhap = string.Empty;
            CCCDNhap = string.Empty;
            EmailNhap = string.Empty;
            DiaChiNhap = string.Empty;
        }
    }
}
