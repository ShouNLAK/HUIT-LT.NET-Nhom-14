using Doan_NET.Helper;
using Doan_NET.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Doan_NET.View;
using System.Windows;

namespace Doan_NET.ViewModel
{
    public class HangXe_VM : BaseViewModel
    {
        private ObservableCollection<HangXe> danhSachHangXe;
        public ObservableCollection<HangXe> DanhSachHangXe
        {
            get { return danhSachHangXe; }
            set
            {
                danhSachHangXe = value;
                OnPropertyChanged("DanhSachHangXe");
            }
        }

        private HangXe hangXeDangChon;
        public HangXe HangXeDangChon
        {
            get { return hangXeDangChon; }
            set
            {
                hangXeDangChon = value;
                OnPropertyChanged();
                if (hangXeDangChon != null)
                {
                    TenHangNhap = hangXeDangChon.TenHang;
                    QuocGiaNhap = hangXeDangChon.QuocGia;
                    LogoNhap = hangXeDangChon.LogoFullPath;
                }
                lenhMoSuaHangXe?.RaiseCanExecuteChanged();
                lenhXoaHangXe?.RaiseCanExecuteChanged();
            }
        }

        private string tenHangNhap;
        public string TenHangNhap
        {
            get { return tenHangNhap; }
            set
            {
                tenHangNhap = value;
                OnPropertyChanged();
            }
        }

        private string quocGiaNhap;
        public string QuocGiaNhap
        {
            get { return quocGiaNhap; }
            set
            {
                quocGiaNhap = value;
                OnPropertyChanged();
            }
        }

        private string logoNhap;
        public string LogoNhap
        {
            get { return logoNhap; }
            set
            {
                logoNhap = value;
                OnPropertyChanged();
            }
        }

        private bool dangSuaHangXe;

        private readonly RelayCommand lenhMoSuaHangXe;
        private readonly RelayCommand lenhXoaHangXe;

        public ICommand LenhMoDanhSachXeTheoHang { get; }
        public ICommand LenhMoThemHangXe { get; }
        public ICommand LenhMoSuaHangXe
        {
            get { return lenhMoSuaHangXe; }
        }
        public ICommand LenhXoaHangXe
        {
            get { return lenhXoaHangXe; }
        }
        public ICommand LenhLuuHangXe { get; }
        public ICommand LenhHuyFormHangXe { get; }

        public HangXe_VM()
        {
            LenhMoDanhSachXeTheoHang = new RelayCommand(
                parameter => MoDanhSachXeTheoHang(parameter as HangXe),
                parameter => parameter is HangXe);

            LenhMoThemHangXe = new RelayCommand(_ => MoThemHangXe());
            lenhMoSuaHangXe = new RelayCommand(_ => MoSuaHangXe(), _ => HangXeDangChon != null);
            lenhXoaHangXe = new RelayCommand(_ => XoaHangXe(), _ => HangXeDangChon != null);
            LenhLuuHangXe = new RelayCommand(parameter => LuuHangXe(parameter as Window));
            LenhHuyFormHangXe = new RelayCommand(parameter => DongFormHangXe(parameter as Window));

            DanhSachHangXe = DuLieuHeThong.DanhSachHangXe;
            LamMoiNhapHangXe();
        }

        private void MoDanhSachXeTheoHang(HangXe hangXeDuocChon)
        {
            if (hangXeDuocChon == null)
            {
                return;
            }

            NavigationService.Navigate("DanhSachXeTheoHang", hangXeDuocChon);
        }

        private void MoThemHangXe()
        {
            dangSuaHangXe = false;
            LamMoiNhapHangXe();
            var cuaSoThemHangXe = new W_ThemHangXe();
            cuaSoThemHangXe.DataContext = this;
            cuaSoThemHangXe.ShowDialog();
        }

        private void MoSuaHangXe()
        {
            if (HangXeDangChon == null)
            {
                return;
            }

            dangSuaHangXe = true;
            TenHangNhap = HangXeDangChon.TenHang;
            QuocGiaNhap = HangXeDangChon.QuocGia;
            LogoNhap = HangXeDangChon.LogoFullPath;
            var cuaSoSuaHangXe = new W_SuaHangXe();
            cuaSoSuaHangXe.DataContext = this;
            cuaSoSuaHangXe.ShowDialog();
        }

