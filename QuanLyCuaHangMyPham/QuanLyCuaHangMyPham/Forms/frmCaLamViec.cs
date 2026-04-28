using Microsoft.EntityFrameworkCore;
using QuanLyCuaHangMyPham.Data;
using QuanLyCuaHangMyPham.TienIch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCuaHangMyPham.Forms
{
    public partial class frmCaLamViec : Form
    {
        private QLCHMPDbContext _context = new QLCHMPDbContext();
        private CoGianGiaoDien _hoTroCoGianForm;
        private bool _xuLyThem = false; 
        string _maCa = "";
        private int _maTKDangNhap;
        public frmCaLamViec(int maTKDangNhap)
        {
            InitializeComponent();
            _hoTroCoGianForm = new CoGianGiaoDien(this);
            _maTKDangNhap = maTKDangNhap;
        }

        private void TaiDuLieuLenBang()
        {
            // 1. Làm mới dữ liệu từ DB
            var ds = _context.CaLamViec.ToList();
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = ds;

            // 2. Xóa sạch Binding cũ để tránh lỗi chồng chéo
            txtMaCa.DataBindings.Clear();
            txtTenCa.DataBindings.Clear();
            txtGhiChu.DataBindings.Clear();
            dtpGioBatDau.DataBindings.Clear();
            dtpGioKetThuc.DataBindings.Clear();

            // 3. Thiết lập Binding cho các TextBox
            txtMaCa.DataBindings.Add("Text", bindingSource, "MaCa", false, DataSourceUpdateMode.Never);
            txtTenCa.DataBindings.Add("Text", bindingSource, "TenCa", false, DataSourceUpdateMode.Never);
            txtGhiChu.DataBindings.Add("Text", bindingSource, "GhiChu", false, DataSourceUpdateMode.Never);

            // 4. Thiết lập Binding cho DateTimePicker (Xử lý kiểu TimeSpan sang DateTime)
            Binding bBatDau = new Binding("Value", bindingSource, "GioBatDau", true, DataSourceUpdateMode.Never);
            bBatDau.Format += (s, ev) => { if (ev.Value != null) ev.Value = DateTime.Today.Add((TimeSpan)ev.Value); };
            dtpGioBatDau.DataBindings.Add(bBatDau);

            Binding bKetThuc = new Binding("Value", bindingSource, "GioKetThuc", true, DataSourceUpdateMode.Never);
            bKetThuc.Format += (s, ev) => { if (ev.Value != null) ev.Value = DateTime.Today.Add((TimeSpan)ev.Value); };
            dtpGioKetThuc.DataBindings.Add(bKetThuc);

            // 5. Đổ lên lưới và reset trạng thái nút
            dgvCaLamViec.DataSource = bindingSource;
            BatTatChucNang(false);
        }

        // Hàm này để bật/tắt các ô nhập và nút bấm cho đúng quy trình buổi 1
        private void BatTatChucNang(bool giaTri)
        {
            txtTenCa.Enabled = giaTri;
            dtpGioBatDau.Enabled = giaTri;
            dtpGioKetThuc.Enabled = giaTri;
            txtGhiChu.Enabled = giaTri;
            txtMaCa.Enabled = false; // Luôn khóa mã vì tự sinh

            // Các nút bấm điều khiển trạng thái 
            btnThem.Enabled = !giaTri;
            btnSua.Enabled = !giaTri;
            btnXoa.Enabled = !giaTri;
            btnLuu.Enabled = giaTri;
            btnHuy.Enabled = giaTri;
        }



        private void frmCaLamViec_Load(object sender, EventArgs e)
        {
            BatTatChucNang(false);
            dgvCaLamViec.AutoGenerateColumns = false;
            TaiDuLieuLenBang();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            _xuLyThem = true;
            BatTatChucNang(true);

            txtMaCa.DataBindings.Clear();
            txtTenCa.DataBindings.Clear();
            txtGhiChu.DataBindings.Clear();

            txtTenCa.Clear();
            txtGhiChu.Clear();

            var dsMa = _context.CaLamViec.Select(c => c.MaCa).ToList();
            txtMaCa.Text = SinhMaTuDong.TuSinhMa(dsMa, "MaCa", "CA", 2);
            txtMaCa.Enabled = false; // Mã ca tự sinh, không cho sửa


        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            _xuLyThem = false;
            BatTatChucNang(true);
            _maCa = dgvCaLamViec.CurrentRow.Cells["MaCa"].Value.ToString();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận xóa?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _maCa = dgvCaLamViec.CurrentRow.Cells["MaCa"].Value.ToString();
                CaLamViec ca = _context.CaLamViec.Find(_maCa);
                if (ca != null)
                {
                    string tenCaBiXoa = ca.TenCa;
                    _context.CaLamViec.Remove(ca);
                    _context.SaveChanges();
                    NhatKyHeThong.GhiLog(_maTKDangNhap, $"Xóa ca làm việc: {_maCa} - {tenCaBiXoa}");
                }

                TaiDuLieuLenBang();
            }
        }


        private void btnHuy_Click(object sender, EventArgs e)
        {
            DialogResult ketQua = MessageBox.Show("Bạn chắc chắn muốn hủy bỏ?", "Xác nhận hủy", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (ketQua == DialogResult.Yes)
            {
                // Xóa cờ đang thêm và tải lại form để reset mọi thứ
                _xuLyThem = false;
                TaiDuLieuLenBang();
            }
            // Nếu chọn No thì màn hình cứ giữ nguyên đó cho người ta gõ tiếp
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
                this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenCa.Text))
            {
                MessageBox.Show("Vui lòng nhập tên ca làm việc!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenCa.Focus();
                return;
            }

            try
            {
                string maCa = txtMaCa.Text.Trim();
                string tenCa = txtTenCa.Text.Trim();
                TimeSpan gioBatDau = dtpGioBatDau.Value.TimeOfDay; // Lấy phần Giờ:Phút
                TimeSpan gioKetThuc = dtpGioKetThuc.Value.TimeOfDay;
                string ghiChu = txtGhiChu.Text.Trim();

                string thongBaoLichSu = ""; 

                if (_xuLyThem)
                {
                    // THÊM MỚI
                    CaLamViec caMoi = new CaLamViec
                    {
                        MaCa = maCa,
                        TenCa = tenCa,
                        GioBatDau = gioBatDau,
                        GioKetThuc = gioKetThuc,
                        GhiChu = ghiChu
                    };
                    _context.CaLamViec.Add(caMoi);
                    thongBaoLichSu = $"Thêm mới ca làm việc: {maCa} - {tenCa}";
                    NhatKyHeThong.GhiLog(_maTKDangNhap, $"Thêm phân ca mới {caMoi.MaCa} vào {txtTenCa.Text}");

                }
                else
                {
                    // CẬP NHẬT (SỬA)
                    CaLamViec caSua = _context.CaLamViec.Find(_maCa);
                    if (caSua != null)
                    {
                        caSua.TenCa = tenCa;
                        caSua.GioBatDau = gioBatDau;
                        caSua.GioKetThuc = gioKetThuc;
                        caSua.GhiChu = ghiChu;
                        thongBaoLichSu = $"Cập nhật thông tin ca làm việc: {_maCa}";
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy ca làm việc cần sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                _context.SaveChanges();
                NhatKyHeThong.GhiLog(_maTKDangNhap, thongBaoLichSu);
                MessageBox.Show("Lưu thông tin ca làm việc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _xuLyThem = false;
                TaiDuLieuLenBang(); // Hàm này đã có sẵn BatTatChucNang(false) ở cuối
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
