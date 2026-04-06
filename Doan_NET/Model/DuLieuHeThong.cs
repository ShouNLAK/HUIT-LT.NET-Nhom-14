using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doan_NET.Model
{
    public static class DuLieuHeThong
    {
        public static ObservableCollection<HangXe> DanhSachHangXe { get; } = new ObservableCollection<HangXe>
        {
            new HangXe { TenHang = "Honda", QuocGia = "Nhật Bản", LogoFullPath = "https://1000logos.net/wp-content/uploads/2018/03/Honda-Logo.png" },
            new HangXe { TenHang = "Yamaha", QuocGia = "Nhật Bản", LogoFullPath = "https://1000logos.net/wp-content/uploads/2020/06/Yamaha-Logo.png" },
            new HangXe { TenHang = "Suzuki", QuocGia = "Nhật Bản", LogoFullPath = "https://1000logos.net/wp-content/uploads/2020/03/Suzuki-Logo.png" },
            new HangXe { TenHang = "Piaggio", QuocGia = "Ý", LogoFullPath = "https://1000logos.net/wp-content/uploads/2021/04/Piaggio-logo.png" }
        };

        public static ObservableCollection<MoTo> DanhSachXe { get; } = new ObservableCollection<MoTo>
        {
            new MoTo
            {
                TenHang = "Honda",
                TenDongXe = "Wave Alpha 110",
                LoaiXe = "Xe số",
                MauSac = "Đỏ đen",
                NamSX = 2024,
                GiaXe = 18190000,
                HinhAnhFullPath = "https://upload.wikimedia.org/wikipedia/commons/7/76/2024_Honda_Wave_110.png",
                MoTa = "Dòng xe số phổ thông, bền bỉ và tiết kiệm nhiên liệu.",
                SoLuongTon = 12
            },
            new MoTo
            {
                TenHang = "Honda",
                TenDongXe = "Vision 110",
                LoaiXe = "Xe tay ga",
                MauSac = "Trắng ngọc trai",
                NamSX = 2024,
                GiaXe = 36500000,
                HinhAnhFullPath = "https://upload.wikimedia.org/wikipedia/commons/c/c1/Honda_Vision_scooter.jpg",
                MoTa = "Mẫu xe tay ga quốc dân, phù hợp đi lại hằng ngày trong đô thị.",
                SoLuongTon = 9
            },
            new MoTo
            {
                TenHang = "Yamaha",
                TenDongXe = "Exciter 155 VVA",
                LoaiXe = "Xe côn tay",
                MauSac = "Xanh GP",
                NamSX = 2024,
                GiaXe = 54800000,
                HinhAnhFullPath = "https://upload.wikimedia.org/wikipedia/commons/f/fb/Yamaha_Y15ZR.jpg",
                MoTa = "Mẫu underbone thể thao, động cơ mạnh và thiết kế trẻ trung.",
                SoLuongTon = 6
            },
            new MoTo
            {
                TenHang = "Yamaha",
                TenDongXe = "Aerox 155 ABS",
                LoaiXe = "Xe tay ga thể thao",
                MauSac = "Đen xám",
                NamSX = 2024,
                GiaXe = 56000000,
                HinhAnhFullPath = "https://upload.wikimedia.org/wikipedia/commons/7/7c/2021_Yamaha_Aerox_155_ABS.jpg",
                MoTa = "Xe tay ga thể thao dành cho khách hàng trẻ, trang bị phanh ABS.",
                SoLuongTon = 5
            },
            new MoTo
            {
                TenHang = "Suzuki",
                TenDongXe = "Raider 150",
                LoaiXe = "Xe côn tay",
                MauSac = "Đen đỏ",
                NamSX = 2024,
                GiaXe = 49190000,
                HinhAnhFullPath = "https://upload.wikimedia.org/wikipedia/commons/4/4d/Suzuki_Raider_150.jpg",
                MoTa = "Mẫu côn tay tăng tốc tốt, phù hợp khách hàng yêu thích cảm giác lái.",
                SoLuongTon = 4
            },
            new MoTo
            {
                TenHang = "Suzuki",
                TenDongXe = "Burgman Street 125",
                LoaiXe = "Xe tay ga",
                MauSac = "Xanh đen",
                NamSX = 2024,
                GiaXe = 48600000,
                HinhAnhFullPath = "https://upload.wikimedia.org/wikipedia/commons/e/eb/2023_Suzuki_Burgman_Street_125_EX.jpg",
                MoTa = "Thiết kế touring đô thị, tư thế ngồi thoải mái cho quãng đường dài.",
                SoLuongTon = 3
            },
            new MoTo
            {
                TenHang = "Piaggio",
                TenDongXe = "Vespa Sprint 150",
                LoaiXe = "Xe tay ga cao cấp",
                MauSac = "Trắng sứ",
                NamSX = 2024,
                GiaXe = 85000000,
                HinhAnhFullPath = "https://upload.wikimedia.org/wikipedia/commons/4/45/Vespa_150_Sprint_V_%282019-09-08_Sp_1777_d2%29.jpg",
                MoTa = "Phong cách Ý đặc trưng, hướng đến khách hàng yêu thích thời trang.",
                SoLuongTon = 2
            },
            new MoTo
            {
                TenHang = "Piaggio",
                TenDongXe = "Liberty 125",
                LoaiXe = "Xe tay ga",
                MauSac = "Xanh navy",
                NamSX = 2024,
                GiaXe = 57500000,
                HinhAnhFullPath = "https://upload.wikimedia.org/wikipedia/commons/c/c5/Piaggio_Liberty_125_%28Salerno%29.jpg",
                MoTa = "Mẫu xe ga bánh lớn linh hoạt, phù hợp di chuyển trong thành phố.",
                SoLuongTon = 4
            }
        };

        public static ObservableCollection<DichVuPhuTung> DanhSachDichVu { get; } = new ObservableCollection<DichVuPhuTung>
        {
            new DichVuPhuTung { MaPT = "DV001", Ten = "Bảo dưỡng tổng quát", Gia = 450000, TonKho = 0 },
            new DichVuPhuTung { MaPT = "DV002", Ten = "Vệ sinh kim phun xăng điện tử", Gia = 280000, TonKho = 0 },
            new DichVuPhuTung { MaPT = "DV003", Ten = "Cân chỉnh phanh trước/sau", Gia = 150000, TonKho = 0 },
            new DichVuPhuTung { MaPT = "PT001", Ten = "Nhớt động cơ 10W-40", Gia = 145000, TonKho = 40 },
            new DichVuPhuTung { MaPT = "PT002", Ten = "Lọc gió động cơ", Gia = 95000, TonKho = 22 },
            new DichVuPhuTung { MaPT = "PT003", Ten = "Bugi Iridium NGK", Gia = 210000, TonKho = 18 },
            new DichVuPhuTung { MaPT = "PT004", Ten = "Má phanh trước", Gia = 320000, TonKho = 16 },
            new DichVuPhuTung { MaPT = "PT005", Ten = "Lốp không săm Michelin City Grip", Gia = 760000, TonKho = 10 }
        };

        public static ObservableCollection<KhachHang> DanhSachKhachHang { get; } = new ObservableCollection<KhachHang>
        {
            new KhachHang { MaKH = "KH001", HoTen = "Nguyễn Gia Bảo", SDT = "0908123456", CCCD = "079204001234", Email = "nguyengiabao@gmail.com", DiaChi = "Phường Bến Nghé, Quận 1, TP.HCM" },
            new KhachHang { MaKH = "KH002", HoTen = "Trần Thảo Vy", SDT = "0912233445", CCCD = "079204005678", Email = "thaovy.tran92@gmail.com", DiaChi = "Phường 12, Quận 10, TP.HCM" },
            new KhachHang { MaKH = "KH003", HoTen = "Lê Hoàng Nam", SDT = "0988112233", CCCD = "079204009999", Email = "lehoangnam88@gmail.com", DiaChi = "Phường Linh Trung, TP Thủ Đức" },
            new KhachHang { MaKH = "KH004", HoTen = "Phạm Ngọc Hân", SDT = "0973456789", CCCD = "079204112233", Email = "phamngochan@gmail.com", DiaChi = "Phường Hiệp Bình Chánh, TP Thủ Đức" },
            new KhachHang { MaKH = "KH005", HoTen = "Đặng Quốc Minh", SDT = "0935678123", CCCD = "079204445566", Email = "dangquocminh.work@gmail.com", DiaChi = "Phường 4, Quận Gò Vấp, TP.HCM" },
            new KhachHang { MaKH = "KH006", HoTen = "Võ Minh Triết", SDT = "0946789123", CCCD = "079204778899", Email = "vominhtriet23@gmail.com", DiaChi = "Phường An Khánh, TP Thủ Đức" }
        };

        public static ObservableCollection<NhanVien> DanhSachNhanVien { get; } = new ObservableCollection<NhanVien>
        {
            new NhanVien { MaNV = "NV001", HoTen = "Trần Minh Quân", SDT = "0933111222", ChucVu = "Quản lý cửa hàng", NgayVaoLam = new DateTime(2021, 3, 15), TrangThai = "Đang làm việc" },
            new NhanVien { MaNV = "NV002", HoTen = "Nguyễn Bảo Trâm", SDT = "0944222333", ChucVu = "Tư vấn bán hàng", NgayVaoLam = new DateTime(2022, 7, 10), TrangThai = "Đang làm việc" },
            new NhanVien { MaNV = "NV003", HoTen = "Võ Quốc Huy", SDT = "0977333444", ChucVu = "Kỹ thuật viên", NgayVaoLam = new DateTime(2020, 9, 20), TrangThai = "Tạm nghỉ" },
            new NhanVien { MaNV = "NV004", HoTen = "Phan Khánh Duy", SDT = "0965111222", ChucVu = "Kế toán", NgayVaoLam = new DateTime(2023, 2, 1), TrangThai = "Đang làm việc" },
            new NhanVien { MaNV = "NV005", HoTen = "Bùi Thu Hà", SDT = "0922888999", ChucVu = "Chăm sóc khách hàng", NgayVaoLam = new DateTime(2024, 1, 5), TrangThai = "Đang làm việc" }
        };

        public static ObservableCollection<HoaDon> DanhSachHoaDon { get; } = new ObservableCollection<HoaDon>
        {
            new HoaDon { MaHD = "HD001", NgayLap = new DateTime(2026, 1, 12), TenNhanVien = "Nguyễn Bảo Trâm", TenKhachHang = "Nguyễn Gia Bảo", SDT = "0908123456", TenDV_SP = "Bảo dưỡng tổng quát", SoLuong = 1, ThanhTien = 450000, PhuongThucThanhToan = "Tiền mặt" },
            new HoaDon { MaHD = "HD002", NgayLap = new DateTime(2026, 1, 18), TenNhanVien = "Phan Khánh Duy", TenKhachHang = "Trần Thảo Vy", SDT = "0912233445", TenDV_SP = "Nhớt động cơ 10W-40", SoLuong = 2, ThanhTien = 290000, PhuongThucThanhToan = "Chuyển khoản" },
            new HoaDon { MaHD = "HD003", NgayLap = new DateTime(2026, 2, 2), TenNhanVien = "Nguyễn Bảo Trâm", TenKhachHang = "Lê Hoàng Nam", SDT = "0988112233", TenDV_SP = "Vệ sinh kim phun xăng điện tử", SoLuong = 1, ThanhTien = 280000, PhuongThucThanhToan = "Tiền mặt" },
            new HoaDon { MaHD = "HD004", NgayLap = new DateTime(2026, 2, 25), TenNhanVien = "Trần Minh Quân", TenKhachHang = "Phạm Ngọc Hân", SDT = "0973456789", TenDV_SP = "Vespa Sprint 150", SoLuong = 1, ThanhTien = 85000000, PhuongThucThanhToan = "Trả góp" },
            new HoaDon { MaHD = "HD005", NgayLap = new DateTime(2026, 3, 5), TenNhanVien = "Bùi Thu Hà", TenKhachHang = "Đặng Quốc Minh", SDT = "0935678123", TenDV_SP = "Bugi Iridium NGK", SoLuong = 1, ThanhTien = 210000, PhuongThucThanhToan = "Tiền mặt" },
            new HoaDon { MaHD = "HD006", NgayLap = new DateTime(2026, 3, 21), TenNhanVien = "Trần Minh Quân", TenKhachHang = "Võ Minh Triết", SDT = "0946789123", TenDV_SP = "Exciter 155 VVA", SoLuong = 1, ThanhTien = 54800000, PhuongThucThanhToan = "Chuyển khoản" },
            new HoaDon { MaHD = "HD007", NgayLap = new DateTime(2026, 4, 1), TenNhanVien = "Nguyễn Bảo Trâm", TenKhachHang = "Nguyễn Gia Bảo", SDT = "0908123456", TenDV_SP = "Cân chỉnh phanh trước/sau", SoLuong = 1, ThanhTien = 150000, PhuongThucThanhToan = "Tiền mặt" }
        };
    }
}