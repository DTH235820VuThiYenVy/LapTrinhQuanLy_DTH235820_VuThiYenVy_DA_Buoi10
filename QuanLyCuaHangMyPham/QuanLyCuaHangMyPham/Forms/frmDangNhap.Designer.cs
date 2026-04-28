namespace QuanLyCuaHangMyPham.Forms
{
    partial class frmDangNhap
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDangNhap));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            groupBox1 = new GroupBox();
            btnHuy = new Guna.UI2.WinForms.Guna2Button();
            btnDangNhap = new Guna.UI2.WinForms.Guna2Button();
            chkHienMatKhau = new CheckBox();
            label4 = new Label();
            label3 = new Label();
            txtTenDangNhap = new TextBox();
            txtMatKhau = new TextBox();
            lnkQuenMatKhau = new LinkLabel();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnHuy);
            groupBox1.Controls.Add(btnDangNhap);
            groupBox1.Controls.Add(chkHienMatKhau);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtTenDangNhap);
            groupBox1.Controls.Add(txtMatKhau);
            groupBox1.Controls.Add(lnkQuenMatKhau);
            groupBox1.Location = new Point(461, 138);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(398, 329);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Enter += groupBox1_Enter;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.Navy;
            btnHuy.CustomizableEdges = customizableEdges5;
            btnHuy.DisabledState.BorderColor = Color.DarkGray;
            btnHuy.DisabledState.CustomBorderColor = Color.DarkGray;
            btnHuy.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnHuy.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnHuy.FillColor = Color.Navy;
            btnHuy.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnHuy.ForeColor = Color.White;
            btnHuy.Image = (Image)resources.GetObject("btnHuy.Image");
            btnHuy.Location = new Point(226, 260);
            btnHuy.Name = "btnHuy";
            btnHuy.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnHuy.Size = new Size(131, 38);
            btnHuy.TabIndex = 23;
            btnHuy.Text = "Hủy";
            btnHuy.Click += btnHuy_Click;
            // 
            // btnDangNhap
            // 
            btnDangNhap.BackColor = Color.Navy;
            btnDangNhap.CustomizableEdges = customizableEdges7;
            btnDangNhap.DisabledState.BorderColor = Color.DarkGray;
            btnDangNhap.DisabledState.CustomBorderColor = Color.DarkGray;
            btnDangNhap.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnDangNhap.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnDangNhap.FillColor = Color.Navy;
            btnDangNhap.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDangNhap.ForeColor = Color.White;
            btnDangNhap.Image = (Image)resources.GetObject("btnDangNhap.Image");
            btnDangNhap.Location = new Point(31, 260);
            btnDangNhap.Name = "btnDangNhap";
            btnDangNhap.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnDangNhap.Size = new Size(131, 38);
            btnDangNhap.TabIndex = 22;
            btnDangNhap.Text = "Đăng nhập";
            btnDangNhap.Click += btnDangNhap_Click;
            // 
            // chkHienMatKhau
            // 
            chkHienMatKhau.AutoSize = true;
            chkHienMatKhau.Location = new Point(213, 214);
            chkHienMatKhau.Name = "chkHienMatKhau";
            chkHienMatKhau.Size = new Size(144, 27);
            chkHienMatKhau.TabIndex = 6;
            chkHienMatKhau.Text = "Hiện mật khẩu";
            chkHienMatKhau.UseVisualStyleBackColor = true;
            chkHienMatKhau.CheckedChanged += chkHienMatKhau_CheckedChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(31, 121);
            label4.Name = "label4";
            label4.Size = new Size(84, 23);
            label4.TabIndex = 5;
            label4.Text = "Mật khẩu";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(31, 26);
            label3.Name = "label3";
            label3.Size = new Size(124, 23);
            label3.TabIndex = 3;
            label3.Text = "Tên đăng nhập";
            // 
            // txtTenDangNhap
            // 
            txtTenDangNhap.BorderStyle = BorderStyle.None;
            txtTenDangNhap.Location = new Point(31, 63);
            txtTenDangNhap.Multiline = true;
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.Size = new Size(326, 42);
            txtTenDangNhap.TabIndex = 0;
            // 
            // txtMatKhau
            // 
            txtMatKhau.BorderStyle = BorderStyle.None;
            txtMatKhau.Location = new Point(31, 160);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Size = new Size(326, 23);
            txtMatKhau.TabIndex = 1;
            txtMatKhau.UseSystemPasswordChar = true;
            txtMatKhau.KeyDown += txtMatKhau_KeyDown;
            // 
            // lnkQuenMatKhau
            // 
            lnkQuenMatKhau.AutoSize = true;
            lnkQuenMatKhau.LinkColor = Color.Black;
            lnkQuenMatKhau.Location = new Point(31, 214);
            lnkQuenMatKhau.Name = "lnkQuenMatKhau";
            lnkQuenMatKhau.Size = new Size(137, 23);
            lnkQuenMatKhau.TabIndex = 3;
            lnkQuenMatKhau.TabStop = true;
            lnkQuenMatKhau.Text = "Quên mật khẩu?";
            lnkQuenMatKhau.LinkClicked += lnkQuenMatKhau_LinkClicked;
            // 
            // frmDangNhap
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(934, 518);
            Controls.Add(groupBox1);
            Font = new Font("Segoe UI", 10F);
            MaximizeBox = false;
            Name = "frmDangNhap";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập";
            Load += frmDangNhap_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private LinkLabel lnkQuenMatKhau;
        private Label label4;
        private Label label3;
        private CheckBox chkHienMatKhau;
        public TextBox txtMatKhau;
        public TextBox txtTenDangNhap;
        private Guna.UI2.WinForms.Guna2Button btnHuy;
        private Guna.UI2.WinForms.Guna2Button btnDangNhap;
    }
}