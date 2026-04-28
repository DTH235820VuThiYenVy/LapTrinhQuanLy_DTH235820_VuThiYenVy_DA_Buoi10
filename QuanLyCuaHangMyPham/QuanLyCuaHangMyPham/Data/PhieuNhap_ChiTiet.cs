using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangMyPham.Data
{
    [Table("PhieuNhap_ChiTiet")]
    public class PhieuNhap_ChiTiet
    {
        [StringLength(20)]
        public string MaPN { get; set; }
        [ForeignKey("MaPN")]
        public virtual PhieuNhap PhieuNhap { get; set; }

        [StringLength(10)]
        public string MaSP { get; set; }
        [ForeignKey("MaSP")]
        public virtual SanPham SanPham { get; set; }

        public int SoLuong { get; set; }
        public decimal GiaNhap { get; set; }
        public decimal ThanhTien { get; set; }
    }
    [NotMapped]
    public class DanhSachPhieuNhap_ChiTiet
    {
        public string MaPN { get; set; }
        public string MaSP { get; set; }

        public string TenSP { get; set; }

        public int SoLuong { get; set; }
        public decimal GiaNhap { get; set; }
        public decimal ThanhTien { get; set; }
    }
}
