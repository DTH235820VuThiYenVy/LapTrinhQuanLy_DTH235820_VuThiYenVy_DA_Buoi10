using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangMyPham.TienIch
{
    public class SinhMaTuDong
    {
        /// <summary>
        /// Hàm tự sinh mã dùng chung (VD: CA01, SP001, KH01...)
        /// </summary>
        /// <param name="danhSachMaHienTai">Danh sách các mã đang có trong bảng</param>
        /// <param name="tienTo">Chữ cái đầu (CA, SP, HD...)</param>
        /// <param name="doDaiSo">Số lượng chữ số đi kèm (2 hoặc 3...)</param>
        /// <returns>Mã mới tự động tăng</returns>
        public static string TuSinhMa(List<string> danhSachMaHienTai, string fieldName, string tienTo, int doDaiSo)
        {
            // 1. Nếu chưa có dữ liệu, trả về mã đầu tiên (VD: CA01)
            if (danhSachMaHienTai == null || danhSachMaHienTai.Count == 0)
            {
                return tienTo + 1.ToString("D" + doDaiSo);
            }

            // 2. Lọc ra những mã đúng định dạng tiền tố và lấy mã lớn nhất
            var maLonNhat = danhSachMaHienTai
                .Where(m => m.StartsWith(tienTo))
                .OrderByDescending(m => m)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(maLonNhat))
                return tienTo + 1.ToString("D" + doDaiSo);

            // 3. Tách phần số ra và tăng thêm 1
            try
            {
                string chuoiSoCu = maLonNhat.Substring(tienTo.Length);
                int soMoi = int.Parse(chuoiSoCu) + 1;

                // 4. Trả về mã mới (VD: CA + 05 = CA05)
                return tienTo + soMoi.ToString("D" + doDaiSo);
            }
            catch
            {
                return tienTo + 1.ToString("D" + doDaiSo);
            }
        }
    }
}
