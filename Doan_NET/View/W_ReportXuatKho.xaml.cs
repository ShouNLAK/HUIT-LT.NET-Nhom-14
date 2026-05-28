using System.Windows;
using Doan_NET.Report;

namespace Doan_NET.View
{
    public partial class W_ReportXuatKho : Window
    {
        public W_ReportXuatKho()
        {
            InitializeComponent();
            crp_XuatKho rpt = new crp_XuatKho();
            rpt.SetDatabaseLogon("sa", "123", "DESKTOP-P1D5RMO\\MSSQLSERVER2025", "QuanLyBanXeMay");
            ViewerXuatKho.ViewerCore.ReportSource = rpt;
        }
    }
}
