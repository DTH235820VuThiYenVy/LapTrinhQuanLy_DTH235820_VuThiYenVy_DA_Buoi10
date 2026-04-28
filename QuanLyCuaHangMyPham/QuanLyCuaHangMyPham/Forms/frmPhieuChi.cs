using ClosedXML.Excel;
using QuanLyCuaHangMyPham.Data;
using QuanLyCuaHangMyPham.TienIch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCuaHangMyPham.Forms
{
    public partial class frmPhieuChi : Form
    {
        private QLCHMPDbContext _context = new QLCHMPDbContext();
        private CoGianGiaoDien _hoTroCoGianForm;
        private bool _xuLyThem = false;
        private string _maPC = "";
        private int _maTKDangNhap;
        public frmPhieuChi(int maTKDangNhap)
        {
            InitializeComponent();
            _hoTroCoGianForm = new CoGianGiaoDien(this);
            _maTKDangNhap = maTKDangNhap;
        }


        private void LayDuLieuVaoComboBox()
        {
            _context = new QLCHMPDbContext();

            // Load Nhân viên
            cboHoTenNV.DataSource = _context.NhanVien.ToList();
            cboHoTenNV.DisplayMember = "HoTenNV";
            cboHoTenNV.ValueMember = "MaNV";
            cboHoTenNV.SelectedIndex = -1;

            // Load Phương thức thanh toán
            cboPT_ThanhToan.DataSource = _context.PT_ThanhToan.ToList();
            cboPT_ThanhToan.DisplayMember = "TenPT";
            cboPT_ThanhToan.ValueMember = "MaPT";
            cboPT_ThanhToan.SelectedIndex = -1;

            // Load Nhà cung cấp
            cboTenNCC.DataSource = _context.NhaCungCap.ToList();
            cboTenNCC.DisplayMember = "TenNCC";
            cboTenNCC.ValueMember = "MaNCC";
            cboTenNCC.SelectedIndex = -1;
        }

        private void BatTatChucNang(bool giaTri)
        {
            txtMaPC.Enabled = false; // Mã tự sinh nên luôn khóa

            dtpNgayChi.Enabled = giaTri;
            numSoTienChi.Enabled = giaTri;
            cboHoTenNV.Enabled = giaTri;
            cboPT_ThanhToan.Enabled = giaTri;
            txtGhiChu.Enabled = giaTri;
            radChiKhac.Enabled = giaTri;
            radTraNCC.Enabled = giaTri;

            // Chỉ mở ComboBox NCC khi đang ở chế độ thêm/sửa VÀ chọn "Trả NCC"
            bool isTraNCC = giaTri && radTraNCC.Checked;
            cboTenNCC.Enabled = isTraNCC;
            cboMaPN.Enabled = isTraNCC;

            btnLuu.Enabled = giaTri;
            btnHuy.Enabled = giaTri;

            btnThem.Enabled = !giaTri;
            btnSua.Enabled = !giaTri;
            btnXoa.Enabled = !giaTri;
            btnTimKiem.Enabled = !giaTri;
            btnLoc.Enabled = !giaTri;
            btnXuat.Enabled = !giaTri;
            btnNhap.Enabled = !giaTri;
        }

        private void TaiDuLieuLenBang()
        {
            _context = new QLCHMPDbContext();

            var dsPhieuChi = _context.PhieuChi.Select(p => new
            {
                MaPC = p.MaPC,
                MaNV = p.MaNV,
                HoTenNV = p.NhanVien.HoTenNV,
                MaNCC = p.MaNCC,
                TenNCC = p.NhaCungCap != null ? p.NhaCungCap.TenNCC : "Chi phí nội bộ",
                NgayChi = p.NgayChi,
                SoTienChi = p.SoTienChi,
                PT_ThanhToan = p.PT_ThanhToan.TenPT,
                GhiChu = p.GhiChu
            }).OrderByDescending(p => p.NgayChi).ToList();

            dgvDSPhieuChi.AutoGenerateColumns = false;
            dgvDSPhieuChi.DataSource = dsPhieuChi;

            BatTatChucNang(false);

            bool coDuLieu = dsPhieuChi.Count > 0;
            btnXoa.Enabled = coDuLieu;
            btnSua.Enabled = coDuLieu;
            btnXuat.Enabled = coDuLieu;
        }


        private void frmPhieuChi_Load(object sender, EventArgs e)
        {
            LayDuLieuVaoComboBox();
            TaiDuLieuLenBang();

            radChiKhac.CheckedChanged += Rad_CheckedChanged;
            radTraNCC.CheckedChanged += Rad_CheckedChanged;
            //cboTenNCC.SelectedIndexChanged += cboTenNCC_SelectedIndexChanged;
            // Mặc định chọn Chi khác khi mở form
            radChiKhac.Checked = true;
        }

        private void Rad_CheckedChanged(object sender, EventArgs e)
        {
            if (radChiKhac.Checked)
            {
                cboTenNCC.Enabled = false;
                cboMaPN.Enabled = false;
                cboTenNCC.SelectedIndex = -1;
                cboMaPN.DataSource = null;
            }
            else if (radTraNCC.Checked && btnLuu.Enabled) // Chỉ mở khi đang ở chế độ Thêm/Sửa
            {
                cboTenNCC.Enabled = true;
                cboMaPN.Enabled = true;
            }
        }

        private void cboTenNCC_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboMaPN == null) return;

            if (cboTenNCC.SelectedIndex != -1 && cboTenNCC.SelectedValue != null)
            {
                if (!(cboTenNCC.SelectedValue is string maNCC)) return;

                try
                {
                    // Lọc tìm các Phiếu Nhập của NCC này mà số tiền nợ vẫn lớn hơn 0
                    var dsPhieuNhap = _context.PhieuNhap
                        .Where(pn => pn.MaNCC == maNCC && (pn.TongChiPhi - pn.SoTienDaTra) > 0)
                        .ToList();

                    // KIỂM TRA VÀ HIỆN THÔNG BÁO:
                    // cboTenNCC.Focused -> thông báo chỉ hiện khi trực tiếp dùng chuột chọn, 
                    // tránh việc nó bị nhảy thông báo tùm lum lúc form mới mở.
                    if (dsPhieuNhap.Count == 0 && cboTenNCC.Focused)
                    {
                        MessageBox.Show("Nhà cung cấp này hiện tại không có công nợ!\n(Chưa có Phiếu Nhập nào hoặc đã thanh toán xong toàn bộ).",
                                        "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // Đổ dữ liệu vào ô chọn Mã Phiếu Nhập
                    cboMaPN.DisplayMember = "MaPN"; // Khai báo cột hiển thị TRƯỚC
                    cboMaPN.ValueMember = "MaPN";   // Khai báo cột lấy giá trị TRƯỚC
                    cboMaPN.DataSource = dsPhieuNhap; // Đổ Data VÀO SAU CÙNG
                    cboMaPN.SelectedIndex = -1;
                }
                catch { }
            }
            else
            {
                // Khi không chọn NCC nào thì xóa rỗng danh sách mã PN
                cboMaPN.DataSource = null;
            }
        }


        private void dgvDSPhieuChi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvDSPhieuChi.Rows[e.RowIndex];

            string ma = row.Cells["MaPC"].Value.ToString();
            var pc = _context.PhieuChi.Find(ma);

            if (pc != null)
            {
                txtMaPC.Text = pc.MaPC;
                dtpNgayChi.Value = pc.NgayChi;
                numSoTienChi.Value = pc.SoTienChi;
                txtGhiChu.Text = pc.GhiChu;
                cboHoTenNV.SelectedValue = pc.MaNV;
                cboPT_ThanhToan.SelectedValue = pc.MaPT;

                if (pc.MaNCC != null)
                {
                    radTraNCC.Checked = true;
                    cboTenNCC.SelectedValue = pc.MaNCC;
                    if (pc.MaPN != null) cboMaPN.SelectedValue = pc.MaPN;
                }
                else
                {
                    radChiKhac.Checked = true;
                }
            }
            BatTatChucNang(false);
        }



        private void cboMaPN_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Tuyệt chiêu: Lấy thẳng cái Phiếu Nhập đang nằm trên giao diện ra để dùng
                if (cboMaPN.SelectedItem is PhieuNhap pn)
                {
                    // Tính số tiền còn nợ
                    decimal conNo = pn.TongChiPhi - pn.SoTienDaTra;

                    // Gán vào ô số tiền
                    numSoTienChi.Value = conNo > 0 ? conNo : 0;
                }
                else
                {
                    numSoTienChi.Value = 0;
                }
            }
            catch
            {
                numSoTienChi.Value = 0;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            _xuLyThem = true;
            BatTatChucNang(true);

            var dsMa = _context.PhieuChi.Select(p => p.MaPC).ToList();
            txtMaPC.Text = SinhMaTuDong.TuSinhMa(dsMa, "MaPC", "PC", 3);

            // Xóa trắng ô
            numSoTienChi.Value = 1;
            txtGhiChu.Clear();
            dtpNgayChi.Value = DateTime.Now;

            cboHoTenNV.SelectedIndex = -1;
            cboPT_ThanhToan.SelectedIndex = -1;
            radChiKhac.Checked = true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvDSPhieuChi.CurrentRow == null) return;
            _xuLyThem = false;
            BatTatChucNang(true);
            _maPC = dgvDSPhieuChi.CurrentRow.Cells["MaPC"].Value.ToString();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSPhieuChi.CurrentRow == null) return;
            _maPC = dgvDSPhieuChi.CurrentRow.Cells["MaPC"].Value.ToString();
            string tenNCC = dgvDSPhieuChi.CurrentRow.Cells["TenNCC"].Value.ToString();

            // Tìm phiếu chi dưới Database để kiểm tra tính chất của nó
            var pc = _context.PhieuChi.Find(_maPC);
            if (pc == null) return;

            // Lời cảnh báo mặc định cho chi phí nội bộ (điện, nước...)
            string loiCanhBao = $"Bạn có chắc chắn muốn xóa Phiếu chi {_maPC} không?";

            if (pc.MaNCC != null)
            {
                loiCanhBao = $"Cảnh báo:\nPhiếu chi {_maPC} là khoản thanh toán cho nhà cung cấp {tenNCC}.\nXóa phiếu này sẽ làm thay đổi số liệu công nợ của cửa hàng!\n";
            }


            if (MessageBox.Show(loiCanhBao, "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    string maPCXoa = pc.MaPC; // Lưu lại mã
                    decimal tienXoa = pc.SoTienChi;

                    _context.PhieuChi.Remove(pc);
                    _context.SaveChanges();

                    NhatKyHeThong.GhiLog(_maTKDangNhap, $"Xóa phiếu chi {maPCXoa} số tiền {tienXoa:N0}đ");

                    MessageBox.Show("Đã xóa phiếu chi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDuLieuLenBang();

                    // Clear UI 
                    txtMaPC.Clear();
                    numSoTienChi.Value = 1;
                    txtGhiChu.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hệ thống không thể xóa: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra ràng buộc dữ liệu
            if (numSoTienChi.Value <= 0)
            {
                MessageBox.Show("Số tiền chi phải lớn hơn 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cboHoTenNV.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn Nhân viên lập phiếu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (radTraNCC.Checked)
            {
                if (cboTenNCC.SelectedIndex == -1 || cboTenNCC.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn Nhà cung cấp cần trả tiền!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboTenNCC.Focus();
                    return;
                }

                if (cboMaPN.SelectedIndex == -1 || cboMaPN.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn Mã phiếu nhập cần thanh toán!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboMaPN.Focus();
                    return;
                }
            }

            if (cboPT_ThanhToan.SelectedIndex == -1 || cboPT_ThanhToan.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Phương thức thanh toán!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboPT_ThanhToan.Focus();
                return;
            }

            // 2. Dùng Transaction để đảm bảo an toàn
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string hanhDongLog = "";
                    PhieuChi pc;
                    decimal soTienCu = 0;

                    if (_xuLyThem)
                    {
                        pc = new PhieuChi { MaPC = txtMaPC.Text.Trim() };
                        _context.PhieuChi.Add(pc);
                    }
                    else
                    {
                        pc = _context.PhieuChi.Find(_maPC);
                        if (pc == null) return;
                        soTienCu = pc.SoTienChi;
                    }
                    // Bổ sung chi tiết vào nội dung log để dễ quản lý
                    string doiTuong = radTraNCC.Checked ? $"trả nợ NCC {cboTenNCC.Text}" : "chi phí nội bộ";
                    hanhDongLog += $" ({doiTuong}) số tiền {numSoTienChi.Value:N0}đ";


                    pc.NgayChi = dtpNgayChi.Value;
                    pc.SoTienChi = numSoTienChi.Value;
                    pc.MaPT = cboPT_ThanhToan.SelectedValue.ToString().Trim();
                    pc.MaNV = cboHoTenNV.SelectedValue.ToString().Trim();
                    pc.GhiChu = txtGhiChu.Text.Trim();

                    // Xử lý nếu là trả nợ cho Nhà cung cấp
                    if (radTraNCC.Checked)
                    {
                        // Lấy mã và gọt sạch sẽ mọi khoảng trắng thừa
                        string maPN = cboMaPN.SelectedValue.ToString().Trim();

                        pc.MaNCC = cboTenNCC.SelectedValue.ToString().Trim();
                        pc.MaPN = maPN;

                        // DÙNG FirstOrDefault ĐỂ SQL TỰ TÌM KẾT QUẢ (Không dùng Find nữa)
                        var pn = _context.PhieuNhap.FirstOrDefault(p => p.MaPN.Trim() == maPN);

                        if (pn != null)
                        {
                            pn.SoTienDaTra = pn.SoTienDaTra - soTienCu + numSoTienChi.Value;

                            if (pn.SoTienDaTra >= pn.TongChiPhi) pn.TrangThai = "Đã thanh toán đủ";
                            else if (pn.SoTienDaTra > 0) pn.TrangThai = "Còn nợ";
                        }
                        else
                        {
                            MessageBox.Show($"Lỗi: Không tìm thấy Phiếu Nhập [{maPN}] dưới Database!\nVui lòng tải lại dữ liệu.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            dbTransaction.Rollback();
                            return;
                        }
                    }
                    else
                    {
                        pc.MaNCC = null;
                        pc.MaPN = null;
                    }

                    _context.SaveChanges();
                    dbTransaction.Commit();
                    NhatKyHeThong.GhiLog(_maTKDangNhap, hanhDongLog);

                    TienIch.NhatKyHeThong.GhiLog(1, "Đã lưu phiếu chi: " + pc.MaPC);

                    MessageBox.Show("Đã lưu Phiếu chi và cập nhật công nợ thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDuLieuLenBang();
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    MessageBox.Show("Lỗi hệ thống: " + (ex.InnerException?.Message ?? ex.Message), "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Nhập dữ liệu Phiếu chi từ Excel";
            ofd.Filter = "Excel Files|*.xlsx;*.xls";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                int thanhCong = 0, dongLoi = 0;
                StringBuilder chiTietLoi = new StringBuilder();

                try
                {
                    using (XLWorkbook wb = new XLWorkbook(ofd.FileName))
                    {
                        var ws = wb.Worksheet(1);
                        var rows = ws.RowsUsed().Skip(1);

                        foreach (var r in rows)
                        {
                            _context = new QLCHMPDbContext();

                            try
                            {
                                // 1. Đọc dữ liệu theo đúng số thứ tự cột vừa Xuất
                                string maNV = r.Cell(2).GetFormattedString().Trim();        // Cột 2: MÃ NV
                                string maNCC = r.Cell(4).GetFormattedString().Trim();       // Cột 4: MÃ NCC (Có thể trống)
                                string ngayChiStr = r.Cell(6).GetFormattedString().Trim();  // Cột 6: Ngày chi

                                decimal soTien = 0;
                                var cellTien = r.Cell(7);                                   // Cột 7: Số tiền
                                if (cellTien.DataType == XLDataType.Number) soTien = (decimal)cellTien.GetDouble();
                                else decimal.TryParse(cellTien.Value.ToString(), out soTien);

                                string tenPT = r.Cell(8).GetFormattedString().Trim();       // Cột 8: Hình thức
                                string ghiChu = r.Cell(9).GetFormattedString().Trim();      // Cột 9: Ghi chú

                                // 2. Ràng buộc & Dò tìm bằng mã
                                if (string.IsNullOrEmpty(maNV)) throw new Exception("Mã nhân viên bị trống.");
                                var nv = _context.NhanVien.Find(maNV);
                                if (nv == null) throw new Exception($"Không tìm thấy nhân viên mang mã '{maNV}'.");

                                string maNhaCungCapLuu = null;
                                if (!string.IsNullOrEmpty(maNCC)) // Nếu có nhập Mã NCC thì mới dò tìm
                                {
                                    var ncc = _context.NhaCungCap.Find(maNCC);
                                    if (ncc == null) throw new Exception($"Không tìm thấy nhà cung cấp mang mã '{maNCC}'.");
                                    maNhaCungCapLuu = ncc.MaNCC;
                                }

                                if (string.IsNullOrEmpty(tenPT)) throw new Exception("Hình thức thanh toán trống.");
                                var pt = _context.PT_ThanhToan.FirstOrDefault(p => p.TenPT.ToLower() == tenPT.ToLower());
                                if (pt == null) throw new Exception($"Không có phương thức TT nào tên '{tenPT}'.");

                                if (soTien <= 0) throw new Exception("Số tiền chi phải lớn hơn 0.");

                                DateTime ngayChi;
                                if (!DateTime.TryParseExact(ngayChiStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngayChi))
                                {
                                    if (!DateTime.TryParse(ngayChiStr, out ngayChi)) ngayChi = DateTime.Now;
                                }

                                // 3. Lưu vào Database
                                var dsMaHienTai = _context.PhieuChi.Select(p => p.MaPC).ToList();
                                string maMoi = SinhMaTuDong.TuSinhMa(dsMaHienTai, "MaPC", "PC", 3);

                                PhieuChi pc = new PhieuChi
                                {
                                    MaPC = maMoi,
                                    MaNV = nv.MaNV,
                                    MaPT = pt.MaPT,
                                    NgayChi = ngayChi,
                                    SoTienChi = soTien,
                                    MaNCC = maNhaCungCapLuu, // Null nếu chi nội bộ, có giá trị nếu chi cho NCC
                                    GhiChu = ghiChu,
                                    MaPN = null
                                };

                                _context.PhieuChi.Add(pc);
                                _context.SaveChanges();
                                thanhCong++;
                            }
                            catch (Exception ex)
                            {
                                dongLoi++;
                                string errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                                chiTietLoi.AppendLine($"- Dòng {r.RowNumber()}: {errorMsg}");
                            }
                        }
                    }

                    string thongBao = $"Nhập hoàn tất!\n- Thành công: {thanhCong} phiếu chi.\n- Thất bại: {dongLoi} dòng.";
                    if (dongLoi > 0) thongBao += "\n\nChi tiết lỗi:\n" + chiTietLoi.ToString();
                    if (thanhCong > 0)
                    {
                        NhatKyHeThong.GhiLog(_maTKDangNhap, $"Nhập dữ liệu Phiếu chi từ Excel (Thành công: {thanhCong} phiếu)");
                    }
                    MessageBox.Show(thongBao, "Kết quả Import", MessageBoxButtons.OK, dongLoi > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
                    TaiDuLieuLenBang();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hệ thống đọc file Excel: " + ex.Message, "Lỗi file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Xuất danh sách Phiếu chi";
            sfd.Filter = "Excel Workbook|*.xlsx";
            sfd.FileName = "PhieuChi_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Mã PC");           // Cột 1
                    dt.Columns.Add("Mã NV");           // Cột 2 
                    dt.Columns.Add("Nhân viên lập");       // Cột 3
                    dt.Columns.Add("Mã NCC");          // Cột 4 
                    dt.Columns.Add("Đối tượng nhận");  // Cột 5
                    dt.Columns.Add("Ngày chi");        // Cột 6
                    dt.Columns.Add("Số tiền");         // Cột 7
                    dt.Columns.Add("Hình thức");       // Cột 8
                    dt.Columns.Add("Ghi chú");         // Cột 9

                    var list = dgvDSPhieuChi.DataSource as System.Collections.IEnumerable;
                    foreach (dynamic p in list)
                    {
                        dt.Rows.Add(
                            p.MaPC,
                            p.MaNV,
                            p.HoTenNV,
                            p.MaNCC, // Mã NCC lấy từ LINQ
                            p.TenNCC,
                            p.NgayChi.ToString("dd/MM/yyyy"),
                            p.SoTienChi,
                            p.PT_ThanhToan,
                            p.GhiChu
                        );
                    }

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add(dt, "PhieuChi");
                        ws.Row(1).Style.Font.Bold = true;
                        ws.Row(1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                        ws.Columns().AdjustToContents();
                        wb.SaveAs(sfd.FileName);
                    }
                    NhatKyHeThong.GhiLog(_maTKDangNhap, "Xuất danh sách Phiếu chi ra tập tin Excel");
                    MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();
            _context = new QLCHMPDbContext();

            if (string.IsNullOrEmpty(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TaiDuLieuLenBang();
                return;
            }

            var ketQua = _context.PhieuChi.Where(p =>
                p.MaPC.ToLower().Contains(tuKhoa) ||
                p.GhiChu.ToLower().Contains(tuKhoa) ||
                (p.NhaCungCap != null && p.NhaCungCap.TenNCC.ToLower().Contains(tuKhoa))
            ).Select(p => new
            {
                MaPC = p.MaPC,
                MaNV = p.MaNV,
                HoTenNV = p.NhanVien.HoTenNV,
                MaNCC = p.MaNCC,
                TenNCC = p.NhaCungCap != null ? p.NhaCungCap.TenNCC : "Chi phí nội bộ",
                NgayChi = p.NgayChi,
                SoTienChi = p.SoTienChi,
                PT_ThanhToan = p.PT_ThanhToan.TenPT,
                GhiChu = p.GhiChu
            }).ToList();

            if (ketQua.Count == 0)
            {
                MessageBox.Show($"Không tìm thấy phiếu chi nào khớp với từ khóa '{txtTimKiem.Text.Trim()}'!",
                                "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);

                TaiDuLieuLenBang();
                txtTimKiem.Clear();
                txtTimKiem.Focus();
                txtTimKiem.SelectAll();
            }
            else
            {
                // Nếu tìm thấy thì đổ dữ liệu lên bảng
                dgvDSPhieuChi.DataSource = ketQua;
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
            var ketQua = _context.PhieuChi
                .Where(p => p.NgayChi.Date >= tuNgay && p.NgayChi.Date <= denNgay)
                .Select(p => new
                {
                    MaPC = p.MaPC,
                    MaNV = p.MaNV,
                    HoTenNV = p.NhanVien.HoTenNV,
                    MaNCC = p.MaNCC,
                    TenNCC = p.NhaCungCap != null ? p.NhaCungCap.TenNCC : "Chi phí nội bộ",
                    NgayChi = p.NgayChi,
                    SoTienChi = p.SoTienChi,
                    PT_ThanhToan = p.PT_ThanhToan.TenPT,
                    GhiChu = p.GhiChu
                }).ToList();

            dgvDSPhieuChi.DataSource = ketQua;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();

            dtpTuNgay.Value = DateTime.Now;
            dtpDenNgay.Value = DateTime.Now;

            TaiDuLieuLenBang();

            this.ActiveControl = null;
        }
    }
}
