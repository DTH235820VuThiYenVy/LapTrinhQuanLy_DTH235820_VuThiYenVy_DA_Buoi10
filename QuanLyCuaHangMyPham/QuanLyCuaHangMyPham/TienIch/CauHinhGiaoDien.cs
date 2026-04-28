using QuanLyCuaHangMyPham.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangMyPham.TienIch
{
    [Table("CauHinhGiaoDien")]
    public class CauHinhGiaoDien
    {
        [Key]
        [StringLength(10)]
        public string MaNV { get; set; } // Khóa chính là MaNV
        public string MauNen { get; set; }
        public string MauChu { get; set; }
        public string FontChu { get; set; }
        public int CoChu { get; set; }
        public string Theme { get; set; }

        [ForeignKey("MaNV")]
        public virtual NhanVien NhanVien { get; set; }
    }
}
