using System.Windows;
using Doan_NET.Report;

namespace Doan_NET.View
{
    public partial class W_ReportThongKe : Window
    {
        public W_ReportThongKe()
        {
            InitializeComponent();
            crp_ThongKe rpt = new crp_ThongKe();
            rpt.SetDatabaseLogon("sa", "123", "DESKTOP-P1D5RMO\\MSSQLSERVER2025", "QuanLyBanXeMay");
            ViewerThongKe.ViewerCore.ReportSource = rpt;
        }
    }
}
