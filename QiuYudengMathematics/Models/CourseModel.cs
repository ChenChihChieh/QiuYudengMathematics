using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QiuYudengMathematics.Models
{
    public class CourseModel
    {
        /// <summary>
        /// 科目
        /// </summary>
        public int? SubjectId { get; set; }
        /// <summary>
        /// 試聽課程(True:查詢帳號被授權的課程)
        /// </summary>
        public bool Audition { get; set; }
        public bool? Enable { get; set; }
    }

    public class CourseProgressModel
    {
        public string Account { get; set; }
        public int CourseSeq { get; set; }
        public decimal Progress { get; set; }
    }
}