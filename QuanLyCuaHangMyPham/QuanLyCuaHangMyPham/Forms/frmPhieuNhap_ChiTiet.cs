using Microsoft.EntityFrameworkCore;
using QuanLyCuaHangMyPham.Data;
using QuanLyCuaHangMyPham.TienIch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCuaHangMyPham.Forms
{
    public partial class frmPhieuNhap_ChiTiet : Form
    {
        private string _maPN = "";
        private QLCHMPDbContext _context = new QLCHMPDbContext();
        private BindingList<DanhSachPhieuNhap_ChiTiet> phieuNhapChiTiet = new BindingList<DanhSachPhieuNhap_ChiTiet>();
        private CoGianGiaoDien _hoTroCoGianForm;
        private int _maTKDangNhap;

        private const int CB_SETCUEBANNER = 0x1703;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        public frmPhieuNhap_ChiTiet(string MaPN = "", int maTKDangNhap = 0)
        {
            InitializeComponent();
            _maPN = MaPN;
            _hoTroCoGianForm = new CoGianGiaoDien(this);
            _maTKDangNhap = maTKDangNhap;

            // Bắt sự kiện gõ tiền trả để nhảy số Còn nợ tự động
            numSoTienDaTra.ValueChanged += numSoTienDaTra_ValueChanged;
            numSoTienDaTra.KeyUp += numSoTienDaTra_ValueChanged;
        }
        private string SinhMaPhieuNhapTheoNgay()
        {
            string ngayHienTai = DateTime.Now.ToString("ddMMyy");
            string tienTo = "PN" + ngayHienTai;

            var dsMaHomNay = _context.PhieuNhap
                .Where(p => p.MaPN.StartsWith(tienTo))
                .Select(p => p.MaPN).ToList();

            if (dsMaHomNay.Count == 0) return tienTo + "001";

            var maLonNhat = dsMaHomNay.OrderByDescending(m => m).First();
            string soCu = maLonNhat.Substring(tienTo.Length);
            return tienTo + (int.Parse(soCu) + 1).ToString("D3");
        }

        private void LoadDuLieuCu()
        {
            var pn = _context.PhieuNhap.Find(_maPN);
            if (pn != null)
            {
                dtpNgayLap.Value = pn.NgayNhap;
                cboHoTenNV.SelectedValue = pn.MaNV;
                cboTenNCC.SelectedValue = pn.MaNCC;
                txtGhiChu.Text = pn.GhiChu;
                cboPT_ThanhToan.Text = pn.PT_ThanhToan;
                numSoTienDaTra.Value = pn.SoTienDaTra;

                phieuNhapChiTiet.Clear();
                //var dsCT = _context.ChiTietPN.Where(ct => ct.MaPN == _maPN).ToList();
                var dsCT = _context.ChiTietPN.Include(ct => ct.SanPham).Where(ct => ct.MaPN == _maPN).ToList();
                foreach (var item in dsCT)
                {
                    phieuNhapChiTiet.Add(new DanhSachPhieuNhap_ChiTiet
                    {
                        MaPN = item.MaPN,
                        MaSP = item.MaSP,
                        TenSP = item.SanPham?.TenSP,
                        SoLuong = item.SoLuong,
                        GiaNhap = item.GiaNhap,
                        ThanhTien = item.ThanhTien
                    });
                }
                cboHoTenNV.Enabled = false; // Khóa lại không cho đổi người lập
            }
        }
        private void TaiDuLieuLenBang()
        {
            // 1. Nạp nhân viên
            cboHoTenNV.DataSource = _context.NhanVien.ToList();
            cboHoTenNV.ValueMember = "MaNV";
            cboHoTenNV.DisplayMember = "HoTenNV";

            // 2. Nạp nhà cung cấp
            cboTenNCC.DataSource = _context.NhaCungCap.ToList();
            cboTenNCC.ValueMember = "MaNCC";
            cboTenNCC.DisplayMember = "TenNCC";

            //nạp sp
            var dsSP = _context.SanPham.Select(s => new
            {
                s.MaSP,
                HienThi = s.TenSP + " (" + s.MaSP + ")"
            }).ToList();
            cboTenSP.DataSource = dsSP;
            cboTenSP.ValueMember = "MaSP";
            cboTenSP.DisplayMember = "HienThi";
            cboTenSP.SelectedIndex = -1;

            //tồn < 50
            /*var dsSP = _context.SanPham
                .Where(s => s.SLTon < 50) 
                .Select(s => new
                {
                    s.MaSP,
                    HienThi = s.TenSP + " (" + s.MaSP + ")"
                }).ToList();*/

            cboTenSP.DataSource = dsSP;
            cboTenSP.ValueMember = "MaSP";
            cboTenSP.DisplayMember = "HienThi";
            cboTenSP.SelectedIndex = -1;


            // 4. Nạp phương thức thanh toán
            cboPT_ThanhToan.DataSource = _context.PT_ThanhToan.ToList();
            cboPT_ThanhToan.ValueMember = "MaPT";
            cboPT_ThanhToan.DisplayMember = "TenPT";

            // 5. Gắn nguồn dữ liệu cho lưới chi tiết
            dgvDSChiTiet.AutoGenerateColumns = false;
            dgvDSChiTiet.DataSource = phieuNhapChiTiet;
        }
        private void CapNhatTongTien()
        {
            decimal tongChiPhi = phieuNhapChiTiet.Sum(x => x.ThanhTien);
            lblTongChiPhi.Text = tongChiPhi.ToString("N0");

            decimal daTra = numSoTienDaTra.Value;
            decimal conNo = tongChiPhi - daTra;
            if (conNo < 0) conNo = 0;

            lblConNo.Text = conNo.ToString("N0");

            // Tự động phân loại trạng thái công nợ
            if (tongChiPhi == 0) txtTrangThai.Text = "";
            else if (daTra == 0) txtTrangThai.Text = "Chưa thanh toán";
            else if (daTra < tongChiPhi) txtTrangThai.Text = "Còn nợ";
            else txtTrangThai.Text = "Đã thanh toán đủ";

            btnLuuPN.Enabled = phieuNhapChiTiet.Count > 0;
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPhieuNhap_ChiTiet_Load(object sender, EventArgs e)
        {
           
            cboTenSP.DropDownStyle = ComboBoxStyle.DropDown;
            
                

            // Gán chữ mờ hiển thị
            SendMessage(cboTenSP.Handle, CB_SETCUEBANNER, (IntPtr)0, "Nhập từ khóa để tìm kiếm...");

            TaiDuLieuLenBang();

            if (string.IsNullOrEmpty(_maPN))
            {
                txtMaPN.Text = SinhMaPhieuNhapTheoNgay();
                dtpNgayLap.Value = DateTime.Now;
            }
            else
            {
                txtMaPN.Text = _maPN;
                LoadDuLieuCu(); // Hàm này để bốc dữ liệu cũ lên nếu là chế độ Sửa
            }
            CapNhatTongTien();
        }

        private void cboTenSP_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Kiểm tra nếu chưa chọn gì thì thoát
            if (cboTenSP.SelectedValue == null) return;

            try
            {
                // Lấy mã sản phẩm đang chọn
                string maSP = cboTenSP.SelectedValue.ToString();

                // Tìm sản phẩm trong Database để lấy giá nhập mới nhất
                var sp = _context.SanPham.Find(maSP);

                if (sp != null)
                {
                    numGiaNhap.Value = sp.GiaNhap; // Tự động điền đơn giá vào ô nhập
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy thông tin sản phẩm: " + ex.Message);
            }
        }

        private void btnXacNhanNhap_Click(object sender, EventArgs e)
        {
            if (cboTenSP.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //string maSP = cboTenSP.SelectedValue.ToString();
            string maSP = cboTenSP.SelectedValue.ToString().Trim();
            var sanPhamDaCo = phieuNhapChiTiet.FirstOrDefault(x => x.MaSP == maSP);

            if (sanPhamDaCo != null)
            {
                sanPhamDaCo.SoLuong += (int)numSoLuong.Value;
                sanPhamDaCo.ThanhTien = sanPhamDaCo.SoLuong * sanPhamDaCo.GiaNhap;
                phieuNhapChiTiet.ResetBindings();
            }
            else
            {
                string tenGoc = "";
                //var spDB = _context.SanPham.Find(maSP);
                var spDB = _context.SanPham.FirstOrDefault(s => s.MaSP == maSP);
                if (spDB != null) tenGoc = spDB.TenSP;
                else
                {
                    MessageBox.Show("Cảnh báo: Không tìm thấy tên sản phẩm trong DB!");
                }

                phieuNhapChiTiet.Add(new DanhSachPhieuNhap_ChiTiet
                {
                    MaPN = txtMaPN.Text,
                    MaSP = maSP,
                    TenSP = tenGoc,
                    SoLuong = (int)numSoLuong.Value,
                    GiaNhap = numGiaNhap.Value,
                    ThanhTien = numSoLuong.Value * numGiaNhap.Value
                });
            }

            CapNhatTongTien();
            cboTenSP.SelectedIndex = -1;
            numSoLuong.Value = 1;
            cboTenSP.Focus();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSChiTiet.CurrentRow == null) return;
            var item = dgvDSChiTiet.CurrentRow.DataBoundItem as DanhSachPhieuNhap_ChiTiet;
            if (item != null)
            {
                phieuNhapChiTiet.Remove(item);
                CapNhatTongTien();
            }
        }

        private void numSoTienDaTra_ValueChanged(object sender, EventArgs e)
        {
            CapNhatTongTien();
        }

        private void btnLuuPN_Click(object sender, EventArgs e)
        {
            if (cboHoTenNV.SelectedValue == null || cboTenNCC.SelectedValue == null || cboPT_ThanhToan.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ NV, NCC và Thanh toán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var db = _context.Database.BeginTransaction())
            {
                try
                {
                    string hanhDongLog = "";
                    var pn = _context.PhieuNhap.Find(txtMaPN.Text);
                    if (pn == null)
                    {
                        pn = new PhieuNhap { MaPN = txtMaPN.Text };
                        _context.PhieuNhap.Add(pn);
                        hanhDongLog = $"Lập phiếu nhập mới {txtMaPN.Text} từ NCC {cboTenNCC.Text}";
                    }
                    else
                    {
                        // Nội dung log khi CẬP NHẬT (SỬA)
                        hanhDongLog = $"Cập nhật nội dung phiếu nhập {txtMaPN.Text}";
                    }
                    pn.NgayNhap = dtpNgayLap.Value;
                    pn.MaNV = cboHoTenNV.SelectedValue.ToString();
                    pn.MaNCC = cboTenNCC.SelectedValue.ToString();
                    pn.GhiChu = txtGhiChu.Text;
                    pn.PT_ThanhToan = cboPT_ThanhToan.Text; // Lưu tên phương thức
                    pn.TongChiPhi = decimal.Parse(lblTongChiPhi.Text);
                    pn.SoTienDaTra = numSoTienDaTra.Value;
                    pn.TrangThai = txtTrangThai.Text;

                    // Xử lý tồn kho: Hủy cái cũ, nạp cái mới
                    var oldCt = _context.ChiTietPN.Where(c => c.MaPN == pn.MaPN).ToList();
                    foreach (var oldItem in oldCt)
                    {
                        var sp = _context.SanPham.Find(oldItem.MaSP);
                        if (sp != null) sp.SLTon -= oldItem.SoLuong; // Rút lại số lượng đã nhập cũ
                    }
                    _context.ChiTietPN.RemoveRange(oldCt);
                    _context.SaveChanges();
                    foreach (var item in phieuNhapChiTiet)
                    {
                        _context.ChiTietPN.Add(new PhieuNhap_ChiTiet
                        {
                            MaPN = pn.MaPN,
                            MaSP = item.MaSP,
                            SoLuong = item.SoLuong,
                            GiaNhap = item.GiaNhap,
                            ThanhTien = item.ThanhTien
                        });

                        // Cộng tồn kho mới
                        var sp = _context.SanPham.Find(item.MaSP);
                        if (sp != null) sp.SLTon += item.SoLuong;
                    }

                    _context.SaveChanges();
                    db.Commit();
                    NhatKyHeThong.GhiLog(_maTKDangNhap, hanhDongLog);

                    MessageBox.Show("Đã lưu Phiếu Nhập thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    db.Rollback();
                    MessageBox.Show("Lỗi Database: " + (ex.InnerException?.Message ?? ex.Message), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnThemNCCMoi_Click(object sender, EventArgs e)
        {
            frmNhaCungCap frm = new frmNhaCungCap(_maTKDangNhap);
            frm.ShowDialog();

            cboTenNCC.DataSource = _context.NhaCungCap.ToList();
            cboTenNCC.ValueMember = "MaNCC"; cboTenNCC.DisplayMember = "TenNCC";
        }

        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        // Giả sử dtSanPham là DataTable chứa tất cả sản phẩm  lấy từ CSDL
        DataTable dtSanPham;
        private void cboTenSP_TextUpdate(object sender, EventArgs e)
        {
            string tuKhoa = cboTenSP.Text.ToLower();

            var dsLoc = _context.SanPham
                .Where(s => s.TenSP.ToLower().Contains(tuKhoa) || s.MaSP.ToLower().Contains(tuKhoa))
                .Select(s => new
                {
                    s.MaSP,
                    HienThi = s.TenSP + " (" + s.MaSP + ")"
                }).ToList();

            /*var dsLoc = _context.SanPham
                .Where(s => s.SLTon < 50 && (s.TenSP.ToLower().Contains(tuKhoa) || s.MaSP.ToLower().Contains(tuKhoa))) 
                .Select(s => new
                {
                    s.MaSP,
                    HienThi = s.TenSP + " (" + s.MaSP + ")"
                }).ToList();*/


            // 3. Gán lại danh sách đã lọc vào ComboBox
            cboTenSP.DataSource = dsLoc;
            cboTenSP.ValueMember = "MaSP";
            cboTenSP.DisplayMember = "HienThi";

            // 4. Mở dropdown để khách thấy kết quả
            cboTenSP.DroppedDown = true;

            // 5. Giữ lại từ khóa và đặt con trỏ chuột ở cuối dòng để gõ tiếp
            cboTenSP.Text = tuKhoa;
            cboTenSP.SelectionStart = tuKhoa.Length;
            Cursor.Current = Cursors.Default;
        }

        private void dgvDSChiTiet_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvDSChiTiet.Columns[e.ColumnIndex].Name == "STT" && e.RowIndex >= 0)
            {
                // Gán giá trị = vị trí dòng (bắt đầu từ 0) + 1
                e.Value = (e.RowIndex + 1).ToString();
            }
        }

        private void btnInPN_Click(object sender, EventArgs e)
        {
            using (QuanLyCuaHangMyPham.Reports.frmInPhieuNhap frmIn = new QuanLyCuaHangMyPham.Reports.frmInPhieuNhap(_maPN))
            {
                frmIn.ShowDialog();
                NhatKyHeThong.GhiLog(_maTKDangNhap, "In phiếu nhập hàng: " + _maPN);
            }
        }

        private void cboTenSP_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
