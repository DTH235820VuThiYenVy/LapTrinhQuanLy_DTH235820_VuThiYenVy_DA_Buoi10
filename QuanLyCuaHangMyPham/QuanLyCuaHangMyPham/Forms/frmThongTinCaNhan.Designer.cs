namespace QuanLyCuaHangMyPham.Forms
{
    partial class frmThongTinCaNhan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmThongTinCaNhan));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            groupBox1 = new GroupBox();
            radNu = new RadioButton();
            radNam = new RadioButton();
            label10 = new Label();
            txtLuongCoBan = new TextBox();
            txtChucVu = new TextBox();
            label9 = new Label();
            txtEmail = new TextBox();
            dtpNgayVao = new DateTimePicker();
            label7 = new Label();
            label8 = new Label();
            dtpNgaySinh = new DateTimePicker();
            label5 = new Label();
            label6 = new Label();
            label3 = new Label();
            label4 = new Label();
            txtSDT = new TextBox();
            txtDiaChi = new TextBox();
            txtMaNV = new TextBox();
            txtHoTenNV = new TextBox();
            label2 = new Label();
            label1 = new Label();
            btnThoat = new Guna.UI2.WinForms.Guna2Button();
            btnLuu = new Guna.UI2.WinForms.Guna2Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.Transparent;
            groupBox1.Controls.Add(btnThoat);
            groupBox1.Controls.Add(radNu);
            groupBox1.Controls.Add(btnLuu);
            groupBox1.Controls.Add(radNam);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(txtLuongCoBan);
            groupBox1.Controls.Add(txtChucVu);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(txtEmail);
            groupBox1.Controls.Add(dtpNgayVao);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(dtpNgaySinh);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(txtSDT);
            groupBox1.Controls.Add(txtDiaChi);
            groupBox1.Controls.Add(txtMaNV);
            groupBox1.Controls.Add(txtHoTenNV);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.ForeColor = Color.White;
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(925, 335);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Thông tin cá nhân";
            groupBox1.Paint += groupBox1_Paint;
            // 
            // radNu
            // 
            radNu.AutoSize = true;
            radNu.Location = new Point(768, 220);
            radNu.Name = "radNu";
            radNu.Size = new Size(54, 27);
            radNu.TabIndex = 35;
            radNu.TabStop = true;
            radNu.Text = "Nữ";
            radNu.UseVisualStyleBackColor = true;
            // 
            // radNam
            // 
            radNam.AutoSize = true;
            radNam.Location = new Point(594, 218);
            radNam.Name = "radNam";
            radNam.Size = new Size(68, 27);
            radNam.TabIndex = 34;
            radNam.TabStop = true;
            radNam.Text = "Nam";
            radNam.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(492, 222);
            label10.Name = "label10";
            label10.Size = new Size(79, 23);
            label10.TabIndex = 33;
            label10.Text = "Giới tính:";
            // 
            // txtLuongCoBan
            // 
            txtLuongCoBan.BorderStyle = BorderStyle.FixedSingle;
            txtLuongCoBan.Location = new Point(152, 167);
            txtLuongCoBan.Name = "txtLuongCoBan";
            txtLuongCoBan.ReadOnly = true;
            txtLuongCoBan.Size = new Size(308, 30);
            txtLuongCoBan.TabIndex = 32;
            // 
            // txtChucVu
            // 
            txtChucVu.BorderStyle = BorderStyle.FixedSingle;
            txtChucVu.Location = new Point(153, 122);
            txtChucVu.Name = "txtChucVu";
            txtChucVu.ReadOnly = true;
            txtChucVu.Size = new Size(308, 30);
            txtChucVu.TabIndex = 31;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(15, 222);
            label9.Name = "label9";
            label9.Size = new Size(55, 23);
            label9.TabIndex = 29;
            label9.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Location = new Point(152, 219);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(308, 30);
            txtEmail.TabIndex = 30;
            // 
            // dtpNgayVao
            // 
            dtpNgayVao.CustomFormat = "dd/MM/yyyy";
            dtpNgayVao.Enabled = false;
            dtpNgayVao.Format = DateTimePickerFormat.Custom;
            dtpNgayVao.Location = new Point(594, 167);
            dtpNgayVao.Name = "dtpNgayVao";
            dtpNgayVao.Size = new Size(305, 30);
            dtpNgayVao.TabIndex = 27;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(492, 178);
            label7.Name = "label7";
            label7.Size = new Size(86, 23);
            label7.TabIndex = 25;
            label7.Text = "Ngày vào:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(15, 167);
            label8.Name = "label8";
            label8.Size = new Size(119, 23);
            label8.TabIndex = 26;
            label8.Text = "Lương cơ bản:";
            // 
            // dtpNgaySinh
            // 
            dtpNgaySinh.CustomFormat = "dd/MM/yyyy";
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            dtpNgaySinh.Location = new Point(591, 124);
            dtpNgaySinh.Name = "dtpNgaySinh";
            dtpNgaySinh.Size = new Size(305, 30);
            dtpNgaySinh.TabIndex = 23;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(492, 124);
            label5.Name = "label5";
            label5.Size = new Size(90, 23);
            label5.TabIndex = 16;
            label5.Text = "Ngày sinh:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(15, 120);
            label6.Name = "label6";
            label6.Size = new Size(76, 23);
            label6.TabIndex = 18;
            label6.Text = "Chức vụ:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(492, 84);
            label3.Name = "label3";
            label3.Size = new Size(66, 23);
            label3.TabIndex = 12;
            label3.Text = "Địa chỉ:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(492, 38);
            label4.Name = "label4";
            label4.Size = new Size(93, 23);
            label4.TabIndex = 14;
            label4.Text = "Điện thoại:";
            // 
            // txtSDT
            // 
            txtSDT.BorderStyle = BorderStyle.FixedSingle;
            txtSDT.Location = new Point(591, 34);
            txtSDT.Name = "txtSDT";
            txtSDT.Size = new Size(308, 30);
            txtSDT.TabIndex = 15;
            // 
            // txtDiaChi
            // 
            txtDiaChi.BorderStyle = BorderStyle.FixedSingle;
            txtDiaChi.Location = new Point(591, 77);
            txtDiaChi.Name = "txtDiaChi";
            txtDiaChi.Size = new Size(308, 30);
            txtDiaChi.TabIndex = 13;
            // 
            // txtMaNV
            // 
            txtMaNV.BorderStyle = BorderStyle.FixedSingle;
            txtMaNV.Location = new Point(153, 34);
            txtMaNV.Name = "txtMaNV";
            txtMaNV.ReadOnly = true;
            txtMaNV.Size = new Size(308, 30);
            txtMaNV.TabIndex = 11;
            // 
            // txtHoTenNV
            // 
            txtHoTenNV.BorderStyle = BorderStyle.FixedSingle;
            txtHoTenNV.Location = new Point(153, 77);
            txtHoTenNV.Name = "txtHoTenNV";
            txtHoTenNV.Size = new Size(308, 30);
            txtHoTenNV.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 80);
            label2.Name = "label2";
            label2.Size = new Size(88, 23);
            label2.TabIndex = 8;
            label2.Text = "Họ và tên:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 38);
            label1.Name = "label1";
            label1.Size = new Size(118, 23);
            label1.TabIndex = 10;
            label1.Text = "Mã nhân viên:";
            // 
            // btnThoat
            // 
            btnThoat.BackColor = Color.Navy;
            btnThoat.CustomizableEdges = customizableEdges1;
            btnThoat.DisabledState.BorderColor = Color.DarkGray;
            btnThoat.DisabledState.CustomBorderColor = Color.DarkGray;
            btnThoat.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnThoat.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnThoat.FillColor = Color.RoyalBlue;
            btnThoat.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnThoat.ForeColor = Color.White;
            btnThoat.Image = (Image)resources.GetObject("btnThoat.Image");
            btnThoat.Location = new Point(517, 276);
            btnThoat.Name = "btnThoat";
            btnThoat.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnThoat.Size = new Size(145, 38);
            btnThoat.TabIndex = 29;
            btnThoat.Text = "Thoát";
            btnThoat.Click += btnThoat_Click;
            // 
            // btnLuu
            // 
            btnLuu.BackColor = Color.Navy;
            btnLuu.CustomizableEdges = customizableEdges3;
            btnLuu.DisabledState.BorderColor = Color.DarkGray;
            btnLuu.DisabledState.CustomBorderColor = Color.DarkGray;
            btnLuu.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnLuu.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnLuu.FillColor = Color.RoyalBlue;
            btnLuu.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLuu.ForeColor = Color.White;
            btnLuu.Image = (Image)resources.GetObject("btnLuu.Image");
            btnLuu.Location = new Point(284, 276);
            btnLuu.Name = "btnLuu";
            btnLuu.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnLuu.Size = new Size(145, 38);
            btnLuu.TabIndex = 28;
            btnLuu.Text = "Cập nhật";
            btnLuu.Click += btnLuu_Click;
            // 
            // frmThongTinCaNhan
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(950, 356);
            Controls.Add(groupBox1);
            Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "frmThongTinCaNhan";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Thông tin cá nhân";
            Load += frmThongTinCaNhan_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Label label9;
        private TextBox txtEmail;
        private DateTimePicker dtpNgayVao;
        private Label label7;
        private Label label8;
        private DateTimePicker dtpNgaySinh;
        private Label label5;
        private Label label6;
        private Label label3;
        private Label label4;
        private TextBox txtSDT;
        private TextBox txtDiaChi;
        private TextBox txtMaNV;
        private TextBox txtHoTenNV;
        private Label label2;
        private Label label1;
        private TextBox txtLuongCoBan;
        private TextBox txtChucVu;
        private Label label10;
        private RadioButton radNu;
        private RadioButton radNam;
        private Guna.UI2.WinForms.Guna2Button btnThoat;
        private Guna.UI2.WinForms.Guna2Button btnLuu;
    }
}