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

namespace Doan_NET.ViewModel
{
    public class NhanVien_VM : BaseViewModel
    {
        private ObservableCollection<NhanVien> danhSachNhanVien;
        public ObservableCollection<NhanVien> DanhSachNhanVien
        {
            get { return danhSachNhanVien; }
            set
            {
                danhSachNhanVien = value;
                OnPropertyChanged();
            }
        }

        private NhanVien nhanVienDangChon;
        public NhanVien NhanVienDangChon
        {
            get { return nhanVienDangChon; }
            set
            {
                nhanVienDangChon = value;
                OnPropertyChanged();
                if (nhanVienDangChon != null)
                {
                    MaNVNhap = nhanVienDangChon.MaNV;
                    HoTenNhap = nhanVienDangChon.HoTen;
                    SDTNhap = nhanVienDangChon.SDT;
                    ChucVuNhap = nhanVienDangChon.ChucVu;
                    NgayVaoLamNhap = nhanVienDangChon.NgayVaoLam;
                    TrangThaiNhap = nhanVienDangChon.TrangThai;
                    dangThemMoi = false;
                    dangSua = false;
                }
                lenhSuaNhanVien?.RaiseCanExecuteChanged();
                lenhXoaNhanVien?.RaiseCanExecuteChanged();
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

        private string maNVNhap;
        public string MaNVNhap
        {
            get { return maNVNhap; }
            set
            {
                maNVNhap = value;
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

        private string chucVuNhap;
        public string ChucVuNhap
        {
            get { return chucVuNhap; }
            set
            {
                chucVuNhap = value;
                OnPropertyChanged();
            }
        }

        private DateTime ngayVaoLamNhap;
        public DateTime NgayVaoLamNhap
        {
            get { return ngayVaoLamNhap; }
            set
            {
                ngayVaoLamNhap = value;
                OnPropertyChanged();
            }
        }

        private string trangThaiNhap;
        public string TrangThaiNhap
        {
            get { return trangThaiNhap; }
            set
            {
                trangThaiNhap = value;
                OnPropertyChanged();
            }
        }

        private bool dangThemMoi;
        private bool dangSua;
        private readonly RelayCommand lenhSuaNhanVien;
        private readonly RelayCommand lenhXoaNhanVien;

        public ICommand LenhThemNhanVien { get; }
        public ICommand LenhSuaNhanVien
        {
            get { return lenhSuaNhanVien; }
        }
        public ICommand LenhXoaNhanVien
        {
            get { return lenhXoaNhanVien; }
        }
        public ICommand LenhLuuNhanVien { get; }
        public ICommand LenhTimKiemNhanVien { get; }
        public ICommand LenhHuyNhapNhanVien { get; }

        public NhanVien_VM()
        {
            LenhThemNhanVien = new RelayCommand(_ => ThemNhanVien());
            lenhSuaNhanVien = new RelayCommand(_ => SuaNhanVien(), _ => NhanVienDangChon != null);
            lenhXoaNhanVien = new RelayCommand(_ => XoaNhanVien(), _ => NhanVienDangChon != null);
            LenhLuuNhanVien = new RelayCommand(_ => LuuNhanVien());
            LenhTimKiemNhanVien = new RelayCommand(_ => TaiDanhSachNhanVien());
            LenhHuyNhapNhanVien = new RelayCommand(_ => HuyNhapNhanVien());

            DanhSachNhanVien = new ObservableCollection<NhanVien>();
            NgayVaoLamNhap = DateTime.Now;
            TrangThaiNhap = "Đang làm việc";
            TaiDanhSachNhanVien();
        }

        private void TaiDanhSachNhanVien()
        {
            IEnumerable<NhanVien> danhSachLoc = DuLieuHeThong.DanhSachNhanVien;

            if (!string.IsNullOrWhiteSpace(TuKhoaTimKiem))
            {
                string tuKhoa = TuKhoaTimKiem.Trim().ToLower();
                danhSachLoc = danhSachLoc.Where(item =>
                    (item.MaNV ?? string.Empty).ToLower().Contains(tuKhoa) ||
                    (item.HoTen ?? string.Empty).ToLower().Contains(tuKhoa) ||
                    (item.SDT ?? string.Empty).ToLower().Contains(tuKhoa) ||
                    (item.ChucVu ?? string.Empty).ToLower().Contains(tuKhoa));
            }

            DanhSachNhanVien = new ObservableCollection<NhanVien>(danhSachLoc);

            if (NhanVienDangChon != null)
            {
                string maDangChon = NhanVienDangChon.MaNV;
                NhanVienDangChon = DanhSachNhanVien.FirstOrDefault(item =>
                    string.Equals(item.MaNV, maDangChon, StringComparison.OrdinalIgnoreCase));
            }

            lenhSuaNhanVien.RaiseCanExecuteChanged();
            lenhXoaNhanVien.RaiseCanExecuteChanged();
        }

        private void ThemNhanVien()
        {
            dangThemMoi = true;
            dangSua = false;
            MaNVNhap = TaoMaNhanVienMoi();
            HoTenNhap = string.Empty;
            SDTNhap = string.Empty;
            ChucVuNhap = string.Empty;
            NgayVaoLamNhap = DateTime.Now;
            TrangThaiNhap = "Đang làm việc";
            NhanVienDangChon = null;
        }

        private void SuaNhanVien()
        {
            if (NhanVienDangChon == null)
            {
                return;
            }

            dangThemMoi = false;
            dangSua = true;
            MaNVNhap = NhanVienDangChon.MaNV;
            HoTenNhap = NhanVienDangChon.HoTen;
            SDTNhap = NhanVienDangChon.SDT;
            ChucVuNhap = NhanVienDangChon.ChucVu;
            NgayVaoLamNhap = NhanVienDangChon.NgayVaoLam;
            TrangThaiNhap = NhanVienDangChon.TrangThai;
        }

        private void XoaNhanVien()
        {
            try
            {
                if (NhanVienDangChon == null)
                {
                    return;
                }

                MessageBoxResult ketQua = MessageBox.Show("Bạn có chắc muốn xóa nhân viên đang chọn?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (ketQua != MessageBoxResult.Yes)
                {
                    return;
                }

                DuLieuHeThong.DanhSachNhanVien.Remove(NhanVienDangChon);
                NhanVienDangChon = null;
                HuyNhapNhanVien();
                TaiDanhSachNhanVien();
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể xóa nhân viên. Vui lòng thử lại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LuuNhanVien()
        {
            try
            {
                if (!KiemTraDuLieuNhanVien())
                {
                    return;
                }

                if (dangThemMoi)
                {
                    NhanVien nhanVienTrung = DuLieuHeThong.DanhSachNhanVien.FirstOrDefault(item =>
                        string.Equals(item.MaNV, MaNVNhap, StringComparison.OrdinalIgnoreCase));
                    if (nhanVienTrung != null)
                    {
                        MessageBox.Show("Mã nhân viên đã tồn tại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (KiemTraTrungSDTNhanVien(null))
                    {
                        return;
                    }

                    var nhanVienMoi = new NhanVien
                    {
                        MaNV = MaNVNhap.Trim().ToUpper(),
                        HoTen = HoTenNhap.Trim(),
                        SDT = SDTNhap.Trim(),
                        ChucVu = ChucVuNhap.Trim(),
                        NgayVaoLam = NgayVaoLamNhap,
                        TrangThai = ChuanHoaTrangThai(TrangThaiNhap)
                    };

                    DuLieuHeThong.DanhSachNhanVien.Add(nhanVienMoi);
                    dangThemMoi = false;
                    TaiDanhSachNhanVien();
                    NhanVienDangChon = DanhSachNhanVien.FirstOrDefault(item => item.MaNV == nhanVienMoi.MaNV);
                    return;
                }

                if (dangSua && NhanVienDangChon != null)
                {
                    NhanVien nhanVienTrungKhac = DuLieuHeThong.DanhSachNhanVien.FirstOrDefault(item =>
                        item != NhanVienDangChon &&
                        string.Equals(item.MaNV, MaNVNhap, StringComparison.OrdinalIgnoreCase));
                    if (nhanVienTrungKhac != null)
                    {
                        MessageBox.Show("Mã nhân viên đã tồn tại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (KiemTraTrungSDTNhanVien(NhanVienDangChon))
                    {
                        return;
                    }

                    int viTri = DuLieuHeThong.DanhSachNhanVien.IndexOf(NhanVienDangChon);
                    if (viTri < 0)
                    {
                        NhanVien nhanVienCanSua = DuLieuHeThong.DanhSachNhanVien.FirstOrDefault(item =>
                            string.Equals(item.MaNV, NhanVienDangChon.MaNV, StringComparison.OrdinalIgnoreCase));
                        viTri = nhanVienCanSua != null ? DuLieuHeThong.DanhSachNhanVien.IndexOf(nhanVienCanSua) : -1;
                    }

                    if (viTri >= 0)
                    {
                        var nhanVienMoi = new NhanVien
                        {
                            MaNV = MaNVNhap.Trim().ToUpper(),
                            HoTen = HoTenNhap.Trim(),
                            SDT = SDTNhap.Trim(),
                            ChucVu = ChucVuNhap.Trim(),
                            NgayVaoLam = NgayVaoLamNhap,
                            TrangThai = ChuanHoaTrangThai(TrangThaiNhap)
                        };

                        DuLieuHeThong.DanhSachNhanVien[viTri] = nhanVienMoi;
                        dangSua = false;
                        TaiDanhSachNhanVien();
                        NhanVienDangChon = DanhSachNhanVien.FirstOrDefault(item => item.MaNV == nhanVienMoi.MaNV);
                    }
                    return;
                }

                MessageBox.Show("Vui lòng bấm Thêm hoặc Sửa trước khi Lưu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể lưu dữ liệu nhân viên. Vui lòng thử lại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool KiemTraTrungSDTNhanVien(NhanVien nhanVienDangBoQua)
        {
            string sdt = SDTNhap.Trim();
            NhanVien nhanVienTrungSDT = DuLieuHeThong.DanhSachNhanVien.FirstOrDefault(item =>
                item != nhanVienDangBoQua &&
                string.Equals(item.SDT, sdt, StringComparison.OrdinalIgnoreCase));

            if (nhanVienTrungSDT != null)
            {
                MessageBox.Show("Số điện thoại đã tồn tại trong hệ thống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return true;
            }

            return false;
        }

        private string ChuanHoaTrangThai(string trangThai)
        {
            string giaTri = (trangThai ?? string.Empty).Trim();

            if (string.Equals(giaTri, "Dang lam viec", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(giaTri, "Đang làm việc", StringComparison.OrdinalIgnoreCase))
            {
                return "Đang làm việc";
            }

            return "Tạm nghỉ";
        }

        private bool KiemTraDuLieuNhanVien()
        {
            if (string.IsNullOrWhiteSpace(MaNVNhap))
            {
                MessageBox.Show("Mã nhân viên không được để trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
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

            if (string.IsNullOrWhiteSpace(ChucVuNhap))
            {
                MessageBox.Show("Chức vụ không được để trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (NgayVaoLamNhap > DateTime.Now)
            {
                MessageBox.Show("Ngày vào làm không hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(TrangThaiNhap))
            {
                MessageBox.Show("Trạng thái không được để trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!string.Equals(TrangThaiNhap.Trim(), "Dang lam viec", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(TrangThaiNhap.Trim(), "Tam nghi", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(TrangThaiNhap.Trim(), "Đang làm việc", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(TrangThaiNhap.Trim(), "Tạm nghỉ", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Trạng thái chỉ được là Đang làm việc hoặc Tạm nghỉ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
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

        private string TaoMaNhanVienMoi()
        {
            int maLonNhat = 0;
            foreach (NhanVien item in DuLieuHeThong.DanhSachNhanVien)
            {
                if (item == null || string.IsNullOrWhiteSpace(item.MaNV))
                {
                    continue;
                }

                string so = item.MaNV.Trim().ToUpper().Replace("NV", string.Empty);
                int maSo;
                if (int.TryParse(so, out maSo) && maSo > maLonNhat)
                {
                    maLonNhat = maSo;
                }
            }

            return "NV" + (maLonNhat + 1).ToString("000");
        }

        private void HuyNhapNhanVien()
        {
            dangThemMoi = false;
            dangSua = false;
            MaNVNhap = string.Empty;
            HoTenNhap = string.Empty;
            SDTNhap = string.Empty;
            ChucVuNhap = string.Empty;
            NgayVaoLamNhap = DateTime.Now;
            TrangThaiNhap = "Đang làm việc";
        }
    }
}
