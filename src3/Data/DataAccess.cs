using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System.Text;
using StudentInfoApp3.Models;

namespace StudentInfoApp3.Data
{
    public static class DataAccess
    {
        public static string ConnectionString { get; set; } = "Data Source=StudentInfoDB.db;";

        static string Hash(string input)
        {
            using var sha = SHA256.Create();
            var b = Encoding.UTF8.GetBytes(input);
            var h = sha.ComputeHash(b);
            return Convert.ToBase64String(h);
        }

        public static bool CreateUser(string username, string password)
        {
            var hashed = Hash(password);
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Users (Username, PasswordHash) VALUES (@u,@p)";
            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@p", hashed);
            try { return cmd.ExecuteNonQuery() == 1; } catch { return false; }
        }

        public static bool VerifyUser(string username, string password)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT PasswordHash FROM Users WHERE Username = @u";
            cmd.Parameters.AddWithValue("@u", username);
            var obj = cmd.ExecuteScalar();
            if (obj == null) return false;
            return obj.ToString() == Hash(password);
        }

        public static bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (!VerifyUser(username, oldPassword))
                return false;

            var hashed = Hash(newPassword);
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Users SET PasswordHash = @p WHERE Username = @u";
            cmd.Parameters.AddWithValue("@p", hashed);
            cmd.Parameters.AddWithValue("@u", username);
            return cmd.ExecuteNonQuery() == 1;
        }

        public static bool ResetPassword(string username, string newPassword)
        {
            var hashed = Hash(newPassword);
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Users SET PasswordHash = @p WHERE Username = @u";
            cmd.Parameters.AddWithValue("@p", hashed);
            cmd.Parameters.AddWithValue("@u", username);
            return cmd.ExecuteNonQuery() == 1;
        }

        public static List<Student> GetStudents()
        {
            var list = new List<Student>();
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT StudentId,FirstName,LastName,Email,DateOfBirth,Phone,Address FROM Students";
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
                list.Add(new Student
                {
                    StudentId = rdr.GetInt32(0),
                    FirstName = rdr.GetString(1),
                    LastName = rdr.GetString(2),
                    Email = rdr.IsDBNull(3) ? string.Empty : rdr.GetString(3),
                    DateOfBirth = rdr.IsDBNull(4) ? (DateTime?)null : rdr.GetDateTime(4),
                    Phone = rdr.IsDBNull(5) ? string.Empty : rdr.GetString(5),
                    Address = rdr.IsDBNull(6) ? string.Empty : rdr.GetString(6)
                });
            return list;
        }

        public static bool AddStudent(Student s)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Students (FirstName,LastName,Email,DateOfBirth,Phone,Address) VALUES (@f,@l,@e,@d,@p,@a)";
            cmd.Parameters.AddWithValue("@f", s.FirstName);
            cmd.Parameters.AddWithValue("@l", s.LastName);
            cmd.Parameters.AddWithValue("@e", (object)s.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@d", (object)s.DateOfBirth ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@p", (object)s.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@a", (object)s.Address ?? DBNull.Value);
            if (cmd.ExecuteNonQuery() == 1)
            {
                using var idCmd = conn.CreateCommand();
                idCmd.CommandText = "SELECT last_insert_rowid();";
                s.StudentId = Convert.ToInt32(idCmd.ExecuteScalar());
                return true;
            }
            return false;
        }

        public static bool UpdateStudent(Student s)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Students SET FirstName=@f,LastName=@l,Email=@e,DateOfBirth=@d,Phone=@p,Address=@a WHERE StudentId=@id";
            cmd.Parameters.AddWithValue("@f", s.FirstName);
            cmd.Parameters.AddWithValue("@l", s.LastName);
            cmd.Parameters.AddWithValue("@e", (object)s.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@d", (object)s.DateOfBirth ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@p", (object)s.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@a", (object)s.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@id", s.StudentId);
            return cmd.ExecuteNonQuery() == 1;
        }

        public static bool DeleteStudent(int id)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Students WHERE StudentId=@id";
            cmd.Parameters.AddWithValue("@id", id);
            return cmd.ExecuteNonQuery() == 1;
        }

        public static List<Course> GetCourses()
        {
            var list = new List<Course>();
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT CourseId,Code,Title,Credits,Description FROM Courses";
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
                list.Add(new Course
                {
                    CourseId = rdr.GetInt32(0),
                    Code = rdr.GetString(1),
                    Title = rdr.GetString(2),
                    Credits = rdr.GetInt32(3),
                    Description = rdr.IsDBNull(4) ? string.Empty : rdr.GetString(4)
                });
            return list;
        }

        public static bool AddCourse(Course c)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Courses (Code,Title,Credits,Description) VALUES (@code,@title,@credits,@desc)";
            cmd.Parameters.AddWithValue("@code", c.Code);
            cmd.Parameters.AddWithValue("@title", c.Title);
            cmd.Parameters.AddWithValue("@credits", c.Credits);
            cmd.Parameters.AddWithValue("@desc", (object)c.Description ?? DBNull.Value);
            if (cmd.ExecuteNonQuery() == 1)
            {
                using var idCmd = conn.CreateCommand();
                idCmd.CommandText = "SELECT last_insert_rowid();";
                c.CourseId = Convert.ToInt32(idCmd.ExecuteScalar());
                return true;
            }
            return false;
        }

        public static bool UpdateCourse(Course c)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Courses SET Code=@code,Title=@title,Credits=@credits,Description=@desc WHERE CourseId=@id";
            cmd.Parameters.AddWithValue("@code", c.Code);
            cmd.Parameters.AddWithValue("@title", c.Title);
            cmd.Parameters.AddWithValue("@credits", c.Credits);
            cmd.Parameters.AddWithValue("@desc", (object)c.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@id", c.CourseId);
            return cmd.ExecuteNonQuery() == 1;
        }

        public static bool DeleteCourse(int id)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Courses WHERE CourseId=@id";
            cmd.Parameters.AddWithValue("@id", id);
            return cmd.ExecuteNonQuery() == 1;
        }

        // Enrollment methods
        public static List<Course> GetEnrolledCourses(int studentId)
        {
            var list = new List<Course>();
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT c.CourseId, c.Code, c.Title, c.Credits, c.Description 
                               FROM Courses c 
                               INNER JOIN Enrollments e ON c.CourseId = e.CourseId 
                               WHERE e.StudentId = @sid";
            cmd.Parameters.AddWithValue("@sid", studentId);
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
                list.Add(new Course
                {
                    CourseId = rdr.GetInt32(0),
                    Code = rdr.GetString(1),
                    Title = rdr.GetString(2),
                    Credits = rdr.GetInt32(3),
                    Description = rdr.IsDBNull(4) ? string.Empty : rdr.GetString(4)
                });
            return list;
        }

        public static bool EnrollStudent(int studentId, int courseId)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Enrollments (StudentId, CourseId) VALUES (@sid, @cid)";
            cmd.Parameters.AddWithValue("@sid", studentId);
            cmd.Parameters.AddWithValue("@cid", courseId);
            try { return cmd.ExecuteNonQuery() == 1; } catch { return false; }
        }

        public static bool UnenrollStudent(int studentId, int courseId)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Enrollments WHERE StudentId = @sid AND CourseId = @cid";
            cmd.Parameters.AddWithValue("@sid", studentId);
            cmd.Parameters.AddWithValue("@cid", courseId);
            return cmd.ExecuteNonQuery() == 1;
        }

        // Grade methods
        public static List<Grade> GetStudentGrades(int studentId)
        {
            var list = new List<Grade>();
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT g.GradeId, g.StudentId, g.CourseId, g.Grade, g.Semester, g.Year, c.Title 
                               FROM Grades g 
                               INNER JOIN Courses c ON g.CourseId = c.CourseId 
                               WHERE g.StudentId = @sid";
            cmd.Parameters.AddWithValue("@sid", studentId);
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
                list.Add(new Grade
                {
                    GradeId = rdr.GetInt32(0),
                    StudentId = rdr.GetInt32(1),
                    CourseId = rdr.GetInt32(2),
                    GradeValue = rdr.GetString(3),
                    Semester = rdr.IsDBNull(4) ? string.Empty : rdr.GetString(4),
                    Year = rdr.IsDBNull(5) ? 0 : rdr.GetInt32(5)
                });
            return list;
        }

        public static bool AddGrade(Grade g)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Grades (StudentId, CourseId, Grade, Semester, Year) VALUES (@sid, @cid, @grade, @sem, @year)";
            cmd.Parameters.AddWithValue("@sid", g.StudentId);
            cmd.Parameters.AddWithValue("@cid", g.CourseId);
            cmd.Parameters.AddWithValue("@grade", g.GradeValue);
            cmd.Parameters.AddWithValue("@sem", (object)g.Semester ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@year", g.Year);
            return cmd.ExecuteNonQuery() == 1;
        }

        public static double GetStudentGPA(int studentId)
        {
            var grades = GetStudentGrades(studentId);
            if (!grades.Any()) return 0.0;
            return grades.Average(g => GradeToPoint(g.GradeValue));
        }

        private static double GradeToPoint(string grade)
        {
            return grade.ToUpper() switch
            {
                "A" => 4.0,
                "B" => 3.0,
                "C" => 2.0,
                "D" => 1.0,
                "F" => 0.0,
                _ => 0.0
            };
        }

        // Document methods
        public static List<Document> GetStudentDocuments(int studentId)
        {
            var list = new List<Document>();
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT DocumentId, StudentId, DocumentType, FileName, FilePath, FileSize, UploadDate FROM Documents WHERE StudentId = @sid";
            cmd.Parameters.AddWithValue("@sid", studentId);
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
                list.Add(new Document
                {
                    DocumentId = rdr.GetInt32(0),
                    StudentId = rdr.GetInt32(1),
                    DocumentType = rdr.GetString(2),
                    FileName = rdr.GetString(3),
                    FilePath = rdr.IsDBNull(4) ? string.Empty : rdr.GetString(4),
                    FileSize = rdr.IsDBNull(5) ? 0 : rdr.GetInt64(5),
                    UploadDate = rdr.GetDateTime(6)
                });
            return list;
        }

        public static bool AddDocument(Document d)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Documents (StudentId, DocumentType, FileName, FilePath, FileSize) VALUES (@sid, @type, @file, @path, @size)";
            cmd.Parameters.AddWithValue("@sid", d.StudentId);
            cmd.Parameters.AddWithValue("@type", d.DocumentType);
            cmd.Parameters.AddWithValue("@file", d.FileName);
            cmd.Parameters.AddWithValue("@path", d.FilePath);
            cmd.Parameters.AddWithValue("@size", d.FileSize);
            return cmd.ExecuteNonQuery() == 1;
        }

        public static bool DeleteDocument(int documentId)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Documents WHERE DocumentId = @id";
            cmd.Parameters.AddWithValue("@id", documentId);
            return cmd.ExecuteNonQuery() == 1;
        }

        // Get current student (assuming logged in user corresponds to a student)
        public static Student GetCurrentStudent(string username)
        {
            // For simplicity, assume username is student ID or something. In real app, link users to students.
            // Here, just return first student for demo.
            var students = GetStudents();
            return students.FirstOrDefault();
        }

        // Admin statistics methods
        public static int GetTotalStudents()
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Students";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static int GetTotalCourses()
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Courses";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static int GetTotalGrades()
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Grades";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static int GetTotalUsers()
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Users";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static int GetTotalDocuments()
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Documents";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static double GetAverageGPA()
        {
            var students = GetStudents();
            if (!students.Any()) return 0.0;
            var gpas = students.Select(s => GetStudentGPA(s.StudentId)).Where(g => g > 0);
            return gpas.Any() ? gpas.Average() : 0.0;
        }

        public static Dictionary<string, int> GetGradeDistribution()
        {
            var dist = new Dictionary<string, int> { ["A"] = 0, ["B"] = 0, ["C"] = 0, ["D"] = 0, ["F"] = 0 };
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Grade, COUNT(*) FROM Grades GROUP BY Grade";
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var grade = rdr.GetString(0).ToUpper();
                if (dist.ContainsKey(grade))
                    dist[grade] = rdr.GetInt32(1);
            }
            return dist;
        }

        public static int GetTotalEnrollments()
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Enrollments";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static double GetAverageStudentsPerCourse()
        {
            int courses = GetTotalCourses();
            int enrollments = GetTotalEnrollments();
            return courses > 0 ? (double)enrollments / courses : 0.0;
        }

        public static Dictionary<string, int> GetIssueStatusCounts()
        {
            var result = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                ["Open"] = 0,
                ["Resolved"] = 0,
                ["Pending"] = 0
            };
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Status, COUNT(*) FROM Issues GROUP BY Status";
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var status = rdr.GetString(0);
                var count = rdr.GetInt32(1);
                result[status] = count;
            }
            return result;
        }

        public static int GetActiveUserCount()
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(DISTINCT UserId) FROM Sessions WHERE UserId IS NOT NULL";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static List<(string Username, DateTime LoginTime, DateTime? LogoutTime, int Duration)> GetRecentSessions(int top = 10)
        {
            var list = new List<(string Username, DateTime LoginTime, DateTime? LogoutTime, int Duration)>();
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT u.Username, s.LoginTime, s.LogoutTime, s.Duration
                               FROM Sessions s
                               LEFT JOIN Users u ON s.UserId = u.UserId
                               ORDER BY s.LoginTime DESC
                               LIMIT @top";
            cmd.Parameters.AddWithValue("@top", top);
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var username = rdr.IsDBNull(0) ? "Unknown" : rdr.GetString(0);
                var login = rdr.GetDateTime(1);
                var logout = rdr.IsDBNull(2) ? (DateTime?)null : rdr.GetDateTime(2);
                var duration = rdr.IsDBNull(3) ? 0 : rdr.GetInt32(3);
                list.Add((username, login, logout, duration));
            }
            return list;
        }

        public static List<(string IssueType, int Count)> GetIssueTypeCounts()
        {
            var result = new List<(string IssueType, int Count)>();
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT IssueType, COUNT(*) FROM Issues GROUP BY IssueType ORDER BY COUNT(*) DESC";
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                result.Add((rdr.GetString(0), rdr.GetInt32(1)));
            }
            return result;
        }

        public static List<Student> GetTopLowGPAStudents(int top = 10)
        {
            return GetStudents()
                .Select(s => new { Student = s, GPA = GetStudentGPA(s.StudentId) })
                .Where(x => x.GPA > 0)
                .OrderBy(x => x.GPA)
                .Take(top)
                .Select(x => x.Student)
                .ToList();
        }

        public static List<Student> GetTopPerformingStudents(int top = 5)
        {
            var students = GetStudents();
            return students.OrderByDescending(s => GetStudentGPA(s.StudentId)).Take(top).ToList();
        }

        public static List<Course> GetMostPopularCourses(int top = 5)
        {
            var courses = GetCourses();
            return courses.OrderByDescending(c => GetEnrollmentsForCourse(c.CourseId)).Take(top).ToList();
        }

        public static int GetEnrollmentsForCourse(int courseId)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Enrollments WHERE CourseId = @cid";
            cmd.Parameters.AddWithValue("@cid", courseId);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        // Session methods
        public static void LogLogin(int userId)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Sessions (UserId, LoginTime) VALUES (@uid, @time)";
            cmd.Parameters.AddWithValue("@uid", userId);
            cmd.Parameters.AddWithValue("@time", DateTime.Now);
            cmd.ExecuteNonQuery();
        }

        public static void LogLogout(int userId)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE Sessions SET LogoutTime = @time, Duration = 
                               CAST((julianday(@time) - julianday(LoginTime)) * 1440 AS INTEGER)
                               WHERE UserId = @uid AND LogoutTime IS NULL";
            cmd.Parameters.AddWithValue("@uid", userId);
            cmd.Parameters.AddWithValue("@time", DateTime.Now);
            cmd.ExecuteNonQuery();
        }

        public static int GetTotalSessions()
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Sessions";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static double GetAverageSessionDuration()
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT AVG(Duration) FROM Sessions WHERE Duration IS NOT NULL";
            var result = cmd.ExecuteScalar();
            return result != DBNull.Value ? Convert.ToDouble(result) : 0.0;
        }

        // Issues methods
        public static List<Issue> GetIssues()
        {
            var list = new List<Issue>();
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT i.IssueId, i.StudentId, s.FirstName, s.LastName, i.IssueType, i.Description, i.ReportedDate, i.Status
                               FROM Issues i
                               INNER JOIN Students s ON i.StudentId = s.StudentId";
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new Issue
                {
                    IssueId = rdr.GetInt32(0),
                    StudentId = rdr.GetInt32(1),
                    StudentName = $"{rdr.GetString(2)} {rdr.GetString(3)}",
                    IssueType = rdr.GetString(4),
                    Description = rdr.IsDBNull(5) ? string.Empty : rdr.GetString(5),
                    ReportedDate = rdr.GetDateTime(6),
                    Status = rdr.GetString(7)
                });
            }
            return list;
        }

        public static void AddIssue(int studentId, string issueType, string description)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Issues (StudentId, IssueType, Description) VALUES (@sid, @type, @desc)";
            cmd.Parameters.AddWithValue("@sid", studentId);
            cmd.Parameters.AddWithValue("@type", issueType);
            cmd.Parameters.AddWithValue("@desc", description);
            cmd.ExecuteNonQuery();
        }

        // Dummy data generation
        public static void GenerateDummyData()
        {
            // Clear existing activity and academic data, preserve admin login and recreate demo accounts.
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                DELETE FROM Issues;
                DELETE FROM Sessions;
                DELETE FROM Documents;
                DELETE FROM Grades;
                DELETE FROM Enrollments;
                DELETE FROM Courses;
                DELETE FROM Students;
                DELETE FROM Users WHERE Username <> 'admin';
            ";
            cmd.ExecuteNonQuery();

            CreateUser("admin", "Admin@2026");
            CreateUser("teacher1", "teacher123");
            CreateUser("student1", "student123");

            // Generate students
            string[] firstNames = { "John", "Jane", "Bob", "Alice", "Charlie", "Diana", "Eve", "Frank", "Grace", "Henry" };
            string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };

            var students = new List<Student>();
            for (int i = 0; i < 50; i++)
            {
                var student = new Student
                {
                    FirstName = firstNames[i % firstNames.Length],
                    LastName = lastNames[i % lastNames.Length] + (i / lastNames.Length + 1),
                    Email = $"student{i + 1}@university.edu",
                    DateOfBirth = DateTime.Now.AddYears(-20 - (i % 5)),
                    Phone = $"555-010{i % 100}",
                    Address = $"Address {i + 1}"
                };
                AddStudent(student);
                students.Add(student);
            }

            // Generate courses
            string[] courseCodes = { "CS101", "MATH201", "ENG102", "PHYS301", "CHEM202", "BIO103", "HIST104", "ART105" };
            string[] courseTitles = { "Introduction to Computer Science", "Calculus II", "English Composition", "Physics III", "Organic Chemistry", "Biology I", "World History", "Digital Art" };

            var courses = new List<Course>();
            for (int i = 0; i < courseCodes.Length; i++)
            {
                var course = new Course
                {
                    Code = courseCodes[i],
                    Title = courseTitles[i],
                    Credits = 3 + (i % 2),
                    Description = $"Description for {courseTitles[i]}"
                };
                AddCourse(course);
                courses.Add(course);
            }

            // Generate enrollments and grades
            Random rand = new Random();
            string[] grades = { "A", "B", "C", "D", "F" };
            foreach (var student in students)
            {
                int numCourses = rand.Next(3, 7);
                var enrolledCourses = courses.OrderBy(x => rand.Next()).Take(numCourses).ToList();
                foreach (var course in enrolledCourses)
                {
                    EnrollStudent(student.StudentId, course.CourseId);
                    string grade = grades[rand.Next(grades.Length)];
                    AddGrade(new Grade
                    {
                        StudentId = student.StudentId,
                        CourseId = course.CourseId,
                        GradeValue = grade,
                        Semester = "Fall",
                        Year = 2023
                    });
                }
            }

            // Generate documents
            string[] docTypes = { "Transcript", "ID Card", "Application", "Certificate" };
            foreach (var student in students)
            {
                int numDocs = rand.Next(1, 4);
                for (int i = 0; i < numDocs; i++)
                {
                    AddDocument(new Document
                    {
                        StudentId = student.StudentId,
                        DocumentType = docTypes[rand.Next(docTypes.Length)],
                        FileName = $"doc_{student.StudentId}_{i + 1}.pdf"
                    });
                }
            }

            // Generate issues
            string[] issueTypes = { "Low GPA", "Missing Documents", "Attendance Issue", "Financial Aid", "Academic Probation" };
            foreach (var student in students)
            {
                if (rand.Next(10) < 3) // 30% chance
                {
                    AddIssue(student.StudentId, issueTypes[rand.Next(issueTypes.Length)], "Generated issue for testing");
                }
            }

            // Generate sessions (simulate past logins)
            for (int i = 0; i < 100; i++)
            {
                int userId = rand.Next(1, students.Count + 1);
                DateTime loginTime = DateTime.Now.AddDays(-rand.Next(30));
                DateTime logoutTime = loginTime.AddMinutes(rand.Next(30, 240));
                using var cmd2 = conn.CreateCommand();
                cmd2.CommandText = "INSERT INTO Sessions (UserId, LoginTime, LogoutTime, Duration) VALUES (@uid, @login, @logout, @dur)";
                cmd2.Parameters.AddWithValue("@uid", userId);
                cmd2.Parameters.AddWithValue("@login", loginTime);
                cmd2.Parameters.AddWithValue("@logout", logoutTime);
                cmd2.Parameters.AddWithValue("@dur", (int)(logoutTime - loginTime).TotalMinutes);
                cmd2.ExecuteNonQuery();
            }
        }    }
}
