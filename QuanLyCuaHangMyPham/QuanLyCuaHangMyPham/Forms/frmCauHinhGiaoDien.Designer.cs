namespace QuanLyCuaHangMyPham.Forms
{
    partial class frmCauHinhGiaoDien
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCauHinhGiaoDien));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            colorDialog1 = new ColorDialog();
            fontDialog1 = new FontDialog();
            lblMau = new Label();
            panelPreview = new Panel();
            cboTheme = new ComboBox();
            label1 = new Label();
            btnThoat = new Guna.UI2.WinForms.Guna2Button();
            btnHuy = new Guna.UI2.WinForms.Guna2Button();
            btnLuu = new Guna.UI2.WinForms.Guna2Button();
            btnChonMauNen = new Guna.UI2.WinForms.Guna2Button();
            btnChonFont = new Guna.UI2.WinForms.Guna2Button();
            btnChonMauChu = new Guna.UI2.WinForms.Guna2Button();
            SuspendLayout();
            // 
            // lblMau
            // 
            lblMau.AutoSize = true;
            lblMau.Location = new Point(300, 20);
            lblMau.Name = "lblMau";
            lblMau.Size = new Size(257, 23);
            lblMau.TabIndex = 1;
            lblMau.Text = "Dòng chữ mẫu - Elvie Cosmetics";
            // 
            // panelPreview
            // 
            panelPreview.Location = new Point(28, 64);
            panelPreview.Name = "panelPreview";
            panelPreview.Size = new Size(843, 125);
            panelPreview.TabIndex = 2;
            // 
            // cboTheme
            // 
            cboTheme.FormattingEnabled = true;
            cboTheme.Items.AddRange(new object[] { "Mặc định", "Chế độ sáng (Light Mode)", "Chế độ tối (Dark Mode)", "Mùa xuân (Hồng)", "Tùy chỉnh " });
            cboTheme.Location = new Point(341, 209);
            cboTheme.Name = "cboTheme";
            cboTheme.Size = new Size(329, 31);
            cboTheme.TabIndex = 6;
            cboTheme.SelectedIndexChanged += cboTheme_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(213, 212);
            label1.Name = "label1";
            label1.Size = new Size(108, 23);
            label1.TabIndex = 7;
            label1.Text = "Chọn chủ đề";
            // 
            // btnThoat
            // 
            btnThoat.BackColor = Color.Navy;
            btnThoat.CustomizableEdges = customizableEdges1;
            btnThoat.DisabledState.BorderColor = Color.DarkGray;
            btnThoat.DisabledState.CustomBorderColor = Color.DarkGray;
            btnThoat.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnThoat.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnThoat.FillColor = Color.Navy;
            btnThoat.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnThoat.ForeColor = Color.White;
            btnThoat.Image = (Image)resources.GetObject("btnThoat.Image");
            btnThoat.Location = new Point(776, 266);
            btnThoat.Name = "btnThoat";
            btnThoat.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnThoat.Size = new Size(95, 38);
            btnThoat.TabIndex = 27;
            btnThoat.Text = "Thoát";
            btnThoat.Click += btnThoat_Click;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.Navy;
            btnHuy.CustomizableEdges = customizableEdges3;
            btnHuy.DisabledState.BorderColor = Color.DarkGray;
            btnHuy.DisabledState.CustomBorderColor = Color.DarkGray;
            btnHuy.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnHuy.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnHuy.FillColor = Color.Navy;
            btnHuy.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnHuy.ForeColor = Color.White;
            btnHuy.Image = (Image)resources.GetObject("btnHuy.Image");
            btnHuy.Location = new Point(640, 266);
            btnHuy.Name = "btnHuy";
            btnHuy.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnHuy.Size = new Size(95, 38);
            btnHuy.TabIndex = 26;
            btnHuy.Text = "Hủy bỏ";
            // 
            // btnLuu
            // 
            btnLuu.BackColor = Color.Navy;
            btnLuu.CustomizableEdges = customizableEdges5;
            btnLuu.DisabledState.BorderColor = Color.DarkGray;
            btnLuu.DisabledState.CustomBorderColor = Color.DarkGray;
            btnLuu.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnLuu.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnLuu.FillColor = Color.Navy;
            btnLuu.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLuu.ForeColor = Color.White;
            btnLuu.Image = (Image)resources.GetObject("btnLuu.Image");
            btnLuu.Location = new Point(504, 266);
            btnLuu.Name = "btnLuu";
            btnLuu.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnLuu.Size = new Size(95, 38);
            btnLuu.TabIndex = 25;
            btnLuu.Text = "Lưu";
            btnLuu.Click += btnLuu_Click;
            // 
            // btnChonMauNen
            // 
            btnChonMauNen.BackColor = Color.Navy;
            btnChonMauNen.CustomizableEdges = customizableEdges7;
            btnChonMauNen.DisabledState.BorderColor = Color.DarkGray;
            btnChonMauNen.DisabledState.CustomBorderColor = Color.DarkGray;
            btnChonMauNen.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnChonMauNen.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnChonMauNen.FillColor = Color.Navy;
            btnChonMauNen.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnChonMauNen.ForeColor = Color.White;
            btnChonMauNen.Location = new Point(28, 266);
            btnChonMauNen.Name = "btnChonMauNen";
            btnChonMauNen.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnChonMauNen.Size = new Size(95, 38);
            btnChonMauNen.TabIndex = 28;
            btnChonMauNen.Text = "Màu nền";
            btnChonMauNen.Click += btnChonMauNen_Click;
            // 
            // btnChonFont
            // 
            btnChonFont.BackColor = Color.Navy;
            btnChonFont.CustomizableEdges = customizableEdges9;
            btnChonFont.DisabledState.BorderColor = Color.DarkGray;
            btnChonFont.DisabledState.CustomBorderColor = Color.DarkGray;
            btnChonFont.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnChonFont.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnChonFont.FillColor = Color.Navy;
            btnChonFont.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnChonFont.ForeColor = Color.White;
            btnChonFont.Location = new Point(300, 266);
            btnChonFont.Name = "btnChonFont";
            btnChonFont.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnChonFont.Size = new Size(163, 38);
            btnChonFont.TabIndex = 29;
            btnChonFont.Text = "Phông chữ";
            btnChonFont.Click += btnChonFont_Click;
            // 
            // btnChonMauChu
            // 
            btnChonMauChu.BackColor = Color.Navy;
            btnChonMauChu.CustomizableEdges = customizableEdges11;
            btnChonMauChu.DisabledState.BorderColor = Color.DarkGray;
            btnChonMauChu.DisabledState.CustomBorderColor = Color.DarkGray;
            btnChonMauChu.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnChonMauChu.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnChonMauChu.FillColor = Color.Navy;
            btnChonMauChu.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnChonMauChu.ForeColor = Color.White;
            btnChonMauChu.Location = new Point(164, 266);
            btnChonMauChu.Name = "btnChonMauChu";
            btnChonMauChu.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btnChonMauChu.Size = new Size(95, 38);
            btnChonMauChu.TabIndex = 30;
            btnChonMauChu.Text = "Màu chữ";
            btnChonMauChu.Click += btnChonMauChu_Click;
            // 
            // frmCauHinhGiaoDien
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 327);
            Controls.Add(btnChonMauChu);
            Controls.Add(btnChonFont);
            Controls.Add(btnChonMauNen);
            Controls.Add(btnThoat);
            Controls.Add(btnHuy);
            Controls.Add(btnLuu);
            Controls.Add(label1);
            Controls.Add(cboTheme);
            Controls.Add(panelPreview);
            Controls.Add(lblMau);
            Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "frmCauHinhGiaoDien";
            Text = "Cấu hình giao diện";
            Load += frmCauHinhGiaoDien_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ColorDialog colorDialog1;
        private FontDialog fontDialog1;
        private Label lblMau;
        private Panel panelPreview;
        private ComboBox cboTheme;
        private Label label1;
        private Guna.UI2.WinForms.Guna2Button btnThoat;
        private Guna.UI2.WinForms.Guna2Button btnHuy;
        private Guna.UI2.WinForms.Guna2Button btnLuu;
        private Guna.UI2.WinForms.Guna2Button btnChonMauNen;
        private Guna.UI2.WinForms.Guna2Button btnChonFont;
        private Guna.UI2.WinForms.Guna2Button btnChonMauChu;
    }
}