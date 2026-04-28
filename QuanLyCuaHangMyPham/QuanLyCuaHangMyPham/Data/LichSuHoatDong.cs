using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangMyPham.Data
{
    [Table("LichSuHoatDong")]
    public class LichSuHoatDong
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Tự động tăng 1, 2, 3...
        public int MaLS { get; set; }

        public int MaTK { get; set; }

        [ForeignKey("MaTK")]
        public virtual TaiKhoan TaiKhoan { get; set; }

        [Required]
        [StringLength(255)]
        public string HanhDong { get; set; } // Ghi chi tiết: "Đăng nhập", "Thêm Sản Phẩm SP001", "Xóa Phiếu Chi PC002"...

        public DateTime ThoiGian { get; set; } = DateTime.Now; // Thời điểm thực hiện hành động
    }
}
