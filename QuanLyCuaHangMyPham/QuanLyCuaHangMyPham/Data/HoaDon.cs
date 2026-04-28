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
    [Table("HoaDon")]
    public class HoaDon
    {
        [Key]
        [StringLength(20)]
        public string MaHD { get; set; }
        public DateTime NgayLap { get; set; }
        public decimal TongTien { get; set; }
        public decimal GiamGia { get; set; }
        public string PT_ThanhToan { get; set; }
        public string GhiChu { get; set; }

        [StringLength(10)]
        public string MaNV { get; set; }
        [ForeignKey("MaNV")]
        public virtual NhanVien NhanVien { get; set; }

        [StringLength(10)]
        public string MaKH { get; set; }
        [ForeignKey("MaKH")]
        public virtual KhachHang KhachHang { get; set; }

        //Số điểm khách đã sử dụng để đổi lấy mức giảm 5% của hóa đơn này
        public int DiemDaDung { get; set; } = 0;

        //Số điểm khách nhận được thêm sau khi thanh toán hóa đơn này
        public int DiemCongThem { get; set; } = 0;

        public virtual ObservableCollectionListSource<HoaDon_ChiTiet> ChiTietHDs { get; } = new();
    }

    [NotMapped]
    public class DanhSachHoaDon
    {
        public string MaKH  { get; set; }
        public string MaNV { get; set; }
        public string MaHD { get; set; }
        public string HoTenNV { get; set; }
        public string HoTenKH { get; set; }
        public DateTime NgayLap { get; set; }
        public double? TongTien { get; set; }
        public decimal GiamGia { get; set; }
        public string PT_ThanhToan { get; set; }
        public string GhiChu { get; set; }
        public string XemChiTiet { get; set; } = "Xem chi tiết";


    }
}
