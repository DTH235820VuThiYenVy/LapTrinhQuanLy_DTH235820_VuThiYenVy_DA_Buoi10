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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCuaHangMyPham.Forms
{
    public partial class frmHangSanXuat : Form
    {
        private QLCHMPDbContext _context = new QLCHMPDbContext();
        private bool _xuLyThem = false;
        private string _maHSX; // Dùng để lưu mã Loại SP khi Sửa/Xóa
        private CoGianGiaoDien _hoTroCoGianForm;
        private int _maTKDangNhap;

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lParam);
        private const int EM_SETCUEBANNER = 0x1501;
        public frmHangSanXuat(int maTKDangNhap)
        {
            InitializeComponent();
            _hoTroCoGianForm = new CoGianGiaoDien(this);
            _maTKDangNhap = maTKDangNhap;
        }

        private void BatTatChucNang(bool giaTri)
        {
            btnLuu.Enabled = giaTri;
            btnHuy.Enabled = giaTri;
            txtTenHSX.Enabled = giaTri;
            txtMaHSX.Enabled = false; // Mã loại tự sinh, không dc sửa
            txtXuatXu.Enabled = giaTri;

            btnThem.Enabled = !giaTri;
            btnSua.Enabled = !giaTri;
            btnXoa.Enabled = !giaTri;
        }

        private void TaiDuLieuLenBang()
        {
            try
            {
                // 1. Lấy danh sách Loại sản phẩm từ CSDL 
                List<HangSanXuat> hsx = _context.HangSanXuat.ToList();

                // 2. Thiết lập BindingSource để đồng bộ hóa dữ liệu 
                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = hsx;

                // 3. Xóa các liên kết cũ để tránh lỗi chồng chéo 
                txtMaHSX.DataBindings.Clear();
                txtTenHSX.DataBindings.Clear();
                txtXuatXu.DataBindings.Clear();

                // 4. Thiết lập DataBindings cho các TextBox 
                txtMaHSX.DataBindings.Add("Text", bindingSource, "MaHSX", false, DataSourceUpdateMode.Never);
                txtTenHSX.DataBindings.Add("Text", bindingSource, "TenHSX", false, DataSourceUpdateMode.Never);
                txtXuatXu.DataBindings.Add("Text", bindingSource, "XuatXu", false, DataSourceUpdateMode.Never);

                // 5. Đổ dữ liệu lên lưới hiển thị
                dgvHSX.DataSource = bindingSource;
                BatTatChucNang(false);
                // Gọi hàm BatTatChucNang(false) để đưa các ô nhập liệu về trạng thái đóng

                // Nếu danh sách rỗng (count == 0) thì khóa nút Xóa và Sửa
                bool coDuLieu = hsx.Count > 0;
                btnXoa.Enabled = coDuLieu;
                btnSua.Enabled = coDuLieu;
                btnXuat.Enabled = coDuLieu;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối với Cơ sở dữ liệu! Vui lòng kiểm tra lại SQL Server.\nChi tiết: " + ex.Message,
                                "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmHangSanXuat_Load(object sender, EventArgs e)
        {
            BatTatChucNang(false);
            TaiDuLieuLenBang();

            SendMessage(txtTimKiem.Handle, EM_SETCUEBANNER, 1, "Nhập mã hãng, tên hãng hoặc xuất xứ...");

            this.ActiveControl = null;
        }



        private void btnThem_Click(object sender, EventArgs e)
        {
            _xuLyThem = true;
            BatTatChucNang(true);
            txtTenHSX.Clear();
            txtXuatXu.Clear();
            txtTenHSX.Focus();


            var dsMa = _context.HangSanXuat.Select(c => c.MaHSX).ToList();
            txtMaHSX.Text = SinhMaTuDong.TuSinhMa(dsMa, "MaHSX", "HSX", 3);
            txtMaHSX.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvHSX.CurrentRow == null) return;
            _xuLyThem = false;
            BatTatChucNang(true);
            _maHSX = Convert.ToString(dgvHSX.CurrentRow.Cells["MaHSX"].Value); // Lấy mã để sửa
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvHSX.CurrentRow == null) return;

            // Lấy mã và tên Hãng sản xuất từ DataGridView để làm thông báo cho thân thiện
            string maHSX = Convert.ToString(dgvHSX.CurrentRow.Cells["MaHSX"].Value);
            string tenHSX = Convert.ToString(dgvHSX.CurrentRow.Cells["TenHSX"].Value); // Cột tên trên lưới của bạn có thể tên khác, hãy chỉnh lại cho khớp nhé

            string cauHoi = $"Bạn có chắc chắn muốn xóa Hãng sản xuất '{tenHSX}' không?";

            if (MessageBox.Show(cauHoi, "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var hsx = _context.HangSanXuat.Find(maHSX);
                if (hsx != null)
                {
                    // Lấy lại tên chuẩn xác từ CSDL để chuẩn bị ghi Log
                    string tenHSXDaXoa = hsx.TenHSX;

                    try
                    {
                        // 1. Tiến hành xóa và lưu xuống Database
                        _context.HangSanXuat.Remove(hsx);
                        _context.SaveChanges();

                        //GHI LỊCH SỬ NGAY SAU KHI LƯU THÀNH CÔNG
                        NhatKyHeThong.GhiLog(_maTKDangNhap, $"Xóa Hãng sản xuất: {tenHSXDaXoa} (Mã: {maHSX})");

                        MessageBox.Show("Đã xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TaiDuLieuLenBang();
                    }
                    catch (Exception ex)
                    {
                        // Bắt lỗi an toàn: Trường hợp Hãng này đang có Sản phẩm thì không được xóa
                        MessageBox.Show("Không thể xóa Hãng sản xuất này vì đã có Sản phẩm thuộc hãng này trong hệ thống!",
                                        "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenHSX.Text))
            {
                MessageBox.Show("Vui lòng nhập tên hãng sản xuất!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Tạo 1 biến chuỗi để chuẩn bị nội dung ghi log
            string hanhDongLog = "";

            if (_xuLyThem) // Trường hợp thêm mới 
            {
                HangSanXuat hsx = new HangSanXuat
                {
                    TenHSX = txtTenHSX.Text,
                    MaHSX = txtMaHSX.Text,
                    XuatXu = txtXuatXu.Text
                };
                _context.HangSanXuat.Add(hsx);

                // Chuẩn bị câu log cho việc Thêm
                hanhDongLog = $"Thêm mới Hãng sản xuất: {txtTenHSX.Text} (Mã: {txtMaHSX.Text})";

            }
            else // Trường hợp sửa 
            {
                var hsx = _context.HangSanXuat.Find(_maHSX);
                if (hsx != null)
                {
                    hsx.TenHSX = txtTenHSX.Text;
                    hsx.XuatXu = txtXuatXu.Text;
                    _context.HangSanXuat.Update(hsx);

                    // Chuẩn bị câu log cho việc Sửa
                    hanhDongLog = $"Cập nhật Hãng sản xuất: {txtTenHSX.Text} (Mã: {_maHSX})";
                }
            }

            try
            {
                _context.SaveChanges();

                if (!string.IsNullOrEmpty(hanhDongLog))
                {
                    TienIch.NhatKyHeThong.GhiLog(1, hanhDongLog);
                }

                MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDuLieuLenBang();

                BatTatChucNang(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + (ex.InnerException?.Message ?? ex.Message), "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnNhap_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Nhập dữ liệu Hãng sản xuất từ Excel";
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
                            try
                            {
                                string tenHSX = row.Cell(2).Value.ToString().Trim();
                                // Cột C (3) là Xuất Xứ
                                string xuatXu = row.Cell(3).Value.ToString().Trim();

                                if (string.IsNullOrEmpty(tenHSX)) throw new Exception("Tên hãng bị trống.");

                                // 1. Kiểm tra trùng tên trong Database để tránh thêm rác
                                var trungTen = _context.HangSanXuat.FirstOrDefault(h => h.TenHSX == tenHSX);
                                if (trungTen != null) throw new Exception($"Hãng '{tenHSX}' đã tồn tại trong hệ thống.");

                                var dsMa = _context.HangSanXuat.Select(c => c.MaHSX).ToList();
                                string maMoi = SinhMaTuDong.TuSinhMa(dsMa, "MaHSX", "HSX", 3);

                                // 3. Tạo đối tượng và thêm vào CSDL
                                HangSanXuat hsx = new HangSanXuat
                                {
                                    MaHSX = maMoi,
                                    TenHSX = tenHSX,
                                    XuatXu = xuatXu
                                };

                                _context.HangSanXuat.Add(hsx);
                                _context.SaveChanges();
                                NhatKyHeThong.GhiLog(_maTKDangNhap, $"Nhập dữ liệu Hãng sản xuất từ Excel (Thành công: {thanhCong} hãng)");
                                thanhCong++;
                            }
                            catch (Exception ex)
                            {
                                dongLoi++;
                                chiTietLoi.AppendLine($"- Dòng {row.RowNumber()}: {ex.Message}");
                            }
                        }
                    }
                    if (thanhCong > 0)
                    {
                        TienIch.NhatKyHeThong.GhiLog(1, $"Nhập dữ liệu Hãng sản xuất từ Excel (Thành công: {thanhCong} hãng)");
                    }
                    string thongBao = $"Nhập hoàn tất!\n- Thành công: {thanhCong} dòng.\n- Lỗi: {dongLoi} dòng.";
                    if (dongLoi > 0) thongBao += "\n\nChi tiết:\n" + chiTietLoi.ToString();

                    MessageBox.Show(thongBao, "Kết quả Import", MessageBoxButtons.OK,
                                    dongLoi > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                    TaiDuLieuLenBang(); // Load lại GridView cho Vy thấy
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi");
                }
            }
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Xuất danh sách Hãng sản xuất ra Excel";
            saveFileDialog.Filter = "Tập tin Excel|*.xlsx";
            saveFileDialog.FileName = "HangSanXuat_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("Mã Hãng", typeof(string));
                    table.Columns.Add("Tên Hãng Sản Xuất", typeof(string));
                    table.Columns.Add("Xuất Xứ", typeof(string));

                    var danhSach = _context.HangSanXuat.ToList();
                    if (danhSach.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo");
                        return;
                    }

                    foreach (var item in danhSach)
                    {
                        table.Rows.Add(item.MaHSX, item.TenHSX, item.XuatXu);
                    }

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var sheet = wb.Worksheets.Add(table, "HangSanXuat");
                        sheet.Columns().AdjustToContents(); // Tự căn chỉnh cột cho đẹp
                        wb.SaveAs(saveFileDialog.FileName);
                    }
                    NhatKyHeThong.GhiLog(_maTKDangNhap, "Xuất danh sách Hãng sản xuất ra tập tin Excel");
                    MessageBox.Show("Xuất file thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi hệ thống");
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TaiDuLieuLenBang();
                return;
            }

            try
            {
                _context = new QLCHMPDbContext();

                // Mã Hãng, Tên Hãng hoặc Xuất Xứ
                var ketQua = _context.HangSanXuat
                    .Where(h => h.MaHSX.ToLower().Contains(tuKhoa) ||
                                h.TenHSX.ToLower().Contains(tuKhoa) ||
                                h.XuatXu.ToLower().Contains(tuKhoa))
                    .ToList();

                if (ketQua.Count == 0)
                {
                    MessageBox.Show($"Không tìm thấy hãng sản xuất nào khớp với từ khóa '{txtTimKiem.Text.Trim()}'!", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDuLieuLenBang();
                    txtTimKiem.Clear();
                    txtTimKiem.Focus();
                    txtTimKiem.SelectAll();
                }
                else
                {
                    BindingSource bindingSource = new BindingSource();
                    bindingSource.DataSource = ketQua;

                    txtMaHSX.DataBindings.Clear();
                    txtTenHSX.DataBindings.Clear();
                    txtXuatXu.DataBindings.Clear();

                    txtMaHSX.DataBindings.Add("Text", bindingSource, "MaHSX", false, DataSourceUpdateMode.Never);
                    txtTenHSX.DataBindings.Add("Text", bindingSource, "TenHSX", false, DataSourceUpdateMode.Never);
                    txtXuatXu.DataBindings.Add("Text", bindingSource, "XuatXu", false, DataSourceUpdateMode.Never);

                    dgvHSX.DataSource = bindingSource;
                    BatTatChucNang(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tìm kiếm: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
