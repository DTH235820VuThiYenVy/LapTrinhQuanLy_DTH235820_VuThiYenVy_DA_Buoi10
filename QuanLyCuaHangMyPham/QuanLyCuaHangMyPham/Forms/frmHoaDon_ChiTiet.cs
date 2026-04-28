using QuanLyCuaHangMyPham.Data;
using QuanLyCuaHangMyPham.Reports;
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
    public partial class frmHoaDon_ChiTiet : Form
    {
        private string _maHoaDon = "";
        private QLCHMPDbContext context = new QLCHMPDbContext();
        private BindingList<DanhSachHoaDon_ChiTiet> hoaDonChiTiet = new BindingList<DanhSachHoaDon_ChiTiet>();
        private CoGianGiaoDien _hoTroCoGianForm;
        private int _maTKDangNhap;

        public frmHoaDon_ChiTiet(string MaHD = "", int maTKDangNhap = 0)
        {
            InitializeComponent();
            _maHoaDon = MaHD;
            _hoTroCoGianForm = new CoGianGiaoDien(this);
            _maTKDangNhap = maTKDangNhap;
        }

        private void BatTatChucNang(bool giaTri)
        {
            btnLuuHD.Enabled = giaTri;
            btnInHD.Enabled = giaTri;
            btnXoa.Enabled = giaTri;

            txtMaHD.ReadOnly = true;
            numGiaBan.ReadOnly = true;

            //Nếu sửa hóa đơn cũ, không cho đổi Khách hàng/Nhân viên
            if (!string.IsNullOrEmpty(_maHoaDon))
            {
                cboHoTenKH.Enabled = false;
                cboHoTenNV.Enabled = false;
            }
        }


        private string SinhMaHoaDonTheoNgay()
        {
            //Lấy chuỗi ngày hiện tại (định dạng ddmmyy)
            string ngayHienTai = DateTime.Now.ToString("ddMMyy");
            string tienTo = "HD" + ngayHienTai; // Kết quả: HD210326

            //Tìm các mã hóa đơn trong ngày hôm nay
            var dsMaHomNay = context.HoaDon
                .Where(h => h.MaHD.StartsWith(tienTo))
                .Select(h => h.MaHD)
                .ToList();

            //Nếu chưa có hóa đơn nào trong ngày, bắt đầu từ 001
            if (dsMaHomNay.Count == 0)
            {
                return tienTo + "001";
            }

            //Nếu có, tìm mã lớn nhất để tăng lên 1
            var maLonNhat = dsMaHomNay.OrderByDescending(m => m).First();
            string soCu = maLonNhat.Substring(tienTo.Length); // Cắt lấy phần "001"
            int soMoi = int.Parse(soCu) + 1;

            return tienTo + soMoi.ToString("D3"); // Kết quả: HD210326002
        }

        private void frmHoaDon_ChiTiet_Load(object sender, EventArgs e)
        {
            // Tải dữ liệu vào các ComboBox
            LayNhanVienVaoComboBox();
            LayKhachHangVaoComboBox();
            LaySanPhamVaoComboBox();
            LayPTTTVaoComboBox();

            // Cấu hình DataGridView
            dgvDSChiTiet.AutoGenerateColumns = false;
            dgvDSChiTiet.DataSource = hoaDonChiTiet;

            // Xử lý Lập mới hoặc Sửa
            if (string.IsNullOrEmpty(_maHoaDon))
            {
                txtMaHD.Text = SinhMaHoaDonTheoNgay();
                dtpNgayLap.Value = DateTime.Now;
            }
            else
            {
                txtMaHD.Text = _maHoaDon;
                txtMaHD.ReadOnly = true;
                LoadDuLieuCu();
            }
            CapNhatTongThanhToan();
        }

        private void LayNhanVienVaoComboBox()
        {
            cboHoTenNV.DataSource = context.NhanVien.ToList();
            cboHoTenNV.ValueMember = "MaNV";
            cboHoTenNV.DisplayMember = "HoTenNV";
        }

        private void LayKhachHangVaoComboBox()
        {
            // Lấy danh sách khách hàng và tạo một danh sách mới có chuỗi hiển thị kết hợp
            var dsKH = context.KhachHang.Select(k => new
            {
                k.MaKH,
                // Nối Tên và SĐT thành một chuỗi duy nhất để tìm kiếm
                HienThi = k.HoTenKH + " - " + k.SDT
            }).ToList();

            cboHoTenKH.DataSource = dsKH;
            cboHoTenKH.ValueMember = "MaKH";
            cboHoTenKH.DisplayMember = "HienThi"; // Hiển thị cả 2 thông tin

            // Bật tính năng tìm kiếm 
            //cboHoTenKH.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cboHoTenKH.AutoCompleteSource = AutoCompleteSource.ListItems;

        }

        private void LaySanPhamVaoComboBox()
        {
            cboTenSP.DataSource = context.SanPham.ToList();
            cboTenSP.ValueMember = "MaSP";
            cboTenSP.DisplayMember = "TenSP";
            cboTenSP.SelectedIndex = -1;
        }

        private void LayPTTTVaoComboBox()
        {
            // Truy vấn lấy danh sách từ Database
            var dsPT = context.PT_ThanhToan.ToList();

            cboPT_ThanhToan.DataSource = dsPT;
            cboPT_ThanhToan.DisplayMember = "TenPT"; // Hiện chữ "Tiền mặt", "Chuyển khoản"...
            cboPT_ThanhToan.ValueMember = "MaPT";    // Khi lưu sẽ lấy mã "TM", "CK"...
        }

        private void LoadDuLieuCu()
        {
            //Tìm hóa đơn trong Database
            var hd = context.HoaDon.Find(_maHoaDon);
            if (hd != null)
            {
                //Gán các thông tin cơ bản lên Toolbox
                txtMaHD.Text = hd.MaHD;
                dtpNgayLap.Value = hd.NgayLap;
                txtGhiChu.Text = hd.GhiChu;

                //Gán giá trị cho các ComboBox ( SelectedValue)
                cboHoTenNV.SelectedValue = hd.MaNV;
                cboHoTenKH.SelectedValue = hd.MaKH;

                //lưu Mã PTTT thì dùng SelectedValue, nếu lưu chuỗi trực tiếp thì dùng .Text
                if (cboPT_ThanhToan.DataSource != null)
                    cboPT_ThanhToan.SelectedValue = hd.PT_ThanhToan;
                else
                    cboPT_ThanhToan.Text = hd.PT_ThanhToan;

                //Đổ dữ liệu vào bảng chi tiết dưới lưới
                hoaDonChiTiet.Clear();
                var dsChiTiet = context.ChiTietHD.Where(ct => ct.MaHD == _maHoaDon).ToList();
                foreach (var item in dsChiTiet)
                {
                    hoaDonChiTiet.Add(new DanhSachHoaDon_ChiTiet
                    {
                        MaHD = item.MaHD,
                        MaSP = item.MaSP,
                        TenSP = item.SanPham?.TenSP,
                        SoLuong = item.SoLuong,
                        GiaBan = item.GiaBan,
                        ThanhTien = item.ThanhTien
                    });
                }

                //HIỆN ĐIỂM VÀ TÍNH TOÁN GIẢM GIÁ 

                // Gọi hàm này để nó tự bốc điểm của Khách hàng hiện tại lên Label
                cboHoTenKH_SelectedIndexChanged(null, null);

                // Gán số tiền giảm và tổng tiền từ hóa đơn cũ lên Label
                lblTienGiam.Text = hd.GiamGia.ToString("N0");
                lblTongThanhToan.Text = hd.TongTien.ToString("N0");

                // Nếu hóa đơn này có tiền giảm > 0 thì tự động tích vào ô Đổi điểm
                if (hd.GiamGia > 0)
                {
                    ckbDoiDiem.Checked = true;
                }
                else
                {
                    ckbDoiDiem.Checked = false;
                }

                //Cập nhật lại trạng thái các nút bấm (Sửa thì khóa bớt NV/KH)
                BatTatChucNang(hoaDonChiTiet.Count > 0);
                CapNhatTongThanhToan();
            }

        }

        private void CapNhatTongThanhToan()
        {
            // Tính tổng tiền từ lưới
            decimal tongTienHang = hoaDonChiTiet.Sum(x => x.ThanhTien);
            lblTongTienHang.Text = tongTienHang.ToString("N0");
            decimal tienGiam = 0;

            // Tính tiền giảm nếu đổi điểm (10 điểm = 5%)
            int diemHienCo = 0;
            int.TryParse(lblTongDiemTichLuy.Text, out diemHienCo);

            if (ckbDoiDiem.Checked && diemHienCo >= 10)
            {
                int heSoGiam = diemHienCo / 10;
                tienGiam = tongTienHang * (heSoGiam * 0.05m);
            }

            lblTienGiam.Text = tienGiam.ToString("N0");
            lblTongThanhToan.Text = (tongTienHang - tienGiam).ToString("N0");

            // Bật tắt nút lưu 
            btnLuuHD.Enabled = dgvDSChiTiet.Rows.Count > 0;
            btnXoa.Enabled = dgvDSChiTiet.Rows.Count > 0;
        }
        private void lblDiemHienCo_Click(object sender, EventArgs e)
        {

        }

        private void cboHoTenKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboHoTenKH.SelectedValue != null)
            {
                string maKH = cboHoTenKH.SelectedValue.ToString();
                var kh = context.KhachHang.Find(maKH);
                if (kh != null)
                {
                    lblTongDiemTichLuy.Text = kh.DiemTichLuy.ToString();
                    CapNhatTongThanhToan();
                }
            }
            if (cboHoTenKH.SelectedValue != null)
            {
                string maKH = cboHoTenKH.SelectedValue.ToString();
                var kh = context.KhachHang.Find(maKH);
                if (kh != null)
                {
                    lblTongDiemTichLuy.Text = kh.DiemTichLuy.ToString();

                    // 2. LOGIC TỰ ĐỘNG CHECK: Nếu điểm >= 5 thì tự tích vào ô
                    if (kh.DiemTichLuy >= 10)
                    {
                        ckbDoiDiem.Checked = true;
                    }
                    else
                    {
                        ckbDoiDiem.Checked = false;
                    }

                    //Tính lại tiền 
                    CapNhatTongThanhToan();
                }
            }
        }

        private void cboTenSP_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboTenSP.SelectedValue != null)
            {
                string maSP = cboTenSP.SelectedValue.ToString();
                var sp = context.SanPham.Find(maSP);
                if (sp != null)
                {
                    numGiaBan.Value = sp.GiaBan;
                }
            }
        }





        private void ckbDoiDiem_CheckedChanged(object sender, EventArgs e)
        {
            CapNhatTongThanhToan();
        }




        private void dgvDSChiTiet_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            // Tính toán vị trí để chữ nằm giữa ô STT (Cột index 0)
            // Tọa độ X = lề trái của Grid + chiều rộng ô xám đầu dòng + một khoảng đệm
            float x = e.RowBounds.Left + grid.RowHeadersWidth + 10;
            float y = e.RowBounds.Top + (e.RowBounds.Height - grid.Font.Height) / 2;

            e.Graphics.DrawString(rowIdx, grid.Font, SystemBrushes.ControlText, x, y);
        }

        private void dgvDSChiTiet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu bấm vào tiêu đề thì bỏ qua
            if (e.RowIndex < 0) return;

            // Lấy dòng đang chọn
            var row = dgvDSChiTiet.Rows[e.RowIndex];

            // Gán Sản phẩm (Dùng MaSP để ComboBox tự nhảy đúng tên)
            cboTenSP.SelectedValue = row.Cells["MaSP"].Value.ToString();

            //Gán Đơn giá
            numGiaBan.Value = decimal.Parse(row.Cells["GiaBan"].Value.ToString());

            //Gán Số lượng
            numSoLuong.Value = decimal.Parse(row.Cells["SoLuong"].Value.ToString());

        }

       

        private void btnXacNhanBan_Click(object sender, EventArgs e)
        {
            if (cboTenSP.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần bán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTenSP.Focus();
                return;
            }

            if (numSoLuong.Value <= 0)
            {
                MessageBox.Show("Số lượng bán phải lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numSoLuong.Focus();
                return;
            }

            // 1. Lấy thông tin sản phẩm và kiểm tra tồn kho
            string maSP = cboTenSP.SelectedValue.ToString();
            var spTrongKho = context.SanPham.Find(maSP);

            // 2. Tìm xem sản phẩm này đã có trên lưới chưa
            var sanPhamDaCo = hoaDonChiTiet.FirstOrDefault(x => x.MaSP == maSP);

            if (sanPhamDaCo != null)
            {
                // --- LOGIC CỘNG DỒN ---
                int tongSoLuongMoi = sanPhamDaCo.SoLuong + (int)numSoLuong.Value;

                // Kiểm tra tồn kho cho tổng mới
                if (spTrongKho != null && spTrongKho.SLTon < tongSoLuongMoi)
                {
                    MessageBox.Show($"Kho chỉ còn {spTrongKho.SLTon} sản phẩm. Không đủ cộng thêm!", "Hết hàng");
                    return;
                }

                // Cập nhật giá trị
                sanPhamDaCo.SoLuong = tongSoLuongMoi;
                sanPhamDaCo.ThanhTien = sanPhamDaCo.SoLuong * sanPhamDaCo.GiaBan;

                // --- DÒNG QUAN TRỌNG NHẤT NÈ VY ---
                // Lệnh này ép BindingList thông báo cho Grid vẽ lại con số mới
                hoaDonChiTiet.ResetBindings();
            }
            else
            {
                // TRƯỜNG HỢP THÊM MỚI LẦN ĐẦU
                if (spTrongKho != null && spTrongKho.SLTon < (int)numSoLuong.Value)
                {
                    MessageBox.Show($"Kho chỉ còn {spTrongKho.SLTon} sản phẩm!", "Hết hàng");
                    return;
                }

                hoaDonChiTiet.Add(new DanhSachHoaDon_ChiTiet
                {
                    MaHD = txtMaHD.Text,
                    MaSP = maSP,
                    TenSP = cboTenSP.Text,
                    SoLuong = (int)numSoLuong.Value,
                    GiaBan = numGiaBan.Value,
                    ThanhTien = numSoLuong.Value * numGiaBan.Value
                });
            }

            // Cập nhật tổng tiền và reset form nhập
            CapNhatTongThanhToan();

            cboTenSP.SelectedIndex = -1;
            numSoLuong.Value = 1;
            cboTenSP.Focus();
        }

        private void btnThemKHMoi_Click(object sender, EventArgs e)
        {
            frmKhachHang frm = new frmKhachHang(_maTKDangNhap);
            frm.MoDeThemMoi = true;
            frm.ShowDialog();
            LayKhachHangVaoComboBox();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSChiTiet.CurrentRow == null) return;
            var item = dgvDSChiTiet.CurrentRow.DataBoundItem as DanhSachHoaDon_ChiTiet;
            if (item != null)
            {
                hoaDonChiTiet.Remove(item);
                CapNhatTongThanhToan();
            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnLuuHD_Click(object sender, EventArgs e)
        {
            if (cboHoTenNV.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên lập hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboHoTenNV.Focus();
                return;
            }

            if (cboHoTenKH.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboHoTenKH.Focus();
                return; // Dừng ngay
            }

            if (cboPT_ThanhToan.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn phương thức thanh toán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (hoaDonChiTiet.Count == 0)
            {
                MessageBox.Show("Hóa đơn chưa có sản phẩm nào, không thể lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var db = context.Database.BeginTransaction())
            {
                try
                {
                    string hanhDongLog = "";
                    //Lưu Hóa Đơn
                    var hd = context.HoaDon.Find(txtMaHD.Text);
                    if (hd == null)
                    {
                        hd = new HoaDon { MaHD = txtMaHD.Text };
                        context.HoaDon.Add(hd);
                        hanhDongLog = $"Lập hóa đơn mới {txtMaHD.Text} cho khách hàng {cboHoTenKH.Text}";
                    }
                    else
                    {
                        // Log SỬA
                        hanhDongLog = $"Cập nhật hóa đơn {txtMaHD.Text}";
                    }

                    hd.NgayLap = DateTime.Now;
                    hd.MaNV = cboHoTenNV.SelectedValue.ToString();
                    hd.MaKH = cboHoTenKH.SelectedValue.ToString();
                    hd.GhiChu = txtGhiChu.Text;
                    hd.PT_ThanhToan = cboPT_ThanhToan.SelectedValue.ToString();
                    hd.GiamGia = decimal.Parse(lblTienGiam.Text);
                    hd.TongTien = decimal.Parse(lblTongThanhToan.Text);

                    // Xử lý Điểm Tích Lũy
                    var kh = context.KhachHang.Find(hd.MaKH);
                    if (ckbDoiDiem.Checked)
                    {
                        int diemDung = (kh.DiemTichLuy / 10) * 10;
                        kh.DiemTichLuy -= diemDung;
                    }
                    // Cộng điểm mới (100k = 1 điểm)
                    kh.DiemTichLuy += (int)(hd.TongTien / 100000);


                    // Tìm chi tiết cũ
                    var oldCt = context.ChiTietHD.Where(c => c.MaHD == hd.MaHD).ToList();
                    //Cộng trả lại tồn kho (Nếu đang ở sửa hd)
                    foreach (var oldItem in oldCt)
                    {
                        var sp = context.SanPham.Find(oldItem.MaSP);
                        if (sp != null) sp.SLTon += oldItem.SoLuong; // Trả hàng lại kho
                    }
                    //Xóa sạch chi tiết cũ khỏi DB
                    context.ChiTietHD.RemoveRange(oldCt);

                    foreach (var item in hoaDonChiTiet)
                    {
                        //Thêm vào bảng Chi tiết hóa đơn
                        context.ChiTietHD.Add(new HoaDon_ChiTiet
                        {
                            MaHD = hd.MaHD,
                            MaSP = item.MaSP,
                            SoLuong = item.SoLuong,
                            GiaBan = item.GiaBan,
                            ThanhTien = item.ThanhTien
                        });

                        //CẬP NHẬT TỒN KHO: Tìm sản phẩm và trừ số lượng
                        var sp = context.SanPham.Find(item.MaSP);
                        if (sp != null)
                        {
                            if (sp.SLTon < item.SoLuong)
                            {
                                //báo lỗi nếu hết hàng 
                                throw new Exception($"Sản phẩm '{sp.TenSP}' không đủ hàng trong kho!");
                            }
                            sp.SLTon -= item.SoLuong; // Trừ số lượng tồn
                        }
                    }

                    // SaveChanges xuống Database
                    context.SaveChanges();
                    db.Commit();
                    NhatKyHeThong.GhiLog(_maTKDangNhap, hanhDongLog);
                    MessageBox.Show("Đã lưu hóa đơn thành công!");
                    this.Close();
                }

                catch (Exception ex)
                {
                    db.Rollback();
                    string errorMsg = ex.Message;
                    if (ex.InnerException != null)
                    {
                        errorMsg += "\nChi tiết: " + ex.InnerException.Message;
                    }
                    MessageBox.Show(errorMsg, "Lỗi Database");
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnInHD_Click(object sender, EventArgs e)
        {
            using (frmInHoaDon inHoaDon = new frmInHoaDon(_maHoaDon))
            {
                inHoaDon.ShowDialog();
            }
        }

        
    }
}
