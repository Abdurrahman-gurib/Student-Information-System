using System;
using System.Drawing;
using System.Windows.Forms;
using StudentInfoApp3.Models;
using StudentInfoApp3.Data;

namespace StudentInfoApp3.Forms
{
    public class StudentForm : Form
    {
        TextBox txtFirst;
        TextBox txtLast;
        TextBox txtEmail;
        DateTimePicker dtDob;
        Button btnOk;
        Button btnCancel;
        Student editing;

        public StudentForm(Student? s = null)
        {
            Text = s == null ? "Add Student" : "Edit Student";
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
                Text = s == null ? "Add New Student" : "Edit Student",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 150, 243),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 30)
            };

            // First Name
            Label lblFirst = new Label
            {
                Text = "First Name",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                AutoSize = true,
                Margin = new Padding(0, 15, 0, 5)
            };

            txtFirst = new TextBox
            {
                Width = 400,
                Height = 40,
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                Margin = new Padding(0, 0, 0, 15)
            };

            // Last Name
            Label lblLast = new Label
            {
                Text = "Last Name",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                AutoSize = true,
                Margin = new Padding(0, 15, 0, 5)
            };

            txtLast = new TextBox
            {
                Width = 400,
                Height = 40,
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                Margin = new Padding(0, 0, 0, 15)
            };

            // Email
            Label lblEmail = new Label
            {
                Text = "Email Address",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                AutoSize = true,
                Margin = new Padding(0, 15, 0, 5)
            };

            txtEmail = new TextBox
            {
                Width = 400,
                Height = 40,
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                Margin = new Padding(0, 0, 0, 15)
            };

            // Date of Birth
            Label lblDob = new Label
            {
                Text = "Date of Birth",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                AutoSize = true,
                Margin = new Padding(0, 15, 0, 5)
            };

            dtDob = new DateTimePicker
            {
                Width = 400,
                Height = 40,
                Font = new Font("Segoe UI", 11),
                Format = DateTimePickerFormat.Short,
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
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnOk.FlatAppearance.BorderSize = 0;

            buttonPanel.Controls.AddRange(new Control[] { btnCancel, btnOk });

            // Add controls to main panel
            mainPanel.Controls.Add(buttonPanel);
            mainPanel.Controls.Add(dtDob);
            mainPanel.Controls.Add(lblDob);
            mainPanel.Controls.Add(txtEmail);
            mainPanel.Controls.Add(lblEmail);
            mainPanel.Controls.Add(txtLast);
            mainPanel.Controls.Add(lblLast);
            mainPanel.Controls.Add(txtFirst);
            mainPanel.Controls.Add(lblFirst);
            mainPanel.Controls.Add(lblTitle);

            Controls.Add(mainPanel);

            btnOk.Click += BtnOk_Click;
            btnCancel.Click += (s2, e) => DialogResult = DialogResult.Cancel;

            editing = s ?? new Student();
            if (s != null)
            {
                txtFirst.Text = s.FirstName;
                txtLast.Text = s.LastName;
                txtEmail.Text = s.Email;
                if (s.DateOfBirth.HasValue) dtDob.Value = s.DateOfBirth.Value;
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            editing.FirstName = txtFirst.Text.Trim();
            editing.LastName = txtLast.Text.Trim();
            editing.Email = txtEmail.Text.Trim();
            editing.DateOfBirth = dtDob.Value.Date;
            if (string.IsNullOrEmpty(editing.FirstName) || string.IsNullOrEmpty(editing.LastName))
            {
                MessageBox.Show("First and Last names are required", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            bool ok = editing.StudentId == 0
                ? DataAccess.AddStudent(editing)
                : DataAccess.UpdateStudent(editing);
            if (ok)
                DialogResult = DialogResult.OK;
            else
                MessageBox.Show("Save failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
