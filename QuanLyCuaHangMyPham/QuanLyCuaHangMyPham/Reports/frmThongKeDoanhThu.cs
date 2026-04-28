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
    public partial class frmThongKeDoanhThu : Form
    {
        private QLCHMPDbContext _context = new QLCHMPDbContext();
        private QLBHDataSet.DanhSachHoaDonDataTable _dtHoaDon = new QLBHDataSet.DanhSachHoaDonDataTable();
        private string _tenNguoiLap = "";
        private CoGianGiaoDien _hoTroCoGianForm;
        public frmThongKeDoanhThu(string tenNV)
        {
            InitializeComponent();
            _tenNguoiLap = tenNV; // Gán tên nhân viên đang đăng nhập vào biến
            _hoTroCoGianForm = new CoGianGiaoDien(this);
        }

        private void frmThongKeDoanhThu_Load(object sender, EventArgs e)
        {
            LoadDuLieuComboBox();
            TaiDuLieuLenBaoCao();
            this.reportViewer1.RefreshReport();
        }

        private void LoadDuLieuComboBox()
        {
            // ComboBox Nhân Viên
            var dsNV = _context.NhanVien.Select(nv => new { MaNV = nv.MaNV, HoTenNV = nv.HoTenNV }).ToList();
            dsNV.Insert(0, new { MaNV = "ALL", HoTenNV = "Tất cả" });

            cboHoTenNV.DataSource = dsNV;
            cboHoTenNV.DisplayMember = "HoTenNV";
            cboHoTenNV.ValueMember = "MaNV";

            //ComboBox Khách Hàng
            var dsKH = _context.KhachHang.Select(kh => new { MaKH = kh.MaKH, HoTenKH = kh.HoTenKH }).ToList();
            dsKH.Insert(0, new { MaKH = "ALL", HoTenKH = "Tất cả" });

            cboHoTenKH.DataSource = dsKH;
            cboHoTenKH.DisplayMember = "HoTenKH";
            cboHoTenKH.ValueMember = "MaKH";

            //ComboBox Phương Thức Thanh Toán
            var dsPTTT = _context.PT_ThanhToan.Select(pt => new { MaPT = pt.MaPT, TenPT = pt.TenPT }).ToList();
            dsPTTT.Insert(0, new { MaPT = "ALL", TenPT = "Tất cả" });

            cboPT_ThanhToan.DataSource = dsPTTT;
            cboPT_ThanhToan.DisplayMember = "TenPT";
            cboPT_ThanhToan.ValueMember = "MaPT";
        }
        private void TaiDuLieuLenBaoCao()
        {
            // Tự động dãn rộng báo cáo vừa khít với chiều ngang cửa sổ
            reportViewer1.ZoomMode = ZoomMode.PageWidth;

            try
            {
                // Truy vấn dữ liệu Hóa Đơn kèm theo thông tin Nhân Viên và Khách Hàng
                var dsHoaDon = _context.HoaDon
                    .Include(h => h.NhanVien)
                    .Include(h => h.KhachHang)
                    .Select(h => new
                    {
                        h.MaHD,
                        h.NgayLap,
                        // Dùng toán tử ba ngôi kiểm tra null để tránh lỗi crash phần mềm
                        HoTenNV = h.NhanVien != null ? h.NhanVien.HoTenNV : "Không xác định",
                        HoTenKH = h.KhachHang != null ? h.KhachHang.HoTenKH : "Khách vãng lai",
                        TongTien = h.TongTien + h.GiamGia,
                        h.GiamGia,
                        ThanhTien = h.TongTien
                    }).ToList();

                // Làm sạch và đổ dữ liệu vào DataTable của DataSet
                _dtHoaDon.Clear();
                foreach (var item in dsHoaDon)
                {
                    // LƯU Ý: Thứ tự truyền biến vào hàm AddDanhSachHoaDonRow phải khớp
                    // với thứ tự các cột mà bạn đã tạo trong file QLBHDataSet.xsd
                    _dtHoaDon.AddDanhSachHoaDonRow(
                        item.MaHD,
                        item.NgayLap,
                        item.HoTenNV,
                        item.HoTenKH,
                        item.TongTien, // Ép kiểu nếu CSDL là Decimal mà DataSet là Double
                        item.GiamGia,
                        item.ThanhTien
                    );
                }

                // Xác định đường dẫn file báo cáo .rdlc
                string reportPath = Path.Combine(Application.StartupPath, "Reports", "rptThongKeDoanhThu.rdlc");
                reportViewer1.LocalReport.ReportPath = reportPath;

                // Cấu hình ReportDataSource (Tên "DanhSachHoaDon" phải gõ giống y hệt tên bảng trong RDLC)
                ReportDataSource rds = new ReportDataSource();
                rds.Name = "DanhSachHoaDon";
                rds.Value = _dtHoaDon;

                // TRUYỀN THAM SỐ NGƯỜI LẬP VÀO BÁO CÁO
                ReportParameter p = new ReportParameter("pNguoiLap", _tenNguoiLap);
                reportViewer1.LocalReport.SetParameters(new ReportParameter[] { p });

                // Xóa và nạp lại nguồn dữ liệu cho ReportViewer
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                // Cấu hình chế độ hiển thị mặc định (Layout in ấn)
                reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                // Xử lý và hiển thị thông báo lỗi an toàn
                string errorMsg = ex.Message;
                if (ex.InnerException != null)
                    errorMsg += "\nChi tiết: " + ex.InnerException.Message;

                MessageBox.Show("Lỗi nạp dữ liệu báo cáo doanh thu: \n" + errorMsg, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            try
            {
                var query = _context.HoaDon
                    .Include(h => h.NhanVien)
                    .Include(h => h.KhachHang)
                    .AsQueryable();

                string tieuChiLoc = $"Từ ngày {dtpTuNgay.Value:dd/MM/yyyy} đến {dtpDenNgay.Value:dd/MM/yyyy}";

                //BẮT BUỘC: Lọc theo thời gian
                DateTime tuNgay = dtpTuNgay.Value.Date;
                DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddTicks(-1); // Lấy đến 23:59:59 của ngày kết thúc

                query = query.Where(h => h.NgayLap >= tuNgay && h.NgayLap <= denNgay);

                //Lọc theo Nhân Viên 
                // (Điều kiện SelectedIndex > 0 giả định dòng đầu tiên [Index 0] là "Tất cả")
                if (cboHoTenNV.SelectedIndex > 0 && cboHoTenNV.SelectedValue != null)
                {
                    string maNV = cboHoTenNV.SelectedValue.ToString();
                    query = query.Where(h => h.MaNV == maNV);
                    tieuChiLoc += $" | Nhân viên: {cboHoTenNV.Text}";
                }

                //Lọc theo Khách hàng
                if (cboHoTenKH.SelectedIndex > 0 && cboHoTenKH.SelectedValue != null)
                {
                    string maKH = cboHoTenKH.SelectedValue.ToString();
                    query = query.Where(h => h.MaKH == maKH);
                    tieuChiLoc += $" | Khách hàng: {cboHoTenKH.Text}";
                }

                //Lọc theo Phương thức thanh toán
                if (cboPT_ThanhToan.SelectedIndex > 0 && cboPT_ThanhToan.SelectedValue != null)
                {
                    string maPT = cboPT_ThanhToan.SelectedValue.ToString();
                    query = query.Where(h => h.PT_ThanhToan == maPT);
                    tieuChiLoc += $" | PTTT: {cboPT_ThanhToan.Text}";
                }
                var dsHoaDon = query.Select(h => new
                {
                    h.MaHD,
                    h.NgayLap,
                    HoTenNV = h.NhanVien != null ? h.NhanVien.HoTenNV : "Không xác định",
                    HoTenKH = h.KhachHang != null ? h.KhachHang.HoTenKH : "Khách vãng lai",
                    TongTien = h.TongTien + h.GiamGia, // Tiền hàng gốc
                    h.GiamGia,
                    ThanhTien = h.TongTien // Tiền thực thu
                }).ToList();

                _dtHoaDon.Clear();
                foreach (var item in dsHoaDon)
                {
                    _dtHoaDon.AddDanhSachHoaDonRow(
                        item.MaHD, item.NgayLap, item.HoTenNV, item.HoTenKH,
                        item.TongTien, item.GiamGia, item.ThanhTien
                    );
                }

                ReportParameter[] p = new ReportParameter[]
                {
            new ReportParameter("pNguoiLap", _tenNguoiLap),
            new ReportParameter("pTieuChiLoc", tieuChiLoc)
                };

                ReportDataSource rds = new ReportDataSource("DanhSachHoaDon", (DataTable)_dtHoaDon);

                reportViewer1.LocalReport.SetParameters(p);
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.RefreshReport();

                if (dsHoaDon.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn nào khớp với tiêu chí lọc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lọc dữ liệu: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {

            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dtpDenNgay.Value = DateTime.Now;

            if (cboHoTenNV.Items.Count > 0) cboHoTenNV.SelectedIndex = 0;
            if (cboHoTenKH.Items.Count > 0) cboHoTenKH.SelectedIndex = 0;
            if (cboPT_ThanhToan.Items.Count > 0) cboPT_ThanhToan.SelectedIndex = 0;

            // lệnh PerformClick() sẽ giả lập việc người dùng vừa lấy chuột bấm vào nút Lọc một lần nữa.
            btnLoc.PerformClick();
        }
    }
}
