
-- =============================================
-- TẠO DATABASE
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'QuanLyBanXeMay')
BEGIN
    CREATE DATABASE QuanLyBanXeMay;
END
GO

USE QuanLyBanXeMay;
GO

-- =============================================
-- XÓA BẢNG CŨ (đúng thứ tự FK)
-- =============================================
IF OBJECT_ID('HoaDon', 'U') IS NOT NULL DROP TABLE HoaDon;
IF OBJECT_ID('TaiKhoan', 'U') IS NOT NULL DROP TABLE TaiKhoan;
IF OBJECT_ID('Xe', 'U') IS NOT NULL DROP TABLE Xe;
IF OBJECT_ID('DichVuPhuTung', 'U') IS NOT NULL DROP TABLE DichVuPhuTung;
IF OBJECT_ID('NhanVien', 'U') IS NOT NULL DROP TABLE NhanVien;
IF OBJECT_ID('KhachHang', 'U') IS NOT NULL DROP TABLE KhachHang;
IF OBJECT_ID('HangXe', 'U') IS NOT NULL DROP TABLE HangXe;
GO

-- =============================================
-- TẠO BẢNG HÃNG XE
-- =============================================
CREATE TABLE HangXe (
    MaHang VARCHAR(20) PRIMARY KEY,
    TenHang NVARCHAR(100) NOT NULL,
    QuocGia NVARCHAR(50),
    LogoPath NVARCHAR(500)
);
GO

-- =============================================
-- TẠO BẢNG XE
-- =============================================
CREATE TABLE Xe (
    MaXe VARCHAR(20) PRIMARY KEY,
    TenXe NVARCHAR(100) NOT NULL,
    LoaiXe NVARCHAR(50),
    NamSX INT,
    GiaBan DECIMAL(18,2) CHECK (GiaBan > 0),
    MauSac NVARCHAR(30),
    MoTa NVARCHAR(MAX),
    HinhAnh NVARCHAR(500),
    MaHang VARCHAR(20),
    SoLuongTon INT CHECK (SoLuongTon >= 0),

    CONSTRAINT FK_Xe_HangXe
    FOREIGN KEY (MaHang) REFERENCES HangXe(MaHang)
);

-- =============================================
-- TẠO BẢNG KHÁCH HÀNG
-- =============================================
CREATE TABLE KhachHang (
    MaKH VARCHAR(20) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    SDT VARCHAR(15),
    CCCD VARCHAR(20),
    Email VARCHAR(100),
    DiaChi NVARCHAR(255)
);
GO

-- =============================================
-- TẠO BẢNG NHÂN VIÊN
-- =============================================
CREATE TABLE NhanVien (
    MaNV VARCHAR(20) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    ChucVu NVARCHAR(50),
    NgayVaoLam DATE DEFAULT GETDATE(),
    SDT VARCHAR(15),
    TrangThai NVARCHAR(50) DEFAULT N'Đang làm việc'
);
GO

-- =============================================
-- TẠO BẢNG TÀI KHOẢN
-- =============================================
CREATE TABLE TaiKhoan (
    Username VARCHAR(50) PRIMARY KEY,
    Password VARCHAR(255) NOT NULL,
    Role NVARCHAR(50),
    MaNV VARCHAR(20),

    CONSTRAINT FK_TaiKhoan_NhanVien
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);
GO

-- =============================================
-- TẠO BẢNG DỊCH VỤ PHỤ TÙNG
-- =============================================
CREATE TABLE DichVuPhuTung (
    MaPT VARCHAR(20) PRIMARY KEY,
    Ten NVARCHAR(100) NOT NULL,
    Gia DECIMAL(18,2) CHECK (Gia >= 0),
    TonKho INT CHECK (TonKho >= 0)
);
GO

-- =============================================
-- TẠO BẢNG HÓA ĐƠN
-- =============================================
CREATE TABLE HoaDon (
    MaHD VARCHAR(20) PRIMARY KEY,
    NgayLap DATETIME DEFAULT GETDATE(),
    MaNV VARCHAR(20),
    MaKH VARCHAR(20),
    TenDV_SP NVARCHAR(255),
    SoLuong INT CHECK (SoLuong > 0),
    ThanhTien DECIMAL(18,2) CHECK (ThanhTien > 0),
    PhuongThucThanhToan NVARCHAR(50),

    CONSTRAINT FK_HoaDon_NhanVien
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),

    CONSTRAINT FK_HoaDon_KhachHang
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)
);
GO

