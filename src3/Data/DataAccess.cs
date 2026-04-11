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
            return cmd.ExecuteNonQuery() == 1;
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
            return cmd.ExecuteNonQuery() == 1;
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
    }
}
