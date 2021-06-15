﻿using QiuYudengMathematics.Models;
using QiuYudengMathematics.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
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
                var data = db.Student
                    .Select(item => new AccountQueryViewModel()
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
                if (model.Enable.HasValue)
                    data = data.Where(x => x.Enable == model.Enable.Value).ToList();
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
        public RtnModel Insert(AccountModel model)
        {
            RtnModel rtn = new RtnModel();
            using (var db = new QiuYudengMathematicsEntities())
            {
                string Seq = string.Empty;
                var StudentCount = db.Student.Count() + 1;
                Seq = StudentCount <= 999999 ? string.Format("{0:000000}", StudentCount) : StudentCount.ToString();

                while (db.Student.Where(x => x.Account == Seq).Any())
                {
                    StudentCount++;
                    Seq = StudentCount <= 999999 ? string.Format("{0:000000}", StudentCount) : StudentCount.ToString();
                }

                Student s = new Student()
                {
                    Account = Seq,
                    Pwd = new AESComm().AES("12345", true),
                    Name = model.Name,
                    Enable = model.Enable,
                    Grade = model.Grade
                };
                var subjectList = db.GroupGradeSubject.Where(x => x.GradeID == model.Grade && x.Enable).ToList();
                model.Subject.ForEach(x =>
                {
                    if (subjectList.Where(y => y.ID == x).Any())
                        s.GroupGradeSubject.Add(subjectList.Where(y => y.ID == x).First());
                });

                db.Student.Add(s);
                rtn.Success = db.SaveChanges() > 0;
                rtn.Msg = rtn.Success ? "新增成功" : "新增失敗";

                return rtn;
            }
        }
        public RtnModel Update(AccountModel model)
        {
            RtnModel rtn = new RtnModel();
            using (var db = new QiuYudengMathematicsEntities())
            {
                var StudentData = db.Student.Where(x => x.Account == model.Account).FirstOrDefault();
                if (StudentData != null)
                {
                    StudentData.Name = model.Name;
                    StudentData.Enable = model.Enable;
                    StudentData.Grade = model.Grade;
                    StudentData.GroupGradeSubject.Clear();
                    var subjectList = db.GroupGradeSubject.Where(x => x.GradeID == model.Grade && x.Enable).ToList();
                    model.Subject.ForEach(x =>
                    {
                        if (subjectList.Where(y => y.ID == x).Any())
                            StudentData.GroupGradeSubject.Add(subjectList.Where(y => y.ID == x).First());
                    });
                    rtn.Success = db.SaveChanges() > 0;
                    rtn.Msg = rtn.Success ? "更新成功" : "更新失敗";
                    return rtn;
                }
                else
                    return new RtnModel() { Success = false, Msg = "查無資料" };
            }
        }
    }
}