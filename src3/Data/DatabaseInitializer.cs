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
                        DateOfBirth DATE
                    );",
                    
                    // Courses table
                    @"CREATE TABLE IF NOT EXISTS Courses (
                        CourseId INTEGER PRIMARY KEY AUTOINCREMENT,
                        Code TEXT NOT NULL UNIQUE,
                        Title TEXT NOT NULL,
                        Credits INTEGER NOT NULL
                    );",
                    
                    // Enrollments table
                    @"CREATE TABLE IF NOT EXISTS Enrollments (
                        EnrollmentId INTEGER PRIMARY KEY AUTOINCREMENT,
                        StudentId INTEGER NOT NULL,
                        CourseId INTEGER NOT NULL,
                        EnrolledOn DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (StudentId) REFERENCES Students(StudentId) ON DELETE CASCADE,
                        FOREIGN KEY (CourseId) REFERENCES Courses(CourseId) ON DELETE CASCADE
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

            // Add dummy students
            var students = new[]
            {
                ("John", "Smith", "john.smith@school.edu", new DateTime(2005, 3, 15)),
                ("Sarah", "Johnson", "sarah.johnson@school.edu", new DateTime(2004, 7, 22)),
                ("Michael", "Williams", "michael.williams@school.edu", new DateTime(2005, 11, 8)),
                ("Emily", "Brown", "emily.brown@school.edu", new DateTime(2004, 5, 30)),
                ("David", "Martinez", "david.martinez@school.edu", new DateTime(2005, 9, 12))
            };

            foreach (var (firstName, lastName, email, dob) in students)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Students (FirstName, LastName, Email, DateOfBirth) VALUES (@f, @l, @e, @d)";
                    cmd.Parameters.AddWithValue("@f", firstName);
                    cmd.Parameters.AddWithValue("@l", lastName);
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.Parameters.AddWithValue("@d", dob);
                    cmd.ExecuteNonQuery();
                }
            }

            // Add dummy courses
            var courses = new[]
            {
                ("CS101", "Introduction to Computer Science", 3),
                ("CS201", "Data Structures", 4),
                ("MATH101", "Calculus I", 4),
                ("ENG101", "English Composition", 3),
                ("PHYS101", "Physics I", 4),
                ("CHEM101", "Chemistry I", 3)
            };

            foreach (var (code, title, credits) in courses)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Courses (Code, Title, Credits) VALUES (@c, @t, @cr)";
                    cmd.Parameters.AddWithValue("@c", code);
                    cmd.Parameters.AddWithValue("@t", title);
                    cmd.Parameters.AddWithValue("@cr", credits);
                    cmd.ExecuteNonQuery();
                }
            }

            // Add dummy enrollments
            var enrollments = new[]
            {
                (1, 1), (1, 2), (1, 3),
                (2, 1), (2, 4), (2, 5),
                (3, 2), (3, 3), (3, 6),
                (4, 1), (4, 4), (4, 5),
                (5, 2), (5, 5), (5, 6)
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
        }
    }
}
