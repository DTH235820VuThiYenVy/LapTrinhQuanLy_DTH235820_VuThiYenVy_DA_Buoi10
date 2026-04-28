using Microsoft.EntityFrameworkCore;
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
    public partial class frmLichLamViec : Form
    {
        private TaiKhoan tkHienTai;
        private CoGianGiaoDien _hoTroCoGianForm;
        public frmLichLamViec(TaiKhoan tk)
        {
            InitializeComponent();
            tkHienTai = tk; // Nhận tài khoản
            _hoTroCoGianForm = new CoGianGiaoDien(this); // Khởi tạo trợ giúp co giãn
        }
        private void TaiDuLieuLichLam()
        {
            dgvLichLamViec.AutoGenerateColumns = false;
            using (var context = new QLCHMPDbContext())
            {
                // Lấy ngày từ DateTimePicker
                DateTime tuNgay = dtpTuNgay.Value.Date;
                DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddTicks(-1);

                // CHỈ lấy lịch của nhân viên đang đăng nhập
                var dsLich = context.PhanCa
                    .Include(p => p.CaLamViec)
                    .Where(p => p.MaNV == tkHienTai.MaNV
                             && p.NgayLam >= tuNgay
                             && p.NgayLam <= denNgay)
                    .OrderBy(p => p.NgayLam)
                    .Select(p => new
                    {
                        NgayLam = p.NgayLam,
                        TenCa = p.CaLamViec != null ? p.CaLamViec.TenCa : "Không rõ",
                        GioBatDau = p.CaLamViec != null ? p.CaLamViec.GioBatDau : new TimeSpan(),
                        GioKetThuc = p.CaLamViec != null ? p.CaLamViec.GioKetThuc : new TimeSpan(),
                        GhiChu = p.GhiChu
                    })
                    .ToList();

                dgvLichLamViec.DataSource = dsLich;

                // Format lại cột Ngày làm cho đẹp
                if (dgvLichLamViec.Columns["NgayLam"] != null)
                    dgvLichLamViec.Columns["NgayLam"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void frmLichLamViec_Load(object sender, EventArgs e)
        {
            // Cập nhật câu chào nếu ní có làm Label tiêu đề
            // lblTieuDe.Text = "LỊCH LÀM VIỆC CỦA NHÂN VIÊN: " + tkHienTai.NhanVien.HoTenNV.ToUpper();

            // Setup thời gian mặc định cho 2 ô tìm kiếm (Từ đầu tháng đến cuối tháng)
            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpDenNgay.Value = dtpTuNgay.Value.AddMonths(1).AddDays(-1);

            // Gọi hàm tải dữ liệu lên bảng
            TaiDuLieuLichLam();
        }


        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpDenNgay.Value = dtpTuNgay.Value.AddMonths(1).AddDays(-1);

            TaiDuLieuLichLam();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            if (dtpTuNgay.Value.Date > dtpDenNgay.Value.Date)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!", "Lỗi chọn ngày", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TaiDuLieuLichLam();

            if (dgvLichLamViec.Rows.Count == 0)
            {
                MessageBox.Show("Không có lịch làm việc nào trong khoảng thời gian này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
