using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangMyPham.Migrations
{
    /// <inheritdoc />
    public partial class ManualFixNCC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
    name: "MaNCC",
    table: "SanPham",
    type: "nvarchar(10)",
    maxLength: 10,
    nullable: true);

            migrationBuilder.DropForeignKey(
                name: "FK_CauHinhGiaoDien_NhanVien_MaNV",
                table: "CauHinhGiaoDien");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_KhachHang_MaKH",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_NhanVien_MaNV",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_ChiTiet_HoaDon_MaHD",
                table: "HoaDon_ChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_ChiTiet_SanPham_MaSP",
                table: "HoaDon_ChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_LichSuHoatDong_TaiKhoan_MaTK",
                table: "LichSuHoatDong");

            migrationBuilder.DropForeignKey(
                name: "FK_PhanCa_CaLamViec_MaCa",
                table: "PhanCa");

            migrationBuilder.DropForeignKey(
                name: "FK_PhanCa_NhanVien_MaNV",
                table: "PhanCa");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuChi_NhanVien_MaNV",
                table: "PhieuChi");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuChi_PT_ThanhToan_MaPT",
                table: "PhieuChi");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhap_NhanVien_MaNV",
                table: "PhieuNhap");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhap_ChiTiet_PhieuNhap_MaPN",
                table: "PhieuNhap_ChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhap_ChiTiet_SanPham_MaSP",
                table: "PhieuNhap_ChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPham_HangSanXuat_MaHSX",
                table: "SanPham");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPham_LoaiSanPham_MaLoai",
                table: "SanPham");

            migrationBuilder.DropForeignKey(
                name: "FK_TaiKhoan_NhanVien_MaNV",
                table: "TaiKhoan");

            migrationBuilder.AddForeignKey(
                name: "FK_CauHinhGiaoDien_NhanVien_MaNV",
                table: "CauHinhGiaoDien",
                column: "MaNV",
                principalTable: "NhanVien",
                principalColumn: "MaNV");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_KhachHang_MaKH",
                table: "HoaDon",
                column: "MaKH",
                principalTable: "KhachHang",
                principalColumn: "MaKH");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_NhanVien_MaNV",
                table: "HoaDon",
                column: "MaNV",
                principalTable: "NhanVien",
                principalColumn: "MaNV");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_ChiTiet_HoaDon_MaHD",
                table: "HoaDon_ChiTiet",
                column: "MaHD",
                principalTable: "HoaDon",
                principalColumn: "MaHD");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_ChiTiet_SanPham_MaSP",
                table: "HoaDon_ChiTiet",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP");

            migrationBuilder.AddForeignKey(
                name: "FK_LichSuHoatDong_TaiKhoan_MaTK",
                table: "LichSuHoatDong",
                column: "MaTK",
                principalTable: "TaiKhoan",
                principalColumn: "MaTK");

            migrationBuilder.AddForeignKey(
                name: "FK_PhanCa_CaLamViec_MaCa",
                table: "PhanCa",
                column: "MaCa",
                principalTable: "CaLamViec",
                principalColumn: "MaCa");

            migrationBuilder.AddForeignKey(
                name: "FK_PhanCa_NhanVien_MaNV",
                table: "PhanCa",
                column: "MaNV",
                principalTable: "NhanVien",
                principalColumn: "MaNV");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuChi_NhanVien_MaNV",
                table: "PhieuChi",
                column: "MaNV",
                principalTable: "NhanVien",
                principalColumn: "MaNV");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuChi_PT_ThanhToan_MaPT",
                table: "PhieuChi",
                column: "MaPT",
                principalTable: "PT_ThanhToan",
                principalColumn: "MaPT");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNhap_NhanVien_MaNV",
                table: "PhieuNhap",
                column: "MaNV",
                principalTable: "NhanVien",
                principalColumn: "MaNV");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNhap_ChiTiet_PhieuNhap_MaPN",
                table: "PhieuNhap_ChiTiet",
                column: "MaPN",
                principalTable: "PhieuNhap",
                principalColumn: "MaPN");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNhap_ChiTiet_SanPham_MaSP",
                table: "PhieuNhap_ChiTiet",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPham_HangSanXuat_MaHSX",
                table: "SanPham",
                column: "MaHSX",
                principalTable: "HangSanXuat",
                principalColumn: "MaHSX");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPham_LoaiSanPham_MaLoai",
                table: "SanPham",
                column: "MaLoai",
                principalTable: "LoaiSanPham",
                principalColumn: "MaLoai");

            migrationBuilder.AddForeignKey(
                name: "FK_TaiKhoan_NhanVien_MaNV",
                table: "TaiKhoan",
                column: "MaNV",
                principalTable: "NhanVien",
                principalColumn: "MaNV");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "MaNCC", table: "SanPham");

            migrationBuilder.DropForeignKey(
                name: "FK_CauHinhGiaoDien_NhanVien_MaNV",
                table: "CauHinhGiaoDien");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_KhachHang_MaKH",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_NhanVien_MaNV",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_ChiTiet_HoaDon_MaHD",
                table: "HoaDon_ChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_ChiTiet_SanPham_MaSP",
                table: "HoaDon_ChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_LichSuHoatDong_TaiKhoan_MaTK",
                table: "LichSuHoatDong");

            migrationBuilder.DropForeignKey(
                name: "FK_PhanCa_CaLamViec_MaCa",
                table: "PhanCa");

            migrationBuilder.DropForeignKey(
                name: "FK_PhanCa_NhanVien_MaNV",
                table: "PhanCa");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuChi_NhanVien_MaNV",
                table: "PhieuChi");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuChi_PT_ThanhToan_MaPT",
                table: "PhieuChi");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhap_NhanVien_MaNV",
                table: "PhieuNhap");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhap_ChiTiet_PhieuNhap_MaPN",
                table: "PhieuNhap_ChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhap_ChiTiet_SanPham_MaSP",
                table: "PhieuNhap_ChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPham_HangSanXuat_MaHSX",
                table: "SanPham");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPham_LoaiSanPham_MaLoai",
                table: "SanPham");

            migrationBuilder.DropForeignKey(
                name: "FK_TaiKhoan_NhanVien_MaNV",
                table: "TaiKhoan");

            migrationBuilder.AddForeignKey(
                name: "FK_CauHinhGiaoDien_NhanVien_MaNV",
                table: "CauHinhGiaoDien",
                column: "MaNV",
                principalTable: "NhanVien",
                principalColumn: "MaNV",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_KhachHang_MaKH",
                table: "HoaDon",
                column: "MaKH",
                principalTable: "KhachHang",
                principalColumn: "MaKH",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_NhanVien_MaNV",
                table: "HoaDon",
                column: "MaNV",
                principalTable: "NhanVien",
                principalColumn: "MaNV",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_ChiTiet_HoaDon_MaHD",
                table: "HoaDon_ChiTiet",
                column: "MaHD",
                principalTable: "HoaDon",
                principalColumn: "MaHD",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_ChiTiet_SanPham_MaSP",
                table: "HoaDon_ChiTiet",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LichSuHoatDong_TaiKhoan_MaTK",
                table: "LichSuHoatDong",
                column: "MaTK",
                principalTable: "TaiKhoan",
                principalColumn: "MaTK",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PhanCa_CaLamViec_MaCa",
                table: "PhanCa",
                column: "MaCa",
                principalTable: "CaLamViec",
                principalColumn: "MaCa",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PhanCa_NhanVien_MaNV",
                table: "PhanCa",
                column: "MaNV",
                principalTable: "NhanVien",
                principalColumn: "MaNV",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuChi_NhanVien_MaNV",
                table: "PhieuChi",
                column: "MaNV",
                principalTable: "NhanVien",
                principalColumn: "MaNV",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuChi_PT_ThanhToan_MaPT",
                table: "PhieuChi",
                column: "MaPT",
                principalTable: "PT_ThanhToan",
                principalColumn: "MaPT",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNhap_NhanVien_MaNV",
                table: "PhieuNhap",
                column: "MaNV",
                principalTable: "NhanVien",
                principalColumn: "MaNV",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNhap_ChiTiet_PhieuNhap_MaPN",
                table: "PhieuNhap_ChiTiet",
                column: "MaPN",
                principalTable: "PhieuNhap",
                principalColumn: "MaPN",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNhap_ChiTiet_SanPham_MaSP",
                table: "PhieuNhap_ChiTiet",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPham_HangSanXuat_MaHSX",
                table: "SanPham",
                column: "MaHSX",
                principalTable: "HangSanXuat",
                principalColumn: "MaHSX",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPham_LoaiSanPham_MaLoai",
                table: "SanPham",
                column: "MaLoai",
                principalTable: "LoaiSanPham",
                principalColumn: "MaLoai",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TaiKhoan_NhanVien_MaNV",
                table: "TaiKhoan",
                column: "MaNV",
                principalTable: "NhanVien",
                principalColumn: "MaNV",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
