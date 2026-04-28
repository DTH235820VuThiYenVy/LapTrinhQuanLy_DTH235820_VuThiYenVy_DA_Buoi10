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
    [Table("PT_ThanhToan")]
    public class PT_ThanhToan
    {
            [Key]
            [StringLength(20)]
            public string MaPT { get; set; } // Ví dụ: TM, CK, POS

            [Required]
            [StringLength(50)]
            public string TenPT { get; set; } // Ví dụ: Tiền mặt, Chuyển khoản
            public virtual ObservableCollectionListSource<PhieuChi> PhieuChis { get; } = new();


    }
}
