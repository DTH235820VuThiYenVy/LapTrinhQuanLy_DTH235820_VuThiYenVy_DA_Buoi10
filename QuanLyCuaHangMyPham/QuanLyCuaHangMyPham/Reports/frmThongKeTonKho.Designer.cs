namespace QuanLyCuaHangMyPham.Reports
{
    partial class frmThongKeTonKho
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmThongKeTonKho));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            groupBox2 = new GroupBox();
            cboSLTon = new ComboBox();
            label2 = new Label();
            cboTenNCC = new ComboBox();
            label3 = new Label();
            cboTenHSX = new ComboBox();
            label1 = new Label();
            btnLamMoi = new Guna.UI2.WinForms.Guna2Button();
            btnLoc = new Guna.UI2.WinForms.Guna2Button();
            reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(cboSLTon);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(cboTenNCC);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(cboTenHSX);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(btnLamMoi);
            groupBox2.Controls.Add(btnLoc);
            groupBox2.Location = new Point(12, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1161, 125);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            // 
            // cboSLTon
            // 
            cboSLTon.FormattingEnabled = true;
            cboSLTon.Location = new Point(212, 80);
            cboSLTon.Name = "cboSLTon";
            cboSLTon.Size = new Size(240, 31);
            cboSLTon.TabIndex = 76;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(29, 83);
            label2.Name = "label2";
            label2.Size = new Size(155, 23);
            label2.TabIndex = 75;
            label2.Text = "Trạng thái tồn kho:";
            // 
            // cboTenNCC
            // 
            cboTenNCC.FormattingEnabled = true;
            cboTenNCC.Items.AddRange(new object[] { "" });
            cboTenNCC.Location = new Point(633, 34);
            cboTenNCC.Name = "cboTenNCC";
            cboTenNCC.Size = new Size(240, 31);
            cboTenNCC.TabIndex = 74;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(503, 39);
            label3.Name = "label3";
            label3.Size = new Size(121, 23);
            label3.TabIndex = 73;
            label3.Text = "Nhà cung cấp:";
            // 
            // cboTenHSX
            // 
            cboTenHSX.FormattingEnabled = true;
            cboTenHSX.Location = new Point(212, 34);
            cboTenHSX.Name = "cboTenHSX";
            cboTenHSX.Size = new Size(240, 31);
            cboTenHSX.TabIndex = 72;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(29, 37);
            label1.Name = "label1";
            label1.Size = new Size(124, 23);
            label1.TabIndex = 71;
            label1.Text = "Hãng sản xuất:";
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
            btnLamMoi.Location = new Point(1049, 34);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnLamMoi.Size = new Size(97, 38);
            btnLamMoi.TabIndex = 70;
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
            btnLoc.Location = new Point(946, 34);
            btnLoc.Name = "btnLoc";
            btnLoc.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnLoc.Size = new Size(97, 38);
            btnLoc.TabIndex = 69;
            btnLoc.Text = "Lọc";
            btnLoc.Click += btnLoc_Click;
            // 
            // reportViewer1
            // 
            reportViewer1.Location = new Point(18, 151);
            reportViewer1.Name = "reportViewer1";
            reportViewer1.ServerReport.BearerToken = null;
            reportViewer1.Size = new Size(1155, 355);
            reportViewer1.TabIndex = 3;
            // 
            // frmThongKeTonKho
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1181, 518);
            Controls.Add(reportViewer1);
            Controls.Add(groupBox2);
            Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "frmThongKeTonKho";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Thống kê tồn kho";
            Load += frmThongKeTonKho_Load;
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox2;
        private ComboBox cboSLTon;
        private Label label2;
        private ComboBox cboTenNCC;
        private Label label3;
        private ComboBox cboTenHSX;
        private Label label1;
        private Guna.UI2.WinForms.Guna2Button btnLamMoi;
        private Guna.UI2.WinForms.Guna2Button btnLoc;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
    }
}