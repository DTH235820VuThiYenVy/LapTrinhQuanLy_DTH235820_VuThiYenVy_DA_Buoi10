using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using QuanLyCuaHangMyPham.Data;
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
using ClosedXML.Excel;

namespace QuanLyCuaHangMyPham.Forms
{
    public partial class frmKhachHang : Form
    {
        private QLCHMPDbContext _context = new QLCHMPDbContext();
        private bool _xuLyThem = false;
        private string _maKH = "";
        private int _maTKDangNhap;
        public bool MoDeThemMoi = false;

        private CoGianGiaoDien _hoTroCoGianForm;
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lParam);
        private const int EM_SETCUEBANNER = 0x1501;
        public frmKhachHang(int maTKDangNhap)
        {
            InitializeComponent();
            _hoTroCoGianForm = new CoGianGiaoDien(this);
            _maTKDangNhap = maTKDangNhap;

        }

        private void BatTatChucNang(bool giaTri)
        {
            txtHoTenKH.Enabled = giaTri;
            txtSDT.Enabled = giaTri;
            txtDiaChi.Enabled = giaTri;
            dtpNgaySinh.Enabled = giaTri;
            txtDiemTichLuy.Enabled = giaTri;

            txtMaKH.Enabled = false; 

            btnLuu.Enabled = giaTri;
            btnHuy.Enabled = giaTri;
            btnThem.Enabled = !giaTri;
            btnSua.Enabled = !giaTri;
            btnXoa.Enabled = !giaTri;
        }

        private void TaiDuLieuLenBang()
        {
            List<KhachHang> kh = _context.KhachHang.ToList();
            //var ds = _context.KhachHang.ToList();
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = kh;

            // Xóa sạch Binding cũ
            txtMaKH.DataBindings.Clear();
            txtHoTenKH.DataBindings.Clear();
            txtSDT.DataBindings.Clear();
            txtDiaChi.DataBindings.Clear();
            txtDiemTichLuy.DataBindings.Clear();
            dtpNgaySinh.DataBindings.Clear();

            // Thiết lập Binding mới (Không cập nhật tức thì để tránh lỗi dữ liệu)
            txtMaKH.DataBindings.Add("Text", bindingSource, "MaKH", false, DataSourceUpdateMode.Never);
            txtHoTenKH.DataBindings.Add("Text", bindingSource, "HoTenKH", false, DataSourceUpdateMode.Never);
            txtSDT.DataBindings.Add("Text", bindingSource, "SDT", false, DataSourceUpdateMode.Never);
            txtDiaChi.DataBindings.Add("Text", bindingSource, "DiaChi", false, DataSourceUpdateMode.Never);
            txtDiemTichLuy.DataBindings.Add("Text", bindingSource, "DiemTichLuy", false, DataSourceUpdateMode.Never);

            dtpNgaySinh.DataBindings.Add("Value", bindingSource, "NgaySinh", true, DataSourceUpdateMode.Never);
            dgvDSKhachHang.AutoGenerateColumns = false;
            dgvDSKhachHang.DataSource = bindingSource;
            BatTatChucNang(false);

            // Nếu danh sách rỗng (count == 0) thì khóa nút Xóa và Sửa
            bool coDuLieu = kh.Count > 0;
            btnXoa.Enabled = coDuLieu;
            btnSua.Enabled = coDuLieu;
            btnXuat.Enabled = coDuLieu;

            // --- BỘ LỆNH ÉP MÀU DỨT ĐIỂM CHO DATAGRIDVIEW ---
            dgvDSKhachHang.ForeColor = Color.Black;

            dgvDSKhachHang.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgvDSKhachHang.RowsDefaultCellStyle.BackColor = Color.White;
            dgvDSKhachHang.RowsDefaultCellStyle.SelectionForeColor = Color.Black;     // Ép chữ đen khi click
            dgvDSKhachHang.RowsDefaultCellStyle.SelectionBackColor = Color.LightSkyBlue; // Nền xanh nhạt khi click

            dgvDSKhachHang.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvDSKhachHang.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dgvDSKhachHang.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;     // Ép chữ đen khi click
            dgvDSKhachHang.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.LightSkyBlue; // Nền xanh nhạt khi click

            dgvDSKhachHang.DefaultCellStyle.ForeColor = Color.Black;
            dgvDSKhachHang.DefaultCellStyle.BackColor = Color.White;
            dgvDSKhachHang.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvDSKhachHang.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            // ------------------------------------------------
        }
        private void frmKhachHang_Load(object sender, EventArgs e)
        {
            TaiDuLieuLenBang();

            SendMessage(txtTimKiem.Handle, EM_SETCUEBANNER, 1, "Nhập mã KH, tên hoặc SĐT...");

            this.ActiveControl = null;
            dgvDSKhachHang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDSKhachHang.ClearSelection();
            if (MoDeThemMoi == true)
            {
                btnThem_Click(null, null); // Tự động gọi nút Thêm sau khi đã tải xong bảng
            }

        }



