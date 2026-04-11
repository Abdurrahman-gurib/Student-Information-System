using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        private string currentUsername;
        private string currentTheme = "Light";
        private string currentLanguage = "English";

        private readonly Dictionary<string, Dictionary<string, string>> translations = new()
        {
            ["French"] = new Dictionary<string, string>
            {
                ["University of Mauritius - Campus Portal"] = "Université de Maurice - Portail du campus",
                ["Logout"] = "Déconnexion",
                ["Settings"] = "Paramètres",
                ["Grades"] = "Notes",
                ["Courses"] = "Cours",
                ["Documents"] = "Documents",
                ["Profile"] = "Profil",
                ["Dashboard"] = "Tableau de bord",
                ["Settings Preferences"] = "Préférences",
                ["System Preferences"] = "Préférences système",
                ["Theme:"] = "Thème :",
                ["Language:"] = "Langue :",
                ["Make profile visible to other students"] = "Rendre le profil visible aux autres étudiants",
                ["Allow activity tracking for analytics"] = "Autoriser le suivi des activités pour l'analyse",
                ["Change Password"] = "Changer le mot de passe",
                ["Notification Preferences"] = "Préférences de notification",
                ["Email notifications for important updates"] = "Notifications par email pour les mises à jour importantes",
                ["Grade release notifications"] = "Notifications de publication des notes",
                ["Course enrollment and schedule changes"] = "Inscriptions et modifications d'emploi du temps",
                ["Document approval notifications"] = "Notifications d'approbation de document",
                ["For your security, change your password every 3 months to avoid compromise."] = "Pour votre sécurité, changez votre mot de passe tous les 3 mois pour éviter toute compromission.",
                ["Privacy & Security"] = "Confidentialité et sécurité",
                ["Profile Settings"] = "Paramètres du profil",
                ["Profile Picture:"] = "Photo de profil :",
                ["Display Name:"] = "Nom affiché :",
                ["Bio:"] = "Bio :",
                ["Settings saved successfully!"] = "Paramètres enregistrés avec succès !",
                ["Dashboard Overview"] = "Vue d'ensemble",
                ["Welcome"] = "Bienvenue",
                ["Overview"] = "Aperçu",
                ["My Courses"] = "Mes cours",
                ["Available Courses"] = "Cours disponibles",
                ["Quick Actions"] = "Actions rapides",
                ["Recent Activity"] = "Activité récente",
                ["View Profile"] = "Voir le profil",
                ["Browse Courses"] = "Parcourir les cours",
                ["View Grades"] = "Voir les notes",
                ["Upload Document"] = "Téléverser un document",
                ["Actions"] = "Actions",
                ["Reset"] = "Réinitialiser",
                ["Personal Information"] = "Informations personnelles",
                ["First Name:"] = "Prénom :",
                ["Last Name:"] = "Nom :",
                ["Email:"] = "Email :",
                ["Phone:"] = "Téléphone :",
                ["Address:"] = "Adresse :",
                ["Student Profile"] = "Profil étudiant",
                ["Profile Summary"] = "Résumé du profil",
                ["Your Documents"] = "Vos documents",
                ["Upload New Document"] = "Téléverser un nouveau document",
                ["Document Type:"] = "Type de document :",
                ["Selected File:"] = "Fichier sélectionné :",
                ["Choose File"] = "Choisir un fichier",
                ["Upload"] = "Téléverser",
                ["No documents uploaded yet."] = "Aucun document téléversé pour le moment.",
                ["Document Summary"] = "Résumé des documents",
                ["Course Management"] = "Gestion des cours",
                ["Document Management"] = "Gestion des documents",
                ["Course Summary"] = "Résumé du cours",
                ["Grades Academic Performance"] = "Performances académiques",
                ["Grades Performance"] = "Performances des notes",
                ["Grade History"] = "Historique des notes",
                ["Filter Grades"] = "Filtrer les notes",
                ["Semester:"] = "Semestre :",
                ["All Semesters"] = "Tous les semestres",
                ["Apply"] = "Appliquer",
                ["Academic Overview"] = "Vue académique",
                ["Overall GPA"] = "Moyenne générale",
                ["Total Grades"] = "Total des notes",
                ["Distribution"] = "Distribution",
                ["Back"] = "Retour",
                ["Filter Results"] = "Résultats du filtre",
                ["Filtered Grade History"] = "Historique des notes filtrées",
                ["Language changed to {0}. Application restart recommended for full effect."] = "Langue changée en {0}. Redémarrage de l'application recommandé pour plein effet.",
                ["Language Changed"] = "Langue changée",
                ["Please choose a file to upload."] = "Veuillez choisir un fichier à téléverser.",
                ["Validation Error"] = "Erreur de validation",
                ["Document uploaded successfully!"] = "Document téléversé avec succès !",
                ["Success"] = "Succès",
                ["Failed to upload document."] = "Échec du téléversement du document.",
                ["Error"] = "Erreur"
            },
            ["Spanish"] = new Dictionary<string, string>
            {
                ["University of Mauritius - Campus Portal"] = "Universidad de Mauricio - Portal del Campus",
                ["Logout"] = "Cerrar sesión",
                ["Settings"] = "Configuración",
                ["Grades"] = "Calificaciones",
                ["Courses"] = "Cursos",
                ["Documents"] = "Documentos",
                ["Profile"] = "Perfil",
                ["Dashboard"] = "Panel",
                ["Settings Preferences"] = "Preferencias",
                ["System Preferences"] = "Preferencias del sistema",
                ["Theme:"] = "Tema:",
                ["Language:"] = "Idioma:",
                ["Make profile visible to other students"] = "Hacer el perfil visible para otros estudiantes",
                ["Allow activity tracking for analytics"] = "Permitir seguimiento de actividad para análisis",
                ["Change Password"] = "Cambiar contraseña",
                ["Notification Preferences"] = "Preferencias de notificación",
                ["Email notifications for important updates"] = "Notificaciones por correo para actualizaciones importantes",
                ["Grade release notifications"] = "Notificaciones de publicación de calificaciones",
                ["Course enrollment and schedule changes"] = "Inscripción en cursos y cambios de horario",
                ["Document approval notifications"] = "Notificaciones de aprobación de documentos",
                ["For your security, change your password every 3 months to avoid compromise."] = "Por su seguridad, cambie su contraseña cada 3 meses para evitar compromisos.",
                ["Privacy & Security"] = "Privacidad y seguridad",
                ["Profile Settings"] = "Configuración de perfil",
                ["Profile Picture:"] = "Foto de perfil:",
                ["Display Name:"] = "Nombre para mostrar:",
                ["Bio:"] = "Biografía:",
                ["Settings saved successfully!"] = "¡Configuración guardada con éxito!",
                ["Dashboard Overview"] = "Resumen del panel",
                ["Welcome"] = "Bienvenido",
                ["Overview"] = "Resumen",
                ["My Courses"] = "Mis cursos",
                ["Available Courses"] = "Cursos disponibles",
                ["Quick Actions"] = "Acciones rápidas",
                ["Recent Activity"] = "Actividad reciente",
                ["View Profile"] = "Ver perfil",
                ["Browse Courses"] = "Ver cursos",
                ["View Grades"] = "Ver calificaciones",
                ["Upload Document"] = "Subir documento",
                ["Actions"] = "Acciones",
                ["Reset"] = "Restablecer",
                ["Personal Information"] = "Información personal",
                ["First Name:"] = "Nombre:",
                ["Last Name:"] = "Apellido:",
                ["Email:"] = "Correo electrónico:",
                ["Phone:"] = "Teléfono:",
                ["Address:"] = "Dirección:",
                ["Student Profile"] = "Perfil del estudiante",
                ["Profile Summary"] = "Resumen del perfil",
                ["Your Documents"] = "Tus documentos",
                ["Upload New Document"] = "Subir nuevo documento",
                ["Document Type:"] = "Tipo de documento:",
                ["Selected File:"] = "Archivo seleccionado:",
                ["Choose File"] = "Elegir archivo",
                ["Upload"] = "Subir",
                ["No documents uploaded yet."] = "Aún no se han subido documentos.",
                ["Document Summary"] = "Resumen de documentos",
                ["Course Management"] = "Gestión de cursos",
                ["Document Management"] = "Gestión de documentos",
                ["Course Summary"] = "Resumen del curso",
                ["Grades Academic Performance"] = "Rendimiento académico",
                ["Grades Performance"] = "Rendimiento de calificaciones",
                ["Grade History"] = "Historial de calificaciones",
                ["Filter Grades"] = "Filtrar calificaciones",
                ["Semester:"] = "Semestre:",
                ["All Semesters"] = "Todos los semestres",
                ["Apply"] = "Aplicar",
                ["Academic Overview"] = "Resumen académico",
                ["Overall GPA"] = "Promedio general",
                ["Total Grades"] = "Total de calificaciones",
                ["Distribution"] = "Distribución",
                ["Back"] = "Atrás",
                ["Filter Results"] = "Resultados del filtro",
                ["Filtered Grade History"] = "Historial de calificaciones filtrado",
                ["Language changed to {0}. Application restart recommended for full effect."] = "Idioma cambiado a {0}. Se recomienda reiniciar la aplicación para un efecto completo.",
                ["Language Changed"] = "Idioma cambiado",
                ["Please choose a file to upload."] = "Por favor selecciona un archivo para subir.",
                ["Validation Error"] = "Error de validación",
                ["Document uploaded successfully!"] = "¡Documento subido con éxito!",
                ["Success"] = "Éxito",
                ["Failed to upload document."] = "Error al subir el documento.",
                ["Error"] = "Error"
            }
        };

        private const int SidebarExpandedWidth = 245;
        private const int SidebarCollapsedWidth = 70;
        private const int HeaderHeight = 72;
        private const int PageMargin = 24;
        private const int SectionSpacing = 18;

        public DashboardForm(string username = "")
        {
            currentUsername = username;
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
            Text = T("University of Mauritius - Campus Portal");
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

            AddNavButton(navPanel, "🚪", "Logout", Logout);
            AddNavButton(navPanel, "⚙️", "Settings", ShowSettings);
            AddNavButton(navPanel, "📊", "Grades", ShowGrades);
            AddNavButton(navPanel, "📚", "Courses", ShowCourses);
            AddNavButton(navPanel, "📄", "Documents", ShowDocuments);
            AddNavButton(navPanel, "👤", "Profile", ShowProfile);
            AddNavButton(navPanel, "🏠", "Dashboard", ShowDashboard, true);

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
                Text = T("Dashboard"),
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

        private void AddNavButton(Control parent, string icon, string key, Action action, bool active = false)
        {
            var navItem = new NavItem { Icon = icon, Key = key };
            Button btn = new Button
            {
                Text = sidebarCollapsed ? icon : $"{icon} {T(key)}",
                Tag = navItem,
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

        private void UpdateNavigationLabels()
        {
            UpdateNavigationLabelsRecursive(sidebar);
        }

        private void UpdateNavigationLabelsRecursive(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is Button btn && btn.Tag is NavItem navInfo)
                {
                    btn.Text = sidebarCollapsed ? navInfo.Icon : $"{navInfo.Icon} {T(navInfo.Key)}";
                }

                if (control.HasChildren)
                    UpdateNavigationLabelsRecursive(control);
            }
        }

        private string T(string key, params object[] args)
            => string.Format(T(key), args);

        private string T(string key)
        {
            if (currentLanguage == "English")
                return key;

            if (translations.TryGetValue(currentLanguage, out var languageMap) && languageMap.TryGetValue(key, out var translation))
                return translation;

            return key;
        }

        private class NavItem
        {
            public string Icon { get; set; }
            public string Key { get; set; }
        }

        private void HighlightNavRecursive(Control parent, Button activeButton)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Button b && b.Tag is NavItem)
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
                if (control is Button btn && btn.Tag is NavItem navInfo)
                {
                    btn.Text = sidebarCollapsed ? navInfo.Icon : $"{navInfo.Icon} {T(navInfo.Key)}";
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
                Height = 190,
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

            Label lblSize = new Label
            {
                Text = FormatFileSize(doc.FileSize),
                Location = new Point(70, 102),
                AutoSize = true,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(140, 140, 140)
            };

            Label lblDate = new Label
            {
                Text = $"📅 {doc.UploadDate:dd/MM/yyyy HH:mm}",
                Location = new Point(70, 112),
                AutoSize = true,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(140, 140, 140)
            };

            Button btnView = CreateSmallButton("View", 14, 150, 70, Color.FromArgb(33, 150, 243));
            btnView.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(doc.FilePath) || !File.Exists(doc.FilePath))
                {
                    MessageBox.Show("The document file is not available.", "File Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Process.Start(new ProcessStartInfo(doc.FilePath) { UseShellExecute = true });
            };

            Button btnDownload = CreateSmallButton("Download", 92, 150, 84, Color.FromArgb(76, 175, 80));
            btnDownload.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(doc.FilePath) || !File.Exists(doc.FilePath))
                {
                    MessageBox.Show("The document file is not available to download.", "File Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                using var sfd = new SaveFileDialog
                {
                    FileName = doc.FileName,
                    Filter = "All files (*.*)|*.*"
                };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.Copy(doc.FilePath, sfd.FileName, true);
                    MessageBox.Show("Document downloaded successfully.", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            Button btnDelete = CreateSmallButton("Delete", 184, 150, 70, Color.FromArgb(244, 67, 54));
            btnDelete.Click += (s, e) =>
            {
                if (MessageBox.Show($"Delete {doc.FileName}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (DataAccess.DeleteDocument(doc.DocumentId))
                    {
                        if (!string.IsNullOrWhiteSpace(doc.FilePath) && File.Exists(doc.FilePath))
                            File.Delete(doc.FilePath);
                        SetCurrentPage(ShowDocuments);
                    }
                }
            };

            card.Controls.Add(btnDelete);
            card.Controls.Add(btnDownload);
            card.Controls.Add(btnView);
            card.Controls.Add(lblDate);
            card.Controls.Add(lblSize);
            card.Controls.Add(lblFile);
            card.Controls.Add(lblType);
            card.Controls.Add(lblIcon);

            return card;
        }

        private string FormatFileSize(long sizeInBytes)
        {
            if (sizeInBytes >= 1_000_000)
                return $"{sizeInBytes / 1_000_000.0:F1} MB";
            if (sizeInBytes >= 1_000)
                return $"{sizeInBytes / 1_000.0:F1} KB";
            return $"{sizeInBytes} bytes";
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
            lblHeaderTitle.Text = T("Dashboard");
            FlowLayoutPanel page = CreatePage(T("Dashboard Overview"));

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

            Panel enrolledBody = AddSection(page, T("My Courses"), 525);
            FlowLayoutPanel enrolledFlow = CreateWrapFlow(enrolledBody);
            foreach (var course in enrolledCourses)
                enrolledFlow.Controls.Add(CreateCourseCard(course, true));

            Panel availableBody = AddSection(page, T("Available Courses"), 340);
            FlowLayoutPanel availableFlow = CreateWrapFlow(availableBody);
            foreach (var course in availableCourses)
                availableFlow.Controls.Add(CreateCourseCard(course, false));

            Panel quickBody = AddSection(page, T("Quick Actions"), 140);
            FlowLayoutPanel quickFlow = CreateWrapFlow(quickBody);
            quickFlow.Controls.Add(CreatePrimaryButton(T("View Profile"), ShowProfile));
            quickFlow.Controls.Add(CreatePrimaryButton(T("Browse Courses"), ShowCourses));
            quickFlow.Controls.Add(CreatePrimaryButton(T("View Grades"), ShowGrades));
            quickFlow.Controls.Add(CreatePrimaryButton(T("Upload Document"), ShowDocuments));

            Panel activityBody = AddSection(page, T("Recent Activity"), 200);
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
            lblHeaderTitle.Text = T("Student Profile");
            FlowLayoutPanel page = CreatePage(T("Student Profile"));

            Panel actionBody = AddSection(page, T("Actions"), 150);
            FlowLayoutPanel actionFlow = CreateWrapFlow(actionBody);
            actionFlow.Controls.Add(CreatePrimaryButton(T("Save Changes"), () =>
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
                Text = T("Reset"),
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

            Panel infoBody = AddSection(page, T("Personal Information"), 340);
            TableLayoutPanel form = CreateFormTable(infoBody, 28, 72);

            AddFormRow(form, T("First Name:"), CreateBoundTextBox(currentStudent.FirstName, v => currentStudent.FirstName = v));
            AddFormRow(form, T("Last Name:"), CreateBoundTextBox(currentStudent.LastName, v => currentStudent.LastName = v));
            AddFormRow(form, T("Email:"), CreateBoundTextBox(currentStudent.Email, v => currentStudent.Email = v));
            AddFormRow(form, T("Phone:"), CreateBoundTextBox(currentStudent.Phone, v => currentStudent.Phone = v));
            AddFormRow(form, T("Address:"), CreateBoundTextBox(currentStudent.Address, v => currentStudent.Address = v));

            DateTimePicker dtpDob = new DateTimePicker
            {
                Value = currentStudent.DateOfBirth ?? DateTime.Today,
                Width = 280,
                Font = new Font("Segoe UI", 10)
            };
            dtpDob.ValueChanged += (s, e) => currentStudent.DateOfBirth = dtpDob.Value;
            AddFormRow(form, "Date of Birth:", dtpDob);

            Panel summaryBody = AddSection(page, T("Profile Summary"), 230);

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
                Location = new Point(125, 60),
                AutoSize = true,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(95, 95, 95)
            });

            summaryBody.Controls.Add(summary);
        }

        private void ShowDocuments()
        {
            lblHeaderTitle.Text = T("Document Management");
            FlowLayoutPanel page = CreatePage(T("Document Management"));

            var docs = DataAccess.GetStudentDocuments(currentStudent.StudentId);

            Panel docsBody = AddSection(page, T("Your Documents"), 320);
            docsBody.AutoScroll = true;
            FlowLayoutPanel docsFlow = CreateWrapFlow(docsBody);
            docsFlow.AutoScroll = true;
            docsFlow.FlowDirection = FlowDirection.LeftToRight;
            docsFlow.WrapContents = true;
            docsFlow.Padding = new Padding(4);
            foreach (var doc in docs.OrderByDescending(d => d.UploadDate))
                docsFlow.Controls.Add(CreateDocumentCard(doc));

            Panel uploadBody = AddSection(page, T("Upload New Document"), 300);
            TableLayoutPanel uploadTable = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 4,
                RowCount = 1,
                AutoSize = true
            };
            uploadTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18));
            uploadTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
            uploadTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18));
            uploadTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
            uploadTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            uploadBody.Controls.Add(uploadTable);

            uploadTable.Controls.Add(new Label
            {
                Text = T("Document Type:"),
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
                Text = T("Selected File:"),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Anchor = AnchorStyles.Left,
                AutoSize = true
            }, 2, 0);

            TextBox txtSelectedFile = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BackColor = Color.White
            };
            uploadTable.Controls.Add(txtSelectedFile, 3, 0);

            FlowLayoutPanel buttonRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 50,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0, 12, 0, 0)
            };

            buttonRow.Controls.Add(CreatePrimaryButton(T("Choose File"), () =>
            {
                using var ofd = new OpenFileDialog
                {
                    Filter = "All files (*.*)|*.*",
                    Title = "Select document to upload"
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                    txtSelectedFile.Text = ofd.FileName;
            }));

            buttonRow.Controls.Add(CreatePrimaryButton(T("Upload"), () =>
            {
                if (string.IsNullOrWhiteSpace(txtSelectedFile.Text) || !File.Exists(txtSelectedFile.Text))
                {
                    MessageBox.Show("Please choose a file to upload.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string storageFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StudentInfoApp3", "Documents");
                Directory.CreateDirectory(storageFolder);
                string destinationPath = Path.Combine(storageFolder, $"{Guid.NewGuid()}{Path.GetExtension(txtSelectedFile.Text)}");

                File.Copy(txtSelectedFile.Text, destinationPath, true);

                var doc = new Document
                {
                    StudentId = currentStudent.StudentId,
                    DocumentType = cbType.Text,
                    FileName = Path.GetFileName(txtSelectedFile.Text),
                    FilePath = destinationPath,
                    FileSize = new FileInfo(destinationPath).Length
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

            Panel summaryBody = AddSection(page, T("Document Summary"), 300);
            TableLayoutPanel summaryTable = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 4,
                RowCount = 1,
                AutoSize = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };
            summaryTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            summaryTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            summaryTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            summaryTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            summaryTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            void AddSummaryCell(string text, Font font, int column, int row, ContentAlignment align = ContentAlignment.MiddleLeft)
            {
                var label = new Label
                {
                    Text = text,
                    Font = font,
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                    TextAlign = align,
                    Margin = new Padding(6)
                };
                summaryTable.Controls.Add(label, column, row);
            }

            AddSummaryCell("File Name", new Font("Segoe UI", 10, FontStyle.Bold), 0, 0);
            AddSummaryCell("Type", new Font("Segoe UI", 10, FontStyle.Bold), 1, 0);
            AddSummaryCell("Size", new Font("Segoe UI", 10, FontStyle.Bold), 2, 0);
            AddSummaryCell("Uploaded", new Font("Segoe UI", 10, FontStyle.Bold), 3, 0);

            if (docs.Any())
            {
                int row = 1;
                foreach (var doc in docs.OrderByDescending(d => d.UploadDate))
                {
                    summaryTable.RowCount++;
                    summaryTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    AddSummaryCell(doc.FileName, new Font("Segoe UI", 10), 0, row);
                    AddSummaryCell(doc.DocumentType, new Font("Segoe UI", 10), 1, row);
                    AddSummaryCell(FormatFileSize(doc.FileSize), new Font("Segoe UI", 10), 2, row);
                    AddSummaryCell(doc.UploadDate.ToString("dd/MM/yyyy HH:mm"), new Font("Segoe UI", 10), 3, row);
                    row++;
                }
            }
            else
            {
                summaryTable.Controls.Clear();
                summaryTable.ColumnCount = 1;
                summaryTable.RowCount = 1;
                summaryTable.ColumnStyles.Clear();
                summaryTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                summaryTable.RowStyles.Clear();
                summaryTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                summaryTable.Controls.Add(new Label
                {
                    Text = "No documents uploaded yet.",
                    Font = new Font("Segoe UI", 10),
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Margin = new Padding(6)
                }, 0, 0);
            }

            summaryBody.Controls.Add(summaryTable);
        }

        private void ShowCourses()
        {
            lblHeaderTitle.Text = T("Course Management");
            FlowLayoutPanel page = CreatePage(T("Course Management"));

            var enrolledCourses = DataAccess.GetEnrolledCourses(currentStudent.StudentId);
            var totalCredits = enrolledCourses.Sum(c => c.Credits);
            var allCourses = DataAccess.GetCourses();
            var enrolledIds = enrolledCourses.Select(c => c.CourseId).ToHashSet();
            var availableCourses = allCourses.Where(c => !enrolledIds.Contains(c.CourseId)).ToList();

            Panel enrolledBody = AddSection(page, T("My Courses"), 280);
            FlowLayoutPanel enrolledFlow = CreateWrapFlow(enrolledBody);
            foreach (var course in enrolledCourses)
                enrolledFlow.Controls.Add(CreateCourseCard(course, true));

            Panel availableBody = AddSection(page, T("Available Courses"), 280);
            FlowLayoutPanel availableFlow = CreateWrapFlow(availableBody);
            foreach (var course in availableCourses)
                availableFlow.Controls.Add(CreateCourseCard(course, false));

            Panel summaryBody = AddSection(page, T("Course Summary"), 160);
            summaryBody.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Text = $"Total Credits: {totalCredits} | GPA: {DataAccess.GetStudentGPA(currentStudent.StudentId):F2}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            });
        }

        private void ShowGrades()
        {
            lblHeaderTitle.Text = T("Grades Academic Performance");
            FlowLayoutPanel page = CreatePage(T("Grades Performance"));

            var grades = DataAccess.GetStudentGrades(currentStudent.StudentId);
            double gpa = grades.Any() ? grades.Average(g => GradeToPoint(g.GradeValue)) : 0.0;
            var gradeCounts = grades.GroupBy(g => g.GradeValue)
                                    .Select(g => $"{g.Key}:{g.Count()}")
                                    .OrderBy(x => x)
                                    .ToList();

            Panel gradesBody = AddSection(page, T("Grade History"), 250);
            FlowLayoutPanel gradesFlow = CreateWrapFlow(gradesBody);
            foreach (var grade in grades.OrderByDescending(g => g.Year).ThenByDescending(g => g.Semester))
                gradesFlow.Controls.Add(CreateGradeCard(grade));

            Panel filterBody = AddSection(page, T("Filter Grades"), 185);
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
                Text = T("Semester:"),
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
            cbSemester.Items.Add(T("All Semesters"));
            foreach (var semester in grades.Select(g => $"{g.Semester} {g.Year}").Distinct())
                cbSemester.Items.Add(semester);
            cbSemester.SelectedIndex = 0;
            filterTable.Controls.Add(cbSemester, 1, 0);

            Button btnApply = CreatePrimaryButton(T("Apply"), () =>
            {
                if (cbSemester.Text == T("All Semesters"))
                {
                    SetCurrentPage(ShowGrades);
                    return;
                }

                ShowGradesFiltered(cbSemester.Text);
            });
            btnApply.Width = 110;
            filterTable.Controls.Add(btnApply, 2, 0);

            Panel overviewBody = AddSection(page, T("Academic Overview"), 255);
            FlowLayoutPanel overviewFlow = CreateWrapFlow(overviewBody);
            overviewFlow.Controls.Add(CreateStatCard("Overall GPA", $"{gpa:F2}", "Current academic standing", GetGpaColor(gpa)));
            overviewFlow.Controls.Add(CreateStatCard("Total Grades", $"{grades.Count}", "Recorded results", Color.FromArgb(33, 150, 243)));
            overviewFlow.Controls.Add(CreateStatCard("Distribution", string.Join(", ", gradeCounts), "Grade distribution", Color.FromArgb(255, 152, 0)));
        }

        private void ShowGradesFiltered(string semesterText)
        {
            lblHeaderTitle.Text = T("Grades Academic Performance");
            FlowLayoutPanel page = CreatePage(T("Grades Performance") + $" - {semesterText}");

            Panel actionBody = AddSection(page, T("Filter Results"), 140);
            FlowLayoutPanel actionFlow = CreateWrapFlow(actionBody, false);
            actionFlow.WrapContents = false;
            actionFlow.AutoSize = true;
            actionFlow.Controls.Add(CreatePrimaryButton(T("Back"), () => SetCurrentPage(ShowGrades)));
            actionFlow.Controls.Add(new Label
            {
                Text = $"Showing grades for {semesterText}",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(90, 90, 90),
                Margin = new Padding(12, 12, 0, 0)
            });

            var filtered = DataAccess.GetStudentGrades(currentStudent.StudentId)
                                     .Where(g => $"{g.Semester} {g.Year}" == semesterText)
                                     .ToList();

            Panel body = AddSection(page, T("Filtered Grade History"), 260);
            FlowLayoutPanel flow = CreateWrapFlow(body);

            foreach (var grade in filtered)
                flow.Controls.Add(CreateGradeCard(grade));
        }

        private void ShowSettings()
        {
            lblHeaderTitle.Text = T("Settings Preferences");
            FlowLayoutPanel page = CreatePage(T("Settings Preferences"));

            Panel systemBody = AddSection(page, T("System Preferences"), 160);
            TableLayoutPanel systemTable = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 4,
                RowCount = 2,
                AutoSize = true
            };
            systemTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18));
            systemTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
            systemTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18));
            systemTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
            systemTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            systemTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            systemBody.Controls.Add(systemTable);

            systemTable.Controls.Add(new Label
            {
                Text = T("Theme:"),
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
            cbTheme.SelectedItem = currentTheme;
            cbTheme.SelectedIndexChanged += (s, e) =>
            {
                currentTheme = cbTheme.SelectedItem.ToString();
                ApplyTheme(currentTheme);
            };
            systemTable.Controls.Add(cbTheme, 1, 0);

            systemTable.Controls.Add(new Label
            {
                Text = T("Language:"),
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
            cbLanguage.SelectedItem = currentLanguage;
            cbLanguage.SelectedIndexChanged += (s, e) =>
            {
                currentLanguage = cbLanguage.SelectedItem.ToString();
                ApplyLanguage(currentLanguage);
            };
            systemTable.Controls.Add(cbLanguage, 3, 0);

            Panel privacyBody = AddSection(page, T("Privacy & Security"), 280);
            FlowLayoutPanel privacyFlow = CreateWrapFlow(privacyBody, true);

            privacyFlow.Controls.Add(new CheckBox
            {
                Text = T("Make profile visible to other students"),
                Font = new Font("Segoe UI", 10),
                Checked = true,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            });

            privacyFlow.Controls.Add(new CheckBox
            {
                Text = T("Allow activity tracking for analytics"),
                Font = new Font("Segoe UI", 10),
                Checked = false,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            });

            privacyFlow.Controls.Add(new Label
            {
                Text = T("For your security, change your password every 3 months to avoid compromise."),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Italic),
                ForeColor = Color.FromArgb(110, 110, 110),
                AutoSize = true,
                MaximumSize = new Size(520, 0),
                Margin = new Padding(0, 0, 0, 10)
            });

            privacyFlow.Controls.Add(CreatePrimaryButton(T("Change Password"), () =>
            {
                ShowChangePasswordDialog();
            }));

            Panel notificationBody = AddSection(page, T("Notification Preferences"), 245);
            FlowLayoutPanel notificationFlow = CreateWrapFlow(notificationBody, true);

            notificationFlow.Controls.Add(new CheckBox
            {
                Text = T("Email notifications for important updates"),
                Font = new Font("Segoe UI", 10),
                Checked = true,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            });

            notificationFlow.Controls.Add(new CheckBox
            {
                Text = T("Grade release notifications"),
                Font = new Font("Segoe UI", 10),
                Checked = true,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            });

            notificationFlow.Controls.Add(new CheckBox
            {
                Text = T("Course enrollment and schedule changes"),
                Font = new Font("Segoe UI", 10),
                Checked = true,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            });

            notificationFlow.Controls.Add(new CheckBox
            {
                Text = T("Document approval notifications"),
                Font = new Font("Segoe UI", 10),
                Checked = false,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            });

            Panel profileBody = AddSection(page, T("Profile Settings"), 210);
            TableLayoutPanel profileTable = CreateFormTable(profileBody, 25, 75);

            Button btnPicture = new Button
            {
                Text = T("Change Picture"),
                Width = 150,
                Height = 34,
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };
            btnPicture.FlatAppearance.BorderSize = 0;
            btnPicture.Click += (s, e) => MessageBox.Show("Profile picture upload feature coming soon!");

            AddFormRow(profileTable, T("Profile Picture:"), btnPicture);

            TextBox txtDisplayName = new TextBox
            {
                Text = $"{currentStudent.FirstName} {currentStudent.LastName}",
                Width = 420,
                Font = new Font("Segoe UI", 10)
            };
            AddFormRow(profileTable, T("Display Name:"), txtDisplayName);

            TextBox txtBio = new TextBox
            {
                Text = T("Computer Science student passionate about technology and innovation."),
                Width = 420,
                Height = 70,
                Multiline = true,
                Font = new Font("Segoe UI", 10)
            };
            AddFormRow(profileTable, T("Bio:"), txtBio);

            Panel actionBody = AddSection(page, T("Actions"), 150);
            FlowLayoutPanel actionFlow = CreateWrapFlow(actionBody);
            actionFlow.Controls.Add(CreatePrimaryButton(T("Save All Settings"), () =>
            {
                MessageBox.Show(T("Settings saved successfully!"), T("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }));
        }

        private void ShowChangePasswordDialog()
        {
            Form changePasswordForm = new Form
            {
                Text = "Change Password",
                Width = 420,
                Height = 350,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblOld = new Label
            {
                Text = "Current Password:",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            changePasswordForm.Controls.Add(lblOld);

            TextBox txtOldPassword = new TextBox
            {
                Location = new Point(20, 50),
                Width = 360,
                PasswordChar = '*',
                Font = new Font("Segoe UI", 10)
            };
            changePasswordForm.Controls.Add(txtOldPassword);

            Label lblNew = new Label
            {
                Text = "New Password:",
                Location = new Point(20, 90),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            changePasswordForm.Controls.Add(lblNew);

            TextBox txtNewPassword = new TextBox
            {
                Location = new Point(20, 120),
                Width = 360,
                PasswordChar = '*',
                Font = new Font("Segoe UI", 10)
            };
            changePasswordForm.Controls.Add(txtNewPassword);

            Label lblConfirm = new Label
            {
                Text = "Confirm New Password:",
                Location = new Point(20, 160),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            changePasswordForm.Controls.Add(lblConfirm);

            TextBox txtConfirmPassword = new TextBox
            {
                Location = new Point(20, 190),
                Width = 360,
                PasswordChar = '*',
                Font = new Font("Segoe UI", 10)
            };
            changePasswordForm.Controls.Add(txtConfirmPassword);

            Button btnChange = new Button
            {
                Text = "Change Password",
                Location = new Point(140, 240),
                Width = 130,
                Height = 34,
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnChange.FlatAppearance.BorderSize = 0;
            btnChange.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtOldPassword.Text))
                {
                    MessageBox.Show("Please enter your current password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNewPassword.Text) || txtNewPassword.Text.Length < 6)
                {
                    MessageBox.Show("New password must be at least 6 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txtNewPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("New passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (DataAccess.ChangePassword(currentUsername, txtOldPassword.Text, txtNewPassword.Text))
                {
                    MessageBox.Show("Password changed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    changePasswordForm.Close();
                }
                else
                {
                    MessageBox.Show("Current password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            changePasswordForm.Controls.Add(btnChange);

            changePasswordForm.ShowDialog();
        }

        private void ApplyTheme(string theme)
        {
            Color bgColor, headerColor, sidebarColor, textColor;

            switch (theme)
            {
                case "Dark":
                    bgColor = Color.FromArgb(30, 30, 30);
                    headerColor = Color.FromArgb(20, 20, 20);
                    sidebarColor = Color.FromArgb(15, 15, 15);
                    textColor = Color.White;
                    break;
                case "Auto":
                    bgColor = Color.FromArgb(240, 242, 245);
                    headerColor = Color.FromArgb(33, 150, 243);
                    sidebarColor = Color.FromArgb(24, 55, 120);
                    textColor = Color.Black;
                    break;
                default:
                    bgColor = Color.FromArgb(240, 242, 245);
                    headerColor = Color.FromArgb(33, 150, 243);
                    sidebarColor = Color.FromArgb(24, 55, 120);
                    textColor = Color.Black;
                    break;
            }

            contentPanel.BackColor = bgColor;
            SetCurrentPage(currentPageAction);
        }

        private void ApplyLanguage(string language)
        {
            currentLanguage = language;
            Text = T("University of Mauritius - Campus Portal");
            UpdateNavigationLabels();
            MessageBox.Show(T("Language changed to {0}. Application restart recommended for full effect.", language), T("Language Changed"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            SetCurrentPage(currentPageAction);
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