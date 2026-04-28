using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangMyPham.Data
{
    [Table("PhieuChi")]
    public class PhieuChi
    {
        [Key]
        [StringLength(20)]
        public string MaPC { get; set; }

        public DateTime NgayChi { get; set; } = DateTime.Now;
        public decimal SoTienChi { get; set; }

        [StringLength(20)]
        public string MaPT { get; set; }
        [ForeignKey("MaPT")]
        public virtual PT_ThanhToan PT_ThanhToan { get; set; }
        public string GhiChu { get; set; } 

        [StringLength(10)]
        public string MaNV { get; set; }
        [ForeignKey("MaNV")]
        public virtual NhanVien NhanVien { get; set; }

        // Cho phép Null (Dấu ?) để chi các khoản không phải trả NCC
        [StringLength(10)]
        public string? MaNCC { get; set; }
        [ForeignKey("MaNCC")]
        public virtual NhaCungCap? NhaCungCap { get; set; }

        // Cho phép Null (Dấu ?) vì có thể trả nợ gộp, không theo đơn cụ thể
        [StringLength(20)]
        public string? MaPN { get; set; }


    }
}
