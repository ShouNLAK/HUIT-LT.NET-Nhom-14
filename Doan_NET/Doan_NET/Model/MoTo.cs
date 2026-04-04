using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doan_NET.Model
{
    internal class MoTo
    {
        private string tenDongXe;
        private string loaiXe;
        private string giaXe;
        private string hinhAnhFullPath;
        private string moTa;
        private int namSX;

        public string TenDongXe
        {
            get { return tenDongXe; }
            set { tenDongXe = value; }
        }
        public string LoaiXe
        {
            get { return loaiXe; }
            set { loaiXe = value; }
        }
        public string GiaXe
        {
            get { return giaXe; }
            set { giaXe = value; }
        }
        public string HinhAnhFullPath
        {
            get { return hinhAnhFullPath; }
            set { hinhAnhFullPath = value; }
        }
        public string MoTa
        {
            get { return moTa; }
            set { moTa = value; }
        }
        public int NamSX
        {
            get { return namSX; }
            set { namSX = value; }
        }

        public MoTo() { }
        public MoTo(string tenDongXe, string loaiXe, int namSX, string giaXe, string hinhAnhFullPath, string moTa)
        {
            TenDongXe = tenDongXe;
            LoaiXe = loaiXe;
            NamSX = namSX;
            GiaXe = giaXe;
            HinhAnhFullPath = hinhAnhFullPath;
            MoTa = moTa;
        }
    }
}
