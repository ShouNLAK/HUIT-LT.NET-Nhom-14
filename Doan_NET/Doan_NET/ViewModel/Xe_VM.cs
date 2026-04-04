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
    internal class Xe_VM : BaseViewModel
    {
        // Danh sach xe hien thi tren UC_DSXe.
        private ObservableCollection<MoTo> danhSachXe;
        public ObservableCollection<MoTo> DanhSachXe
        {
            get { return danhSachXe; }
            set
            {
                danhSachXe = value;
                OnPropertyChanged("DanhSachXe");
            }
        }

        private MoTo xeDangChon;
        public MoTo XeDangChon
        {
            get { return xeDangChon; }
            set
            {
                xeDangChon = value;
                OnPropertyChanged();
            }
        }

        public ICommand LenhQuayLaiHangXe { get; }
        public ICommand LenhMoThemXe { get; }
        public ICommand LenhMoSuaXe { get; }

        public Xe_VM()
        {
            LenhQuayLaiHangXe = new RelayCommand(_ => NavigationService.Navigate("QuanLyXe"));
            LenhMoThemXe = new RelayCommand(_ => MoThemXe());
            LenhMoSuaXe = new RelayCommand(_ => MoSuaXe(), _ => XeDangChon != null);

            DanhSachXe = new ObservableCollection<MoTo>()
            {
                new MoTo {
                    TenDongXe = "Honda Wave RSX",
                    LoaiXe = "Xe số",
                    NamSX =2024,
                    GiaXe = "22.000.000 VNĐ",
                    HinhAnhFullPath = "https://via.placeholder.com/100",
                    MoTa = "Xe số bền bỉ, tiết kiệm xăng."
                },
                new MoTo {
                    TenDongXe = "Honda Vision",
                    LoaiXe = "Xe ga",
                    NamSX =2024,
                    GiaXe = "33.000.000 VNĐ",
                    HinhAnhFullPath = "https://via.placeholder.com/100",
                    MoTa = "Xe ga quốc dân, thiết kế thời trang."
                },
                new MoTo {
                    TenDongXe = "Honda Winner X",
                    LoaiXe = "Xe tay côn",
                    NamSX =2024,
                    GiaXe = "46.000.000 VNĐ",
                    HinhAnhFullPath = "https://via.placeholder.com/100",
                    MoTa = "Sức mạnh vượt trội, phong cách thể thao."
                }
            };
        }

        private void MoThemXe()
        {
            var cuaSoThemXe = new W_ThemXe();
            cuaSoThemXe.ShowDialog();
        }

        private void MoSuaXe()
        {
            if (XeDangChon == null)
            {
                return;
            }

            var cuaSoSuaXe = new W_SuaXe();
            cuaSoSuaXe.ShowDialog();
        }
    }
}
