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
    public partial class frmThongTinCaNhan : Form
    {
        private TaiKhoan tkHienTai = null;
        private CoGianGiaoDien _hoTroCoGianForm;
        public frmThongTinCaNhan(TaiKhoan tk)
        {
            InitializeComponent();
            tkHienTai = tk;
            _hoTroCoGianForm = new CoGianGiaoDien(this);
        }

        private void frmThongTinCaNhan_Load(object sender, EventArgs e)
        {
            
            using (var context = new QLCHMPDbContext())
            {
                var nhanVien = context.NhanVien.FirstOrDefault(nv => nv.MaNV == tkHienTai.MaNV);
                if (nhanVien != null)
                {
                    // Đổ dữ liệu lên các ô TextBox
                    txtMaNV.Text = nhanVien.MaNV;
                    txtHoTenNV.Text = nhanVien.HoTenNV;
                    txtChucVu.Text = nhanVien.ChucVu;
                    txtLuongCoBan.Text = nhanVien.LuongCoBan.ToString("N0"); // Hiện số có dấu phẩy cho đẹp
                    txtEmail.Text = nhanVien.Email;
                    txtSDT.Text = nhanVien.SDT;
                    txtDiaChi.Text = nhanVien.DiaChi;
                    dtpNgaySinh.Value = nhanVien.NgaySinh;
                    dtpNgayVao.Value = nhanVien.NgayVao;
                    if (nhanVien.GioiTinh == "Nam")
                    {
                        radNam.Checked = true;
                    }
                    else if (nhanVien.GioiTinh == "Nữ")
                    {
                        radNu.Checked = true;
                    }
                }
            }
            foreach (Control c in groupBox1.Controls)
            {
                if (c is Label || c is RadioButton)
                {
                    c.ForeColor = Color.White;         // Đổi chữ thành màu trắng
                    c.BackColor = Color.Transparent;   // Giúp nền chữ trong suốt, ăn nhập với hình nền
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu trống cơ bản
            if (string.IsNullOrWhiteSpace(txtHoTenNV.Text) || string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Họ tên và Số điện thoại không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var context = new QLCHMPDbContext())
            {
                var nvUpdate = context.NhanVien.FirstOrDefault(nv => nv.MaNV == tkHienTai.MaNV);
                if (nvUpdate != null)
                {
                    // Chỉ cập nhật những ô cho phép sửa
                    nvUpdate.HoTenNV = txtHoTenNV.Text;
                    nvUpdate.Email = txtEmail.Text;
                    nvUpdate.SDT = txtSDT.Text;
                    nvUpdate.DiaChi = txtDiaChi.Text;
                    nvUpdate.NgaySinh = dtpNgaySinh.Value;
                    if (radNam.Checked == true)
                    {
                        nvUpdate.GioiTinh = "Nam";
                    }
                    else if (radNu.Checked == true)
                    {
                        nvUpdate.GioiTinh = "Nữ";
                    }
                    // Lưu xuống Database
                    context.SaveChanges();
                    string hanhDongLog = $"Tự cập nhật thông tin cá nhân (SĐT: {nvUpdate.SDT}, Email: {nvUpdate.Email})";
                    NhatKyHeThong.GhiLog(tkHienTai.MaTK, hanhDongLog);
                    MessageBox.Show("Cập nhật thông tin cá nhân thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
           
        }
    }
}