        private void LuuHangXe(Window cuaSo)
        {
            try
            {
                if (!KiemTraDuLieuHangXe())
                {
                    return;
                }

                if (dangSuaHangXe && HangXeDangChon != null)
                {
                    HangXe hangTrung = DanhSachHangXe.FirstOrDefault(item =>
                        item != HangXeDangChon &&
                        string.Equals(item.TenHang?.Trim(), TenHangNhap.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (hangTrung != null)
                    {
                        MessageBox.Show("Tên hãng xe đã tồn tại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    int viTri = DanhSachHangXe.IndexOf(HangXeDangChon);
                    if (viTri >= 0)
                    {
                        var hangXeMoi = new HangXe
                        {
                            TenHang = TenHangNhap.Trim(),
                            QuocGia = QuocGiaNhap.Trim(),
                            LogoFullPath = LogoNhap.Trim()
                        };

                        string tenHangCu = HangXeDangChon.TenHang;
                        DanhSachHangXe[viTri] = hangXeMoi;
                        HangXeDangChon = hangXeMoi;

                        foreach (MoTo xe in DuLieuHeThong.DanhSachXe)
                        {
                            if (string.Equals(xe.TenHang, tenHangCu, StringComparison.OrdinalIgnoreCase))
                            {
                                xe.TenHang = hangXeMoi.TenHang;
                            }
                        }
                    }
                }
                else
                {
                    HangXe hangTrung = DanhSachHangXe.FirstOrDefault(item =>
                        string.Equals(item.TenHang?.Trim(), TenHangNhap.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (hangTrung != null)
                    {
                        MessageBox.Show("Tên hãng xe đã tồn tại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    DanhSachHangXe.Add(new HangXe
                    {
                        TenHang = TenHangNhap.Trim(),
                        QuocGia = QuocGiaNhap.Trim(),
                        LogoFullPath = LogoNhap.Trim()
                    });
                }

                dangSuaHangXe = false;
                LamMoiNhapHangXe();
                cuaSo?.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể lưu dữ liệu hãng xe. Vui lòng thử lại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void XoaHangXe()
        {
            try
            {
                if (HangXeDangChon == null)
                {
                    return;
                }

                var ketQua = MessageBox.Show("Bạn có chắc muốn xóa hãng xe đang chọn?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (ketQua != MessageBoxResult.Yes)
                {
                    return;
                }

                string tenHangCanXoa = HangXeDangChon.TenHang;
                DanhSachHangXe.Remove(HangXeDangChon);

                List<MoTo> danhSachXeCanXoa = DuLieuHeThong.DanhSachXe
                    .Where(item => string.Equals(item.TenHang, tenHangCanXoa, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                foreach (MoTo xe in danhSachXeCanXoa)
                {
                    DuLieuHeThong.DanhSachXe.Remove(xe);
                }

                HangXeDangChon = null;
                LamMoiNhapHangXe();
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể xóa hãng xe. Vui lòng thử lại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DongFormHangXe(Window cuaSo)
        {
            dangSuaHangXe = false;
            LamMoiNhapHangXe();
            cuaSo?.Close();
        }

        private bool KiemTraDuLieuHangXe()
        {
            if (string.IsNullOrWhiteSpace(TenHangNhap))
            {
                MessageBox.Show("Tên hãng xe không được để trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (TenHangNhap.Trim().Length < 2)
            {
                MessageBox.Show("Tên hãng xe phải có ít nhất 2 ký tự.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(QuocGiaNhap))
            {
                MessageBox.Show("Quốc gia không được để trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(LogoNhap))
            {
                LogoNhap = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/ac/No_image_available.svg/320px-No_image_available.svg.png";
            }

            if (!LaUrlHopLe(LogoNhap.Trim()))
            {
                MessageBox.Show("Đường dẫn logo phải là URL hợp lệ (http/https).", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private bool LaUrlHopLe(string duongDan)
        {
            Uri uri;
            return Uri.TryCreate(duongDan, UriKind.Absolute, out uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }

        private void LamMoiNhapHangXe()
        {
            TenHangNhap = string.Empty;
            QuocGiaNhap = string.Empty;
            LogoNhap = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/ac/No_image_available.svg/320px-No_image_available.svg.png";
        }
    }
}
