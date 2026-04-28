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
    [Table("LoaiSanPham")]
    public class LoaiSanPham
    {
        [Key]
        [StringLength(10)]
        public string MaLoai { get; set; }

        [Required]
        [StringLength(50)]
        public string TenLoai { get; set; }

        public virtual ObservableCollectionListSource<SanPham> SanPhams { get; } = new();
    }
}
