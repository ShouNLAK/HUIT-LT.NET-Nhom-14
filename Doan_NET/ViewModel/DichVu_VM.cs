using Doan_NET.Helper;
using Doan_NET.Model;
using Doan_NET.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Doan_NET.ViewModel
{
    public class DichVu_VM : BaseViewModel
    {
        private static readonly ObservableCollection<MatHangGio_VM> gioHangDungChung = new ObservableCollection<MatHangGio_VM>();

        private readonly RelayCommand lenhMoSuaDichVu;
        private readonly RelayCommand lenhXoaDichVu;
        private bool dangSuaDichVu;
        
        public bool KhongPhaiSua
        {
            get { return !dangSuaDichVu; }
        }

        private int loaiMatHangNhap;
        public int LoaiMatHangNhap
        {
            get { return loaiMatHangNhap; }
            set
            {
                loaiMatHangNhap = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DichVuPhuTung> danhSachDichVu;
        public ObservableCollection<DichVuPhuTung> DanhSachDichVu
        {
            get { return danhSachDichVu; }
            set
            {
                danhSachDichVu = value;
                OnPropertyChanged();
            }
        }

        private DichVuPhuTung dichVuDangChon;
        public DichVuPhuTung DichVuDangChon
        {
            get { return dichVuDangChon; }
            set
            {
                dichVuDangChon = value;
                OnPropertyChanged();
                if (dichVuDangChon != null)
                {
                    MaPTNhap = dichVuDangChon.MaPT;
                    TenNhap = dichVuDangChon.Ten;
                    GiaNhap = (dichVuDangChon.Gia ?? 0).ToString();
                    TonKhoNhap = (dichVuDangChon.TonKho ?? 0).ToString();
                }
                lenhMoSuaDichVu?.RaiseCanExecuteChanged();
                lenhXoaDichVu?.RaiseCanExecuteChanged();
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

        private string maPTNhap;
        public string MaPTNhap
        {
            get { return maPTNhap; }
            set
            {
                maPTNhap = value;
                OnPropertyChanged();
            }
        }

        private string tenNhap;
        public string TenNhap
        {
            get { return tenNhap; }
            set
            {
                tenNhap = value;
                OnPropertyChanged();
            }
        }

        private string giaNhap;
        public string GiaNhap
        {
            get { return giaNhap; }
            set
            {
                giaNhap = value;
                OnPropertyChanged();
            }
        }

        private string tonKhoNhap;
        public string TonKhoNhap
        {
            get { return tonKhoNhap; }
            set
            {
                tonKhoNhap = value;
                OnPropertyChanged();
            }
        }

        public static ObservableCollection<MatHangGio_VM> GioHangDungChung
        {
            get { return gioHangDungChung; }
        }

        public ObservableCollection<MatHangGio_VM> GioHangTam
        {
            get { return GioHangDungChung; }
        }

        public decimal TongTienTamTinh
        {
            get { return GioHangTam.Sum(item => item.ThanhTien); }
        }

        public ICommand LenhMoThemDichVu { get; }
        public ICommand LenhMoSuaDichVu
        {
            get { return lenhMoSuaDichVu; }
        }
        public ICommand LenhXoaDichVu
        {
            get { return lenhXoaDichVu; }
        }
        public ICommand LenhLuuDichVu { get; }
        public ICommand LenhHuyFormDichVu { get; }
        public ICommand LenhTimKiemDichVu { get; }
        public ICommand LenhThanhToan { get; }
        public ICommand LenhChonBan { get; }
        public ICommand LenhTangSoLuongTrongGio { get; }
        public ICommand LenhGiamSoLuongTrongGio { get; }
        public ICommand LenhXoaKhoiGio { get; }

        public DichVu_VM()
        {
            LenhMoThemDichVu = new RelayCommand(_ => MoManHinhThemSuaDichVu());
            lenhMoSuaDichVu = new RelayCommand(_ => MoManHinhSuaDichVu(), _ => DichVuDangChon != null);
            lenhXoaDichVu = new RelayCommand(_ => XoaDichVu(), _ => DichVuDangChon != null);
            LenhLuuDichVu = new RelayCommand(parameter => LuuDichVu(parameter as Window));
            LenhHuyFormDichVu = new RelayCommand(parameter => DongFormDichVu(parameter as Window));
            LenhTimKiemDichVu = new RelayCommand(_ => TaiDanhSachDichVu());
            LenhThanhToan = new RelayCommand(_ => ChuyenSangThanhToan());
            LenhChonBan = new RelayCommand(parameter => ThemVaoGio(parameter as DichVuPhuTung), parameter => parameter is DichVuPhuTung);
            LenhTangSoLuongTrongGio = new RelayCommand(parameter => TangSoLuongTrongGio(parameter as MatHangGio_VM), parameter => parameter is MatHangGio_VM);
            LenhGiamSoLuongTrongGio = new RelayCommand(parameter => GiamSoLuongTrongGio(parameter as MatHangGio_VM), parameter => parameter is MatHangGio_VM);
            LenhXoaKhoiGio = new RelayCommand(parameter => XoaKhoiGio(parameter as MatHangGio_VM), parameter => parameter is MatHangGio_VM);

            GioHangTam.CollectionChanged -= XuLyThayDoiGioHang;
            GioHangTam.CollectionChanged += XuLyThayDoiGioHang;

            TaiDanhSachDichVu();
            LamMoiNhapDichVu();
            CapNhatTienTamTinh();
        }

        private void MoManHinhThemSuaDichVu()
        {
            dangSuaDichVu = false;
            OnPropertyChanged(nameof(KhongPhaiSua));
            LamMoiNhapDichVu();
            var cuaSoThemSuaDichVu = new W_ThemSuaDVxaml();
            cuaSoThemSuaDichVu.DataContext = this;
            cuaSoThemSuaDichVu.ShowDialog();
        }

        private void MoManHinhSuaDichVu()
        {
            if (DichVuDangChon == null)
            {
                return;
            }

            dangSuaDichVu = true;
            OnPropertyChanged(nameof(KhongPhaiSua));
            MaPTNhap = DichVuDangChon.MaPT;
            TenNhap = DichVuDangChon.Ten;
            GiaNhap = (DichVuDangChon.Gia ?? 0).ToString();
            TonKhoNhap = (DichVuDangChon.TonKho ?? 0).ToString();
            LoaiMatHangNhap = (MaPTNhap != null && MaPTNhap.StartsWith("PT", StringComparison.OrdinalIgnoreCase)) ? 1 : 0;

            var cuaSoThemSuaDichVu = new W_ThemSuaDVxaml();
            cuaSoThemSuaDichVu.DataContext = this;
            cuaSoThemSuaDichVu.ShowDialog();
        }

        private void TaiDanhSachDichVu()
        {
            List<DichVuPhuTung> danhSachLoc;

            using (var ctx = new QuanLyBanXeMayEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                danhSachLoc = ctx.DichVuPhuTungs.ToList();
            }

            if (!string.IsNullOrWhiteSpace(TuKhoaTimKiem))
            {
                string tuKhoa = TuKhoaTimKiem.Trim().ToLower();
                danhSachLoc = danhSachLoc.Where(item =>
                    (item.MaPT ?? string.Empty).ToLower().Contains(tuKhoa) ||
                    (item.Ten ?? string.Empty).ToLower().Contains(tuKhoa)).ToList();
            }

            DanhSachDichVu = new ObservableCollection<DichVuPhuTung>(danhSachLoc);

            if (DichVuDangChon != null)
            {
                string maDangChon = DichVuDangChon.MaPT;
                string maDangChonLocal = maDangChon;
                DichVuDangChon = DanhSachDichVu.FirstOrDefault(item =>
                    item.MaPT == maDangChonLocal);
            }

            lenhMoSuaDichVu.RaiseCanExecuteChanged();
            lenhXoaDichVu.RaiseCanExecuteChanged();
        }

        private bool KiemTraDuLieuDichVu(out int gia, out int tonKho)
        {
            gia = 0;
            tonKho = 0;

            if (string.IsNullOrWhiteSpace(TenNhap))
            {
                MessageBox.Show("Tên dịch vụ/phụ tùng không được để trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            string giaChuanHoa;
            if (!ThuChuanHoaGia(GiaNhap, out gia, out giaChuanHoa))
            {
                MessageBox.Show("Đơn giá không hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            GiaNhap = giaChuanHoa;

            if (string.IsNullOrWhiteSpace(TonKhoNhap))
            {
                tonKho = 0;
            }
            else if (!int.TryParse(TonKhoNhap, out tonKho) || tonKho < 0)
            {
                MessageBox.Show("Tồn kho không hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private bool KiemTraMaDichVu(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma) || ma.Trim().Length < 5)
            {
                return false;
            }

            string giaTri = ma.Trim().ToUpper();
            bool hopLeTienTo = giaTri.StartsWith("DV") || giaTri.StartsWith("PT");
            if (!hopLeTienTo)
            {
                return false;
            }

            string phanSo = giaTri.Substring(2);
            int so;
            return int.TryParse(phanSo, out so);
        }

        private bool ThuChuanHoaGia(string giaNhap, out int gia, out string giaChuanHoa)
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

        private string TaoMaMoi(string tienTo)
        {
            using (var ctx = new QuanLyBanXeMayEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var ds = ctx.DichVuPhuTungs
                    .Where(d => d.MaPT.StartsWith(tienTo))
                    .Select(d => d.MaPT)
                    .ToList();
                
                var numbers = ds.Select(m => {
                    int num;
                    if (int.TryParse(m.Substring(tienTo.Length), out num)) return num;
                    return 0;
                }).Where(n => n > 0).OrderBy(n => n).ToList();

                int nextNum = 1;
                foreach (int n in numbers)
                {
                    if (n == nextNum)
                    {
                        nextNum++;
                    }
                    else if (n > nextNum)
                    {
                        break;
                    }
                }
                return tienTo + nextNum.ToString("D3");
            }
        }

        private void LuuDichVu(Window cuaSo)
        {
            int gia;
            int tonKho;
            if (!KiemTraDuLieuDichVu(out gia, out tonKho))
            {
                return;
            }

            DichVuPhuTung trungTen;
            using (var ctx = new QuanLyBanXeMayEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                string tenLower = TenNhap.Trim().ToLower();
                trungTen = ctx.DichVuPhuTungs.ToList().FirstOrDefault(item =>
                    item.Ten != null && item.Ten.Trim().ToLower() == tenLower);
            }

            if (trungTen != null && (DichVuDangChon == null || trungTen.MaPT != DichVuDangChon.MaPT))
            {
                MessageBox.Show("Tên dịch vụ/phụ tùng đã tồn tại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dangSuaDichVu && DichVuDangChon != null)
            {
                using (var ctx = new QuanLyBanXeMayEntities())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    var ef = ctx.DichVuPhuTungs.FirstOrDefault(d => d.MaPT == DichVuDangChon.MaPT);
                    if (ef != null)
                    {
                        ef.MaPT = MaPTNhap.Trim().ToUpper();
                        ef.Ten = TenNhap.Trim();
                        ef.Gia = gia;
                        ef.TonKho = tonKho;
                        ctx.SaveChanges();
                    }
                }

                TaiDanhSachDichVu();
                DichVuDangChon = DanhSachDichVu.FirstOrDefault(d => d.MaPT == MaPTNhap.Trim().ToUpper());
            }
            else
            {
                string tienTo = LoaiMatHangNhap == 0 ? "DV" : "PT";
                string maMoi = TaoMaMoi(tienTo);

                using (var ctx = new QuanLyBanXeMayEntities())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    var ef = new DichVuPhuTung
                    {
                        MaPT = maMoi,
                        Ten = TenNhap.Trim(),
                        Gia = gia,
                        TonKho = tonKho
                    };
                    ctx.DichVuPhuTungs.Add(ef);
                    ctx.SaveChanges();
                }

                TaiDanhSachDichVu();
                DichVuDangChon = DanhSachDichVu.FirstOrDefault(d => d.MaPT == maMoi);
            }

            dangSuaDichVu = false;
            TaiDanhSachDichVu();
            cuaSo?.Close();
        }

        private void XoaDichVu()
        {
            if (DichVuDangChon == null)
            {
                return;
            }

            var ketQua = MessageBox.Show("Bạn có chắc muốn xóa dịch vụ/phụ tùng đang chọn?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (ketQua != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                using (var ctx = new QuanLyBanXeMayEntities())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    var ef = ctx.DichVuPhuTungs.FirstOrDefault(d => d.MaPT == DichVuDangChon.MaPT);
                    if (ef != null)
                    {
                        ctx.DichVuPhuTungs.Remove(ef);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa mặt hàng này vì đã có dữ liệu liên quan (ví dụ: hóa đơn cũ).\nChi tiết lỗi: " + ex.Message, "Lỗi xóa dữ liệu", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DichVuDangChon = null;
            LamMoiNhapDichVu();
            TaiDanhSachDichVu();
        }

        private void DongFormDichVu(Window cuaSo)
        {
            dangSuaDichVu = false;
            LamMoiNhapDichVu();
            cuaSo?.Close();
        }

        private void LamMoiNhapDichVu()
        {
            MaPTNhap = string.Empty;
            TenNhap = string.Empty;
            GiaNhap = "0";
            TonKhoNhap = "0";
            LoaiMatHangNhap = 0;
        }

        private void ThemVaoGio(DichVuPhuTung dichVu)
        {
            if (dichVu == null)
            {
                return;
            }

            if (LaPhuTung(dichVu.MaPT) && dichVu.TonKho <= 0)
            {
                MessageBox.Show("Phụ tùng đã hết tồn kho.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MatHangGio_VM matHang = GioHangTam.FirstOrDefault(item =>
                item.MaMatHang == dichVu.MaPT);

            if (matHang == null)
            {
                GioHangTam.Add(new MatHangGio_VM
                {
                    MaMatHang = dichVu.MaPT,
                    TenMatHang = dichVu.Ten,
                    DonGia = (decimal)(dichVu.Gia ?? 0),
                    SoLuong = 1
                });
            }
            else
            {
                if (LaPhuTung(dichVu.MaPT) && matHang.SoLuong + 1 > dichVu.TonKho)
                {
                    MessageBox.Show("Số lượng trong giỏ vượt quá tồn kho hiện có.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                matHang.SoLuong += 1;
            }

            CapNhatTienTamTinh();
        }

        private void TangSoLuongTrongGio(MatHangGio_VM matHang)
        {
            if (matHang == null)
            {
                return;
            }

            if (matHang.LaPhuTung)
            {
                using (var ctx = new QuanLyBanXeMayEntities())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    var phuTung = ctx.DichVuPhuTungs.FirstOrDefault(item =>
                        item.MaPT == matHang.MaMatHang);
                    if (phuTung != null && matHang.SoLuong + 1 > (phuTung.TonKho ?? 0))
                    {
                        MessageBox.Show("Số lượng trong giỏ vượt quá tồn kho hiện có.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
            }

            matHang.SoLuong += 1;
            CapNhatTienTamTinh();
        }

        private void GiamSoLuongTrongGio(MatHangGio_VM matHang)
        {
            if (matHang == null)
            {
                return;
            }

            if (matHang.SoLuong <= 1)
            {
                GioHangTam.Remove(matHang);
            }
            else
            {
                matHang.SoLuong -= 1;
            }

            CapNhatTienTamTinh();
        }

        private void XoaKhoiGio(MatHangGio_VM matHang)
        {
            if (matHang == null)
            {
                return;
            }

            GioHangTam.Remove(matHang);
            CapNhatTienTamTinh();
        }

        private void ChuyenSangThanhToan()
        {
            if (GioHangTam.Count == 0)
            {
                MessageBox.Show("Giỏ hàng tạm tính đang trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DieuHuongTuMain("ThanhToan");
        }

        private void DieuHuongTuMain(string tenManHinh, object duLieu = null)
        {
            var mainVm = Application.Current?.MainWindow?.DataContext as MainWindows_VM;
            if (mainVm != null)
            {
                mainVm.DieuHuong(tenManHinh, duLieu);
            }
        }

        private bool LaPhuTung(string ma)
        {
            return !string.IsNullOrWhiteSpace(ma) && ma.Trim().StartsWith("PT", StringComparison.OrdinalIgnoreCase);
        }

        private void XuLyThayDoiGioHang(object nguon, System.Collections.Specialized.NotifyCollectionChangedEventArgs suKien)
        {
            CapNhatTienTamTinh();
        }

        private void CapNhatTienTamTinh()
        {
            OnPropertyChanged(nameof(TongTienTamTinh));
            OnPropertyChanged(nameof(GioHangTam));
        }
    }
}
