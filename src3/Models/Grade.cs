using System;

namespace StudentInfoApp3.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string GradeValue { get; set; }
        public string Semester { get; set; }
        public int Year { get; set; }
    }
}