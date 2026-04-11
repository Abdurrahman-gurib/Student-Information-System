-- SQLite database schema for Student Information System
-- This script creates tables for StudentInfoDB.db

-- Enable foreign keys
PRAGMA foreign_keys = ON;

-- Users table
CREATE TABLE IF NOT EXISTS Users (
    UserId INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL
);

-- Students table
CREATE TABLE IF NOT EXISTS Students (
    StudentId INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    Email TEXT,
    DateOfBirth DATE,
    Phone TEXT,
    Address TEXT
);

-- Courses table
CREATE TABLE IF NOT EXISTS Courses (
    CourseId INTEGER PRIMARY KEY AUTOINCREMENT,
    Code TEXT NOT NULL UNIQUE,
    Title TEXT NOT NULL,
    Credits INTEGER NOT NULL,
    Description TEXT
);

-- Enrollments table (normalized many-to-many relationship)
CREATE TABLE IF NOT EXISTS Enrollments (
    EnrollmentId INTEGER PRIMARY KEY AUTOINCREMENT,
    StudentId INTEGER NOT NULL,
    CourseId INTEGER NOT NULL,
    EnrolledOn DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId) ON DELETE CASCADE,
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId) ON DELETE CASCADE
);

-- Grades table
CREATE TABLE IF NOT EXISTS Grades (
    GradeId INTEGER PRIMARY KEY AUTOINCREMENT,
    StudentId INTEGER NOT NULL,
    CourseId INTEGER NOT NULL,
    Grade TEXT NOT NULL,
    Semester TEXT,
    Year INTEGER,
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId) ON DELETE CASCADE,
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId) ON DELETE CASCADE
);

-- Documents table
CREATE TABLE IF NOT EXISTS Documents (
    DocumentId INTEGER PRIMARY KEY AUTOINCREMENT,
    StudentId INTEGER NOT NULL,
    DocumentType TEXT NOT NULL,
    FileName TEXT NOT NULL,
    UploadDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId) ON DELETE CASCADE
);

-- Sessions table for tracking user activity
CREATE TABLE IF NOT EXISTS Sessions (
    SessionId INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER,
    LoginTime DATETIME DEFAULT CURRENT_TIMESTAMP,
    LogoutTime DATETIME,
    Duration INTEGER, -- in minutes
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- Issues table for student issues
CREATE TABLE IF NOT EXISTS Issues (
    IssueId INTEGER PRIMARY KEY AUTOINCREMENT,
    StudentId INTEGER NOT NULL,
    IssueType TEXT NOT NULL, -- e.g., 'Low GPA', 'Missing Documents', 'Attendance'
    Description TEXT,
    ReportedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    Status TEXT DEFAULT 'Open', -- Open, Resolved, Pending
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId) ON DELETE CASCADE
);

-- Create indexes for better query performance
CREATE INDEX IF NOT EXISTS idx_username ON Users(Username);
CREATE INDEX IF NOT EXISTS idx_student_name ON Students(LastName, FirstName);
CREATE INDEX IF NOT EXISTS idx_course_code ON Courses(Code);
CREATE INDEX IF NOT EXISTS idx_enrollments_student ON Enrollments(StudentId);
CREATE INDEX IF NOT EXISTS idx_enrollments_course ON Enrollments(CourseId);
CREATE INDEX IF NOT EXISTS idx_grades_student ON Grades(StudentId);
CREATE INDEX IF NOT EXISTS idx_documents_student ON Documents(StudentId);
CREATE INDEX IF NOT EXISTS idx_sessions_user ON Sessions(UserId);
CREATE INDEX IF NOT EXISTS idx_issues_student ON Issues(StudentId);