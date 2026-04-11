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
        Button btnTogglePassword;
        LinkLabel lnkForgetPassword;

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
                Height = 540,
                Anchor = AnchorStyles.None
            };

            // Title
            Label lblTitle = new Label
            {
                Text = "Welcome Back",
                Font = new Font("Segoe UI", 32, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 150, 243),
                Location = new Point(30, 30),
                Size = new Size(350, 80),
                TextAlign = ContentAlignment.MiddleLeft
            };
            mainPanel.Controls.Add(lblTitle);

            // Subtitle
            Label lblSubtitle = new Label
            {
                Text = "Sign in to your account",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.FromArgb(120, 120, 120),
                Location = new Point(30, 102),
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

            // Password textbox with embedded eye icon inside the input panel
            Panel passwordPanel = new Panel
            {
                Location = new Point(30, 245),
                Width = 380,
                Height = 40,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            txtPass = new TextBox
            {
                Location = new Point(8, 7),
                Width = 320,
                Height = 26,
                Font = new Font("Segoe UI", 11),
                UseSystemPasswordChar = true,
                BorderStyle = BorderStyle.None,
                PlaceholderText = "Password"
            };
            passwordPanel.Controls.Add(txtPass);

            btnTogglePassword = new Button
            {
                Text = "👁",
                Location = new Point(330, 0),
                Width = 50,
                Height = 40,
                Font = new Font("Segoe UI", 12),
                BackColor = Color.White,
                ForeColor = Color.Gray,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnTogglePassword.FlatAppearance.BorderSize = 0;
            passwordPanel.Controls.Add(btnTogglePassword);
            mainPanel.Controls.Add(passwordPanel);

            // Forget password link
            lnkForgetPassword = new LinkLabel
            {
                Text = "Forgot Password?",
                Location = new Point(30, 295),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                LinkColor = Color.FromArgb(33, 150, 243),
                ActiveLinkColor = Color.FromArgb(17, 104, 210),
                VisitedLinkColor = Color.FromArgb(33, 150, 243)
            };
            mainPanel.Controls.Add(lnkForgetPassword);

            // Sign In button
            btnLogin = new Button
            {
                Text = "Sign In",
                Location = new Point(30, 340),
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
                Location = new Point(230, 340),
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
            Resize += (s, e) =>
            {
                mainPanel.Left = (ClientSize.Width - mainPanel.Width) / 2;
                mainPanel.Top = (ClientSize.Height - mainPanel.Height) / 2;
            };
            mainPanel.Left = (ClientSize.Width - mainPanel.Width) / 2;
            mainPanel.Top = (ClientSize.Height - mainPanel.Height) / 2;

            btnLogin.Click += BtnLogin_Click;
            btnRegister.Click += BtnRegister_Click;
            btnTogglePassword.Click += BtnTogglePassword_Click;
            lnkForgetPassword.Click += LnkForgetPassword_Click;
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
                var dashboard = new DashboardForm(u);
                dashboard.FormClosed += (s, a) => Close();
                dashboard.Show();
            }
            else
                MessageBox.Show("Invalid credentials");
        }

        private void BtnTogglePassword_Click(object sender, EventArgs e)
        {
            txtPass.UseSystemPasswordChar = !txtPass.UseSystemPasswordChar;
            btnTogglePassword.Text = txtPass.UseSystemPasswordChar ? "👁" : "🙈";
        }

        private void LnkForgetPassword_Click(object sender, EventArgs e)
        {
            Form resetForm = new Form
            {
                Text = "Reset Password",
                Width = 420,
                Height = 360,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblTitle = new Label
            {
                Text = "Reset Your Password",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };
            resetForm.Controls.Add(lblTitle);

            Label lblUser = new Label
            {
                Text = "Username:",
                Location = new Point(20, 60),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            resetForm.Controls.Add(lblUser);

            TextBox txtUsername = new TextBox
            {
                Location = new Point(20, 90),
                Width = 360,
                Font = new Font("Segoe UI", 10)
            };
            resetForm.Controls.Add(txtUsername);

            Label lblNewPass = new Label
            {
                Text = "New Password:",
                Location = new Point(20, 130),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            resetForm.Controls.Add(lblNewPass);

            TextBox txtNewPassword = new TextBox
            {
                Location = new Point(20, 160),
                Width = 360,
                PasswordChar = '*',
                Font = new Font("Segoe UI", 10)
            };
            resetForm.Controls.Add(txtNewPassword);

            Label lblConfirm = new Label
            {
                Text = "Confirm Password:",
                Location = new Point(20, 200),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            resetForm.Controls.Add(lblConfirm);

            TextBox txtConfirmPassword = new TextBox
            {
                Location = new Point(20, 230),
                Width = 360,
                PasswordChar = '*',
                Font = new Font("Segoe UI", 10)
            };
            resetForm.Controls.Add(txtConfirmPassword);

            Button btnReset = new Button
            {
                Text = "Reset Password",
                Location = new Point(140, 270),
                Width = 130,
                Height = 34,
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnReset.FlatAppearance.BorderSize = 0;
            btnReset.Click += (s, a) =>
            {
                string username = txtUsername.Text.Trim();
                string newPassword = txtNewPassword.Text;
                string confirmPassword = txtConfirmPassword.Text;

                if (string.IsNullOrWhiteSpace(username))
                {
                    MessageBox.Show("Please enter your username.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
                {
                    MessageBox.Show("New password must be at least 6 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (newPassword != confirmPassword)
                {
                    MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (DataAccess.ResetPassword(username, newPassword))
                {
                    MessageBox.Show("Password reset successfully! Please login with your new password.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    resetForm.Close();
                }
                else
                {
                    MessageBox.Show("Username not found or reset failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            resetForm.Controls.Add(btnReset);

            resetForm.ShowDialog();
        }
    }
}
