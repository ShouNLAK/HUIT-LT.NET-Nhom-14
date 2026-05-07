using Doan_NET.Helper;
using Doan_NET.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Doan_NET.View;
using System.Windows;

namespace Doan_NET.ViewModel
{
    public class Xe_VM : BaseViewModel
    {
        private readonly HangXe hangXeDuocChon;
        private readonly RelayCommand lenhMoSuaXe;
        private readonly RelayCommand lenhXoaXe;
        private bool dangSuaXe;

        private ObservableCollection<MoTo> danhSachXe;
        public ObservableCollection<MoTo> DanhSachXe
        {
            get { return danhSachXe; }
            set
            {
                danhSachXe = value;
                OnPropertyChanged("DanhSachXe");
            }
        }

        private MoTo xeDangChon;
        public MoTo XeDangChon
        {
            get { return xeDangChon; }
            set
            {
                xeDangChon = value;
                OnPropertyChanged();
                if (xeDangChon != null)
                {
                    TenDongXeNhap = xeDangChon.TenDongXe;
                    LoaiXeNhap = xeDangChon.LoaiXe;
                    MauSacNhap = xeDangChon.MauSac;
                    NamSXNhap = xeDangChon.NamSX.ToString();
                    GiaXeNhap = string.Format("{0:N0}", xeDangChon.GiaXe).Replace(",", ".");
                    HinhAnhNhap = xeDangChon.HinhAnhFullPath;
                    MoTaNhap = xeDangChon.MoTa;
                    SoLuongTonNhap = xeDangChon.SoLuongTon.ToString();
                }
                lenhMoSuaXe?.RaiseCanExecuteChanged();
                lenhXoaXe?.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(CoXeDuocChon));
            }
        }

