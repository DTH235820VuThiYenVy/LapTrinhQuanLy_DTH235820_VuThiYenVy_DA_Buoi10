using Microsoft.EntityFrameworkCore.ChangeTracking;
using QuanLyCuaHangMyPham.TienIch;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangMyPham.Data
{
    [Table("NhanVien")]
    public class NhanVien
    {
        [Key]
        [StringLength(10)]
        public string MaNV { get; set; }

        [Required]
        [StringLength(50)]
        public string HoTenNV { get; set; }

        public string SDT { get; set; }
        public string ChucVu { get; set; }
        public decimal LuongCoBan { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public DateTime NgayVao { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Email { get; set; }
        public virtual TaiKhoan TaiKhoan { get; set; }
        public virtual CauHinhGiaoDien CauHinhGiaoDien { get; set; }

        public virtual ObservableCollectionListSource<HoaDon> HoaDons { get; } = new();
        public virtual ObservableCollectionListSource<PhieuNhap> PhieuNhaps { get; } = new();
        public virtual ObservableCollectionListSource<PhieuChi> PhieuChis { get; } = new();
        public virtual ObservableCollectionListSource<PhanCa> PhanCas { get; } = new();
    }
}
