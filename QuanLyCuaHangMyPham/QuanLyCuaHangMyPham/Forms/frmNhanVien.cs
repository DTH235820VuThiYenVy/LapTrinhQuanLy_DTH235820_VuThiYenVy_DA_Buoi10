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
using BCrypt.Net;

namespace QuanLyCuaHangMyPham.Forms
{
    public partial class frmNhanVien : Form
    {

        private QLCHMPDbContext _context = new QLCHMPDbContext();
        private bool _xuLyThem = false;
        private string _maNV = "";
        private CoGianGiaoDien _hoTroCoGianForm;
        private int _maTKDangNhap;

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lParam);
        private const int EM_SETCUEBANNER = 0x1501;
        public frmNhanVien(int maTKDangNhap)
        {
            InitializeComponent();
            _hoTroCoGianForm = new CoGianGiaoDien(this);
            _maTKDangNhap = maTKDangNhap;
        }

        public void LayChucVuVaoComboBox()
        {
            // Lấy danh sách các quyền hạn đang có trong bảng Tài khoản
            var dsChucVu = _context.TaiKhoan
                                  .Select(tk => tk.QuyenHan)
                                  .Where(q => q != null && q != "")
                                  .Distinct() // Quan trọng: Để không bị trùng lặp chức vụ
                                  .ToList();

            // bảng TaiKhoan đang trống thì nạp vô
            if (dsChucVu.Count == 0)
            {
                dsChucVu = new List<string> { "Chủ cửa hàng", "Quản lý", "Nhân viên bán hàng", "Nhân viên kho" };
            }

            cboChucVu.DataSource = dsChucVu;
            cboChucVu.SelectedIndex = -1; // Mặc định không chọn cái nào
        }

        public void LayLuongCoBanVaoComboBox()
        {
            // Lấy danh sách lương cơ bản từ bảng Nhân Viên, bỏ trùng lặp
            var dsLuong = _context.NhanVien
                                  .Select(nv => nv.LuongCoBan)
                                  .Where(l => l != null)
                                  .Distinct()
                                  .ToList();
            cboLuongCoBan.DataSource = dsLuong;
        }
        private void BatTatChucNang(bool giaTri)
        {
            txtHoTenNV.Enabled = giaTri;
            txtSDT.Enabled = giaTri;
            txtDiaChi.Enabled = giaTri;
            txtEmail.Enabled = giaTri;
            cboChucVu.Enabled = giaTri;
            cboLuongCoBan.Enabled = giaTri;
            dtpNgaySinh.Enabled = giaTri;
            dtpNgayVao.Enabled = giaTri;

            txtMaNV.Enabled = false; // Mã tự sinh luôn khóa

            btnLuu.Enabled = giaTri;
            btnHuy.Enabled = giaTri;
            btnThem.Enabled = !giaTri;
            btnSua.Enabled = !giaTri;
            btnXoa.Enabled = !giaTri;
        }

