using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QiuYudengMathematics.Models
{
    public class AccountModel
    {
        public string Account { get; set; }
        public string Pwd { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }
        public bool Enable { get; set; }
        public List<int> Subject { get; set; }
    }
    public class AccountQueryModel
    {
        public string Name { get; set; }
        public int? Grade { get; set; }
        public bool Enable { get; set; }
    }
}