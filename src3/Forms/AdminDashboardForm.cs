using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using StudentInfoApp3.Data;
using StudentInfoApp3.Models;

namespace StudentInfoApp3.Forms
{
    public class AdminDashboardForm : Form
    {
        private TabControl tabControl;

        public AdminDashboardForm()
        {
            Text = "Admin Dashboard - Student Information System";
            Width = 1400;
            Height = 900;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10);
            MinimumSize = new Size(1000, 720);

            InitializeDashboardUI();
            Resize += AdminDashboardForm_Resize;
        }

        private void InitializeDashboardUI()
        {
            Controls.Clear();
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12)
            };

            var overviewTab = new TabPage("Overview");
            CreateOverviewTab(overviewTab);
            tabControl.TabPages.Add(overviewTab);

            var analyticsTab = new TabPage("Analytics");
            CreateAnalyticsTab(analyticsTab);
            tabControl.TabPages.Add(analyticsTab);

            var dataTab = new TabPage("Data Summary");
            CreateDataTab(dataTab);
            tabControl.TabPages.Add(dataTab);

            var reportsTab = new TabPage("Reports");
            CreateReportsTab(reportsTab);
            tabControl.TabPages.Add(reportsTab);

            Controls.Add(tabControl);
        }

        private void AdminDashboardForm_Resize(object sender, EventArgs e)
        {
            tabControl.Size = new Size(ClientSize.Width, ClientSize.Height);
        }

        private void CreateOverviewTab(TabPage tab)
        {
            tab.BackColor = Color.FromArgb(245, 247, 250);
            int totalStudents = DataAccess.GetTotalStudents();
            int totalCourses = DataAccess.GetTotalCourses();
            int totalEnrollments = DataAccess.GetTotalEnrollments();
            double avgGPA = DataAccess.GetAverageGPA();
            int totalSessions = DataAccess.GetTotalSessions();
            double avgSessionDuration = DataAccess.GetAverageSessionDuration();
            var issueCounts = DataAccess.GetIssueStatusCounts();
            issueCounts.TryGetValue("Open", out int openIssues);
            int activeUsers = DataAccess.GetActiveUserCount();

            var headerPanel = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(Width - 80, 90),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            var headerTitle = new Label
            {
                Text = "Operational Overview",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 44, 62),
                Location = new Point(20, 18),
                AutoSize = true
            };
            var headerSub = new Label
            {
                Text = "Live system metrics based on current database activity and student performance.",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(20, 50),
                AutoSize = true
            };
            headerPanel.Controls.Add(headerTitle);
            headerPanel.Controls.Add(headerSub);
            tab.Controls.Add(headerPanel);

            var cardFlow = new FlowLayoutPanel
            {
                Location = new Point(20, 130),
                Size = new Size(Width - 80, 320),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                AutoScroll = true,
                WrapContents = true
            };
            cardFlow.Controls.Add(CreateMetricCard("Total Students", totalStudents.ToString(), "Student profiles"));
            cardFlow.Controls.Add(CreateMetricCard("Total Courses", totalCourses.ToString(), "Course offerings"));
            cardFlow.Controls.Add(CreateMetricCard("Total Enrollments", totalEnrollments.ToString(), "Course registrations"));
            cardFlow.Controls.Add(CreateMetricCard("Average GPA", avgGPA.ToString("F2"), "System GPA average"));
            cardFlow.Controls.Add(CreateMetricCard("Total Sessions", totalSessions.ToString(), "Login history"));
            cardFlow.Controls.Add(CreateMetricCard("Avg Session", $"{avgSessionDuration:F1} min", "Average engagement"));
            cardFlow.Controls.Add(CreateMetricCard("Open Issues", openIssues.ToString(), "Pending attention"));
            cardFlow.Controls.Add(CreateMetricCard("Active Users", activeUsers.ToString(), "Recent participants"));
            tab.Controls.Add(cardFlow);

            var btnGenerateData = new Button
            {
                Text = "Regenerate Dummy Data",
                Size = new Size(240, 44),
                Location = new Point(20, 470),
                BackColor = Color.FromArgb(40, 84, 140),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnGenerateData.Click += BtnGenerateData_Click;
            tab.Controls.Add(btnGenerateData);

            var note = new Label
            {
                Text = "Refresh the data set only when you need a clean, realistic demo environment.",
                Location = new Point(280, 478),
                Size = new Size(760, 40),
                ForeColor = Color.FromArgb(100, 116, 139),
                Font = new Font("Segoe UI", 9)
            };
            tab.Controls.Add(note);
        }

        private void BtnGenerateData_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will rebuild the database with fresh dummy data and restore the admin account. Continue?", "Confirm Regenerate", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DataAccess.GenerateDummyData();
                MessageBox.Show("Dummy data regenerated successfully.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                InitializeDashboardUI();
            }
        }

        private Panel CreateMetricCard(string title, string value, string caption)
        {
            var card = new Panel
            {
                Size = new Size(310, 120),
                Margin = new Padding(10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var accent = new Panel
            {
                Size = new Size(6, 120),
                BackColor = Color.FromArgb(40, 84, 140),
                Dock = DockStyle.Left
            };
            card.Controls.Add(accent);

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 44, 62),
                Location = new Point(18, 15),
                AutoSize = true
            };
            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(10, 25, 52),
                Location = new Point(18, 45),
                AutoSize = true
            };
            var lblCaption = new Label
            {
                Text = caption,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(18, 85),
                AutoSize = true
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);
            card.Controls.Add(lblCaption);
            return card;
        }

        private void CreateAnalyticsTab(TabPage tab)
        {
            tab.BackColor = Color.FromArgb(245, 247, 250);
            var gradeDist = DataAccess.GetGradeDistribution();
            var topStudents = DataAccess.GetTopPerformingStudents(8);
            var popularCourses = DataAccess.GetMostPopularCourses(8);
            var issueCounts = DataAccess.GetIssueStatusCounts();

            var title = new Label
            {
                Text = "Academic Analytics",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 44, 62),
                Location = new Point(20, 20),
                AutoSize = true
            };
            tab.Controls.Add(title);

            var chartPanel = CreateGradeDistributionPanel(gradeDist);
            chartPanel.Location = new Point(20, 70);
            chartPanel.Size = new Size(520, 320);
            tab.Controls.Add(chartPanel);

            var topStudentGrid = CreateGridView(new[] { ("Student", 220), ("GPA", 100), ("Courses", 120) }, new Point(560, 70), new Size(520, 220));
            foreach (var student in topStudents)
            {
                double gpa = DataAccess.GetStudentGPA(student.StudentId);
                int courses = DataAccess.GetEnrolledCourses(student.StudentId).Count;
                topStudentGrid.Rows.Add($"{student.FirstName} {student.LastName}", gpa.ToString("F2"), courses);
            }
            tab.Controls.Add(topStudentGrid);

            var popularLabel = new Label
            {
                Text = "Most Enrolled Courses",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 44, 62),
                Location = new Point(560, 300),
                AutoSize = true
            };
            tab.Controls.Add(popularLabel);

            var popularGrid = CreateGridView(new[] { ("Course", 250), ("Enrollments", 100), ("Credits", 100) }, new Point(560, 330), new Size(520, 200));
            foreach (var course in popularCourses)
            {
                popularGrid.Rows.Add(course.Title, DataAccess.GetEnrollmentsForCourse(course.CourseId), course.Credits);
            }
            tab.Controls.Add(popularGrid);

            var issueLabel = new Label
            {
                Text = "Issue Status Summary",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 44, 62),
                Location = new Point(20, 410),
                AutoSize = true
            };
            tab.Controls.Add(issueLabel);

            var issuePanel = new FlowLayoutPanel
            {
                Location = new Point(20, 450),
                Size = new Size(520, 130),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoScroll = true
            };
            issuePanel.Controls.Add(CreateMetricCard("Open Issues", issueCounts.TryGetValue("Open", out var open) ? open.ToString() : "0", "Needs attention"));
            issuePanel.Controls.Add(CreateMetricCard("Pending", issueCounts.TryGetValue("Pending", out var pending) ? pending.ToString() : "0", "Under review"));
            issuePanel.Controls.Add(CreateMetricCard("Resolved", issueCounts.TryGetValue("Resolved", out var resolved) ? resolved.ToString() : "0", "Closed cases"));
            tab.Controls.Add(issuePanel);
        }

        private Panel CreateGradeDistributionPanel(Dictionary<string, int> distribution)
        {
            var panel = new Panel { BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            var title = new Label { Text = "Grade Distribution", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.FromArgb(30, 44, 62), Location = new Point(16, 12), AutoSize = true };
            panel.Controls.Add(title);

            string[] grades = { "A", "B", "C", "D", "F" };
            Color[] colors = { Color.FromArgb(40, 84, 140), Color.FromArgb(76, 132, 188), Color.FromArgb(123, 150, 179), Color.FromArgb(164, 182, 201), Color.FromArgb(196, 206, 219) };
            int maxCount = distribution.Values.DefaultIfEmpty(0).Max();
            int x = 40;
            for (int i = 0; i < grades.Length; i++)
            {
                int count = distribution.TryGetValue(grades[i], out var value) ? value : 0;
                int height = maxCount > 0 ? (count * 180) / maxCount : 0;
                var bar = new Panel { BackColor = colors[i], Size = new Size(50, height), Location = new Point(x, 260 - height) };
                panel.Controls.Add(bar);

                var gradeLabel = new Label { Text = grades[i], Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(55, 71, 79), Location = new Point(x, 270), Size = new Size(50, 22), TextAlign = ContentAlignment.MiddleCenter };
                var countLabel = new Label { Text = count.ToString(), Font = new Font("Segoe UI", 9), ForeColor = Color.FromArgb(55, 71, 79), Location = new Point(x, 240 - height), Size = new Size(50, 20), TextAlign = ContentAlignment.MiddleCenter };
                panel.Controls.Add(gradeLabel);
                panel.Controls.Add(countLabel);
                x += 80;
            }
            return panel;
        }

        private void CreateDataTab(TabPage tab)
        {
            tab.BackColor = Color.FromArgb(245, 247, 250);
            var title = new Label
            {
                Text = "Data Snapshot",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 44, 62),
                Location = new Point(20, 20),
                AutoSize = true
            };
            tab.Controls.Add(title);

            var dgvStudents = CreateGridView(new[] { ("Student ID", 140), ("Name", 220), ("Email", 260), ("GPA", 100) }, new Point(20, 70), new Size(640, 520));
            foreach (var student in DataAccess.GetStudents().Take(25))
            {
                dgvStudents.Rows.Add(student.StudentId, $"{student.FirstName} {student.LastName}", student.Email, DataAccess.GetStudentGPA(student.StudentId).ToString("F2"));
            }
            tab.Controls.Add(dgvStudents);

            var dgvCourses = CreateGridView(new[] { ("Code", 120), ("Title", 260), ("Credits", 80), ("Enrollments", 120) }, new Point(690, 70), new Size(620, 520));
            foreach (var course in DataAccess.GetCourses().Take(25))
            {
                dgvCourses.Rows.Add(course.Code, course.Title, course.Credits, DataAccess.GetEnrollmentsForCourse(course.CourseId));
            }
            tab.Controls.Add(dgvCourses);
        }

        private void CreateReportsTab(TabPage tab)
        {
            tab.BackColor = Color.FromArgb(245, 247, 250);
            var title = new Label
            {
                Text = "Actionable Reports",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 44, 62),
                Location = new Point(20, 20),
                AutoSize = true
            };
            tab.Controls.Add(title);

            var dgvIssues = CreateGridView(new[] { ("Student", 200), ("Issue Type", 150), ("Status", 110), ("Date", 150), ("Description", 400) }, new Point(20, 70), new Size(820, 300));
            var issueList = DataAccess.GetIssues();
            foreach (var issue in issueList)
            {
                dgvIssues.Rows.Add(issue.StudentName, issue.IssueType, issue.Status, issue.ReportedDate.ToString("yyyy-MM-dd"), issue.Description);
            }
            tab.Controls.Add(dgvIssues);

            var dgvSessions = CreateGridView(new[] { ("Username", 160), ("Login", 190), ("Logout", 190), ("Duration (min)", 120) }, new Point(20, 390), new Size(820, 280));
            foreach (var session in DataAccess.GetRecentSessions(15))
            {
                dgvSessions.Rows.Add(session.Username, session.LoginTime.ToString("g"), session.LogoutTime?.ToString("g") ?? "Active", session.Duration);
            }
            tab.Controls.Add(dgvSessions);

            var riskLabel = new Label
            {
                Text = "Students at Risk",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 44, 62),
                Location = new Point(860, 20),
                AutoSize = true
            };
            tab.Controls.Add(riskLabel);

            var lvLowGPA = new ListView
            {
                Location = new Point(860, 70),
                Size = new Size(450, 200),
                View = View.Details,
                FullRowSelect = true
            };
            lvLowGPA.Columns.Add("Student", 260);
            lvLowGPA.Columns.Add("GPA", 120);
            lvLowGPA.Columns.Add("Issues", 60);
            foreach (var student in DataAccess.GetStudents())
            {
                double gpa = DataAccess.GetStudentGPA(student.StudentId);
                if (gpa > 0 && gpa < 2.0)
                {
                    int issueCount = issueList.Count(i => i.StudentId == student.StudentId);
                    lvLowGPA.Items.Add(new ListViewItem(new[] { $"{student.FirstName} {student.LastName}", gpa.ToString("F2"), issueCount.ToString() }));
                }
            }
            tab.Controls.Add(lvLowGPA);

            var issueTypeLabel = new Label
            {
                Text = "Issue Types",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 44, 62),
                Location = new Point(860, 290),
                AutoSize = true
            };
            tab.Controls.Add(issueTypeLabel);

            var lvIssueTypes = new ListView
            {
                Location = new Point(860, 320),
                Size = new Size(450, 170),
                View = View.Details,
                FullRowSelect = true
            };
            lvIssueTypes.Columns.Add("Issue Type", 280);
            lvIssueTypes.Columns.Add("Count", 100);
            foreach (var item in DataAccess.GetIssueTypeCounts())
            {
                lvIssueTypes.Items.Add(new ListViewItem(new[] { item.IssueType, item.Count.ToString() }));
            }
            tab.Controls.Add(lvIssueTypes);
        }

        private DataGridView CreateGridView((string Text, int Width)[] columns, Point location, Size size)
        {
            var grid = new DataGridView
            {
                Location = location,
                Size = size,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            foreach (var (text, width) in columns)
            {
                grid.Columns.Add(text, text);
                grid.Columns[text].Width = width;
            }
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            return grid;
        }
    }
}
