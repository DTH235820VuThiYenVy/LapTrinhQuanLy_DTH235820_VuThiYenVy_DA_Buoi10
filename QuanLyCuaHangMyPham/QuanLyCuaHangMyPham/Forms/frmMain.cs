using Microsoft.EntityFrameworkCore;
using QuanLyCuaHangMyPham.Data;
using QuanLyCuaHangMyPham.TienIch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BC = BCrypt.Net.BCrypt;

namespace QuanLyCuaHangMyPham.Forms
{
    public partial class frmMain : Form
    {
        private QuanLyCuaHangMyPham.Reports.frmThongKeSanPham thongKeSanPham = null;
        private QuanLyCuaHangMyPham.Reports.frmThongKeDoanhThu thongKeDoanhThu = null;
        private QuanLyCuaHangMyPham.Reports.frmThongKeTonKho thongKeTonKho = null;
        private QuanLyCuaHangMyPham.Reports.frmBaoCaoThuChi baoCaoThuChi = null;
        private QuanLyCuaHangMyPham.Reports.frmSanPhamBanChay sanPhamBanChay = null;
        QLCHMPDbContext context = new QLCHMPDbContext(); // Khởi tạo biến ngữ cảnh CSDL
        frmDangNhap dangNhap = null;
        frmLoaiSanPham loaiSanPham = null;
        frmHangSanXuat hangSanXuat = null;
        frmSanPham sanPham = null;
        frmKhachHang khachHang = null;
        frmNhanVien nhanVien = null;
        frmHoaDon hoaDon = null;
        frmNhaCungCap nhaCungCap = null;
        frmPhieuNhap phieuNhap = null;
        frmPhieuChi phieuChi = null;
        frmPhanCa phanCa = null;
        frmCaLamViec caLamViec = null;
        frmLichSuHoatDong lichSuHoatDong = null;
        frmTaiKhoan taiKhoan = null;
        frmThongTinCaNhan thongTinCaNhan = null;
        frmLichLamViec lichLamViec = null;
        string hoTenNhanVien = "";
        TaiKhoan taiKhoanDangNhap = null;
        private CauHinhGiaoDien _currentConfig;
        public frmMain()
        {
            InitializeComponent();

        }

