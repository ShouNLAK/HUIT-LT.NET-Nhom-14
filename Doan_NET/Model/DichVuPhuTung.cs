using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doan_NET.Model
{
    public class DichVuPhuTung
    {
        private string maPT;
        private string ten;
        private int gia;
        private int tonKho;

        public string MaPT { get { return maPT; } set { maPT = value; } }
        public string Ten { get { return ten; } set { ten = value; } }
        public int Gia { get { return gia; } set { gia = value; } }
        public int TonKho { get { return tonKho; } set { tonKho = value; } }

        public DichVuPhuTung() { }
        public DichVuPhuTung(string maPT, string ten, int gia, int tonKho)
        {
            MaPT = maPT;
            Ten = ten;
            Gia = gia;
            TonKho = tonKho;
        }
    }
}