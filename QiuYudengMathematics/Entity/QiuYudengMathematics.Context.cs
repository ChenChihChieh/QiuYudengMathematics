﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class QiuYudengMathematicsEntities : DbContext
    {
        public QiuYudengMathematicsEntities()
            : base("name=QiuYudengMathematicsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<GroupGrade> GroupGrade { get; set; }
        public DbSet<GroupGradeSubject> GroupGradeSubject { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<StudentPosition> StudentPosition { get; set; }
        public DbSet<sysdiagrams> sysdiagrams { get; set; }
        public DbSet<StudentDevice> StudentDevice { get; set; }
        public DbSet<LogError> LogError { get; set; }
        public DbSet<BoardComment> BoardComment { get; set; }
        public DbSet<BoardSubComment> BoardSubComment { get; set; }
        public DbSet<CourseVIdeoProgress> CourseVIdeoProgress { get; set; }
        public DbSet<CourseVideo> CourseVideo { get; set; }
        public DbSet<BulletinBoard> BulletinBoard { get; set; }
    }
}
