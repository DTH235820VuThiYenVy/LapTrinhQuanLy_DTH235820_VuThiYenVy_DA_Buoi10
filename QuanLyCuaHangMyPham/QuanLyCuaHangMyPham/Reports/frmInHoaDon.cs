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
    public partial class frmInHoaDon : Form
    {
        private QLCHMPDbContext _context = new QLCHMPDbContext();
        private QLBHDataSet.DanhSachHoaDon_ChiTietDataTable _dtChiTiet = new QLBHDataSet.DanhSachHoaDon_ChiTietDataTable();

        private string _maHD;
        public frmInHoaDon(string maHoaDon)
        {
            InitializeComponent();
            _maHD = maHoaDon;
        }

        private void frmInHoaDon_Load(object sender, EventArgs e)
        {
            try
            {
                var hoaDon = _context.HoaDon
                    .Include(hd => hd.KhachHang)
                    .Include(hd => hd.ChiTietHDs)
                    .ThenInclude(ct => ct.SanPham)
                    .FirstOrDefault(hd => hd.MaHD == _maHD);

                if (hoaDon != null)
                {
                    if (hoaDon.ChiTietHDs == null || hoaDon.ChiTietHDs.Count == 0)
                    {
                        MessageBox.Show("Hóa đơn này chưa có sản phẩm nào, không thể in!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Dừng tiến trình in ngay lập tức
                    }
                    _dtChiTiet.Clear();
                    foreach (var item in hoaDon.ChiTietHDs)
                    {
                        _dtChiTiet.AddDanhSachHoaDon_ChiTietRow(
                            item.MaHD.ToString(), // MaHD
                            item.MaSP,            // MaSP
                            item.SanPham.TenSP,   // TenSP
                            item.SoLuong,         // SoLuong
                            item.GiaBan,          // GiaBan
                            item.SoLuong * item.GiaBan // ThanhTien
                        );
                    }

                    reportViewer1.LocalReport.ReportPath = Path.Combine(Application.StartupPath, "Reports", "rptInHoaDon.rdlc");
                    reportViewer1.LocalReport.DataSources.Clear();
                    //reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DanhSachHoaDon_ChiTiet", (DataTable)_dtChiTiet));
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ChiTietHoaDon", (DataTable)_dtChiTiet));
                    string tenKH = hoaDon.KhachHang != null ? hoaDon.KhachHang.HoTenKH : "Khách mới";
                    string diaChiKH = hoaDon.KhachHang != null ? hoaDon.KhachHang.DiaChi : "";

                    string giamGia = hoaDon.GiamGia.ToString();
                    if (string.IsNullOrEmpty(giamGia)) giamGia = "0";

                    // Dùng toán tử 3 ngôi kiểm tra Null: Nếu trong CSDL bị trống thì để "Chưa xác định", nếu có thì lấy giá trị thật
                    string phuongThuc = string.IsNullOrEmpty(hoaDon.PT_ThanhToan) ? "Chưa xác định" : hoaDon.PT_ThanhToan;

                    ReportParameter[] param = new ReportParameter[]
                    {
                        new ReportParameter("NgayLap", string.Format("Ngày {0} Tháng {1} Năm {2}", hoaDon.NgayLap.Day, hoaDon.NgayLap.Month, hoaDon.NgayLap.Year)),
                        new ReportParameter("NguoiBan_Ten", "ELVIE COSMETIC"),
                        new ReportParameter("NguoiBan_DiaChi", "Long Xuyên, An Giang"),
                        new ReportParameter("NguoiBan_MaSoThue", "1602162070"), 
                        new ReportParameter("NguoiMua_Ten", tenKH),
                        new ReportParameter("NguoiMua_DiaChi", diaChiKH),
                        new ReportParameter("TongTien", "0"), //Sum(ThanhTien) trong bảng -> = 0
                        new ReportParameter("GiamGia", giamGia),
                        new ReportParameter("PT_ThanhToan", phuongThuc)
                    };

                    reportViewer1.LocalReport.SetParameters(param);

                    reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                    reportViewer1.ZoomMode = ZoomMode.Percent;
                    reportViewer1.ZoomPercent = 100;
                    reportViewer1.RefreshReport();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy dữ liệu hóa đơn này trong hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                // Bắt lỗi sâu bên trong của RDLC
                if (ex.InnerException != null)
                {
                    msg += "\n\nNguyên nhân gốc: " + ex.InnerException.Message;
                }
                MessageBox.Show("Chi tiết lỗi:\n" + msg, "Báo lỗi RDLC", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
