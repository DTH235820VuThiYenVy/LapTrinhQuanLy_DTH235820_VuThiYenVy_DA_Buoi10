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
    public partial class frmLichSuHoatDong : Form
    {
        private QLCHMPDbContext _context;
        private CoGianGiaoDien _hoTroCoGianForm;
        public frmLichSuHoatDong()
        {
            InitializeComponent();
            _hoTroCoGianForm = new CoGianGiaoDien(this);

        }
        private void LayDuLieuVaoComboBox()
        {
            try
            {
                _context = new QLCHMPDbContext();

                // Load danh sách nhân viên vào ComboBox để lọc
                cboHoTenNV.DisplayMember = "HoTenNV";
                cboHoTenNV.ValueMember = "MaNV";
                cboHoTenNV.DataSource = _context.NhanVien.ToList();

                // Mặc định không chọn ai cả
                cboHoTenNV.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách nhân viên: " + ex.Message);
            }
        }

        private void TaiDuLieuLenBang()
        {
            try
            {
                _context = new QLCHMPDbContext();

                DateTime tuNgay = new DateTime(DateTime.Now.Year, 1, 1);
                DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);

                //Truy vấn SQL (An toàn, không bị lỗi Null của bảng liên kết)
                var dataGoc = _context.LichSuHoatDong
                    .Where(p => p.ThoiGian >= tuNgay && p.ThoiGian <= denNgay)
                    .OrderByDescending(p => p.ThoiGian)
                    .Select(p => new
                    {
                        p.MaLS,
                        p.ThoiGian,
                        HoTenNV = p.TaiKhoan != null && p.TaiKhoan.NhanVien != null ? p.TaiKhoan.NhanVien.HoTenNV : "Không rõ",
                        p.HanhDong,
                        TenTK = p.TaiKhoan != null ? p.TaiKhoan.TenDangNhap : "Không rõ"
                    })
                    .ToList();

                // BƯỚC 2: Xử lý chuỗi trên RAM
                var dsLog = dataGoc.Select(p => new
                {
                    MaLS = p.MaLS,
                    ThoiGian = p.ThoiGian.ToString("dd/MM/yyyy HH:mm:ss"),
                    HoTenNV = p.HoTenNV,
                    HanhDong = p.HanhDong,
                    TenTK = p.TenTK
                }).ToList();

                dgvDSLSHD.AutoGenerateColumns = false;
                dgvDSLSHD.DataSource = dsLog;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu lịch sử: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmLichSuHoatDong_Load(object sender, EventArgs e)
        {
            dtpTuNgay.Value = DateTime.Now;
            dtpDenNgay.Value = DateTime.Now;

            LayDuLieuVaoComboBox();
            TaiDuLieuLenBang();
        }


        private void btnTimVaLoc_Click(object sender, EventArgs e)
        {
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);
            string tuKhoa = txtTuKhoa.Text.Trim().ToLower();

            if (tuNgay > dtpDenNgay.Value.Date)
            {
                MessageBox.Show("Từ ngày không được lớn hơn Đến ngày!", "Lỗi bộ lọc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _context = new QLCHMPDbContext();

                var query = _context.LichSuHoatDong.AsQueryable();

                // 1. Lọc theo thời gian
                query = query.Where(p => p.ThoiGian >= tuNgay && p.ThoiGian <= denNgay);

                // 2. Lọc theo từ khóa (Hành động)
                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    query = query.Where(p => p.HanhDong.ToLower().Contains(tuKhoa));
                }

                // 3. Lọc theo nhân viên
                if (cboHoTenNV.SelectedIndex != -1 && cboHoTenNV.SelectedValue != null)
                {
                    string maNVSearch = cboHoTenNV.SelectedValue.ToString();
                    query = query.Where(p => p.TaiKhoan.MaNV == maNVSearch);
                }

                // 4. Lấy dữ liệu cơ bản từ SQL (Tránh lỗi Null)
                var dataGoc = query
                    .OrderByDescending(p => p.ThoiGian)
                    .Select(p => new
                    {
                        p.MaLS,
                        p.ThoiGian,
                        HoTenNV = p.TaiKhoan != null && p.TaiKhoan.NhanVien != null ? p.TaiKhoan.NhanVien.HoTenNV : "Không rõ",
                        p.HanhDong,
                        TenTK = p.TaiKhoan != null ? p.TaiKhoan.TenDangNhap : "Không rõ"
                    })
                    .ToList(); // Kéo về RAM ở đây

                // 5. Xử lý định dạng chuỗi trên RAM
                var ketQua = dataGoc.Select(p => new
                {
                    MaLS = p.MaLS,
                    ThoiGian = p.ThoiGian.ToString("dd/MM/yyyy HH:mm:ss"),
                    HoTenNV = p.HoTenNV,
                    HanhDong = p.HanhDong,
                    TenTK = p.TenTK
                }).ToList();

                dgvDSLSHD.AutoGenerateColumns = false; // Đảm bảo đã đặt đúng DataPropertyName
                dgvDSLSHD.DataSource = ketQua;

                if (ketQua.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy dữ liệu nào khớp với yêu cầu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            if (dgvDSLSHD.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Xuất danh sách Lịch sử hoạt động";
            sfd.Filter = "Excel Workbook|*.xlsx";
            sfd.FileName = "LichSuHoatDong_" + DateTime.Now.ToString("dd_MM_yyyy_HHmm") + ".xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Mã LS");
                    dt.Columns.Add("Thời gian");
                    dt.Columns.Add("Người thực hiện");
                    dt.Columns.Add("Hành động");
                    dt.Columns.Add("Tên tài khoản (Username)");

                    // Lấy dữ liệu trực tiếp từ DataSource của DataGridView
                    var list = dgvDSLSHD.DataSource as System.Collections.IEnumerable;
                    foreach (dynamic p in list)
                    {
                        dt.Rows.Add(
                            p.MaLS,
                            p.ThoiGian,
                            p.HoTenNV,
                            p.HanhDong,
                            p.TenTK
                        );
                    }

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add(dt, "LichSuHoatDong");
                        ws.Row(1).Style.Font.Bold = true;
                        ws.Row(1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                        ws.Columns().AdjustToContents();
                        wb.SaveAs(sfd.FileName);
                    }
                    MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi trong quá trình xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            // Reset các công cụ tìm kiếm về trạng thái ban đầu
            dtpTuNgay.Value = DateTime.Now;
            dtpDenNgay.Value = DateTime.Now;
            cboHoTenNV.SelectedIndex = -1;
            txtTuKhoa.Clear();

            // Tải lại bảng
            TaiDuLieuLenBang();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
