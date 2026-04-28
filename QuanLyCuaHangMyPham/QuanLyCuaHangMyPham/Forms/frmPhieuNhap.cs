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
    public partial class frmPhieuNhap : Form
    {
        private QLCHMPDbContext _context = new QLCHMPDbContext();
        private CoGianGiaoDien _hoTroCoGianForm;
        private string _maPN = "";
        private int _maTKDangNhap;

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lParam);
        private const int EM_SETCUEBANNER = 0x1501;
        public frmPhieuNhap(int maTKDangNhap)
        {
            InitializeComponent();
            _hoTroCoGianForm = new CoGianGiaoDien(this);
            _maTKDangNhap = maTKDangNhap;
        }

        private void frmPhieuNhap_Load(object sender, EventArgs e)
        {
            TaiDuLieuLenBang();

            SendMessage(txtTimKiem.Handle, EM_SETCUEBANNER, 1, "Nhập mã PN, tên NV hoặc tên NCC...");
            this.ActiveControl = null;
        }
        private void TaiDuLieuLenBang()
        {
            _context = new QLCHMPDbContext();
            dgvDSPhieuNhap.AutoGenerateColumns = false;

            var ds = _context.PhieuNhap.Select(p => new DanhSachPhieuNhap
            {
                MaPN = p.MaPN,
                MaNV = p.MaNV,
                MaNCC = p.MaNCC,
                HoTenNV = p.NhanVien.HoTenNV,
                TenNCC = p.NhaCungCap.TenNCC,
                NgayNhap = p.NgayNhap,
                TongChiPhi = p.TongChiPhi,
                SoTienDaTra = p.SoTienDaTra,
                TrangThai = p.TrangThai,
                PT_ThanhToan = p.PT_ThanhToan,
                GhiChu = p.GhiChu,
                XemChiTiet = "Xem chi tiết"
            }).OrderByDescending(p => p.NgayNhap).ToList();

            dgvDSPhieuNhap.DataSource = ds;
        }

        private void btnLapPN_Click(object sender, EventArgs e)
        {
            using (frmPhieuNhap_ChiTiet frm = new frmPhieuNhap_ChiTiet())
            {
                frm.ShowDialog();
                TaiDuLieuLenBang();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_maPN))
            {
                MessageBox.Show("Vui lòng chọn một phiếu nhập từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (frmPhieuNhap_ChiTiet frm = new frmPhieuNhap_ChiTiet(_maPN))
            {
                frm.ShowDialog();
                TaiDuLieuLenBang();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_maPN))
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa Phiếu Nhập {_maPN}?\nHành động này sẽ TRỪ ĐI số lượng sản phẩm trong kho.",
                "Cảnh báo quan trọng", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                using (var db = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var pn = _context.PhieuNhap.Find(_maPN);
                        if (pn != null)
                        {
                            var listChiTiet = _context.ChiTietPN.Where(ct => ct.MaPN == _maPN).ToList();

                            // Trừ tồn kho vì hủy phiếu nhập
                            foreach (var ct in listChiTiet)
                            {
                                var sp = _context.SanPham.Find(ct.MaSP);
                                if (sp != null)
                                {
                                    sp.SLTon -= ct.SoLuong;
                                }
                            }

                            _context.ChiTietPN.RemoveRange(listChiTiet);
                            _context.PhieuNhap.Remove(pn);
                            _context.SaveChanges();
                            db.Commit();
                            NhatKyHeThong.GhiLog(_maTKDangNhap, $"Xóa phiếu nhập {_maPN} và trừ tồn kho các sản phẩm liên quan");

                            MessageBox.Show("Xóa phiếu nhập và trừ tồn kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _maPN = "";
                            TaiDuLieuLenBang();
                        }
                    }
                    catch (Exception ex)
                    {
                        db.Rollback();
                        MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void dgvDSPhieuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var value = dgvDSPhieuNhap.Rows[e.RowIndex].Cells["MaPN"].Value;
            if (value != null) _maPN = value.ToString();
        }

        private void dgvDSPhieuNhap_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvDSPhieuNhap.Columns[e.ColumnIndex].Name == "XemChiTiet")
            {
                var cellValue = dgvDSPhieuNhap.Rows[e.RowIndex].Cells["MaPN"].Value;
                if (cellValue != null)
                {
                    string ma = cellValue.ToString();
                    using (frmPhieuNhap_ChiTiet frm = new frmPhieuNhap_ChiTiet(ma))
                    {
                        frm.ShowDialog();
                        TaiDuLieuLenBang();
                    }
                }
            }
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date;

            if (tuNgay > denNgay)
            {
                MessageBox.Show("Từ ngày không được lớn hơn Đến ngày!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _context = new QLCHMPDbContext();
            var dsLoc = _context.PhieuNhap
                .Where(p => p.NgayNhap.Date >= tuNgay && p.NgayNhap.Date <= denNgay)
                .Select(p => new DanhSachPhieuNhap
                {
                    MaPN = p.MaPN,
                    MaNV = p.MaNV,
                    MaNCC = p.MaNCC,
                    HoTenNV = p.NhanVien.HoTenNV,
                    TenNCC = p.NhaCungCap.TenNCC,
                    NgayNhap = p.NgayNhap,
                    TongChiPhi = p.TongChiPhi,
                    SoTienDaTra = p.SoTienDaTra,
                    TrangThai = p.TrangThai,
                    PT_ThanhToan = p.PT_ThanhToan,
                    GhiChu = p.GhiChu,
                    XemChiTiet = "Xem chi tiết"
                })
                .OrderByDescending(p => p.NgayNhap).ToList();

            dgvDSPhieuNhap.DataSource = dsLoc;
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Xuất dữ liệu Phiếu Nhập ra Excel";
            sfd.Filter = "Tập tin Excel|*.xlsx";
            sfd.FileName = "BaoCao_PhieuNhap_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var dsPN = _context.PhieuNhap.Select(p => new
                        {
                            p.MaPN,
                            p.MaNV,
                            TenNV = p.NhanVien.HoTenNV,
                            p.MaNCC,
                            TenNCC = p.NhaCungCap.TenNCC,
                            p.NgayNhap,
                            p.TongChiPhi,
                            p.SoTienDaTra,
                            p.TrangThai,
                            p.PT_ThanhToan,
                            p.GhiChu
                        }).ToList();

                        var wsPN = wb.Worksheets.Add("PhieuNhap");
                        wsPN.Cell(1, 1).InsertTable(dsPN);

                        var dsCT = _context.ChiTietPN.Select(ct => new
                        {
                            ct.MaPN,
                            ct.MaSP,
                            TenSP = ct.SanPham.TenSP,
                            ct.SoLuong,
                            ct.GiaNhap,
                            ct.ThanhTien
                        }).ToList();

                        var wsCT = wb.Worksheets.Add("PhieuNhap_ChiTiet");
                        wsCT.Cell(1, 1).InsertTable(dsCT);

                        foreach (var ws in wb.Worksheets)
                        {
                            ws.Columns().AdjustToContents();
                            var header = ws.Row(1);
                            header.Style.Font.Bold = true;
                            header.Style.Fill.BackgroundColor = XLColor.FromHtml("#16a085");
                            header.Style.Font.FontColor = XLColor.White;
                        }

                        wb.SaveAs(sfd.FileName);
                    }
                    NhatKyHeThong.GhiLog(_maTKDangNhap, "Xuất danh sách Phiếu nhập ra tập tin Excel");
                    MessageBox.Show("Xuất báo cáo thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show("Lỗi xuất file: " + ex.Message); }
            }
        }

        private void btnNhap_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Tập tin Excel|*.xlsx;*.xls" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                int thanhCongPN = 0, dongLoiPN = 0, thanhCongCT = 0, dongLoiCT = 0;
                StringBuilder chiTietLoi = new StringBuilder();

                try
                {
                    using (XLWorkbook workbook = new XLWorkbook(ofd.FileName))
                    {
                        // 1. NHẬP PHIẾU NHẬP
                        var wsPN = workbook.Worksheet("PhieuNhap");
                        foreach (var row in wsPN.RowsUsed().Skip(1))
                        {
                            _context = new QLCHMPDbContext();
                            try
                            {
                                string maPN = row.Cell(1).GetFormattedString().Trim();
                                string maNV = row.Cell(2).GetFormattedString().Trim();
                                string maNCC = row.Cell(4).GetFormattedString().Trim();

                                if (string.IsNullOrEmpty(maPN) || string.IsNullOrEmpty(maNV) || string.IsNullOrEmpty(maNCC))
                                    throw new Exception("Mã PN, NV, NCC không được để trống.");

                                if (!_context.NhanVien.Any(nv => nv.MaNV == maNV)) throw new Exception($"Mã NV '{maNV}' không tồn tại.");
                                if (!_context.NhaCungCap.Any(ncc => ncc.MaNCC == maNCC)) throw new Exception($"Mã NCC '{maNCC}' không tồn tại.");

                                if (!_context.PhieuNhap.Any(p => p.MaPN == maPN))
                                {
                                    string ngayNhapStr = row.Cell(6).GetFormattedString().Trim();

                                    // 1. Khai báo biến ngayNhap trước khi xài
                                    DateTime ngayNhap;

                                    // 2. Đổi ngayLapStr thành ngayNhapStr ở đây nè Vy
                                    if (!DateTime.TryParseExact(ngayNhapStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngayNhap))
                                    {
                                        if (!DateTime.TryParse(ngayNhapStr, out ngayNhap))
                                            ngayNhap = DateTime.Now; // Nếu sai quá thì cho ngày hiện tại
                                    }

                                    decimal.TryParse(row.Cell(7).GetFormattedString(), out decimal tongChiPhi);
                                    decimal.TryParse(row.Cell(8).GetFormattedString(), out decimal daTra);

                                    PhieuNhap pn = new PhieuNhap
                                    {
                                        MaPN = maPN,
                                        MaNV = maNV,
                                        MaNCC = maNCC,
                                        NgayNhap = ngayNhap, // Giờ biến ngayNhap đã có dữ liệu xịn
                                        TongChiPhi = tongChiPhi,
                                        SoTienDaTra = daTra,
                                        TrangThai = row.Cell(9).GetFormattedString().Trim(),
                                        PT_ThanhToan = row.Cell(10).GetFormattedString().Trim(),
                                        GhiChu = row.Cell(11).GetFormattedString().Trim()
                                    };

                                    _context.PhieuNhap.Add(pn);
                                    _context.SaveChanges();
                                    thanhCongPN++;
                                }
                            }
                            catch (Exception ex)
                            {
                                dongLoiPN++;
                                chiTietLoi.AppendLine($"- [Sheet PhieuNhap] Dòng {row.RowNumber()}: {(ex.InnerException != null ? ex.InnerException.Message : ex.Message)}");
                            }
                        }

                        // 2. NHẬP CHI TIẾT
                        var wsCT = workbook.Worksheet("PhieuNhap_ChiTiet");
                        foreach (var row in wsCT.RowsUsed().Skip(1))
                        {
                            _context = new QLCHMPDbContext();
                            try
                            {
                                string maPN = row.Cell(1).GetFormattedString().Trim();
                                string maSP = row.Cell(2).GetFormattedString().Trim();

                                if (!_context.PhieuNhap.Any(h => h.MaPN == maPN)) throw new Exception($"Phiếu nhập '{maPN}' không tồn tại.");
                                if (!_context.SanPham.Any(sp => sp.MaSP == maSP)) throw new Exception($"Sản phẩm '{maSP}' không tồn tại.");

                                if (!_context.ChiTietPN.Any(ct => ct.MaPN == maPN && ct.MaSP == maSP))
                                {
                                    int.TryParse(row.Cell(4).GetFormattedString(), out int soLuong);
                                    decimal.TryParse(row.Cell(5).GetFormattedString(), out decimal giaNhap);
                                    decimal.TryParse(row.Cell(6).GetFormattedString(), out decimal thanhTien);

                                    PhieuNhap_ChiTiet ct = new PhieuNhap_ChiTiet
                                    {
                                        MaPN = maPN,
                                        MaSP = maSP,
                                        SoLuong = soLuong,
                                        GiaNhap = giaNhap,
                                        ThanhTien = thanhTien
                                    };
                                    _context.ChiTietPN.Add(ct);

                                    // Cộng tồn kho
                                    var sp = _context.SanPham.Find(maSP);
                                    if (sp != null) sp.SLTon += soLuong;

                                    _context.SaveChanges();
                                    thanhCongCT++;
                                }
                            }
                            catch (Exception ex)
                            {
                                dongLoiCT++;
                                chiTietLoi.AppendLine($"- [Sheet ChiTiet] Dòng {row.RowNumber()}: {(ex.InnerException != null ? ex.InnerException.Message : ex.Message)}");
                            }
                        }
                    }

                    int tongLoi = dongLoiPN + dongLoiCT;
                    string thongBao = $"Nhập hoàn tất!\n- Thành công: {thanhCongPN} phiếu nhập, {thanhCongCT} chi tiết.\n- Thất bại: {tongLoi} dòng.";
                    if (tongLoi > 0) thongBao += "\n\nLỗi:\n" + chiTietLoi.ToString();
                    if (thanhCongPN > 0)
                    {
                        NhatKyHeThong.GhiLog(_maTKDangNhap, $"Nhập dữ liệu Phiếu nhập từ Excel (Thành công: {thanhCongPN} phiếu, {thanhCongCT} chi tiết)");
                    }
                    MessageBox.Show(thongBao, "Kết quả Import", MessageBoxButtons.OK, tongLoi > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
                    TaiDuLieuLenBang();
                }
                catch (Exception ex) { MessageBox.Show("Lỗi file Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {

            txtTimKiem.Clear();
            dtpTuNgay.Value = DateTime.Now;
            dtpDenNgay.Value = DateTime.Now;

            TaiDuLieuLenBang(); // Lấy lại toàn bộ Data gốc
            this.ActiveControl = null;
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

                var dsTimKiem = _context.PhieuNhap
                    .Where(p => p.MaPN.ToLower().Contains(tuKhoa) ||
                                p.NhanVien.HoTenNV.ToLower().Contains(tuKhoa) ||
                                p.NhaCungCap.TenNCC.ToLower().Contains(tuKhoa))
                    .Select(p => new DanhSachPhieuNhap
                    {
                        MaPN = p.MaPN,
                        MaNV = p.MaNV,
                        MaNCC = p.MaNCC,
                        HoTenNV = p.NhanVien.HoTenNV,
                        TenNCC = p.NhaCungCap.TenNCC,
                        NgayNhap = p.NgayNhap,
                        TongChiPhi = p.TongChiPhi,
                        SoTienDaTra = p.SoTienDaTra,
                        TrangThai = p.TrangThai,
                        PT_ThanhToan = p.PT_ThanhToan,
                        GhiChu = p.GhiChu,
                        XemChiTiet = "Xem chi tiết"
                    })
                    .OrderByDescending(p => p.NgayNhap).ToList();

                if (dsTimKiem.Count == 0)
                {
                    MessageBox.Show($"Không tìm thấy phiếu nhập nào khớp với từ khóa '{txtTimKiem.Text.Trim()}'!", "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDuLieuLenBang();
                    txtTimKiem.Clear();
                    txtTimKiem.Focus();
                    txtTimKiem.SelectAll();
                }
                else
                {
                    dgvDSPhieuNhap.DataSource = dsTimKiem; // Đổ kết quả tìm được lên bảng
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            TaiDuLieuLenBang();
        }

        private void btnInHD_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_maPN))
            {
                MessageBox.Show("Vui lòng click chọn một phiếu nhập từ danh sách để in!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (QuanLyCuaHangMyPham.Reports.frmInPhieuNhap frmIn = new QuanLyCuaHangMyPham.Reports.frmInPhieuNhap(_maPN))
            {
                frmIn.ShowDialog();
                NhatKyHeThong.GhiLog(_maTKDangNhap, "In phiếu nhập hàng: " + _maPN);
            }
        }
    }
}
