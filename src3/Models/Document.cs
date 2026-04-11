using System;

namespace StudentInfoApp3.Models
{
    public class Document
    {
        public int DocumentId { get; set; }
        public int StudentId { get; set; }
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadDate { get; set; }
    }
}