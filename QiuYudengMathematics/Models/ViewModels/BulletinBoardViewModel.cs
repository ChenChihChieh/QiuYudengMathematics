using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QiuYudengMathematics.Models.ViewModels
{
    public class BulletinBoardViewModel
    {
        public int BulletinBoardSeq { get; set; }
        public string Content { get; set; }
        public int SubjectId { get; set; }
        public SubbjectInfo Subject { get; set; }
        public bool Enable { get; set; }
        public List<Comment> Comment { get; set; }
    }
    public class Comment
    {
        public int Seq { get; set; }
        public string Account { get; set; }
        public string AcoountName { get; set; }
        public bool Display { get; set; }
        public string Commentary { get; set; }
        public List<SubComment> SubComment { get; set; }
    }
    public class SubComment
    {
        public int Seq { get; set; }
        public string Account { get; set; }
        public string AcoountName { get; set; }
        public bool Display { get; set; }
        public string Commentary { get; set; }
    }
}