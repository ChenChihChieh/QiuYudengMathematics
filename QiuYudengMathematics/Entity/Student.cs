//------------------------------------------------------------------------------
// <auto-generated>
//    這個程式碼是由範本產生。
//
//    對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//    如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace QiuYudengMathematics.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Student
    {
        public Student()
        {
            this.StudentPosition = new HashSet<StudentPosition>();
            this.GroupGradeSubject = new HashSet<GroupGradeSubject>();
            this.StudentDevice = new HashSet<StudentDevice>();
            this.CourseVideo = new HashSet<CourseVideo>();
            this.CourseVIdeoProgress = new HashSet<CourseVIdeoProgress>();
        }
    
        public string Account { get; set; }
        public string Pwd { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }
        public bool Enable { get; set; }
        public bool PwdReset { get; set; }
    
        public virtual GroupGrade GroupGrade { get; set; }
        public virtual ICollection<StudentPosition> StudentPosition { get; set; }
        public virtual ICollection<GroupGradeSubject> GroupGradeSubject { get; set; }
        public virtual ICollection<StudentDevice> StudentDevice { get; set; }
        public virtual ICollection<CourseVideo> CourseVideo { get; set; }
        public virtual ICollection<CourseVIdeoProgress> CourseVIdeoProgress { get; set; }
    }
}
