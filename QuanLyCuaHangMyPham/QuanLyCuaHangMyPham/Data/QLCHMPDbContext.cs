using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using static QuanLyCuaHangMyPham.Data.CaLamViec;
using QuanLyCuaHangMyPham.TienIch;

namespace QuanLyCuaHangMyPham.Data
{
    public class QLCHMPDbContext : DbContext
    {
        //Khai báo các bảng (DbSet) 
        public DbSet<LoaiSanPham> LoaiSanPham { get; set; }
        public DbSet<HangSanXuat> HangSanXuat { get; set; }
        public DbSet<SanPham> SanPham { get; set; }
        public DbSet<NhanVien> NhanVien { get; set; }
        public DbSet<TaiKhoan> TaiKhoan { get; set; }
        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<NhaCungCap> NhaCungCap { get; set; }


        // Các bảng nghiệp vụ
        public DbSet<HoaDon> HoaDon { get; set; }
        public DbSet<HoaDon_ChiTiet> ChiTietHD { get; set; }
        public DbSet<PhieuNhap> PhieuNhap { get; set; }
        public DbSet<PhieuNhap_ChiTiet> ChiTietPN { get; set; }
        public DbSet<PhieuChi> PhieuChi { get; set; }

        // Các bảng hệ thống & bổ sung
        public DbSet<CauHinhGiaoDien> CauHinhGiaoDien { get; set; }
        public DbSet<LichSuHoatDong> LichSuHoatDong { get; set; }

        // Sửa DM_CaLam thành CaLamViec
        public virtual DbSet<CaLamViec> CaLamViec { get; set; }
        public virtual DbSet<PT_ThanhToan> PT_ThanhToan { get; set; }
        public DbSet<PhanCa> PhanCa { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Đọc chuỗi kết nối từ App.config
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["QLCHMPConnection"].ConnectionString);
        }

        // Cấu hình khóa chính
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HoaDon_ChiTiet>().HasKey(ct => new { ct.MaHD, ct.MaSP });
            modelBuilder.Entity<PhieuNhap_ChiTiet>().HasKey(ct => new { ct.MaPN, ct.MaSP });

            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }
        }
    }
}
