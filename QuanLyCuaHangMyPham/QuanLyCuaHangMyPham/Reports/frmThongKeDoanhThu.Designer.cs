namespace QuanLyCuaHangMyPham.Reports
{
    partial class frmThongKeDoanhThu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmThongKeDoanhThu));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            groupBox1 = new GroupBox();
            cboPT_ThanhToan = new ComboBox();
            label5 = new Label();
            cboHoTenKH = new ComboBox();
            label3 = new Label();
            cboHoTenNV = new ComboBox();
            label1 = new Label();
            btnLamMoi = new Guna.UI2.WinForms.Guna2Button();
            btnLoc = new Guna.UI2.WinForms.Guna2Button();
            dtpDenNgay = new DateTimePicker();
            dtpTuNgay = new DateTimePicker();
            label4 = new Label();
            label2 = new Label();
            reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cboPT_ThanhToan);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(cboHoTenKH);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(cboHoTenNV);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(btnLamMoi);
            groupBox1.Controls.Add(btnLoc);
            groupBox1.Controls.Add(dtpDenNgay);
            groupBox1.Controls.Add(dtpTuNgay);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label2);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1092, 150);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            // 
            // cboPT_ThanhToan
            // 
            cboPT_ThanhToan.FormattingEnabled = true;
            cboPT_ThanhToan.Location = new Point(832, 33);
            cboPT_ThanhToan.Name = "cboPT_ThanhToan";
            cboPT_ThanhToan.Size = new Size(240, 31);
            cboPT_ThanhToan.TabIndex = 70;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(590, 38);
            label5.Name = "label5";
            label5.Size = new Size(203, 23);
            label5.TabIndex = 69;
            label5.Text = "Phương thức thanh toán:";
            // 
            // cboHoTenKH
            // 
            cboHoTenKH.FormattingEnabled = true;
            cboHoTenKH.Items.AddRange(new object[] { "" });
            cboHoTenKH.Location = new Point(590, 87);
            cboHoTenKH.Name = "cboHoTenKH";
            cboHoTenKH.Size = new Size(240, 31);
            cboHoTenKH.TabIndex = 68;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(460, 92);
            label3.Name = "label3";
            label3.Size = new Size(105, 23);
            label3.TabIndex = 67;
            label3.Text = "Khách hàng:";
            // 
            // cboHoTenNV
            // 
            cboHoTenNV.FormattingEnabled = true;
            cboHoTenNV.Location = new Point(141, 87);
            cboHoTenNV.Name = "cboHoTenNV";
            cboHoTenNV.Size = new Size(240, 31);
            cboHoTenNV.TabIndex = 66;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 95);
            label1.Name = "label1";
            label1.Size = new Size(92, 23);
            label1.TabIndex = 65;
            label1.Text = "Nhân viên:";
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
            btnLamMoi.Location = new Point(975, 87);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnLamMoi.Size = new Size(97, 38);
            btnLamMoi.TabIndex = 64;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnLoc
            // 
            btnLoc.BackColor = Color.Navy;
            btnLoc.CustomizableEdges = customizableEdges3;
            btnLoc.DisabledState.BorderColor = Color.DarkGray;
            btnLoc.DisabledState.CustomBorderColor = Color.DarkGray;
            btnLoc.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnLoc.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnLoc.FillColor = Color.Navy;
            btnLoc.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLoc.ForeColor = Color.White;
            btnLoc.Image = (Image)resources.GetObject("btnLoc.Image");
            btnLoc.Location = new Point(872, 87);
            btnLoc.Name = "btnLoc";
            btnLoc.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnLoc.Size = new Size(97, 38);
            btnLoc.TabIndex = 63;
            btnLoc.Text = "Lọc";
            btnLoc.Click += btnLoc_Click;
            // 
            // dtpDenNgay
            // 
            dtpDenNgay.CustomFormat = "dd/MM/yyyy";
            dtpDenNgay.Format = DateTimePickerFormat.Custom;
            dtpDenNgay.Location = new Point(394, 36);
            dtpDenNgay.Name = "dtpDenNgay";
            dtpDenNgay.Size = new Size(149, 30);
            dtpDenNgay.TabIndex = 60;
            // 
            // dtpTuNgay
            // 
            dtpTuNgay.CustomFormat = "dd/MM/yyyy";
            dtpTuNgay.Format = DateTimePickerFormat.Custom;
            dtpTuNgay.Location = new Point(107, 36);
            dtpTuNgay.Name = "dtpTuNgay";
            dtpTuNgay.Size = new Size(149, 30);
            dtpTuNgay.TabIndex = 59;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(305, 41);
            label4.Name = "label4";
            label4.Size = new Size(83, 23);
            label4.TabIndex = 62;
            label4.Text = "Đến ngày";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 41);
            label2.Name = "label2";
            label2.Size = new Size(75, 23);
            label2.TabIndex = 61;
            label2.Text = "Từ ngày:";
            // 
            // reportViewer1
            // 
            reportViewer1.Location = new Point(12, 182);
            reportViewer1.Name = "reportViewer1";
            reportViewer1.ServerReport.BearerToken = null;
            reportViewer1.Size = new Size(1092, 324);
            reportViewer1.TabIndex = 1;
            // 
            // frmThongKeDoanhThu
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1116, 518);
            Controls.Add(reportViewer1);
            Controls.Add(groupBox1);
            Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "frmThongKeDoanhThu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Thống kê doanh thu";
            Load += frmThongKeDoanhThu_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private Guna.UI2.WinForms.Guna2Button btnLamMoi;
        private Guna.UI2.WinForms.Guna2Button btnLoc;
        private DateTimePicker dtpDenNgay;
        private DateTimePicker dtpTuNgay;
        private Label label4;
        private Label label2;
        private ComboBox cboHoTenKH;
        private Label label3;
        private ComboBox cboHoTenNV;
        private Label label1;
        private ComboBox cboPT_ThanhToan;
        private Label label5;
    }
}