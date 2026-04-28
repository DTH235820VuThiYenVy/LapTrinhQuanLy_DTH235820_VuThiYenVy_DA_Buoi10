using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.WinForms;
using QuanLyCuaHangMyPham.Data;
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
    public partial class frmInPhieuNhap : Form
    {
        private QLCHMPDbContext _context = new QLCHMPDbContext();

        private QLBHDataSet.DanhSachPhieuNhap_ChiTietDataTable _dtChiTiet = new QLBHDataSet.DanhSachPhieuNhap_ChiTietDataTable();

        private string _maPN;
        public frmInPhieuNhap(string maPhieuNhap)
        {
            InitializeComponent();
            _maPN = maPhieuNhap;
        }

        private void frmInPhieuNhap_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Lấy dữ liệu từ Database (Bao gồm cả Nhân viên, Nhà cung cấp và Chi tiết)
                var phieuNhap = _context.PhieuNhap
                    .Include(pn => pn.NhanVien)
                    .Include(pn => pn.NhaCungCap)
                    .Include(pn => pn.ChiTietPNs)
                    .ThenInclude(ct => ct.SanPham)
                    .FirstOrDefault(pn => pn.MaPN == _maPN);

                if (phieuNhap != null)
                {
                    // 2. Đổ dữ liệu vào DataTable
                    _dtChiTiet.Clear();
                    foreach (var item in phieuNhap.ChiTietPNs)
                    {
                        _dtChiTiet.AddDanhSachPhieuNhap_ChiTietRow(
                            item.MaPN,
                            item.MaSP,
                            item.SanPham != null ? item.SanPham.TenSP : "Không xác định",
                            item.SoLuong,
                            item.GiaNhap,
                            item.ThanhTien
                        );
                    }

                    // 3. Cấu hình ReportViewer
                    reportViewer1.LocalReport.ReportPath = Path.Combine(Application.StartupPath, "Reports", "rptInPhieuNhap.rdlc");
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DanhSachPhieuNhap_ChiTiet", (DataTable)_dtChiTiet));

                    string tenNCC = phieuNhap.NhaCungCap != null ? phieuNhap.NhaCungCap.TenNCC : "NCC";
                    string diaChiNCC = phieuNhap.NhaCungCap != null ? phieuNhap.NhaCungCap.DiaChi : "";
                    string sdtNCC = phieuNhap.NhaCungCap != null ? phieuNhap.NhaCungCap.SDT : ""; // Hoặc .SDT tùy Model

                    string tenNV = phieuNhap.NhanVien != null ? phieuNhap.NhanVien.HoTenNV : "Không xác định";

                    string ptThanhToan = string.IsNullOrEmpty(phieuNhap.PT_ThanhToan) ? "Chưa xác định" : phieuNhap.PT_ThanhToan;
                    string trangThai = string.IsNullOrEmpty(phieuNhap.TrangThai) ? "Chưa xác định" : phieuNhap.TrangThai;

                    // 5. Truyền Parameters vào Report
                    ReportParameter[] param = new ReportParameter[]
                    {
                        // Nhớ thiết lập kiểu dữ liệu của tất cả Parameter trong RDLC là 'Text' để không bị lỗi ép kiểu nhé
                        new ReportParameter("NgayNhap", string.Format("Ngày {0} Tháng {1} Năm {2}", phieuNhap.NgayNhap.Day, phieuNhap.NgayNhap.Month, phieuNhap.NgayNhap.Year)),
                        new ReportParameter("MaPhieuNhap", phieuNhap.MaPN),
                        new ReportParameter("NhaCungCap_Ten", tenNCC),
                        new ReportParameter("NhaCungCap_DiaChi", diaChiNCC),
                        new ReportParameter("NhaCungCap_SDT", sdtNCC),
                        new ReportParameter("NguoiLapPhieu", tenNV),
                        new ReportParameter("TongTien", phieuNhap.TongChiPhi.ToString()),
                        new ReportParameter("NhaCungCap_MaSoThue", ""), // Tùy chỉnh nếu bảng NCC của bạn có Mã số thuế
                        new ReportParameter("PT_ThanhToan", ptThanhToan),
                        new ReportParameter("TrangThai", trangThai),
                        new ReportParameter("SoTienDaTra", phieuNhap.SoTienDaTra.ToString())
                    };

                    reportViewer1.LocalReport.SetParameters(param);

                    // 6. Hiển thị báo cáo
                    reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                    reportViewer1.ZoomMode = ZoomMode.Percent;
                    reportViewer1.ZoomPercent = 100;
                    reportViewer1.RefreshReport();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy dữ liệu phiếu nhập này trong hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Bắt lỗi sâu của RDLC để dễ debug
                string msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += "\n\nNguyên nhân gốc: " + ex.InnerException.Message;
                }
                MessageBox.Show("Lỗi trong quá trình tạo phiếu nhập:\n" + msg, "Lỗi RDLC", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
