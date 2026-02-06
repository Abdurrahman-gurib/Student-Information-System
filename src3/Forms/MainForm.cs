using System;
using System.Drawing;
using System.Windows.Forms;
using StudentInfoApp3.Data;
using StudentInfoApp3.Models;

namespace StudentInfoApp3.Forms
{
    public class MainForm : Form
    {
        DataGridView dgv;
        ToolStrip tool;
        Label lblTitle;

        public MainForm()
        {
            // Form styling
            Text = "Student Information System";
            Width = 1200;
            Height = 700;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(240, 242, 245);
            Font = new Font("Segoe UI", 10);

            // Top panel with title
            Panel topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(33, 150, 243),
                Padding = new Padding(20)
            };

            lblTitle = new Label
            {
                Text = "📚 Student Management",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 15)
            };
            topPanel.Controls.Add(lblTitle);

            // Modern toolbar
            tool = new ToolStrip
            {
                BackColor = Color.White,
                Height = 60,
                Dock = DockStyle.Top,
                ImageScalingSize = new Size(32, 32),
                RenderMode = ToolStripRenderMode.System
            };
            tool.Items.Add(CreateToolButton("➕ Add Student", (s, e) =>
            {
                var f = new StudentForm();
                if (f.ShowDialog() == DialogResult.OK) RefreshGrid();
            }));
            tool.Items.Add(new ToolStripSeparator());
            tool.Items.Add(CreateToolButton("✏️ Edit Student", (s, e) =>
            {
                if (dgv.CurrentRow == null)
                {
                    MessageBox.Show("Select a student first");
                    return;
                }
                var st = dgv.CurrentRow.DataBoundItem as Student;
                var f = new StudentForm(st);
                if (f.ShowDialog() == DialogResult.OK) RefreshGrid();
            }));
            tool.Items.Add(new ToolStripSeparator());
            tool.Items.Add(CreateToolButton("🗑️ Delete Student", (s, e) =>
            {
                if (dgv.CurrentRow == null)
                {
                    MessageBox.Show("Select a student first");
                    return;
                }
                var st = dgv.CurrentRow.DataBoundItem as Student;
                if (MessageBox.Show($"Delete {st.FirstName} {st.LastName}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataAccess.DeleteStudent(st.StudentId);
                    RefreshGrid();
                }
            }));
            tool.Items.Add(new ToolStripSeparator());
            tool.Items.Add(CreateToolButton("🔄 Refresh", (s, e) => RefreshGrid()));
            tool.Items.Add(new ToolStripSeparator());
            tool.Items.Add(CreateToolButton("📖 Manage Courses", (s, e) => { new CourseForm().ShowDialog(); }));

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
                    BackColor = Color.FromArgb(33, 150, 243),
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

            // Set column widths
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                DataPropertyName = "StudentId",
                Width = 50,
                ReadOnly = true
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "First Name",
                DataPropertyName = "FirstName",
                Width = 150,
                ReadOnly = true
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Last Name",
                DataPropertyName = "LastName",
                Width = 150,
                ReadOnly = true
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Email",
                DataPropertyName = "Email",
                Width = 250,
                ReadOnly = true
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Date of Birth",
                DataPropertyName = "DateOfBirth",
                Width = 150,
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
                Width = 140,
                Height = 45
            };
            btn.Click += click;
            return btn;
        }

        void RefreshGrid()
        {
            dgv.DataSource = null;
            dgv.DataSource = DataAccess.GetStudents();
        }
    }
}
