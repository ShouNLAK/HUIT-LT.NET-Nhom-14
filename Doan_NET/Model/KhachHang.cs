using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doan_NET.Model
{
    public class KhachHang
    {
        private string maKH;
        private string hoTen;
        private string sdt;
        private string cccd;
        private string email;
        private string diaChi;

        public string MaKH { get { return maKH; } set { maKH = value; } }
        public string HoTen { get { return hoTen; } set { hoTen = value; } }
        public string SDT { get { return sdt; } set { sdt = value; } }
        public string CCCD { get { return cccd; } set { cccd = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string DiaChi { get { return diaChi; } set { diaChi = value; } }

        public KhachHang() { }
        public KhachHang(string maKH, string hoTen, string sdt, string cccd, string email, string diaChi)
        {
            MaKH = maKH;
            HoTen = hoTen;
            SDT = sdt;
            CCCD = cccd;
            Email = email;
            DiaChi = diaChi;
        }
    }
}