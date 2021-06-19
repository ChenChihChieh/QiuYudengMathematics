using System.Collections.Generic;

namespace QiuYudengMathematics.Models.ViewModels
{
    public class CourseManagementViewModel
    {
        public int CourseSeq { get; set; }
        public string CourseName { get; set; }
        public string Url { get; set; }
        public int SubjectId { get; set; }
        public bool Enable { get; set; }
        public List<string> Student { get; set; }
    }
}