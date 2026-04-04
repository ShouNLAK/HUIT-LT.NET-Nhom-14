using Doan_NET.Helper;
using Doan_NET.View;
using System.Windows.Input;

namespace Doan_NET.ViewModel
{
    public class DichVu_VM : BaseViewModel
    {
        public ICommand LenhMoThemDichVu { get; }
        public ICommand LenhMoSuaDichVu { get; }
        public ICommand LenhThanhToan { get; }

        public DichVu_VM()
        {
            LenhMoThemDichVu = new RelayCommand(_ => MoManHinhThemSuaDichVu());
            LenhMoSuaDichVu = new RelayCommand(_ => MoManHinhThemSuaDichVu());
            LenhThanhToan = new RelayCommand(_ => NavigationService.Navigate("DonHang"));
        }

        private void MoManHinhThemSuaDichVu()
        {
            var cuaSoThemSuaDichVu = new W_ThemSuaDVxaml();
            cuaSoThemSuaDichVu.ShowDialog();
        }
    }
}
