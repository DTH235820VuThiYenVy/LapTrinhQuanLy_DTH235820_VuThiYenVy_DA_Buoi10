namespace QuanLyCuaHangMyPham.Forms
{
    partial class frmLichSuHoatDong
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLichSuHoatDong));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            groupBox1 = new GroupBox();
            btnLamMoi = new Guna.UI2.WinForms.Guna2Button();
            btnXuat = new Guna.UI2.WinForms.Guna2Button();
            btnTimVaLoc = new Guna.UI2.WinForms.Guna2Button();
            txtTuKhoa = new TextBox();
            label4 = new Label();
            label3 = new Label();
            cboHoTenNV = new ComboBox();
            label2 = new Label();
            dtpDenNgay = new DateTimePicker();
            label1 = new Label();
            dtpTuNgay = new DateTimePicker();
            groupBox2 = new GroupBox();
            dgvDSLSHD = new DataGridView();
            MaLS = new DataGridViewTextBoxColumn();
            ThoiGian = new DataGridViewTextBoxColumn();
            NguoiThucHien = new DataGridViewTextBoxColumn();
            HanhDong = new DataGridViewTextBoxColumn();
            TenTK = new DataGridViewTextBoxColumn();
            btnThoat = new Guna.UI2.WinForms.Guna2Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDSLSHD).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnLamMoi);
            groupBox1.Controls.Add(btnXuat);
            groupBox1.Controls.Add(btnTimVaLoc);
            groupBox1.Controls.Add(txtTuKhoa);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(cboHoTenNV);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(dtpDenNgay);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(dtpTuNgay);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(876, 180);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Bộ lọc - Tìm kiếm";
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = Color.Navy;
            btnLamMoi.CustomizableEdges = customizableEdges1;
            btnLamMoi.DisabledState.BorderColor = Color.DarkGray;
            btnLamMoi.DisabledState.CustomBorderColor = Color.DarkGray;
            btnLamMoi.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnLamMoi.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnLamMoi.FillColor = Color.Navy;
            btnLamMoi.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLamMoi.ForeColor = Color.White;
            btnLamMoi.Image = (Image)resources.GetObject("btnLamMoi.Image");
            btnLamMoi.Location = new Point(714, 103);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnLamMoi.Size = new Size(116, 38);
            btnLamMoi.TabIndex = 40;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnXuat
            // 
            btnXuat.BackColor = Color.Navy;
            btnXuat.CustomizableEdges = customizableEdges3;
            btnXuat.DisabledState.BorderColor = Color.DarkGray;
            btnXuat.DisabledState.CustomBorderColor = Color.DarkGray;
            btnXuat.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnXuat.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnXuat.FillColor = Color.Navy;
            btnXuat.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnXuat.ForeColor = Color.White;
            btnXuat.Image = (Image)resources.GetObject("btnXuat.Image");
            btnXuat.Location = new Point(591, 103);
            btnXuat.Name = "btnXuat";
            btnXuat.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnXuat.Size = new Size(116, 38);
            btnXuat.TabIndex = 40;
            btnXuat.Text = "Xuất Excel";
            btnXuat.Click += btnXuat_Click;
            // 
            // btnTimVaLoc
            // 
            btnTimVaLoc.BackColor = Color.Navy;
            btnTimVaLoc.CustomizableEdges = customizableEdges5;
            btnTimVaLoc.DisabledState.BorderColor = Color.DarkGray;
            btnTimVaLoc.DisabledState.CustomBorderColor = Color.DarkGray;
            btnTimVaLoc.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnTimVaLoc.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnTimVaLoc.FillColor = Color.Navy;
            btnTimVaLoc.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnTimVaLoc.ForeColor = Color.White;
            btnTimVaLoc.Image = (Image)resources.GetObject("btnTimVaLoc.Image");
            btnTimVaLoc.Location = new Point(591, 50);
            btnTimVaLoc.Name = "btnTimVaLoc";
            btnTimVaLoc.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnTimVaLoc.Size = new Size(239, 38);
            btnTimVaLoc.TabIndex = 39;
            btnTimVaLoc.Text = "Tìm kiếm / Lọc";
            btnTimVaLoc.Click += btnTimVaLoc_Click;
            // 
            // txtTuKhoa
            // 
            txtTuKhoa.BorderStyle = BorderStyle.FixedSingle;
            txtTuKhoa.Location = new Point(200, 139);
            txtTuKhoa.Name = "txtTuKhoa";
            txtTuKhoa.Size = new Size(317, 30);
            txtTuKhoa.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(72, 136);
            label4.Name = "label4";
            label4.Size = new Size(75, 23);
            label4.TabIndex = 6;
            label4.Text = "Từ khóa:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(72, 87);
            label3.Name = "label3";
            label3.Size = new Size(92, 23);
            label3.TabIndex = 5;
            label3.Text = "Nhân viên:";
            // 
            // cboHoTenNV
            // 
            cboHoTenNV.FormattingEnabled = true;
            cboHoTenNV.Location = new Point(199, 87);
            cboHoTenNV.Name = "cboHoTenNV";
            cboHoTenNV.Size = new Size(318, 31);
            cboHoTenNV.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(308, 32);
            label2.Name = "label2";
            label2.Size = new Size(45, 23);
            label2.TabIndex = 3;
            label2.Text = "Đến:";
            // 
            // dtpDenNgay
            // 
            dtpDenNgay.CustomFormat = "dd/MM/yyyy";
            dtpDenNgay.Format = DateTimePickerFormat.Custom;
            dtpDenNgay.Location = new Point(377, 29);
            dtpDenNgay.Name = "dtpDenNgay";
            dtpDenNgay.Size = new Size(140, 30);
            dtpDenNgay.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(72, 34);
            label1.Name = "label1";
            label1.Size = new Size(33, 23);
            label1.TabIndex = 1;
            label1.Text = "Từ:";
            // 
            // dtpTuNgay
            // 
            dtpTuNgay.CustomFormat = "dd/MM/yyyy";
            dtpTuNgay.Format = DateTimePickerFormat.Custom;
            dtpTuNgay.Location = new Point(129, 29);
            dtpTuNgay.Name = "dtpTuNgay";
            dtpTuNgay.Size = new Size(140, 30);
            dtpTuNgay.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dgvDSLSHD);
            groupBox2.Location = new Point(0, 251);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(900, 267);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Danh sách lịch sử hoạt động";
            // 
            // dgvDSLSHD
            // 
            dgvDSLSHD.AllowUserToOrderColumns = true;
            dgvDSLSHD.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.Navy;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvDSLSHD.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvDSLSHD.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDSLSHD.Columns.AddRange(new DataGridViewColumn[] { MaLS, ThoiGian, NguoiThucHien, HanhDong, TenTK });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvDSLSHD.DefaultCellStyle = dataGridViewCellStyle3;
            dgvDSLSHD.Dock = DockStyle.Fill;
            dgvDSLSHD.EnableHeadersVisualStyles = false;
            dgvDSLSHD.Location = new Point(3, 26);
            dgvDSLSHD.MultiSelect = false;
            dgvDSLSHD.Name = "dgvDSLSHD";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.Navy;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle4.ForeColor = Color.White;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvDSLSHD.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvDSLSHD.RowHeadersVisible = false;
            dgvDSLSHD.RowHeadersWidth = 51;
            dgvDSLSHD.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDSLSHD.Size = new Size(894, 238);
            dgvDSLSHD.TabIndex = 0;
            // 
            // MaLS
            // 
            MaLS.DataPropertyName = "MaLS";
            MaLS.HeaderText = "Mã lịch sử";
            MaLS.MinimumWidth = 6;
            MaLS.Name = "MaLS";
            MaLS.Visible = false;
            // 
            // ThoiGian
            // 
            ThoiGian.DataPropertyName = "ThoiGian";
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.Blue;
            dataGridViewCellStyle2.Format = "dd/MM/yyyy HH:mm:ss";
            ThoiGian.DefaultCellStyle = dataGridViewCellStyle2;
            ThoiGian.HeaderText = "Thời gian";
            ThoiGian.MinimumWidth = 6;
            ThoiGian.Name = "ThoiGian";
            // 
            // NguoiThucHien
            // 
            NguoiThucHien.DataPropertyName = "HoTenNV";
            NguoiThucHien.HeaderText = "Người thực hiện";
            NguoiThucHien.MinimumWidth = 6;
            NguoiThucHien.Name = "NguoiThucHien";
            NguoiThucHien.ReadOnly = true;
            // 
            // HanhDong
            // 
            HanhDong.DataPropertyName = "HanhDong";
            HanhDong.HeaderText = "Hành động";
            HanhDong.MinimumWidth = 6;
            HanhDong.Name = "HanhDong";
            HanhDong.ReadOnly = true;
            // 
            // TenTK
            // 
            TenTK.DataPropertyName = "TenTK";
            TenTK.HeaderText = "Tên tài khoản";
            TenTK.MinimumWidth = 6;
            TenTK.Name = "TenTK";
            TenTK.ReadOnly = true;
            // 
            // btnThoat
            // 
            btnThoat.BackColor = Color.Navy;
            btnThoat.CustomizableEdges = customizableEdges7;
            btnThoat.DisabledState.BorderColor = Color.DarkGray;
            btnThoat.DisabledState.CustomBorderColor = Color.DarkGray;
            btnThoat.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnThoat.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnThoat.FillColor = Color.Navy;
            btnThoat.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnThoat.ForeColor = Color.White;
            btnThoat.Image = (Image)resources.GetObject("btnThoat.Image");
            btnThoat.Location = new Point(603, 207);
            btnThoat.Name = "btnThoat";
            btnThoat.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnThoat.Size = new Size(239, 38);
            btnThoat.TabIndex = 36;
            btnThoat.Text = "Thoát";
            btnThoat.Click += btnThoat_Click;
            // 
            // frmLichSuHoatDong
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 502);
            Controls.Add(btnThoat);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Font = new Font("Segoe UI", 10F);
            Name = "frmLichSuHoatDong";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Lịch sử hoạt động";
            Load += frmLichSuHoatDong_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDSLSHD).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private DataGridView dgvDSLSHD;
        private Label label4;
        private Label label3;
        private ComboBox cboHoTenNV;
        private Label label2;
        private DateTimePicker dtpDenNgay;
        private Label label1;
        private DateTimePicker dtpTuNgay;
        private TextBox txtTuKhoa;
        private Guna.UI2.WinForms.Guna2Button btnTimVaLoc;
        private Guna.UI2.WinForms.Guna2Button btnXuat;
        private Guna.UI2.WinForms.Guna2Button btnLamMoi;
        private DataGridViewTextBoxColumn MaLS;
        private DataGridViewTextBoxColumn ThoiGian;
        private DataGridViewTextBoxColumn NguoiThucHien;
        private DataGridViewTextBoxColumn HanhDong;
        private DataGridViewTextBoxColumn TenTK;
        private Guna.UI2.WinForms.Guna2Button btnThoat;
    }
}