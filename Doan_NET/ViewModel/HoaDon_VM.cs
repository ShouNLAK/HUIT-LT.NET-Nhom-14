using Doan_NET.Helper;
using Doan_NET.Model;
using Doan_NET.View;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;

namespace Doan_NET.ViewModel
{
    // Màn hình LỊCH SỬ GIAO DỊCH (đã tách khỏi màn thanh toán).
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

        private HoaDon_HienThi_VM hoaDonDuocChon;
        public HoaDon_HienThi_VM HoaDonDuocChon
        {
            get { return hoaDonDuocChon; }
            set
            {
                hoaDonDuocChon = value;
                OnPropertyChanged();
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

        private int tongSoHoaDon;
        public int TongSoHoaDon
        {
            get { return tongSoHoaDon; }
            set { tongSoHoaDon = value; OnPropertyChanged(); }
        }

        private decimal tongDoanhThu;
        public decimal TongDoanhThu
        {
            get { return tongDoanhThu; }
            set { tongDoanhThu = value; OnPropertyChanged(); }
        }

        public ICommand LenhTimKiem { get; }
        public ICommand LenhLamMoi { get; }
        public ICommand LenhInHoaDon { get; }

        public HoaDon_VM()
        {
            LenhTimKiem = new RelayCommand(_ => TaiDanhSachHoaDonHienThi());
            LenhLamMoi = new RelayCommand(_ => { TuKhoaTimKiem = string.Empty; TaiDanhSachHoaDonHienThi(); });
            LenhInHoaDon = new RelayCommand(_ => InHoaDon(), _ => HoaDonDuocChon != null);

            DanhSachHoaDonHienThi = new ObservableCollection<HoaDon_HienThi_VM>();
            TaiDanhSachHoaDonHienThi();
        }

        private void InHoaDon()
        {
            if (HoaDonDuocChon != null)
            {
                W_ReportHoaDon frm = new W_ReportHoaDon(HoaDonDuocChon.MaHD);
                frm.Show();
            }
        }

        private void TaiDanhSachHoaDonHienThi()
        {
            using (var ctx = new QuanLyBanXeMayEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var ds = ctx.HoaDons.AsNoTracking()
                    .Include("NhanVien")
                    .Include("KhachHang")
                    .OrderByDescending(item => item.NgayLap)
                    .ThenByDescending(item => item.MaHD)
                    .ToList();

                var danhSachHienThi = ds.GroupBy(h => h.MaHD).Select(group => 
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

                if (!string.IsNullOrWhiteSpace(TuKhoaTimKiem))
                {
                    string tuKhoa = TuKhoaTimKiem.Trim().ToLower();
                    danhSachHienThi = danhSachHienThi.Where(item =>
                        (item.MaHD ?? string.Empty).ToLower().Contains(tuKhoa) ||
                        (item.TenKhachHang ?? string.Empty).ToLower().Contains(tuKhoa) ||
                        (item.SDT ?? string.Empty).ToLower().Contains(tuKhoa) ||
                        (item.TenDV_SP ?? string.Empty).ToLower().Contains(tuKhoa)).ToList();
                }

                DanhSachHoaDonHienThi = new ObservableCollection<HoaDon_HienThi_VM>(danhSachHienThi);
                TongSoHoaDon = danhSachHienThi.Count;
                TongDoanhThu = danhSachHienThi.Sum(item => item.ThanhTien ?? 0);
            }
        }
    }
}
