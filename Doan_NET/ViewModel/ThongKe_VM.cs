using Doan_NET.Helper;
using Doan_NET.Model;
using Doan_NET.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Doan_NET.ViewModel
{
    public class ThongKe_VM : BaseViewModel
    {
        private decimal tongDoanhThu;
        public decimal TongDoanhThu
        {
            get { return tongDoanhThu; }
            set
            {
                tongDoanhThu = value;
                OnPropertyChanged();
            }
        }

        private int soHoaDonMoi;
        public int SoHoaDonMoi
        {
            get { return soHoaDonMoi; }
            set
            {
                soHoaDonMoi = value;
                OnPropertyChanged();
            }
        }

        private int soKhachHangPhucVu;
        public int SoKhachHangPhucVu
        {
            get { return soKhachHangPhucVu; }
            set
            {
                soKhachHangPhucVu = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<CotDoanhThuThang_VM> duLieuDoanhThu6Thang;
        public ObservableCollection<CotDoanhThuThang_VM> DuLieuDoanhThu6Thang
        {
            get { return duLieuDoanhThu6Thang; }
            set
            {
                duLieuDoanhThu6Thang = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MucThongKeTop_VM> danhSachDichVuBanChay;
        public ObservableCollection<MucThongKeTop_VM> DanhSachDichVuBanChay
        {
            get { return danhSachDichVuBanChay; }
            set
            {
                danhSachDichVuBanChay = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MucThongKeTop_VM> danhSachXeBanChay;
        public ObservableCollection<MucThongKeTop_VM> DanhSachXeBanChay
        {
            get { return danhSachXeBanChay; }
            set
            {
                danhSachXeBanChay = value;
                OnPropertyChanged();
            }
        }

        public ICommand LenhTaiLaiThongKe { get; }
        public ICommand LenhInThongKe { get; }

        public ThongKe_VM()
        {
            LenhTaiLaiThongKe = new RelayCommand(_ => TaiThongKe());
            LenhInThongKe = new RelayCommand(_ => { W_ReportThongKe frm = new W_ReportThongKe(); frm.Show(); });
            TaiThongKe();
        }

        private void TaiThongKe()
        {
            List<HoaDon> danhSachHoaDon;
            try
            {
                using (var ctx = new QuanLyBanXeMayEntities())
                {
                    danhSachHoaDon = ctx.HoaDons.ToList();
                }
            }
            catch (Exception)
            {
                danhSachHoaDon = new List<HoaDon>();
            }

            TongDoanhThu = danhSachHoaDon.Sum(item => item.ThanhTien ?? 0);
            SoHoaDonMoi = danhSachHoaDon
                .Select(item => item.MaHD ?? string.Empty)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count();

            SoKhachHangPhucVu = danhSachHoaDon
                .Select(item => item.MaKH ?? string.Empty)
                .Where(item => !string.IsNullOrWhiteSpace(item))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count();

            TaiDuLieuDoanhThu6Thang(danhSachHoaDon);
            TaiDanhSachTop(danhSachHoaDon);
        }

        private void TaiDuLieuDoanhThu6Thang(List<HoaDon> danhSachHoaDon)
        {
            var danhSachThang = danhSachHoaDon
                .Where(x => x.NgayLap.HasValue)
                .Select(x => new { Nam = x.NgayLap.Value.Year, Thang = x.NgayLap.Value.Month })
                .Distinct()
                .OrderByDescending(x => x.Nam).ThenByDescending(x => x.Thang)
                .Take(6)
                .OrderBy(x => x.Nam).ThenBy(x => x.Thang)
                .ToList();

            if (danhSachThang.Count == 0)
            {
                for (int i = 5; i >= 0; i--)
                {
                    var date = DateTime.Now.AddMonths(-i);
                    danhSachThang.Add(new { Nam = date.Year, Thang = date.Month });
                }
            }

            List<CotDoanhThuThang_VM> duLieu = new List<CotDoanhThuThang_VM>();
            decimal doanhThuLonNhat = 0;

            foreach (var t in danhSachThang)
            {
                decimal doanhThu = danhSachHoaDon
                    .Where(item => item.NgayLap.HasValue && item.NgayLap.Value.Year == t.Nam && item.NgayLap.Value.Month == t.Thang)
                    .Sum(item => item.ThanhTien ?? 0);

                if (doanhThu > doanhThuLonNhat)
                {
                    doanhThuLonNhat = doanhThu;
                }

                string strHienThi = doanhThu.ToString("N0") + " đ";
                if (doanhThu >= 1000000000)
                    strHienThi = (doanhThu / 1000000000m).ToString("0.##") + " Tỷ";
                else if (doanhThu >= 1000000)
                    strHienThi = (doanhThu / 1000000m).ToString("0.##") + " Tr";

                duLieu.Add(new CotDoanhThuThang_VM
                {
                    ThangHienThi = t.Thang + "/" + (t.Nam % 100).ToString("00"),
                    DoanhThuThang = doanhThu,
                    GiaTriHienThi = strHienThi
                });
            }

            foreach (CotDoanhThuThang_VM cot in duLieu)
            {
                if (doanhThuLonNhat <= 0)
                {
                    cot.ChieuCaoCot = 20;
                }
                else
                {
                    cot.ChieuCaoCot = Math.Max(20, ((double)cot.DoanhThuThang / (double)doanhThuLonNhat) * 160);
                }
            }

            DuLieuDoanhThu6Thang = new ObservableCollection<CotDoanhThuThang_VM>(duLieu);
        }

        private void TaiDanhSachTop(List<HoaDon> danhSachHoaDon)
        {
            HashSet<string> tapTenXe;
            try
            {
                using (var ctx = new QuanLyBanXeMayEntities())
                {
                    tapTenXe = new HashSet<string>(ctx.Xes.Select(x => x.TenXe ?? string.Empty), StringComparer.OrdinalIgnoreCase);
                }
            }
            catch (Exception)
            {
                tapTenXe = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }

            List<MucThongKeTop_VM> topDichVu = danhSachHoaDon
                .Where(item => !tapTenXe.Contains(item.TenDV_SP ?? string.Empty))
                .GroupBy(item => item.TenDV_SP ?? "Không xác định")
                .Select(nhom => new MucThongKeTop_VM
                {
                    TenMuc = nhom.Key,
                    SoLuongBan = nhom.Sum(item => item.SoLuong ?? 0),
                    DoanhThu = nhom.Sum(item => item.ThanhTien ?? 0)
                })
                .OrderByDescending(item => item.DoanhThu)
                .ThenByDescending(item => item.SoLuongBan)
                .Take(5)
                .ToList();

            List<MucThongKeTop_VM> topXe = danhSachHoaDon
                .Where(item => tapTenXe.Contains(item.TenDV_SP ?? string.Empty))
                .GroupBy(item => item.TenDV_SP ?? "Không xác định")
                .Select(nhom => new MucThongKeTop_VM
                {
                    TenMuc = nhom.Key,
                    SoLuongBan = nhom.Sum(item => item.SoLuong ?? 0),
                    DoanhThu = nhom.Sum(item => item.ThanhTien ?? 0)
                })
                .OrderByDescending(item => item.DoanhThu)
                .ThenByDescending(item => item.SoLuongBan)
                .Take(5)
                .ToList();

            if (topDichVu.Count == 0)
            {
                topDichVu.Add(new MucThongKeTop_VM { TenMuc = "Chưa có dữ liệu", SoLuongBan = 0, DoanhThu = 0 });
            }

            if (topXe.Count == 0)
            {
                topXe.Add(new MucThongKeTop_VM { TenMuc = "Chưa có dữ liệu", SoLuongBan = 0, DoanhThu = 0 });
            }

            DanhSachDichVuBanChay = new ObservableCollection<MucThongKeTop_VM>(topDichVu);
            DanhSachXeBanChay = new ObservableCollection<MucThongKeTop_VM>(topXe);
        }
    }

    public class CotDoanhThuThang_VM : BaseViewModel
    {
        private string thangHienThi;
        public string ThangHienThi
        {
            get { return thangHienThi; }
            set
            {
                thangHienThi = value;
                OnPropertyChanged();
            }
        }

        private decimal doanhThuThang;
        public decimal DoanhThuThang
        {
            get { return doanhThuThang; }
            set
            {
                doanhThuThang = value;
                OnPropertyChanged();
            }
        }

        private double chieuCaoCot;
        public double ChieuCaoCot
        {
            get { return chieuCaoCot; }
            set
            {
                chieuCaoCot = value;
                OnPropertyChanged();
            }
        }

        private string giaTriHienThi;
        public string GiaTriHienThi
        {
            get { return giaTriHienThi; }
            set
            {
                giaTriHienThi = value;
                OnPropertyChanged();
            }
        }
    }

    public class MucThongKeTop_VM
    {
        public string TenMuc { get; set; }
        public int SoLuongBan { get; set; }
        public decimal DoanhThu { get; set; }
    }
}
