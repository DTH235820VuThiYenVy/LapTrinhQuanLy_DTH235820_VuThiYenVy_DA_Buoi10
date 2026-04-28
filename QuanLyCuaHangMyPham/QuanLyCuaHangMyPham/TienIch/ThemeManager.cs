using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangMyPham.TienIch
{
    public static class ThemeManager
    {
        //Theme Tối (Dark Mode)
        public static void ToDarkMode(Form f)
        {
            Color back = Color.FromArgb(30, 30, 30);
            Color btn = Color.FromArgb(60, 60, 60);
            Color text = Color.White;

            ApplyTheme(f, back, btn, text);
        }

        //Theme Mùa Xuân (Hồng - Spring Mode)
        public static void ToSpringMode(Form f)
        {
            Color back = Color.MistyRose;      // Màu nền hồng nhạt
            Color btn = Color.LightPink;      // Màu nút hồng đậm hơn chút
            Color text = Color.DeepPink;      // Màu chữ hồng đậm

            ApplyTheme(f, back, btn, text);
        }

        public static void ToCustomMode(Form f, Color backColor, Color textColor)
        {
            f.BackColor = backColor;
            ApplyCustomToControls(f.Controls, backColor, textColor);
        }

        public static void ToLightMode(Form f)
        {
            f.BackColor = SystemColors.Control; // Màu xám tiêu chuẩn của Windows
            ResetToLight(f.Controls);
        }
        public static void ToDefaultMode(Form f)
        {
            Color back = Color.FromArgb(235, 245, 255); // Màu nền form hơi xanh nhạt/sáng
            Color btn = Color.Navy; // Nút bấm màu Navy
            Color text = Color.Black; // Chữ hiển thị màu đen

            f.BackColor = back;
            ApplyNavyTheme(f.Controls, back, btn, text);
        }
        private static void ApplyNavyTheme(Control.ControlCollection controls, Color back, Color btn, Color text)
        {
            foreach (Control c in controls)
            {
                if (c is Label || c is CheckBox || c is GroupBox) c.ForeColor = text;

                if (c is Button b)
                {
                    b.BackColor = btn;
                    b.ForeColor = Color.White; // Nền Navy thì chữ phải Trắng hoặc Vàng
                    b.FlatStyle = FlatStyle.Flat;
                }

                if (c is DataGridView dgv)
                {
                    dgv.BackgroundColor = Color.White;
                    dgv.EnableHeadersVisualStyles = false;

                    // 1. Xử lý màu Tiêu đề cột
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = btn;
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                    // 2. Ép màu ô bình thường (Chữ phải Đen)
                    dgv.DefaultCellStyle.BackColor = Color.White;
                    dgv.DefaultCellStyle.ForeColor = Color.Black;
                    dgv.RowsDefaultCellStyle.BackColor = Color.White;
                    dgv.RowsDefaultCellStyle.ForeColor = Color.Black;

                    // 3. Ép màu ô khi được Click (Nền xanh, Chữ trắng)
                    dgv.DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue;
                    dgv.DefaultCellStyle.SelectionForeColor = Color.White;
                    dgv.RowsDefaultCellStyle.SelectionBackColor = Color.CornflowerBlue;
                    dgv.RowsDefaultCellStyle.SelectionForeColor = Color.White;

                    // 4. Lệnh QUAN TRỌNG NHẤT: Bỏ qua bước đệ quy, không chui vào các control ẩn của bảng
                    continue;
                }

                if (c is MenuStrip ms)
                {
                    ms.BackColor = back;
                    ms.ForeColor = text;
                }

                if (c.HasChildren) ApplyNavyTheme(c.Controls, back, btn, text);
            }
        }
        private static void ApplyCustomToControls(Control.ControlCollection controls, Color back, Color text)
        {
            foreach (Control c in controls)
            {
                // 1. Nhuộm Label, CheckBox, GroupBox
                if (c is Label || c is CheckBox || c is GroupBox)
                {
                    c.ForeColor = text;
                }

                // 2. Nhuộm Button (Fix lỗi mất chữ)
                if (c is Button btn)
                {
                    btn.BackColor = Color.FromArgb(240, 240, 240); // Để nền nút xám nhạt cho dễ nhìn chữ
                    btn.ForeColor = Color.Black; // Chữ nút luôn để đen cho chắc chắn hiện hình
                    btn.FlatStyle = FlatStyle.Standard;
                }

                // 3. Nhuộm DataGridView (Đặc trị lỗi mất chữ trong bảng)
                if (c is DataGridView dgv)
                {
                    dgv.BackgroundColor = Color.White;
                    dgv.DefaultCellStyle.ForeColor = Color.Black; // Chữ trong ô luôn đen
                    dgv.DefaultCellStyle.BackColor = Color.White;

                    // Màu khi chọn dòng
                    dgv.DefaultCellStyle.SelectionBackColor = Color.SkyBlue;
                    dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

                    // Fix màu tiêu đề cột
                    dgv.EnableHeadersVisualStyles = false;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                }

                // 4. Nếu có con (Panel, GroupBox...) thì chui vào tiếp (Đệ quy)
                if (c.HasChildren)
                {
                    ApplyCustomToControls(c.Controls, back, text);
                }
            }
        }

        // 3. Hàm dùng chung để áp dụng màu (Xử lý đệ quy cho mọi control)
        private static void ApplyTheme(Form f, Color backColor, Color btnColor, Color textColor)
        {
            f.BackColor = backColor;
            ChangeControlsColor(f.Controls, backColor, btnColor, textColor);
        }

        

        private static void ResetToLight(Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                if (c is Label || c is CheckBox || c is GroupBox) c.ForeColor = Color.Black;

                if (c is Button b)
                {
                    b.BackColor = SystemColors.Control;
                    b.ForeColor = Color.Black;
                    b.UseVisualStyleBackColor = true;
                    b.FlatStyle = FlatStyle.Standard;
                }

                if (c is DataGridView dgv)
                {
                    dgv.BackgroundColor = SystemColors.AppWorkspace;
                    dgv.EnableHeadersVisualStyles = true;
                    dgv.DefaultCellStyle.BackColor = Color.White;
                    dgv.DefaultCellStyle.ForeColor = Color.Black;
                    dgv.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                    dgv.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
                }

                if (c.HasChildren) ResetToLight(c.Controls);
            }
        }
        private static void NhuomMenuCon(ToolStripMenuItem item, Color back, Color text)
        {
            item.BackColor = back;
            item.ForeColor = text;
            foreach (ToolStripItem sub in item.DropDownItems)
            {
                if (sub is ToolStripMenuItem subMenu)
                {
                    NhuomMenuCon(subMenu, back, text);
                }
            }
        }
        private static void ChangeControlsColor(Control.ControlCollection controls, Color back, Color btn, Color text)
        {
            foreach (Control c in controls)
            {
                // 1. Label, CheckBox, GroupBox
                if (c is Label || c is CheckBox || c is GroupBox) c.ForeColor = text;

                // 2. Button
                if (c is Button b)
                {
                    b.BackColor = btn;
                    b.ForeColor = (back.R < 100) ? Color.White : text;
                    b.FlatStyle = FlatStyle.Flat;
                    b.FlatAppearance.BorderColor = text;
                }

                // 3. ĐẶC TRỊ DATAGRIDVIEW (Nhuộm màu bảng dữ liệu)
                if (c is DataGridView dgv)
                {
                    dgv.BackgroundColor = (back.R < 100) ? Color.FromArgb(45, 45, 48) : Color.White;
                    dgv.EnableHeadersVisualStyles = false; // Phải tắt cái này mới đổi màu tiêu đề được

                    dgv.ColumnHeadersDefaultCellStyle.BackColor = btn;
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = text;

                    dgv.DefaultCellStyle.BackColor = (back.R < 100) ? Color.FromArgb(30, 30, 30) : Color.White;
                    dgv.DefaultCellStyle.ForeColor = (back.R < 100) ? Color.White : Color.Black;

                    dgv.DefaultCellStyle.SelectionBackColor = text; // Màu khi chọn dòng
                    dgv.DefaultCellStyle.SelectionForeColor = back;
                }

                // 4. MenuStrip (Xử lý các mục menu con)
                if (c is MenuStrip ms)
                {
                    ms.BackColor = back;
                    ms.ForeColor = text;
                    foreach (ToolStripMenuItem item in ms.Items)
                    {
                        NhuomMenuCon(item, back, text);
                    }
                }

                // 5. Đệ quy
                if (c.HasChildren) ChangeControlsColor(c.Controls, back, btn, text);
            }
        }
    }
}