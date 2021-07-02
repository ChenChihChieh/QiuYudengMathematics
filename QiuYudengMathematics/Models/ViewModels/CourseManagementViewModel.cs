using System.Collections.Generic;
using System.Web;

namespace QiuYudengMathematics.Models.ViewModels
{
    public class CourseManagementViewModel
    {
        public int CourseSeq { get; set; }
        public string CourseName { get; set; }
        public string Url { get; set; }
        public HttpPostedFileBase Video { get; set; }
        /// <summary>
        /// (新增&更新用)
        /// </summary>
        public int SubjectId { get; set; }
        public SubbjectInfo SubbjectInfo { get; set; }
        public bool Enable { get; set; }
        public List<string> Student { get; set; }
    }
    public class SubbjectInfo
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int SubjectGradeId { get; set; }
        public string SubjectGradeName { get; set; }
    }
}