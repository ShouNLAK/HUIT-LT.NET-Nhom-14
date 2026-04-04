using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doan_NET.Model
{
    internal class HoaDon
    {
        private string maHD;
        private string ngayLap;
        private string tenNhanVien;
        private string tenKhachHang;
        private string sdt;
        private string tenDV_SP;
        private int soLuong;
        private string thanhTien;
        private string phuongThucThanhToan;

        public string MaHD { get { return maHD; } set { maHD = value; } }
        public string NgayLap { get { return ngayLap; } set { ngayLap = value; } }
        public string TenNhanVien { get { return tenNhanVien; } set { tenNhanVien = value; } }
        public string TenKhachHang { get { return tenKhachHang; } set { tenKhachHang = value; } }
        public string SDT { get { return sdt; } set { sdt = value; } }
        public string TenDV_SP { get { return tenDV_SP; } set { tenDV_SP = value; } }
        public int SoLuong { get { return soLuong; } set { soLuong = value; } }
        public string ThanhTien { get { return thanhTien; } set { thanhTien = value; } }
        public string PhuongThucThanhToan { get { return phuongThucThanhToan; } set { phuongThucThanhToan = value; } }

        public HoaDon() { }
        public HoaDon(string maHD, string ngayLap, string tenNhanVien, string tenKhachHang, string sdt, string tenDV_SP, int soLuong, string thanhTien, string phuongThucThanhToan)
        {
            MaHD = maHD;
            NgayLap = ngayLap;
            TenNhanVien = tenNhanVien;
            TenKhachHang = tenKhachHang;
            SDT = sdt;
            TenDV_SP = tenDV_SP;
            SoLuong = soLuong;
            ThanhTien = thanhTien;
            PhuongThucThanhToan = phuongThucThanhToan;
        }
    }
}