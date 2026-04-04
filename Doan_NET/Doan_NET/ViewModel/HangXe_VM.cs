using Doan_NET.Helper;
using Doan_NET.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Doan_NET.View;

namespace Doan_NET.ViewModel
{
    public class HangXe_VM : BaseViewModel
    {
        // Danh sach hang xe hien thi tren UC_DSHangXe.
        private ObservableCollection<HangXe> danhSachHangXe;
        public ObservableCollection<HangXe> DanhSachHangXe
        {
            get { return danhSachHangXe; }
            set
            {
                danhSachHangXe = value;
                OnPropertyChanged("DanhSachHangXe");
            }
        }

        // Hang xe dang duoc chon tren danh sach.
        private HangXe hangXeDangChon;
        public HangXe HangXeDangChon
        {
            get { return hangXeDangChon; }
            set
            {
                hangXeDangChon = value;
                OnPropertyChanged();
            }
        }

        public ICommand LenhMoDanhSachXeTheoHang { get; }
        public ICommand LenhMoThemHangXe { get; }
        public ICommand LenhMoSuaHangXe { get; }

        public HangXe_VM()
        {
            LenhMoDanhSachXeTheoHang = new RelayCommand(
                parameter => MoDanhSachXeTheoHang(parameter as HangXe),
                parameter => parameter is HangXe);

            LenhMoThemHangXe = new RelayCommand(_ => MoThemHangXe());
            LenhMoSuaHangXe = new RelayCommand(_ => MoSuaHangXe(), _ => HangXeDangChon != null);

            // Khoi tao du lieu mau
            DanhSachHangXe = new ObservableCollection<HangXe>()
            {
                new HangXe { TenHang="Toyota", QuocGia="Japan", LogoFullPath = "https://via.placeholder.com/80" },
                new HangXe { TenHang="BMW", QuocGia="Germany", LogoFullPath = "https://via.placeholder.com/80"},
                new HangXe { TenHang="Mercedes", QuocGia="Germany", LogoFullPath = "https://via.placeholder.com/80"},
                new HangXe { TenHang="Honda", QuocGia="Japan", LogoFullPath = "https://via.placeholder.com/80"}
            };
        }

        private void MoDanhSachXeTheoHang(HangXe hangXeDuocChon)
        {
            if (hangXeDuocChon == null)
            {
                return;
            }

            NavigationService.Navigate("DanhSachXeTheoHang", hangXeDuocChon);
        }

        private void MoThemHangXe()
        {
            var cuaSoThemHangXe = new W_ThemHangXe();
            cuaSoThemHangXe.ShowDialog();
        }

        private void MoSuaHangXe()
        {
            if (HangXeDangChon == null)
            {
                return;
            }

            var cuaSoSuaHangXe = new W_SuaHangXe();
            cuaSoSuaHangXe.ShowDialog();
        }
    }
}
