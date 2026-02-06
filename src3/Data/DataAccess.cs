using System;
using System.Collections.Generic;
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

        public static List<Student> GetStudents()
        {
            var list = new List<Student>();
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT StudentId,FirstName,LastName,Email,DateOfBirth FROM Students";
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
                list.Add(new Student
                {
                    StudentId = rdr.GetInt32(0),
                    FirstName = rdr.GetString(1),
                    LastName = rdr.GetString(2),
                    Email = rdr.IsDBNull(3) ? string.Empty : rdr.GetString(3),
                    DateOfBirth = rdr.IsDBNull(4) ? (DateTime?)null : rdr.GetDateTime(4)
                });
            return list;
        }

        public static bool AddStudent(Student s)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Students (FirstName,LastName,Email,DateOfBirth) VALUES (@f,@l,@e,@d)";
            cmd.Parameters.AddWithValue("@f", s.FirstName);
            cmd.Parameters.AddWithValue("@l", s.LastName);
            cmd.Parameters.AddWithValue("@e", (object)s.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@d", (object)s.DateOfBirth ?? DBNull.Value);
            return cmd.ExecuteNonQuery() == 1;
        }

        public static bool UpdateStudent(Student s)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Students SET FirstName=@f,LastName=@l,Email=@e,DateOfBirth=@d WHERE StudentId=@id";
            cmd.Parameters.AddWithValue("@f", s.FirstName);
            cmd.Parameters.AddWithValue("@l", s.LastName);
            cmd.Parameters.AddWithValue("@e", (object)s.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@d", (object)s.DateOfBirth ?? DBNull.Value);
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
            cmd.CommandText = "SELECT CourseId,Code,Title,Credits FROM Courses";
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
                list.Add(new Course
                {
                    CourseId = rdr.GetInt32(0),
                    Code = rdr.GetString(1),
                    Title = rdr.GetString(2),
                    Credits = rdr.GetInt32(3)
                });
            return list;
        }

        public static bool AddCourse(Course c)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Courses (Code,Title,Credits) VALUES (@code,@title,@credits)";
            cmd.Parameters.AddWithValue("@code", c.Code);
            cmd.Parameters.AddWithValue("@title", c.Title);
            cmd.Parameters.AddWithValue("@credits", c.Credits);
            return cmd.ExecuteNonQuery() == 1;
        }

        public static bool UpdateCourse(Course c)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Courses SET Code=@code,Title=@title,Credits=@credits WHERE CourseId=@id";
            cmd.Parameters.AddWithValue("@code", c.Code);
            cmd.Parameters.AddWithValue("@title", c.Title);
            cmd.Parameters.AddWithValue("@credits", c.Credits);
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
    }
}
