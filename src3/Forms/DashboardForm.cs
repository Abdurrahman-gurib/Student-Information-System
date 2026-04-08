using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using StudentInfoApp3.Data;
using StudentInfoApp3.Models;

namespace StudentInfoApp3.Forms
{
    public class DashboardForm : Form
    {
        private Label lblHeaderTitle;
        private Panel contentPanel;
        private Panel sidebar;
        private Button btnToggleSidebar;
        private bool sidebarCollapsed;
        private Student currentStudent;
        private Action currentPageAction;

        private const int SidebarExpandedWidth = 245;
        private const int SidebarCollapsedWidth = 70;
        private const int HeaderHeight = 72;
        private const int PageMargin = 24;
        private const int SectionSpacing = 18;

        public DashboardForm()
        {
            currentStudent = DataAccess.GetStudents().FirstOrDefault();
            if (currentStudent == null)
            {
                MessageBox.Show("No student found. Please add a student first.");
                Close();
                return;
            }

            InitializeForm();
            BuildShell();

            Load += (s, e) => SetCurrentPage(ShowDashboard);
            Resize += (s, e) => currentPageAction?.Invoke();
        }

        private void InitializeForm()
        {
            Text = "University of Mauritius - Campus Portal";
            Width = 1650;
            Height = 950;
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1200, 760);
            BackColor = Color.FromArgb(240, 242, 245);
        }

        private void BuildShell()
        {
            sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = SidebarExpandedWidth,
                BackColor = Color.FromArgb(24, 55, 120)
            };

