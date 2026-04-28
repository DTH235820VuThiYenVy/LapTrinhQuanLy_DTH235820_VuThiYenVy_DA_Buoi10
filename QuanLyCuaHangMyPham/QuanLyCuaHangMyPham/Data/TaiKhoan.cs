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
    [Table("TaiKhoan")]
    public class TaiKhoan
    {
        [Key]
        [StringLength(10)]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaTK { get; set; }
        [Required]
        [StringLength(50)]
        public string TenDangNhap { get; set; }
        [Required]
        public string MatKhau { get; set; }
        public string QuyenHan { get; set; }
        public bool TrangThai { get; set; } = true;
        [StringLength(10)]
        public string MaNV { get; set; }
        [ForeignKey("MaNV")]
        public virtual NhanVien NhanVien { get; set; }

        public virtual ObservableCollectionListSource<LichSuHoatDong> LichSuHoatDongs { get; } = new();
    }
}
