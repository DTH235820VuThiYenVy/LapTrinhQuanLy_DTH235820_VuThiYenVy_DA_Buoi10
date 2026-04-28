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
    [Table("CaLamViec")]
    public class CaLamViec
    {
            [Key][StringLength(10)] public string MaCa { get; set; }
            public string TenCa { get; set; }
            public TimeSpan GioBatDau { get; set; }
            public TimeSpan GioKetThuc { get; set; }
            public string GhiChu { get; set; }
            public virtual ObservableCollectionListSource<PhanCa> PhanCas { get; } = new();
    }
}