            Panel navPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            Panel logoPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 84,
                BackColor = Color.FromArgb(8, 28, 70)
            };

            Label logo = new Label
            {
                Name = "logoLabel",
                Text = "🎓 UNIVERSITY OF\r\nMAURITIUS",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            logoPanel.Controls.Add(logo);

            AddNavButton(navPanel, "🚪 Logout", Logout);
            AddNavButton(navPanel, "⚙️ Settings", ShowSettings);
            AddNavButton(navPanel, "📊 Grades", ShowGrades);
            AddNavButton(navPanel, "📚 Courses", ShowCourses);
            AddNavButton(navPanel, "📄 Documents", ShowDocuments);
            AddNavButton(navPanel, "👤 Profile", ShowProfile);
            AddNavButton(navPanel, "🏠 Dashboard", ShowDashboard, true);

            sidebar.Controls.Add(navPanel);
            sidebar.Controls.Add(logoPanel);

            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 242, 245)
            };

            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = HeaderHeight,
                BackColor = Color.FromArgb(33, 150, 243)
            };

            btnToggleSidebar = new Button
            {
                Text = "☰",
                Size = new Size(42, 42),
                Location = new Point(18, 15),
                BackColor = Color.FromArgb(17, 104, 210),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnToggleSidebar.FlatAppearance.BorderSize = 0;
            btnToggleSidebar.Click += (s, e) => ToggleSidebar();

            lblHeaderTitle = new Label
            {
                Text = "Dashboard",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(74, 18)
            };

            header.Controls.Add(btnToggleSidebar);
            header.Controls.Add(lblHeaderTitle);

            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(240, 242, 245),
                Padding = new Padding(PageMargin)
            };

            rightPanel.Controls.Add(contentPanel);
            rightPanel.Controls.Add(header);

            Controls.Add(rightPanel);
            Controls.Add(sidebar);
        }

        private void AddNavButton(Control parent, string text, Action action, bool active = false)
        {
            Button btn = new Button
            {
                Text = text,
                Tag = text,
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = active ? Color.FromArgb(28, 108, 208) : Color.FromArgb(36, 67, 133),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (s, e) =>
            {
                HighlightNav(btn);
                SetCurrentPage(action);
            };

            parent.Controls.Add(btn);
        }

        private void HighlightNav(Button activeButton)
        {
            HighlightNavRecursive(sidebar, activeButton);
        }

        private void HighlightNavRecursive(Control parent, Button activeButton)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Button b && b.Tag is string)
                {
                    b.BackColor = b == activeButton
                        ? Color.FromArgb(28, 108, 208)
                        : Color.FromArgb(36, 67, 133);
                }

                if (c.HasChildren)
                    HighlightNavRecursive(c, activeButton);
            }
        }

        private void ToggleSidebar()
        {
            sidebarCollapsed = !sidebarCollapsed;
            sidebar.Width = sidebarCollapsed ? SidebarCollapsedWidth : SidebarExpandedWidth;

            ToggleSidebarText(sidebar);
        }

        private void ToggleSidebarText(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is Button btn && btn.Tag is string fullText)
                {
                    string iconOnly = fullText.Split(' ')[0];
                    btn.Text = sidebarCollapsed ? iconOnly : fullText;
                    btn.TextAlign = sidebarCollapsed ? ContentAlignment.MiddleCenter : ContentAlignment.MiddleLeft;
                    btn.Padding = sidebarCollapsed ? new Padding(0) : new Padding(12, 0, 0, 0);
                }
                else if (control is Label lbl && lbl.Name == "logoLabel")
                {
                    lbl.Text = sidebarCollapsed ? "🎓" : "🎓 UNIVERSITY OF\r\nMAURITIUS";
                }

                if (control.HasChildren)
                    ToggleSidebarText(control);
            }
        }

        private void SetCurrentPage(Action pageAction)
        {
            currentPageAction = pageAction;
            currentPageAction?.Invoke();
        }

        private void ClearContent()
        {
            contentPanel.Controls.Clear();
        }

        private FlowLayoutPanel CreatePage(string title)
        {
            ClearContent();

            FlowLayoutPanel page = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(0),
                Padding = new Padding(0),
                BackColor = Color.Transparent,
                Width = contentPanel.ClientSize.Width - 10
            };

            Label pageTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(4, 0, 4, 16)
            };

            page.Controls.Add(pageTitle);
            contentPanel.Controls.Add(page);

            return page;
        }

        private Panel AddSection(FlowLayoutPanel page, string title, int height = 140)
        {
            int width = Math.Max(700, contentPanel.ClientSize.Width - 45);

            Panel section = new Panel
            {
                Width = width,
                Height = height,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, SectionSpacing),
                Padding = new Padding(18)
            };

            Label titleLabel = new Label
            {
                Text = title,
                Dock = DockStyle.Top,
                Height = 34,
                Font = new Font("Segoe UI", 16, FontStyle.Bold)
            };

            Panel body = new Panel
            {
                Name = "Body",
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(0, 10, 0, 0)
            };

            section.Controls.Add(body);
            section.Controls.Add(titleLabel);
            page.Controls.Add(section);

            return body;
        }

        private FlowLayoutPanel CreateWrapFlow(Control parent, bool topDown = false)
        {
            FlowLayoutPanel flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = topDown ? FlowDirection.TopDown : FlowDirection.LeftToRight,
                WrapContents = !topDown,
                AutoScroll = true,
                BackColor = Color.White,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            parent.Controls.Add(flow);
            return flow;
        }

        private TableLayoutPanel CreateFormTable(Control parent, params float[] columns)
        {
            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = columns.Length,
                RowCount = 0
            };

            foreach (float col in columns)
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, col));

            parent.Controls.Add(table);
            return table;
        }

        private void AddFormRow(TableLayoutPanel table, string labelText, Control input)
        {
            int row = table.RowCount;
            table.RowCount += 1;
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            Label lbl = new Label
            {
                Text = labelText,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Anchor = AnchorStyles.Left,
                AutoSize = true,
                Margin = new Padding(0, 8, 10, 8)
            };

            input.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            input.Margin = new Padding(0, 4, 0, 4);

            table.Controls.Add(lbl, 0, row);
            table.Controls.Add(input, 1, row);
        }

        private Panel CreateStatCard(string title, string value, string subtitle, Color accent)
        {
            Panel card = new Panel
            {
                Width = 245,
                Height = 135,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 16, 16)
            };

            Panel topBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 4,
                BackColor = accent
            };

            Label lblTitle = new Label
            {
                Text = title,
                Location = new Point(14, 16),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(90, 90, 90)
            };

            Label lblValue = new Label
            {
                Text = value,
                Location = new Point(14, 42),
                AutoSize = true,
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = accent
            };

            Label lblSubtitle = new Label
            {
                Text = subtitle,
                Location = new Point(14, 90),
                Size = new Size(210, 28),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(130, 130, 130)
            };

            card.Controls.Add(lblSubtitle);
            card.Controls.Add(lblValue);
            card.Controls.Add(lblTitle);
            card.Controls.Add(topBar);

            return card;
        }

        private Button CreatePrimaryButton(string text, Action action)
        {
            Button btn = new Button
            {
                Text = text,
                Width = 145,
                Height = 38,
                Margin = new Padding(0, 0, 12, 12),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (s, e) => action();

            return btn;
        }

        private Button CreateSmallButton(string text, int x, int y, int width, Color color)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Width = width,
                Height = 30,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private Panel CreateCourseCard(Course course, bool isEnrolled)
        {
            Panel card = new Panel
            {
                Width = 300,
                Height = 180,
                Margin = new Padding(0, 0, 16, 16),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = isEnrolled ? Color.FromArgb(76, 175, 80) : Color.FromArgb(33, 150, 243)
            };

            Label lblStatus = new Label
            {
                Text = isEnrolled ? "✓ ENROLLED" : "📚 AVAILABLE",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White
            };
            header.Controls.Add(lblStatus);

            Label lblCode = new Label
            {
                Text = course.Code,
                Location = new Point(15, 52),
                AutoSize = true,
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };

            Label lblTitle = new Label
            {
                Text = course.Title.Length > 25 ? course.Title.Substring(0, 22) + "..." : course.Title,
                Location = new Point(15, 78),
                Size = new Size(265, 22),
                Font = new Font("Segoe UI", 12)
            };

            Label lblCredits = new Label
            {
                Text = $"🎓 {course.Credits} Credits",
                Location = new Point(15, 102),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(100, 100, 100)
            };

            Label lblDesc = new Label
            {
                Text = course.Description.Length > 40 ? course.Description.Substring(0, 37) + "..." : course.Description,
                Location = new Point(15, 124),
                Size = new Size(270, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(150, 150, 150)
            };

            Button btnAction = CreateSmallButton(
                isEnrolled ? "Unenroll" : "Enroll",
                15, 145, 85,
                isEnrolled ? Color.FromArgb(244, 67, 54) : Color.FromArgb(33, 150, 243));

            btnAction.Click += (s, e) =>
            {
                if (isEnrolled)
                {
                    if (MessageBox.Show($"Unenroll from {course.Title}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (DataAccess.UnenrollStudent(currentStudent.StudentId, course.CourseId))
                            SetCurrentPage(ShowCourses);
                    }
                }
                else
                {
                    if (DataAccess.EnrollStudent(currentStudent.StudentId, course.CourseId))
                        SetCurrentPage(ShowCourses);
                }
            };

            Button btnDetails = CreateSmallButton("Details", 108, 145, 72, Color.Gray);
            btnDetails.Click += (s, e) =>
            {
                MessageBox.Show(
                    $"Course: {course.Title}\nCode: {course.Code}\nCredits: {course.Credits}\n\nDescription:\n{course.Description}",
                    "Course Details",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            };

            card.Controls.Add(btnDetails);
            card.Controls.Add(btnAction);
            card.Controls.Add(lblDesc);
            card.Controls.Add(lblCredits);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblCode);
            card.Controls.Add(header);

            return card;
        }

        private Panel CreateDocumentCard(Document doc)
        {
            Panel card = new Panel
            {
                Width = 285,
                Height = 165,
                Margin = new Padding(0, 0, 16, 16),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            string icon = doc.DocumentType switch
            {
                "ID Card" => "🆔",
                "Transcript" => "📜",
                "Certificate" => "🏆",
                "Assignment" => "📝",
                "Research Paper" => "📄",
                _ => "📎"
            };

            Label lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI Emoji", 26),
                Location = new Point(14, 16),
                AutoSize = true
            };

            Label lblType = new Label
            {
                Text = doc.DocumentType,
                Location = new Point(70, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            Label lblFile = new Label
            {
                Text = doc.FileName.Length > 28 ? doc.FileName.Substring(0, 25) + "..." : doc.FileName,
                Location = new Point(70, 46),
                Size = new Size(185, 36),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(110, 110, 110)
            };

            Label lblDate = new Label
            {
                Text = $"📅 {doc.UploadDate:dd/MM/yyyy}",
                Location = new Point(70, 84),
                AutoSize = true,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(140, 140, 140)
            };

            Button btnView = CreateSmallButton("View", 14, 118, 70, Color.FromArgb(33, 150, 243));
            btnView.Click += (s, e) => MessageBox.Show($"Viewing: {doc.FileName}");

            Button btnDownload = CreateSmallButton("Download", 92, 118, 84, Color.FromArgb(76, 175, 80));
            btnDownload.Click += (s, e) => MessageBox.Show($"Downloading: {doc.FileName}");

            Button btnDelete = CreateSmallButton("Delete", 184, 118, 70, Color.FromArgb(244, 67, 54));
            btnDelete.Click += (s, e) =>
            {
                if (MessageBox.Show($"Delete {doc.FileName}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (DataAccess.DeleteDocument(doc.DocumentId))
                        SetCurrentPage(ShowDocuments);
                }
            };

            card.Controls.Add(btnDelete);
            card.Controls.Add(btnDownload);
            card.Controls.Add(btnView);
            card.Controls.Add(lblDate);
            card.Controls.Add(lblFile);
            card.Controls.Add(lblType);
            card.Controls.Add(lblIcon);

            return card;
        }

        private Panel CreateGradeCard(Grade grade)
        {
            var course = DataAccess.GetCourses().FirstOrDefault(c => c.CourseId == grade.CourseId);
            string courseCode = course?.Code ?? "N/A";
            string courseTitle = course?.Title ?? "Unknown Course";

            Panel card = new Panel
            {
                Width = 300,
                Height = 140,
                Margin = new Padding(0, 0, 16, 16),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblCode = new Label
            {
                Text = courseCode,
                Location = new Point(15, 14),
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            Label lblTitle = new Label
            {
                Text = courseTitle.Length > 25 ? courseTitle.Substring(0, 22) + "..." : courseTitle,
                Location = new Point(15, 38),
                Size = new Size(260, 20),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(100, 100, 100)
            };

            Panel gradePanel = new Panel
            {
                Location = new Point(15, 68),
                Size = new Size(84, 48),
                BackColor = GetGradeColor(grade.GradeValue)
            };

            Label lblGrade = new Label
            {
                Text = grade.GradeValue,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White
            };
            gradePanel.Controls.Add(lblGrade);

            Label lblSemester = new Label
            {
                Text = $"{grade.Semester} {grade.Year}",
                Location = new Point(112, 74),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Label lblPoints = new Label
            {
                Text = $"{GradeToPoint(grade.GradeValue):F1} points",
                Location = new Point(112, 96),
                AutoSize = true,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(140, 140, 140)
            };

            Button btnDetails = CreateSmallButton("Details", 206, 100, 78, Color.Gray);
            btnDetails.Click += (s, e) =>
            {
                MessageBox.Show(
                    $"Course: {courseTitle}\nCode: {courseCode}\nGrade: {grade.GradeValue}\nPoints: {GradeToPoint(grade.GradeValue):F1}\nSemester: {grade.Semester} {grade.Year}",
                    "Grade Details",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            };

            card.Controls.Add(btnDetails);
            card.Controls.Add(lblPoints);
            card.Controls.Add(lblSemester);
            card.Controls.Add(gradePanel);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblCode);

            return card;
        }

        private TextBox CreateBoundTextBox(string value, Action<string> setter)
        {
            TextBox txt = new TextBox
            {
                Text = value ?? string.Empty,
                Width = 420,
                Font = new Font("Segoe UI", 10)
            };
            txt.TextChanged += (s, e) => setter(txt.Text);
            return txt;
        }

        private void ShowDashboard()
        {
            lblHeaderTitle.Text = "Dashboard";
            FlowLayoutPanel page = CreatePage("Dashboard Overview");

            var enrolledCourses = DataAccess.GetEnrolledCourses(currentStudent.StudentId);
            var allCourses = DataAccess.GetCourses();
            var grades = DataAccess.GetStudentGrades(currentStudent.StudentId);
            var documents = DataAccess.GetStudentDocuments(currentStudent.StudentId);

            var availableCourses = allCourses
                .Where(c => !enrolledCourses.Select(ec => ec.CourseId).Contains(c.CourseId))
                .ToList();

            double avgGrade = grades.Any() ? grades.Average(g => GradeToPoint(g.GradeValue)) : 0;
            string gpaText = grades.Any() ? avgGrade.ToString("F2") : "N/A";
            int totalCredits = enrolledCourses.Sum(c => c.Credits);

            Panel welcomeBody = AddSection(page, "Welcome", 110);
            welcomeBody.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Text = $"🎓 Welcome back, {currentStudent.FirstName} {currentStudent.LastName}!\nStudent ID: {currentStudent.StudentId:D4} | Email: {currentStudent.Email}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            });

            Panel statsBody = AddSection(page, "Overview", 225);
            FlowLayoutPanel statsFlow = CreateWrapFlow(statsBody);
            statsFlow.Controls.Add(CreateStatCard("📚 Enrolled Courses", $"{enrolledCourses.Count}", "Active courses this semester", Color.FromArgb(33, 150, 243)));
            statsFlow.Controls.Add(CreateStatCard("📊 GPA", gpaText, "Current academic standing", Color.FromArgb(76, 175, 80)));
            statsFlow.Controls.Add(CreateStatCard("📄 Documents", $"{documents.Count}", "Uploaded files", Color.FromArgb(255, 152, 0)));
            statsFlow.Controls.Add(CreateStatCard("🎯 Credits", $"{totalCredits}", "Credits this semester", Color.FromArgb(156, 39, 176)));

            Panel enrolledBody = AddSection(page, "My Courses", 525);
            FlowLayoutPanel enrolledFlow = CreateWrapFlow(enrolledBody);
            foreach (var course in enrolledCourses)
                enrolledFlow.Controls.Add(CreateCourseCard(course, true));

            Panel availableBody = AddSection(page, "Available Courses", 340);
            FlowLayoutPanel availableFlow = CreateWrapFlow(availableBody);
            foreach (var course in availableCourses)
                availableFlow.Controls.Add(CreateCourseCard(course, false));

            Panel quickBody = AddSection(page, "Quick Actions", 140);
            FlowLayoutPanel quickFlow = CreateWrapFlow(quickBody);
            quickFlow.Controls.Add(CreatePrimaryButton("View Profile", ShowProfile));
            quickFlow.Controls.Add(CreatePrimaryButton("Browse Courses", ShowCourses));
            quickFlow.Controls.Add(CreatePrimaryButton("View Grades", ShowGrades));
            quickFlow.Controls.Add(CreatePrimaryButton("Upload Document", ShowDocuments));

            Panel activityBody = AddSection(page, "Recent Activity", 200);
            TableLayoutPanel activityTable = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                AutoSize = true
            };
            activityTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72));
            activityTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28));

            foreach (var grade in grades.OrderByDescending(g => g.Year).ThenByDescending(g => g.Semester).Take(5))
            {
                string courseName = DataAccess.GetCourses().FirstOrDefault(c => c.CourseId == grade.CourseId)?.Title ?? "Unknown Course";

                int row = activityTable.RowCount++;
                activityTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                activityTable.Controls.Add(new Label
                {
                    Text = $"Grade: {grade.GradeValue} in {courseName}",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10),
                    Margin = new Padding(0, 0, 0, 8)
                }, 0, row);

                activityTable.Controls.Add(new Label
                {
                    Text = $"{grade.Semester} {grade.Year}",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.FromArgb(110, 110, 110),
                    Anchor = AnchorStyles.Right,
                    Margin = new Padding(0, 0, 0, 8)
                }, 1, row);
            }

            activityBody.Controls.Add(activityTable);
        }

        private void ShowProfile()
        {
            lblHeaderTitle.Text = "Student Profile";
            FlowLayoutPanel page = CreatePage("Student Profile");

            Panel actionBody = AddSection(page, "Actions", 150);
            FlowLayoutPanel actionFlow = CreateWrapFlow(actionBody);
            actionFlow.Controls.Add(CreatePrimaryButton("Save Changes", () =>
            {
                if (DataAccess.UpdateStudent(currentStudent))
                {
                    MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SetCurrentPage(ShowProfile);
                }
                else
                {
                    MessageBox.Show("Failed to update profile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }));

            Button btnReset = new Button
            {
                Text = "Reset",
                Width = 120,
                Height = 38,
                Margin = new Padding(0, 0, 12, 12),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnReset.FlatAppearance.BorderSize = 0;
            btnReset.Click += (s, e) => SetCurrentPage(ShowProfile);
            actionFlow.Controls.Add(btnReset);

            Panel infoBody = AddSection(page, "Personal Information", 340);
            TableLayoutPanel form = CreateFormTable(infoBody, 28, 72);

            AddFormRow(form, "First Name:", CreateBoundTextBox(currentStudent.FirstName, v => currentStudent.FirstName = v));
            AddFormRow(form, "Last Name:", CreateBoundTextBox(currentStudent.LastName, v => currentStudent.LastName = v));
            AddFormRow(form, "Email:", CreateBoundTextBox(currentStudent.Email, v => currentStudent.Email = v));
            AddFormRow(form, "Phone:", CreateBoundTextBox(currentStudent.Phone, v => currentStudent.Phone = v));
            AddFormRow(form, "Address:", CreateBoundTextBox(currentStudent.Address, v => currentStudent.Address = v));

            DateTimePicker dtpDob = new DateTimePicker
            {
                Value = currentStudent.DateOfBirth ?? DateTime.Today,
                Width = 280,
                Font = new Font("Segoe UI", 10)
            };
            dtpDob.ValueChanged += (s, e) => currentStudent.DateOfBirth = dtpDob.Value;
            AddFormRow(form, "Date of Birth:", dtpDob);

            Panel summaryBody = AddSection(page, "Profile Summary", 230);

            Panel summary = new Panel
            {
                Dock = DockStyle.Top,
                Height = 85,
                BackColor = Color.White
            };

            Panel avatar = new Panel
            {
                Size = new Size(86, 86),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(66, 165, 245)
            };

            avatar.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Text = $"{currentStudent.FirstName.FirstOrDefault()}{currentStudent.LastName.FirstOrDefault()}",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.White
            });

            summary.Controls.Add(avatar);
            summary.Controls.Add(new Label
            {
                Text = $"{currentStudent.FirstName} {currentStudent.LastName}",
                Location = new Point(105, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 22, FontStyle.Bold)
            });
            summary.Controls.Add(new Label
            {
                Text = $"Student ID: {currentStudent.StudentId:D4}    Email: {currentStudent.Email}",
                Location = new Point(105, 48),
                AutoSize = true,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(95, 95, 95)
            });

            summaryBody.Controls.Add(summary);
        }

        private void ShowDocuments()
        {
            lblHeaderTitle.Text = "Document Management";
            FlowLayoutPanel page = CreatePage("Document Management");

            var docs = DataAccess.GetStudentDocuments(currentStudent.StudentId);

            Panel docsBody = AddSection(page, "Your Documents", 260);
            FlowLayoutPanel docsFlow = CreateWrapFlow(docsBody);
            foreach (var doc in docs.OrderByDescending(d => d.UploadDate))
                docsFlow.Controls.Add(CreateDocumentCard(doc));

            Panel uploadBody = AddSection(page, "Upload New Document", 250);
            TableLayoutPanel uploadTable = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 4,
                AutoSize = true
            };
            uploadTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18));
            uploadTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
            uploadTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18));
            uploadTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
            uploadBody.Controls.Add(uploadTable);

            uploadTable.Controls.Add(new Label
            {
                Text = "Document Type:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Anchor = AnchorStyles.Left,
                AutoSize = true
            }, 0, 0);

            ComboBox cbType = new ComboBox
            {
                Font = new Font("Segoe UI", 10),
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbType.Items.AddRange(new object[] { "ID Card", "Transcript", "Certificate", "Assignment", "Research Paper", "Other" });
            cbType.SelectedIndex = 0;
            uploadTable.Controls.Add(cbType, 1, 0);

            uploadTable.Controls.Add(new Label
            {
                Text = "File Name:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Anchor = AnchorStyles.Left,
                AutoSize = true
            }, 2, 0);

            TextBox txtFileName = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Dock = DockStyle.Fill
            };
            uploadTable.Controls.Add(txtFileName, 3, 0);

            FlowLayoutPanel buttonRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 50,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0, 12, 0, 0)
            };

            buttonRow.Controls.Add(CreatePrimaryButton("Upload", () =>
            {
                if (string.IsNullOrWhiteSpace(txtFileName.Text))
                {
                    MessageBox.Show("Please enter a file name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var doc = new Document
                {
                    StudentId = currentStudent.StudentId,
                    DocumentType = cbType.Text,
                    FileName = txtFileName.Text.Trim()
                };

                if (DataAccess.AddDocument(doc))
                {
                    MessageBox.Show("Document uploaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SetCurrentPage(ShowDocuments);
                }
                else
                {
                    MessageBox.Show("Failed to upload document.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }));

            uploadBody.Controls.Add(buttonRow);

            Panel summaryBody = AddSection(page, "Document Summary", 180);
            summaryBody.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Text = $"Total Documents: {docs.Count} | Types: {docs.Select(d => d.DocumentType).Distinct().Count()}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            });
        }

        private void ShowCourses()
        {
            lblHeaderTitle.Text = "Course Management";
            FlowLayoutPanel page = CreatePage("Course Management");

            var enrolledCourses = DataAccess.GetEnrolledCourses(currentStudent.StudentId);
            var totalCredits = enrolledCourses.Sum(c => c.Credits);
            var allCourses = DataAccess.GetCourses();
            var enrolledIds = enrolledCourses.Select(c => c.CourseId).ToHashSet();
            var availableCourses = allCourses.Where(c => !enrolledIds.Contains(c.CourseId)).ToList();

            Panel enrolledBody = AddSection(page, "My Courses", 280);
            FlowLayoutPanel enrolledFlow = CreateWrapFlow(enrolledBody);
            foreach (var course in enrolledCourses)
                enrolledFlow.Controls.Add(CreateCourseCard(course, true));

            Panel availableBody = AddSection(page, "Available Courses", 280);
            FlowLayoutPanel availableFlow = CreateWrapFlow(availableBody);
            foreach (var course in availableCourses)
                availableFlow.Controls.Add(CreateCourseCard(course, false));

            Panel summaryBody = AddSection(page, "Course Summary", 160);
            summaryBody.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Text = $"Total Credits: {totalCredits} | GPA: {DataAccess.GetStudentGPA(currentStudent.StudentId):F2}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            });
        }

        private void ShowGrades()
        {
            lblHeaderTitle.Text = "Grades Academic Performance";
            FlowLayoutPanel page = CreatePage("Grades Performance");

            var grades = DataAccess.GetStudentGrades(currentStudent.StudentId);
            double gpa = grades.Any() ? grades.Average(g => GradeToPoint(g.GradeValue)) : 0.0;
            var gradeCounts = grades.GroupBy(g => g.GradeValue)
                                    .Select(g => $"{g.Key}:{g.Count()}")
                                    .OrderBy(x => x)
                                    .ToList();

            Panel gradesBody = AddSection(page, "Grade History", 250);
            FlowLayoutPanel gradesFlow = CreateWrapFlow(gradesBody);
            foreach (var grade in grades.OrderByDescending(g => g.Year).ThenByDescending(g => g.Semester))
                gradesFlow.Controls.Add(CreateGradeCard(grade));

            Panel filterBody = AddSection(page, "Filter Grades", 185);
            TableLayoutPanel filterTable = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 3,
                AutoSize = true
            };
            filterTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            filterTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            filterTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            filterBody.Controls.Add(filterTable);

            filterTable.Controls.Add(new Label
            {
                Text = "Semester:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Anchor = AnchorStyles.Left
            }, 0, 0);

            ComboBox cbSemester = new ComboBox
            {
                Font = new Font("Segoe UI", 10),
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbSemester.Items.Add("All Semesters");
            foreach (var semester in grades.Select(g => $"{g.Semester} {g.Year}").Distinct())
                cbSemester.Items.Add(semester);
            cbSemester.SelectedIndex = 0;
            filterTable.Controls.Add(cbSemester, 1, 0);

            Button btnApply = CreatePrimaryButton("Apply", () =>
            {
                if (cbSemester.Text == "All Semesters")
                {
                    SetCurrentPage(ShowGrades);
                    return;
                }

                ShowGradesFiltered(cbSemester.Text);
            });
            btnApply.Width = 110;
            filterTable.Controls.Add(btnApply, 2, 0);

            Panel overviewBody = AddSection(page, "Academic Overview", 255);
            FlowLayoutPanel overviewFlow = CreateWrapFlow(overviewBody);
            overviewFlow.Controls.Add(CreateStatCard("Overall GPA", $"{gpa:F2}", "Current academic standing", GetGpaColor(gpa)));
            overviewFlow.Controls.Add(CreateStatCard("Total Grades", $"{grades.Count}", "Recorded results", Color.FromArgb(33, 150, 243)));
            overviewFlow.Controls.Add(CreateStatCard("Distribution", string.Join(", ", gradeCounts), "Grade distribution", Color.FromArgb(255, 152, 0)));
        }

        private void ShowGradesFiltered(string semesterText)
        {
            lblHeaderTitle.Text = "Grades Academic Performance";
            FlowLayoutPanel page = CreatePage($"Grades - {semesterText}");

            var filtered = DataAccess.GetStudentGrades(currentStudent.StudentId)
                                     .Where(g => $"{g.Semester} {g.Year}" == semesterText)
                                     .ToList();

            Panel body = AddSection(page, "Filtered Grade History", 220);
            FlowLayoutPanel flow = CreateWrapFlow(body);

            foreach (var grade in filtered)
                flow.Controls.Add(CreateGradeCard(grade));
        }

        private void ShowSettings()
        {
            lblHeaderTitle.Text = "Settings Preferences";
            FlowLayoutPanel page = CreatePage("Settings Preferences");

            Panel systemBody = AddSection(page, "System Preferences", 120);
            TableLayoutPanel systemTable = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 4,
                AutoSize = true
            };
            systemTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18));
            systemTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
            systemTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18));
            systemTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
            systemBody.Controls.Add(systemTable);

            systemTable.Controls.Add(new Label
            {
                Text = "Theme:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Anchor = AnchorStyles.Left
            }, 0, 0);

            ComboBox cbTheme = new ComboBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbTheme.Items.AddRange(new object[] { "Light", "Dark", "Auto" });
            cbTheme.SelectedIndex = 0;
            systemTable.Controls.Add(cbTheme, 1, 0);

            systemTable.Controls.Add(new Label
            {
                Text = "Language:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Anchor = AnchorStyles.Left
            }, 2, 0);

            ComboBox cbLanguage = new ComboBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbLanguage.Items.AddRange(new object[] { "English", "French", "Spanish" });
            cbLanguage.SelectedIndex = 0;
            systemTable.Controls.Add(cbLanguage, 3, 0);

            Panel privacyBody = AddSection(page, "Privacy & Security", 200);
            FlowLayoutPanel privacyFlow = CreateWrapFlow(privacyBody, true);

            privacyFlow.Controls.Add(new CheckBox
            {
                Text = "Make profile visible to other students",
                Font = new Font("Segoe UI", 10),
                Checked = true,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            });

            privacyFlow.Controls.Add(new CheckBox
            {
                Text = "Allow activity tracking for analytics",
                Font = new Font("Segoe UI", 10),
                Checked = false,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            });

            privacyFlow.Controls.Add(CreatePrimaryButton("Change Password", () =>
            {
                MessageBox.Show("Password change feature coming soon!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }));

            Panel notificationBody = AddSection(page, "Notification Preferences", 245);
            FlowLayoutPanel notificationFlow = CreateWrapFlow(notificationBody, true);

            notificationFlow.Controls.Add(new CheckBox
            {
                Text = "Email notifications for important updates",
                Font = new Font("Segoe UI", 10),
                Checked = true,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            });

            notificationFlow.Controls.Add(new CheckBox
            {
                Text = "Grade release notifications",
                Font = new Font("Segoe UI", 10),
                Checked = true,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            });

            notificationFlow.Controls.Add(new CheckBox
            {
                Text = "Course enrollment and schedule changes",
                Font = new Font("Segoe UI", 10),
                Checked = true,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            });

            notificationFlow.Controls.Add(new CheckBox
            {
                Text = "Document approval notifications",
                Font = new Font("Segoe UI", 10),
                Checked = false,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            });

            Panel profileBody = AddSection(page, "Profile Settings", 210);
            TableLayoutPanel profileTable = CreateFormTable(profileBody, 25, 75);

            Button btnPicture = new Button
            {
                Text = "Change Picture",
                Width = 150,
                Height = 34,
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };
            btnPicture.FlatAppearance.BorderSize = 0;
            btnPicture.Click += (s, e) => MessageBox.Show("Profile picture upload feature coming soon!");

            AddFormRow(profileTable, "Profile Picture:", btnPicture);

            TextBox txtDisplayName = new TextBox
            {
                Text = $"{currentStudent.FirstName} {currentStudent.LastName}",
                Width = 420,
                Font = new Font("Segoe UI", 10)
            };
            AddFormRow(profileTable, "Display Name:", txtDisplayName);

            TextBox txtBio = new TextBox
            {
                Text = "Computer Science student passionate about technology and innovation.",
                Width = 420,
                Height = 70,
                Multiline = true,
                Font = new Font("Segoe UI", 10)
            };
            AddFormRow(profileTable, "Bio:", txtBio);

            Panel actionBody = AddSection(page, "Actions", 150);
            FlowLayoutPanel actionFlow = CreateWrapFlow(actionBody);
            actionFlow.Controls.Add(CreatePrimaryButton("Save All Settings", () =>
            {
                MessageBox.Show("Settings saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }));
        }

        private double GradeToPoint(string grade)
        {
            return grade?.ToUpper() switch
            {
                "A+" => 4.0,
                "A" => 4.0,
                "A-" => 3.7,
                "B+" => 3.3,
                "B" => 3.0,
                "B-" => 2.7,
                "C+" => 2.3,
                "C" => 2.0,
                "C-" => 1.7,
                "D+" => 1.3,
                "D" => 1.0,
                "F" => 0.0,
                _ => 0.0
            };
        }

        private Color GetGpaColor(double gpa)
        {
            if (gpa >= 3.5) return Color.FromArgb(76, 175, 80);
            if (gpa >= 3.0) return Color.FromArgb(255, 152, 0);
            if (gpa >= 2.0) return Color.FromArgb(255, 193, 7);
            return Color.FromArgb(244, 67, 54);
        }

        private Color GetGradeColor(string grade)
        {
            return grade?.ToUpper() switch
            {
                "A+" => Color.FromArgb(56, 142, 60),
                "A" => Color.FromArgb(76, 175, 80),
                "A-" => Color.FromArgb(102, 187, 106),
                "B+" => Color.FromArgb(255, 152, 0),
                "B" => Color.FromArgb(255, 167, 38),
                "B-" => Color.FromArgb(255, 183, 77),
                "C+" => Color.FromArgb(255, 193, 7),
                "C" => Color.FromArgb(255, 213, 79),
                "D" => Color.FromArgb(244, 67, 54),
                "F" => Color.FromArgb(211, 47, 47),
                _ => Color.FromArgb(158, 158, 158)
            };
        }

        private void Logout()
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }
    }
}