-- =============================================
-- DỮ LIỆU HÃNG XE
-- =============================================
INSERT INTO HangXe VALUES
('HX01', N'Honda',        N'Nhật Bản', 'https://logos-world.net/wp-content/uploads/2021/03/Honda-Logo.png'),
('HX02', N'Yamaha',       N'Nhật Bản', 'https://logos-world.net/wp-content/uploads/2020/10/Yamaha-Logo-700x394.png'),
('HX03', N'Suzuki',       N'Nhật Bản', 'https://logos-world.net/wp-content/uploads/2021/10/Suzuki-Logo-700x394.png'),
('HX04', N'Piaggio',      N'Ý',        'https://logos-world.net/wp-content/uploads/2022/12/Piaggio-Logo-500x281.png'),
('HX05', N'SYM',          N'Đài Loan', 'https://logos-world.net/wp-content/uploads/2021/04/SYM-Motors-Logo-500x281.png'),
('HX06', N'Ducati',       N'Ý',        'https://logos-world.net/wp-content/uploads/2021/03/Ducati-Logo-700x394.png'),
('HX07', N'Kawasaki',     N'Nhật Bản', 'https://logos-world.net/wp-content/uploads/2020/11/Kawasaki-Logo-700x394.png'),
('HX08', N'BMW Motorrad', N'Đức',      'https://logos-world.net/wp-content/uploads/2024/10/BMW-Motorrad-Logo-500x281.png'),
('HX09', N'Triumph',      N'Anh',      'https://logos-world.net/wp-content/uploads/2020/11/Triumph-Logo-700x394.png'),
('HX10', N'KTM',          N'Áo',       'https://logos-world.net/wp-content/uploads/2020/11/KTM-Logo-700x394.png');
GO

-- =============================================
-- DỮ LIỆU XE
-- =============================================
INSERT INTO Xe VALUES
('XE01', N'Honda Winner X 2024',      N'Xe côn tay', 2024, 46900000,   N'Đỏ Đen',      N'Động cơ SOHC 150cc, phuộc USD, đèn LED full. Phù hợp đường đô thị lẫn địa hình.', 'https://hondathanhbinhan.com/wp-content/uploads/2024/01/winner-x-2024.png', 'HX01', 15),
('XE02', N'Honda Vision 2024',        N'Xe tay ga',  2024, 34990000,   N'Trắng Ngọc',  N'Tay ga phổ thông bán chạy nhất Việt Nam. Cốp 21L, tiết kiệm nhiên liệu 1.8L/100km.', 'https://lajumotor.com/wp-content/uploads/2023/10/honda-vision-110-2024-beige.jpg', 'HX01', 30),
('XE03', N'Honda SH 160i 2024',       N'Xe tay ga',  2024, 88900000,   N'Xám Xi Măng', N'Tay ga cao cấp nhất phân khúc. Động cơ eSP+ 160cc, smart key, ABS.', 'https://files01.danhgiaxe.com/rotrH2b4Q4UpTvL-Ey7sM2zEBUw=/fit-in/2560x0/20240218/honda-sh-160i-2024--4-013934.jpg', 'HX01', 10),
('XE04', N'Yamaha Exciter 155 2024',  N'Xe côn tay', 2024, 52900000,   N'Xanh GP',     N'Underbone thể thao đỉnh cao. Động cơ VVA 155cc, khung Delta Box, phuộc KYB.', 'https://moto.yugatech.com/wp-content/uploads/2023/09/Yamaha-Exciter-155-VVA-ABS-2024-10.png', 'HX02', 18),
('XE05', N'Yamaha Grande Hybrid 2023',N'Xe tay ga',  2023, 54900000,   N'Đen Nhám',    N'Tay ga cốp rộng với hệ thống hybrid thông minh. Tiết kiệm xăng tới 45km/L.', 'https://lajumotor.com/wp-content/uploads/2022/09/yamaha-grande-2023-hybrid.jpg', 'HX02', 12),
('XE06', N'Suzuki Raider R150 2023',  N'Xe côn tay', 2023, 50900000,   N'Xanh Đen',    N'Hyper Underbone với khung kim cương, động cơ 150cc phun xăng điện tử FI.', 'https://www.dsf.my/wp-content/uploads/2022/03/Suzuki-Raider-R150-Fi-Belang-Launch.jpeg?v=1646963855', 'HX03', 9),
('XE07', N'Vespa Sprint 125 2024',    N'Xe tay ga',  2024, 82000000,   N'Vàng Cát',    N'Biểu tượng xe tay ga Ý với thiết kế retro hiện đại. Động cơ 125cc iGet, ABS.', 'https://images5.1000ps.net/images_bikekat/2024/39-Vespa/10947-Sprint_125_S/003-638538602238427418-vespa-sprint-125-s.jpg', 'HX04', 7),
('XE08', N'Vespa GTS Super 300 2024', N'Xe tay ga',  2024, 165000000,  N'Xanh Pastel', N'Tay ga hạng sang. Động cơ HPE 300cc, nồng nhiệt hội tụ phong cách Ý cổ điển.', 'https://images5.1000ps.net/images_bikekat/2025/39-Vespa/10955-GTS_125_Super_Sport/005-638681198583248918-vespa-gts-125-super-sport.jpg', 'HX04', 5),
('XE09', N'Kawasaki Ninja 400 2023',  N'Sportbike',  2023, 168000000,  N'Xanh KRT',    N'Sportbike 400cc lý tưởng cho người mới bắt đầu lên phân khúc lớn. Khung trellis nhôm.', 'https://storage.kawasaki.eu/public/kawasaki.eu/en-EU/model/N400_P_GN1.jpg', 'HX07', 4),
('XE10', N'Ducati Panigale V4 2024',  N'Superbike',  2024, 1260000000, N'Đỏ Ducati',   N'Siêu mô tô đường đua thuần chủng. Động cơ Desmosedici Stradale V4 1103cc, 215 mã lực.', 'https://dhqlmcogwd1an.cloudfront.net/images/phocagallery/ducati/panigale-v4-2023/01-ducati-panigale-v4-2023-estudio-rojo-01.jpg', 'HX06', 2);
GO

