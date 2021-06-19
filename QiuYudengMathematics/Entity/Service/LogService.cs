using System;
using QiuYudengMathematics.Extension;

namespace QiuYudengMathematics.Entity.Service
{
    public class LogService
    {
        public void Insert(Exception e)
        {
            using (var db = new QiuYudengMathematicsEntities())
            {
                db.LogError.Add(new LogError()
                {
                    ErrMsg = e.GetErrorMsg(),
                    StackTrace = e.StackTrace,
                    CreateDate = DateTime.Now
                });

                db.SaveChanges();
            }
        }
    }
}