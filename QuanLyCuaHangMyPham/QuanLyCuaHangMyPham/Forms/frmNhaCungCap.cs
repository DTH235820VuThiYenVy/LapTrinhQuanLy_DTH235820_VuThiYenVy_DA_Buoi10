using ClosedXML.Excel;
using QuanLyCuaHangMyPham.Data;
using QuanLyCuaHangMyPham.TienIch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace QuanLyCuaHangMyPham.Forms
{
    public partial class frmNhaCungCap : Form
    {
        private QLCHMPDbContext _context = new QLCHMPDbContext();
        private CoGianGiaoDien _hoTroCoGianForm;
        private bool _xuLyThem = false;
        private string _maNCC = "";
        private int _maTKDangNhap;
        public frmNhaCungCap(int maTKDangNhap)
        {
            InitializeComponent();
            _hoTroCoGianForm = new CoGianGiaoDien(this);
            _maTKDangNhap = maTKDangNhap;
        }
        //làm mờ txtTimKiem
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lParam);
        private void BatTatChucNang(bool giaTri)
        {
            txtMaNCC.Enabled = false; // Luôn khóa mã tự sinh
            txtTenNCC.Enabled = giaTri;
            txtDiaChi.Enabled = giaTri;
            txtSDT.Enabled = giaTri;
            txtEmail.Enabled = giaTri;

            btnLuu.Enabled = giaTri;
            btnHuy.Enabled = giaTri;

            btnThem.Enabled = !giaTri;
            btnSua.Enabled = !giaTri;
            btnXoa.Enabled = !giaTri;
            btnTimKiem.Enabled = !giaTri;
            btnXuat.Enabled = !giaTri;
            btnNhap.Enabled = !giaTri;
        }

        private void TaiDuLieuLenBang()
        {
            _context = new QLCHMPDbContext();
            var ds = _context.NhaCungCap.ToList();

            dgvDSNhaCC.AutoGenerateColumns = false;
            dgvDSNhaCC.DataSource = ds;

            BatTatChucNang(false);

            bool coDuLieu = ds.Count > 0;
            btnXoa.Enabled = coDuLieu;
            btnSua.Enabled = coDuLieu;
            btnXuat.Enabled = coDuLieu;
        }
        private void frmNhaCungCap_Load(object sender, EventArgs e)
        {
            TaiDuLieuLenBang();
            SendMessage(txtTimKiem.Handle, 0x1501, 1, "Nhập tên nhà cung cấp để tìm kiếm");
        }


        private void dgvDSNhaCC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvDSNhaCC.Rows[e.RowIndex];
            txtMaNCC.Text = row.Cells["MaNCC"].Value?.ToString();
            txtTenNCC.Text = row.Cells["TenNCC"].Value?.ToString();
            txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString();
            txtEmail.Text = row.Cells["Email"].Value?.ToString();
            txtSDT.Text = row.Cells["SDT"].Value?.ToString();
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            _xuLyThem = true;
            BatTatChucNang(true);

            // Xóa trắng ô nhập
            txtTenNCC.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            txtEmail.Clear();

            // Sinh mã tự động
            var dsMa = _context.NhaCungCap.Select(n => n.MaNCC).ToList();
            txtMaNCC.Text = SinhMaTuDong.TuSinhMa(dsMa, "MaNCC", "NCC", 3);
            txtTenNCC.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvDSNhaCC.CurrentRow == null) return;
            _xuLyThem = false;
            BatTatChucNang(true);
            _maNCC = dgvDSNhaCC.CurrentRow.Cells["MaNCC"].Value.ToString();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSNhaCC.CurrentRow == null) return;

            _maNCC = dgvDSNhaCC.CurrentRow.Cells["MaNCC"].Value.ToString();

            // Chặn xóa nếu đã có phiếu nhập
            if (_context.PhieuNhap.Any(p => p.MaNCC == _maNCC))
            {
                MessageBox.Show("Nhà cung cấp này đã có lịch sử giao dịch, không thể xóa!", "Cảnh báo");
                return;
            }

            if (MessageBox.Show("Xác nhận xóa nhà cung cấp này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var ncc = _context.NhaCungCap.Find(_maNCC);
                if (ncc != null)
                {
                    string tenNCCDaXoa = ncc.TenNCC; // Lưu tên để ghi log
                    _context.NhaCungCap.Remove(ncc);
                    _context.SaveChanges();
                    NhatKyHeThong.GhiLog(_maTKDangNhap, $"Xóa nhà cung cấp: {tenNCCDaXoa} (Mã: {_maNCC})");
                    TaiDuLieuLenBang();
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Ràng buộc dữ liệu
            if (string.IsNullOrWhiteSpace(txtTenNCC.Text) || string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhà cung cấp và SĐT!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                try
                {
                    // Thử tạo một đối tượng MailAddress từ chuỗi nhập vào
                    var addr = new System.Net.Mail.MailAddress(txtEmail.Text.Trim());

                    // Kiểm tra xem chuỗi sau khi tạo có khớp hoàn toàn với chuỗi nhập không
                    if (addr.Address != txtEmail.Text.Trim())
                    {
                        throw new Exception(); // Ép văng lỗi nếu lọt khe định dạng
                    }
                }
                catch
                {
                    // Nếu văng lỗi thì báo liền
                    MessageBox.Show("Định dạng Email không hợp lệ!\n(Ví dụ đúng: nguyenvan_a@gmail.com)", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtEmail.Focus();
                    return; // Dừng lệnh lưu ngay lập tức
                }

                string sdt = txtSDT.Text.Trim();
                if (!Regex.IsMatch(sdt, @"^0[0-9]{9}$"))
                {
                    MessageBox.Show("SĐT không hợp lệ! (10 số, bắt đầu bằng 0)", "Lỗi");
                    return;
                }

                try
                {
                    string hanhDongLog = ""; // Dùng để ghi log
                    if (_xuLyThem)
                    {
                        if (_context.NhaCungCap.Any(n => n.SDT == sdt))
                        {
                            MessageBox.Show("Số điện thoại này đã tồn tại!", "Trùng dữ liệu");
                            return;
                        }

                        var ncc = new NhaCungCap
                        {
                            MaNCC = txtMaNCC.Text,
                            TenNCC = txtTenNCC.Text.Trim(),
                            DiaChi = txtDiaChi.Text.Trim(),
                            SDT = sdt,
                            Email = txtEmail.Text.Trim()
                        };
                        _context.NhaCungCap.Add(ncc);
                        hanhDongLog = $"Thêm mới nhà cung cấp: {ncc.TenNCC} (Mã: {ncc.MaNCC})";
                    }
                    else
                    {
                        var ncc = _context.NhaCungCap.Find(_maNCC);
                        if (ncc != null)
                        {
                            ncc.TenNCC = txtTenNCC.Text.Trim();
                            ncc.DiaChi = txtDiaChi.Text.Trim();
                            ncc.SDT = sdt;
                            ncc.Email = txtEmail.Text.Trim();
                            hanhDongLog = $"Cập nhật nhà cung cấp: {ncc.TenNCC} (Mã: {_maNCC})";
                        }
                    }
                    _context.SaveChanges();
                    if (!string.IsNullOrEmpty(hanhDongLog))
                    {
                        NhatKyHeThong.GhiLog(_maTKDangNhap, hanhDongLog);
                    }
                    MessageBox.Show("Đã lưu dữ liệu thành công!", "Thông báo");
                    TaiDuLieuLenBang();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hệ thống: " + ex.Message);
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            _xuLyThem = false;
            TaiDuLieuLenBang();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Workbook|*.xlsx";
            sfd.FileName = "DanhSachNhaCungCap_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Mã NCC");
                    dt.Columns.Add("Tên nhà cung cấp");
                    dt.Columns.Add("Địa chỉ");
                    dt.Columns.Add("Email");
                    dt.Columns.Add("Số điện thoại");

                    var list = _context.NhaCungCap.ToList();
                    foreach (var n in list)
                    {
                        dt.Rows.Add(n.MaNCC, n.TenNCC, n.DiaChi, n.Email, n.SDT);
                    }

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add(dt, "NhaCungCap");
                        ws.Columns().AdjustToContents();
                        wb.SaveAs(sfd.FileName);
                    }
                    NhatKyHeThong.GhiLog(_maTKDangNhap, "Xuất danh sách Nhà cung cấp ra tập tin Excel");
                    MessageBox.Show("Xuất Excel thành công!", "Thông báo");
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            }
        }

        private void btnNhap_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Nhập dữ liệu Nhà cung cấp từ Excel";
            openFileDialog.Filter = "Tập tin Excel|*.xlsx;*.xls";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                int thanhCong = 0;
                int dongLoi = 0;
                StringBuilder chiTietLoi = new StringBuilder();

                try
                {
                    using (XLWorkbook workbook = new XLWorkbook(openFileDialog.FileName))
                    {
                        IXLWorksheet worksheet = workbook.Worksheet(1);
                        var rows = worksheet.RowsUsed().Skip(1); // Bỏ qua dòng tiêu đề

                        foreach (var row in rows)
                        {
                            _context = new QLCHMPDbContext();

                            try
                            {
                                // 1. Đọc dữ liệu từ các cột
                                string ten = row.Cell(2).GetFormattedString().Trim();
                                string dc = row.Cell(3).GetFormattedString().Trim();
                                string em = row.Cell(4).GetFormattedString().Trim();
                                string sdt = row.Cell(5).GetFormattedString().Trim();

                                // 2. Kiểm tra ràng buộc
                                if (string.IsNullOrEmpty(ten) || string.IsNullOrEmpty(sdt))
                                    throw new Exception("Tên NCC hoặc SĐT bị trống.");

                                if (_context.NhaCungCap.Any(n => n.TenNCC == ten))
                                    throw new Exception($"Nhà cung cấp '{ten}' đã có trong hệ thống.");

                                // 3. Sinh mã mới
                                var dsMaHienTai = _context.NhaCungCap.Select(n => n.MaNCC).ToList();
                                string maMoi = SinhMaTuDong.TuSinhMa(dsMaHienTai, "MaNCC", "NCC", 3);

                                // 4. Tạo Nhà cung cấp mới
                                NhaCungCap ncc = new NhaCungCap
                                {
                                    MaNCC = maMoi,
                                    TenNCC = ten,
                                    DiaChi = dc,
                                    Email = em,
                                    SDT = sdt
                                };

                                _context.NhaCungCap.Add(ncc);
                                _context.SaveChanges();

                                thanhCong++;
                            }
                            catch (Exception ex)
                            {
                                dongLoi++;
                                // Bắt lỗi InnerException y như bên form Nhân viên
                                string errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                                chiTietLoi.AppendLine($"- Dòng {row.RowNumber()}: {errorMsg}");
                            }
                        }
                    }

                    // 5. Hiển thị thông báo tổng hợp
                    string thongBao = $"Nhập hoàn tất!\n- Thành công: {thanhCong} nhà cung cấp.\n- Lỗi: {dongLoi} dòng.";
                    if (dongLoi > 0) thongBao += "\n\nChi tiết:\n" + chiTietLoi.ToString();

                    if (thanhCong > 0)
                    {
                        NhatKyHeThong.GhiLog(_maTKDangNhap, $"Nhập dữ liệu Nhà cung cấp từ Excel (Thành công: {thanhCong} dòng)");
                    }

                    MessageBox.Show(thongBao, "Kết quả Import", MessageBoxButtons.OK,
                                    dongLoi > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                    TaiDuLieuLenBang();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hệ thống không thể đọc file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDuLieuLenBang();
                return;
            }

            _context = new QLCHMPDbContext();

            // Tìm kiếm (mã, tên, số điện thoại, email)
            var ketQua = _context.NhaCungCap
                .Where(ncc => ncc.MaNCC.ToLower().Contains(tuKhoa) ||
                              ncc.TenNCC.ToLower().Contains(tuKhoa) ||
                              ncc.SDT.Contains(tuKhoa) ||
                              (ncc.Email != null && ncc.Email.ToLower().Contains(tuKhoa)))
                .ToList();

            dgvDSNhaCC.DataSource = ketQua;

            // Báo lỗi nếu không tìm thấy
            if (ketQua.Count == 0)
            {
                MessageBox.Show("Không tìm thấy dữ liệu phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDuLieuLenBang();

                // Reset lại ô tìm kiếm để gõ chữ mới
                txtTimKiem.Text = "";
                txtTimKiem.Focus();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();

            TaiDuLieuLenBang();

            this.ActiveControl = null;
        }
    }
}

