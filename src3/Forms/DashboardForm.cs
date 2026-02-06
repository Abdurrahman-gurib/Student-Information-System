using System;
using System.Drawing;
using System.Windows.Forms;
using StudentInfoApp3.Data;

namespace StudentInfoApp3.Forms
{
    public class DashboardForm : Form
    {
        Label lblHeaderTitle;
        Panel contentPanel;

        public DashboardForm()
        {
            Text = "University of Mauritius - Campus Portal";
            Width = 1650;
            Height = 950;
            StartPosition = FormStartPosition.CenterScreen;

            // LEFT SIDEBAR
            Panel sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = Color.FromArgb(15, 32, 65)
            };

            // LOGO
            Panel logoPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(10, 25, 55)
            };

            Label logo = new Label
            {
                Text = "🎓 UNIVERSITY OF MAURITIUS",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            logoPanel.Controls.Add(logo);
            sidebar.Controls.Add(logoPanel);

            // NAV BUTTONS
            AddNavButton(sidebar, "🏠 Dashboard", ShowDashboard);
            AddNavButton(sidebar, "👤 Student Corner", ShowStudentCorner);
            AddNavButton(sidebar, "📚 Courses & Reports", ShowCoursesReports);
            AddNavButton(sidebar, "💬 Student Exchange", ShowStudentExchange);
            AddNavButton(sidebar, "🗺️ Campus Explorer", ShowCampusExplorer);
            AddNavButton(sidebar, "📋 Code of Conduct", ShowCodeOfConduct);
            AddNavButton(sidebar, "🚑 First Aid", ShowFirstAid);
            AddNavButton(sidebar, "⚙️ Settings", ShowSettings);
            AddNavButton(sidebar, "🚪 Logout", Logout);

            // RIGHT PANEL
            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill
            };

            // HEADER
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 75,
                BackColor = Color.FromArgb(33, 150, 243)
            };

            lblHeaderTitle = new Label
            {
                Text = "Dashboard",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            header.Controls.Add(lblHeaderTitle);

            // MAIN CONTENT AREA
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White
            };

            rightPanel.Controls.Add(contentPanel);
            rightPanel.Controls.Add(header);

            Controls.Add(rightPanel);
            Controls.Add(sidebar);

            Load += (s, e) => ShowDashboard();
        }

        private void AddNavButton(Panel sidebar, string text, Action action)
        {
            Button btn = new Button
            {
                Text = text,
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = Color.FromArgb(30, 60, 120),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft
            };

            btn.FlatAppearance.BorderSize = 0;

            btn.Click += (s, e) =>
            {
                foreach (Control c in sidebar.Controls)
                    if (c is Button b)
                        b.BackColor = Color.FromArgb(30, 60, 120);

                btn.BackColor = Color.FromArgb(13, 100, 197);

                action();
            };

            sidebar.Controls.Add(btn);
        }

        private void ClearContent()
        {
            contentPanel.Controls.Clear();
        }

        private void ShowDashboard()
        {
            ClearContent();
            lblHeaderTitle.Text = "Dashboard";

            Label lbl = new Label
            {
                Text = "Welcome to University Portal Dashboard",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 30)
            };

            contentPanel.Controls.Add(lbl);
        }

        private void ShowStudentCorner()
        {
            ClearContent();
            lblHeaderTitle.Text = "Student Corner";

            Label lbl = new Label
            {
                Text = "Student Corner Page Loaded Successfully",
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                Location = new Point(30, 30)
            };

            contentPanel.Controls.Add(lbl);
        }

        private void ShowCoursesReports()
        {
            ClearContent();
            lblHeaderTitle.Text = "Courses & Reports";

            Label lbl = new Label
            {
                Text = "Courses and Reports Section",
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                Location = new Point(30, 30)
            };

            contentPanel.Controls.Add(lbl);
        }

        private void ShowStudentExchange()
        {
            ClearContent();
            lblHeaderTitle.Text = "Student Exchange";

            Label lbl = new Label
            {
                Text = "Student Exchange Section Loaded",
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                Location = new Point(30, 30)
            };

            contentPanel.Controls.Add(lbl);
        }

        private void ShowCampusExplorer()
        {
            ClearContent();
            lblHeaderTitle.Text = "Campus Explorer";

            Label lbl = new Label
            {
                Text = "Campus Explorer Loaded",
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                Location = new Point(30, 30)
            };

            contentPanel.Controls.Add(lbl);
        }

        private void ShowCodeOfConduct()
        {
            ClearContent();
            lblHeaderTitle.Text = "Code of Conduct";

            Label lbl = new Label
            {
                Text = "University Code of Conduct Section",
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                Location = new Point(30, 30)
            };

            contentPanel.Controls.Add(lbl);
        }

        private void ShowFirstAid()
        {
            ClearContent();
            lblHeaderTitle.Text = "First Aid Emergency";

            Label lbl = new Label
            {
                Text = "Emergency Contacts:\nAmbulance: 114\nPolice: 112\nFire: 115",
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                Location = new Point(30, 30)
            };

            contentPanel.Controls.Add(lbl);
        }

        private void ShowSettings()
        {
            ClearContent();
            lblHeaderTitle.Text = "Settings";

            Label lbl = new Label
            {
                Text = "Settings Page Loaded",
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                Location = new Point(30, 30)
            };

            contentPanel.Controls.Add(lbl);
        }

        private void Logout()
        {
            Close();
        }
    }
}
