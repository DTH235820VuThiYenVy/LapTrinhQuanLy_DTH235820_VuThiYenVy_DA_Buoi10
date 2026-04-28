using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangMyPham.Data
{
    [Table("HoaDon_ChiTiet")]
    public class HoaDon_ChiTiet
    {
        [StringLength(20)]
        public string MaHD { get; set; }
        [ForeignKey("MaHD")] 
        public virtual HoaDon HoaDon { get; set; }

        [StringLength(10)]
        public string MaSP { get; set; }
        [ForeignKey("MaSP")]
        public virtual SanPham SanPham { get; set; }

        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }
        public decimal ThanhTien { get; set; }
    }

    [NotMapped] 
    public class DanhSachHoaDon_ChiTiet
    {
        public string MaHD { get; set; }
        public string MaSP { get; set; }
        public string TenSP { get; set; }

        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }
        public decimal ThanhTien { get; set; }
    }
}
