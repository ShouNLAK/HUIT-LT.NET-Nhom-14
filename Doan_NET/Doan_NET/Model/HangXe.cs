using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doan_NET.Model
{
    public class HangXe
    {
        private string tenHang;
        private string quocGia;
        private string logoFullPath;
        public string TenHang
        {
            get { return tenHang; }
            set { tenHang = value; }
        }
        public string QuocGia
        {
            get { return quocGia; }
            set { quocGia = value; }
        }
        public string LogoFullPath
        {
            get { return logoFullPath; }
            set { logoFullPath = value; }
        }

        public HangXe() { }
        public HangXe(string tenHang, string quocGia, string logoFullPath)
        {
            TenHang = tenHang;
            QuocGia = quocGia;
            LogoFullPath = logoFullPath;
        }
    }
}
