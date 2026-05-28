
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
    DiaChi NVARCHAR(255),
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    AnhCaNhan NVARCHAR(500)
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
    MaHD VARCHAR(20),
    NgayLap DATETIME DEFAULT GETDATE(),
    MaNV VARCHAR(20),
    MaKH VARCHAR(20),
    TenDV_SP NVARCHAR(255),
    SoLuong INT CHECK (SoLuong > 0),
    ThanhTien DECIMAL(18,2) CHECK (ThanhTien > 0),
    PhuongThucThanhToan NVARCHAR(50),
    TrangThai NVARCHAR(50) DEFAULT N'Đã xác nhận',

    CONSTRAINT PK_HoaDon PRIMARY KEY (MaHD, MaNV, MaKH, TenDV_SP),

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
('HX01', N'Honda',        N'Nhật Bản', 'https://www.carlogos.org/car-logos/honda-logo.png'),
('HX02', N'Yamaha',       N'Nhật Bản', 'https://img.favpng.com/11/16/24/yamaha-motor-company-yamaha-corporation-logo-motorcycle-all-terrain-vehicle-png-favpng-aVLLZpk8UMwSEiPK7PVqte4zy.jpg'),
('HX03', N'Suzuki',       N'Nhật Bản', 'https://www.carlogos.org/car-logos/suzuki-logo.png'),
('HX04', N'Piaggio',      N'Ý',        'https://cdn.freebiesupply.com/logos/large/2x/piaggio-logo-png-transparent.png'),
('HX05', N'SYM',          N'Đài Loan', 'https://cdn.freebiesupply.com/logos/large/2x/sym-logo-png-transparent.png'),
('HX06', N'Ducati',       N'Ý',        'https://cdn.freebiesupply.com/logos/large/2x/ducati-logo-png-transparent.png'),
('HX07', N'Kawasaki',     N'Nhật Bản', 'https://cdn.freebiesupply.com/logos/large/2x/kawasaki-logo-png-transparent.png'),
('HX08', N'BMW Motorrad', N'Đức',      'https://www.carlogos.org/car-logos/bmw-logo.png'),
('HX09', N'Triumph',      N'Anh',      'https://www.carlogos.org/car-logos/triumph-logo.png'),
('HX10', N'KTM',          N'Áo',       'https://www.carlogos.org/car-logos/ktm-logo.png');
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
('XE08', N'Vespa GTS Super 300 2024', N'Xe tay ga',  2024, 165000000,  N'Xanh Pastel', N'Tay ga hạng sang. Động cơ HPE 300cc, nồng nhiệt hội tụ phong cách Ý cổ điển.', 'https://files01.danhgiaxe.com/Jca8VeJUyGvL1wHcM2QrebK0xr4=/fit-in/1280x0/20230324/vespa-gts-super-sport-300-mau-xanh-112451.jpg', 'HX04', 5),
('XE09', N'Kawasaki Ninja 400 2023',  N'Sportbike',  2023, 168000000,  N'Xanh KRT',    N'Sportbike 400cc lý tưởng cho người mới bắt đầu lên phân khúc lớn. Khung trellis nhôm.', 'https://storage.kawasaki.eu/public/kawasaki.eu/en-EU/model/N400_P_GN1.jpg', 'HX07', 4),
('XE10', N'Ducati Panigale V4 2024',  N'Superbike',  2024, 1260000000, N'Đỏ Ducati',   N'Siêu mô tô đường đua thuần chủng. Động cơ Desmosedici Stradale V4 1103cc, 215 mã lực.', 'https://dhqlmcogwd1an.cloudfront.net/images/phocagallery/ducati/panigale-v4-2023/01-ducati-panigale-v4-2023-estudio-rojo-01.jpg', 'HX06', 2),
('XE11', N'SYM Star SR 170 2023',N'Naked bike', 2023, 84900000, N'Đen Nhám',N'Naked bike 170cc phong cách thể thao. Khung thép cứng cáp, phuộc đơn ngược, đèn LED, phù hợp đô thị.','https://aima.com.vn/wp-content/uploads/2023/07/sym-star-sr-170-1.jpg','HX05', 10),
('XE12', N'SYM Attila VI 125 2024',N'Xe tay ga', 2024, 33500000, N'Trắng Bạc',N'Tay ga nữ kiểu dáng Á Đông thanh lịch. Động cơ 125cc, cốp 18L, trọng lượng nhẹ 95kg, dễ điều khiển.','https://www.sym.com.vn/uploads/san-pham/scooter-125cc/new-attila-125/white/2.jpg','HX05', 20),
('XE13', N'BMW G 310 R 2024',N'Naked bike', 2024, 155000000, N'Xanh Sapphire',N'Naked bike nhập khẩu châu Âu. Động cơ đơn xy-lanh 313cc, khung thép phân đoạn, ABS 2 kênh, TFT 5 inch.','https://www.motorrad-bilder.at/slideshows/291/022269/BMW_Neuheiten_2024_054.jpg','HX08', 6),
('XE14', N'BMW R 1250 GS Adventure 2024',N'Adventure', 2024, 690000000, N'Đen Xám Rally',N'Vua off-road touring thế giới. Động cơ Boxer 1254cc 136 mã lực, hệ thống treo bán tích cực, 6 chế độ lái.','https://images5.1000ps.net/images_bikekat/2023/7-BMW/9550-R_1250_GS_Adventure/011-637925204232658664-bmw-r-1250-gs-adventure.jpg','HX08', 3),
('XE15', N'Triumph Street Triple 765 RS 2024',N'Naked bike', 2024, 365000000, N'Bạc Matt',N'Naked bike hạng trung đỉnh cao nước Anh. Động cơ 3 xy-lanh 765cc 130 mã lực, Öhlins NIX30, Brembo Stylema.','https://www.virgintriumph.com/vt_data/img/models/streettriple765r/2023/01.jpg','HX09', 4),
('XE16', N'Triumph Bonneville T100 2024',N'Classic', 2024, 318000000, N'Đỏ Carnival',N'Biểu tượng xe cổ điển Anh quốc. Động cơ song parallel 900cc, cảm giác lái retro chính hãng, trang bị hiện đại.','https://media.triumphmotorcycles.co.uk/image/upload/f_auto/q_auto/sitecoremedialibrary/media-library/images/motorcycles/modern-classics/my24%20colours/dd4_speed_twin_1200/speed_twin_1200_my24_carnival_red_rhs_1080px.png','HX09', 5),
('XE17', N'KTM 390 Duke 2024',N'Naked bike', 2024, 148000000, N'Cam KTM',N'Naked bike thể thao bậc nhất phân khúc tầm trung. Động cơ 399cc LC4c, khung thép Chromoly, TFT Bluetooth.','https://www.bostancioglu.com.tr/sites/default/files/styles/dikdortgen/public/sasi_1.png','HX10', 12),
('XE18', N'KTM RC 390 2024',N'Sportbike', 2024, 158000000, N'Cam Trắng',N'Full-fairing sportbike hiệu suất cao. Động cơ 399cc 46 mã lực, khí động học đường đua, phuộc WP APEX.','https://ultimatemotorcycling.com/wp-content/uploads/2016/06/2016-ktm-rc-390-buyers-guide-1-770x454.jpg','HX10', 8);
GO

