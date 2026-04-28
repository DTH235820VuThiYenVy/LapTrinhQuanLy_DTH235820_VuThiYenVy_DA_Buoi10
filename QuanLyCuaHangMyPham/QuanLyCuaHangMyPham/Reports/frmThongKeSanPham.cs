using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.WinForms;
using QuanLyCuaHangMyPham.Data;
using QuanLyCuaHangMyPham.TienIch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCuaHangMyPham.Reports
{
    public partial class frmThongKeSanPham : Form
    {
        private QLCHMPDbContext _context = new QLCHMPDbContext();
        private QLBHDataSet.DanhSachSanPhamDataTable _dtSanPham = new QLBHDataSet.DanhSachSanPhamDataTable();
        private string _tenNguoiLap = "";
        private CoGianGiaoDien _hoTroCoGianForm;
        public frmThongKeSanPham(string tenNV)
        {
            InitializeComponent();
            _tenNguoiLap = tenNV; // Gán tên nhận được vào biến
            _hoTroCoGianForm = new CoGianGiaoDien(this);
        }

        private void frmThongKeSanPham_Load(object sender, EventArgs e)
        {
            LoadDuLieuComboBox();
            TaiDuLieuLenBaoCao();
        }

        private void LoadDuLieuComboBox()
        {
            //Hãng Sản Xuất
            var dsHSX = _context.HangSanXuat.Select(h => new { MaHSX = h.MaHSX, TenHSX = h.TenHSX }).ToList();
            dsHSX.Insert(0, new { MaHSX = "ALL", TenHSX = "Tất cả" });
            cboTenHSX.DataSource = dsHSX;
            cboTenHSX.DisplayMember = "TenHSX";
            cboTenHSX.ValueMember = "MaHSX";

            //Nhà Cung Cấp
            var dsNCC = _context.NhaCungCap.Select(n => new { MaNCC = n.MaNCC, TenNCC = n.TenNCC }).ToList();
            dsNCC.Insert(0, new { MaNCC = "ALL", TenNCC = "Tất cả" });
            cboTenNCC.DataSource = dsNCC;
            cboTenNCC.DisplayMember = "TenNCC";
            cboTenNCC.ValueMember = "MaNCC";

            //Trạng Thái Tồn Kho
            cboSLTon.Items.Clear();
            cboSLTon.Items.Add("Tất cả");
            cboSLTon.Items.Add("Còn nhiều hàng ( > 10 )");
            cboSLTon.Items.Add("Sắp hết hàng ( 1 đến 10 )");
            cboSLTon.Items.Add("Hết hàng ( 0 )");
            cboSLTon.SelectedIndex = 0;
        }
        private void TaiDuLieuLenBaoCao()
        {

            //reportViewer1.ZoomMode = ZoomMode.PageWidth;
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent;
            reportViewer1.ZoomPercent = 100;

            reportViewer1.RefreshReport();

            try
            {
                var dsSanPham = _context.SanPham
                    .Include(p => p.LoaiSanPham)
                    .Include(p => p.HangSanXuat)
                    .Include(p => p.NhaCungCap)
                    .Select(p => new
                    {
                        p.MaSP,
                        p.TenSP,
                        TenLoai = p.LoaiSanPham != null ? p.LoaiSanPham.TenLoai : "",
                        TenHSX = p.HangSanXuat != null ? p.HangSanXuat.TenHSX : "",
                        TenNCC = p.NhaCungCap != null ? p.NhaCungCap.TenNCC : "",
                        MaNCC = p.MaNCC ?? "",
                        p.GiaNhap,
                        p.GiaBan,
                        p.SLTon,
                        p.HinhAnh,
                        TrangThai = p.TrangThai ? "Đang kinh doanh" : "Ngừng kinh doanh",
                        p.MaLoai,
                        p.MaHSX
                    }).ToList();

                _dtSanPham.Clear();
                foreach (var item in dsSanPham)
                {
                    _dtSanPham.AddDanhSachSanPhamRow(
                        item.MaSP, item.TenSP, item.TenLoai, item.TenHSX,
                        item.GiaNhap, item.GiaBan, item.SLTon, item.HinhAnh ?? "",
                        item.TrangThai, item.MaLoai, item.MaHSX, item.MaNCC, item.TenNCC
                    );
                }

                string reportPath = Path.Combine(Application.StartupPath, "Reports", "rptThongKeSanPham.rdlc");
                reportViewer1.LocalReport.ReportPath = reportPath;

                ReportDataSource rds = new ReportDataSource("DanhSachSanPham", (DataTable)_dtSanPham);
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                ReportParameter[] p = new ReportParameter[]
                {
            new ReportParameter("pNguoiLap", _tenNguoiLap),
            new ReportParameter("pTieuChiLoc", "Tất cả sản phẩm (Mặc định)")
                };
                reportViewer1.LocalReport.SetParameters(p);
                // ----------------------------------------------

                reportViewer1.RefreshReport();

                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

                reportViewer1.RefreshReport();


            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                if (ex.InnerException != null) errorMsg += "\nChi tiết: " + ex.InnerException.Message;
                MessageBox.Show("Lỗi nạp dữ liệu báo cáo: " + errorMsg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            try
            {
                var query = _context.SanPham
                    .Include(p => p.LoaiSanPham)
                    .Include(p => p.HangSanXuat)
                    .Include(p => p.NhaCungCap)
                    .AsQueryable();

                string tieuChiLoc = "Tất cả sản phẩm";
                bool hasFilter = false;

                //Hãng sản xuất
                if (cboTenHSX.SelectedIndex > 0 && cboTenHSX.SelectedValue != null)
                {
                    string maHSX = cboTenHSX.SelectedValue.ToString();
                    query = query.Where(p => p.MaHSX == maHSX);
                    tieuChiLoc = hasFilter ? tieuChiLoc + $"\nHãng SX: {cboTenHSX.Text}" : $"Hãng SX: {cboTenHSX.Text}";
                    hasFilter = true;
                }

                //Nhà cung cấp
                if (cboTenNCC.SelectedIndex > 0 && cboTenNCC.SelectedValue != null)
                {
                    string maNCC = cboTenNCC.SelectedValue.ToString();
                    query = query.Where(p => p.MaNCC == maNCC);
                    tieuChiLoc += hasFilter ? $"\nNhà CC: {cboTenNCC.Text}" : $"Nhà CC: {cboTenNCC.Text}";
                    hasFilter = true;
                }

                //Tồn kho
                if (cboSLTon.SelectedIndex > 0)
                {
                    if (cboSLTon.SelectedIndex == 1) // Còn nhiều hàng
                        query = query.Where(p => p.SLTon > 10);
                    else if (cboSLTon.SelectedIndex == 2) // Sắp hết
                        query = query.Where(p => p.SLTon > 0 && p.SLTon <= 10);
                    else if (cboSLTon.SelectedIndex == 3) // Hết hàng
                        query = query.Where(p => p.SLTon == 0);

                    tieuChiLoc += hasFilter ? $"\nTồn kho: {cboSLTon.Text}" : $"Tồn kho: {cboSLTon.Text}";
                }

                var dsSanPham = query.Select(p => new
                {
                    p.MaSP,
                    p.TenSP,
                    TenLoai = p.LoaiSanPham != null ? p.LoaiSanPham.TenLoai : "",
                    TenHSX = p.HangSanXuat != null ? p.HangSanXuat.TenHSX : "",
                    TenNCC = p.NhaCungCap != null ? p.NhaCungCap.TenNCC : "",
                    MaNCC = p.MaNCC ?? "",
                    p.GiaNhap,
                    p.GiaBan,
                    p.SLTon,
                    HinhAnh = p.HinhAnh ?? "",
                    TrangThai = p.TrangThai ? "Đang kinh doanh" : "Ngừng kinh doanh",
                    p.MaLoai,
                    p.MaHSX
                }).ToList();

                _dtSanPham.Clear();
                foreach (var item in dsSanPham)
                {
                    _dtSanPham.AddDanhSachSanPhamRow(
                        item.MaSP, item.TenSP, item.TenLoai, item.TenHSX,
                        item.GiaNhap, item.GiaBan, item.SLTon, item.HinhAnh,
                        item.TrangThai, item.MaLoai, item.MaHSX, item.MaNCC, item.TenNCC
                    );
                }
                ReportParameter[] p = new ReportParameter[]
                {
                    new ReportParameter("pNguoiLap", _tenNguoiLap),
                    new ReportParameter("pTieuChiLoc", tieuChiLoc)
                };

                ReportDataSource rds = new ReportDataSource("DanhSachSanPham", (DataTable)_dtSanPham);

                reportViewer1.LocalReport.SetParameters(p);
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.RefreshReport();

                if (dsSanPham.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm nào khớp với tiêu chí!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lọc dữ liệu: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            if (cboTenHSX.Items.Count > 0) cboTenHSX.SelectedIndex = 0;
            if (cboTenNCC.Items.Count > 0) cboTenNCC.SelectedIndex = 0;
            if (cboSLTon.Items.Count > 0) cboSLTon.SelectedIndex = 0;
            btnLoc.PerformClick();
        }
    }
}
