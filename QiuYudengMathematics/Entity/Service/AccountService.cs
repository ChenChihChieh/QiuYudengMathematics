using QiuYudengMathematics.Models;
using QiuYudengMathematics.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QiuYudengMathematics.Comm;

namespace QiuYudengMathematics.Entity.Service
{
    public class AccountService
    {
        public Dictionary<int, string> getGrade()
        {
            using (var db = new QiuYudengMathematicsEntities())
                return db.GroupGrade.Where(x => x.Enable).ToDictionary(y => y.ID, z => z.Grade);
        }
        public List<AccountQueryViewModel> Query(AccountQueryModel model)
        {
            using (var db = new QiuYudengMathematicsEntities())
            {
                var data = db.Student.Select(item => new AccountQueryViewModel()
                {
                    Account = item.Account,
                    Name = item.Name,
                    Grade = item.Grade,
                    GradeName = item.GroupGrade.Grade,
                    Enable = item.Enable
                }).ToList();

                if (!string.IsNullOrEmpty(model.Name))
                    data = data.Where(x => x.Name.Contains(model.Name)).ToList();
                if (model.Grade.HasValue)
                    data = data.Where(x => x.Grade == model.Grade.Value).ToList();
                return data;
            }
        }

        public AccountViewModel SingleQuery(string ID)
        {
            using (var db = new QiuYudengMathematicsEntities())
            {
                var data = db.Student.AsEnumerable()
                    .Where(x => x.Account == ID)
                    .Select(item => new AccountViewModel()
                    {
                        Account = item.Account,
                        Pwd = new AESComm().AES(item.Pwd, false),
                        Name = item.Name,
                        Grade = item.Grade,
                        Enable = item.Enable,
                        Subject = item.GroupGradeSubject.Select(y => new Subject()
                        {
                            ID = y.ID,
                            SubjectName = y.Subject,
                            Detriment = true
                        }).ToList()
                    }).FirstOrDefault();

                if (data != null)
                {
                    db.GroupGradeSubject.Where(x => x.Enable && x.GradeID == data.Grade).ToList().ForEach(x =>
                    {
                        if (!data.Subject.Where(y => y.ID == x.ID).Any())
                            data.Subject.Add(new Subject()
                            {
                                ID = x.ID,
                                SubjectName = x.Subject,
                                Detriment = false
                            });
                    });

                    data.Subject = data.Subject.OrderBy(x => x.ID).ToList();
                }

                return data;
            }
        }
    }
}