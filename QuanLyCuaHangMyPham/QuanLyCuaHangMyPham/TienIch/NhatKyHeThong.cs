using QuanLyCuaHangMyPham.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangMyPham.TienIch
{
    public static class NhatKyHeThong
    {
        public static void GhiLog(int maTK, string hanhDong)
        {
            try
            {
                using (var db = new QLCHMPDbContext())
                {
                    var log = new LichSuHoatDong
                    {
                        MaTK = maTK,
                        HanhDong = hanhDong,
                        ThoiGian = DateTime.Now
                    };
                    db.LichSuHoatDong.Add(log);
                    db.SaveChanges();
                }
            }
            catch { /* Bỏ qua nếu lỗi log để không làm đứng phần mềm */ }
        }
    }
}
