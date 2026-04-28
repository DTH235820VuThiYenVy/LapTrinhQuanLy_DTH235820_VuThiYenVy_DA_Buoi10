using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QuanLyCuaHangMyPham.Data.CaLamViec;

namespace QuanLyCuaHangMyPham.Data
{
    [Table("PhanCa")]
    public class PhanCa
    {
        [Key][StringLength(10)] public string MaPC { get; set; }
        public DateTime NgayLam { get; set; }

        [StringLength(10)] public string MaNV { get; set; }
        [ForeignKey("MaNV")] public virtual NhanVien NhanVien { get; set; }

        [StringLength(10)] public string MaCa { get; set; }
        [ForeignKey("MaCa")] public virtual CaLamViec CaLamViec { get; set; }
        [StringLength(255)] public string GhiChu { get; set; }
    }
}
