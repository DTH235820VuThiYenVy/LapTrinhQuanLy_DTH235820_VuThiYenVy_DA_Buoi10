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
    public partial class frmPhanCa : Form
    {
        private QLCHMPDbContext _context = new QLCHMPDbContext();
        private bool _xuLyThem = false;
        private string _maPC = "";
        private CoGianGiaoDien _hoTroCoGianForm;

        private int _maTKDangNhap; 
        public frmPhanCa(int maTKDangNhap)
        {
            InitializeComponent();
            _hoTroCoGianForm = new CoGianGiaoDien(this);
            _maTKDangNhap = maTKDangNhap;
        }

        private void frmPhanCa_Load(object sender, EventArgs e)
        {
            dtpTuNgay.Value = DateTime.Now.AddDays(-7); // Mặc định xem 7 ngày gần nhất
            dtpDenNgay.Value = DateTime.Now.AddDays(7);

            TaiDuLieuVaoComboBox();
            TaiDuLieuLenBang();
        }
        private void TaiDuLieuVaoComboBox()
        {
            _context = new QLCHMPDbContext();

            // 1. Load Cbo Tên Ca
            var dsCaLam = _context.CaLamViec.ToList();
            cboTenCa.DataSource = dsCaLam;
            cboTenCa.DisplayMember = "TenCa";
            cboTenCa.ValueMember = "MaCa";
            cboTenCa.SelectedIndex = -1;

            // 2. Load Cbo Nhân Viên (Nhập liệu)
            var dsNhanVien = _context.NhanVien.ToList();
            cboHoTenNV.DataSource = dsNhanVien;
            cboHoTenNV.DisplayMember = "HoTenNV";
            cboHoTenNV.ValueMember = "MaNV";
            cboHoTenNV.SelectedIndex = -1;

            // 3. Load Cbo Nhân Viên (Tìm kiếm) - Copy list để không bị đụng DataSource
            var dsNVSearch = _context.NhanVien.ToList();
            cboHoTenNV_TimKiem.DataSource = dsNVSearch;
            cboHoTenNV_TimKiem.DisplayMember = "HoTenNV";
            cboHoTenNV_TimKiem.ValueMember = "MaNV";
            cboHoTenNV_TimKiem.SelectedIndex = -1;
        }

        private void BatTatChucNang(bool giaTri)
        {
            // Các ô nhập liệu
            cboTenCa.Enabled = giaTri;
            cboHoTenNV.Enabled = giaTri;
            dtpNgayLam.Enabled = giaTri;
            txtGhiChu.Enabled = giaTri;

            txtMaPC.Enabled = false;

            // Các nút chức năng
            btnLuu.Enabled = giaTri;
            btnHuy.Enabled = giaTri;
            btnThem.Enabled = !giaTri;
            btnSua.Enabled = !giaTri;
            btnXoa.Enabled = !giaTri;
        }

        private void TaiDuLieuLenBang()
        {
            _context = new QLCHMPDbContext();

            // Join 3 bảng: PhanCa, NhanVien, DM_CaLam để lấy đầy đủ thông tin hiển thị lên Lưới
            var dsPhanCa = _context.PhanCa
                .Select(pc => new
                {
                    MaPC = pc.MaPC,
                    NgayLam = pc.NgayLam,
                    MaNV = pc.MaNV,
                    HoTenNV = pc.NhanVien.HoTenNV,
                    MaCa = pc.MaCa,
                    TenCa = pc.CaLamViec.TenCa,
                    // Ép kiểu TimeSpan thành chuỗi (vd: "08:00")
                    GioBatDau = pc.CaLamViec.GioBatDau.ToString(@"hh\:mm"),
                    GioKetThuc = pc.CaLamViec.GioKetThuc.ToString(@"hh\:mm"),
                    GhiChu = pc.GhiChu
                })
                .OrderByDescending(x => x.NgayLam)
                .ToList();

            dgvDSPhanCa.AutoGenerateColumns = false;
            dgvDSPhanCa.DataSource = dsPhanCa;

            BatTatChucNang(false);

            bool coDuLieu = dsPhanCa.Count > 0;
            btnSua.Enabled = coDuLieu;
            btnXoa.Enabled = coDuLieu;
            btnXuat.Enabled = coDuLieu;
        }
        private void HienThiChiTiet()
        {
            if (dgvDSPhanCa.CurrentRow != null && dgvDSPhanCa.CurrentRow.Index >= 0)
            {
                var row = dgvDSPhanCa.CurrentRow;

                dynamic data = row.DataBoundItem;
                _maPC = data.MaPC;

                txtMaPC.Text = data.MaPC;
                dtpNgayLam.Value = data.NgayLam;
                cboHoTenNV.SelectedValue = data.MaNV;
                cboTenCa.SelectedValue = data.MaCa;
                txtGhiChu.Text = data.GhiChu != null ? data.GhiChu.ToString() : "";
            }
        }



        private void dgvDSPhanCa_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            HienThiChiTiet();
        }



        private void cboHoTenNV_TimKiem_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng có đang chọn một nhân viên thực sự không
            // (Tránh lỗi lúc form mới load lên chưa có dữ liệu)
            if (cboHoTenNV_TimKiem.SelectedIndex != -1 && cboHoTenNV_TimKiem.SelectedValue != null)
            {
                // Vì lúc load ComboBox mình đã cài ValueMember = "MaNV"
                // Nên SelectedValue chính là Mã Nhân Viên. Chỉ cần ép kiểu sang chuỗi rồi gán vào Textbox.
                txtMaNV.Text = cboHoTenNV_TimKiem.SelectedValue.ToString();
            }
            else
            {
                // Nếu nhấn nút "Làm mới" (reset index về -1) thì xóa trắng ô Textbox
                txtMaNV.Clear();
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            _xuLyThem = true;
            BatTatChucNang(true);
            _maPC = "";

            // Xóa trắng dữ liệu cũ trên form
            cboTenCa.SelectedIndex = -1;
            cboHoTenNV.SelectedIndex = -1;
            dtpNgayLam.Value = DateTime.Now;
            txtGhiChu.Clear();

            // Tự động sinh mã PC mới
            var dsMa = _context.PhanCa.Select(p => p.MaPC).ToList();
            txtMaPC.Text = SinhMaTuDong.TuSinhMa(dsMa, "MaPC", "PC", 3);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvDSPhanCa.CurrentRow == null) return;
            HienThiChiTiet();

            _xuLyThem = false;
            BatTatChucNang(true);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSPhanCa.CurrentRow == null) return;
            HienThiChiTiet();

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa lịch làm việc của nhân viên {cboHoTenNV.Text} vào ngày {dtpNgayLam.Value:dd/MM/yyyy}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    var pc = _context.PhanCa.Find(_maPC);
                    if (pc != null)
                    {
                        _context.PhanCa.Remove(pc);
                        _context.SaveChanges();

                        // [GHI LOG]
                        NhatKyHeThong.GhiLog(_maTKDangNhap, $"Xóa phân ca {_maPC} của nhân viên {pc.MaNV} ngày {pc.NgayLam:dd/MM/yyyy}");

                        TaiDuLieuLenBang();
                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // 1. Ràng buộc: Kiểm tra chọn thiếu thông tin
            if (cboTenCa.SelectedIndex == -1 || cboHoTenNV.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn Nhân viên và Ca làm!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maCa = cboTenCa.SelectedValue.ToString();
            string maNV = cboHoTenNV.SelectedValue.ToString();
            DateTime ngayLam = dtpNgayLam.Value.Date;

            // --- XÂY DỰNG TRUY VẤN NỀN TẢNG ---
            // Tìm TẤT CẢ các ca của NV này trong ngày hôm đó
            var queryCaTrongNgay = _context.PhanCa
                .Where(p => p.MaNV == maNV
                       && p.NgayLam.Year == ngayLam.Year
                       && p.NgayLam.Month == ngayLam.Month
                       && p.NgayLam.Day == ngayLam.Day)
                .AsQueryable();

            // CHỈ loại trừ mã hiện tại nếu đang ở chế độ SỬA
            if (!_xuLyThem)
            {
                queryCaTrongNgay = queryCaTrongNgay.Where(p => p.MaPC != _maPC);
            }

            // 2. Ràng buộc: Chống trùng chính xác cùng 1 ca trong 1 ngày
            var trungLich = queryCaTrongNgay.FirstOrDefault(p => p.MaCa == maCa);
            if (trungLich != null)
            {
                MessageBox.Show($"Nhân viên {cboHoTenNV.Text} đã được xếp vào {cboTenCa.Text} trong ngày {ngayLam:dd/MM/yyyy} rồi!", "Trùng lịch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 3. Ràng buộc: Kiểm tra đè khung giờ (Overlap)
            var caDangChon = _context.CaLamViec.Find(maCa);
            TimeSpan gioBD_Moi = caDangChon.GioBatDau;
            TimeSpan gioKT_Moi = caDangChon.GioKetThuc;

            // Lấy thông tin giờ của các ca đã tồn tại
            var dsCaDaCo = queryCaTrongNgay
                .Select(p => new { p.CaLamViec.TenCa, p.CaLamViec.GioBatDau, p.CaLamViec.GioKetThuc })
                .ToList();

            foreach (var ca in dsCaDaCo)
            {
                // Thuật toán: Nếu (Bắt đầu A < Kết thúc B) và (Kết thúc A > Bắt đầu B) thì bị đè giờ
                if (gioBD_Moi < ca.GioKetThuc && gioKT_Moi > ca.GioBatDau)
                {
                    MessageBox.Show($"Trùng khung giờ làm việc!\n\n" +
                                    $"Ca đang chọn: {cboTenCa.Text} ({gioBD_Moi:hh\\:mm} - {gioKT_Moi:hh\\:mm})\n" +
                                    $"Bị đè lên ca: {ca.TenCa} ({ca.GioBatDau:hh\\:mm} - {ca.GioKetThuc:hh\\:mm}) đã có.",
                                    "Lỗi trùng giờ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // 4. Tiến hành Lưu dữ liệu
            try
            {
                if (_xuLyThem)
                {
                    PhanCa pcMoi = new PhanCa
                    {
                        MaPC = txtMaPC.Text.Trim(),
                        MaNV = maNV,
                        MaCa = maCa,
                        NgayLam = ngayLam,
                        GhiChu = txtGhiChu.Text.Trim()
                    };
                    _context.PhanCa.Add(pcMoi);
                    _context.SaveChanges();

                    NhatKyHeThong.GhiLog(_maTKDangNhap, $"Thêm phân ca mới {pcMoi.MaPC} cho NV {maNV} vào {cboTenCa.Text} ngày {ngayLam:dd/MM/yyyy}");
                }
                else
                {
                    var pcSua = _context.PhanCa.Find(_maPC);
                    if (pcSua != null)
                    {
                        pcSua.MaNV = maNV;
                        pcSua.MaCa = maCa;
                        pcSua.NgayLam = ngayLam;
                        pcSua.GhiChu = txtGhiChu.Text.Trim();
                        _context.SaveChanges();

                        NhatKyHeThong.GhiLog(_maTKDangNhap, $"Cập nhật phân ca {_maPC} (NV: {maNV}, Ca: {maCa}, Ngày: {ngayLam:dd/MM/yyyy})");
                    }
                }

                MessageBox.Show("Lưu lịch phân ca thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDuLieuLenBang();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            _xuLyThem = false;
            TaiDuLieuLenBang();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNhap_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Tập tin Excel|*.xlsx;*.xls" };
            openFileDialog.Title = "Nhập dữ liệu Phân ca từ Excel";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                int thanhCong = 0;
                int dongLoi = 0;
                StringBuilder chiTietLoi = new StringBuilder();

                try
                {
                    using (XLWorkbook workbook = new XLWorkbook(openFileDialog.FileName))
                    {
                        var worksheet = workbook.Worksheet(1);
                        var rows = worksheet.RowsUsed().Skip(1); // Bỏ qua tiêu đề dòng 1

                        foreach (var row in rows)
                        {
                            _context = new QLCHMPDbContext();

                            try
                            {
                                // 1. ĐỌC DỮ LIỆU TỪ EXCEL (Theo đúng file Excel mới nhất)
                                // Cột A(1): Ngày | Cột B(2): Mã NV | Cột D(4): Mã Ca | Cột H(8): Ghi chú
                                string maNV = row.Cell(2).GetString().Trim();
                                string maCa = row.Cell(4).GetString().Trim(); // Lấy thẳng Mã Ca từ cột D
                                string ghiChu = row.Cell(8).GetString().Trim(); // Ghi chú đã chuyển sang cột H

                                // 2. XỬ LÝ NGÀY LÀM (Cột A - Cell 1)
                                var cellNgayLam = row.Cell(1);
                                DateTime ngayLam;
                                if (!cellNgayLam.TryGetValue<DateTime>(out ngayLam))
                                {
                                    string ngayLamStr = cellNgayLam.GetString().Trim();
                                    string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "MM/dd/yyyy", "yyyy-MM-dd" };
                                    if (!DateTime.TryParseExact(ngayLamStr, formats, null, System.Globalization.DateTimeStyles.None, out ngayLam))
                                    {
                                        if (!DateTime.TryParse(ngayLamStr, out ngayLam))
                                            throw new Exception($"Ngày '{ngayLamStr}' không hợp lệ.");
                                    }
                                }

                                // 3. KIỂM TRA NHÂN VIÊN VÀ CA LÀM
                                if (!_context.NhanVien.Any(n => n.MaNV == maNV))
                                    throw new Exception($"Mã NV '{maNV}' không tồn tại.");

                                // Vì Excel giờ lưu Mã Ca, ta tìm trực tiếp bằng Mã Ca luôn (Rất an toàn)
                                var caLam = _context.CaLamViec.FirstOrDefault(c => c.MaCa == maCa);
                                if (caLam == null)
                                    throw new Exception($"Mã ca '{maCa}' không tồn tại trong hệ thống.");

                                TimeSpan gioBD_Excel = caLam.GioBatDau;
                                TimeSpan gioKT_Excel = caLam.GioKetThuc;

                                // 4. KIỂM TRA RÀNG BUỘC GIỜ GIẤC (Overlap)
                                var dsCaTrongDB = _context.PhanCa
                                    .Where(p => p.MaNV == maNV
                                           && p.NgayLam.Year == ngayLam.Year
                                           && p.NgayLam.Month == ngayLam.Month
                                           && p.NgayLam.Day == ngayLam.Day)
                                    .Select(p => new { p.MaCa, p.CaLamViec.TenCa, p.CaLamViec.GioBatDau, p.CaLamViec.GioKetThuc })
                                    .ToList();

                                foreach (var ca in dsCaTrongDB)
                                {
                                    // Nếu trùng chính xác Mã Ca đó
                                    if (ca.MaCa == maCa)
                                        throw new Exception($"NV '{maNV}' đã có lịch '{ca.TenCa}' ngày này rồi.");

                                    // Nếu đè khung giờ (ví dụ ca Hành chính đè ca Sáng)
                                    if (gioBD_Excel < ca.GioKetThuc && gioKT_Excel > ca.GioBatDau)
                                    {
                                        throw new Exception($"Trùng giờ với ca '{ca.TenCa}' ({ca.GioBatDau:hh\\:mm}-{ca.GioKetThuc:hh\\:mm}) đã có.");
                                    }
                                }

                                // 5. SINH MÃ PHÂN CA TỰ ĐỘNG
                                var dsMaHienTai = _context.PhanCa.Select(p => p.MaPC).ToList();
                                string maMoi = SinhMaTuDong.TuSinhMa(dsMaHienTai, "MaPC", "PC", 3);

                                // 6. LƯU VÀO DATABASE
                                PhanCa pcMoi = new PhanCa
                                {
                                    MaPC = maMoi,
                                    MaNV = maNV,
                                    MaCa = maCa,
                                    NgayLam = ngayLam,
                                    GhiChu = ghiChu
                                };

                                _context.PhanCa.Add(pcMoi);
                                _context.SaveChanges();
                                thanhCong++;
                            }
                            catch (Exception ex)
                            {
                                dongLoi++;
                                string msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                                chiTietLoi.AppendLine($"- Dòng {row.RowNumber()}: {msg}");
                            }
                        }
                    }

                    // 7. THÔNG BÁO TỔNG KẾT
                    string thongBao = $"Nhập hoàn tất!\n- Thành công: {thanhCong} ca\n- Thất bại: {dongLoi} ca";
                    if (dongLoi > 0) thongBao += "\n\nChi tiết lỗi:\n" + chiTietLoi.ToString();

                    MessageBox.Show(thongBao, "Kết quả Import", MessageBoxButtons.OK,
                                    dongLoi > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                    if (thanhCong > 0)
                        NhatKyHeThong.GhiLog(_maTKDangNhap, $"Import thành công {thanhCong} lịch phân ca từ Excel.");

                    TaiDuLieuLenBang();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            if (dgvDSPhanCa.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog()
            {
                Title = "Xuất lịch phân ca ra Excel",
                Filter = "Excel Workbook|*.xlsx",
                FileName = "LichPhanCa_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataTable dt = new DataTable();
                    // Thêm các cột theo đúng yêu cầu của bạn
                    dt.Columns.Add("Ngày làm");
                    dt.Columns.Add("Mã NV");
                    dt.Columns.Add("Tên NV");
                    dt.Columns.Add("Mã ca"); // Cột này lấy từ data ẩn
                    dt.Columns.Add("Tên ca");
                    dt.Columns.Add("Giờ bắt đầu");
                    dt.Columns.Add("Giờ kết thúc");
                    dt.Columns.Add("Ghi chú");

                    // Lấy danh sách dữ liệu từ DataSource của DataGridView
                    var list = dgvDSPhanCa.DataSource as System.Collections.IEnumerable;

                    foreach (dynamic p in list)
                    {
                        dt.Rows.Add(
                            p.NgayLam.ToString("dd/MM/yyyy"),
                            p.MaNV,
                            p.HoTenNV,
                            p.MaCa, // Móc từ anonymous object đã Select ở hàm TaiDuLieuLenBang
                            p.TenCa,
                            p.GioBatDau,
                            p.GioKetThuc,
                            p.GhiChu
                        );
                    }

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add(dt, "PhanCa");

                        // Trang trí tiêu đề cho đẹp
                        var header = ws.Row(1);
                        header.Style.Font.Bold = true;
                        header.Style.Fill.BackgroundColor = XLColor.LightGreen;
                        header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        // Tự động căn độ rộng cột
                        ws.Columns().AdjustToContents();

                        wb.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Xuất file Excel lịch phân ca thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // [GHI LOG]
                    NhatKyHeThong.GhiLog(_maTKDangNhap, $"Xuất Excel danh sách phân ca ({dt.Rows.Count} dòng)");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            dtpTuNgay.Value = DateTime.Now.AddDays(-7);
            dtpDenNgay.Value = DateTime.Now.AddDays(7);
            txtMaNV.Clear();
            cboHoTenNV_TimKiem.SelectedIndex = -1;

            TaiDuLieuLenBang();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date;
            string keywordNV = txtMaNV.Text.Trim().ToLower();

            var query = _context.PhanCa.Where(pc => pc.NgayLam >= tuNgay && pc.NgayLam <= denNgay);

            if (!string.IsNullOrEmpty(keywordNV))
            {
                query = query.Where(pc => pc.MaNV.ToLower().Contains(keywordNV));
            }

            if (cboHoTenNV_TimKiem.SelectedIndex != -1)
            {
                string cboMaNV = cboHoTenNV_TimKiem.SelectedValue.ToString();
                query = query.Where(pc => pc.MaNV == cboMaNV);
            }

            var ketQua = query.Select(pc => new
            {
                MaPC = pc.MaPC,
                NgayLam = pc.NgayLam,
                MaNV = pc.MaNV,
                HoTenNV = pc.NhanVien.HoTenNV,
                MaCa = pc.MaCa,
                TenCa = pc.CaLamViec.TenCa,
                // Ép kiểu TimeSpan thành chuỗi
                GioBatDau = pc.CaLamViec.GioBatDau.ToString(@"hh\:mm"),
                GioKetThuc = pc.CaLamViec.GioKetThuc.ToString(@"hh\:mm"),
                GhiChu = pc.GhiChu
            }).OrderByDescending(x => x.NgayLam).ToList();

            dgvDSPhanCa.DataSource = ketQua;

            if (ketQua.Count == 0)
            {
                MessageBox.Show("Không tìm thấy ca làm việc nào khớp với điều kiện!", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
