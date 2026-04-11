using System;
using Microsoft.Data.Sqlite;
using System.IO;

namespace StudentInfoApp3.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            using (var conn = new SqliteConnection(DataAccess.ConnectionString))
            {
                conn.Open();
                
                // Enable foreign keys
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "PRAGMA foreign_keys = ON;";
                    cmd.ExecuteNonQuery();
                }

                // Create tables if they don't exist
                string[] createTableScripts = new string[]
                {
                    // Users table
                    @"CREATE TABLE IF NOT EXISTS Users (
                        UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL UNIQUE,
                        PasswordHash TEXT NOT NULL
                    );",
                    
                    // Students table
                    @"CREATE TABLE IF NOT EXISTS Students (
                        StudentId INTEGER PRIMARY KEY AUTOINCREMENT,
                        FirstName TEXT NOT NULL,
                        LastName TEXT NOT NULL,
                        Email TEXT,
                        DateOfBirth DATE,
                        Phone TEXT,
                        Address TEXT
                    );",
                    
                    // Courses table
                    @"CREATE TABLE IF NOT EXISTS Courses (
                        CourseId INTEGER PRIMARY KEY AUTOINCREMENT,
                        Code TEXT NOT NULL UNIQUE,
                        Title TEXT NOT NULL,
                        Credits INTEGER NOT NULL,
                        Description TEXT
                    );",
                    
                    // Enrollments table
                    @"CREATE TABLE IF NOT EXISTS Enrollments (
                        EnrollmentId INTEGER PRIMARY KEY AUTOINCREMENT,
                        StudentId INTEGER NOT NULL,
                        CourseId INTEGER NOT NULL,
                        EnrolledOn DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (StudentId) REFERENCES Students(StudentId) ON DELETE CASCADE,
                        FOREIGN KEY (CourseId) REFERENCES Courses(CourseId) ON DELETE CASCADE
                    );",

                    // Grades table
                    @"CREATE TABLE IF NOT EXISTS Grades (
                        GradeId INTEGER PRIMARY KEY AUTOINCREMENT,
                        StudentId INTEGER NOT NULL,
                        CourseId INTEGER NOT NULL,
                        Grade TEXT NOT NULL,
                        Semester TEXT,
                        Year INTEGER,
                        FOREIGN KEY (StudentId) REFERENCES Students(StudentId) ON DELETE CASCADE,
                        FOREIGN KEY (CourseId) REFERENCES Courses(CourseId) ON DELETE CASCADE
                    );",

                    // Documents table
                    @"CREATE TABLE IF NOT EXISTS Documents (
                        DocumentId INTEGER PRIMARY KEY AUTOINCREMENT,
                        StudentId INTEGER NOT NULL,
                        DocumentType TEXT NOT NULL,
                        FileName TEXT NOT NULL,
                        FilePath TEXT,
                        FileSize INTEGER DEFAULT 0,
                        UploadDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (StudentId) REFERENCES Students(StudentId) ON DELETE CASCADE
                    );",

                    // Sessions table
                    @"CREATE TABLE IF NOT EXISTS Sessions (
                        SessionId INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId INTEGER,
                        LoginTime DATETIME DEFAULT CURRENT_TIMESTAMP,
                        LogoutTime DATETIME,
                        Duration INTEGER,
                        FOREIGN KEY (UserId) REFERENCES Users(UserId)
                    );",

                    // Issues table
                    @"CREATE TABLE IF NOT EXISTS Issues (
                        IssueId INTEGER PRIMARY KEY AUTOINCREMENT,
                        StudentId INTEGER NOT NULL,
                        IssueType TEXT NOT NULL,
                        Description TEXT,
                        ReportedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        Status TEXT DEFAULT 'Open',
                        FOREIGN KEY (StudentId) REFERENCES Students(StudentId) ON DELETE CASCADE
                    );"
                };

                foreach (string script in createTableScripts)
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = script;
                        cmd.ExecuteNonQuery();
                    }
                }

                // Ensure new document columns exist for older databases
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "PRAGMA table_info(Documents);";
                    using var rdr = cmd.ExecuteReader();
                    var columns = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    while (rdr.Read())
                        columns.Add(rdr.GetString(1));

                    if (!columns.Contains("FilePath"))
                    {
                        using var alter = conn.CreateCommand();
                        alter.CommandText = "ALTER TABLE Documents ADD COLUMN FilePath TEXT;";
                        alter.ExecuteNonQuery();
                    }

                    if (!columns.Contains("FileSize"))
                    {
                        using var alter = conn.CreateCommand();
                        alter.CommandText = "ALTER TABLE Documents ADD COLUMN FileSize INTEGER DEFAULT 0;";
                        alter.ExecuteNonQuery();
                    }
                }

                // Add dummy data if tables are empty
                AddDummyData(conn);
            }
        }

        private static void AddDummyData(SqliteConnection conn)
        {
            // Check if Users table is empty
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM Users";
                if ((long)cmd.ExecuteScalar() > 0) return; // Data already exists
            }

            // Helper function to hash passwords (same as DataAccess)
            var hashPassword = new Func<string, string>(password =>
            {
                using var sha = System.Security.Cryptography.SHA256.Create();
                var b = System.Text.Encoding.UTF8.GetBytes(password);
                var h = sha.ComputeHash(b);
                return Convert.ToBase64String(h);
            });

            // Add dummy users
            var users = new[] {
                ("admin", "password123"),
                ("teacher1", "teacher123"),
                ("student1", "student123")
            };

            foreach (var (username, password) in users)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Users (Username, PasswordHash) VALUES (@u, @p)";
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", hashPassword(password));
                    cmd.ExecuteNonQuery();
                }
            }

            // Add dummy students with more diverse data
            var students = new[]
            {
                ("John", "Smith", "john.smith@school.edu", new DateTime(2005, 3, 15), "+1-555-0101", "123 Main St, Springfield"),
                ("Sarah", "Johnson", "sarah.johnson@school.edu", new DateTime(2004, 7, 22), "+1-555-0102", "456 Oak Ave, Springfield"),
                ("Michael", "Williams", "michael.williams@school.edu", new DateTime(2005, 11, 8), "+1-555-0103", "789 Pine Rd, Springfield"),
                ("Emily", "Brown", "emily.brown@school.edu", new DateTime(2004, 5, 30), "+1-555-0104", "321 Elm St, Springfield"),
                ("David", "Martinez", "david.martinez@school.edu", new DateTime(2005, 9, 12), "+1-555-0105", "654 Maple Dr, Springfield"),
                ("Jessica", "Garcia", "jessica.garcia@school.edu", new DateTime(2004, 12, 3), "+1-555-0106", "987 Cedar Ln, Springfield"),
                ("Christopher", "Miller", "chris.miller@school.edu", new DateTime(2005, 6, 18), "+1-555-0107", "147 Birch St, Springfield"),
                ("Amanda", "Davis", "amanda.davis@school.edu", new DateTime(2004, 8, 25), "+1-555-0108", "258 Walnut Ave, Springfield"),
                ("Daniel", "Rodriguez", "daniel.rodriguez@school.edu", new DateTime(2005, 2, 14), "+1-555-0109", "369 Spruce St, Springfield"),
                ("Ashley", "Martinez", "ashley.martinez@school.edu", new DateTime(2004, 10, 7), "+1-555-0110", "741 Poplar Dr, Springfield")
            };

            foreach (var (firstName, lastName, email, dob, phone, address) in students)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Students (FirstName, LastName, Email, DateOfBirth, Phone, Address) VALUES (@f, @l, @e, @d, @p, @a)";
                    cmd.Parameters.AddWithValue("@f", firstName);
                    cmd.Parameters.AddWithValue("@l", lastName);
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.Parameters.AddWithValue("@d", dob);
                    cmd.Parameters.AddWithValue("@p", phone);
                    cmd.Parameters.AddWithValue("@a", address);
                    cmd.ExecuteNonQuery();
                }
            }

            // Add dummy courses with detailed descriptions
            var courses = new[]
            {
                ("CS101", "Introduction to Computer Science", 3, "Fundamental concepts of programming, algorithms, and problem-solving using Python. Covers variables, loops, functions, and basic data structures."),
                ("CS201", "Data Structures and Algorithms", 4, "Advanced programming concepts including arrays, linked lists, stacks, queues, trees, graphs, and algorithm analysis. Focus on efficiency and optimization."),
                ("MATH101", "Calculus I", 4, "Limits, derivatives, and integrals. Applications in physics, engineering, and economics. Includes differential equations and series."),
                ("ENG101", "English Composition", 3, "Academic writing skills, research methods, and critical thinking. Learn to write essays, reports, and research papers with proper citation."),
                ("PHYS101", "Physics I: Mechanics", 4, "Classical mechanics including kinematics, dynamics, energy, momentum, and rotational motion. Laboratory experiments included."),
                ("CHEM101", "Chemistry I", 3, "Atomic structure, chemical bonding, stoichiometry, and basic organic chemistry. Introduction to laboratory techniques and safety."),
                ("BIO101", "Biology I", 4, "Cell structure, genetics, evolution, and ecology. Includes laboratory work with microscopy and field studies."),
                ("HIST101", "World History", 3, "Major civilizations, cultural developments, and historical events from ancient times to the modern era."),
                ("ECON101", "Principles of Economics", 3, "Micro and macroeconomic principles, market structures, fiscal and monetary policy, and economic decision-making."),
                ("ART101", "Introduction to Art", 3, "Art history, techniques, and appreciation. Covers various mediums including painting, sculpture, and digital art."),
                ("PSY101", "Psychology I", 3, "Human behavior, cognition, learning theories, and personality. Includes research methods and psychological disorders."),
                ("SOC101", "Sociology I", 3, "Social structures, institutions, culture, and social change. Examines group dynamics and societal issues.")
            };

            foreach (var (code, title, credits, desc) in courses)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Courses (Code, Title, Credits, Description) VALUES (@c, @t, @cr, @d)";
                    cmd.Parameters.AddWithValue("@c", code);
                    cmd.Parameters.AddWithValue("@t", title);
                    cmd.Parameters.AddWithValue("@cr", credits);
                    cmd.Parameters.AddWithValue("@d", desc);
                    cmd.ExecuteNonQuery();
                }
            }

            // Add dummy enrollments (more comprehensive)
            var enrollments = new[]
            {
                // Student 1 enrollments
                (1, 1), (1, 2), (1, 3), (1, 4), (1, 5),
                // Student 2 enrollments
                (2, 1), (2, 4), (2, 5), (2, 6), (2, 7),
                // Student 3 enrollments
                (3, 2), (3, 3), (3, 6), (3, 8), (3, 9),
                // Student 4 enrollments
                (4, 1), (4, 4), (4, 5), (4, 7), (4, 10),
                // Student 5 enrollments
                (5, 2), (5, 5), (5, 6), (5, 8), (5, 11),
                // Student 6 enrollments
                (6, 1), (6, 3), (6, 6), (6, 9), (6, 12),
                // Student 7 enrollments
                (7, 2), (7, 4), (7, 7), (7, 8), (7, 10),
                // Student 8 enrollments
                (8, 1), (8, 5), (8, 8), (8, 11), (8, 12),
                // Student 9 enrollments
                (9, 3), (9, 6), (9, 7), (9, 9), (9, 10),
                // Student 10 enrollments
                (10, 2), (10, 4), (10, 5), (10, 8), (10, 11)
            };

            foreach (var (studentId, courseId) in enrollments)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Enrollments (StudentId, CourseId) VALUES (@s, @c)";
                    cmd.Parameters.AddWithValue("@s", studentId);
                    cmd.Parameters.AddWithValue("@c", courseId);
                    try { cmd.ExecuteNonQuery(); } catch { } // Ignore duplicates
                }
            }

            // Add comprehensive dummy grades
            var grades = new[]
            {
                // Student 1 grades
                (1, 1, "A", "Fall", 2023), (1, 2, "B+", "Spring", 2024), (1, 3, "A-", "Fall", 2023), (1, 4, "A", "Spring", 2024), (1, 5, "B", "Fall", 2023),
                // Student 2 grades
                (2, 1, "B", "Fall", 2023), (2, 4, "A", "Spring", 2024), (2, 5, "B+", "Fall", 2023), (2, 6, "A-", "Spring", 2024), (2, 7, "B", "Fall", 2023),
                // Student 3 grades
                (3, 2, "A", "Spring", 2024), (3, 3, "B", "Fall", 2023), (3, 6, "A-", "Spring", 2024), (3, 8, "B+", "Fall", 2023), (3, 9, "A", "Spring", 2024),
                // Student 4 grades
                (4, 1, "B+", "Fall", 2023), (4, 4, "A", "Spring", 2024), (4, 5, "B", "Fall", 2023), (4, 7, "A-", "Spring", 2024), (4, 10, "B+", "Fall", 2023),
                // Student 5 grades
                (5, 2, "A-", "Spring", 2024), (5, 5, "B+", "Fall", 2023), (5, 6, "A", "Spring", 2024), (5, 8, "B", "Fall", 2023), (5, 11, "A-", "Spring", 2024),
                // Student 6 grades
                (6, 1, "B", "Fall", 2023), (6, 3, "A", "Spring", 2024), (6, 6, "B+", "Fall", 2023), (6, 9, "A-", "Spring", 2024), (6, 12, "B", "Fall", 2023),
                // Student 7 grades
                (7, 2, "A", "Spring", 2024), (7, 4, "B+", "Fall", 2023), (7, 7, "A-", "Spring", 2024), (7, 8, "B", "Fall", 2023), (7, 10, "A", "Spring", 2024),
                // Student 8 grades
                (8, 1, "B+", "Fall", 2023), (8, 5, "A", "Spring", 2024), (8, 8, "B", "Fall", 2023), (8, 11, "A-", "Spring", 2024), (8, 12, "B+", "Fall", 2023),
                // Student 9 grades
                (9, 3, "A-", "Fall", 2023), (9, 6, "B+", "Spring", 2024), (9, 7, "A", "Fall", 2023), (9, 9, "B", "Spring", 2024), (9, 10, "A-", "Fall", 2023),
                // Student 10 grades
                (10, 2, "B", "Spring", 2024), (10, 4, "A", "Fall", 2023), (10, 5, "B+", "Spring", 2024), (10, 8, "A-", "Fall", 2023), (10, 11, "B", "Spring", 2024),
                // Additional historical grades for more depth
                (1, 1, "A-", "Fall", 2022), (1, 3, "B+", "Spring", 2023), (2, 1, "B", "Fall", 2022), (3, 2, "A", "Fall", 2022), (4, 4, "A", "Fall", 2022)
            };

            foreach (var (studentId, courseId, grade, semester, year) in grades)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Grades (StudentId, CourseId, Grade, Semester, Year) VALUES (@s, @c, @g, @sem, @y)";
                    cmd.Parameters.AddWithValue("@s", studentId);
                    cmd.Parameters.AddWithValue("@c", courseId);
                    cmd.Parameters.AddWithValue("@g", grade);
                    cmd.Parameters.AddWithValue("@sem", semester);
                    cmd.Parameters.AddWithValue("@y", year);
                    try { cmd.ExecuteNonQuery(); } catch { } // Ignore duplicates
                }
            }

            // Add comprehensive dummy documents
            var documents = new[]
            {
                // Student 1 documents
                (1, "ID Card", "john_smith_id_card.pdf"),
                (1, "Transcript", "john_smith_academic_transcript.pdf"),
                (1, "Medical Record", "john_smith_medical_form.pdf"),
                (1, "Financial Aid", "john_smith_fafsa_application.pdf"),
                (1, "Housing Agreement", "john_smith_dorm_contract.pdf"),
                // Student 2 documents
                (2, "ID Card", "sarah_johnson_id_card.pdf"),
                (2, "Transcript", "sarah_johnson_transcript.pdf"),
                (2, "Certificate", "sarah_johnson_honors_certificate.pdf"),
                (2, "Research Paper", "sarah_johnson_research_submission.pdf"),
                (2, "Club Membership", "sarah_johnson_science_club_card.pdf"),
                // Student 3 documents
                (3, "ID Card", "michael_williams_id_card.pdf"),
                (3, "Transcript", "michael_williams_transcript.pdf"),
                (3, "Sports Waiver", "michael_williams_athletic_waiver.pdf"),
                (3, "Scholarship", "michael_williams_merit_scholarship.pdf"),
                (3, "Internship", "michael_williams_internship_agreement.pdf"),
                // Student 4 documents
                (4, "ID Card", "emily_brown_id_card.pdf"),
                (4, "Transcript", "emily_brown_transcript.pdf"),
                (4, "Art Portfolio", "emily_brown_portfolio.pdf"),
                (4, "Volunteer Hours", "emily_brown_community_service.pdf"),
                (4, "Library Card", "emily_brown_library_membership.pdf"),
                // Student 5 documents
                (5, "ID Card", "david_martinez_id_card.pdf"),
                (5, "Certificate", "david_martinez_deans_list.pdf"),
                (5, "Research Grant", "david_martinez_research_proposal.pdf"),
                (5, "Conference", "david_martinez_conference_abstract.pdf"),
                (5, "Tutoring", "david_martinez_peer_tutor_certification.pdf"),
                // Student 6 documents
                (6, "ID Card", "jessica_garcia_id_card.pdf"),
                (6, "Transcript", "jessica_garcia_transcript.pdf"),
                (6, "Exchange Program", "jessica_garcia_study_abroad.pdf"),
                (6, "Language Certificate", "jessica_garcia_spanish_cert.pdf"),
                (6, "Cultural Event", "jessica_garcia_festival_participation.pdf"),
                // Student 7 documents
                (7, "ID Card", "christopher_miller_id_card.pdf"),
                (7, "Transcript", "christopher_miller_transcript.pdf"),
                (7, "Music Performance", "christopher_miller_recital_program.pdf"),
                (7, "Band Membership", "christopher_miller_orchestra_card.pdf"),
                (7, "Concert Review", "christopher_miller_critique.pdf"),
                // Student 8 documents
                (8, "ID Card", "amanda_davis_id_card.pdf"),
                (8, "Transcript", "amanda_davis_transcript.pdf"),
                (8, "Debate Team", "amanda_davis_debate_award.pdf"),
                (8, "Public Speaking", "amanda_davis_speech_competition.pdf"),
                (8, "Leadership", "amanda_davis_student_council.pdf"),
                // Student 9 documents
                (9, "ID Card", "daniel_rodriguez_id_card.pdf"),
                (9, "Transcript", "daniel_rodriguez_transcript.pdf"),
                (9, "Engineering Project", "daniel_rodriguez_robotics_project.pdf"),
                (9, "Patent", "daniel_rodriguez_invention_disclosure.pdf"),
                (9, "Competition", "daniel_rodriguez_science_fair.pdf"),
                // Student 10 documents
                (10, "ID Card", "ashley_martinez_id_card.pdf"),
                (10, "Transcript", "ashley_martinez_transcript.pdf"),
                (10, "Photography", "ashley_martinez_exhibit_catalog.pdf"),
                (10, "Gallery", "ashley_martinez_art_show.pdf"),
                (10, "Creative Writing", "ashley_martinez_literary_magazine.pdf")
            };

            foreach (var (studentId, type, fileName) in documents)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Documents (StudentId, DocumentType, FileName) VALUES (@s, @t, @f)";
                    cmd.Parameters.AddWithValue("@s", studentId);
                    cmd.Parameters.AddWithValue("@t", type);
                    cmd.Parameters.AddWithValue("@f", fileName);
                    try { cmd.ExecuteNonQuery(); } catch { } // Ignore duplicates
                }
            }
        }
    }
}
