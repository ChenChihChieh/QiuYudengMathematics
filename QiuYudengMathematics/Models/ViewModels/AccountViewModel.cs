using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QiuYudengMathematics.Models.ViewModels
{
    public class AccountViewModel
    {
        public string Account { get; set; }
        public string Pwd { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }
        public bool Enable { get; set; }
        public List<Subject> Subject { get; set; }
    }
    public class Subject
    {
        public int ID { get; set; }
        public string SubjectName { get; set; }
        /// <summary>
        /// 學生是否有購買科目
        /// </summary>
        public bool Detriment { get; set; }
    }
    public class AccountQueryViewModel
    {
        public string Account { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }
        public string GradeName { get; set; }
        public bool Enable { get; set; }
    }
}