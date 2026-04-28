namespace QuanLyCuaHangMyPham.Forms
{
    partial class frmDoiMatKhau
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDoiMatKhau));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            txtMatKhauCu = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtMatKhauMoi = new TextBox();
            label4 = new Label();
            txtXacNhanMatKhau = new TextBox();
            btnHuy = new Guna.UI2.WinForms.Guna2Button();
            btnXacNhan = new Guna.UI2.WinForms.Guna2Button();
            groupBox1 = new GroupBox();
            SuspendLayout();
            // 
            // txtMatKhauCu
            // 
            txtMatKhauCu.Location = new Point(230, 90);
            txtMatKhauCu.Name = "txtMatKhauCu";
            txtMatKhauCu.Size = new Size(314, 30);
            txtMatKhauCu.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(45, 97);
            label1.Name = "label1";
            label1.Size = new Size(114, 23);
            label1.TabIndex = 1;
            label1.Text = "Mật khẩu cũ:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(229, 18);
            label2.Name = "label2";
            label2.Size = new Size(137, 23);
            label2.TabIndex = 2;
            label2.Text = "ĐỔI MẬT KHẨU";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            label3.ForeColor = Color.White;
            label3.Location = new Point(45, 142);
            label3.Name = "label3";
            label3.Size = new Size(128, 23);
            label3.TabIndex = 4;
            label3.Text = "Mật khẩu mới:";
            // 
            // txtMatKhauMoi
            // 
            txtMatKhauMoi.Location = new Point(230, 135);
            txtMatKhauMoi.Name = "txtMatKhauMoi";
            txtMatKhauMoi.Size = new Size(314, 30);
            txtMatKhauMoi.TabIndex = 3;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            label4.ForeColor = Color.White;
            label4.Location = new Point(45, 185);
            label4.Name = "label4";
            label4.Size = new Size(168, 23);
            label4.TabIndex = 6;
            label4.Text = "Xác nhận mật khẩu:";
            // 
            // txtXacNhanMatKhau
            // 
            txtXacNhanMatKhau.Location = new Point(230, 182);
            txtXacNhanMatKhau.Name = "txtXacNhanMatKhau";
            txtXacNhanMatKhau.Size = new Size(314, 30);
            txtXacNhanMatKhau.TabIndex = 5;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.Navy;
            btnHuy.CustomizableEdges = customizableEdges1;
            btnHuy.DisabledState.BorderColor = Color.DarkGray;
            btnHuy.DisabledState.CustomBorderColor = Color.DarkGray;
            btnHuy.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnHuy.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnHuy.FillColor = Color.RoyalBlue;
            btnHuy.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnHuy.ForeColor = Color.White;
            btnHuy.Image = (Image)resources.GetObject("btnHuy.Image");
            btnHuy.Location = new Point(382, 239);
            btnHuy.Name = "btnHuy";
            btnHuy.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnHuy.Size = new Size(95, 38);
            btnHuy.TabIndex = 21;
            btnHuy.Text = "Hủy bỏ";
            btnHuy.Click += btnHuy_Click;
            // 
            // btnXacNhan
            // 
            btnXacNhan.BackColor = Color.Navy;
            btnXacNhan.CustomizableEdges = customizableEdges3;
            btnXacNhan.DisabledState.BorderColor = Color.DarkGray;
            btnXacNhan.DisabledState.CustomBorderColor = Color.DarkGray;
            btnXacNhan.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnXacNhan.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnXacNhan.FillColor = Color.RoyalBlue;
            btnXacNhan.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnXacNhan.ForeColor = Color.White;
            btnXacNhan.Image = (Image)resources.GetObject("btnXacNhan.Image");
            btnXacNhan.Location = new Point(136, 239);
            btnXacNhan.Name = "btnXacNhan";
            btnXacNhan.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnXacNhan.Size = new Size(95, 38);
            btnXacNhan.TabIndex = 22;
            btnXacNhan.Text = "Xác nhận";
            btnXacNhan.Click += btnXacNhan_Click;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.Transparent;
            groupBox1.Location = new Point(21, 60);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(552, 235);
            groupBox1.TabIndex = 23;
            groupBox1.TabStop = false;
            groupBox1.Paint += groupBox1_Paint;
            // 
            // frmDoiMatKhau
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(598, 322);
            Controls.Add(btnXacNhan);
            Controls.Add(btnHuy);
            Controls.Add(label4);
            Controls.Add(txtXacNhanMatKhau);
            Controls.Add(label3);
            Controls.Add(txtMatKhauMoi);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtMatKhauCu);
            Controls.Add(groupBox1);
            Font = new Font("Segoe UI", 10F);
            MaximizeBox = false;
            Name = "frmDoiMatKhau";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đổi mật khẩu";
            Load += frmDoiMatKhau_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtMatKhauCu;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtMatKhauMoi;
        private Label label4;
        private TextBox txtXacNhanMatKhau;
        //private Button btnXacNhan;
        //private Button btnHuy;
        private Guna.UI2.WinForms.Guna2Button btnHuy;
        private Guna.UI2.WinForms.Guna2Button btnXacNhan;
        private GroupBox groupBox1;
    }
}