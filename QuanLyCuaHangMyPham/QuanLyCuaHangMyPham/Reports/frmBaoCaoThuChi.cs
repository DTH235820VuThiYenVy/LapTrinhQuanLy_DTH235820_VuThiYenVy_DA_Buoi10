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
    public partial class frmBaoCaoThuChi : Form
    {
        private QLCHMPDbContext _context = new QLCHMPDbContext();
        private QLBHDataSet.DanhSachThuChiDataTable _dtThuChi = new QLBHDataSet.DanhSachThuChiDataTable();
        private string _tenNguoiLap = "";
        private CoGianGiaoDien _hoTroCoGianForm;
        public frmBaoCaoThuChi(string tenNV)
        {
            InitializeComponent();
            _tenNguoiLap = tenNV;
            _hoTroCoGianForm = new CoGianGiaoDien(this);
        }

        private void LoadDuLieuComboBox()
        {
            // 1. Đổ dữ liệu Nhân viên
            var dsNV = _context.NhanVien.Select(nv => new { nv.MaNV, nv.HoTenNV }).ToList();
            dsNV.Insert(0, new { MaNV = "ALL", HoTenNV = "Tất cả nhân viên" });
            cboHoTenNV.DataSource = dsNV;
            cboHoTenNV.DisplayMember = "HoTenNV";
            cboHoTenNV.ValueMember = "MaNV";

            // 2. Đổ dữ liệu Phương thức thanh toán
            var dsPT = _context.PT_ThanhToan.Select(pt => new { pt.MaPT, pt.TenPT }).ToList();
            dsPT.Insert(0, new { MaPT = "ALL", TenPT = "Tất cả phương thức" });
            cboPT_ThanhToan.DataSource = dsPT;
            cboPT_ThanhToan.DisplayMember = "TenPT";
            cboPT_ThanhToan.ValueMember = "MaPT";

            cboNoiDung.Items.Clear();
            cboNoiDung.Items.Add("Tất cả mục đích");
            cboNoiDung.Items.Add("Bán hàng");
            cboNoiDung.Items.Add("Nhập hàng");
            cboNoiDung.SelectedIndex = 0;
        }

        private void TaiDuLieuLenBaoCao()
        {
            // Cấu hình hiển thị chuẩn như bên Doanh thu
            reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            reportViewer1.ZoomMode = ZoomMode.Percent;
            reportViewer1.ZoomPercent = 100;

            try
            {
                // Gọi nút Lọc để thực hiện truy vấn mặc định
                btnLoc.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp báo cáo: " + ex.Message);
            }
        }
        private void frmBaoCaoThuChi_Load(object sender, EventArgs e)
        {
            LoadDuLieuComboBox();
            // Mặc định lọc từ đầu tháng đến hiện tại
            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dtpDenNgay.Value = DateTime.Now;

            TaiDuLieuLenBaoCao();
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            try
            {
                string reportPath = Path.Combine(Application.StartupPath, "Reports", "rptBaoCaoThuChi.rdlc");
                reportViewer1.LocalReport.ReportPath = reportPath;
                DateTime tuNgay = dtpTuNgay.Value.Date;
                DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddTicks(-1);
                string tieuChi = $"Từ ngày {tuNgay:dd/MM/yyyy} đến {dtpDenNgay.Value:dd/MM/yyyy}";

                

                // --- LẤY DỮ LIỆU THU (Từ HoaDon) ---
                var queryThu = _context.HoaDon.Include(h => h.NhanVien).AsQueryable();
                queryThu = queryThu.Where(h => h.NgayLap >= tuNgay && h.NgayLap <= denNgay);

                // --- LẤY DỮ LIỆU CHI (Từ PhieuNhap) ---
                var queryChi = _context.PhieuNhap.Include(p => p.NhanVien).AsQueryable();
                queryChi = queryChi.Where(p => p.NgayNhap >= tuNgay && p.NgayNhap <= denNgay);

                // Lọc theo Nhân viên nếu có chọn
                if (cboHoTenNV.SelectedIndex > 0)
                {
                    string maNV = cboHoTenNV.SelectedValue.ToString();
                    queryThu = queryThu.Where(x => x.MaNV == maNV);
                    queryChi = queryChi.Where(x => x.MaNV == maNV);
                }

                // Lọc theo Phương thức thanh toán (So sánh chuỗi vì PT_ThanhToan của bạn là string)
                if (cboPT_ThanhToan.SelectedIndex > 0)
                {
                    string pttt = cboPT_ThanhToan.Text; // Lấy tên phương thức để so sánh
                    queryThu = queryThu.Where(x => x.PT_ThanhToan == pttt);
                    queryChi = queryChi.Where(x => x.PT_ThanhToan == pttt);
                }

                // Thực thi lấy danh sách
                var listThu = queryThu.Select(h => new {
                    Ma = h.MaHD,
                    Ngay = h.NgayLap,
                    Loai = "Thu",
                    NoiDung = "Bán hàng",
                    TienThu = (decimal)h.TongTien, // Ép về decimal
                    TienChi = 0m,
                    NV = h.NhanVien != null ? h.NhanVien.HoTenNV : ""
                }).ToList();

                var listChi = queryChi.Select(p => new {
                    Ma = p.MaPN,
                    Ngay = p.NgayNhap,
                    Loai = "Chi",
                    NoiDung = "Nhập hàng",
                    TienThu = 0m,                  // Chữ 'm' giúp máy hiểu đây là decimal
                    TienChi = (decimal)p.TongChiPhi, // Sử dụng TongChiPhi theo Class của bạn
                    NV = p.NhanVien != null ? p.NhanVien.HoTenNV : ""
                }).ToList();

                // Gộp dữ liệu
                var dsTongHop = listThu.Concat(listChi).OrderBy(x => x.Ngay).ToList();

                // Lọc theo Mục đích (Nếu người dùng chọn "Bán hàng" hoặc "Nhập hàng")
                if (cboNoiDung.SelectedIndex == 1) // Bán hàng
                    dsTongHop = dsTongHop.Where(x => x.Loai == "Thu").ToList();
                else if (cboNoiDung.SelectedIndex == 2) // Nhập hàng
                    dsTongHop = dsTongHop.Where(x => x.Loai == "Chi").ToList();

                // Đổ vào DataTable của DataSet
                _dtThuChi.Clear();
                foreach (var item in dsTongHop)
                {
                    _dtThuChi.AddDanhSachThuChiRow(
                        item.Ma, item.Ngay, item.Loai, item.NoiDung,
                        (decimal)item.TienThu, (decimal)item.TienChi, item.NV
                    );
                }

                // Cấu hình ReportViewer
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DanhSachThuChi", (DataTable)_dtThuChi));
                reportViewer1.LocalReport.SetParameters(new ReportParameter[] {
            new ReportParameter("pNguoiLap", _tenNguoiLap),
            new ReportParameter("pTieuChiLoc", tieuChi)
        });

                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpDenNgay.Value = DateTime.Now;
            cboHoTenNV.SelectedIndex = 0;
            cboPT_ThanhToan.SelectedIndex = 0;
            cboNoiDung.SelectedIndex = 0;
            btnLoc.PerformClick();
        }
    }
}
