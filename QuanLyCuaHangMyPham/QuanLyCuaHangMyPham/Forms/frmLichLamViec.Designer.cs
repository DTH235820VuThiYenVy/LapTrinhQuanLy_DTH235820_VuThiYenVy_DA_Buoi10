namespace QuanLyCuaHangMyPham.Forms
{
    partial class frmLichLamViec
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLichLamViec));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            groupBox2 = new GroupBox();
            dgvLichLamViec = new DataGridView();
            NgayLam = new DataGridViewTextBoxColumn();
            TenCa = new DataGridViewTextBoxColumn();
            GioBatDau = new DataGridViewTextBoxColumn();
            GioKetThuc = new DataGridViewTextBoxColumn();
            GhiChu = new DataGridViewTextBoxColumn();
            groupBox1 = new GroupBox();
            btnThoat = new Guna.UI2.WinForms.Guna2Button();
            btnLamMoi = new Guna.UI2.WinForms.Guna2Button();
            btnLoc = new Guna.UI2.WinForms.Guna2Button();
            dtpDenNgay = new DateTimePicker();
            dtpTuNgay = new DateTimePicker();
            label4 = new Label();
            label2 = new Label();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLichLamViec).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dgvLichLamViec);
            groupBox2.Location = new Point(12, 143);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1254, 276);
            groupBox2.TabIndex = 8;
            groupBox2.TabStop = false;
            groupBox2.Text = "Lịch làm việc";
            // 
            // dgvLichLamViec
            // 
            dgvLichLamViec.AllowUserToAddRows = false;
            dgvLichLamViec.AllowUserToDeleteRows = false;
            dgvLichLamViec.AllowUserToOrderColumns = true;
            dgvLichLamViec.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.Navy;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvLichLamViec.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvLichLamViec.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLichLamViec.Columns.AddRange(new DataGridViewColumn[] { NgayLam, TenCa, GioBatDau, GioKetThuc, GhiChu });
            dgvLichLamViec.Dock = DockStyle.Fill;
            dgvLichLamViec.EnableHeadersVisualStyles = false;
            dgvLichLamViec.Location = new Point(3, 26);
            dgvLichLamViec.MultiSelect = false;
            dgvLichLamViec.Name = "dgvLichLamViec";
            dgvLichLamViec.RowHeadersVisible = false;
            dgvLichLamViec.RowHeadersWidth = 51;
            dgvLichLamViec.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLichLamViec.Size = new Size(1248, 247);
            dgvLichLamViec.TabIndex = 0;
            // 
            // NgayLam
            // 
            NgayLam.DataPropertyName = "NgayLam";
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.Blue;
            dataGridViewCellStyle2.Format = "dd/MM/yyyy";
            NgayLam.DefaultCellStyle = dataGridViewCellStyle2;
            NgayLam.HeaderText = "Ngày làm";
            NgayLam.MinimumWidth = 6;
            NgayLam.Name = "NgayLam";
            // 
            // TenCa
            // 
            TenCa.DataPropertyName = "TenCa";
            TenCa.HeaderText = "Tên ca";
            TenCa.MinimumWidth = 6;
            TenCa.Name = "TenCa";
            // 
            // GioBatDau
            // 
            GioBatDau.DataPropertyName = "GioBatDau";
            GioBatDau.HeaderText = "Giờ bắt đầu";
            GioBatDau.MinimumWidth = 6;
            GioBatDau.Name = "GioBatDau";
            // 
            // GioKetThuc
            // 
            GioKetThuc.DataPropertyName = "GioKetThuc";
            GioKetThuc.HeaderText = "Giờ kết thúc";
            GioKetThuc.MinimumWidth = 6;
            GioKetThuc.Name = "GioKetThuc";
            // 
            // GhiChu
            // 
            GhiChu.DataPropertyName = "GhiChu";
            GhiChu.HeaderText = "Ghi chú";
            GhiChu.MinimumWidth = 6;
            GhiChu.Name = "GhiChu";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnThoat);
            groupBox1.Controls.Add(btnLamMoi);
            groupBox1.Controls.Add(btnLoc);
            groupBox1.Controls.Add(dtpDenNgay);
            groupBox1.Controls.Add(dtpTuNgay);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label2);
            groupBox1.Location = new Point(153, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(991, 113);
            groupBox1.TabIndex = 48;
            groupBox1.TabStop = false;
            groupBox1.Text = "Lọc và Tìm kiếm";
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
            btnThoat.Location = new Point(844, 44);
            btnThoat.Name = "btnThoat";
            btnThoat.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnThoat.Size = new Size(116, 38);
            btnThoat.TabIndex = 59;
            btnThoat.Text = "Thoát";
            btnThoat.Click += btnThoat_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = Color.Navy;
            btnLamMoi.CustomizableEdges = customizableEdges3;
            btnLamMoi.DisabledState.BorderColor = Color.DarkGray;
            btnLamMoi.DisabledState.CustomBorderColor = Color.DarkGray;
            btnLamMoi.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnLamMoi.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnLamMoi.FillColor = Color.Navy;
            btnLamMoi.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLamMoi.ForeColor = Color.White;
            btnLamMoi.Image = (Image)resources.GetObject("btnLamMoi.Image");
            btnLamMoi.Location = new Point(691, 44);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnLamMoi.Size = new Size(116, 38);
            btnLamMoi.TabIndex = 58;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnLoc
            // 
            btnLoc.BackColor = Color.Navy;
            btnLoc.CustomizableEdges = customizableEdges5;
            btnLoc.DisabledState.BorderColor = Color.DarkGray;
            btnLoc.DisabledState.CustomBorderColor = Color.DarkGray;
            btnLoc.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnLoc.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnLoc.FillColor = Color.Navy;
            btnLoc.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLoc.ForeColor = Color.White;
            btnLoc.Image = (Image)resources.GetObject("btnLoc.Image");
            btnLoc.Location = new Point(538, 44);
            btnLoc.Name = "btnLoc";
            btnLoc.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnLoc.Size = new Size(116, 38);
            btnLoc.TabIndex = 56;
            btnLoc.Text = "Lọc";
            btnLoc.Click += btnLoc_Click;
            // 
            // dtpDenNgay
            // 
            dtpDenNgay.CustomFormat = "dd/MM/yyyy";
            dtpDenNgay.Format = DateTimePickerFormat.Custom;
            dtpDenNgay.Location = new Point(346, 44);
            dtpDenNgay.Name = "dtpDenNgay";
            dtpDenNgay.Size = new Size(149, 30);
            dtpDenNgay.TabIndex = 42;
            // 
            // dtpTuNgay
            // 
            dtpTuNgay.CustomFormat = "dd/MM/yyyy";
            dtpTuNgay.Format = DateTimePickerFormat.Custom;
            dtpTuNgay.Location = new Point(102, 44);
            dtpTuNgay.Name = "dtpTuNgay";
            dtpTuNgay.Size = new Size(149, 30);
            dtpTuNgay.TabIndex = 41;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(257, 49);
            label4.Name = "label4";
            label4.Size = new Size(83, 23);
            label4.TabIndex = 44;
            label4.Text = "Đến ngày";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(22, 49);
            label2.Name = "label2";
            label2.Size = new Size(75, 23);
            label2.TabIndex = 43;
            label2.Text = "Từ ngày:";
            // 
            // frmLichLamViec
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1274, 431);
            Controls.Add(groupBox1);
            Controls.Add(groupBox2);
            Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "frmLichLamViec";
            Text = "Lịch làm việc";
            Load += frmLichLamViec_Load;
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLichLamViec).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private GroupBox groupBox2;
        private DataGridView dgvLichLamViec;
        private DataGridViewTextBoxColumn NgayLam;
        private DataGridViewTextBoxColumn TenCa;
        private DataGridViewTextBoxColumn GioBatDau;
        private DataGridViewTextBoxColumn GioKetThuc;
        private DataGridViewTextBoxColumn GhiChu;
        private GroupBox groupBox1;
        private Guna.UI2.WinForms.Guna2Button btnLamMoi;
        private Guna.UI2.WinForms.Guna2Button btnLoc;
        private DateTimePicker dtpDenNgay;
        private DateTimePicker dtpTuNgay;
        private Label label4;
        private Label label2;
        private Guna.UI2.WinForms.Guna2Button btnThoat;
    }
}