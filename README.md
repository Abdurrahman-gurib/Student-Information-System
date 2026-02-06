Student Information System (Windows Forms, C#, SQL Server)

Overview
- Simple desktop app built with .NET 6 Windows Forms.
- Features user login/registration and CRUD for Students and Courses.

How to run
1. Install .NET 6 SDK and Visual Studio (or build with `dotnet build`).
2. Create the database by running `sql/create_db.sql` in SQL Server Management Studio or run the script programmatically.
3. Open the `src` folder in Visual Studio and run the project.

Notes
- Default connection string uses LocalDB: `Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=StudentInfoDB;` (change in `Data/DataAccess.cs` if needed).
- This repo includes minimal UI code to demonstrate required functionality.

ERD:
- A placeholder ERD SVG is included as `erd.svg`. Export a PNG from Visual Paradigm Online and replace the file `erd.png` before final submission if required.
