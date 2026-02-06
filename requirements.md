Group Name: [Insert Group Name Here]
Group Members: [List members here]

1. Introduction / Aims & Objectives
This Student Information System (SIS) is a Windows Forms desktop application designed to manage student and course records for a small educational institution. It solves the manual-paper problem by providing a simple database-backed system for storing, retrieving, and maintaining student information and course data. Benefits include faster record lookup, reliable backups (via SQL Server), and basic access control via user login/registration.

2. Key Features (Functional)
- User registration and login (authentication)
- Students: Create, Read (list), Update, Delete records
- Courses: Create, Read, Update, Delete records
- Basic enrollment join table to model many-to-many student-course relationships
- Input validation (basic)

3. Non-Functional Requirements
- Desktop Windows Forms UI using Visual Studio and .NET 6
- SQL Server (LocalDB) persistent storage
- Secure password storage (hashed with SHA256)
- Simple, responsive UI for ordinary desktop screen sizes

4. Available Forms (UI screens)
- Login / Registration Form: authenticate users or create new accounts.
- Main Dashboard Form: lists students, toolstrip to add/edit/delete/refresh.
- Student Form: add/edit a single student.
- (Optional) Course Management Form: add/edit courses (similar to Student Form).

5. Entity-Relationship (ER) Summary
Entities (tables): Users, Students, Courses, Enrollments.
- Users(UserId, Username, PasswordHash)
- Students(StudentId, FirstName, LastName, Email, DateOfBirth)
- Courses(CourseId, Code, Title, Credits)
- Enrollments(EnrollmentId, StudentId -> Students, CourseId -> Courses, EnrolledOn)

Normalization: The schema is normalized: Students and Courses are separate entities; Enrollments implements many-to-many relation without redundant data.

6. Implementation Notes
- The sample code uses ADO.NET (`System.Data.SqlClient`) for clarity and minimal dependencies.
- To create the database run `sql/create_db.sql` or use `DataAccess.EnsureDatabaseExists` with the script contents.

ERD: Use Visual-Paradigm Online to draw the ERD and export PNG. Place ERD PNG in project root as `erd.png` before submission.

Deadline: Sunday February 08, 2026
