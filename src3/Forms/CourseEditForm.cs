using System;
using System.Drawing;
using System.Windows.Forms;
using StudentInfoApp3.Models;
using StudentInfoApp3.Data;

namespace StudentInfoApp3.Forms
{
    public class CourseEditForm : Form
    {
        TextBox txtCode;
        TextBox txtTitle;
        NumericUpDown numCredits;
        Button btnOk;
        Button btnCancel;
        Course editing;

        public CourseEditForm(Course? c = null)
        {
            Text = c == null ? "Add Course" : "Edit Course";
            Width = 550;
            Height = 450;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(240, 242, 245);
            Font = new Font("Segoe UI", 10);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            // Main panel
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(40)
            };

            // Title
            Label lblTitle = new Label
            {
                Text = c == null ? "Add New Course" : "Edit Course",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(63, 81, 181),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 30)
            };

            // Code
            Label lblCode = new Label
            {
                Text = "Course Code",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                AutoSize = true,
                Margin = new Padding(0, 15, 0, 5)
            };

            txtCode = new TextBox
            {
                Width = 400,
                Height = 40,
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                Margin = new Padding(0, 0, 0, 15)
            };

            // Title
            Label lblCourseTitle = new Label
            {
                Text = "Course Title",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                AutoSize = true,
                Margin = new Padding(0, 15, 0, 5)
            };

            txtTitle = new TextBox
            {
                Width = 400,
                Height = 40,
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                Margin = new Padding(0, 0, 0, 15)
            };

            // Credits
            Label lblCredits = new Label
            {
                Text = "Credit Hours",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                AutoSize = true,
                Margin = new Padding(0, 15, 0, 5)
            };

            numCredits = new NumericUpDown
            {
                Width = 400,
                Height = 40,
                Font = new Font("Segoe UI", 11),
                Minimum = 0,
                Maximum = 10,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 30)
            };

            // Buttons
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Width = 400,
                Height = 50,
                AutoSize = false,
                FlowDirection = FlowDirection.RightToLeft,
                Margin = new Padding(0, 20, 0, 0)
            };

            btnCancel = new Button
            {
                Text = "Cancel",
                Width = 120,
                Height = 45,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(200, 200, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(10, 0, 0, 0),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            btnOk = new Button
            {
                Text = "Save",
                Width = 120,
                Height = 45,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(63, 81, 181),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnOk.FlatAppearance.BorderSize = 0;

            buttonPanel.Controls.AddRange(new Control[] { btnCancel, btnOk });

            // Add controls to main panel
            mainPanel.Controls.Add(buttonPanel);
            mainPanel.Controls.Add(numCredits);
            mainPanel.Controls.Add(lblCredits);
            mainPanel.Controls.Add(txtTitle);
            mainPanel.Controls.Add(lblCourseTitle);
            mainPanel.Controls.Add(txtCode);
            mainPanel.Controls.Add(lblCode);
            mainPanel.Controls.Add(lblTitle);

            Controls.Add(mainPanel);

            btnOk.Click += BtnOk_Click;
            btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            editing = c ?? new Course();
            if (c != null)
            {
                txtCode.Text = c.Code;
                txtTitle.Text = c.Title;
                numCredits.Value = c.Credits;
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            editing.Code = txtCode.Text.Trim();
            editing.Title = txtTitle.Text.Trim();
            editing.Credits = (int)numCredits.Value;
            if (string.IsNullOrEmpty(editing.Code) || string.IsNullOrEmpty(editing.Title))
            {
                MessageBox.Show("Code and Title are required", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            bool ok = editing.CourseId == 0
                ? DataAccess.AddCourse(editing)
                : DataAccess.UpdateCourse(editing);
            if (ok)
                DialogResult = DialogResult.OK;
            else
                MessageBox.Show("Save failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
