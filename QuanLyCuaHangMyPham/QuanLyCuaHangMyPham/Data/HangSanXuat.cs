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
    [Table("HangSanXuat")]
    public class HangSanXuat
    {
        [Key]
        [StringLength(10)]
        public string MaHSX { get; set; }

        [Required]
        [StringLength(50)]
        public string TenHSX { get; set; }

        [StringLength(50)]
        public string XuatXu { get; set; }

        public virtual ObservableCollectionListSource<SanPham> SanPhams { get; } = new();
    }
}
