using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangMyPham.TienIch
{
    public class CoGianGiaoDien
    {
        private Form _formHienTai;
        private Rectangle _kichThuocFormGoc;
        private Dictionary<Control, Rectangle> _kichThuocControlGoc;
        private Dictionary<Control, float> _coChuGoc;

        public CoGianGiaoDien(Form formCanCoGian) 
        {
            _formHienTai = formCanCoGian;
            _kichThuocControlGoc = new Dictionary<Control, Rectangle>();
            _coChuGoc = new Dictionary<Control, float>();

            _formHienTai.Load += FormHienTai_KhiTai;
            _formHienTai.Resize += FormHienTai_KhiThayDoiKichThuoc;
        }

        private void FormHienTai_KhiTai(object sender, EventArgs e)
        {
            // Lưu lại khung viền (kích thước, vị trí) của Form lúc vừa chạy
            _kichThuocFormGoc = new Rectangle(_formHienTai.Location.X, _formHienTai.Location.Y, _formHienTai.Width, _formHienTai.Height);

            // Quét tất cả các control bên trong
            LuuThongSoBanDau(_formHienTai);
        }

        private void LuuThongSoBanDau(Control controlCha)
        {
            foreach (Control controlCon in controlCha.Controls)
            {
                // Ghi nhớ vị trí, kích thước và cỡ chữ gốc của từng công cụ
                _kichThuocControlGoc.Add(controlCon, new Rectangle(controlCon.Location.X, controlCon.Location.Y, controlCon.Width, controlCon.Height));
                _coChuGoc.Add(controlCon, controlCon.Font.Size);

                // Đệ quy: Nếu trong công cụ này có chứa công cụ khác (vd: GroupBox, Panel) thì quét tiếp
                if (controlCon.HasChildren)
                {
                    LuuThongSoBanDau(controlCon);
                }
            }
        }

        private void FormHienTai_KhiThayDoiKichThuoc(object sender, EventArgs e)
        {
            // Nếu Form đang bị thu nhỏ (Minimize) hoặc chưa có dữ liệu gốc thì không làm gì cả
            if (_formHienTai.WindowState == FormWindowState.Minimized || _kichThuocFormGoc.Width == 0)
                return;

            // Tính tỷ lệ thay đổi (Biến cục bộ: camelCase)
            float tyLeNgang = (float)_formHienTai.Width / _kichThuocFormGoc.Width;
            float tyLeDoc = (float)_formHienTai.Height / _kichThuocFormGoc.Height;

            ThucHienCoGian(_formHienTai, tyLeNgang, tyLeDoc);
        }

        private void ThucHienCoGian(Control controlCha, float tyLeNgang, float tyLeDoc)
        {
            foreach (Control controlCon in controlCha.Controls)
            {
                // Kiểm tra xem control này đã được lưu thông số gốc chưa
                if (_kichThuocControlGoc.ContainsKey(controlCon))
                {
                    Rectangle kichThuocCu = _kichThuocControlGoc[controlCon];
                    float coChuCu = _coChuGoc[controlCon];

                    // Tính thông số mới
                    int toaDoXMoi = (int)(kichThuocCu.X * tyLeNgang);
                    int toaDoYMoi = (int)(kichThuocCu.Y * tyLeDoc);
                    int chieuRongMoi = (int)(kichThuocCu.Width * tyLeNgang);
                    int chieuCaoMoi = (int)(kichThuocCu.Height * tyLeDoc);

                    // Cập nhật vị trí và kích thước
                    controlCon.SetBounds(toaDoXMoi, toaDoYMoi, chieuRongMoi, chieuCaoMoi);

                    // Cập nhật cỡ chữ (lấy tỷ lệ nhỏ hơn để tránh chữ to quá tràn ra ngoài ô)
                    float tyLeNhoNhat = Math.Min(tyLeNgang, tyLeDoc);
                    float coChuMoi = coChuCu * tyLeNhoNhat;

                    if (coChuMoi < 8) coChuMoi = 8; // Giới hạn cỡ chữ nhỏ nhất là 8

                    controlCon.Font = new Font(controlCon.Font.FontFamily, coChuMoi, controlCon.Font.Style);
                }

                // Tiếp tục co giãn cho các control con bên trong
                if (controlCon.HasChildren)
                {
                    ThucHienCoGian(controlCon, tyLeNgang, tyLeDoc);
                }
            }
        }
    }
}
