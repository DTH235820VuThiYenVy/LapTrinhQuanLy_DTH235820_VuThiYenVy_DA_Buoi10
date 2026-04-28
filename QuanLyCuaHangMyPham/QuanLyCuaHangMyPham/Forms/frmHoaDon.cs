using ClosedXML.Excel;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using QuanLyCuaHangMyPham.Data;
using QuanLyCuaHangMyPham.Reports;
using QuanLyCuaHangMyPham.TienIch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCuaHangMyPham.Forms
{
    public partial class frmHoaDon : Form
    {
        private QLCHMPDbContext _context = new QLCHMPDbContext();
        private CoGianGiaoDien _hoTroCoGianForm;
        private string _maHD = "";

        private int _maTKDangNhap;

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        public frmHoaDon(int maTKDangNhap)
        {
            InitializeComponent();
            _hoTroCoGianForm = new CoGianGiaoDien(this);
            _maTKDangNhap = maTKDangNhap;
        }

        private void TaiDuLieuLenBang()
        {
            _context = new QLCHMPDbContext();
            dgvDSHoaDon.AutoGenerateColumns = false;

            // Dùng kiểu dữ liệu ẩn danh (anonymous type) để lấy luôn MaNV và MaKH lên lưới
            var hd = _context.HoaDon.Select(r => new
            {
                MaHD = r.MaHD,
                MaNV = r.MaNV,               // <-- Đã thêm Mã NV
                MaKH = r.MaKH,               // <-- Đã thêm Mã KH
                HoTenNV = r.NhanVien.HoTenNV,
                HoTenKH = r.KhachHang.HoTenKH,
                NgayLap = r.NgayLap,
                GhiChu = r.GhiChu,
                PT_ThanhToan = r.PT_ThanhToan, // Chỉ lấy chuỗi Tên PT, không dùng mã
                GiamGia = r.GiamGia,
                TongTien = r.TongTien,
                XemChiTiet = "Xem chi tiết"
            }).OrderByDescending(h => h.NgayLap).ToList();

            dgvDSHoaDon.DataSource = hd;
        }

        private void frmHoaDon_Load(object sender, EventArgs e)
        {
            TaiDuLieuLenBang();
            SendMessage(txtTimKiem.Handle, EM_SETCUEBANNER, 0, "Nhập mã HĐ, tên nhân viên hoặc khách hàng...");
        }


        private void dgvDSHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvDSHoaDon.Columns[e.ColumnIndex].Name == "XemChiTiet")
            {
                var cellValue = dgvDSHoaDon.Rows[e.RowIndex].Cells["MaHD"].Value;
                if (cellValue != null)
                {
                    string maHD = cellValue.ToString();
                    using (frmHoaDon_ChiTiet frm = new frmHoaDon_ChiTiet(maHD))
                    {
                        frm.ShowDialog();
                        TaiDuLieuLenBang();
                    }
                }
            }
        }

        private void dgvDSHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var value = dgvDSHoaDon.Rows[e.RowIndex].Cells["MaHD"].Value;
            if (value != null)
            {
                _maHD = value.ToString();
            }
        }


        private void btnLapHD_Click(object sender, EventArgs e)
        {
            using (frmHoaDon_ChiTiet chiTiet = new frmHoaDon_ChiTiet())
            {
                chiTiet.ShowDialog();
                TaiDuLieuLenBang();
            }

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_maHD))
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (frmHoaDon_ChiTiet chiTiet = new frmHoaDon_ChiTiet(_maHD))
            {
                chiTiet.ShowDialog();
            }
            TaiDuLieuLenBang();

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_maHD))
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần xóa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa hóa đơn {_maHD}?\nHành động này sẽ trả lại số lượng sản phẩm vào kho.",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                try
                {
                    var hd = _context.HoaDon.Find(_maHD);
                    if (hd != null)
                    {
                        var listChiTiet = _context.ChiTietHD.Where(ct => ct.MaHD == _maHD).ToList();
                        foreach (var ct in listChiTiet)
                        {
                            var sp = _context.SanPham.Find(ct.MaSP);
                            if (sp != null)
                            {
                                sp.SLTon += ct.SoLuong; // Cộng trả lại kho
                            }
                        }

                        _context.ChiTietHD.RemoveRange(listChiTiet);
                        _context.HoaDon.Remove(hd);
                        _context.SaveChanges();
                        NhatKyHeThong.GhiLog(_maTKDangNhap, $"Xóa hóa đơn {_maHD} và hoàn lại số lượng sản phẩm vào kho");

                        MessageBox.Show("Xóa hóa đơn và hoàn kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _maHD = "";
                        TaiDuLieuLenBang();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thực hiện xóa: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            TaiDuLieuLenBang();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNhap_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Tập tin Excel|*.xlsx;*.xls" };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                int thanhCongHD = 0, dongLoiHD = 0;
                int thanhCongCT = 0, dongLoiCT = 0;
                StringBuilder chiTietLoi = new StringBuilder();

                try
                {
                    using (XLWorkbook workbook = new XLWorkbook(openFileDialog.FileName))
                    {
                        // ============================================
                        // 1. NHẬP SHEET HOADON
                        // ============================================
                        var worksheetHD = workbook.Worksheet("HoaDon");
                        var rowsHD = worksheetHD.RowsUsed().Skip(1); // Bỏ qua tiêu đề

                        foreach (var row in rowsHD)
                        {
                            // 1. LÀM MỚI CONTEXT mỗi dòng để tránh lỗi Tracked và cập nhật dữ liệu mới nhất
                            _context = new QLCHMPDbContext();

                            try
                            {
                                // Đọc dữ liệu từ các cột
                                string maHD = row.Cell(1).GetFormattedString().Trim();
                                string maNV = row.Cell(2).GetFormattedString().Trim();
                                string maKH = row.Cell(4).GetFormattedString().Trim();
                                string ptThanhToan = row.Cell(11).GetFormattedString().Trim();

                                // 2. KIỂM TRA RÀNG BUỘC
                                if (string.IsNullOrEmpty(maHD)) throw new Exception("Mã hóa đơn không được để trống.");
                                if (string.IsNullOrEmpty(maNV)) throw new Exception("Mã nhân viên không được để trống.");
                                if (string.IsNullOrEmpty(maKH)) throw new Exception("Mã khách hàng không được để trống.");
                                if (string.IsNullOrEmpty(ptThanhToan)) throw new Exception("Phương thức thanh toán không được để trống.");

                                // Chặn trùng mã hóa đơn
                                bool daTonTai = _context.HoaDon.Any(h => h.MaHD == maHD);
                                if (daTonTai)
                                {
                                    throw new Exception($"Hóa đơn '{maHD}' đã tồn tại trong hệ thống.");
                                }

                                // 3. Tìm Khóa ngoại từ hệ thống
                                if (!_context.NhanVien.Any(nv => nv.MaNV == maNV)) throw new Exception($"Mã NV '{maNV}' không tồn tại.");
                                if (!_context.KhachHang.Any(kh => kh.MaKH == maKH)) throw new Exception($"Mã KH '{maKH}' không tồn tại.");

                                // 4. Đọc Ngày và Số an toàn
                                string ngayLapStr = row.Cell(6).GetFormattedString().Trim();
                                DateTime ngayLap;
                                if (!DateTime.TryParseExact(ngayLapStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngayLap))
                                {
                                    if (!DateTime.TryParse(ngayLapStr, out ngayLap))
                                        throw new Exception("Ngày lập sai định dạng (vd: 20/10/2026).");
                                }

                                if (!decimal.TryParse(row.Cell(7).GetFormattedString().Trim(), out decimal tongTien))
                                    throw new Exception("Tổng tiền phải là định dạng số.");

                                decimal.TryParse(row.Cell(8).GetFormattedString().Trim(), out decimal giamGia);
                                int.TryParse(row.Cell(9).GetFormattedString().Trim(), out int diemDaDung);
                                int.TryParse(row.Cell(10).GetFormattedString().Trim(), out int diemCongThem);
                                string ghiChu = row.Cell(12).GetFormattedString().Trim();

                                // 5. Tạo đối tượng Hóa đơn và lưu
                                HoaDon hd = new HoaDon
                                {
                                    MaHD = maHD,
                                    MaNV = maNV,
                                    MaKH = maKH,
                                    NgayLap = ngayLap,
                                    TongTien = tongTien,
                                    GiamGia = giamGia,
                                    DiemDaDung = diemDaDung,
                                    DiemCongThem = diemCongThem,
                                    PT_ThanhToan = ptThanhToan,
                                    GhiChu = ghiChu
                                };

                                _context.HoaDon.Add(hd);
                                _context.SaveChanges();
                                thanhCongHD++;
                            }
                            catch (Exception ex)
                            {
                                dongLoiHD++;
                                // Lấy lỗi chi tiết từ InnerException nếu có
                                string msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                                chiTietLoi.AppendLine($"- [Sheet HoaDon] Dòng {row.RowNumber()}: {msg}");
                            }
                        }

                        // ============================================
                        // 2. NHẬP SHEET HOADON_CHITIET
                        // ============================================
                        var worksheetCT = workbook.Worksheet("HoaDon_ChiTiet");
                        var rowsCT = worksheetCT.RowsUsed().Skip(1);

                        foreach (var row in rowsCT)
                        {
                            _context = new QLCHMPDbContext();

                            try
                            {
                                string maHD = row.Cell(1).GetFormattedString().Trim();
                                string maSP = row.Cell(2).GetFormattedString().Trim();

                                if (string.IsNullOrEmpty(maHD)) throw new Exception("Mã hóa đơn không được để trống.");
                                if (string.IsNullOrEmpty(maSP)) throw new Exception("Mã sản phẩm không được để trống.");

                                // RÀNG BUỘC: Kiểm tra hóa đơn và sản phẩm có tồn tại thực sự không
                                if (!_context.HoaDon.Any(h => h.MaHD == maHD)) throw new Exception($"Hóa đơn '{maHD}' không tồn tại.");
                                if (!_context.SanPham.Any(sp => sp.MaSP == maSP)) throw new Exception($"Sản phẩm '{maSP}' không tồn tại.");

                                // Chặn trùng lặp dòng chi tiết
                                bool daCo = _context.ChiTietHD.Any(ct => ct.MaHD == maHD && ct.MaSP == maSP);
                                if (daCo)
                                {
                                    throw new Exception($"Sản phẩm '{maSP}' đã tồn tại trong hóa đơn '{maHD}'.");
                                }

                                // Đọc số lượng và giá an toàn
                                if (!int.TryParse(row.Cell(4).GetFormattedString().Trim(), out int soLuong) || soLuong <= 0)
                                    throw new Exception("Số lượng phải là số nguyên lớn hơn 0.");

                                if (!decimal.TryParse(row.Cell(5).GetFormattedString().Trim(), out decimal giaBan) || giaBan < 0)
                                    throw new Exception("Giá bán không hợp lệ.");

                                if (!decimal.TryParse(row.Cell(6).GetFormattedString().Trim(), out decimal thanhTien) || thanhTien < 0)
                                    throw new Exception("Thành tiền không hợp lệ.");

                                HoaDon_ChiTiet ct = new HoaDon_ChiTiet
                                {
                                    MaHD = maHD,
                                    MaSP = maSP,
                                    SoLuong = soLuong,
                                    GiaBan = giaBan,
                                    ThanhTien = thanhTien
                                };

                                _context.ChiTietHD.Add(ct);
                                _context.SaveChanges();
                                thanhCongCT++;
                            }
                            catch (Exception ex)
                            {
                                dongLoiCT++;
                                string msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                                chiTietLoi.AppendLine($"- [Sheet ChiTiet] Dòng {row.RowNumber()}: {msg}");
                            }
                        }
                    }

                    // 6. Thông báo tổng kết
                    int tongThanhCong = thanhCongHD + thanhCongCT;
                    int tongLoi = dongLoiHD + dongLoiCT;

                    string thongBao = $"Nhập hoàn tất!\n- Thành công: {tongThanhCong} dòng (gồm {thanhCongHD} hóa đơn, {thanhCongCT} chi tiết)\n- Thất bại: {tongLoi} dòng";
                    if (tongLoi > 0) thongBao += "\n\nChi tiết lỗi:\n" + chiTietLoi.ToString();

                    if (tongThanhCong > 0)
                    {
                        NhatKyHeThong.GhiLog(_maTKDangNhap, $"Nhập dữ liệu Hóa đơn từ Excel (Thành công: {thanhCongHD} hóa đơn, {thanhCongCT} chi tiết)");
                    }

                    MessageBox.Show(thongBao, "Kết quả Import", MessageBoxButtons.OK,
                                    tongLoi > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                    TaiDuLieuLenBang(); // Load lại lưới
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Xuất dữ liệu Hóa đơn ra Excel";
            saveFileDialog.Filter = "Tập tin Excel|*.xlsx";
            saveFileDialog.FileName = "BaoCao_HoaDon_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        // --- SHEET 1: HOADON ---
                        // Khai báo Tiếng Việt cho tiêu đề Excel đẹp mắt
                        var dsHD = _context.HoaDon.Select(h => new
                        {
                            MaHD = h.MaHD,
                            MaNV = h.MaNV,
                            TenNhanVien = h.NhanVien.HoTenNV,
                            MaKH = h.MaKH,
                            TenKhachHang = h.KhachHang.HoTenKH,
                            NgayLap = h.NgayLap,
                            TongTien = h.TongTien,
                            GiamGia = h.GiamGia,
                            DiemDaDung = h.DiemDaDung,
                            DiemCongThem = h.DiemCongThem,
                            PT_ThanhToan = h.PT_ThanhToan, // Xuất thẳng chuỗi
                            GhiChu = h.GhiChu
                        }).ToList();

                        var wsHD = wb.Worksheets.Add("HoaDon");
                        wsHD.Cell(1, 1).InsertTable(dsHD);

                        // --- SHEET 2: HOADON_CHITIET ---
                        var dsCT = _context.ChiTietHD.Select(ct => new
                        {
                            MaHD = ct.MaHD,
                            MaSP = ct.MaSP,
                            TenSanPham = ct.SanPham.TenSP,
                            SoLuong = ct.SoLuong,
                            GiaBan = ct.GiaBan,
                            ThanhTien = ct.ThanhTien
                        }).ToList();

                        var wsCT = wb.Worksheets.Add("HoaDon_ChiTiet");
                        wsCT.Cell(1, 1).InsertTable(dsCT);

                        // Format UI Excel
                        foreach (var ws in wb.Worksheets)
                        {
                            ws.Columns().AdjustToContents();
                            var header = ws.Row(1);
                            header.Style.Font.Bold = true;
                            header.Style.Fill.BackgroundColor = XLColor.FromHtml("#2980b9");
                            header.Style.Font.FontColor = XLColor.White;
                        }

                        wb.SaveAs(saveFileDialog.FileName);
                        NhatKyHeThong.GhiLog(_maTKDangNhap, "Xuất danh sách Hóa đơn ra tập tin Excel");
                    }
                    MessageBox.Show("Xuất báo cáo 2 sheet đầy đủ thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            // 1. Lấy giá trị ngày từ giao diện (Cắt bỏ phần giờ phút giây)
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date;

            // 2. Chặn lỗi logic của người dùng
            if (tuNgay > denNgay)
            {
                MessageBox.Show("Từ ngày không được lớn hơn Đến ngày!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _context = new QLCHMPDbContext();

            try
            {
                // 3. Lọc dữ liệu theo khoảng thời gian
                var dsLọc = _context.HoaDon
                    .Where(h => h.NgayLap.Date >= tuNgay && h.NgayLap.Date <= denNgay) // So sánh theo Ngày (Date)
                    .Select(h => new
                    {
                        MaHD = h.MaHD,
                        MaNV = h.MaNV,
                        MaKH = h.MaKH,
                        HoTenNV = h.NhanVien.HoTenNV,
                        HoTenKH = h.KhachHang.HoTenKH,
                        NgayLap = h.NgayLap,
                        GhiChu = h.GhiChu,
                        PT_ThanhToan = h.PT_ThanhToan,
                        GiamGia = h.GiamGia,
                        TongTien = h.TongTien,
                        XemChiTiet = "Xem chi tiết"
                    })
                    .OrderByDescending(h => h.NgayLap)
                    .ToList();

                // 4. Gắn dữ liệu vừa lọc lên DataGridView
                dgvDSHoaDon.DataSource = dsLọc;

                // Bật thông báo nhỏ nếu không tìm thấy
                if (dsLọc.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn nào trong khoảng thời gian này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lọc dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            //Lấy từ Trim()xóa dấu cách dư 
            string tuKhoa = txtTimKiem.Text.Trim();

            //RB: Nếu ô tìm kiếm để trống -> Hiển thị lại toàn bộ danh sách hóa đơn ban đầu
            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TaiDuLieuLenBang();
                return;
            }

            //Tìm cả Mã HĐ, Tên Nhân viên hoặc Tên Khách hàng
            try
            {
                using (var context = new QLCHMPDbContext())
                {
                    var ketQua = context.HoaDon
                        .Include(h => h.NhanVien)
                        .Include(h => h.KhachHang)
                        .Where(h => h.MaHD.Contains(tuKhoa) ||
                                    (h.NhanVien != null && h.NhanVien.HoTenNV.Contains(tuKhoa)) ||
                                    (h.KhachHang != null && h.KhachHang.HoTenKH.Contains(tuKhoa)))
                        .Select(h => new
                        {
                            MaHD = h.MaHD,
                            HoTenNV = h.NhanVien != null ? h.NhanVien.HoTenNV : "Không có",
                            HoTenKH = h.KhachHang != null ? h.KhachHang.HoTenKH : "Khách lẻ",
                            NgayLap = h.NgayLap,
                            TongTien = h.TongTien,
                            GiamGia = h.GiamGia,
                            PT_ThanhToan = h.PT_ThanhToan,
                            GhiChu = h.GhiChu,
                            MaNV = h.MaNV,
                            MaKH = h.MaKH
                        })
                        .ToList();

                    dgvDSHoaDon.DataSource = ketQua;

                    // RB: Báo lỗi nếu không tìm ra kết quả
                    if (ketQua.Count == 0)
                    {
                        MessageBox.Show($"Không tìm thấy hóa đơn nào khớp với từ khóa '{tuKhoa}'!", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Khôi phục lại bảng nguyên gốc và quét khối chữ cũ cho người ta dễ gõ lại
                        TaiDuLieuLenBang();
                        txtTimKiem.Clear();
                        txtTimKiem.Focus();
                        txtTimKiem.SelectAll();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tìm kiếm: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            dtpTuNgay.Value = DateTime.Now;
            dtpDenNgay.Value = DateTime.Now;

            TaiDuLieuLenBang();
            this.ActiveControl = null;
        }

        private void btnInHD_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_maHD))
            {
                MessageBox.Show("Vui lòng click chọn một hóa đơn từ danh sách để in!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (frmInHoaDon inHoaDon = new frmInHoaDon(_maHD))
            {
                inHoaDon.ShowDialog();
            }
        }
    }
}
