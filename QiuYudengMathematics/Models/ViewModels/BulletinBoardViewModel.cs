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
    }
}