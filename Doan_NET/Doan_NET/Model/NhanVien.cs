using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doan_NET.Model
{
    public class NhanVien
    {
        private string maNV;
        private string hoTen;
        private string sdt;
        private string chucVu;
        private DateTime ngayVaoLam;
        private string trangThai;

        public string MaNV { get { return maNV; } set { maNV = value; } }
        public string HoTen { get { return hoTen; } set { hoTen = value; } }
        public string SDT { get { return sdt; } set { sdt = value; } }
        public string ChucVu { get { return chucVu; } set { chucVu = value; } }
        public DateTime NgayVaoLam { get { return ngayVaoLam; } set { ngayVaoLam = value; } }
        public string TrangThai { get { return trangThai; } set { trangThai = value; } }

        public NhanVien() { }
        public NhanVien(string maNV, string hoTen, string sdt, string chucVu, DateTime ngayVaoLam, string trangThai)
        {
            MaNV = maNV;
            HoTen = hoTen;
            SDT = sdt;
            ChucVu = chucVu;
            NgayVaoLam = ngayVaoLam;
            TrangThai = trangThai;
        }
    }
}