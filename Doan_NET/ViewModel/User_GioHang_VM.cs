using Doan_NET.Helper;
using Doan_NET.Model;
using Doan_NET.View;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Doan_NET.ViewModel
{
    public class User_GioHang_VM : BaseViewModel
    {
        public ObservableCollection<MatHangGio_VM> GioHang
        {
            get { return PhienDangNhap.GioHangKhach; }
        }

        public decimal TongTien
        {
            get { return GioHang.Sum(item => item.ThanhTien); }
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

        public string TenKhachHienThi
        {
            get
            {
                if (PhienDangNhap.KhachHangHienTai != null)
                {
                    return PhienDangNhap.KhachHangHienTai.HoTen ?? string.Empty;
                }
                return "(Chưa có hồ sơ khách hàng)";
            }
        }

        public string SDTKhachHienThi
        {
            get
            {
                if (PhienDangNhap.KhachHangHienTai != null)
                {
                    return PhienDangNhap.KhachHangHienTai.SDT ?? string.Empty;
                }
                return string.Empty;
            }
        }

        public string DiaChiKhachHienThi
        {
            get
            {
                if (PhienDangNhap.KhachHangHienTai != null && !string.IsNullOrWhiteSpace(PhienDangNhap.KhachHangHienTai.DiaChi))
                {
                    return PhienDangNhap.KhachHangHienTai.DiaChi;
                }
                return "(Chưa có địa chỉ)";
            }
        }

        public ICommand LenhTangSoLuong { get; }
        public ICommand LenhGiamSoLuong { get; }
        public ICommand LenhXoaKhoiGio { get; }
        public ICommand LenhXoaTatCa { get; }
        public ICommand LenhDatHang { get; }

        public User_GioHang_VM()
        {
            DanhSachHinhThucThanhToan = new ObservableCollection<string>
            {
                "Tiền mặt",
                "Chuyển khoản",
                "Trả góp"
            };
            HinhThucThanhToanDangChon = DanhSachHinhThucThanhToan.FirstOrDefault();

            LenhTangSoLuong = new RelayCommand(p => TangSoLuong(p as MatHangGio_VM), p => p is MatHangGio_VM);
            LenhGiamSoLuong = new RelayCommand(p => GiamSoLuong(p as MatHangGio_VM), p => p is MatHangGio_VM);
            LenhXoaKhoiGio = new RelayCommand(p => XoaKhoiGio(p as MatHangGio_VM), p => p is MatHangGio_VM);
            LenhXoaTatCa = new RelayCommand(_ => XoaTatCa());
            LenhDatHang = new RelayCommand(_ => DatHang());

            DangKySuKienGioHang();
        }

        private void DangKySuKienGioHang()
        {
            GioHang.CollectionChanged -= XuLyThayDoiGioHang;
            GioHang.CollectionChanged += XuLyThayDoiGioHang;

            foreach (MatHangGio_VM item in GioHang)
            {
                item.PropertyChanged -= XuLyThayDoiMatHang;
                item.PropertyChanged += XuLyThayDoiMatHang;
            }
        }

        private void XuLyThayDoiGioHang(object nguon, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (MatHangGio_VM item in e.NewItems)
                {
                    item.PropertyChanged -= XuLyThayDoiMatHang;
                    item.PropertyChanged += XuLyThayDoiMatHang;
                }
            }
            if (e.OldItems != null)
            {
                foreach (MatHangGio_VM item in e.OldItems)
                {
                    item.PropertyChanged -= XuLyThayDoiMatHang;
                }
            }
            OnPropertyChanged(nameof(TongTien));
        }

        private void XuLyThayDoiMatHang(object nguon, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MatHangGio_VM.SoLuong) ||
                e.PropertyName == nameof(MatHangGio_VM.DonGia))
            {
                OnPropertyChanged(nameof(TongTien));
            }
        }

        private void TangSoLuong(MatHangGio_VM item)
        {
            if (item == null) return;
            int tonKhoToiDa = LayTonKhoToiDa(item);
            if (item.SoLuong + 1 > tonKhoToiDa)
            {
                MessageBox.Show("Vượt quá tồn kho (" + tonKhoToiDa + ").", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            item.SoLuong = item.SoLuong + 1;
        }

        private void GiamSoLuong(MatHangGio_VM item)
        {
            if (item == null) return;
            if (item.SoLuong <= 1)
            {
                XoaKhoiGio(item);
                return;
            }
            item.SoLuong = item.SoLuong - 1;
        }

        private void XoaKhoiGio(MatHangGio_VM item)
        {
            if (item == null) return;
            GioHang.Remove(item);
        }

        private void XoaTatCa()
        {
            if (GioHang.Count == 0) return;
            var ketQua = MessageBox.Show("Bạn có chắc muốn xóa toàn bộ giỏ hàng?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (ketQua == MessageBoxResult.Yes)
            {
                GioHang.Clear();
            }
        }

        private int LayTonKhoToiDa(MatHangGio_VM item)
        {
            string ma = (item.MaMatHang ?? string.Empty).Trim().ToUpper();
            using (var ctx = new QuanLyBanXeMayEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                if (ma.StartsWith("XE"))
                {
                    var xe = ctx.Xes.FirstOrDefault(x => x.MaXe == item.MaMatHang);
                    return xe?.SoLuongTon ?? 0;
                }
                if (ma.StartsWith("PT"))
                {
                    var pt = ctx.DichVuPhuTungs.FirstOrDefault(d => d.MaPT == item.MaMatHang);
                    return pt?.TonKho ?? 0;
                }
            }
            return int.MaxValue; // Dịch vụ không bị giới hạn tồn kho
        }

        private void DatHang()
        {
            if (GioHang.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (PhienDangNhap.KhachHangHienTai == null)
            {
                MessageBox.Show("Vui lòng cập nhật thông tin tại trang Tài khoản trước khi đặt hàng.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!KiemTraTonKho())
            {
                return;
            }

            DateTime ngayLap = DateTime.Now;
            string maHoaDon = "";

            try
            {
                int soBatDau = LaySoHoaDonLonNhat();
                maHoaDon = "HD" + (soBatDau + 1).ToString("000");

                using (var ctx = new QuanLyBanXeMayEntities())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    var dsHopLe = GioHang.Where(x => x.ThanhTien > 0).ToList();
                    if (dsHopLe.Count == 0) return;

                    // Lấy mã nhân viên đầu tiên làm mặc định cho khách tự đặt
                    // (Vì bảng HoaDon quy định MaNV nằm trong khóa chính nên không được NULL)
                    string maNVMacDinh = ctx.NhanViens.Select(nv => nv.MaNV).FirstOrDefault();

                    foreach (MatHangGio_VM item in dsHopLe)
                    {
                        string sql = "INSERT INTO HoaDon (MaHD, NgayLap, MaNV, MaKH, TenDV_SP, SoLuong, ThanhTien, PhuongThucThanhToan, TrangThai) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})";
                        ctx.Database.ExecuteSqlCommand(sql,
                            maHoaDon,
                            ngayLap,
                            string.IsNullOrEmpty(maNVMacDinh) ? (object)DBNull.Value : maNVMacDinh,
                            string.IsNullOrEmpty(PhienDangNhap.KhachHangHienTai.MaKH) ? (object)DBNull.Value : PhienDangNhap.KhachHangHienTai.MaKH,
                            item.TenMatHang,
                            item.SoLuong,
                            item.ThanhTien,
                            string.IsNullOrEmpty(HinhThucThanhToanDangChon) ? (object)DBNull.Value : HinhThucThanhToanDangChon,
                            "Chờ xác nhận"
                        );

                        string ma = (item.MaMatHang ?? string.Empty).Trim().ToUpper();
                        if (ma.StartsWith("XE"))
                        {
                            var xe = ctx.Xes.FirstOrDefault(x => x.MaXe == item.MaMatHang);
                            if (xe != null)
                            {
                                xe.SoLuongTon = (xe.SoLuongTon ?? 0) - item.SoLuong;
                                if (xe.SoLuongTon < 0) xe.SoLuongTon = 0;
                            }
                        }
                        else if (ma.StartsWith("PT"))
                        {
                            var pt = ctx.DichVuPhuTungs.FirstOrDefault(d => d.MaPT == item.MaMatHang);
                            if (pt != null)
                            {
                                pt.TonKho = (pt.TonKho ?? 0) - item.SoLuong;
                                if (pt.TonKho < 0) pt.TonKho = 0;
                            }
                        }
                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đặt hàng thất bại: " + ex.Message, "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            GioHang.Clear();
            OnPropertyChanged(nameof(TongTien));
            MessageBox.Show("Đặt hàng thành công hóa đơn " + maHoaDon + ".", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);

            // Mở report hóa đơn mới
            W_ReportHoaDon frmReport = new W_ReportHoaDon(maHoaDon);
            frmReport.Show();
        }

        private bool KiemTraTonKho()
        {
            foreach (MatHangGio_VM item in GioHang)
            {
                int ton = LayTonKhoToiDa(item);
                if (item.SoLuong > ton)
                {
                    MessageBox.Show("Mặt hàng \"" + item.TenMatHang + "\" không đủ tồn kho (còn " + ton + ").",
                        "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            return true;
        }

        private int LaySoHoaDonLonNhat()
        {
            int maLonNhat = 0;
            using (var ctx = new QuanLyBanXeMayEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var all = ctx.HoaDons.Select(h => h.MaHD).ToList();
                foreach (string ma in all)
                {
                    if (string.IsNullOrWhiteSpace(ma)) continue;
                    string so = ma.Trim().ToUpper().Replace("HD", string.Empty);
                    int maSo;
                    if (int.TryParse(so, out maSo) && maSo > maLonNhat)
                    {
                        maLonNhat = maSo;
                    }
                }
            }
            return maLonNhat;
        }
    }
}
