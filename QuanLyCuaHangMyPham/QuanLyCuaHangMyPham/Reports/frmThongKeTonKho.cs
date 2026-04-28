using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.WinForms;
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

namespace QuanLyCuaHangMyPham.Reports
{
    public partial class frmThongKeTonKho : Form
    {
        private QLCHMPDbContext _context = new QLCHMPDbContext();
        private QLBHDataSet.DanhSachTonKhoDataTable _dtTonKho = new QLBHDataSet.DanhSachTonKhoDataTable();
        private string _tenNguoiLap = "";
        private CoGianGiaoDien _hoTroCoGianForm;
        public frmThongKeTonKho(string tenNV)
        {
            InitializeComponent();
            _tenNguoiLap = tenNV;
            _hoTroCoGianForm = new CoGianGiaoDien(this);
        }

        private void frmThongKeTonKho_Load(object sender, EventArgs e)
        {
            LoadDuLieuComboBox();
            TaiDuLieuLenBaoCao();
        }

        private void LoadDuLieuComboBox()
        {
            // 1. Đổ dữ liệu Hãng Sản Xuất
            var dsHSX = _context.HangSanXuat.Select(h => new { h.MaHSX, h.TenHSX }).ToList();
            dsHSX.Insert(0, new { MaHSX = "ALL", TenHSX = "Tất cả" });
            cboTenHSX.DataSource = dsHSX;
            cboTenHSX.DisplayMember = "TenHSX";
            cboTenHSX.ValueMember = "MaHSX";

            // 2. Đổ dữ liệu Nhà Cung Cấp
            var dsNCC = _context.NhaCungCap.Select(n => new { n.MaNCC, n.TenNCC }).ToList();
            dsNCC.Insert(0, new { MaNCC = "ALL", TenNCC = "Tất cả" });
            cboTenNCC.DataSource = dsNCC;
            cboTenNCC.DisplayMember = "TenNCC";
            cboTenNCC.ValueMember = "MaNCC";

            // 3. Trạng Thái Tồn Kho (Tự định nghĩa mức độ)
            cboSLTon.Items.Clear();
            cboSLTon.Items.Add("Tất cả");
            cboSLTon.Items.Add("Hết hàng (0)");
            cboSLTon.Items.Add("Sắp hết hàng (1 - 10)");
            cboSLTon.Items.Add("Còn nhiều hàng (> 10)");
            cboSLTon.SelectedIndex = 0;
        }

