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
    public partial class frmCauHinhGiaoDien : Form
    {
        private string _maNV; // Khai báo biến để lưu mã nhân viên
        public frmCauHinhGiaoDien(string maNV)
        {
            InitializeComponent();
            this._maNV = maNV;
        }


        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new QLCHMPDbContext())
                {
                    var config = db.CauHinhGiaoDien.Find(_maNV);
                    if (config == null)
                    {
                        config = new CauHinhGiaoDien { MaNV = _maNV };
                        db.CauHinhGiaoDien.Add(config);
                    }

                    config.MauNen = ColorTranslator.ToHtml(panelPreview.BackColor);
                    config.MauChu = ColorTranslator.ToHtml(lblMau.ForeColor);
                    config.FontChu = lblMau.Font.Name;
                    config.CoChu = (int)lblMau.Font.Size;
                    config.Theme = cboTheme.Text; // Lưu tên theme người dùng đã chọn

                    db.SaveChanges();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                // Hiện ra lỗi thật sự từ InnerException
                string msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show("Lỗi lưu cấu hình: " + msg, "Thông báo lỗi");
            }
        }

        private void frmCauHinhGiaoDien_Load(object sender, EventArgs e)
        {
            using (var db = new QLCHMPDbContext())
            {
                var config = db.CauHinhGiaoDien.Find(_maNV);
                if (config != null)
                {
                    // Hiển thị lại tên Theme đã lưu vào ComboBox
                    cboTheme.Text = config.Theme;

                    // Nạp lại các màu xem trước
                    panelPreview.BackColor = ColorTranslator.FromHtml(config.MauNen);
                    lblMau.ForeColor = ColorTranslator.FromHtml(config.MauChu);
                    lblMau.Font = new Font(config.FontChu, config.CoChu);
                }
            }
        }


        private void cboTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTheme = cboTheme.SelectedItem.ToString();

            switch (selectedTheme)
            {
                case "Mặc định":
                    panelPreview.BackColor = Color.White; // Nền form màu trắng hoặc trắng xám
                    lblMau.ForeColor = Color.Navy; // Chữ màu Xanh Navy hoặc Royal Blue
                    break;

                case "Chế độ sáng (Light Mode)":
                    // Cái cũ hồi xưa: Trắng đen cơ bản
                    panelPreview.BackColor = Color.WhiteSmoke; // Xám siêu nhạt cho chuẩn Light Mode
                    lblMau.ForeColor = Color.Black;
                    break;

                case "Chế độ tối (Dark Mode)":
                    panelPreview.BackColor = Color.FromArgb(45, 45, 48); // Màu xám đen
                    lblMau.ForeColor = Color.White;
                    break;

                case "Mùa xuân (Hồng)":
                    panelPreview.BackColor = Color.MistyRose;
                    lblMau.ForeColor = Color.DeepPink;
                    break;

                case "Tùy chỉnh":
                    // Không làm gì để người dùng tự bấm nút chọn màu
                    break;
            }
        }

        private void btnChonMauNen_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                panelPreview.BackColor = colorDialog1.Color;
            }
        }

        private void btnChonMauChu_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                lblMau.ForeColor = colorDialog1.Color;
            }
        }

        private void btnChonFont_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                lblMau.Font = fontDialog1.Font;
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
