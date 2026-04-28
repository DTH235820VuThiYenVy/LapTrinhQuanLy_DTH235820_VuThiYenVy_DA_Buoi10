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
    [Table("KhachHang")]
    public class KhachHang
    {
        [Key]
        [StringLength(10)]
        public string MaKH { get; set; }

        [Required]
        [StringLength(50)]
        public string HoTenKH { get; set; }

        [StringLength(15)]
        public string SDT { get; set; }

        [StringLength(100)]
        public string DiaChi { get; set; }

        public int DiemTichLuy { get; set; } = 0;
        public DateTime? NgaySinh { get; set; }

        public virtual ObservableCollectionListSource<HoaDon> HoaDons { get; } = new();
    }
}