-- =============================================
-- DỮ LIỆU KHÁCH HÀNG
INSERT INTO dbo.KhachHang (MaKH, HoTen, SDT, CCCD, Email, DiaChi, NgaySinh, GioiTinh) VALUES
('KH001', N'Nguyễn Minh Tuấn',    '0931845672', '079091234571', 'nguyenminhtuan@gmail.com',    N'123 Nguyễn Huệ, Phường Bến Nghé, Quận 1, TP.HCM', '1985-05-12', N'Nam'),
('KH002', N'Trần Thị Thanh Hà',   '0768234591', '079185234582', 'tranthithanhhakh@gmail.com',  N'45 Lê Lợi, Phường Bến Thành, Quận 3, TP.HCM', '1990-08-25', N'Nữ'),
('KH003', N'Lê Quốc Cường',       '0853712946', '079094537893', 'lequoccuongkh@gmail.com',     N'78 Xô Viết Nghệ Tĩnh, Phường 24, Bình Thạnh, TP.HCM', '1988-11-03', N'Nam'),
('KH004', N'Phạm Thị Lan Anh',    '0912638457', '079185812604', 'phamthilananh@gmail.com',     N'12 Phan Văn Trị, Phường 7, Gò Vấp, TP.HCM', '1995-02-14', N'Nữ'),
('KH005', N'Hoàng Trọng Nghĩa',   '0375824193', '079095634215', 'hoangtrongnghia@gmail.com',   N'56 Võ Văn Ngân, Phường Linh Chiểu, TP.Thủ Đức, TP.HCM', '1992-07-30', N'Nam'),
('KH006', N'Vũ Thị Mỹ Hạnh',      '0896314725', '079185417326', 'vuthimyhanh@gmail.com',       N'34 Bình Giã, Phường 13, Quận 10, TP.HCM', '1987-09-15', N'Nữ'),
('KH007', N'Đặng Văn Hùng',       '0582947361', '079093248537', 'dangvanhung@gmail.com',        N'89 Trường Chinh, Phường 14, Tân Bình, TP.HCM', '1983-04-20', N'Nam'),
('KH008', N'Bùi Ngọc Hương',      '0703581924', '079180539148', 'buingochuong@gmail.com',       N'21 Nguyễn Thị Thập, Phường Tân Phú, Quận 7, TP.HCM', '1991-12-05', N'Nữ'),
('KH009', N'Đỗ Thanh Khoa',       '0846275319', '079095624759', 'dothankhoa@gmail.com',         N'67 Nguyễn Tất Thành, Phường 13, Quận 4, TP.HCM', '1994-06-18', N'Nam'),
('KH010', N'Ngô Thị Phương Linh', '0563748291', '079181234860', 'ngothiphuonglinh@gmail.com',   N'15 Kinh Dương Vương, Phường An Lạc, Bình Tân, TP.HCM', '1998-01-27', N'Nữ');
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
('DV001', N'Bảo dưỡng định kỳ toàn bộ',     199000,   27),
('DV002', N'Rửa xe nội thất + ngoại thất',    50000,   64),
('DV003', N'Thay nhớt máy',                   80000,   18),
('DV004', N'Sửa chữa động cơ nặng',          800000,   91),
('DV005', N'Vá và bơm lốp',                   30000,   42),
('DV006', N'Kiểm tra hệ thống điện',         120000,   73),
('DV007', N'Thay nước làm mát',                 150000,   36),
('DV008', N'Vệ sinh buồng đốt, kim phun Fi',    200000,   58),
('DV009', N'Phục hồi phuộc nhún trước/sau',     250000,   14),
('DV010', N'Căn chỉnh khe hở xú páp (Đồng tiền)',180000,   87),
('DV011', N'Dán keo trong chống xước toàn xe',  650000,   25),
('DV012', N'Ép chảng ba, cân vành',             300000,   69),
('PT001', N'Nhớt Castrol Power1 Racing 4T',   145000,   80),
('PT002', N'Nhớt Motul 7100 300V 4T 10W-40',  420000,   45),
('PT003', N'Bugi NGK CR8EH-9',                 55000,  150),
('PT004', N'Lốp Michelin Pilot Street 2',    1350000,   25),
('PT005', N'Nhông sên đĩa DID 428 Honda',    950000,   18),
('PT006', N'Bố thắng đĩa Brembo 4 pit',      380000,   30),
('PT007', N'Lọc gió HRC Racing',               95000,   60),
('PT008', N'Piston + xéc măng STD 57mm',     680000,   12),
('PT009', N'Nước làm mát Liqui Moly Đỏ',        185000,   50),
('PT010', N'Bình ắc quy GS MF GTZ6V',           450000,   40),
('PT011', N'Lốp Dunlop ScootSmart',            1150000,   20),
('PT012', N'Dầu láp (nhớt hộp số) Liqui Moly',  120000,   60),
('PT013', N'Dây curoa Bando chính hãng',        550000,   25),
('PT014', N'Bóng đèn pha LED Philips',          350000,   35),
('PT015', N'Bi nồi bọc carbon Bando',           180000,   45),
('PT016', N'Phuộc sau Ohlins (Mẫu phổ thông)', 8500000,    5),
('PT017', N'Che két nước CNC Nhôm nguyên khối', 250000,   22),
('PT018', N'Kính chiếu hậu H2C',                480000,   30);
GO

