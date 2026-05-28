using System.Windows;
using Doan_NET.Report;

namespace Doan_NET.View
{
    public partial class W_ReportHoaDon : Window
    {
        public W_ReportHoaDon(string maHD)
        {
            InitializeComponent();
            crp_HoaDon rpt = new crp_HoaDon();
            if (!string.IsNullOrEmpty(maHD))
            {
                rpt.RecordSelectionFormula = "{HoaDon.MaHD} = '" + maHD + "'";
            }
            rpt.SetDatabaseLogon("sa", "123", "DESKTOP-P1D5RMO\\MSSQLSERVER2025", "QuanLyBanXeMay");
            ViewerHoaDon.ViewerCore.ReportSource = rpt;
        }
    }
}
