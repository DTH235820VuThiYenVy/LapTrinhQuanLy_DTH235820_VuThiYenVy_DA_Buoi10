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
using BC = BCrypt.Net.BCrypt;

namespace QuanLyCuaHangMyPham.Forms
{
    public partial class frmDoiMatKhau : Form
    {
        private TaiKhoan _tkHienTai;
        public frmDoiMatKhau(TaiKhoan tk)
        {
            InitializeComponent();
            _tkHienTai = tk;
            txtMatKhauCu.UseSystemPasswordChar = txtMatKhauMoi.UseSystemPasswordChar = txtXacNhanMatKhau.UseSystemPasswordChar = true;
        }

        private void frmDoiMatKhau_Load(object sender, EventArgs e)
        {

        }


        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {

            using (SolidBrush coVeKinhMo = new SolidBrush(Color.FromArgb(120, 255, 255, 255)))
            {
                e.Graphics.FillRectangle(coVeKinhMo, e.ClipRectangle);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            string cu = txtMatKhauCu.Text;
            string moi = txtMatKhauMoi.Text;
            string xacNhan = txtXacNhanMatKhau.Text;

            if (string.IsNullOrWhiteSpace(cu) || string.IsNullOrWhiteSpace(moi))
            {
                MessageBox.Show("Vui lòng không để trống ô nào!", "Thông báo");
                return;
            }

            //Kiểm tra mật khẩu cũ có đúng không (Dùng BCrypt.Verify)
            // Lưu ý: Nếu mật khẩu cũ trong DB chưa mã hóa, Verify sẽ lỗi=>dùng logic an toàn
            bool hopLe = false;
            try
            {
                hopLe = BC.Verify(cu, _tkHienTai.MatKhau);
            }
            catch
            {
                hopLe = (cu == _tkHienTai.MatKhau);
            }

            if (!hopLe)
            {
                MessageBox.Show("Mật khẩu cũ không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Kiểm tra mật khẩu mới và xác nhận có khớp không
            if (moi != xacNhan)
            {
                MessageBox.Show("Mật khẩu mới và Xác nhận không trùng khớp.", "Lỗi");
                return;
            }

            //lưu vào CSDL
            try
            {
                using (var context = new QLCHMPDbContext())
                {
                    //var tkDB = context.TaiKhoan.Find(_tkHienTai.TenDangNhap);
                    var tkDB = context.TaiKhoan.FirstOrDefault(t => t.TenDangNhap == _tkHienTai.TenDangNhap);
                    if (tkDB != null)
                    {
                        // BĂM MẬT KHẨU MỚI TRƯỚC KHI LƯU
                        tkDB.MatKhau = BC.HashPassword(moi);
                        context.SaveChanges();

                        NhatKyHeThong.GhiLog(_tkHienTai.MaTK, "Đổi mật khẩu tài khoản");

                        this.DialogResult = DialogResult.OK; // Đóng form và báo về Form Main
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}