-- =============================================
-- DỮ LIỆU HÓA ĐƠN
INSERT INTO HoaDon (MaHD, NgayLap, MaNV, MaKH, TenDV_SP, SoLuong, ThanhTien, PhuongThucThanhToan) VALUES
('HD001', '2025-01-05 09:15:00', 'NV002', 'KH001', N'Honda Winner X 2024',              1, 46900000, N'Trả góp'),
('HD002', '2025-01-10 10:30:00', 'NV004', 'KH002', N'Bảo dưỡng định kỳ toàn bộ',        1,   199000, N'Tiền mặt'),
('HD002', '2025-01-10 10:30:00', 'NV004', 'KH002', N'Nhớt Castrol Power1 Racing 4T',    1,   145000, N'Tiền mặt'),
('HD002', '2025-01-10 10:30:00', 'NV004', 'KH002', N'Rửa xe nội thất + ngoại thất',     1,    50000, N'Tiền mặt'),
('HD003', '2025-01-15 14:00:00', 'NV003', 'KH003', N'Honda Vision 2024',                1, 34990000, N'Tiền mặt'),
('HD004', '2025-01-20 08:45:00', 'NV005', 'KH004', N'Sửa chữa động cơ nặng',            1,   800000, N'Chuyển khoản'),
('HD004', '2025-01-20 08:45:00', 'NV005', 'KH004', N'Piston + xéc măng STD 57mm',       1,   680000, N'Chuyển khoản'),
('HD004', '2025-01-20 08:45:00', 'NV005', 'KH004', N'Nhớt Motul 7100 300V 4T 10W-40',   1,   420000, N'Chuyển khoản'),
('HD005', '2025-02-05 11:00:00', 'NV010', 'KH005', N'Honda SH 160i 2024',               1, 88900000, N'Chuyển khoản'),
('HD006', '2025-02-12 13:30:00', 'NV002', 'KH006', N'Yamaha Exciter 155 2024',          1, 52900000, N'Trả góp'),
('HD007', '2025-02-18 09:00:00', 'NV008', 'KH007', N'Nhông sên đĩa DID 428 Honda',      1,   950000, N'Tiền mặt'),
('HD007', '2025-02-18 09:00:00', 'NV008', 'KH007', N'Rửa xe nội thất + ngoại thất',     1,    50000, N'Tiền mặt'),
('HD008', '2025-02-25 15:00:00', 'NV004', 'KH008', N'Lốp Michelin Pilot Street 2',      2,  2700000, N'Chuyển khoản'),
('HD008', '2025-02-25 15:00:00', 'NV004', 'KH008', N'Vá và bơm lốp',                    1,    30000, N'Chuyển khoản'),
('HD009', '2025-03-02 10:00:00', 'NV003', 'KH009', N'Vespa Sprint 125 2024',            1, 82000000, N'Trả góp'),
('HD010', '2025-03-08 14:15:00', 'NV005', 'KH010', N'Dây curoa Bando chính hãng',       1,   550000, N'Chuyển khoản'),
('HD010', '2025-03-08 14:15:00', 'NV005', 'KH010', N'Bi nồi bọc carbon Bando',          1,   180000, N'Chuyển khoản'),
('HD010', '2025-03-08 14:15:00', 'NV005', 'KH010', N'Dầu láp (nhớt hộp số) Liqui Moly', 1,   120000, N'Chuyển khoản'),
('HD011', '2025-03-12 09:30:00', 'NV002', 'KH001', N'BMW G 310 R 2024',                 1,155000000, N'Chuyển khoản'),
('HD012', '2025-03-18 10:45:00', 'NV008', 'KH002', N'Phuộc sau Ohlins (Mẫu phổ thông)', 1,  8500000, N'Chuyển khoản'),
('HD012', '2025-03-18 10:45:00', 'NV008', 'KH002', N'Bố thắng đĩa Brembo 4 pit',        2,   760000, N'Chuyển khoản'),
('HD013', '2025-03-25 14:15:00', 'NV003', 'KH003', N'KTM 390 Duke 2024',                1,148000000, N'Trả góp'),
('HD014', '2025-04-02 08:30:00', 'NV004', 'KH004', N'Ép chảng ba, cân vành',            1,   300000, N'Tiền mặt'),
('HD014', '2025-04-02 08:30:00', 'NV004', 'KH004', N'Phục hồi phuộc nhún trước/sau',    1,   250000, N'Tiền mặt'),
('HD015', '2025-04-10 15:00:00', 'NV010', 'KH005', N'Yamaha Grande Hybrid 2023',        1, 54900000, N'Chuyển khoản'),
('HD016', '2025-04-15 09:45:00', 'NV005', 'KH006', N'Vệ sinh buồng đốt, kim phun Fi',   1,   200000, N'Tiền mặt'),
('HD016', '2025-04-15 09:45:00', 'NV005', 'KH006', N'Bugi NGK CR8EH-9',                 1,    55000, N'Tiền mặt'),
('HD017', '2025-04-20 11:20:00', 'NV002', 'KH007', N'Suzuki Raider R150 2023',          1, 50900000, N'Tiền mặt'),
('HD018', '2025-04-28 16:30:00', 'NV008', 'KH008', N'Kiểm tra hệ thống điện',           1,   120000, N'Chuyển khoản'),
('HD018', '2025-04-28 16:30:00', 'NV008', 'KH008', N'Bình ắc quy GS MF GTZ6V',          1,   450000, N'Chuyển khoản'),
('HD018', '2025-04-28 16:30:00', 'NV008', 'KH008', N'Bóng đèn pha LED Philips',         2,   700000, N'Chuyển khoản'),
('HD019', '2025-05-02 10:00:00', 'NV003', 'KH009', N'Kawasaki Ninja 400 2023',          1,168000000, N'Trả góp'),
('HD020', '2025-05-08 14:45:00', 'NV002', 'KH010', N'Ducati Panigale V4 2024',          1,1260000000,N'Chuyển khoản'),
('HD021', '2025-05-15 08:15:00', 'NV004', 'KH001', N'Thay nước làm mát',                1,   150000, N'Tiền mặt'),
('HD021', '2025-05-15 08:15:00', 'NV004', 'KH001', N'Nước làm mát Liqui Moly Đỏ',       1,   185000, N'Tiền mặt'),
('HD022', '2025-05-20 09:30:00', 'NV010', 'KH002', N'SYM Star SR 170 2023',             1, 84900000, N'Trả góp'),
('HD023', '2025-05-25 10:20:00', 'NV005', 'KH003', N'Dán keo trong chống xước toàn xe', 1,   650000, N'Chuyển khoản'),
('HD023', '2025-05-25 10:20:00', 'NV005', 'KH003', N'Rửa xe nội thất + ngoại thất',     1,    50000, N'Chuyển khoản'),
('HD024', '2025-06-02 13:40:00', 'NV003', 'KH004', N'SYM Attila VI 125 2024',           1, 33500000, N'Tiền mặt'),
('HD025', '2025-06-10 15:55:00', 'NV002', 'KH005', N'BMW R 1250 GS Adventure 2024',     1,690000000, N'Chuyển khoản'),
('HD026', '2025-06-15 08:00:00', 'NV008', 'KH006', N'Lốp Dunlop ScootSmart',            2,  2300000, N'Chuyển khoản'),
('HD027', '2025-06-20 11:15:00', 'NV003', 'KH007', N'Triumph Street Triple 765 RS 2024',1,365000000, N'Chuyển khoản'),
('HD028', '2025-06-28 14:00:00', 'NV002', 'KH008', N'Triumph Bonneville T100 2024',     1,318000000, N'Chuyển khoản'),
('HD029', '2025-07-05 07:30:00', 'NV004', 'KH009', N'Căn chỉnh khe hở xú páp (Đồng tiền)',1, 180000, N'Tiền mặt'),
('HD029', '2025-07-05 07:30:00', 'NV004', 'KH009', N'Lọc gió HRC Racing',               1,    95000, N'Tiền mặt'),
('HD030', '2025-07-12 10:10:00', 'NV003', 'KH010', N'KTM RC 390 2024',                  1,158000000, N'Trả góp'),
('HD031', '2025-07-18 09:00:00', 'NV002', 'KH001', N'Vespa GTS Super 300 2024',         1,165000000, N'Chuyển khoản'),
('HD032', '2025-07-25 13:00:00', 'NV005', 'KH002', N'Che két nước CNC Nhôm nguyên khối',1,   250000, N'Chuyển khoản'),
('HD032', '2025-07-25 13:00:00', 'NV005', 'KH002', N'Kính chiếu hậu H2C',               2,   960000, N'Chuyển khoản'),
('HD033', '2025-08-01 16:20:00', 'NV008', 'KH003', N'Nhớt Castrol Power1 Racing 4T',    1,   145000, N'Tiền mặt'),
('HD033', '2025-08-01 16:20:00', 'NV008', 'KH003', N'Rửa xe nội thất + ngoại thất',     1,    50000, N'Tiền mặt'),
('HD034', '2025-08-08 11:30:00', 'NV010', 'KH004', N'Honda Vision 2024',                1, 34990000, N'Tiền mặt'),
('HD035', '2025-08-15 08:45:00', 'NV004', 'KH005', N'Bảo dưỡng định kỳ toàn bộ',        1,   199000, N'Chuyển khoản'),
('HD035', '2025-08-15 08:45:00', 'NV004', 'KH005', N'Nhớt Motul 7100 300V 4T 10W-40',   1,   420000, N'Chuyển khoản'),
('HD035', '2025-08-15 08:45:00', 'NV004', 'KH005', N'Lọc gió HRC Racing',               1,    95000, N'Chuyển khoản'),
('HD035', '2025-08-15 08:45:00', 'NV004', 'KH005', N'Bugi NGK CR8EH-9',                 1,    55000, N'Chuyển khoản'),
('HD036', '2025-08-20 14:15:00', 'NV002', 'KH006', N'Honda Winner X 2024',              1, 46900000, N'Trả góp'),
('HD037', '2025-08-25 10:45:00', 'NV003', 'KH007', N'Yamaha Exciter 155 2024',          1, 52900000, N'Chuyển khoản'),
('HD038', '2025-08-28 15:30:00', 'NV005', 'KH008', N'Phục hồi phuộc nhún trước/sau',    1,   250000, N'Tiền mặt'),
('HD038', '2025-08-28 15:30:00', 'NV005', 'KH008', N'Ép chảng ba, cân vành',            1,   300000, N'Tiền mặt'),
('HD039', '2025-09-02 09:15:00', 'NV008', 'KH009', N'Bình ắc quy GS MF GTZ6V',          1,   450000, N'Chuyển khoản'),
('HD039', '2025-09-02 09:15:00', 'NV008', 'KH009', N'Bóng đèn pha LED Philips',         1,   350000, N'Chuyển khoản'),
('HD040', '2025-09-10 13:00:00', 'NV004', 'KH010', N'Sửa chữa động cơ nặng',            1,   800000, N'Chuyển khoản'),
('HD040', '2025-09-10 13:00:00', 'NV004', 'KH010', N'Piston + xéc măng STD 57mm',       1,   680000, N'Chuyển khoản'),
('HD040', '2025-09-10 13:00:00', 'NV004', 'KH010', N'Nước làm mát Liqui Moly Đỏ',       1,   185000, N'Chuyển khoản'),
('HD040', '2025-09-10 13:00:00', 'NV004', 'KH010', N'Nhớt Motul 7100 300V 4T 10W-40',   1,   420000, N'Chuyển khoản');

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
