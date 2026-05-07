using Doan_NET.Helper;
using Doan_NET.Model;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Doan_NET.ViewModel
{
    public class HoaDon_VM : BaseViewModel
    {
        private ObservableCollection<HoaDon_HienThi_VM> danhSachHoaDonHienThi;
        public ObservableCollection<HoaDon_HienThi_VM> DanhSachHoaDonHienThi
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
                    KhachHangDuocChon.SDT != (sdtKhachNhap ?? string.Empty).Trim())
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
            using (var ctx = new QuanLyBanXeMayEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var ds = ctx.HoaDons
                    .Include("NhanVien")
                    .Include("KhachHang")
                    .OrderByDescending(item => item.NgayLap)
                    .ThenByDescending(item => item.MaHD)
                    .ToList();

                var danhSachHienThi = ds.Select(item => new HoaDon_HienThi_VM
                {
                    MaHD = item.MaHD,
                    NgayLap = item.NgayLap,
                    TenNhanVien = item.NhanVien != null ? item.NhanVien.HoTen : string.Empty,
                    TenKhachHang = item.KhachHang != null ? item.KhachHang.HoTen : string.Empty,
                    SDT = item.KhachHang != null ? item.KhachHang.SDT : string.Empty,
                    TenDV_SP = item.TenDV_SP,
                    SoLuong = item.SoLuong,
                    ThanhTien = item.ThanhTien,
                    PhuongThucThanhToan = item.PhuongThucThanhToan
                }).ToList();

                DanhSachHoaDonHienThi = new ObservableCollection<HoaDon_HienThi_VM>(danhSachHienThi);
            }
        }

        private void KiemTraSDTKhach()
        {
            if (!KiemTraChuoiSo(SDTKhachNhap, 10, 11))
            {
                MessageBox.Show("Số điện thoại khách phải từ 10 đến 11 chữ số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string sdt = SDTKhachNhap.Trim();

            using (var ctx = new QuanLyBanXeMayEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var khachHang = ctx.KhachHangs.FirstOrDefault(item =>
                    item.SDT == sdt);
                if (khachHang == null)
                {
                    KhachHangDuocChon = null;
                    MessageBox.Show("Không tìm thấy khách hàng theo số điện thoại đã nhập.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                KhachHangDuocChon = khachHang;
            }
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

            using (var ctx = new QuanLyBanXeMayEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var nv = ctx.NhanViens.FirstOrDefault(n => n.HoTen == TenNhanVienLap.Trim());
                string maNV = nv != null ? nv.MaNV : null;
                string maKH = KhachHangDuocChon != null ? KhachHangDuocChon.MaKH : null;

                foreach (MatHangGio_VM item in GioHangHienTai.ToList())
                {
                    var hd = new HoaDon
                    {
                        MaHD = maHoaDonMoi,
                        NgayLap = ngayLap,
                        MaNV = maNV,
                        MaKH = maKH,
                        TenDV_SP = item.TenMatHang,
                        SoLuong = item.SoLuong,
                        ThanhTien = item.ThanhTien,
                        PhuongThucThanhToan = HinhThucThanhToanDangChon
                    };
                    ctx.HoaDons.Add(hd);
                }

                ctx.SaveChanges();
            }

            CapNhatTonKhoSauThanhToan();

            GioHangHienTai.Clear();
            TaiDanhSachHoaDonHienThi();
            CapNhatTienThanhToan();

            MessageBox.Show("Thanh toán thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LamMoiThongTinHoaDon(false);
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

                using (var ctx = new QuanLyBanXeMayEntities())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    var phuTung = ctx.DichVuPhuTungs.FirstOrDefault(dv =>
                        dv.MaPT == item.MaMatHang);
                    if (phuTung == null)
                    {
                        MessageBox.Show("Không tìm thấy phụ tùng " + item.TenMatHang + " trong hệ thống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }

                    if (item.SoLuong > (phuTung.TonKho ?? 0))
                    {
                        MessageBox.Show("Phụ tùng " + item.TenMatHang + " không đủ tồn kho.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
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

                using (var ctx = new QuanLyBanXeMayEntities())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    var phuTung = ctx.DichVuPhuTungs.FirstOrDefault(dv =>
                        dv.MaPT == item.MaMatHang);
                    if (phuTung != null)
                    {
                        phuTung.TonKho = (phuTung.TonKho ?? 0) - item.SoLuong;
                        if (phuTung.TonKho < 0)
                        {
                            phuTung.TonKho = 0;
                        }
                        ctx.SaveChanges();
                    }
                }
            }
        }

        private string TaoMaHoaDonMoi()
        {
            int maLonNhat = 0;

            using (var ctx = new QuanLyBanXeMayEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var all = ctx.HoaDons.Select(h => h.MaHD).ToList();
                foreach (string ma in all)
                {
                    if (string.IsNullOrWhiteSpace(ma))
                    {
                        continue;
                    }

                    string so = ma.Trim().ToUpper().Replace("HD", string.Empty);
                    int maSo;
                    if (int.TryParse(so, out maSo) && maSo > maLonNhat)
                    {
                        maLonNhat = maSo;
                    }
                }
            }

            return "HD" + (maLonNhat + 1).ToString("000");
        }

        private string LayTenNhanVienMacDinh()
        {
            using (var ctx = new QuanLyBanXeMayEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var nhanVienDangLam = ctx.NhanViens.FirstOrDefault(item =>
                    item.TrangThai == "Đang làm việc");
                if (nhanVienDangLam != null)
                {
                    return nhanVienDangLam.HoTen;
                }

                var nhanVienBatKy = ctx.NhanViens.FirstOrDefault();
                if (nhanVienBatKy != null)
                {
                    return nhanVienBatKy.HoTen;
                }
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
