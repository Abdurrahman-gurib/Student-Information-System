using System;
using System.Drawing;
using System.Windows.Forms;
using StudentInfoApp3.Models;
using StudentInfoApp3.Data;

namespace StudentInfoApp3.Forms
{
    public class CourseForm : Form
    {
        DataGridView dgv;

        public CourseForm()
        {
            Text = "Manage Courses";
            Width = 900;
            Height = 550;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(240, 242, 245);
            Font = new Font("Segoe UI", 10);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            // Top panel with title
            Panel topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(63, 81, 181),
                Padding = new Padding(20)
            };

            Label lblTitle = new Label
            {
                Text = "📖 Course Management",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 15)
            };
            topPanel.Controls.Add(lblTitle);

            // Modern toolbar
            ToolStrip tool = new ToolStrip
            {
                BackColor = Color.White,
                Height = 60,
                Dock = DockStyle.Top,
                ImageScalingSize = new Size(32, 32),
                RenderMode = ToolStripRenderMode.System
            };

            tool.Items.Add(CreateToolButton("➕ Add Course", (s, e) =>
            {
                var f = new CourseEditForm();
                if (f.ShowDialog(this) == DialogResult.OK) RefreshGrid();
            }));
            tool.Items.Add(new ToolStripSeparator());
            tool.Items.Add(CreateToolButton("✏️ Edit Course", (s, e) =>
            {
                if (dgv.CurrentRow == null)
                {
                    MessageBox.Show("Select a course first");
                    return;
                }
                var c = dgv.CurrentRow.DataBoundItem as Course;
                var f = new CourseEditForm(c);
                if (f.ShowDialog(this) == DialogResult.OK) RefreshGrid();
            }));
            tool.Items.Add(new ToolStripSeparator());
            tool.Items.Add(CreateToolButton("🗑️ Delete Course", (s, e) =>
            {
                if (dgv.CurrentRow == null)
                {
                    MessageBox.Show("Select a course first");
                    return;
                }
                var c = dgv.CurrentRow.DataBoundItem as Course;
                if (MessageBox.Show($"Delete {c.Code} - {c.Title}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataAccess.DeleteCourse(c.CourseId);
                    RefreshGrid();
                }
            }));
            tool.Items.Add(new ToolStripSeparator());
            tool.Items.Add(CreateToolButton("🔄 Refresh", (s, e) => RefreshGrid()));
            tool.Items.Add(new ToolStripSeparator());
            tool.Items.Add(CreateToolButton("❌ Close", (s, e) => Close()));

            // DataGridView with modern styling
            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(249, 249, 249) },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(63, 81, 181),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Padding = new Padding(5)
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 10),
                    Padding = new Padding(5)
                }
            };

            // Configure columns
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                DataPropertyName = "CourseId",
                Width = 50,
                ReadOnly = true
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Course Code",
                DataPropertyName = "Code",
                Width = 120,
                ReadOnly = true
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Title",
                DataPropertyName = "Title",
                Width = 350,
                ReadOnly = true
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Credits",
                DataPropertyName = "Credits",
                Width = 100,
                ReadOnly = true
            });

            Controls.Add(dgv);
            Controls.Add(tool);
            Controls.Add(topPanel);

            Load += (s, e) => RefreshGrid();
        }

        private ToolStripButton CreateToolButton(string text, EventHandler click)
        {
            var btn = new ToolStripButton(text)
            {
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = false,
                Width = 130,
                Height = 45
            };
            btn.Click += click;
            return btn;
        }

        void RefreshGrid()
        {
            dgv.DataSource = null;
            dgv.DataSource = DataAccess.GetCourses();
        }
    }
}
