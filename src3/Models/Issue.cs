using System;

namespace StudentInfoApp3.Models
{
    public class Issue
    {
        public int IssueId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string IssueType { get; set; }
        public string Description { get; set; }
        public DateTime ReportedDate { get; set; }
        public string Status { get; set; }
    }
}