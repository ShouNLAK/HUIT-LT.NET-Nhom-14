using Doan_NET.Helper;
using Doan_NET.Model;
using Doan_NET.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Doan_NET.ViewModel
{
    public class XuLyDon_VM : BaseViewModel
    {
        private DonChoXuLy_VM don;
        private Window window;

        public string KhachHangTieuDe { get; set; }
        public string SdtTieuDe { get; set; }
        public string DiaChiTieuDe { get; set; }
        public string MatHangMoTa { get; set; }
        public string TongTienMoTa { get; set; }

        private ObservableCollection<NhanVien> danhSachNhanVien;
        public ObservableCollection<NhanVien> DanhSachNhanVien
        {
            get { return danhSachNhanVien; }
            set { danhSachNhanVien = value; OnPropertyChanged(); }
        }

        private NhanVien nhanVienChon;
        public NhanVien NhanVienChon
        {
            get { return nhanVienChon; }
            set { nhanVienChon = value; OnPropertyChanged(); }
        }

        public ICommand LenhHuy { get; }
        public ICommand LenhXacNhan { get; }

        public XuLyDon_VM(DonChoXuLy_VM don, Window window)
        {
            this.don = don;
            this.window = window;

            KhachHangTieuDe = don.TenKhachHang;
            SdtTieuDe = "SĐT: " + don.SDT;
            DiaChiTieuDe = "Địa chỉ: " + don.DiaChi;
            MatHangMoTa = don.MoTaMatHang;
            TongTienMoTa = string.Format("Tổng tiền: {0:N0} đ", don.TongTien);

            LenhHuy = new RelayCommand(_ => Huy());
            LenhXacNhan = new RelayCommand(_ => XacNhan());

            TaiDanhSachNhanVien();
        }

        private void TaiDanhSachNhanVien()
        {
            using (var ctx = new QuanLyBanXeMayEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var ds = ctx.NhanViens
                    .Where(nv => nv.TrangThai == "Đang làm việc")
                    .OrderBy(nv => nv.HoTen)
                    .ToList();
                DanhSachNhanVien = new ObservableCollection<NhanVien>(ds);
                NhanVienChon = DanhSachNhanVien.FirstOrDefault();
            }
        }

        private void Huy()
        {
            if (window != null)
            {
                window.DialogResult = false;
                window.Close();
            }
        }

        private void XacNhan()
        {
            if (NhanVienChon == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên phụ trách.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var maHDList = don.DanhSachMaHD;
                using (var ctx = new QuanLyBanXeMayEntities())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    var cacDong = ctx.HoaDons.Where(h => maHDList.Contains(h.MaHD)).ToList();
                    foreach (var hd in cacDong)
                    {
                        hd.MaNV = NhanVienChon.MaNV;
                        hd.TrangThai = "Đã xác nhận";
                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xử lý đơn thất bại: " + ex.Message, "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Đã xác nhận đơn. Khách sẽ thấy đơn 'Đã xác nhận' và được mời đến cơ sở nhận xe.",
                "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

            if (don.DanhSachMaHD != null && don.DanhSachMaHD.Count > 0)
            {
                var maHD = don.DanhSachMaHD.FirstOrDefault();
                W_ReportHoaDon frmReport = new W_ReportHoaDon(maHD);
                frmReport.Show();
            }

            if (window != null)
            {
                window.DialogResult = true;
                window.Close();
            }
        }
    }
}