        private string tieuDeDanhSachXe;
        public string TieuDeDanhSachXe
        {
            get { return tieuDeDanhSachXe; }
            set
            {
                tieuDeDanhSachXe = value;
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

        private string tenDongXeNhap;
        public string TenDongXeNhap
        {
            get { return tenDongXeNhap; }
            set
            {
                tenDongXeNhap = value;
                OnPropertyChanged();
            }
        }

        private string loaiXeNhap;
        public string LoaiXeNhap
        {
            get { return loaiXeNhap; }
            set
            {
                loaiXeNhap = value;
                OnPropertyChanged();
            }
        }

        private string mauSacNhap;
        public string MauSacNhap
        {
            get { return mauSacNhap; }
            set
            {
                mauSacNhap = value;
                OnPropertyChanged();
            }
        }

        private string namSXNhap;
        public string NamSXNhap
        {
            get { return namSXNhap; }
            set
            {
                namSXNhap = value;
                OnPropertyChanged();
            }
        }

        private string giaXeNhap;
        public string GiaXeNhap
        {
            get { return giaXeNhap; }
            set
            {
                giaXeNhap = value;
                OnPropertyChanged();
            }
        }

        private string hinhAnhNhap;
        public string HinhAnhNhap
        {
            get { return hinhAnhNhap; }
            set
            {
                hinhAnhNhap = value;
                OnPropertyChanged();
            }
        }

        private string moTaNhap;
        public string MoTaNhap
        {
            get { return moTaNhap; }
            set
            {
                moTaNhap = value;
                OnPropertyChanged();
            }
        }

        private string soLuongTonNhap;
        public string SoLuongTonNhap
        {
            get { return soLuongTonNhap; }
            set
            {
                soLuongTonNhap = value;
                OnPropertyChanged();
            }
        }

        public bool CoXeDuocChon
        {
            get { return XeDangChon != null; }
        }

        public ICommand LenhQuayLaiHangXe { get; }
        public ICommand LenhMoThemXe { get; }
        public ICommand LenhMoSuaXe
        {
            get { return lenhMoSuaXe; }
        }
        public ICommand LenhXoaXe
        {
            get { return lenhXoaXe; }
        }
        public ICommand LenhLuuXe { get; }
        public ICommand LenhHuyFormXe { get; }
        public ICommand LenhTimKiemXe { get; }

        public Xe_VM() : this(null)
        {
        }

        public Xe_VM(HangXe hangXe)
        {
            hangXeDuocChon = hangXe;
            LenhQuayLaiHangXe = new RelayCommand(_ => DieuHuongTuMain("QuanLyXe"));
            LenhMoThemXe = new RelayCommand(_ => MoThemXe());
            lenhMoSuaXe = new RelayCommand(_ => MoSuaXe(), _ => XeDangChon != null);
            lenhXoaXe = new RelayCommand(_ => XoaXe(), _ => XeDangChon != null);
            LenhLuuXe = new RelayCommand(parameter => LuuXe(parameter as Window));
            LenhHuyFormXe = new RelayCommand(parameter => DongFormXe(parameter as Window));
            LenhTimKiemXe = new RelayCommand(_ => TaiDanhSachXe());

            CapNhatTieuDe();
            TaiDanhSachXe();
            LamMoiNhapXe();
        }

        private void CapNhatTieuDe()
        {
            if (hangXeDuocChon == null || string.IsNullOrWhiteSpace(hangXeDuocChon.TenHang))
            {
                TieuDeDanhSachXe = "DANH SÁCH XE";
                return;
            }

            TieuDeDanhSachXe = "DANH SÁCH XE - " + hangXeDuocChon.TenHang;
        }

        private void DieuHuongTuMain(string tenManHinh, object duLieu = null)
        {
            var mainVm = Application.Current?.MainWindow?.DataContext as MainWindows_VM;
            if (mainVm != null)
            {
                mainVm.DieuHuong(tenManHinh, duLieu);
            }
        }

        private void TaiDanhSachXe()
        {
            List<MoTo> danhSachLoc;

            using (var ctx = new QuanLyBanXeMayEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var xeEntities = ctx.Xes.Include("HangXe").ToList();

                danhSachLoc = xeEntities.Select(x => new MoTo
                {
                    TenHang = x.HangXe != null ? x.HangXe.TenHang : string.Empty,
                    TenDongXe = x.TenXe ?? string.Empty,
                    LoaiXe = x.LoaiXe ?? string.Empty,
                    MauSac = x.MauSac ?? string.Empty,
                    NamSX = x.NamSX ?? 0,
                    GiaXe = x.GiaBan.HasValue ? (int)x.GiaBan.Value : 0,
                    HinhAnhFullPath = x.HinhAnh ?? string.Empty,
                    MoTa = x.MoTa ?? string.Empty,
                    SoLuongTon = 0
                }).ToList();
            }

            if (hangXeDuocChon != null && !string.IsNullOrWhiteSpace(hangXeDuocChon.TenHang))
            {
                danhSachLoc = danhSachLoc.Where(item =>
                    string.Equals(item.TenHang, hangXeDuocChon.TenHang, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(TuKhoaTimKiem))
            {
                string tuKhoa = TuKhoaTimKiem.Trim().ToLower();
                danhSachLoc = danhSachLoc.Where(item =>
                    (item.TenDongXe ?? string.Empty).ToLower().Contains(tuKhoa) ||
                    (item.LoaiXe ?? string.Empty).ToLower().Contains(tuKhoa) ||
                    (item.MauSac ?? string.Empty).ToLower().Contains(tuKhoa)).ToList();
            }

            DanhSachXe = new ObservableCollection<MoTo>(danhSachLoc);

            if (XeDangChon != null)
            {
                XeDangChon = DanhSachXe.FirstOrDefault(item =>
                    string.Equals(item.TenHang, XeDangChon.TenHang, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(item.TenDongXe, XeDangChon.TenDongXe, StringComparison.OrdinalIgnoreCase));
            }

            lenhMoSuaXe.RaiseCanExecuteChanged();
            lenhXoaXe.RaiseCanExecuteChanged();
            OnPropertyChanged(nameof(CoXeDuocChon));
        }

        private void LamMoiNhapXe()
        {
            TenDongXeNhap = string.Empty;
            LoaiXeNhap = string.Empty;
            MauSacNhap = string.Empty;
            NamSXNhap = DateTime.Now.Year.ToString();
            GiaXeNhap = "0";
            HinhAnhNhap = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/ac/No_image_available.svg/320px-No_image_available.svg.png";
            MoTaNhap = string.Empty;
            SoLuongTonNhap = "0";
        }

        private bool KiemTraDuLieuXe(out int namSX, out int soLuongTon, out int giaXe)
        {
            namSX = 0;
            soLuongTon = 0;
            giaXe = 0;

            if (string.IsNullOrWhiteSpace(TenDongXeNhap))
            {
                MessageBox.Show("Tên dòng xe không được để trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(LoaiXeNhap))
            {
                MessageBox.Show("Loại xe không được để trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(NamSXNhap, out namSX) || namSX < 1900)
            {
                MessageBox.Show("Năm sản xuất không hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            int namHienTai = DateTime.Now.Year;
            if (namSX > namHienTai + 1)
            {
                MessageBox.Show("Năm sản xuất không được lớn hơn năm hiện tại + 1.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(SoLuongTonNhap, out soLuongTon) || soLuongTon < 0)
            {
                MessageBox.Show("Số lượng tồn không hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            string giaChuanHoa;
            if (!ThuChuanHoaGiaXe(GiaXeNhap, out giaXe, out giaChuanHoa))
            {
                MessageBox.Show("Giá xe không hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            GiaXeNhap = giaChuanHoa;

            if (string.IsNullOrWhiteSpace(HinhAnhNhap))
            {
                HinhAnhNhap = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/ac/No_image_available.svg/320px-No_image_available.svg.png";
            }

            if (!LaUrlHopLe(HinhAnhNhap.Trim()))
            {
                MessageBox.Show("Đường dẫn hình ảnh phải là URL hợp lệ (http/https).", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private bool ThuChuanHoaGiaXe(string giaNhap, out int gia, out string giaChuanHoa)
        {
            gia = 0;
            giaChuanHoa = "0";

            if (string.IsNullOrWhiteSpace(giaNhap))
            {
                return false;
            }

            string giaRaw = giaNhap.Trim().Replace(".", string.Empty).Replace(",", string.Empty).Replace(" ", string.Empty);
            if (!int.TryParse(giaRaw, out gia) || gia < 0)
            {
                return false;
            }

            giaChuanHoa = string.Format("{0:N0}", gia).Replace(",", ".");
            return true;
        }

        private bool LaUrlHopLe(string duongDan)
        {
            Uri uri;
            return Uri.TryCreate(duongDan, UriKind.Absolute, out uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }

        private string LayTenHangLuuXe()
        {
            if (hangXeDuocChon != null && !string.IsNullOrWhiteSpace(hangXeDuocChon.TenHang))
            {
                return hangXeDuocChon.TenHang;
            }

            if (XeDangChon != null && !string.IsNullOrWhiteSpace(XeDangChon.TenHang))
            {
                return XeDangChon.TenHang;
            }

            using (var ctx = new QuanLyBanXeMayEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var hangMacDinh = ctx.HangXes.FirstOrDefault();
                return hangMacDinh != null ? hangMacDinh.TenHang : string.Empty;
            }
        }

        private void LuuXe(Window cuaSo)
        {
            int namSX;
            int soLuongTon;
            int giaXe;
            if (!KiemTraDuLieuXe(out namSX, out soLuongTon, out giaXe))
            {
                return;
            }

            string tenHangLuu = LayTenHangLuuXe();

            if (dangSuaXe && XeDangChon != null)
            {
                using (var ctx = new QuanLyBanXeMayEntities())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    string tenDongXeNhapTrim = TenDongXeNhap.Trim();
                    string tenXeCu = XeDangChon.TenDongXe;
                    var xeMoi = ctx.Xes.Include("HangXe").FirstOrDefault(x =>
                        x.TenXe == tenDongXeNhapTrim &&
                        x.TenXe != tenXeCu &&
                        x.HangXe != null &&
                        x.HangXe.TenHang == tenHangLuu);

                    if (xeMoi != null)
                    {
                        MessageBox.Show("Tên dòng xe đã tồn tại trong hãng này.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var efXe = ctx.Xes.Include("HangXe").FirstOrDefault(x =>
                        x.TenXe == XeDangChon.TenDongXe &&
                        x.HangXe != null &&
                        x.HangXe.TenHang == tenHangLuu);

                    if (efXe != null)
                    {
                        efXe.TenXe = TenDongXeNhap.Trim();
                        efXe.LoaiXe = LoaiXeNhap.Trim();
                        efXe.MauSac = MauSacNhap.Trim();
                        efXe.NamSX = namSX;
                        efXe.GiaBan = giaXe;
                        efXe.HinhAnh = HinhAnhNhap.Trim();
                        efXe.MoTa = MoTaNhap.Trim();
                        ctx.SaveChanges();
                    }
                }

                dangSuaXe = false;
                TaiDanhSachXe();
                XeDangChon = DanhSachXe.FirstOrDefault(d => d.TenDongXe == TenDongXeNhap.Trim());
                cuaSo?.Close();
            }
            else
            {
                using (var ctx = new QuanLyBanXeMayEntities())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    string tenDongXeNhapTrim2 = TenDongXeNhap.Trim();
                    var xeTrung = ctx.Xes.Include("HangXe").FirstOrDefault(x =>
                        x.TenXe == tenDongXeNhapTrim2 &&
                        x.HangXe != null &&
                        x.HangXe.TenHang == tenHangLuu);

                    if (xeTrung != null)
                    {
                        MessageBox.Show("Tên dòng xe đã tồn tại trong hãng này.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var hang = ctx.HangXes.FirstOrDefault(h => h.TenHang == tenHangLuu);
                    if (hang == null)
                    {
                        MessageBox.Show("Không tìm thấy hãng xe. Vui lòng kiểm tra lại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    string maMoiXe = TaoMaXeMoi(ctx);
                    var efXe = new Xe
                    {
                        MaXe = maMoiXe,
                        TenXe = TenDongXeNhap.Trim(),
                        LoaiXe = LoaiXeNhap.Trim(),
                        MauSac = MauSacNhap.Trim(),
                        NamSX = namSX,
                        GiaBan = giaXe,
                        HinhAnh = HinhAnhNhap.Trim(),
                        MoTa = MoTaNhap.Trim(),
                        MaHang = hang.MaHang
                    };
                    ctx.Xes.Add(efXe);
                    ctx.SaveChanges();
                }

                dangSuaXe = false;
                TaiDanhSachXe();
                XeDangChon = DanhSachXe.FirstOrDefault(item => item.TenDongXe == TenDongXeNhap.Trim());
                cuaSo?.Close();
            }
        }

        private string TaoMaXeMoi(QuanLyBanXeMayEntities ctx)
        {
            int soLonNhat = 0;
            var tatCaMaXe = ctx.Xes.Select(x => x.MaXe).ToList();
            foreach (string ma in tatCaMaXe)
            {
                if (string.IsNullOrWhiteSpace(ma))
                {
                    continue;
                }
                string so = ma.Trim().ToUpper().Replace("XE", string.Empty);
                int maSo;
                if (int.TryParse(so, out maSo) && maSo > soLonNhat)
                {
                    soLonNhat = maSo;
                }
            }
            return "XE" + (soLonNhat + 1).ToString("000");
        }

        private void XoaXe()
        {
            if (XeDangChon == null)
            {
                return;
            }

            var ketQua = MessageBox.Show("Bạn có chắc muốn xóa xe đang chọn?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (ketQua != MessageBoxResult.Yes)
            {
                return;
            }

            using (var ctx = new QuanLyBanXeMayEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var ef = ctx.Xes.FirstOrDefault(x => x.TenXe == XeDangChon.TenDongXe);
                if (ef != null)
                {
                    ctx.Xes.Remove(ef);
                    ctx.SaveChanges();
                }
            }

            XeDangChon = null;
            LamMoiNhapXe();
            TaiDanhSachXe();
        }

        private void MoThemXe()
        {
            dangSuaXe = false;
            LamMoiNhapXe();
            var cuaSoThemXe = new W_ThemXe();
            cuaSoThemXe.DataContext = this;
            cuaSoThemXe.ShowDialog();
        }

        private void MoSuaXe()
        {
            if (XeDangChon == null)
            {
                return;
            }

            dangSuaXe = true;
            TenDongXeNhap = XeDangChon.TenDongXe;
            LoaiXeNhap = XeDangChon.LoaiXe;
            MauSacNhap = XeDangChon.MauSac;
            NamSXNhap = XeDangChon.NamSX.ToString();
            GiaXeNhap = string.Format("{0:N0}", XeDangChon.GiaXe).Replace(",", ".");
            HinhAnhNhap = XeDangChon.HinhAnhFullPath;
            MoTaNhap = XeDangChon.MoTa;
            SoLuongTonNhap = XeDangChon.SoLuongTon.ToString();
            var cuaSoSuaXe = new W_SuaXe();
            cuaSoSuaXe.DataContext = this;
            cuaSoSuaXe.ShowDialog();
        }

        private void DongFormXe(Window cuaSo)
        {
            dangSuaXe = false;
            LamMoiNhapXe();
            cuaSo?.Close();
        }
    }
}
