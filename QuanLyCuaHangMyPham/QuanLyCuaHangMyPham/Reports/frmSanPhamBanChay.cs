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
    public partial class frmSanPhamBanChay : Form
    {
        private QLCHMPDbContext _context = new QLCHMPDbContext();
        private QLBHDataSet.DanhSachSPBanChayDataTable _dtSPBanChay = new QLBHDataSet.DanhSachSPBanChayDataTable();
        private CoGianGiaoDien _hoTroCoGianForm;
        private string _tenNguoiLap = "";
        public frmSanPhamBanChay(string tenNV)
        {
            InitializeComponent();
            _tenNguoiLap = tenNV;
            _hoTroCoGianForm = new CoGianGiaoDien(this);
        }

        private void LoadDuLieuComboBox()
        {
            // Đổ dữ liệu Hãng sản xuất
            var dsHSX = _context.HangSanXuat.Select(h => new { h.MaHSX, h.TenHSX }).ToList();
            dsHSX.Insert(0, new { MaHSX = "ALL", TenHSX = "Tất cả các hãng" });

            cboTenHSX.DataSource = dsHSX;
            cboTenHSX.DisplayMember = "TenHSX";
            cboTenHSX.ValueMember = "MaHSX";
        }
        private void btnLoc_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra an toàn cho ComboBox
                if (cboTenHSX.SelectedValue == null) return;

                DateTime tuNgay = dtpTuNgay.Value.Date;
                DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddTicks(-1);
                string maHSX = cboTenHSX.SelectedValue.ToString();

                // 1. Truy vấn
                var query = _context.ChiTietHD
                    .Include(ct => ct.HoaDon)
                    .Include(ct => ct.SanPham)
                    .ThenInclude(sp => sp.HangSanXuat)
                    .Where(ct => ct.HoaDon.NgayLap >= tuNgay && ct.HoaDon.NgayLap <= denNgay)
                    .AsQueryable();

                if (maHSX != "ALL")
                {
                    query = query.Where(ct => ct.SanPham.MaHSX == maHSX);
                }

                // 2. Gom nhóm và tính toán
                var result = query.GroupBy(ct => new {
                    ct.MaSP,
                    ct.SanPham.TenSP,
                    TenHang = ct.SanPham.HangSanXuat != null ? ct.SanPham.HangSanXuat.TenHSX : "N/A"
                })
                    .Select(g => new {
                        MaSP = g.Key.MaSP,
                        TenSP = g.Key.TenSP,
                        TenHSX = g.Key.TenHang,
                        TongSL = g.Sum(ct => ct.SoLuong),
                        TongDoanhThu = (decimal)g.Sum(ct => ct.ThanhTien)
                    });

                // 3. Sắp xếp
                if (radSoLuong.Checked)
                    result = result.OrderByDescending(x => x.TongSL);
                else
                    result = result.OrderByDescending(x => x.TongDoanhThu);

                var dsKetQua = result.Take(10).ToList();

                // 4. Đổ dữ liệu vào DataTable (Lưu ý thứ tự cột)
                _dtSPBanChay.Clear();
                foreach (var item in dsKetQua)
                {
                    _dtSPBanChay.AddDanhSachSPBanChayRow(
                        item.MaSP,
                        item.TenSP,
                        item.TenHSX,
                        item.TongSL,
                        item.TongDoanhThu
                    );
                }

                // 5. Hiển thị Report
                string tieuChi = $"Từ {tuNgay:dd/MM/yyyy} đến {dtpDenNgay.Value:dd/MM/yyyy}";
                if (maHSX != "ALL") tieuChi += $" | Hãng: {cboTenHSX.Text}";
                tieuChi += radSoLuong.Checked ? " | Lọc theo: Số lượng" : " | Lọc theo: Doanh thu";

                // Đảm bảo file rdlc đã được Copy to Output Directory: Copy Always
                reportViewer1.LocalReport.ReportPath = Path.Combine(Application.StartupPath, "Reports", "rptSanPhamBanChay.rdlc");
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DanhSachSPBanChay", (DataTable)_dtSPBanChay));

                ReportParameter[] p = new ReportParameter[] {
            new ReportParameter("pNguoiLap", _tenNguoiLap),
            new ReportParameter("pTieuChiLoc", tieuChi)
        };
                reportViewer1.LocalReport.SetParameters(p);
                reportViewer1.RefreshReport();

                if (dsKetQua.Count == 0) MessageBox.Show("Không có dữ liệu trong khoảng này!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dtpDenNgay.Value = DateTime.Now;
            cboTenHSX.SelectedIndex = 0;
            radSoLuong.Checked = true;
            btnLoc.PerformClick();
        }

        private void frmSanPhamBanChay_Load(object sender, EventArgs e)
        {
            LoadDuLieuComboBox();
            // Mặc định: Từ đầu năm đến nay
            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dtpDenNgay.Value = DateTime.Now;

            // Mặc định chọn sắp xếp theo số lượng
            radSoLuong.Checked = true;

            // Cấu hình hiển thị ReportViewer
            reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            reportViewer1.ZoomMode = ZoomMode.Percent;
            reportViewer1.ZoomPercent = 100;

            btnLoc.PerformClick();
        }
    }
}
