using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangMyPham.Data
{
    [Table("PhieuNhap")]
    public class PhieuNhap
    {
        [Key]
        [StringLength(20)]
        public string MaPN { get; set; }
        public DateTime NgayNhap { get; set; }
        public decimal TongChiPhi { get; set; } 
        public decimal SoTienDaTra { get; set; }
        public string TrangThai { get; set; }
        public string GhiChu { get; set; }
        public string PT_ThanhToan { get; set; }

        [StringLength(10)]
        public string MaNV { get; set; }
        [ForeignKey("MaNV")]
        public virtual NhanVien NhanVien { get; set; }

        [StringLength(10)]
        public string MaNCC { get; set; }
        [ForeignKey("MaNCC")]
        public virtual NhaCungCap NhaCungCap { get; set; }

        public virtual ObservableCollectionListSource<PhieuNhap_ChiTiet> ChiTietPNs { get; } = new();
    }

    [NotMapped]
    public class DanhSachPhieuNhap
    {
        public string MaPN { get; set; }
        public string MaNV { get; set; }
        public string MaNCC { get; set; }

        public string HoTenNV { get; set; }
        public string TenNCC { get; set; }

        public DateTime NgayNhap { get; set; }
        public decimal TongChiPhi { get; set; }
        public decimal SoTienDaTra { get; set; }
        public string TrangThai { get; set; }
        public string GhiChu { get; set; }
        public string PT_ThanhToan { get; set; }

        public string XemChiTiet { get; set; } = "Xem chi tiết";
    }
}