-- =============================================
-- DỮ LIỆU KHÁCH HÀNG
INSERT INTO KhachHang VALUES
('KH001', N'Nguyễn Minh Tuấn',    '0931845672', '079091234571', 'nguyenminhtuan@gmail.com',    N'123 Nguyễn Huệ, Phường Bến Nghé, Quận 1, TP.HCM'),
('KH002', N'Trần Thị Thanh Hà',   '0768234591', '079185234582', 'tranthithanhhakh@gmail.com',  N'45 Lê Lợi, Phường Bến Thành, Quận 3, TP.HCM'),
('KH003', N'Lê Quốc Cường',       '0853712946', '079094537893', 'lequoccuongkh@gmail.com',     N'78 Xô Viết Nghệ Tĩnh, Phường 24, Bình Thạnh, TP.HCM'),
('KH004', N'Phạm Thị Lan Anh',    '0912638457', '079185812604', 'phamthilananh@gmail.com',     N'12 Phan Văn Trị, Phường 7, Gò Vấp, TP.HCM'),
('KH005', N'Hoàng Trọng Nghĩa',   '0375824193', '079095634215', 'hoangtrongnghia@gmail.com',   N'56 Võ Văn Ngân, Phường Linh Chiểu, TP.Thủ Đức, TP.HCM'),
('KH006', N'Vũ Thị Mỹ Hạnh',      '0896314725', '079185417326', 'vuthimyhanh@gmail.com',       N'34 Bình Giã, Phường 13, Quận 10, TP.HCM'),
('KH007', N'Đặng Văn Hùng',       '0582947361', '079093248537', 'dangvanhung@gmail.com',        N'89 Trường Chinh, Phường 14, Tân Bình, TP.HCM'),
('KH008', N'Bùi Ngọc Hương',      '0703581924', '079180539148', 'buingochuong@gmail.com',       N'21 Nguyễn Thị Thập, Phường Tân Phú, Quận 7, TP.HCM'),
('KH009', N'Đỗ Thanh Khoa',       '0846275319', '079095624759', 'dothankhoa@gmail.com',         N'67 Nguyễn Tất Thành, Phường 13, Quận 4, TP.HCM'),
('KH010', N'Ngô Thị Phương Linh', '0563748291', '079181234860
-- =============================================', 'ngothiphuonglinh@gmail.com',   N'15 Kinh Dương Vương, Phường An Lạc, Bình Tân, TP.HCM');
GO

-- =============================================
-- DỮ LIỆU NHÂN VIÊN
-- =============================================
INSERT INTO NhanVien VALUES
('NV001', N'Huỳnh Thanh Phong',     N'Quản lý',              '2019-03-01', '0938145762', N'Đang làm việc'),
('NV002', N'Lê Minh Khang',         N'Nhân viên bán hàng',   '2021-06-15', '0764823915', N'Đang làm việc'),
('NV003', N'Trần Thị Ngọc Lan',     N'Nhân viên bán hàng',   '2022-08-10', '0852917364', N'Đang làm việc'),
('NV004', N'Nguyễn Hữu Tài',        N'Kỹ thuật viên',        '2020-11-20', '0915284736', N'Đang làm việc'),
('NV005', N'Phan Đình Phúc',        N'Kỹ thuật viên',        '2021-04-05', '0376924158', N'Đang làm việc'),
('NV006', N'Cao Thị Mỹ Linh',       N'Kế toán',              '2020-01-10', '0895471362', N'Đang làm việc'),
('NV007', N'Võ Trung Kiên',         N'Bảo vệ',               '2022-02-28', '0584619273', N'Đang làm việc'),
('NV008', N'Đinh Tuấn Anh',         N'Kỹ thuật viên',        '2023-07-01', '0702381945', N'Đang làm việc'),
('NV009', N'Lương Thị Bích Huyền',  N'Chăm sóc khách hàng', '2023-11-15', '0849627153', N'Đang làm việc'),
('NV010', N'Phan Văn Triều',        N'Nhân viên bán hàng',   '2020-09-12', '0561384972', N'Tạm nghỉ');
GO

-- =============================================
-- DỮ LIỆU TÀI KHOẢN
-- =============================================
INSERT INTO TaiKhoan VALUES
('admin',      '123456', N'Quản lý',              'NV001'),
('nv_khang',   '123456', N'Bán hàng',             'NV002'),
('nv_lan',     '123456', N'Bán hàng',             'NV003'),
('kt_tai',     '123456', N'Kỹ thuật',             'NV004'),
('kt_phuc',    '123456', N'Kỹ thuật',             'NV005'),
('kt_linh',    '123456', N'Kế toán',              'NV006'),
('bv_kien',    '123456', N'Bảo vệ',               'NV007'),
('kt_anh',     '123456', N'Kỹ thuật',             'NV008'),
('cskh_huyen', '123456', N'Chăm sóc khách hàng', 'NV009'),
('nv_trieu',   '123456', N'Bán hàng',             'NV010');
GO

-- =============================================
-- DỮ LIỆU DỊCH VỤ PHỤ TÙNG
-- =============================================
INSERT INTO DichVuPhuTung VALUES
('DV001', N'Bảo dưỡng định kỳ toàn bộ',     199000,    0),
('DV002', N'Rửa xe nội thất + ngoại thất',    50000,    0),
('DV003', N'Thay nhớt máy',                   80000,    0),
('DV004', N'Sửa chữa động cơ nặng',          800000,    0),
('DV005', N'Vá và bơm lốp',                   30000,    0),
('DV006', N'Kiểm tra hệ thống điện',         120000,    0),
('PT001', N'Nhớt Castrol Power1 Racing 4T',   145000,   80),
('PT002', N'Nhớt Motul 7100 300V 4T 10W-40',  420000,   45),
('PT003', N'Bugi NGK CR8EH-9',                 55000,  150),
('PT004', N'Lốp Michelin Pilot Street 2',    1350000,   25),
('PT005', N'Nhông sên đĩa DID 428 Honda',    950000,   18),
('PT006', N'Bố thắng đĩa Brembo 4 pit',      380000,   30),
('PT007', N'Lọc gió HRC Racing',               95000,   60),
('PT008', N'Piston + xéc măng STD 57mm',     680000,   12);
GO

-- =============================================
-- DỮ LIỆU HÓA ĐƠN
-- =============================================
INSERT INTO HoaDon VALUES
('HD001', '2025-01-08 09:15:00', 'NV002', 'KH001', N'Honda Winner X 2024',           1, 46900000,  N'Trả góp'),
('HD002', '2025-01-15 10:30:00', 'NV003', 'KH002', N'Honda Vision 2024',              1, 34990000,  N'Tiền mặt'),
('HD003', '2025-02-03 08:45:00', 'NV004', 'KH003', N'Bảo dưỡng định kỳ toàn bộ',     1,   199000,  N'Tiền mặt'),
('HD004', '2025-02-18 14:00:00', 'NV005', 'KH004', N'Nhớt Motul 7100 300V 4T 10W-40',2,   840000,  N'Chuyển khoản'),
('HD005', '2025-03-05 11:00:00', 'NV002', 'KH005', N'Yamaha Exciter 155 2024',        1, 52900000,  N'Trả góp'),
('HD006', '2025-03-20 13:30:00', 'NV003', 'KH006', N'Suzuki Raider R150 2023',        1, 50900000,  N'Chuyển khoản'),
('HD007', '2025-04-02 09:00:00', 'NV004', 'KH007', N'Rửa xe nội thất + ngoại thất',  1,    50000,  N'Tiền mặt'),
('HD008', '2025-04-10 15:00:00', 'NV002', 'KH008', N'Vespa Sprint 125 2024',          1, 82000000,  N'Trả góp'),
('HD009', '2025-04-25 10:00:00', 'NV003', 'KH009', N'Kawasaki Ninja 400 2023',        1,168000000,  N'Trả góp'),
('HD010', '2025-05-07 08:30:00', 'NV002', 'KH010', N'Nhông sên đĩa DID 428 Honda',   1,   950000,  N'Tiền mặt');
GO

-- =============================================
-- XEM DỮ LIỆU
-- =============================================
SELECT * FROM HangXe;
GO
SELECT * FROM Xe;
GO
SELECT * FROM KhachHang;
GO
SELECT * FROM NhanVien;
GO
SELECT * FROM TaiKhoan;
GO
SELECT * FROM DichVuPhuTung;
GO
SELECT * FROM HoaDon;
GO