        // Class giúp tùy chỉnh màu sắc khi rê chuột (Hover)
        public class MyColorTable : ProfessionalColorTable
        {
            private Color _back;
            private Color _text;
            public MyColorTable(Color back, Color text) { _back = back; _text = text; }
            public override Color MenuItemSelected => Color.FromArgb(80, 80, 80); // Màu khi rê chuột vào
            public override Color MenuItemSelectedGradientBegin => Color.FromArgb(80, 80, 80);
            public override Color MenuItemSelectedGradientEnd => Color.FromArgb(80, 80, 80);
            public override Color MenuItemBorder => _text;
        }
        private void ThucHienDoiMau(Form f, CauHinhGiaoDien config)
        {
            try
            {
                // --- 1. CHIA NHÁNH RÕ RÀNG CHO TỪNG THEME ---
                if (config.Theme == "Mặc định")
                {
                    ThemeManager.ToDefaultMode(f); // Gọi giao diện Navy mới
                }
                else if (config.Theme == "Chế độ sáng (Light Mode")
                {
                    ThemeManager.ToLightMode(f); // Gọi giao diện Xám Windows cũ
                }
                else if (config.Theme == "Chế độ tối (Dark Mode)")
                {
                    ThemeManager.ToDarkMode(f);
                }
                else if (config.Theme == "Mùa xuân (Hồng)")
                {
                    ThemeManager.ToSpringMode(f);
                }
                else
                {
                    // Nhánh Tùy chỉnh
                    f.BackColor = ColorTranslator.FromHtml(config.MauNen);
                    ThemeManager.ToCustomMode(f, f.BackColor, ColorTranslator.FromHtml(config.MauChu));
                }

                // --- 2. XỬ LÝ RIÊNG THANH MENU & STATUS CHO FRMMAIN ---
                foreach (Control c in f.Controls)
                {
                    if (c is MenuStrip ms)
                    {
                        if (config.Theme == "Sáng")
                        {
                            ms.BackColor = SystemColors.Control;
                            ms.ForeColor = Color.Black;
                            ms.RenderMode = ToolStripRenderMode.ManagerRenderMode; // XÓA BỎ LỚP MÀU ÉP BÊN NGOÀI
                        }
                        else if (config.Theme == "Mặc định")
                        {
                            ms.BackColor = Color.Navy; // Menu chính màu Navy
                            ms.ForeColor = Color.White;
                            ms.Renderer = new ToolStripProfessionalRenderer(new MyColorTable(ms.BackColor, ms.ForeColor));
                        }
                        else
                        {
                            ms.BackColor = f.BackColor;
                            ms.ForeColor = (f.BackColor.R < 100) ? Color.White : ColorTranslator.FromHtml(config.MauChu);
                            ms.Renderer = new ToolStripProfessionalRenderer(new MyColorTable(ms.BackColor, ms.ForeColor));
                        }

                        if (!string.IsNullOrEmpty(config.FontChu))
                            ms.Font = new Font(config.FontChu, config.CoChu > 0 ? config.CoChu : 10);
                    }

                    if (c is StatusStrip ss)
                    {
                        if (config.Theme == "Sáng")
                        {
                            ss.BackColor = SystemColors.Control;
                            ss.ForeColor = Color.Black;
                        }
                        else if (config.Theme == "Mặc định")
                        {
                            ss.BackColor = Color.Navy;
                            ss.ForeColor = Color.White;
                        }
                        else
                        {
                            ss.BackColor = f.BackColor;
                            ss.ForeColor = (f.BackColor.R < 100) ? Color.White : Color.Black;
                        }
                        foreach (ToolStripItem item in ss.Items) item.ForeColor = ss.ForeColor;
                    }
                }
            }
            catch { /* Bỏ qua nếu có lỗi đổi màu */ }
        }
        public void ApDungGiaoDien()
        {
            if (taiKhoanDangNhap == null) return;

            using (var db = new QLCHMPDbContext())
            {
                _currentConfig = db.CauHinhGiaoDien.FirstOrDefault(c => c.MaNV == taiKhoanDangNhap.MaNV);

                if (_currentConfig != null)
                {
                    ThucHienDoiMau(this, _currentConfig);
                    // Nhuộm luôn các form đang mở sẵn
                    foreach (Form f in this.MdiChildren)
                    {
                        ThucHienDoiMau(f, _currentConfig);
                    }
                }
            }
        }

