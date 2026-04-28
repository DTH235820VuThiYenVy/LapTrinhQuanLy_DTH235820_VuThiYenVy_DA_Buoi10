using ClosedXML.Excel;
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
using ClosedXML.Excel;

namespace QuanLyCuaHangMyPham.Forms
{
    public partial class frmSanPham : Form
    {
        private QLCHMPDbContext _context = new QLCHMPDbContext();
        private bool _xuLyThem = false;
        private List<string> _listPaths = new List<string>(); // Danh sách đường dẫn ảnh tạm
        private CoGianGiaoDien _hoTroCoGianForm;
        private string _maSP = "";
        private string _pathHienTai = "";
        private int _maTKDangNhap;

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lParam);
        private const int EM_SETCUEBANNER = 0x1501;
        public frmSanPham(int maTKDangNhap)
        {
            InitializeComponent();
            _hoTroCoGianForm = new CoGianGiaoDien(this);
            _maTKDangNhap = maTKDangNhap;
        }

        private string LuuAnhVaoThuMucLocal(string pathGoc)
        {
            try
            {
                // 1. Tạo đường dẫn đến thư mục Images trong bin/Debug
                string folderPath = Path.Combine(Application.StartupPath, "Images");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                // 2. Lấy tên file ngắn gọn (ví dụ: srm.jpg)
                string fileName = Path.GetFileName(pathGoc);
                string pathDich = Path.Combine(folderPath, fileName);

                // 3. Nếu chưa có ảnh này trong thư mục thì mới Copy
                if (!File.Exists(pathDich))
                {
                    File.Copy(pathGoc, pathDich, true);
                }
                return fileName; // Trả về cái tên ngắn để lưu vào SQL
            }
            catch { return ""; }
        }

        private void BatTatChucNang(bool giaTri)
        {
            // Các ô nhập liệu
            txtTenSP.Enabled = giaTri;
            cboLoaiSP.Enabled = giaTri;
            cboHangSX.Enabled = giaTri;
            cboTenNCC.Enabled = giaTri;
            numSLTon.Enabled = giaTri;
            numGiaNhap.Enabled = giaTri;
            numGiaBan.Enabled = giaTri;
            ckbTrangThai.Enabled = giaTri;

            // Nút ảnh luôn cho dùng (như ní đã sửa)
            btnThemAnh.Enabled = true;
            btnXoaAnh.Enabled = true;

            txtMaSP.Enabled = false;

            // QUAN TRỌNG: Các nút điều hướng
            btnLuu.Enabled = giaTri;   // Chỉ sáng khi đang Thêm hoặc Sửa
            btnHuy.Enabled = giaTri;   // Chỉ sáng khi đang Thêm hoặc Sửa

            btnThem.Enabled = !giaTri; // Tắt nút Thêm khi đang nhập liệu
            btnSua.Enabled = !giaTri;  // Tắt nút Sửa khi đang nhập liệu
            btnXoa.Enabled = !giaTri;  // Tắt nút Xóa khi đang nhập liệu
        }