        private void TaiDuLieuLenBang()
        {
            _context = new QLCHMPDbContext(); // Làm mới kết nối
            var nv = _context.NhanVien.ToList();
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = nv;

            dgvDSNhanVien.AutoGenerateColumns = false; // Chặn Grid tự ý thêm cột
            dgvDSNhanVien.DataSource = bindingSource;

            // Xóa Binding cũ
            txtMaNV.DataBindings.Clear();
            txtHoTenNV.DataBindings.Clear();
            txtSDT.DataBindings.Clear();
            txtDiaChi.DataBindings.Clear();
            txtEmail.DataBindings.Clear();
            cboChucVu.DataBindings.Clear();
            cboLuongCoBan.DataBindings.Clear();
            dtpNgaySinh.DataBindings.Clear();
            dtpNgayVao.DataBindings.Clear();

            // Binding mới
            txtMaNV.DataBindings.Add("Text", bindingSource, "MaNV", false, DataSourceUpdateMode.Never);
            txtHoTenNV.DataBindings.Add("Text", bindingSource, "HoTenNV", false, DataSourceUpdateMode.Never);
            txtSDT.DataBindings.Add("Text", bindingSource, "SDT", false, DataSourceUpdateMode.Never);
            txtDiaChi.DataBindings.Add("Text", bindingSource, "DiaChi", false, DataSourceUpdateMode.Never);
            txtEmail.DataBindings.Add("Text", bindingSource, "Email", false, DataSourceUpdateMode.Never);
            cboChucVu.DataBindings.Add("Text", bindingSource, "ChucVu", false, DataSourceUpdateMode.Never);
            cboLuongCoBan.DataBindings.Add("Text", bindingSource, "LuongCoBan", false, DataSourceUpdateMode.Never);

            dtpNgaySinh.DataBindings.Add("Value", bindingSource, "NgaySinh", true, DataSourceUpdateMode.Never);
            dtpNgayVao.DataBindings.Add("Value", bindingSource, "NgayVao", true, DataSourceUpdateMode.Never);

            dgvDSNhanVien.DataSource = bindingSource;

            dgvDSNhanVien.Columns["NgaySinh"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvDSNhanVien.Columns["NgayVao"].DefaultCellStyle.Format = "dd/MM/yyyy";

            BatTatChucNang(false);
            bool coDuLieu = nv.Count > 0;
            btnXoa.Enabled = coDuLieu;
            btnSua.Enabled = coDuLieu;
            btnXuat.Enabled = coDuLieu;
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {

            TaiDuLieuLenBang();
            LayChucVuVaoComboBox();
            LayLuongCoBanVaoComboBox();

            SendMessage(txtTimKiem.Handle, EM_SETCUEBANNER, 1, "Nhập mã NV, tên, SĐT hoặc chức vụ...");

            this.ActiveControl = null;

        }


        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số và phím xóa (backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cboChucVu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboChucVu.SelectedIndex != -1 && btnLuu.Enabled == true)
            {
                string cv = cboChucVu.Text;
                cboLuongCoBan.Enabled = true;

                switch (cv)
                {
                    case "Chủ cửa hàng":
                        cboLuongCoBan.Text = "0";
                        cboLuongCoBan.Enabled = false; // Sếp không cần lương
                        break;
                    case "Quản lý":
                        cboLuongCoBan.Text = "500000";
                        break;
                    case "Nhân viên bán hàng":
                        cboLuongCoBan.Text = "300000";
                        break;
                    case "Nhân viên kho":
                        cboLuongCoBan.Text = "250000";
                        break;
                    default:
                        cboLuongCoBan.Text = "200000"; // Mức lương mặc định cho các quyền khác
                        break;
                }
            }
        }


        private void txtSDT_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            _xuLyThem = true;
            BatTatChucNang(true);

            // Xóa trắng
            txtHoTenNV.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();
            txtEmail.Clear();
            cboChucVu.SelectedIndex = -1;
            cboLuongCoBan.SelectedIndex = -1;
            dtpNgaySinh.Value = new DateTime(2000, 1, 1);
            dtpNgayVao.Value = DateTime.Now;

            // Sinh mã tự động NV001
            var dsMa = _context.NhanVien.Select(n => n.MaNV).ToList();
            txtMaNV.Text = SinhMaTuDong.TuSinhMa(dsMa, "MaNV", "NV", 3);
            txtHoTenNV.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvDSNhanVien.CurrentRow == null) return;
            _xuLyThem = false;
            BatTatChucNang(true);
            _maNV = dgvDSNhanVien.CurrentRow.Cells["MaNV"].Value.ToString();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSNhanVien.CurrentRow == null) return;
            if (MessageBox.Show("Xác nhận xóa nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _maNV = dgvDSNhanVien.CurrentRow.Cells["MaNV"].Value.ToString();
                var nv = _context.NhanVien.Find(_maNV);
                if (nv != null)
                {
                    string tenNVDaXoa = nv.HoTenNV;
                    _context.NhanVien.Remove(nv);
                    _context.SaveChanges();
                    NhatKyHeThong.GhiLog(_maTKDangNhap, $"Xóa nhân viên: {tenNVDaXoa} (Mã: {_maNV})");

                    // Clear form
                    txtMaNV.Clear();
                    txtHoTenNV.Clear();
                    TaiDuLieuLenBang();
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Kiểm tra ràng buộc 
            if (string.IsNullOrWhiteSpace(txtHoTenNV.Text) || string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập tên và SĐT nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra năm sinh
            if (dtpNgaySinh.Value.Year >= DateTime.Now.Year - 15) // Ít nhất phải 15 tuổi
            {
                MessageBox.Show("Ngày sinh không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string sdt = txtSDT.Text.Trim();
            // Kiểm tra: Phải là số, bắt đầu bằng số 0, đủ 10 chữ số
            if (!System.Text.RegularExpressions.Regex.IsMatch(sdt, @"^0[0-9]{9}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! (Phải có 10 chữ số và bắt đầu bằng số 0)", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSDT.Focus();
                return;
            }

            //EMAIL (Nếu người dùng có nhập thì mới kiểm tra
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(txtEmail.Text);
                    if (addr.Address != txtEmail.Text.Trim()) throw new Exception();
                }
                catch
                {
                    MessageBox.Show("Định dạng Email không hợp lệ! (Ví dụ: abc@gmail.com)", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtEmail.Focus();
                    return;
                }
            }

            // Kiểm tra chức vụ
            if (cboChucVu.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn chức vụ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int tuoi = DateTime.Now.Year - dtpNgaySinh.Value.Year;
            if (tuoi < 18)
            {
                MessageBox.Show("Nhân viên phải từ 18 tuổi trở lên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string hanhDongLog = "";
                if (_xuLyThem)
                {
                    // Kiểm tra trùng SĐT 
                    if (_context.NhanVien.Any(n => n.SDT == sdt))
                    {
                        MessageBox.Show("Số điện thoại này đã tồn tại trong hệ thống!", "Trùng dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Khởi tạo đối tượng Nhân viên
                    NhanVien nv = new NhanVien
                    {
                        MaNV = txtMaNV.Text,
                        HoTenNV = txtHoTenNV.Text.Trim(),
                        SDT = sdt,
                        DiaChi = txtDiaChi.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        ChucVu = cboChucVu.Text,
                        LuongCoBan = decimal.Parse(cboLuongCoBan.Text),
                        NgaySinh = dtpNgaySinh.Value,
                        NgayVao = dtpNgayVao.Value,
                        GioiTinh = "Nữ"
                    };
                    _context.NhanVien.Add(nv);

                    // Lưu nhân viên trước để xác lập MaNV trong hệ thống
                    _context.SaveChanges();
                    hanhDongLog = $"Thêm mới nhân viên: {nv.HoTenNV} (Mã: {nv.MaNV})";
                    // Sau khi NV đã lưu xong -> tạo Tài khoản
                    string matKhauMacDinh = "123";
                    string matKhauMaHoa = BCrypt.Net.BCrypt.HashPassword(matKhauMacDinh);
                    TaiKhoan tk = new TaiKhoan
                    {
                        TenDangNhap = nv.MaNV,
                        MatKhau = matKhauMaHoa,
                        QuyenHan = nv.ChucVu,
                        TrangThai = true,
                        MaNV = nv.MaNV

                    };
                    _context.TaiKhoan.Add(tk);

                    // Lưu lần 2 cho bảng Tài khoản
                    _context.SaveChanges();
                }
                else
                {
                    // Chế độ Sửa (Chỉ cần Save 1 lần vì MaNV đã tồn tại sẵn)
                    var nv = _context.NhanVien.Find(_maNV);
                    if (nv != null)
                    {
                        nv.HoTenNV = txtHoTenNV.Text.Trim();
                        nv.SDT = sdt;
                        nv.DiaChi = txtDiaChi.Text.Trim();
                        nv.Email = txtEmail.Text.Trim();
                        nv.ChucVu = cboChucVu.Text;
                        nv.LuongCoBan = decimal.Parse(cboLuongCoBan.Text);
                        nv.NgaySinh = dtpNgaySinh.Value;
                        nv.NgayVao = dtpNgayVao.Value;

                        _context.SaveChanges();
                        hanhDongLog = $"Cập nhật thông tin nhân viên: {nv.HoTenNV} (Mã: {_maNV})";
                    }
                }

                if (!string.IsNullOrEmpty(hanhDongLog))
                {
                    NhatKyHeThong.GhiLog(_maTKDangNhap, hanhDongLog);
                }

                TaiDuLieuLenBang();
                MessageBox.Show("Đã lưu dữ liệu và khởi tạo tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show("Lỗi hệ thống: " + msg, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnNhap_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Tập tin Excel|*.xlsx;*.xls" };
            openFileDialog.Title = "Nhập dữ liệu Nhân viên từ Excel";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                int thanhCong = 0;
                int dongLoi = 0;
                StringBuilder chiTietLoi = new StringBuilder();

                try
                {
                    using (XLWorkbook workbook = new XLWorkbook(openFileDialog.FileName))
                    {
                        var worksheet = workbook.Worksheet(1);
                        var rows = worksheet.RowsUsed().Skip(1); // Bỏ qua tiêu đề

                        foreach (var row in rows)
                        {
                            // 1. LÀM MỚI CONTEXT mỗi dòng để tránh lỗi Tracked và cập nhật dữ liệu mới nhất để check trùng
                            _context = new QLCHMPDbContext();

                            try
                            {
                                // Đọc dữ liệu từ các cột
                                string hoTen = row.Cell(2).GetFormattedString().Trim();
                                string sdt = row.Cell(3).GetFormattedString().Trim();
                                string email = row.Cell(4).GetFormattedString().Trim();
                                string diaChi = row.Cell(5).GetFormattedString().Trim();
                                string chucVu = row.Cell(6).GetFormattedString().Trim();

                                // 2. KIỂM TRA RÀNG BUỘC & CHẶN TRÙNG
                                if (string.IsNullOrEmpty(hoTen)) throw new Exception("Họ tên không được để trống.");
                                if (string.IsNullOrEmpty(sdt)) throw new Exception("Số điện thoại không được để trống.");

                                // Chặn trùng SĐT
                                bool daTonTai = _context.NhanVien.Any(n => n.SDT == sdt);
                                if (daTonTai)
                                {
                                    throw new Exception($"Số điện thoại '{sdt}' đã tồn tại trong hệ thống.");
                                }

                                // Đọc Lương (Xử lý an toàn)
                                decimal luong = 0;
                                var cellLuong = row.Cell(7);
                                if (!cellLuong.IsEmpty())
                                {
                                    if (cellLuong.DataType == XLDataType.Number)
                                        luong = (decimal)cellLuong.GetDouble();
                                    else if (!decimal.TryParse(cellLuong.Value.ToString(), out luong))
                                        throw new Exception("Lương cơ bản phải là định dạng số.");
                                }

                                // Đọc Ngày sinh (Xử lý an toàn)
                                string ngaySinhStr = row.Cell(8).GetFormattedString().Trim();
                                DateTime ngaySinh;
                                if (!DateTime.TryParseExact(ngaySinhStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngaySinh))
                                {
                                    if (!DateTime.TryParse(ngaySinhStr, out ngaySinh))
                                        throw new Exception("Ngày sinh bị trống hoặc sai định dạng (vd: 20/10/2000).");
                                }

                                // 3. Sinh mã mới
                                var dsMaHienTai = _context.NhanVien.Select(n => n.MaNV).ToList();
                                string maMoi = SinhMaTuDong.TuSinhMa(dsMaHienTai, "MaNV", "NV", 3);

                                // 4. Tạo đối tượng Nhân viên
                                NhanVien nv = new NhanVien
                                {
                                    MaNV = maMoi,
                                    HoTenNV = hoTen,
                                    SDT = sdt,
                                    Email = email,
                                    DiaChi = diaChi,
                                    ChucVu = chucVu,
                                    LuongCoBan = luong,
                                    NgaySinh = ngaySinh,
                                    NgayVao = DateTime.Now,
                                    GioiTinh = "Nữ"
                                };

                                // 5. Tạo Tài khoản đính kèm
                                string matKhauMacDinh = "123";
                                string matKhauMaHoa = BCrypt.Net.BCrypt.HashPassword(matKhauMacDinh);
                                TaiKhoan tk = new TaiKhoan
                                {
                                    TenDangNhap = maMoi,
                                    MatKhau = matKhauMaHoa,
                                    QuyenHan = chucVu,
                                    TrangThai = true,
                                    MaNV = maMoi
                                };

                                _context.NhanVien.Add(nv);
                                _context.TaiKhoan.Add(tk);
                                _context.SaveChanges();
                                thanhCong++;
                            }
                            catch (Exception ex)
                            {
                                dongLoi++;
                                // Lấy lỗi chi tiết từ InnerException nếu có
                                string msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                                chiTietLoi.AppendLine($"- Dòng {row.RowNumber()}: {msg}");
                            }
                        }
                    }

                    // 6. Thông báo tổng kết
                    string thongBao = $"Nhập hoàn tất!\n- Thành công: {thanhCong}\n- Thất bại: {dongLoi}";
                    if (dongLoi > 0) thongBao += "\n\nChi tiết lỗi:\n" + chiTietLoi.ToString();

                    if (thanhCong > 0)
                    {
                        NhatKyHeThong.GhiLog(_maTKDangNhap, $"Nhập dữ liệu Nhân viên từ Excel (Thành công: {thanhCong} dòng)");
                    }
                    MessageBox.Show(thongBao, "Kết quả Import", MessageBoxButtons.OK,
                                    dongLoi > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

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
            saveFileDialog.Title = "Xuất danh sách Nhân viên ra Excel";
            saveFileDialog.Filter = "Tập tin Excel|*.xlsx";
            saveFileDialog.FileName = "DanhSachNhanVien_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataTable table = new DataTable();
                    // Thiết lập các cột theo đúng thứ tự file mẫu
                    table.Columns.Add("Mã NV", typeof(string));
                    table.Columns.Add("Họ tên", typeof(string));
                    table.Columns.Add("Số điện thoại", typeof(string));
                    table.Columns.Add("Email", typeof(string));
                    table.Columns.Add("Địa chỉ", typeof(string));
                    table.Columns.Add("Chức vụ", typeof(string));
                    table.Columns.Add("Lương cơ bản", typeof(decimal));
                    table.Columns.Add("Ngày sinh", typeof(string));
                    table.Columns.Add("Ngày vào làm", typeof(string));

                    var danhSach = _context.NhanVien.ToList();
                    if (danhSach.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo");
                        return;
                    }

                    foreach (var nv in danhSach)
                    {
                        table.Rows.Add(
                            nv.MaNV,
                            nv.HoTenNV,
                            nv.SDT,
                            nv.Email,
                            nv.DiaChi ?? "Chưa cập nhật",
                            nv.ChucVu,
                            nv.LuongCoBan,
                            nv.NgaySinh.ToString("dd/MM/yyyy"),
                            nv.NgayVao.ToString("dd/MM/yyyy")
                        );
                    }

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var sheet = wb.Worksheets.Add(table, "NhanVien");

                        sheet.Columns().AdjustToContents();

                        var header = sheet.Row(1);
                        header.Style.Font.Bold = true;
                        header.Style.Fill.BackgroundColor = XLColor.LightBlue;

                        wb.SaveAs(saveFileDialog.FileName);
                    }
                    NhatKyHeThong.GhiLog(_maTKDangNhap, "Xuất danh sách Nhân viên ra tập tin Excel");
                    MessageBox.Show("Xuất dữ liệu nhân viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi hệ thống");
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim();

            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TaiDuLieuLenBang();
                return;
            }

            try
            {
                _context = new QLCHMPDbContext();

                // Lọc Mã, Tên, SĐT, Chức vụ
                var ketQua = _context.NhanVien
                    .Where(n => n.MaNV.Contains(tuKhoa) ||
                                n.HoTenNV.Contains(tuKhoa) ||
                                n.SDT.Contains(tuKhoa) ||
                                n.ChucVu.Contains(tuKhoa))
                    .ToList();

                if (ketQua.Count == 0)
                {
                    MessageBox.Show($"Không tìm thấy nhân viên nào khớp với từ khóa '{tuKhoa}'!", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDuLieuLenBang(); // Trả lại bảng cũ
                    txtTimKiem.Clear();
                    txtTimKiem.Focus();
                    txtTimKiem.SelectAll();
                }
                else
                {
                    // TẠO LẠI BINDING ĐỂ FORM KHÔNG BỊ LỖI
                    BindingSource bindingSource = new BindingSource();
                    bindingSource.DataSource = ketQua;

                    txtMaNV.DataBindings.Clear();
                    txtHoTenNV.DataBindings.Clear();
                    txtSDT.DataBindings.Clear();
                    txtDiaChi.DataBindings.Clear();
                    txtEmail.DataBindings.Clear();
                    cboChucVu.DataBindings.Clear();
                    cboLuongCoBan.DataBindings.Clear();
                    dtpNgaySinh.DataBindings.Clear();
                    dtpNgayVao.DataBindings.Clear();

                    txtMaNV.DataBindings.Add("Text", bindingSource, "MaNV", false, DataSourceUpdateMode.Never);
                    txtHoTenNV.DataBindings.Add("Text", bindingSource, "HoTenNV", false, DataSourceUpdateMode.Never);
                    txtSDT.DataBindings.Add("Text", bindingSource, "SDT", false, DataSourceUpdateMode.Never);
                    txtDiaChi.DataBindings.Add("Text", bindingSource, "DiaChi", false, DataSourceUpdateMode.Never);
                    txtEmail.DataBindings.Add("Text", bindingSource, "Email", false, DataSourceUpdateMode.Never);
                    cboChucVu.DataBindings.Add("Text", bindingSource, "ChucVu", false, DataSourceUpdateMode.Never);
                    cboLuongCoBan.DataBindings.Add("Text", bindingSource, "LuongCoBan", false, DataSourceUpdateMode.Never);
                    dtpNgaySinh.DataBindings.Add("Value", bindingSource, "NgaySinh", true, DataSourceUpdateMode.Never);
                    dtpNgayVao.DataBindings.Add("Value", bindingSource, "NgayVao", true, DataSourceUpdateMode.Never);

                    dgvDSNhanVien.DataSource = bindingSource;
                    BatTatChucNang(false);
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

            TaiDuLieuLenBang();

            this.ActiveControl = null;
        }
    }
}
