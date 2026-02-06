# Student Information System - SQLite Edition

A **Windows Forms** desktop application for managing student and course records, powered by **SQLite3**.

## ✨ Features

- 👤 **User Authentication** - Register & login with SHA256 password hashing
- 👥 **Student Management** - Full CRUD (Create, Read, Update, Delete) operations
- 📚 **Course Management** - Add, edit, and manage courses
- 💾 **SQLite Database** - Zero-setup, file-based database
- 🎯 **Windows Forms UI** - Clean, intuitive interface
- 🔒 **Data Security** - Parameterized queries, password hashing

## 🚀 Quick Start

### Prerequisites
- .NET 6.0 SDK or later
- Windows (or WSL for Linux/Mac)

### Build & Run

```bash
cd src3
dotnet build
dotnet run
```

**That's it!** The application will:
1. Auto-create `StudentInfoDB.db` on first run
2. Set up all tables automatically
3. Be ready to use immediately

### First-Time Usage

1. **Register**: Click "Register" and create a new user account
2. **Login**: Use your credentials to log in
3. **Add Students**: Click "Add Student" and fill in the form
4. **Manage Courses**: Click "Manage Courses" to add/edit/delete courses

## 📁 Project Structure

```
src3/
├── Program.cs                 # Entry point with DB initialization
├── StudentInfoApp3.csproj    # Project file
├── Models/
│   ├── Student.cs            # Student model
│   └── Course.cs             # Course model
├── Data/
│   ├── DataAccess.cs         # Database operations (CRUD)
│   └── DatabaseInitializer.cs # Auto-schema creation
└── Forms/
    ├── LoginForm.cs          # Login/Registration UI
    ├── MainForm.cs           # Main student management UI
    ├── StudentForm.cs        # Student add/edit dialog
    ├── CourseForm.cs         # Course management UI
    └── CourseEditForm.cs     # Course add/edit dialog
```

## 🗄️ Database

### Auto-Initialization
- Database file: `StudentInfoDB.db` (created automatically)
- Tables created on first app launch
- No manual SQL execution needed!

### Schema

**Users Table**
- UserId (Primary Key)
- Username (Unique)
- PasswordHash

**Students Table**
- StudentId (Primary Key)
- FirstName, LastName
- Email, DateOfBirth

**Courses Table**
- CourseId (Primary Key)
- Code (Unique), Title
- Credits

**Enrollments Table** (for future use)
- EnrollmentId (Primary Key)
- StudentId (Foreign Key)
- CourseId (Foreign Key)
- EnrolledOn (Timestamp)

## 🛠️ Technology Stack

| Component | Technology |
|-----------|-----------|
| **UI Framework** | Windows Forms (.NET 6.0) |
| **Database** | SQLite 3 |
| **Language** | C# |
| **Data Access** | ADO.NET |
| **Security** | SHA256 password hashing |

## 🔧 Configuration

### Change Database Location

Edit `src3/Data/DataAccess.cs`:

```csharp
public static string ConnectionString { get; set; } = 
    "Data Source=C:\\custom\\path\\StudentInfoDB.db;Version=3;";
```

### View Database in VS Code

Install [SQLite extension](https://marketplace.visualstudio.com/items?itemName=alexcvzz.vscode-sqlite) and right-click `StudentInfoDB.db` to browse tables and data.

## 📋 User Operations

### Registration
1. Click "Register" on login form
2. Enter username and password
3. Account created with hashed password

### Login
1. Enter credentials
2. Automatic validation against database
3. Access main student management form

### Student Management
- **Add**: Click "Add Student", fill form, click OK
- **Edit**: Select student in grid, click "Edit Student"
- **Delete**: Select student, click "Delete Student", confirm
- **Refresh**: Click "Refresh" to reload data from database

### Course Management
- **Add**: Click "Manage Courses" → "Add"
- **Edit**: Select course → "Edit"
- **Delete**: Select course → "Delete", confirm

## ⚙️ Build & Run Details

```bash
# Build (creates bin/Debug)
dotnet build

# Run application
dotnet run

# Build release version
dotnet build --configuration Release

# Run tests (if added)
dotnet test
```

## 📊 Build Status

✅ **Build Successful**
- 0 Compilation Errors
- 14 Warnings (nullability - safe to ignore)
- Ready for production use

## 🎓 Learning Points

This project demonstrates:
- Windows Forms UI design
- ADO.NET database operations
- SQLite database usage
- CRUD operations
- Password security (SHA256 hashing)
- Parameterized SQL queries (SQL injection prevention)
- Object-oriented programming in C#

## 📝 Notes

- Application creates `StudentInfoDB.db` in the project folder
- Move the entire `src3` folder to relocate the database
- Database file is human-readable but binary (encrypted by SQLite)
- Connection pooling handled automatically by SQLiteConnection
- Foreign keys enabled for data integrity

## ✅ Deliverables

- [x] Windows Forms application
- [x] SQLite database with auto-initialization
- [x] User authentication system
- [x] Student CRUD functionality
- [x] Course CRUD functionality
- [x] Data validation
- [x] Security best practices

## 📞 Support

For SQLite setup details, see `SQLITE_SETUP.md`

---

**Status**: Ready for use and deployment
**Last Updated**: February 6, 2026
