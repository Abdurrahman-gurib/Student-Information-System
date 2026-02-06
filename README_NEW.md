# Student Information System

A Windows Forms desktop application for managing student records, built with C# and .NET 6.0, using SQL Server LocalDB.

## Features
- **User Authentication**: Login and registration with password hashing (SHA256)
- **Student Management**: Full CRUD operations (Create, Read, Update, Delete)
- **Course Management**: Manage courses with course codes, titles, and credit hours
- **Data Persistence**: SQL Server LocalDB backend with normalized schema

## Project Structure

### src3/ (Primary Working Project)
The clean, buildable implementation:
- **Program.cs**: Application entry point  
- **Models/**: Student, Course classes
- **Data/DataAccess.cs**: Database operations using ADO.NET
- **Forms/**: UI forms (LoginForm, MainForm, StudentForm, CourseForm, CourseEditForm)
- **StudentInfoApp3.csproj**: Project configuration

### Database
**SQL Script**: `sql/create_db.sql`
- Creates `StudentInfoDB` database
- Tables: `Users`, `Students`, `Courses`, `Enrollments`
- Includes primary keys, foreign keys, and constraints

### Documentation
- **requirements.md**: Detailed requirements and feature list
- **erd.svg**: Entity-Relationship Diagram placeholder

## Getting Started

### Prerequisites
- .NET 6.0 SDK or later
- SQL Server LocalDB
- Visual Studio Code or Visual Studio

### Build
```bash
cd src3
dotnet build
dotnet run
```

### Database Setup
Before running the application for the first time, execute the SQL script:
```sql
-- Execute sql/create_db.sql against your SQL Server LocalDB instance
-- Creates StudentInfoDB and all required tables
```

Connection string in DataAccess.cs:
```
Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=StudentInfoDB;
```

## Testing
1. Run the application
2. Register a new user (username/password)
3. Login with your credentials
4. Add students and courses from the main form
5. Edit or delete records as needed

## Build Status
✅ **Build Successful** (14 warnings, 0 errors)
- All compilation errors resolved
- Application ready for testing

## Deliverables Completed
- [x] Working Windows Forms application
- [x] User authentication system
- [x] Student CRUD UI and backend
- [x] Course CRUD UI and backend
- [x] SQL database schema with normalized tables
- [x] Requirements documentation

## Notes
- The application uses SHA256 password hashing for security
- Database operations use parameterized queries to prevent SQL injection
- Warnings are related to nullability attributes and can be safely ignored for this version
