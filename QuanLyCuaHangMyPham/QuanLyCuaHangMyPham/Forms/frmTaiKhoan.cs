using BCrypt.Net;
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
    public partial class frmTaiKhoan : Form
    {
        private QLCHMPDbContext _context = new QLCHMPDbContext();
        private CoGianGiaoDien _hoTroCoGianForm;
        private int _maTKDangNhap;
        private int _maTK = -1;



        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lParam);
        private const int EM_SETCUEBANNER = 0x1501;
        public frmTaiKhoan(int maTKDangNhap)
        {
            InitializeComponent();
            _hoTroCoGianForm = new CoGianGiaoDien(this);
            _maTKDangNhap = maTKDangNhap;
        }

        private void LoadQuyenHan()
        {
            // Danh sách quyền hạn cố định
            cboQuyenHan.Items.Clear();
            cboQuyenHan.Items.Add("Admin");
            cboQuyenHan.Items.Add("Quản lý");
            cboQuyenHan.Items.Add("Nhân viên bán hàng");
            cboQuyenHan.Items.Add("Nhân viên kho");
            cboQuyenHan.SelectedIndex = 0;
        }

        private void BatTatChucNang(bool giaTri)
        {
            // Tên đăng nhập và Nhân viên không cho sửa để đảm bảo tính toàn vẹn
            txtTenDangNhap.Enabled = false;
            txtHoTenNV.Enabled = false;

            cboQuyenHan.Enabled = giaTri;
            txtMatKhau.Enabled = giaTri;

            btnLuu.Enabled = giaTri;
            btnHuy.Enabled = giaTri;

            btnSua.Enabled = !giaTri;
            btnKhoa.Enabled = !giaTri;
            btnReset.Enabled = !giaTri;
        }

        private void TaiDuLieuLenBang()
        {
            _context = new QLCHMPDbContext();

            // 1. Chuyển đổi True/False thành chữ tiếng Việt ngay trong lệnh Select
            var ds = _context.TaiKhoan.Select(t => new
            {
                t.MaTK,
                HoTenNV = t.NhanVien.HoTenNV,
                t.TenDangNhap,
                t.QuyenHan,
                TrangThai = t.TrangThai ? "Đang hoạt động" : "Ngừng hoạt động"
            }).ToList();

            dgvDSTaiKhoan.DataSource = ds;

            // 2. Định dạng tiêu đề và ẩn mã
            dgvDSTaiKhoan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dgvDSTaiKhoan.Columns["MaTK"] != null) dgvDSTaiKhoan.Columns["MaTK"].Visible = false;
        }
        private void frmTaiKhoan_Load(object sender, EventArgs e)
        {
            LoadQuyenHan();
            TaiDuLieuLenBang();
            BatTatChucNang(false);

            SendMessage(txtTimKiem.Handle, EM_SETCUEBANNER, 1, "Nhập Tên đăng nhập hoặc Họ tên NV...");
            this.ActiveControl = null;
        }

        private void dgvDSTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvDSTaiKhoan.Rows[e.RowIndex];
            _maTK = (int)row.Cells["MaTK"].Value;
            txtTenDangNhap.Text = row.Cells["TenDangNhap"].Value.ToString();
            txtHoTenNV.Text = row.Cells["HoTenNV"].Value.ToString();
            cboQuyenHan.Text = row.Cells["QuyenHan"].Value.ToString();
            txtMatKhau.Clear();

            // Nếu tài khoản đang hoạt động thì sáng nút Khóa, ẩn nút Mở và ngược lại
            string trangThaiText = row.Cells["TrangThai"].Value.ToString();
            if (trangThaiText == "Đang hoạt động")
            {
                btnKhoa.Enabled = true;
                btnMoKhoa.Enabled = false;
            }
            else
            {
                btnKhoa.Enabled = false;
                btnMoKhoa.Enabled = true;
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (_maTK == -1)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần chỉnh sửa!");
                return;
            }
            BatTatChucNang(true);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                var tk = _context.TaiKhoan.Find(_maTK);
                if (tk != null)
                {
                    string hanhDongLog = $"Cập nhật tài khoản {tk.TenDangNhap}: Quyền mới là {cboQuyenHan.Text}";
                    tk.QuyenHan = cboQuyenHan.Text;

                    // Nếu người dùng có nhập mật khẩu mới thì mã hóa rồi mới lưu
                    if (!string.IsNullOrWhiteSpace(txtMatKhau.Text))
                    {
                        tk.MatKhau = BCrypt.Net.BCrypt.HashPassword(txtMatKhau.Text);
                        hanhDongLog += " (kèm đổi mật khẩu)";
                    }

                    _context.SaveChanges();
                    NhatKyHeThong.GhiLog(_maTKDangNhap, hanhDongLog);
                    MessageBox.Show("Cập nhật tài khoản thành công!");
                    TaiDuLieuLenBang();
                    BatTatChucNang(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (_maTK == -1) return;

            if (MessageBox.Show("Đặt mật khẩu về mặc định là '123'?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var tk = _context.TaiKhoan.Find(_maTK);
                if (tk != null)
                {
                    tk.MatKhau = BCrypt.Net.BCrypt.HashPassword("123");
                    _context.SaveChanges();
                    NhatKyHeThong.GhiLog(_maTKDangNhap, $"Reset mật khẩu tài khoản {tk.TenDangNhap} về mặc định");
                    MessageBox.Show("Đã đặt lại mật khẩu thành công!");
                }
            }
        }



        private void btnHuy_Click(object sender, EventArgs e)
        {
            BatTatChucNang(false);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void btnKhoa_Click(object sender, EventArgs e)
        {
            if (_maTK == -1) return;

            var tk = _context.TaiKhoan.Find(_maTK);
            if (tk != null)
            {
                tk.TrangThai = false; // Khóa
                _context.SaveChanges();
                NhatKyHeThong.GhiLog(_maTKDangNhap, $"Khóa tài khoản nhân viên: {tk.TenDangNhap}");
                MessageBox.Show("Đã khóa tài khoản thành công!", "Thông báo");
                TaiDuLieuLenBang();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnMoKhoa_Click(object sender, EventArgs e)
        {
            if (_maTK == -1) return;

            var tk = _context.TaiKhoan.Find(_maTK);
            if (tk != null)
            {
                tk.TrangThai = true; // Mở khóa
                NhatKyHeThong.GhiLog(_maTKDangNhap, $"Mở khóa tài khoản nhân viên: {tk.TenDangNhap}");
                _context.SaveChanges();
                MessageBox.Show("Đã mở khóa tài khoản thành công!", "Thông báo");
                TaiDuLieuLenBang();
            }
        }

        private void dgvDSTaiKhoan_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvDSTaiKhoan.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                if (e.Value.ToString() == "Ngừng hoạt động")
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new Font(dgvDSTaiKhoan.Font, FontStyle.Bold);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            TaiDuLieuLenBang();

            txtTenDangNhap.Clear();
            txtHoTenNV.Clear();
            txtMatKhau.Clear();
            _maTK = -1;
            BatTatChucNang(false);

            this.ActiveControl = null; // Để chữ mờ hiện lại
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TaiDuLieuLenBang();
                return;
            }

            try
            {
                _context = new QLCHMPDbContext();

                // Lọc tìm theo Tên đăng nhập hoặc Họ Tên Nhân Viên (phải Include NhanVien vô)
                var ketQua = _context.TaiKhoan
                    .Where(t => t.TenDangNhap.ToLower().Contains(tuKhoa) ||
                                (t.NhanVien != null && t.NhanVien.HoTenNV.ToLower().Contains(tuKhoa)))
                    .Select(t => new
                    {
                        t.MaTK,
                        HoTenNV = t.NhanVien != null ? t.NhanVien.HoTenNV : "Không xác định",
                        t.TenDangNhap,
                        t.QuyenHan,
                        TrangThai = t.TrangThai ? "Đang hoạt động" : "Ngừng hoạt động"
                    }).ToList();

                if (ketQua.Count == 0)
                {
                    MessageBox.Show($"Không tìm thấy tài khoản nào khớp với '{txtTimKiem.Text.Trim()}'!", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDuLieuLenBang();
                    txtTimKiem.Clear();
                    txtTimKiem.Focus();
                    txtTimKiem.SelectAll();
                }
                else
                {
                    // Tìm thấy thì đổ lên bảng
                    dgvDSTaiKhoan.DataSource = ketQua;

                    // Reset giao diện bên dưới để tránh chọn nhầm người cũ
                    txtTenDangNhap.Clear();
                    txtHoTenNV.Clear();
                    txtMatKhau.Clear();
                    _maTK = -1;
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
