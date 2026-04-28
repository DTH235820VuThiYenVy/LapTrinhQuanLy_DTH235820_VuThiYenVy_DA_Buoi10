using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using QuanLyCuaHangMyPham.Data;
using QuanLyCuaHangMyPham.TienIch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace QuanLyCuaHangMyPham.Forms
{
    public partial class frmLoaiSanPham : Form
    {
        private QLCHMPDbContext _context = new QLCHMPDbContext();
        private bool _xuLyThem = false;
        private string _maLoai; // Dùng để lưu mã Loại SP khi Sửa/Xóa
        private CoGianGiaoDien _hoTroCoGianForm;
        private int _maTKDangNhap;

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        public frmLoaiSanPham(int maTKDangNhap)
        {
            InitializeComponent();
            _hoTroCoGianForm = new CoGianGiaoDien(this);
            _maTKDangNhap = maTKDangNhap;
        }

        private void BatTatChucNang(bool giaTri)
        {
            btnLuu.Enabled = giaTri;
            btnHuy.Enabled = giaTri;
            txtTenLoai.Enabled = giaTri;
            txtMaLoai.Enabled = false; // Mã loại tự sinh, không dc sửa

            btnThem.Enabled = !giaTri;
            btnSua.Enabled = !giaTri;
            btnXoa.Enabled = !giaTri;
        }

        private void TaiDuLieuLenBang()
        {
            // 1. Lấy danh sách Loại sản phẩm từ CSDL 
            List<LoaiSanPham> lsp = _context.LoaiSanPham.ToList();

            // 2. Thiết lập BindingSource để đồng bộ hóa dữ liệu 
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = lsp;

            // 3. Xóa các liên kết cũ để tránh lỗi chồng chéo 
            txtMaLoai.DataBindings.Clear();
            txtTenLoai.DataBindings.Clear();

            // 4. Thiết lập DataBindings cho các TextBox 
            txtMaLoai.DataBindings.Add("Text", bindingSource, "MaLoai", false, DataSourceUpdateMode.Never);
            txtTenLoai.DataBindings.Add("Text", bindingSource, "TenLoai", false, DataSourceUpdateMode.Never);

            // 5. Đổ dữ liệu lên lưới hiển thị [cite: 334]
            dgvLoaiSanPham.DataSource = bindingSource;
            BatTatChucNang(false);

            // Nếu danh sách rỗng (count == 0) thì khóa nút Xóa và Sửa
            bool coDuLieu = lsp.Count > 0;
            btnXoa.Enabled = coDuLieu;
            btnSua.Enabled = coDuLieu;
            btnXuat.Enabled = coDuLieu;
        }


        private void frmLoaiSanPham_Load(object sender, EventArgs e)
        {
            BatTatChucNang(false);
            TaiDuLieuLenBang();

            SendMessage(txtTimKiem.Handle, EM_SETCUEBANNER, 1, "Nhập mã loại hoặc tên loại...");
            this.ActiveControl = null;

        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            _xuLyThem = true;
            BatTatChucNang(true);
            txtTenLoai.Clear();
            txtTenLoai.Focus();

            var dsMa = _context.LoaiSanPham.Select(c => c.MaLoai).ToList();
            txtMaLoai.Text = SinhMaTuDong.TuSinhMa(dsMa, "MaLoai", "LSP", 3);
            txtMaLoai.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvLoaiSanPham.CurrentRow == null) return;
            _xuLyThem = false;
            BatTatChucNang(true);
            _maLoai = Convert.ToString(dgvLoaiSanPham.CurrentRow.Cells["MaLoai"].Value); // Lấy mã để sửa 
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvLoaiSanPham.CurrentRow == null) return;

            if (MessageBox.Show("Xác nhận xóa loại sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _maLoai = Convert.ToString(dgvLoaiSanPham.CurrentRow.Cells["MaLoai"].Value);
                    var lsp = _context.LoaiSanPham.Find(_maLoai);
                    if (lsp != null)
                    {
                        string tenLoaiDaXoa = lsp.TenLoai; // Lưu tên trước khi xóa

                        _context.LoaiSanPham.Remove(lsp);
                        _context.SaveChanges();

                        // --- GHI LỊCH SỬ ---
                        NhatKyHeThong.GhiLog(_maTKDangNhap, $"Xóa loại sản phẩm: {tenLoaiDaXoa} (Mã: {_maLoai})");

                        MessageBox.Show("Đã xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TaiDuLieuLenBang();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Không thể xóa do loại sản phẩm này đang được sử dụng trong danh sách Sản phẩm!", "Lỗi khóa ngoại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnNhap_Click(object sender, EventArgs e)
        {
            // Khởi tạo hộp thoại mở file Excel
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Nhập dữ liệu Loại sản phẩm từ Excel";
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
                        var rows = worksheet.RowsUsed().Skip(1); // Bỏ qua dòng tiêu đề (Mã Loại, Tên Loại)

                        foreach (var row in rows)
                        {
                            try
                            {
                                string tenLoaiMoi = row.Cell(2).Value.ToString().Trim();

                                if (string.IsNullOrEmpty(tenLoaiMoi)) throw new Exception("Tên loại không được để trống.");

                                //  Nếu tên này đã có trong DB thì bỏ qua hoặc báo lỗi
                                var trungTen = _context.LoaiSanPham.FirstOrDefault(l => l.TenLoai == tenLoaiMoi);
                                if (trungTen != null) throw new Exception($"Tên loại '{tenLoaiMoi}' đã tồn tại trong hệ thống.");

                                // TỰ SINH MÃ MỚI (Lấy danh sách mã hiện tại để tránh trùng)
                                var dsMa = _context.LoaiSanPham.Select(c => c.MaLoai).ToList();
                                string maMoi = SinhMaTuDong.TuSinhMa(dsMa, "MaLoai", "LSP", 3);

                                // TẠO ĐỐI TƯỢNG VÀ LƯU
                                LoaiSanPham lsp = new LoaiSanPham
                                {
                                    MaLoai = maMoi,
                                    TenLoai = tenLoaiMoi
                                };

                                _context.LoaiSanPham.Add(lsp);
                                _context.SaveChanges(); // Lưu từng dòng để đảm bảo mã tự sinh không bị trùng
                                thanhCong++;
                            }
                            catch (Exception ex)
                            {
                                dongLoi++;
                                chiTietLoi.AppendLine($"- Dòng {row.RowNumber()}: {ex.Message}");
                            }
                        }
                    }

                    // HIỂN THỊ KẾT QUẢ
                    string thongBao = $"Nhập hoàn tất!\n- Thành công: {thanhCong} dòng.\n- Lỗi: {dongLoi} dòng.";
                    if (dongLoi > 0) thongBao += "\n\nChi tiết lỗi:\n" + chiTietLoi.ToString();

                    if (thanhCong > 0)
                    {
                        NhatKyHeThong.GhiLog(_maTKDangNhap, $"Nhập dữ liệu Loại sản phẩm từ Excel (Thành công: {thanhCong} loại)");
                    }

                    MessageBox.Show(thongBao, "Kết quả Import", MessageBoxButtons.OK,
                                    dongLoi > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                    TaiDuLieuLenBang(); // Load lại GridView cho Vy thấy hàng mới
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi");
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenLoai.Text))
            {
                MessageBox.Show("Vui lòng nhập tên loại sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string hanhDongLog = ""; // Dùng để ghi log

                if (_xuLyThem) // Trường hợp thêm mới 
                {
                    LoaiSanPham lsp = new LoaiSanPham
                    {
                        TenLoai = txtTenLoai.Text,
                        MaLoai = txtMaLoai.Text
                    };
                    _context.LoaiSanPham.Add(lsp);
                    hanhDongLog = $"Thêm mới loại sản phẩm: {lsp.TenLoai} (Mã: {lsp.MaLoai})";
                }
                else // Trường hợp sửa 
                {
                    var lsp = _context.LoaiSanPham.Find(_maLoai);
                    if (lsp != null)
                    {
                        lsp.TenLoai = txtTenLoai.Text;
                        _context.LoaiSanPham.Update(lsp);
                        hanhDongLog = $"Cập nhật loại sản phẩm: {lsp.TenLoai} (Mã: {_maLoai})";
                    }
                }

                _context.SaveChanges();

                // --- GHI LỊCH SỬ ---
                if (!string.IsNullOrEmpty(hanhDongLog))
                {
                    NhatKyHeThong.GhiLog(_maTKDangNhap, hanhDongLog);
                }

                MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDuLieuLenBang();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            DialogResult ketQua = MessageBox.Show("Bạn chắc chắn muốn hủy bỏ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ketQua == DialogResult.Yes)
            {
                _xuLyThem = false;
                TaiDuLieuLenBang();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
                this.Close();
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Xuất dữ liệu ra Excel";
            saveFileDialog.Filter = "Tập tin Excel|*.xlsx";
            saveFileDialog.FileName = "LoaiSanPham_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";


            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("Mã Loại", typeof(string));
                    table.Columns.Add("Tên Loại", typeof(string));

                    var danhSach = _context.LoaiSanPham.ToList();
                    if (danhSach.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo");
                        return;
                    }

                    foreach (var item in danhSach)
                    {
                        table.Rows.Add(item.MaLoai, item.TenLoai);
                    }

                    // Tạo và lưu file
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(table, "LoaiSanPham");
                        wb.SaveAs(saveFileDialog.FileName);
                    }
                    NhatKyHeThong.GhiLog(_maTKDangNhap, "Xuất danh sách Loại sản phẩm ra tập tin Excel");
                    MessageBox.Show("Xuất file thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (IOException ex)
                {
                    //lỗi file đang bị mở
                    MessageBox.Show("File này đang được mở bởi một chương trình khác. Vui lòng đóng file và thử lại!", "Lỗi truy cập file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    //lỗi chung khác
                    MessageBox.Show("Có lỗi xảy ra khi xuất file: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TaiDuLieuLenBang();
                return;
            }

            try
            {
                _context = new QLCHMPDbContext();

                // Tìm theo Mã Loại hoặc Tên Loại
                var ketQua = _context.LoaiSanPham
                    .Where(l => l.MaLoai.ToLower().Contains(tuKhoa) ||
                                l.TenLoai.ToLower().Contains(tuKhoa))
                    .ToList();

                if (ketQua.Count == 0)
                {
                    MessageBox.Show($"Không tìm thấy loại sản phẩm nào khớp với từ khóa '{txtTimKiem.Text.Trim()}'!", "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDuLieuLenBang();
                    txtTimKiem.Clear();
                    txtTimKiem.Focus();
                    txtTimKiem.SelectAll();
                }
                else
                {
                    BindingSource bindingSource = new BindingSource();
                    bindingSource.DataSource = ketQua;

                    txtMaLoai.DataBindings.Clear();
                    txtTenLoai.DataBindings.Clear();

                    txtMaLoai.DataBindings.Add("Text", bindingSource, "MaLoai", false, DataSourceUpdateMode.Never);
                    txtTenLoai.DataBindings.Add("Text", bindingSource, "TenLoai", false, DataSourceUpdateMode.Never);

                    dgvLoaiSanPham.DataSource = bindingSource;
                    BatTatChucNang(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
