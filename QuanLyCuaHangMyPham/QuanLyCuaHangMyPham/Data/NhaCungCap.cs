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
    [Table("NhaCungCap")]
    public class NhaCungCap
    {
        [Key]
        [StringLength(10)]
        public string MaNCC { get; set; }

        [Required]
        [StringLength(100)]
        public string TenNCC { get; set; }

        [StringLength(100)]
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }

        public virtual ObservableCollectionListSource<PhieuNhap> PhieuNhaps { get; } = new();
        public virtual ObservableCollectionListSource<PhieuChi> PhieuChis { get; } = new();

        public virtual ObservableCollectionListSource<SanPham> SanPhams { get; } = new();
    }
}
