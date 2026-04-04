using Doan_NET.Helper;
using Doan_NET.Model;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Doan_NET.ViewModel
{
    public class HoaDon_VM : BaseViewModel
    {
        private ObservableCollection<HoaDon> danhSachHoaDonHienThi;
        public ObservableCollection<HoaDon> DanhSachHoaDonHienThi
        {
            get { return danhSachHoaDonHienThi; }
            set
            {
                danhSachHoaDonHienThi = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MatHangGio_VM> GioHangHienTai
        {
            get { return DichVu_VM.GioHangDungChung; }
        }

        private string sdtKhachNhap;
        public string SDTKhachNhap
        {
            get { return sdtKhachNhap; }
            set
            {
                sdtKhachNhap = value;
                OnPropertyChanged();

                if (KhachHangDuocChon != null &&
                    !string.Equals(KhachHangDuocChon.SDT, (sdtKhachNhap ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    KhachHangDuocChon = null;
                }
            }
        }

        private KhachHang khachHangDuocChon;
        public KhachHang KhachHangDuocChon
        {
            get { return khachHangDuocChon; }
            set
            {
                khachHangDuocChon = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DiaChiKhachHang));
            }
        }

        private string tenNhanVienLap;
        public string TenNhanVienLap
        {
            get { return tenNhanVienLap; }
            set
            {
                tenNhanVienLap = value;
                OnPropertyChanged();
            }
        }

        private DateTime ngayLapNhap;
        public DateTime NgayLapNhap
        {
            get { return ngayLapNhap; }
            set
            {
                ngayLapNhap = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> DanhSachHinhThucThanhToan { get; }

        private string hinhThucThanhToanDangChon;
        public string HinhThucThanhToanDangChon
        {
            get { return hinhThucThanhToanDangChon; }
            set
            {
                hinhThucThanhToanDangChon = value;
                OnPropertyChanged();
            }
        }

        public int TongTienHang
        {
            get { return GioHangHienTai.Sum(item => item.ThanhTien); }
        }

        public int ThanhTienThanhToan
        {
            get { return TongTienHang; }
        }

        public string DiaChiKhachHang
        {
            get
            {
                if (KhachHangDuocChon == null || string.IsNullOrWhiteSpace(KhachHangDuocChon.DiaChi))
                {
                    return "Chưa có thông tin địa chỉ";
                }

                return KhachHangDuocChon.DiaChi;
            }
        }

        public ICommand LenhKiemTraSDTKhach { get; }
        public ICommand LenhHuyHoaDon { get; }
        public ICommand LenhXacNhanThanhToan { get; }

        public HoaDon_VM()
        {
            DanhSachHinhThucThanhToan = new ObservableCollection<string>
            {
                "Tiền mặt",
                "Chuyển khoản",
                "Trả góp"
            };

            HinhThucThanhToanDangChon = DanhSachHinhThucThanhToan.FirstOrDefault();
            NgayLapNhap = DateTime.Now;
            TenNhanVienLap = LayTenNhanVienMacDinh();

            LenhKiemTraSDTKhach = new RelayCommand(_ => KiemTraSDTKhach());
            LenhHuyHoaDon = new RelayCommand(_ => HuyHoaDon());
            LenhXacNhanThanhToan = new RelayCommand(_ => XacNhanThanhToan());

            DangKySuKienGioHang();
            TaiDanhSachHoaDonHienThi();
            CapNhatTienThanhToan();
        }

        private void DangKySuKienGioHang()
        {
            GioHangHienTai.CollectionChanged -= XuLyThayDoiGioHang;
            GioHangHienTai.CollectionChanged += XuLyThayDoiGioHang;

            foreach (MatHangGio_VM item in GioHangHienTai)
            {
                item.PropertyChanged -= XuLyThayDoiMatHang;
                item.PropertyChanged += XuLyThayDoiMatHang;
            }
        }

        private void XuLyThayDoiGioHang(object nguon, NotifyCollectionChangedEventArgs suKien)
        {
            if (suKien.NewItems != null)
            {
                foreach (MatHangGio_VM item in suKien.NewItems)
                {
                    item.PropertyChanged -= XuLyThayDoiMatHang;
                    item.PropertyChanged += XuLyThayDoiMatHang;
                }
            }

            if (suKien.OldItems != null)
            {
                foreach (MatHangGio_VM item in suKien.OldItems)
                {
                    item.PropertyChanged -= XuLyThayDoiMatHang;
                }
            }

            CapNhatTienThanhToan();
        }

        private void XuLyThayDoiMatHang(object nguon, PropertyChangedEventArgs suKien)
        {
            if (suKien.PropertyName == nameof(MatHangGio_VM.SoLuong) ||
                suKien.PropertyName == nameof(MatHangGio_VM.DonGia) ||
                suKien.PropertyName == nameof(MatHangGio_VM.ThanhTien))
            {
                CapNhatTienThanhToan();
            }
        }

        private void TaiDanhSachHoaDonHienThi()
        {
            DanhSachHoaDonHienThi = new ObservableCollection<HoaDon>(
                DuLieuHeThong.DanhSachHoaDon
                    .OrderByDescending(item => item.NgayLap)
                    .ThenByDescending(item => item.MaHD));
        }

        private void KiemTraSDTKhach()
        {
            if (!KiemTraChuoiSo(SDTKhachNhap, 10, 11))
            {
                MessageBox.Show("Số điện thoại khách phải từ 10 đến 11 chữ số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string sdt = SDTKhachNhap.Trim();
            KhachHang khachHang = DuLieuHeThong.DanhSachKhachHang.FirstOrDefault(item =>
                string.Equals(item.SDT, sdt, StringComparison.OrdinalIgnoreCase));

            if (khachHang == null)
            {
                KhachHangDuocChon = null;
                MessageBox.Show("Không tìm thấy khách hàng theo số điện thoại đã nhập.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            KhachHangDuocChon = khachHang;
        }

        private void HuyHoaDon()
        {
            bool coGioHang = GioHangHienTai.Count > 0;
            if (coGioHang)
            {
                MessageBoxResult ketQua = MessageBox.Show(
                    "Bạn có chắc muốn xóa toàn bộ giỏ hàng và làm mới thông tin thanh toán?",
                    "Xác nhận",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (ketQua != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            LamMoiThongTinHoaDon(coGioHang);
        }

        private void XacNhanThanhToan()
        {
            try
            {
                if (!KiemTraDuLieuThanhToan())
                {
                    return;
                }

                if (!KiemTraTonKhoTruocThanhToan())
                {
                    return;
                }

                string maHoaDonMoi = TaoMaHoaDonMoi();
                DateTime ngayLap = NgayLapNhap;

                foreach (MatHangGio_VM item in GioHangHienTai.ToList())
                {
                    DuLieuHeThong.DanhSachHoaDon.Add(new HoaDon
                    {
                        MaHD = maHoaDonMoi,
                        NgayLap = ngayLap,
                        TenNhanVien = TenNhanVienLap.Trim(),
                        TenKhachHang = KhachHangDuocChon.HoTen,
                        SDT = KhachHangDuocChon.SDT,
                        TenDV_SP = item.TenMatHang,
                        SoLuong = item.SoLuong,
                        ThanhTien = item.ThanhTien,
                        PhuongThucThanhToan = HinhThucThanhToanDangChon
                    });
                }

                CapNhatTonKhoSauThanhToan();

                GioHangHienTai.Clear();
                TaiDanhSachHoaDonHienThi();
                CapNhatTienThanhToan();

                MessageBox.Show("Thanh toán thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LamMoiThongTinHoaDon(false);
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể xác nhận thanh toán. Vui lòng thử lại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool KiemTraDuLieuThanhToan()
        {
            if (GioHangHienTai.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(SDTKhachNhap))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại khách hàng.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!KiemTraChuoiSo(SDTKhachNhap, 10, 11))
            {
                MessageBox.Show("Số điện thoại khách phải từ 10 đến 11 chữ số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (KhachHangDuocChon == null)
            {
                MessageBox.Show("Vui lòng kiểm tra số điện thoại để chọn đúng khách hàng.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(TenNhanVienLap))
            {
                MessageBox.Show("Tên nhân viên lập hóa đơn không được để trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (NgayLapNhap.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Ngày lập hóa đơn không hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(HinhThucThanhToanDangChon))
            {
                MessageBox.Show("Vui lòng chọn hình thức thanh toán.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private bool KiemTraTonKhoTruocThanhToan()
        {
            foreach (MatHangGio_VM item in GioHangHienTai)
            {
                if (!item.LaPhuTung)
                {
                    continue;
                }

                DichVuPhuTung phuTung = DuLieuHeThong.DanhSachDichVu.FirstOrDefault(dv =>
                    string.Equals(dv.MaPT, item.MaMatHang, StringComparison.OrdinalIgnoreCase));

                if (phuTung == null)
                {
                    MessageBox.Show("Không tìm thấy phụ tùng " + item.TenMatHang + " trong hệ thống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (item.SoLuong > phuTung.TonKho)
                {
                    MessageBox.Show("Phụ tùng " + item.TenMatHang + " không đủ tồn kho.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            return true;
        }

        private void CapNhatTonKhoSauThanhToan()
        {
            foreach (MatHangGio_VM item in GioHangHienTai)
            {
                if (!item.LaPhuTung)
                {
                    continue;
                }

                DichVuPhuTung phuTung = DuLieuHeThong.DanhSachDichVu.FirstOrDefault(dv =>
                    string.Equals(dv.MaPT, item.MaMatHang, StringComparison.OrdinalIgnoreCase));
                if (phuTung != null)
                {
                    phuTung.TonKho -= item.SoLuong;
                    if (phuTung.TonKho < 0)
                    {
                        phuTung.TonKho = 0;
                    }
                }
            }
        }

        private string TaoMaHoaDonMoi()
        {
            int maLonNhat = 0;
            foreach (HoaDon item in DuLieuHeThong.DanhSachHoaDon)
            {
                if (item == null || string.IsNullOrWhiteSpace(item.MaHD))
                {
                    continue;
                }

                string so = item.MaHD.Trim().ToUpper().Replace("HD", string.Empty);
                int maSo;
                if (int.TryParse(so, out maSo) && maSo > maLonNhat)
                {
                    maLonNhat = maSo;
                }
            }

            return "HD" + (maLonNhat + 1).ToString("000");
        }

        private string LayTenNhanVienMacDinh()
        {
            NhanVien nhanVienDangLam = DuLieuHeThong.DanhSachNhanVien.FirstOrDefault(item =>
                string.Equals(item.TrangThai, "Đang làm việc", StringComparison.OrdinalIgnoreCase));
            if (nhanVienDangLam != null)
            {
                return nhanVienDangLam.HoTen;
            }

            NhanVien nhanVienBatKy = DuLieuHeThong.DanhSachNhanVien.FirstOrDefault();
            if (nhanVienBatKy != null)
            {
                return nhanVienBatKy.HoTen;
            }

            return "Nhân viên hệ thống";
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

        private void LamMoiThongTinHoaDon(bool xoaGioHang)
        {
            if (xoaGioHang)
            {
                GioHangHienTai.Clear();
            }

            SDTKhachNhap = string.Empty;
            KhachHangDuocChon = null;
            NgayLapNhap = DateTime.Now;
            HinhThucThanhToanDangChon = DanhSachHinhThucThanhToan.FirstOrDefault();
            TenNhanVienLap = LayTenNhanVienMacDinh();
            CapNhatTienThanhToan();
        }

        private void CapNhatTienThanhToan()
        {
            OnPropertyChanged(nameof(TongTienHang));
            OnPropertyChanged(nameof(ThanhTienThanhToan));
        }
    }
}
