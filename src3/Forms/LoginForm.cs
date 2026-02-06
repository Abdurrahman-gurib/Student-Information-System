using System;
using System.Drawing;
using System.Windows.Forms;
using StudentInfoApp3.Data;

namespace StudentInfoApp3.Forms
{
    public class LoginForm : Form
    {
        TextBox txtUser;
        TextBox txtPass;
        Button btnLogin;
        Button btnRegister;

        public LoginForm()
        {
            // Form styling
            Text = "Student Information System - Login";
            Width = 500;
            Height = 600;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(33, 150, 243);
            Font = new Font("Segoe UI", 10);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            // Main white panel
            Panel mainPanel = new Panel
            {
                BackColor = Color.White,
                Location = new Point(30, 30),
                Width = 440,
                Height = 540
            };

            // Title
            Label lblTitle = new Label
            {
                Text = "Welcome Back",
                Font = new Font("Segoe UI", 32, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 150, 243),
                Location = new Point(30, 30),
                AutoSize = true
            };
            mainPanel.Controls.Add(lblTitle);

            // Subtitle
            Label lblSubtitle = new Label
            {
                Text = "Sign in to your account",
                Font = new Font("Segoe UI", 13),
                ForeColor = Color.FromArgb(120, 120, 120),
                Location = new Point(30, 75),
                AutoSize = true
            };
            mainPanel.Controls.Add(lblSubtitle);

            // Username label
            Label lblUser = new Label
            {
                Text = "Username",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                Location = new Point(30, 130),
                AutoSize = true
            };
            mainPanel.Controls.Add(lblUser);

            // Username textbox
            txtUser = new TextBox
            {
                Location = new Point(30, 160),
                Width = 380,
                Height = 40,
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                Text = ""
            };
            mainPanel.Controls.Add(txtUser);

            // Password label
            Label lblPass = new Label
            {
                Text = "Password",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                Location = new Point(30, 215),
                AutoSize = true
            };
            mainPanel.Controls.Add(lblPass);

            // Password textbox
            txtPass = new TextBox
            {
                Location = new Point(30, 245),
                Width = 380,
                Height = 40,
                Font = new Font("Segoe UI", 11),
                UseSystemPasswordChar = true,
                BorderStyle = BorderStyle.FixedSingle,
                Text = ""
            };
            mainPanel.Controls.Add(txtPass);

            // Sign In button
            btnLogin = new Button
            {
                Text = "Sign In",
                Location = new Point(30, 320),
                Width = 180,
                Height = 50,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            mainPanel.Controls.Add(btnLogin);

            // Create Account button
            btnRegister = new Button
            {
                Text = "Create Account",
                Location = new Point(230, 320),
                Width = 180,
                Height = 50,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            mainPanel.Controls.Add(btnRegister);

            // Add main panel to form
            Controls.Add(mainPanel);

            btnLogin.Click += BtnLogin_Click;
            btnRegister.Click += BtnRegister_Click;
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            var u = txtUser.Text.Trim();
            var p = txtPass.Text;
            if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p))
            {
                MessageBox.Show("Enter username and password");
                return;
            }
            if (DataAccess.CreateUser(u, p))
                MessageBox.Show("User created. You can login now.");
            else
                MessageBox.Show("Create failed (maybe user exists)");
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            var u = txtUser.Text.Trim();
            var p = txtPass.Text;
            if (DataAccess.VerifyUser(u, p))
            {
                Hide();
                var dashboard = new DashboardForm();
                dashboard.FormClosed += (s, a) => Close();
                dashboard.Show();
            }
            else
                MessageBox.Show("Invalid credentials");
        }
    }
}