        private void btnThem_Click(object sender, EventArgs e)
        {
            _xuLyThem = true;
            BatTatChucNang(true);

            // Xóa trắng để nhập mới
            txtHoTenKH.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();
            txtDiemTichLuy.Text = "0"; // Mặc định khách mới có 0 điểm
            dtpNgaySinh.Value = DateTime.Now;

            // Sinh mã tự động KH001
            var dsMa = _context.KhachHang.Select(k => k.MaKH).ToList();
            txtMaKH.Text = SinhMaTuDong.TuSinhMa(dsMa, "MaKH", "KH", 3);

            txtHoTenKH.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvDSKhachHang.CurrentRow == null) return;
            _xuLyThem = false;
            BatTatChucNang(true);
            _maKH = dgvDSKhachHang.CurrentRow.Cells["MaKH"].Value.ToString();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            bool daCoHoaDon = _context.HoaDon.Any(h => h.MaKH == _maKH);

            if (daCoHoaDon)
            {
                MessageBox.Show("Khách hàng này đã có lịch sử mua hàng. Không thể xóa để tránh mất dữ liệu doanh thu và sai lệch tồn kho!",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng lại, không chạy lệnh Remove phía dưới
            }
            if (dgvDSKhachHang.CurrentRow == null) return;

            if (MessageBox.Show("Xác nhận xóa khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _maKH = dgvDSKhachHang.CurrentRow.Cells["MaKH"].Value.ToString();
                var kh = _context.KhachHang.Find(_maKH);
                if (kh != null)
                {
                    _context.KhachHang.Remove(kh);
                    _context.SaveChanges();

                    NhatKyHeThong.GhiLog(_maTKDangNhap, $"Xóa khách hàng: {kh.HoTenKH} (Mã: {kh.MaKH})");

                    txtMaKH.Clear();
                    txtHoTenKH.Clear();
                    txtSDT.Clear();
                    txtDiaChi.Clear();
                    txtDiemTichLuy.Clear();
                    dtpNgaySinh.Value = DateTime.Now; // Đưa ngày về hiện tại

                    TaiDuLieuLenBang();
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

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTenKH.Text) || string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập tên và số điện thoại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string sdt = txtSDT.Text.Trim();
            if (sdt.Length != 10 || !sdt.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Vui lòng nhập đủ 10 chữ số.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSDT.Focus();
                return;
            }

            // Năm sinh không được lớn hơn hoặc bằng năm hiện tại
            if (dtpNgaySinh.Value.Year >= DateTime.Now.Year)
            {
                MessageBox.Show("Năm sinh không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtpNgaySinh.Focus();
                return;
            }

            string sdtMoi = txtSDT.Text.Trim();
            // Số điện thoại không được trùng
            // Nếu thêm mới: check toàn bảng. Nếu sửa: check toàn bảng trừ chính nó
            bool trungSDT = _context.KhachHang.Any(kh => kh.SDT == sdtMoi && (_xuLyThem || kh.MaKH != _maKH));
            if (trungSDT)
            {
                MessageBox.Show("Số điện thoại này đã thuộc về khách hàng khác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSDT.Focus();
                return;
            }

            try
            {
                string hanhDongLog = ""; // Dùng để ghi log
                if (_xuLyThem)
                {
                    KhachHang kh = new KhachHang
                    {
                        MaKH = txtMaKH.Text,
                        HoTenKH = txtHoTenKH.Text,
                        SDT = txtSDT.Text,
                        DiaChi = txtDiaChi.Text,
                        DiemTichLuy = int.Parse(txtDiemTichLuy.Text),
                        NgaySinh = dtpNgaySinh.Value
                    };
                    _context.KhachHang.Add(kh);
                    hanhDongLog = $"Thêm mới khách hàng: {kh.HoTenKH} (Mã: {kh.MaKH})";
                }
                else
                {
                    var kh = _context.KhachHang.Find(_maKH);
                    if (kh != null)
                    {
                        kh.HoTenKH = txtHoTenKH.Text;
                        kh.SDT = txtSDT.Text;
                        kh.DiaChi = txtDiaChi.Text;
                        kh.DiemTichLuy = int.Parse(txtDiemTichLuy.Text);
                        kh.NgaySinh = dtpNgaySinh.Value;

                        hanhDongLog = $"Cập nhật khách hàng: {kh.HoTenKH} (Mã: {_maKH})";
                    }
                }
                _context.SaveChanges();

                if (!string.IsNullOrEmpty(hanhDongLog))
                {
                    NhatKyHeThong.GhiLog(_maTKDangNhap, hanhDongLog);
                }

                TaiDuLieuLenBang();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Xuất danh sách Khách hàng ra Excel";
            saveFileDialog.Filter = "Tập tin Excel|*.xlsx";
            // Đặt tên file theo định dạng dd_MM_yyyy 
            saveFileDialog.FileName = "KhachHang_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("Mã KH", typeof(string));
                    table.Columns.Add("Họ tên khách hàng", typeof(string));
                    table.Columns.Add("Số điện thoại", typeof(string));
                    table.Columns.Add("Địa chỉ", typeof(string));
                    table.Columns.Add("Ngày sinh", typeof(string));
                    table.Columns.Add("Điểm tích lũy", typeof(int));

                    var danhSach = _context.KhachHang.ToList();
                    if (danhSach.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo");
                        return;
                    }

                    foreach (var item in danhSach)
                    {
                        table.Rows.Add(
                            item.MaKH,
                            item.HoTenKH,
                            item.SDT,
                            item.DiaChi,
                            //item.NgaySinh?.ToString("dd/MM/yyyy"),
                            item.NgaySinh.HasValue ? item.NgaySinh.Value.ToString("dd/MM/yyyy") : "Chưa cập nhật",
                            item.DiemTichLuy
                        );
                    }

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var sheet = wb.Worksheets.Add(table, "KhachHang");
                        sheet.Columns().AdjustToContents(); // Tự căn chỉnh cột
                        wb.SaveAs(saveFileDialog.FileName);
                    }

                    NhatKyHeThong.GhiLog(_maTKDangNhap, "Xuất danh sách Khách hàng ra tập tin Excel");
                    
                    MessageBox.Show("Xuất dữ liệu khách hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi hệ thống");
                }
            }
        }

        private void btnNhap_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Nhập dữ liệu Khách hàng từ Excel";
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
                        var rows = worksheet.RowsUsed().Skip(1); // Bỏ qua tiêu đề [cite: 36, 39]

                        foreach (var row in rows)
                        {
                            try
                            {
                                // Giả sử Excel: Cột A: Họ tên, Cột B: SĐT, Cột C: Địa chỉ, Cột D: Ngày sinh
                                string hoTen = row.Cell(2).Value.ToString().Trim();
                                string sdt = row.Cell(3).Value.ToString().Trim();
                                string diaChi = row.Cell(4).Value.ToString().Trim();
                                string ngaySinhStr = row.Cell(5).Value.ToString().Trim();

                                string diemStr = row.Cell(6).Value.ToString().Trim();
                                int diemTichLuy = 0;
                                int.TryParse(diemStr, out diemTichLuy); // Ép kiểu sang số, nếu lỗi thì để là 0


                                if (string.IsNullOrEmpty(hoTen) || string.IsNullOrEmpty(sdt))
                                    throw new Exception("Họ tên và SĐT không được để trống.");

                                // Kiểm tra định dạng SĐT (10 số)
                                if (sdt.Length != 10 || !sdt.All(char.IsDigit))
                                    throw new Exception($"SĐT '{sdt}' phải là 10 chữ số.");

                                //Kiểm tra trùng SĐT trong CSDL
                                if (_context.KhachHang.Any(k => k.SDT == sdt))
                                    throw new Exception($"SĐT '{sdt}' đã tồn tại trên hệ thống.");

                                // ngày sinh theo định dạng dd/MM/yyyy
                                DateTime ngaySinh;
                                if (!DateTime.TryParseExact(ngaySinhStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngaySinh))
                                {
                                    // Nếu sai định dạng thì mặc định hoặc báo lỗi
                                    ngaySinh = DateTime.Now.AddYears(-20);
                                }

                                //sinh mã khách hàng mới (KH001, KH002...)
                                var dsMa = _context.KhachHang.Select(k => k.MaKH).ToList();
                                string maMoi = SinhMaTuDong.TuSinhMa(dsMa, "MaKH", "KH", 3);

                                KhachHang khNew = new KhachHang
                                {
                                    MaKH = maMoi,
                                    HoTenKH = hoTen,
                                    SDT = sdt,
                                    DiaChi = diaChi,
                                    NgaySinh = ngaySinh,
                                    DiemTichLuy = diemTichLuy, // Khách mới nhập mặc định 0 điểm

                                };

                                _context.KhachHang.Add(khNew);
                                _context.SaveChanges(); // Lưu từng dòng để tránh trùng mã tự sinh 
                                thanhCong++;
                            }
                            catch (Exception ex)
                            {
                                dongLoi++;
                                chiTietLoi.AppendLine($"- Dòng {row.RowNumber()}: {ex.Message}");
                            }
                        }
                    }

                    string thongBao = $"Nhập hoàn tất!\n- Thành công: {thanhCong} dòng.\n- Lỗi: {dongLoi} dòng.";
                    if (dongLoi > 0) thongBao += "\n\nChi tiết các lỗi:\n" + chiTietLoi.ToString();

                    if (thanhCong > 0)
                    {
                        NhatKyHeThong.GhiLog(_maTKDangNhap, $"Nhập dữ liệu Khách hàng từ Excel (Thành công: {thanhCong} dòng)");
                    }

                    MessageBox.Show(thongBao, "Kết quả Import", MessageBoxButtons.OK,
                                    dongLoi > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                    TaiDuLieuLenBang();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim();

            // Nếu để trống thì load lại toàn bộ bảng
            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDuLieuLenBang();
                return;
            }

            try
            {
                _context = new QLCHMPDbContext(); 

                // Tìm kiếm  Mã KH, Tên KH, Số điện thoại
                var ketQua = _context.KhachHang
                    .Where(k => k.MaKH.Contains(tuKhoa) ||
                                k.HoTenKH.Contains(tuKhoa) ||
                                k.SDT.Contains(tuKhoa))
                    .ToList();

                if (ketQua.Count == 0)
                {
                    MessageBox.Show($"Không tìm thấy khách hàng nào khớp với từ khóa '{tuKhoa}'!", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDuLieuLenBang();
                    txtTimKiem.Clear();
                    txtTimKiem.Focus();
                    txtTimKiem.SelectAll();
                }
                else
                {
                    // TẠO LẠI BINDING ĐỂ FORM KHÔNG BỊ LỖI KHI CLICK CHỌN
                    BindingSource bindingSource = new BindingSource();
                    bindingSource.DataSource = ketQua;

                    txtMaKH.DataBindings.Clear();
                    txtHoTenKH.DataBindings.Clear();
                    txtSDT.DataBindings.Clear();
                    txtDiaChi.DataBindings.Clear();
                    txtDiemTichLuy.DataBindings.Clear();
                    dtpNgaySinh.DataBindings.Clear();

                    txtMaKH.DataBindings.Add("Text", bindingSource, "MaKH", false, DataSourceUpdateMode.Never);
                    txtHoTenKH.DataBindings.Add("Text", bindingSource, "HoTenKH", false, DataSourceUpdateMode.Never);
                    txtSDT.DataBindings.Add("Text", bindingSource, "SDT", false, DataSourceUpdateMode.Never);
                    txtDiaChi.DataBindings.Add("Text", bindingSource, "DiaChi", false, DataSourceUpdateMode.Never);
                    txtDiemTichLuy.DataBindings.Add("Text", bindingSource, "DiemTichLuy", false, DataSourceUpdateMode.Never);
                    dtpNgaySinh.DataBindings.Add("Value", bindingSource, "NgaySinh", true, DataSourceUpdateMode.Never);

                    // Cập nhật lên Bảng
                    dgvDSKhachHang.DataSource = bindingSource;
                    BatTatChucNang(false); // Khóa các ô nhập liệu lại
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tìm kiếm: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
