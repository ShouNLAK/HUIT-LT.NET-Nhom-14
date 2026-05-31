using Doan_NET.Model;
using Doan_NET.ViewModel;
using System.Windows;

namespace Doan_NET.View
{
    /// <summary>
    /// Interaction logic for W_XuLyDon.xaml
    /// </summary>
    public partial class W_XuLyDon : Window
    {
        public W_XuLyDon(DonChoXuLy_VM don)
        {
            InitializeComponent();
            DataContext = new XuLyDon_VM(don, this);
        }
    }
}