        private void LayDuLieuVaoComboBox()
        {
            cboLoaiSP.DataSource = _context.LoaiSanPham.ToList();
            cboLoaiSP.DisplayMember = "TenLoai";
            cboLoaiSP.ValueMember = "MaLoai";
            cboLoaiSP.SelectedIndex = -1;

            cboHangSX.DataSource = _context.HangSanXuat.ToList();
            cboHangSX.DisplayMember = "TenHSX";
            cboHangSX.ValueMember = "MaHSX";
            cboHangSX.SelectedIndex = -1;

            cboTenNCC.DataSource = _context.NhaCungCap.ToList();
            cboTenNCC.DisplayMember = "TenNCC";
            cboTenNCC.ValueMember = "MaNCC";
            cboTenNCC.SelectedIndex = -1;
        }
        private void TaiDuLieuLenBang()
        {
            _context = new QLCHMPDbContext();

            var dsSanPhamHienThi = _context.SanPham.Select(p => new DanhSachSanPham
            {
                MaSP = p.MaSP,
                TenSP = p.TenSP,
                //TenLoai = p.LoaiSanPham.TenLoai,
                //TenHSX = p.HangSanXuat.TenHSX,
                //TenNCC = p.NhaCungCap.TenNCC,

                TenLoai = p.LoaiSanPham != null ? p.LoaiSanPham.TenLoai : "",
                TenHSX = p.HangSanXuat != null ? p.HangSanXuat.TenHSX : "",
                TenNCC = p.NhaCungCap != null ? p.NhaCungCap.TenNCC : "",

                GiaNhap = p.GiaNhap,

                GiaBan = p.GiaBan,
                SLTon = p.SLTon,
                HinhAnh = p.HinhAnh,
                TrangThai = p.TrangThai ? "Đang kinh doanh" : "Ngừng kinh doanh"
            }).ToList();

            dgvDSSanPham.DataSource = dsSanPhamHienThi;

            if (dgvDSSanPham.Columns["GiaBan"] != null)
                dgvDSSanPham.Columns["GiaBan"].DefaultCellStyle.Format = "N0";

            if (dgvDSSanPham.Columns["GiaNhap"] != null)
                dgvDSSanPham.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";

            bool coDuLieu = dsSanPhamHienThi.Count > 0;
            btnXoa.Enabled = coDuLieu;
            btnSua.Enabled = coDuLieu;
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void frmSanPham_Load(object sender, EventArgs e)
        {
            TaiDuLieuLenBang();

            LayDuLieuVaoComboBox();

            BatTatChucNang(false);

            SendMessage(txtTimKiem.Handle, EM_SETCUEBANNER, 1, "Nhập mã, tên SP, loại, hãng hoặc NCC...");
            this.ActiveControl = null;

        }

        private void btnThemAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Multiselect = true, Filter = "Image Files|*.jpg;*.png;*.jpeg" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                BatTatChucNang(true);
                foreach (string path in ofd.FileNames) //  đường dẫn ảnh
                {
                    if (!_listPaths.Contains(path))
                    {
                        _listPaths.Add(path);
                        PictureBox pic = new PictureBox { Size = new Size(65, 65), SizeMode = PictureBoxSizeMode.Zoom, Image = Image.FromFile(path), Cursor = Cursors.Hand };

                        // Khi click vào ảnh nhỏ mới thêm
                        pic.Click += (s, ev) =>
                        {
                            picHinhAnh.Image = pic.Image;
                            _pathHienTai = path;
                        };
                        flpHinhAnh.Controls.Add(pic);
                    }
                }
                // Sau khi chọn xong, tự động ghim tấm cuối cùng hoặc tấm đầu tiên vào màn hình to
                if (ofd.FileNames.Length > 0)
                {
                    _pathHienTai = ofd.FileNames[0];
                    picHinhAnh.Image = Image.FromFile(_pathHienTai);
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            _xuLyThem = true;
            BatTatChucNang(true);

            txtMaSP.Clear();
            txtTenSP.Clear();
            numSLTon.Value = numGiaNhap.Value = numGiaBan.Value = 0;
            flpHinhAnh.Controls.Clear();
            _listPaths.Clear();
            picHinhAnh.Image = null;
            cboLoaiSP.SelectedIndex = cboHangSX.SelectedIndex = cboTenNCC.SelectedIndex = -1; // THÊM cboTenNCC
            // Lấy danh sách tất cả các mã đang có trong Database
            var dsMaHienTai = _context.SanPham.Select(s => s.MaSP).ToList();
            txtMaSP.Text = SinhMaTuDong.TuSinhMa(dsMaHienTai, "MaSP", "SP", 3);

            txtTenSP.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_maSP))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!", "Thông báo");
                return;
            }
            _xuLyThem = false;
            BatTatChucNang(true);
            txtTenSP.Focus();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_maSP)) return;

            if (MessageBox.Show($"Bạn có chắc muốn xóa sản phẩm {_maSP}?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var sp = _context.SanPham.Find(_maSP);
                if (sp != null)
                {
                    string tenSPXoa = sp.TenSP;
                    _context.SanPham.Remove(sp);
                    _context.SaveChanges();
                    NhatKyHeThong.GhiLog(_maTKDangNhap, $"Xóa sản phẩm: {tenSPXoa} (Mã: {_maSP})");
                    TaiDuLieuLenBang();
                    _maSP = "";

                    // Clear form
                    txtMaSP.Clear();
                    txtTenSP.Clear();
                    numSLTon.Value = numGiaNhap.Value = numGiaBan.Value = 0;
                    flpHinhAnh.Controls.Clear();
                    _listPaths.Clear();
                    picHinhAnh.Image = null;

                    txtMaSP.Focus();
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cboLoaiSP.Text))
                MessageBox.Show("Vui lòng chọn loại sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (string.IsNullOrWhiteSpace(cboTenNCC.Text))
                MessageBox.Show("Vui lòng chọn nhà cung cấp.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (string.IsNullOrWhiteSpace(cboHangSX.Text))
                MessageBox.Show("Vui lòng chọn hãng sản xuất.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (string.IsNullOrWhiteSpace(txtTenSP.Text))
                MessageBox.Show("Vui lòng nhập tên sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (numSLTon.Value <= 0)
                MessageBox.Show("Số lượng phải lớn hơn 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (numGiaNhap.Value <= 0)
                MessageBox.Show("Đơn giá sản phẩm phải lớn hơn 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (numGiaBan.Value <= 0)
                MessageBox.Show("Đơn giá bán phải lớn hơn 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // --- RÀNG BUỘC DỮ LIỆU ---
            if (string.IsNullOrWhiteSpace(txtMaSP.Text))
            {
                MessageBox.Show("Mã sản phẩm không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTenSP.Text))
            {
                MessageBox.Show("Tên sản phẩm không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            try
            {
                string hanhDongLog = "";
                // --- ĐOẠN XỬ LÝ ẢNH TRƯỚC KHI LƯU ---
                List<string> listTenAnhNgan = new List<string>();
                foreach (var path in _listPaths)
                {
                    // Nếu path là đường dẫn dài (có dấu \), thì mình copy vào folder project và lấy tên ngắn
                    if (path.Contains("\\"))
                        listTenAnhNgan.Add(LuuAnhVaoThuMucLocal(path));
                    else
                        listTenAnhNgan.Add(path); // Nếu đã là tên ngắn rồi (do nhập Excel hoặc sửa) thì giữ nguyên
                }
                string chuoiAnhDeLuu = string.Join(",", listTenAnhNgan);
                if (_xuLyThem)
                {
                    if (_context.SanPham.Any(s => s.MaSP == txtMaSP.Text.Trim()))
                    {
                        MessageBox.Show("Mã sản phẩm đã tồn tại!", "Lỗi");
                        return;
                    }

                    SanPham sp = new SanPham
                    {
                        MaSP = txtMaSP.Text.Trim(),
                        TenSP = txtTenSP.Text.Trim(),
                        MaLoai = cboLoaiSP.SelectedValue.ToString(),
                        MaHSX = cboHangSX.SelectedValue.ToString(),
                        MaNCC = cboTenNCC.SelectedValue.ToString(),
                        SLTon = (int)numSLTon.Value,
                        GiaNhap = numGiaNhap.Value,
                        GiaBan = numGiaBan.Value,
                        TrangThai = ckbTrangThai.Checked,
                        HinhAnh = string.Join(",", _listPaths)
                    };
                    _context.SanPham.Add(sp);
                    hanhDongLog = $"Thêm mới sản phẩm: {sp.TenSP} (Mã: {sp.MaSP})";
                }
                else
                {
                    var sp = _context.SanPham.Find(_maSP);
                    if (sp != null)
                    {
                        sp.TenSP = txtTenSP.Text.Trim();
                        sp.MaLoai = cboLoaiSP.SelectedValue.ToString();
                        sp.MaHSX = cboHangSX.SelectedValue.ToString();
                        sp.SLTon = (int)numSLTon.Value;
                        sp.GiaNhap = numGiaNhap.Value;
                        sp.GiaBan = numGiaBan.Value;
                        sp.TrangThai = ckbTrangThai.Checked;
                        //sp.HinhAnh = string.Join(",", _listPaths);
                        sp.HinhAnh = chuoiAnhDeLuu; // Dùng chuỗi đã xử lý tên ngắn

                        hanhDongLog = $"Cập nhật thông tin sản phẩm: {sp.TenSP} (Mã: {_maSP})";
                    }
                }

                _context.SaveChanges();
                if (!string.IsNullOrEmpty(hanhDongLog))
                {
                    NhatKyHeThong.GhiLog(_maTKDangNhap, hanhDongLog);
                }
                MessageBox.Show("Lưu thành công!", "Thông báo");
                TaiDuLieuLenBang();
                BatTatChucNang(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            BatTatChucNang(false);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvDSSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            BatTatChucNang(false);

            try
            {
                var row = dgvDSSanPham.Rows[e.RowIndex];
                _maSP = row.Cells["MaSP"].Value?.ToString() ?? "";

                txtMaSP.Text = _maSP;
                txtTenSP.Text = row.Cells["TenSP"].Value?.ToString() ?? "";

                cboLoaiSP.Text = row.Cells["TenLoai"].Value?.ToString();
                cboHangSX.Text = row.Cells["TenHSX"].Value?.ToString();
                cboTenNCC.Text = row.Cells["TenNCC"].Value?.ToString();

                numSLTon.Value = Convert.ToDecimal(row.Cells["SLTon"].Value ?? 0);
                numGiaNhap.Value = Convert.ToDecimal(row.Cells["GiaNhap"].Value ?? 0);
                numGiaBan.Value = Convert.ToDecimal(row.Cells["GiaBan"].Value ?? 0);

                string trangThai = row.Cells["TrangThai"].Value?.ToString() ?? "";
                ckbTrangThai.Checked = (trangThai == "Đang kinh doanh");


                flpHinhAnh.Controls.Clear();
                picHinhAnh.Image = null;
                _listPaths.Clear();

                string hinhAnhHienTai = row.Cells["HinhAnh"].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(hinhAnhHienTai))
                {
                    // Tách chuỗi tên ảnh ngắn (ví dụ: "son.jpg,kem.jpg")
                    _listPaths = hinhAnhHienTai.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    foreach (var tenFile in _listPaths)
                    {
                        //  tìm ảnh trong thư mục bin/Debug/Images
                        string fullPath = Path.Combine(Application.StartupPath, "Images", tenFile);

                        if (File.Exists(fullPath))
                        {
                            // Đọc ảnh dùng MemoryStream để không bị khóa file
                            byte[] bytes = File.ReadAllBytes(fullPath);
                            MemoryStream ms = new MemoryStream(bytes);
                            Image img = Image.FromStream(ms);

                            // 1. Tạo ảnh nhỏ ở dưới
                            PictureBox picSmall = new PictureBox
                            {
                                Size = new Size(65, 65),
                                SizeMode = PictureBoxSizeMode.Zoom,
                                Image = img,
                                Cursor = Cursors.Hand
                            };

                            // Khi click vào ảnh nhỏ thì hiện lên ảnh to
                            picSmall.Click += (s, ev) =>
                            {
                                picHinhAnh.Image = picSmall.Image;
                                _pathHienTai = tenFile; // Lưu tên file hiện tại
                            };
                            flpHinhAnh.Controls.Add(picSmall);
                        }
                    }

                    // 2. Tự động hiện tấm ảnh đầu tiên lên PictureBox to
                    if (_listPaths.Count > 0)
                    {
                        string firstPath = Path.Combine(Application.StartupPath, "Images", _listPaths[0]);
                        if (File.Exists(firstPath))
                        {
                            byte[] bytes = File.ReadAllBytes(firstPath);
                            using (MemoryStream ms = new MemoryStream(bytes))
                            {
                                picHinhAnh.Image = Image.FromStream(ms);
                            }
                            _pathHienTai = _listPaths[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn sản phẩm: " + ex.Message);
            }
        }

        private void dgvDSSanPham_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            /*if (dgvDSSanPham.Columns[e.ColumnIndex].Name == "HinhAnh" && e.Value != null)
            {
                string pathStr = e.Value.ToString();
                // Lấy tấm ảnh đầu tiên trong chuỗi dấu phẩy
                string firstPath = pathStr.Split(',')[0];

                if (System.IO.File.Exists(firstPath))
                {
                    e.Value = Image.FromFile(firstPath); // Biến chuỗi thành Ảnh thật
                }
                else
                {
                    e.Value = null; // Hoặc một tấm ảnh mặc định "No Image"
                }
            }*/
            if (dgvDSSanPham.Columns[e.ColumnIndex].Name == "HinhAnh" && e.Value != null)
            {
                string pathStr = e.Value.ToString();
                string firstFileName = pathStr.Split(',')[0]; // Lấy tên file đầu tiên

                // Tìm trong bin/Debug/Images
                string fullPath = Path.Combine(Application.StartupPath, "Images", firstFileName);

                if (File.Exists(fullPath))
                {
                    // Dùng MemoryStream để tránh khóa file ảnh, giúp ní Xóa ảnh dễ hơn
                    byte[] bytes = File.ReadAllBytes(fullPath);
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        e.Value = Image.FromStream(ms);
                    }
                }
                else
                {
                    e.Value = null; // Hiện dấu X hoặc ảnh mặc định
                }
            }
        }

        private void btnXoaAnh_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có ảnh nào đang hiển thị để xóa không
            if (picHinhAnh.Image == null || _listPaths.Count == 0)
            {
                MessageBox.Show("Không có ảnh nào để xóa!", "Thông báo");
                return;
            }

            // Xác nhận xóa cho chắc ăn
            if (MessageBox.Show("Bạn có muốn xóa ảnh đang chọn này không?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Tìm đường dẫn của ảnh đang hiển thị trong PictureBox to
                // Vì mỗi khi click ảnh nhỏ, đã gán Image cho picHinhAnh
                // tìm cái Image đó tương ứng với Path nào trong _listPaths

                string pathCanXoa = "";

                foreach (var path in _listPaths)
                {
                    // so sánh file name hoặc dùng Tag
                    // logic: ảnh đang hiện ở picHinhAnh là tấm mình muốn bỏ
                    // Để chính xác nhất, lưu Path đang chọn vào 1 biến toàn cục
                    // xóa theo cơ chế "Xóa tấm đang xem":

                    // Nếu muốn xóa tấm hiện tại, cần biết index của nó.
                    // Tạm thời sẽ xóa tấm đầu tiên nếu không ghim biến, 

                }

                //  click vào ảnh nào đó,-> biến: private string _pathHienTai;
                if (!string.IsNullOrEmpty(_pathHienTai))
                {
                    // Xóa khỏi danh sách String
                    _listPaths.Remove(_pathHienTai);

                    // Xóa khỏi giao diện (FlowLayoutPanel)
                    flpHinhAnh.Controls.Clear();
                    foreach (string p in _listPaths)
                    {
                        if (File.Exists(p))
                        {
                            PictureBox pic = new PictureBox { Size = new Size(65, 65), SizeMode = PictureBoxSizeMode.Zoom, Image = Image.FromFile(p), Cursor = Cursors.Hand };
                            pic.Click += (s, ev) =>
                            {
                                picHinhAnh.Image = pic.Image;
                                _pathHienTai = p; // Ghim lại tấm vừa click
                            };
                            flpHinhAnh.Controls.Add(pic);
                        }
                    }

                    // Hiển thị lại tấm đầu tiên hoặc xóa trắng nếu hết ảnh
                    if (_listPaths.Count > 0)
                    {
                        _pathHienTai = _listPaths[0];
                        picHinhAnh.Image = Image.FromFile(_pathHienTai);
                    }
                    else
                    {
                        _pathHienTai = "";
                        picHinhAnh.Image = null;
                    }
                }

                else
                {
                    _pathHienTai = "";
                    picHinhAnh.Image = null;
                }

                if (!_xuLyThem && !string.IsNullOrEmpty(_maSP))
                {
                    var sp = _context.SanPham.Find(_maSP);
                    if (sp != null)
                    {
                        // Cập nhật lại chuỗi ảnh mới (đã mất tấm vừa xóa)
                        sp.HinhAnh = string.Join(",", _listPaths);
                        _context.SaveChanges();

                        //  Tải lại bảng để cột "Hình ảnh" cập nhật tấm ảnh đại diện mới
                        TaiDuLieuLenBang();
                        MessageBox.Show("Đã cập nhật danh sách ảnh sản phẩm!", "Thông báo");
                    }
                }
                else
                {
                    // Nếu đang thêm mới thì chỉ cần báo đã xóa khỏi danh sách tạm
                    MessageBox.Show("Đã xóa ảnh khỏi danh sách chờ lưu!", "Thông báo");
                }
            }
        }

        private void btnNhap_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Tập tin Excel|*.xlsx;*.xls" };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                int thanhCong = 0;
                int dongLoi = 0;
                StringBuilder chiTietLoi = new StringBuilder();

                try
                {
                    using (XLWorkbook workbook = new XLWorkbook(openFileDialog.FileName))
                    {
                        var worksheet = workbook.Worksheet(1);
                        var rows = worksheet.RowsUsed().Skip(1); // Bỏ qua tiêu đề

                        foreach (var row in rows)
                        {
                            // 1. LÀM MỚI CONTEXT mỗi dòng để tránh lỗi Tracked và cập nhật dữ liệu mới nhất để check trùng
                            _context = new QLCHMPDbContext();

                            try
                            {
                                // Đọc dữ liệu từ các cột (B=2, C=3, D=4, E=5, F=6, G=7, H=8, I=9)
                                string tenSP = row.Cell(2).GetFormattedString().Trim();
                                string tenLoai = row.Cell(3).GetFormattedString().Trim();
                                string tenHang = row.Cell(4).GetFormattedString().Trim();
                                string tenNCC = row.Cell(5).GetFormattedString().Trim();
                                decimal giaNhap = (decimal)(row.Cell(6).IsEmpty() ? 0 : row.Cell(6).GetDouble());
                                decimal giaBan = (decimal)(row.Cell(7).IsEmpty() ? 0 : row.Cell(7).GetDouble());
                                int slTon = (int)(row.Cell(8).IsEmpty() ? 0 : row.Cell(8).GetDouble());
                                string txtTrangThai = row.Cell(9).GetFormattedString().Trim();
                                string hinhAnh = row.Cell(10).GetFormattedString().Trim();

                                // 2. KIỂM TRA RÀNG BUỘC & CHẶN TRÙNG TÊN
                                if (string.IsNullOrEmpty(tenSP))
                                    throw new Exception("Tên sản phẩm không được để trống.");

                                // Chặn trùng tên (không phân biệt hoa thường)
                                bool daTonTai = _context.SanPham.Any(s => s.TenSP.ToLower() == tenSP.ToLower());
                                if (daTonTai)
                                {
                                    throw new Exception($"Sản phẩm '{tenSP}' đã tồn tại trong hệ thống.");
                                }

                                // 3. Tìm ID Loại và Hãng từ Tên
                                var loai = _context.LoaiSanPham.FirstOrDefault(l => l.TenLoai == tenLoai);
                                var hang = _context.HangSanXuat.FirstOrDefault(h => h.TenHSX == tenHang);
                                var ncc = _context.NhaCungCap.FirstOrDefault(n => n.TenNCC == tenNCC);

                                if (loai == null) throw new Exception($"Loại '{tenLoai}' không tồn tại.");
                                if (hang == null) throw new Exception($"Hãng '{tenHang}' không tồn tại.");
                                if (ncc == null) throw new Exception($"Nhà cung cấp '{tenNCC}' không tồn tại.");

                                // 4. Sinh mã SP mới tự động (Lấy danh sách mã mới nhất trong vòng lặp)
                                var dsMa = _context.SanPham.Select(s => s.MaSP).ToList();
                                string maMoi = SinhMaTuDong.TuSinhMa(dsMa, "MaSP", "SP", 3);

                                // 5. Tạo đối tượng Sản phẩm và lưu
                                SanPham sp = new SanPham
                                {
                                    MaSP = maMoi,
                                    TenSP = tenSP,
                                    MaLoai = loai.MaLoai,
                                    MaHSX = hang.MaHSX,
                                    MaNCC = ncc.MaNCC,
                                    GiaNhap = giaNhap,
                                    GiaBan = giaBan,
                                    SLTon = slTon,
                                    HinhAnh = hinhAnh,
                                    TrangThai = (txtTrangThai == "Đang kinh doanh")
                                };

                                _context.SanPham.Add(sp);
                                _context.SaveChanges();
                                thanhCong++;
                            }
                            catch (Exception ex)
                            {
                                dongLoi++;
                                // Lấy lỗi chi tiết từ InnerException nếu có
                                string msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                                chiTietLoi.AppendLine($"- Dòng {row.RowNumber()}: {msg}");
                            }
                        }
                    }

                    // 6. Thông báo tổng kết
                    string thongBao = $"Nhập hoàn tất!\n- Thành công: {thanhCong}\n- Thất bại: {dongLoi}";
                    if (dongLoi > 0) thongBao += "\n\nChi tiết lỗi:\n" + chiTietLoi.ToString();
                    if (thanhCong > 0)
                    {
                        NhatKyHeThong.GhiLog(_maTKDangNhap, $"Nhập dữ liệu Sản phẩm từ Excel (Thành công: {thanhCong} dòng)");
                    }
                    MessageBox.Show(thongBao, "Kết quả Import", MessageBoxButtons.OK,
                                    dongLoi > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                    TaiDuLieuLenBang(); // Load lại lưới
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    string.Join(",", _listPaths);
                }
            }
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Xuất danh sách Sản phẩm ra Excel";
            saveFileDialog.Filter = "Tập tin Excel|*.xlsx";
            saveFileDialog.FileName = "SanPham_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("Mã SP", typeof(string));
                    table.Columns.Add("Tên sản phẩm", typeof(string));
                    table.Columns.Add("Phân loại", typeof(string));
                    table.Columns.Add("Hãng sản xuất", typeof(string));
                    table.Columns.Add("Nhà cung cấp", typeof(string));
                    table.Columns.Add("Giá nhập", typeof(decimal));
                    table.Columns.Add("Giá bán", typeof(decimal));
                    table.Columns.Add("Số lượng tồn", typeof(int));
                    table.Columns.Add("Trạng thái", typeof(string));
                    table.Columns.Add("Hình ảnh", typeof(string));

                    // Lấy dữ liệu theo logic Class DanhSachSanPham
                    var ds = _context.SanPham.Select(p => new
                    {
                        p.MaSP,
                        p.TenSP,
                        TenLoai = p.LoaiSanPham.TenLoai,
                        TenHSX = p.HangSanXuat.TenHSX,
                        TenNCC = p.NhaCungCap.TenNCC,
                        p.GiaNhap,
                        p.GiaBan,
                        p.SLTon,
                        TrangThai = p.TrangThai ? "Đang kinh doanh" : "Ngừng kinh doanh",
                        p.HinhAnh
                    }).ToList();

                    foreach (var p in ds)
                    {
                        table.Rows.Add(p.MaSP, p.TenSP, p.TenLoai, p.TenHSX, p.TenNCC, p.GiaNhap, p.GiaBan, p.SLTon, p.TrangThai, p.HinhAnh);
                    }

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var sheet = wb.Worksheets.Add(table, "SanPham");
                        sheet.Columns().AdjustToContents();

                        // Định dạng tiêu đề cho đẹp
                        var header = sheet.Row(1);
                        header.Style.Font.Bold = true;
                        header.Style.Fill.BackgroundColor = XLColor.FromHtml("#27ae60");
                        header.Style.Font.FontColor = XLColor.White;

                        wb.SaveAs(saveFileDialog.FileName);
                    }
                    NhatKyHeThong.GhiLog(_maTKDangNhap, "Xuất danh sách Sản phẩm ra tập tin Excel");
                    MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất file: " + ex.Message);
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDuLieuLenBang();
                return;
            }

            try
            {
                _context = new QLCHMPDbContext();

                //  Mã SP, Tên SP, Loại, Hãng, NCC
                var dsTimKiem = _context.SanPham
                    .Where(p => p.MaSP.ToLower().Contains(tuKhoa) ||
                                p.TenSP.ToLower().Contains(tuKhoa) ||
                                (p.LoaiSanPham != null && p.LoaiSanPham.TenLoai.ToLower().Contains(tuKhoa)) ||
                                (p.HangSanXuat != null && p.HangSanXuat.TenHSX.ToLower().Contains(tuKhoa)) ||
                                (p.NhaCungCap != null && p.NhaCungCap.TenNCC.ToLower().Contains(tuKhoa)))
                    .Select(p => new DanhSachSanPham // Đổ dữ liệu vào Class hiển thị như lúc Load
                    {
                        MaSP = p.MaSP,
                        TenSP = p.TenSP,
                        TenLoai = p.LoaiSanPham != null ? p.LoaiSanPham.TenLoai : "",
                        TenHSX = p.HangSanXuat != null ? p.HangSanXuat.TenHSX : "",
                        TenNCC = p.NhaCungCap != null ? p.NhaCungCap.TenNCC : "",
                        GiaNhap = p.GiaNhap,
                        GiaBan = p.GiaBan,
                        SLTon = p.SLTon,
                        HinhAnh = p.HinhAnh,
                        TrangThai = p.TrangThai ? "Đang kinh doanh" : "Ngừng kinh doanh"
                    }).ToList();

                if (dsTimKiem.Count == 0)
                {
                    MessageBox.Show($"Không tìm thấy sản phẩm nào khớp với từ khóa '{txtTimKiem.Text.Trim()}'!", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDuLieuLenBang();
                    txtTimKiem.Clear();
                    txtTimKiem.Focus();
                    txtTimKiem.SelectAll();
                }
                else
                {
                    // Nếu tìm thấy thì đổ thẳng danh sách lên lưới
                    dgvDSSanPham.DataSource = dsTimKiem;

                    // Định dạng lại tiền tệ
                    if (dgvDSSanPham.Columns["GiaBan"] != null)
                        dgvDSSanPham.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
                    if (dgvDSSanPham.Columns["GiaNhap"] != null)
                        dgvDSSanPham.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();

            TaiDuLieuLenBang();

            this.ActiveControl = null;
        }
    }

}
