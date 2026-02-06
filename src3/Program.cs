using System;
using System.Windows.Forms;
using StudentInfoApp3.Forms;
using StudentInfoApp3.Data;

namespace StudentInfoApp3
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Initialize SQLite database and tables
            DatabaseInitializer.Initialize();
            
            ApplicationConfiguration.Initialize();
            Application.Run(new LoginForm());
        }
    }
}
