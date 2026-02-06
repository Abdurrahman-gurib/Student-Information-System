# SQLite Setup Guide

Your Student Information System has been converted to use **SQLite3** instead of SQL Server.

## What Changed

### 1. Database
- **From**: SQL Server LocalDB
- **To**: SQLite 3 (file-based database)
- **Database File**: `StudentInfoDB.db` (created automatically in the project folder)

### 2. Dependencies
- Added: `System.Data.SQLite` NuGet package
- Removed dependency on SQL Server

### 3. Code Changes
- Updated `DataAccess.cs` to use `SQLiteConnection` instead of `SqlConnection`
- Updated `Program.cs` to auto-initialize the database on first run
- Created `DatabaseInitializer.cs` for automatic schema creation

## How It Works

### Automatic Database Creation
When you run the application for the first time:
1. `DatabaseInitializer.Initialize()` is called automatically
2. Creates `StudentInfoDB.db` file in the project folder
3. Creates all necessary tables: Users, Students, Courses, Enrollments
4. Enables foreign key constraints

**No manual SQL script execution needed!**

## Running the Application

```bash
# Build
cd src3
dotnet build

# Run (database auto-creates on startup)
dotnet run
```

The application will:
1. Auto-create `StudentInfoDB.db` on first run
2. Create all tables automatically
3. Be ready to use immediately

## Features
- ✅ Zero database setup required
- ✅ File-based (no server installation)
- ✅ Fully portable (move the folder anywhere)
- ✅ Perfect for development/testing
- ✅ VS Code SQLite extension support

## Viewing the Database in VS Code

### Option 1: SQLite Extension
1. Install [SQLite](https://marketplace.visualstudio.com/items?itemName=alexcvzz.vscode-sqlite) extension
2. Open the Explorer in VS Code
3. Right-click `StudentInfoDB.db` and select "Open Database"
4. Browse tables and data in the SQLite view

### Option 2: Command Line
```bash
# Install sqlite3 command line (if not already installed)
# Windows: choco install sqlite or download from sqlite.org

# Open database
sqlite3 StudentInfoDB.db

# View tables
.tables

# Query data
SELECT * FROM Users;
SELECT * FROM Students;
SELECT * FROM Courses;
```

## Connection String

Current connection string in `DataAccess.cs`:
```
Data Source=StudentInfoDB.db;Version=3;
```

Change location by modifying the path in `DataAccess.cs`:
```csharp
public static string ConnectionString { get; set; } = "Data Source=C:\\path\\to\\StudentInfoDB.db;Version=3;";
```

## Advantages of SQLite

| Feature | SQL Server | SQLite |
|---------|-----------|--------|
| Setup | Server installation required | Automatic, file-based |
| Portability | Tied to server | Fully portable |
| File size | Large | Minimal |
| Development | Complex setup | Zero setup |
| Learning | Higher barrier | Great for learning |

## Notes

- The `sql/create_db.sql` file is now in SQLite format (for reference)
- Database auto-initializes, but you can also run the SQL script manually if needed
- All CRUD operations work identically to SQL Server version
- Data is persisted in `StudentInfoDB.db`
