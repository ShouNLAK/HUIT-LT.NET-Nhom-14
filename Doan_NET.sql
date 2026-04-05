-- 1. Tạo bảng Hãng Xe (Brand)
CREATE TABLE HangXe (
    MaHang VARCHAR(20) PRIMARY KEY, 
    TenHang NVARCHAR(100) NOT NULL, 
    QuocGia NVARCHAR(50),          
    LogoPath NVARCHAR(255)          
);

-- 2. Tạo bảng Xe (Vehicle)
CREATE TABLE Xe (
    MaXe VARCHAR(20) PRIMARY KEY,   
    TenXe NVARCHAR(100) NOT NULL,   
    LoaiXe NVARCHAR(50),            
    NamSX INT,                      
    GiaBan DECIMAL(18, 2),          
    MauSac NVARCHAR(30),            
    MoTa NVARCHAR(MAX),             
    HinhAnh NVARCHAR(255),         
    MaHang VARCHAR(20),             -- Khóa ngoại liên kết tới HangXe
    CONSTRAINT FK_Xe_HangXe FOREIGN KEY (MaHang) REFERENCES HangXe(MaHang)
);

-- 3. Tạo bảng Khách Hàng (Customer)
CREATE TABLE KhachHang (
    MaKH VARCHAR(20) PRIMARY KEY,   
    HoTen NVARCHAR(100) NOT NULL,   
    SDT VARCHAR(15),               
    CCCD VARCHAR(20),               
    Email VARCHAR(100),           
    DiaChi NVARCHAR(255)            
);

-- 4. Tạo bảng Nhân Viên (Employee)
CREATE TABLE NhanVien (
    MaNV VARCHAR(20) PRIMARY KEY,   
    HoTen NVARCHAR(100) NOT NULL,  
    ChucVu NVARCHAR(50),            
    NgayVaoLam DATE,                
    TrangThai NVARCHAR(50)          
);

-- 5. Tạo bảng Tài Khoản (Account)
CREATE TABLE TaiKhoan (
    Username VARCHAR(50) PRIMARY KEY, 
    Password VARCHAR(255) NOT NULL,   
    Role NVARCHAR(50),                
    MaLienKet VARCHAR(20),            
    
);

-- 6. Tạo bảng Dịch Vụ & Phụ Tùng (Service & Part)
CREATE TABLE DichVuPhuTung (
    MaPT VARCHAR(20) PRIMARY KEY,   
    Ten NVARCHAR(100) NOT NULL,      
    Gia DECIMAL(18, 2),              
    TonKho INT                      
);

-- 7. Tạo bảng Hóa Đơn (Invoice)
CREATE TABLE HoaDon (
    MaHD VARCHAR(20) PRIMARY KEY,    
    NgayLap DATETIME DEFAULT GETDATE(),
    MaNV VARCHAR(20),                
    MaKH VARCHAR(20),              
    TenDV_SP NVARCHAR(255),         
    SoLuong INT,                     
    ThanhTien DECIMAL(18, 2),       
    PhuongThucThanhToan NVARCHAR(50),
    CONSTRAINT FK_HoaDon_NhanVien FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    CONSTRAINT FK_HoaDon_KhachHang FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)
);