        private void CapNhatMauChoFormCon(Form f)
        {
            using (var db = new QLCHMPDbContext())
            {
                var config = db.CauHinhGiaoDien.FirstOrDefault(c => c.MaNV == taiKhoanDangNhap.MaNV);
                if (config != null)
                {
                    if (config.Theme.Contains("Hồng")) ThemeManager.ToSpringMode(f);
                    else if (config.Theme.Contains("tối")) ThemeManager.ToDarkMode(f);
                }
            }
        }
        private void SetDarkMode()
        {
            Color darkBack = Color.FromArgb(30, 30, 30);
            Color lightText = Color.White;

            this.BackColor = darkBack;
            this.MainMenuStrip.BackColor = darkBack;
            this.MainMenuStrip.ForeColor = lightText;

            // Đổi màu cho tất cả các nút bấm trên Form Main
            foreach (Control c in this.Controls)
            {
                if (c is Button)
                {
                    c.BackColor = Color.FromArgb(60, 60, 60);
                    c.ForeColor = lightText;
                }
            }
        }
        private void DangNhap()
        {

            ApDungGiaoDien();
        LamLai:
            if (dangNhap == null || dangNhap.IsDisposed)
            {
                dangNhap = new frmDangNhap();
            }

            if (dangNhap.ShowDialog() == DialogResult.OK)
            {
                string tenDangNhap = dangNhap.txtTenDangNhap.Text;
                string matKhau = dangNhap.txtMatKhau.Text;

                if (string.IsNullOrWhiteSpace(tenDangNhap))
                {
                    MessageBox.Show("Tên đăng nhập không được bỏ trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dangNhap.txtTenDangNhap.Focus();
                    goto LamLai;
                }
                else if (string.IsNullOrWhiteSpace(matKhau))
                {
                    MessageBox.Show("Mật khẩu không được bỏ trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dangNhap.txtMatKhau.Focus();
                    goto LamLai;
                }
                else
                {
                    using (var context = new QLCHMPDbContext())
                    {
                        //tìm tài khoản bằng Tên đăng nhập và Trạng thái
                        var tk = context.TaiKhoan
                                        .Include(t => t.NhanVien)
                                        .FirstOrDefault(x => x.TenDangNhap == tenDangNhap && x.TrangThai == true);

                        if (tk != null)
                        {
                            bool laMatKhauChinhXac = false;

                            try
                            {
                                if (tk.MatKhau.StartsWith("$2"))
                                {
                                    laMatKhauChinhXac = BC.Verify(matKhau, tk.MatKhau);
                                }
                                else
                                {
                                    laMatKhauChinhXac = (matKhau == tk.MatKhau);
                                }
                            }
                            catch
                            {
                                // Phòng hờ trường hợp chuỗi trong DB bị lỗi định dạng
                                laMatKhauChinhXac = (matKhau == tk.MatKhau);
                            }

                            if (laMatKhauChinhXac)
                            {
                                // Đăng nhập thành công
                                taiKhoanDangNhap = tk;
                                string tenNhanVien = tk.NhanVien != null ? tk.NhanVien.HoTenNV : tk.TenDangNhap;
                                string quyen = tk.QuyenHan.Trim();
                                lblTrangThai.Text = "Xin chào: " + tenNhanVien + " | Chức vụ: " + quyen;
                                ApDungGiaoDien();
                                // Phân quyền
                                if (quyen == VaiTro.Admin) QuyenAdmin();
                                else if (quyen == VaiTro.QuanLy) QuyenQuanLy();
                                else if (quyen == VaiTro.NhanVienBanHang) QuyenNhanVienBanHang();
                                else if (quyen == VaiTro.NhanVienKho) QuyenNhanVienKho();
                                //ghi log
                                NhatKyHeThong.GhiLog(tk.MaTK, $"Đăng nhập vào hệ thống (Quyền: {quyen})");

                                MessageBox.Show("Đăng nhập thành công, xin chào " + tenNhanVien + "!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Mật khẩu không chính xác!", "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                dangNhap.txtMatKhau.Clear();
                                dangNhap.txtMatKhau.Focus();
                                goto LamLai;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Tên đăng nhập không tồn tại hoặc tài khoản bị khóa!", "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            dangNhap.txtTenDangNhap.Focus();
                            goto LamLai;
                        }
                    }
                }
            }
            else
            {
                // Nếu người dùng bấm "Hủy bỏ" hoặc tắt form Đăng nhập
                Application.Exit();
            }
        }
        public void ChuaDangNhap()
        {
            pnlSidebar.Visible = false;
            btnDangNhap.Enabled = true;

            // Mờ tất cả các hệ thống
            btnDangXuat.Enabled = false;
            btnDoiMatKhau.Enabled = false;
            btnThongTinCaNhan.Enabled = false;
            btnLichLamViec.Enabled = false;

            // Mờ các menu quản lý
            btnLoaiSanPham.Enabled = false;
            btnHangSanXuat.Enabled = false;
            btnSanPham.Enabled = false;
            btnKhachHang.Enabled = false;
            btnNhanVien.Enabled = false;
            btnHoaDon.Enabled = false;
            btnNhaCungCap.Enabled = false;
            btnPhieuNhap.Enabled = false;
            btnPhieuChi.Enabled = false;
            btnPhanCa.Enabled = false;
            btnCaLamViec.Enabled = false;
            btnLichSuHoatDong.Enabled = false;
            btnTaiKhoan.Enabled = false;

            // Mờ các menu thống kê báo cáo
            btnBaoCaoThongKe.Enabled = false;

            // Hiển thị thông tin trên thanh trạng thái
            lblTrangThai.Text = "Chưa đăng nhập.";
        }

        public void QuyenAdmin()
        {
            pnlSidebar.Visible = true;
            btnDangNhap.Enabled = false;
            btnDangXuat.Enabled = true;
            btnDoiMatKhau.Enabled = true;

            btnThongTinCaNhan.Enabled = true;
            btnLichLamViec.Enabled = true;
            // Mở khóa toàn bộ các nút trên Sidebar mới cho Admin
            btnLoaiSanPham.Enabled = true;
            btnHangSanXuat.Enabled = true;
            btnSanPham.Enabled = true;
            btnKhachHang.Enabled = true;
            btnNhanVien.Enabled = true;
            btnHoaDon.Enabled = true;
            btnNhaCungCap.Enabled = true;
            btnPhieuNhap.Enabled = true;
            btnPhieuChi.Enabled = true;
            btnPhanCa.Enabled = true;
            btnCaLamViec.Enabled = true;
            btnLichSuHoatDong.Enabled = true;
            btnTaiKhoan.Enabled = true;

            btnBaoCaoThongKe.Enabled = true;
        }

        public void QuyenQuanLy()
        {
            pnlSidebar.Visible = true;
            btnDangNhap.Enabled = false;
            btnDangXuat.Enabled = true;
            btnDoiMatKhau.Enabled = true;

            // Sáng các chức năng được phép
            btnLoaiSanPham.Enabled = true;
            btnHangSanXuat.Enabled = true;
            btnSanPham.Enabled = true;
            btnKhachHang.Enabled = true;
            btnNhanVien.Enabled = true;
            btnHoaDon.Enabled = true;
            btnPhanCa.Enabled = true;
            btnCaLamViec.Enabled = true;
            btnBaoCaoThongKe.Enabled = true;
            btnLichLamViec.Enabled = true;
            btnThongTinCaNhan.Enabled = true;

            btnNhaCungCap.Enabled = true;
            btnPhieuNhap.Enabled = true;
            btnPhieuChi.Enabled = true;

            // Mờ các chức năng quản lý KHÔNG được phép
            btnTaiKhoan.Enabled = false;
            btnLichSuHoatDong.Enabled = false;
        }

        public void QuyenNhanVienBanHang()
        {
            pnlSidebar.Visible = true;
            btnDangNhap.Enabled = false;
            btnDangXuat.Enabled = true;
            btnDoiMatKhau.Enabled = true;

            // Chỉ sáng các nghiệp vụ bán hàng
            btnKhachHang.Enabled = true;
            btnHoaDon.Enabled = true;
            btnSanPham.Enabled = true;
            btnLichLamViec.Enabled = true;
            btnThongTinCaNhan.Enabled = true;

            // Mờ các nghiệp vụ không liên quan
            btnLoaiSanPham.Enabled = false;
            btnHangSanXuat.Enabled = false;
            btnNhanVien.Enabled = false;
            btnNhaCungCap.Enabled = false;
            btnPhieuNhap.Enabled = false;
            btnPhieuChi.Enabled = false;
            btnPhanCa.Enabled = false;
            btnCaLamViec.Enabled = false;
            btnTaiKhoan.Enabled = false;
            btnLichSuHoatDong.Enabled = false;

            btnBaoCaoThongKe.Enabled = true; // Mở ngăn kéo menu Báo cáo

            // MỞ: Báo cáo phục vụ cho việc tư vấn khách hàng
            btnSanPhamBanChay.Enabled = true;
            btnThongKeSanPham.Enabled = true;

            // KHÓA: Các báo cáo chứa dữ liệu mật về tài chính của Chủ/Quản lý
            btnThongKeDoanhThu.Enabled = false;
            btnThongKeTonKho.Enabled = false;
            btnBaoCaoThuChi.Enabled = false;
        }

        public void QuyenNhanVienKho()
        {
            pnlSidebar.Visible = true;
            btnDangNhap.Enabled = false;
            btnDangXuat.Enabled = true;
            btnDoiMatKhau.Enabled = true;

            // Sáng nghiệp vụ kho
            btnSanPham.Enabled = true;
            btnLoaiSanPham.Enabled = true;
            btnHangSanXuat.Enabled = true;
            btnNhaCungCap.Enabled = true;
            btnPhieuNhap.Enabled = true;
            btnLichLamViec.Enabled = true;
            btnThongTinCaNhan.Enabled = true;


            // Mờ các chức năng khác
            btnHoaDon.Enabled = false;
            btnKhachHang.Enabled = false;
            btnNhanVien.Enabled = false;
            btnPhieuChi.Enabled = false;
            btnPhanCa.Enabled = false;
            btnCaLamViec.Enabled = false;
            btnTaiKhoan.Enabled = false;
            btnLichSuHoatDong.Enabled = false;
            btnBaoCaoThongKe.Enabled = false;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            ChuaDangNhap();
            DangNhap();
            ApDungGiaoDien();
        }

        private void lblLienKet_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("https://fit.agu.edu.vn") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở liên kết.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }






        private void frmMain_MdiChildActivate(object sender, EventArgs e)
        {
            // Lấy Form con vừa mới được kích hoạt/mở lên
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null && _currentConfig != null)
            {
                // Tự động nhuộm màu cho nó mà không cần ghi code ở Menu
                ThucHienDoiMau(activeChild, _currentConfig);
            }
        }

        private void btnHeThong_Click(object sender, EventArgs e)
        {
            pnlSubHeThong.Visible = !pnlSubHeThong.Visible;

            //Nếu ngăn kéo Hệ thống ĐANG MỞ -> Ép các ngăn kéo khác PHẢI ĐÓNG LẠI
            if (pnlSubHeThong.Visible == true)
            {
                pnlSubQuanLy.Visible = false; // Đóng Quản lý
                pnlSubBaoCaoThongKe.Visible = false;
                pnlSubTroGiup.Visible = false;
            }
        }

        private void btnQuanLy_Click(object sender, EventArgs e)
        {
            pnlSubQuanLy.Visible = !pnlSubQuanLy.Visible;

            if (pnlSubQuanLy.Visible == true)
            {
                pnlSubHeThong.Visible = false; // Đóng Hệ thống
                pnlSubBaoCaoThongKe.Visible = false;  // Đóng Báo cáo
                pnlSubTroGiup.Visible = false;  // Đóng Trợ giúp
            }
        }

        private void btnBaoCaoThongKe_Click(object sender, EventArgs e)
        {
            pnlSubBaoCaoThongKe.Visible = !pnlSubBaoCaoThongKe.Visible;

            if (pnlSubBaoCaoThongKe.Visible == true)
            {
                pnlSubHeThong.Visible = false; // Đóng Hệ thống
                pnlSubQuanLy.Visible = false;  // Đóng Quản lý
                pnlSubTroGiup.Visible = false;  // Đóng Trợ giúp
            }
        }

        private void btnHuongDanSuDung_Click(object sender, EventArgs e)
        {
            try
            {
                // Trỏ đường dẫn vào thư mục Docs/HuongDanSuDung.html nằm ngay trong thư mục chạy phần mềm
                string path = System.IO.Path.Combine(Application.StartupPath, "Docs", "HuongDanSuDung.html");

                if (System.IO.File.Exists(path))
                {
                    // Mở file HTML bằng trình duyệt mặc định của máy tính (Chrome/Edge/Cốc Cốc)
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(path)
                    {
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("Không tìm thấy file Hướng dẫn sử dụng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThongTinPhanMem_Click(object sender, EventArgs e)
        {
            try
            {
                // Trỏ đường dẫn vào file HTML Thông tin phần mềm
                string path = System.IO.Path.Combine(Application.StartupPath, "Docs", "ThongTinPhanMem.html");

                if (System.IO.File.Exists(path))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(path)
                    {
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("Không tìm thấy trang Thông tin phần mềm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTroGiup_Click(object sender, EventArgs e)
        {
            pnlSubTroGiup.Visible = !pnlSubTroGiup.Visible;

            if (pnlSubTroGiup.Visible == true)
            {
                pnlSubHeThong.Visible = false; // Đóng Hệ thống
                pnlSubQuanLy.Visible = false;  // Đóng Quản lý
                pnlSubBaoCaoThongKe.Visible = false;  // Đóng Báo cáo
            }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            DangNhap();
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn có muốn đăng xuất khỏi tài khoản hiện tại?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes)
            {
                // Đóng tất cả các form con đang mở
                foreach (Form frm in this.MdiChildren)
                {
                    frm.Close();
                }

                if (taiKhoanDangNhap != null)
                {
                    NhatKyHeThong.GhiLog(taiKhoanDangNhap.MaTK, "Đăng xuất khỏi hệ thống");
                }

                taiKhoanDangNhap = null;
                ChuaDangNhap();
                DangNhap(); // Gọi form đăng nhập lên lại
            }
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có ai đang đăng nhập không (phòng hờ)
            if (taiKhoanDangNhap == null)
            {
                MessageBox.Show("Vui lòng đăng nhập để đổi mật khẩu!", "Thông báo");
                return;
            }
            // Khởi tạo Form Đổi mật khẩu và truyền thông tin tài khoản hiện tại qua
            using (frmDoiMatKhau frmDoiMK = new frmDoiMatKhau(taiKhoanDangNhap))
            {
                if (frmDoiMK.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Mật khẩu đã đổi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnThongTinCaNhan_Click(object sender, EventArgs e)
        {
            if (taiKhoanDangNhap == null) return;

            if (thongTinCaNhan == null || thongTinCaNhan.IsDisposed)
            {
                // Khởi tạo và TRUYỀN tài khoản đang đăng nhập qua Form kia
                thongTinCaNhan = new frmThongTinCaNhan(taiKhoanDangNhap);

                thongTinCaNhan.MdiParent = this;
                thongTinCaNhan.Show();
            }
            else
            {
                thongTinCaNhan.Activate();
            }
        }


        private void btnGiaoDien_Click(object sender, EventArgs e)
        {
            // Lấy mã nhân viên từ tài khoản đang đăng nhập
            string maNV = taiKhoanDangNhap.MaNV;

            using (frmCauHinhGiaoDien frm = new frmCauHinhGiaoDien(maNV))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // Gọi hàm cập nhật giao diện ngay lập tức nếu Vy đã viết
                    ApDungGiaoDien();
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult thoat = MessageBox.Show("Bạn có chắc chắn muốn thoát ứng dụng?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (thoat == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnHangSanXuat_Click(object sender, EventArgs e)
        {
            if (hangSanXuat == null || hangSanXuat.IsDisposed)
            {
                hangSanXuat = new frmHangSanXuat(taiKhoanDangNhap.MaTK);
                hangSanXuat.MdiParent = this;
                hangSanXuat.Show();
            }
            else
            {
                hangSanXuat.Activate();
            }
        }

        private void btnLoaiSanPham_Click(object sender, EventArgs e)
        {
            // Kiểm tra: Nếu biến bằng null (chưa tạo lần nào) HOẶC IsDisposed (đã tạo nhưng bị bấm X tắt đi)
            if (loaiSanPham == null || loaiSanPham.IsDisposed)
            {
                loaiSanPham = new frmLoaiSanPham(taiKhoanDangNhap.MaTK);
                loaiSanPham.MdiParent = this;       // Gán form cha
                loaiSanPham.Show();                 // Hiển thị form
            }
            else
            {
                // Nếu form đang mở (bị che lấp), thì đưa nó lên trên cùng
                loaiSanPham.Activate();
            }
        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            if (sanPham == null || sanPham.IsDisposed)
            {
                sanPham = new frmSanPham(taiKhoanDangNhap.MaTK);
                sanPham.MdiParent = this;
                sanPham.Show();
            }
            else
            {
                sanPham.Activate();
            }
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            if (khachHang == null || khachHang.IsDisposed)
            {
                khachHang = new frmKhachHang(taiKhoanDangNhap.MaTK);
                khachHang.MdiParent = this;
                khachHang.Show();
            }
            else
            {
                khachHang.Activate();
            }
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            if (nhanVien == null || nhanVien.IsDisposed)
            {
                nhanVien = new frmNhanVien(taiKhoanDangNhap.MaTK);
                nhanVien.MdiParent = this;
                nhanVien.Show();
            }
            else
            {
                nhanVien.Activate();
            }
        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            if (hoaDon == null || hoaDon.IsDisposed)
            {
                hoaDon = new frmHoaDon(taiKhoanDangNhap.MaTK);
                hoaDon.MdiParent = this;
                hoaDon.Show();
            }
            else
            {
                hoaDon.Activate();
            }
        }

        private void btnNhaCungCap_Click(object sender, EventArgs e)
        {
            if (nhaCungCap == null || nhaCungCap.IsDisposed)
            {
                nhaCungCap = new frmNhaCungCap(taiKhoanDangNhap.MaTK);
                nhaCungCap.MdiParent = this;
                nhaCungCap.Show();
            }
            else
            {
                nhaCungCap.Activate();
            }
        }

        private void btnPhieuNhap_Click(object sender, EventArgs e)
        {
            if (phieuNhap == null || phieuNhap.IsDisposed)
            {
                phieuNhap = new frmPhieuNhap(taiKhoanDangNhap.MaTK);
                phieuNhap.MdiParent = this;
                phieuNhap.Show();
            }
            else
            {
                phieuNhap.Activate();
            }
        }

        private void btnPhieuChi_Click(object sender, EventArgs e)
        {
            if (phieuChi == null || phieuChi.IsDisposed)
            {
                phieuChi = new frmPhieuChi(taiKhoanDangNhap.MaTK);
                phieuChi.MdiParent = this;
                phieuChi.Show();
            }
            else
            {
                phieuChi.Activate();
            }
        }

        private void btnPhanCa_Click(object sender, EventArgs e)
        {
            if (phanCa == null || phanCa.IsDisposed)
            {
                //phanCa = new frmPhanCa();
                phanCa = new frmPhanCa(taiKhoanDangNhap.MaTK);
                phanCa.MdiParent = this;
                phanCa.Show();
            }
            else
            {
                phanCa.Activate();
            }
        }

        private void btnCaLamViec_Click(object sender, EventArgs e)
        {
            if (caLamViec == null || caLamViec.IsDisposed)
            {
                //caLamViec = new frmCaLamViec();
                caLamViec = new frmCaLamViec(taiKhoanDangNhap.MaTK);
                caLamViec.MdiParent = this;
                caLamViec.Show();
            }
            else
            {
                caLamViec.Activate();
            }
        }

        private void btnLichSuHoatDong_Click(object sender, EventArgs e)
        {
            if (lichSuHoatDong == null || lichSuHoatDong.IsDisposed)
            {
                lichSuHoatDong = new frmLichSuHoatDong();
                lichSuHoatDong.MdiParent = this;
                lichSuHoatDong.Show();
            }
            else
            {
                lichSuHoatDong.Activate();
            }
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            if (taiKhoan == null || taiKhoan.IsDisposed)
            {
                taiKhoan = new frmTaiKhoan(taiKhoanDangNhap.MaTK);
                taiKhoan.MdiParent = this;
                taiKhoan.Show();
            }
            else
            {
                taiKhoan.Activate();
            }
        }

        private void btnThongKeSanPham_Click(object sender, EventArgs e)
        {
            if (thongKeSanPham == null || thongKeSanPham.IsDisposed)
            {
                //Lấy tên của người đang đăng nhập hiện tại
                // Lấy tên thật (HoTenNV), nếu không có thì lấy Tên đăng nhập (Username)
                string tenNV = "Chưa xác định";
                if (taiKhoanDangNhap != null)
                {
                    tenNV = taiKhoanDangNhap.NhanVien != null ? taiKhoanDangNhap.NhanVien.HoTenNV : taiKhoanDangNhap.TenDangNhap;
                }

                //Truyền dô Form Thống Kê
                thongKeSanPham = new QuanLyCuaHangMyPham.Reports.frmThongKeSanPham(tenNV);

                thongKeSanPham.MdiParent = this;
                thongKeSanPham.Show();
            }
            else
            {
                thongKeSanPham.Activate();
            }
        }

        private void btnLichLamViec_Click(object sender, EventArgs e)
        {
            if (taiKhoanDangNhap == null) return;

            if (lichLamViec == null || lichLamViec.IsDisposed)
            {
                lichLamViec = new frmLichLamViec(taiKhoanDangNhap);

                lichLamViec.MdiParent = this;
                lichLamViec.Show();
            }
            else
            {
                lichLamViec.Activate();
            }
        }

        private void btnThongKeDoanhThu_Click(object sender, EventArgs e)
        {
            if (thongKeDoanhThu == null || thongKeDoanhThu.IsDisposed)
            {
                //Lấy tên của người đang đăng nhập hiện tại
                // Lấy tên thật (HoTenNV), nếu không có thì lấy Tên đăng nhập (Username)
                string tenNV = "Chưa xác định";
                if (taiKhoanDangNhap != null)
                {
                    tenNV = taiKhoanDangNhap.NhanVien != null ? taiKhoanDangNhap.NhanVien.HoTenNV : taiKhoanDangNhap.TenDangNhap;
                }

                thongKeDoanhThu = new QuanLyCuaHangMyPham.Reports.frmThongKeDoanhThu(tenNV);

                thongKeDoanhThu.MdiParent = this;
                thongKeDoanhThu.Show();
            }
            else
            {
                thongKeDoanhThu.Activate();
            }
        }

        private void btnThongKeTonKho_Click(object sender, EventArgs e)
        {
            if (thongKeTonKho == null || thongKeTonKho.IsDisposed)
            {
                //Lấy tên của người đang đăng nhập hiện tại
                // Lấy tên thật (HoTenNV), nếu không có thì lấy Tên đăng nhập (Username)
                string tenNV = "Chưa xác định";
                if (taiKhoanDangNhap != null)
                {
                    tenNV = taiKhoanDangNhap.NhanVien != null ? taiKhoanDangNhap.NhanVien.HoTenNV : taiKhoanDangNhap.TenDangNhap;
                }

                thongKeTonKho = new QuanLyCuaHangMyPham.Reports.frmThongKeTonKho(tenNV);

                thongKeTonKho.MdiParent = this;
                thongKeTonKho.Show();
            }
            else
            {
                thongKeTonKho.Activate();
            }
        }

        private void btnBaoCaoThuChi_Click(object sender, EventArgs e)
        {
            if (baoCaoThuChi == null || baoCaoThuChi.IsDisposed)
            {
                //Lấy tên của người đang đăng nhập hiện tại
                // Lấy tên thật (HoTenNV), nếu không có thì lấy Tên đăng nhập (Username)
                string tenNV = "Chưa xác định";
                if (taiKhoanDangNhap != null)
                {
                    tenNV = taiKhoanDangNhap.NhanVien != null ? taiKhoanDangNhap.NhanVien.HoTenNV : taiKhoanDangNhap.TenDangNhap;
                }

                baoCaoThuChi = new QuanLyCuaHangMyPham.Reports.frmBaoCaoThuChi(tenNV);

                baoCaoThuChi.MdiParent = this;
                baoCaoThuChi.Show();
            }
            else
            {
                baoCaoThuChi.Activate();
            }
        }

        private void btnSanPhamBanChay_Click(object sender, EventArgs e)
        {
            if (sanPhamBanChay == null || sanPhamBanChay.IsDisposed)
            {
                //Lấy tên của người đang đăng nhập hiện tại
                // Lấy tên thật (HoTenNV), nếu không có thì lấy Tên đăng nhập (Username)
                string tenNV = "Chưa xác định";
                if (taiKhoanDangNhap != null)
                {
                    tenNV = taiKhoanDangNhap.NhanVien != null ? taiKhoanDangNhap.NhanVien.HoTenNV : taiKhoanDangNhap.TenDangNhap;
                }

                sanPhamBanChay = new QuanLyCuaHangMyPham.Reports.frmSanPhamBanChay(tenNV);

                sanPhamBanChay.MdiParent = this;
                sanPhamBanChay.Show();
            }
            else
            {
                sanPhamBanChay.Activate();
            }
        }
    }
}
