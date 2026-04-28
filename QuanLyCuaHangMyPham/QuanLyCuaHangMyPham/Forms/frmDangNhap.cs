using Microsoft.EntityFrameworkCore;
using QuanLyCuaHangMyPham.Data;
using QuanLyCuaHangMyPham.TienIch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BC = BCrypt.Net.BCrypt;
namespace QuanLyCuaHangMyPham.Forms
{
    public partial class frmDangNhap : Form
    {

        public frmDangNhap()
        {
            InitializeComponent();
            txtMatKhau.UseSystemPasswordChar = true; // Mặc định ẩn bằng dấu chấm hệ thống 
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            chkHienMatKhau.Checked = false;
            groupBox1.BackColor = Color.FromArgb(100, 255, 255, 255);

            foreach (Control c in groupBox1.Controls)
            {
                if (c is Label || c is CheckBox)
                {
                    c.BackColor = Color.Transparent;
                    c.ForeColor = Color.White;     
                }
                else if (c is LinkLabel link)
                {
                    link.ForeColor = Color.White;          
                    link.BackColor = Color.Transparent;
                    link.LinkColor = Color.White;           
                    link.ActiveLinkColor = Color.LightGray;
                    link.VisitedLinkColor = Color.White;    
                    link.LinkBehavior = LinkBehavior.HoverUnderline; 
                }
            }

        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            // Nếu Check là True (Hiện) thì UseSystemPasswordChar là False và ngược lại 
            txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;

            // Đảm bảo PasswordChar không giữ ký tự '*' khi muốn hiện mật khẩu 
            if (chkHienMatKhau.Checked)
            {
                txtMatKhau.PasswordChar = '\0';
            }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }





        private void txtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnDangNhap_Click(sender, e);
            }
        }

        // Xử lý sự kiện khi người dùng click vào link "Quên mật khẩu"
        // Hàm tạo mật khẩu ngẫu nhiên 6 ký tự
        private string TaoMatKhauNgauNhien(int doDai = 6)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, doDai)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Hàm thực hiện gửi Email
        private bool GuiEmail(string emailNhan, string matKhauMoi)
        {
            try
            {
                string emailGui = "vuthiyenvy000@gmail.com";
                string matKhauUngDung = "amiosxqidtjeffls"; // Dán 16 ký tự lấy từ Google 

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(emailGui, "Hệ Thống Quản Lý Elvie Cosmetics");
                mail.To.Add(emailNhan);
                mail.Subject = "Yêu cầu khôi phục mật khẩu";
                mail.Body = $"<h3>Chào bạn,</h3>" +
                            $"<p>Hệ thống đã nhận được yêu cầu khôi phục mật khẩu cho tài khoản của bạn.</p>" +
                            $"<p>Mật khẩu mới của bạn là: <b style='color:red; font-size:18px;'>{matKhauMoi}</b></p>" +
                            $"<p>Vui lòng đăng nhập và đổi lại mật khẩu ngay để đảm bảo an toàn.</p>" +
                            $"<br/><p>Trân trọng,<br/>Elvie Cosmetics</p>";
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential(emailGui, matKhauUngDung);
                smtp.EnableSsl = true; // Bắt buộc phải bật SSL cho Gmail

                smtp.Send(mail);
                return true; // Gửi thành công
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi email: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Gửi thất bại
            }
        }
        private void lnkQuenMatKhau_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string tenDN = txtTenDangNhap.Text.Trim();

            if (string.IsNullOrEmpty(tenDN))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập để khôi phục mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return;
            }

            try
            {
                using (var _context = new QLCHMPDbContext())
                {
                    // Tìm tài khoản và Include bảng NhanVien để lấy được Email
                    var taiKhoan = _context.TaiKhoan
                                           .Include(tk => tk.NhanVien)
                                           .FirstOrDefault(tk => tk.TenDangNhap == tenDN);

                    if (taiKhoan != null)
                    {
                        // Kiểm tra xem nhân viên này có khai báo email chưa
                        if (taiKhoan.NhanVien == null || string.IsNullOrWhiteSpace(taiKhoan.NhanVien.Email))
                        {
                            MessageBox.Show("Tài khoản này chưa được thiết lập địa chỉ Email. Vui lòng liên hệ Admin để được hỗ trợ!",
                                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string emailNhanVien = taiKhoan.NhanVien.Email;

                        var result = MessageBox.Show($"Mật khẩu mới sẽ được gửi về Email: {emailNhanVien}\nBạn có muốn tiếp tục?",
                                                     "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            this.Cursor = Cursors.WaitCursor; 

                            //Tạo mật khẩu mới ngẫu nhiên
                            string matKhauMoi = TaoMatKhauNgauNhien(6);

                            //Gửi email
                            bool daGuiThanhCong = GuiEmail(emailNhanVien, matKhauMoi);

                            //Nếu gửi mail thành công thì mới lưu vào Database
                            if (daGuiThanhCong)
                            {
                                taiKhoan.MatKhau = BC.HashPassword(matKhauMoi);
                                _context.SaveChanges();

                                NhatKyHeThong.GhiLog(taiKhoan.MaTK, $"Đã gửi yêu cầu khôi phục mật khẩu về email {emailNhanVien}");

                                MessageBox.Show("Đã gửi mật khẩu mới! Vui lòng kiểm tra hộp thư đến (hoặc hộp thư rác/spam) của bạn.",
                                                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            this.Cursor = Cursors.Default; // Trả lại con trỏ chuột bình thường
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tên đăng nhập này không có trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

       
    }
}