        private void TaiDuLieuLenBaoCao()
        {
            try
            {
                // Lấy toàn bộ sản phẩm
                var dsTonKho = _context.SanPham
                    .Include(p => p.LoaiSanPham)
                    .Select(p => new
                    {
                        p.MaSP,
                        p.TenSP,
                        TenLoai = p.LoaiSanPham != null ? p.LoaiSanPham.TenLoai : "",
                        p.SLTon,
                        p.GiaNhap,
                        // TÍNH TOÁN: Giá trị vốn tồn kho
                        GiaTriTonKho = (double)(p.SLTon * p.GiaNhap)
                    }).ToList();

                _dtTonKho.Clear();
                foreach (var item in dsTonKho)
                {
                    _dtTonKho.AddDanhSachTonKhoRow(
                        item.MaSP,
                        item.TenSP,
                        item.TenLoai,
                        item.SLTon,
                        item.GiaNhap,
                        (decimal)item.GiaTriTonKho
                    );
                }

                // Cấu hình Report
                string reportPath = Path.Combine(Application.StartupPath, "Reports", "rptThongKeTonKho.rdlc");
                reportViewer1.LocalReport.ReportPath = reportPath;
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DanhSachTonKho", (DataTable)_dtTonKho));

                // Truyền tham số
                ReportParameter[] p = new ReportParameter[]
                {
                    new ReportParameter("pNguoiLap", _tenNguoiLap),
                    new ReportParameter("pTieuChiLoc", "Tất cả sản phẩm trong kho")
                };
                reportViewer1.LocalReport.SetParameters(p);

                reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        private void btnLoc_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Lấy dữ liệu gốc từ Database (kèm theo các bảng liên quan)
                var query = _context.SanPham
                    .Include(p => p.LoaiSanPham)
                    .Include(p => p.HangSanXuat)
                    .Include(p => p.NhaCungCap)
                    .AsQueryable();

                string tieuChi = "Tất cả sản phẩm";
                bool coLoc = false;

                // 2. Lọc theo Hãng sản xuất
                if (cboTenHSX.SelectedIndex > 0 && cboTenHSX.SelectedValue != null)
                {
                    string maHSX = cboTenHSX.SelectedValue.ToString();
                    query = query.Where(p => p.MaHSX == maHSX);
                    tieuChi = $"Hãng: {cboTenHSX.Text}";
                    coLoc = true;
                }

                // 3. Lọc theo Nhà cung cấp
                if (cboTenNCC.SelectedIndex > 0 && cboTenNCC.SelectedValue != null)
                {
                    string maNCC = cboTenNCC.SelectedValue.ToString();
                    query = query.Where(p => p.MaNCC == maNCC);
                    tieuChi = coLoc ? tieuChi + $" | NCC: {cboTenNCC.Text}" : $"NCC: {cboTenNCC.Text}";
                    coLoc = true;
                }

                // 4. Lọc theo Trạng thái tồn kho
                if (cboSLTon.SelectedIndex > 0)
                {
                    if (cboSLTon.SelectedIndex == 1) // Hết hàng
                        query = query.Where(p => p.SLTon == 0);
                    else if (cboSLTon.SelectedIndex == 2) // Sắp hết (1-10)
                        query = query.Where(p => p.SLTon > 0 && p.SLTon <= 10);
                    else if (cboSLTon.SelectedIndex == 3) // Còn nhiều (>10)
                        query = query.Where(p => p.SLTon > 10);

                    tieuChi = coLoc ? tieuChi + $" | Trạng thái: {cboSLTon.Text}" : $"Trạng thái: {cboSLTon.Text}";
                }

                // 5. Thực thi truy vấn và đẩy vào DataTable
                var dsKetQua = query.Select(p => new
                {
                    p.MaSP,
                    p.TenSP,
                    TenLoai = p.LoaiSanPham != null ? p.LoaiSanPham.TenLoai : "",
                    p.SLTon,
                    p.GiaNhap,
                    // Tính toán giá trị vốn tồn kho
                    GiaTriTonKho = (double)(p.SLTon * p.GiaNhap)
                }).ToList();

                _dtTonKho.Clear();
                foreach (var item in dsKetQua)
                {
                    _dtTonKho.AddDanhSachTonKhoRow(
                        item.MaSP,
                        item.TenSP,
                        item.TenLoai,
                        item.SLTon,
                        item.GiaNhap,
                        (decimal)item.GiaTriTonKho
                    );
                }

                // 6. Truyền tham số và cập nhật ReportViewer
                ReportParameter[] pParams = new ReportParameter[]
                {
            new ReportParameter("pNguoiLap", _tenNguoiLap),
            new ReportParameter("pTieuChiLoc", tieuChi)
                };

                reportViewer1.LocalReport.SetParameters(pParams);
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DanhSachTonKho", (DataTable)_dtTonKho));

                // Cấu hình hiển thị 100% cho cân đối
                reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

                reportViewer1.RefreshReport();

                if (dsKetQua.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm nào khớp với tiêu chí lọc!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lọc: " + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            // Reset các ComboBox về dòng "Tất cả"
            if (cboTenHSX.Items.Count > 0) cboTenHSX.SelectedIndex = 0;
            if (cboTenNCC.Items.Count > 0) cboTenNCC.SelectedIndex = 0;
            if (cboSLTon.Items.Count > 0) cboSLTon.SelectedIndex = 0;

            // Tự động bấm nút Lọc để tải lại toàn bộ dữ liệu
            btnLoc.PerformClick();
        }
    }
}
