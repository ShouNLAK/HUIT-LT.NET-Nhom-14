using Doan_NET.Helper;
using Doan_NET.Model;
using Doan_NET.View;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;

namespace Doan_NET.ViewModel
{
    public class User_DonHang_VM : BaseViewModel
    {
        private ObservableCollection<HoaDon_HienThi_VM> danhSachDonHang;
        public ObservableCollection<HoaDon_HienThi_VM> DanhSachDonHang
        {
            get { return danhSachDonHang; }
            set
            {
                danhSachDonHang = value;
                OnPropertyChanged();
            }
        }

        private HoaDon_HienThi_VM donHangDuocChon;
        public HoaDon_HienThi_VM DonHangDuocChon
        {
            get { return donHangDuocChon; }
            set
            {
                donHangDuocChon = value;
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
                return "(Chưa đăng nhập với hồ sơ khách)";
            }
        }

        public int TongSoDon
        {
            get { return DanhSachDonHang?.Count ?? 0; }
        }

        public decimal TongTienDaMua
        {
            get
            {
                if (DanhSachDonHang == null) return 0;
                return DanhSachDonHang.Sum(h => h.ThanhTien ?? 0);
            }
        }

        public ICommand LenhTaiLai { get; }
        public ICommand LenhInHoaDon { get; }

        public User_DonHang_VM()
        {
            LenhTaiLai = new RelayCommand(_ => TaiDanhSach());
            LenhInHoaDon = new RelayCommand(_ => InHoaDon(), _ => DonHangDuocChon != null);
            TaiDanhSach();
        }

        private void InHoaDon()
        {
            if (DonHangDuocChon != null && DonHangDuocChon.TrangThai == "Đã xác nhận")
            {
                W_ReportHoaDon frm = new W_ReportHoaDon(DonHangDuocChon.MaHD);
                frm.Show();
            }
        }

        private void TaiDanhSach()
        {
            if (PhienDangNhap.KhachHangHienTai == null)
            {
                DanhSachDonHang = new ObservableCollection<HoaDon_HienThi_VM>();
                OnPropertyChanged(nameof(TongSoDon));
                OnPropertyChanged(nameof(TongTienDaMua));
                return;
            }

            string maKH = PhienDangNhap.KhachHangHienTai.MaKH;

            using (var ctx = new QuanLyBanXeMayEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var ds = ctx.HoaDons.AsNoTracking()
                    .Include("NhanVien")
                    .Include("KhachHang")
                    .Where(item => item.MaKH == maKH)
                    .OrderByDescending(item => item.NgayLap)
                    .ThenByDescending(item => item.MaHD)
                    .ToList();

                var ket = ds.GroupBy(h => h.MaHD).Select(group => 
                {
                    var firstItem = group.First();
                    string tenHienThi = firstItem.TenDV_SP;
                    if (group.Count() > 1)
                    {
                        tenHienThi += string.Format(" (+ {0} sản phẩm khác)", group.Count() - 1);
                    }

                    return new HoaDon_HienThi_VM
                    {
                        MaHD = firstItem.MaHD,
                        NgayLap = firstItem.NgayLap,
                        TenNhanVien = firstItem.NhanVien != null ? firstItem.NhanVien.HoTen : "(Khách tự đặt)",
                        TenKhachHang = firstItem.KhachHang != null ? firstItem.KhachHang.HoTen : string.Empty,
                        SDT = firstItem.KhachHang != null ? firstItem.KhachHang.SDT : string.Empty,
                        TenDV_SP = tenHienThi,
                        SoLuong = group.Sum(x => x.SoLuong ?? 0),
                        ThanhTien = group.Sum(x => x.ThanhTien ?? 0),
                        PhuongThucThanhToan = firstItem.PhuongThucThanhToan,
                        TrangThai = firstItem.TrangThai
                    };
                }).ToList();

                DanhSachDonHang = new ObservableCollection<HoaDon_HienThi_VM>(ket);
            }

            OnPropertyChanged(nameof(TongSoDon));
            OnPropertyChanged(nameof(TongTienDaMua));
        }
    }
}
