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
    [Table("SanPham")]
    public class SanPham
    {
        [Key]
        [StringLength(10)]
        public string MaSP { get; set; }

        [Required]
        [StringLength(100)]
        public string TenSP { get; set; }

        public string HinhAnh { get; set; }
        public int SLTon { get; set; }
        public decimal GiaNhap { get; set; }
        public decimal GiaBan { get; set; }
        public bool TrangThai { get; set; } // True: Đang kinh doanh

        [StringLength(10)]
        public string MaLoai { get; set; }
        [ForeignKey("MaLoai")]
        public virtual LoaiSanPham LoaiSanPham { get; set; }

        [StringLength(10)]
        public string MaHSX { get; set; }
        [ForeignKey("MaHSX")]
        public virtual HangSanXuat HangSanXuat { get; set; }

        [StringLength(10)]
        public string MaNCC { get; set; }

        [ForeignKey("MaNCC")]
        public virtual NhaCungCap NhaCungCap { get; set; }
        public virtual ObservableCollectionListSource<HoaDon_ChiTiet> ChiTietHDs { get; } = new();
        public virtual ObservableCollectionListSource<PhieuNhap_ChiTiet> ChiTietPNs { get; } = new();
    }

    [NotMapped]
    public class DanhSachSanPham
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string TenLoai { get; set; } // hiện tên thay vì mã
        public string TenHSX { get; set; }  // hiện tên hãng

        public string TenNCC { get; set; }

        public decimal GiaNhap { get; set; }
        public decimal GiaBan { get; set; }
        public int SLTon { get; set; }
        public string? HinhAnh { get; set; }
        public string TrangThai { get; set; }
    }

}
