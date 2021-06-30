using QiuYudengMathematics.Models;
using QiuYudengMathematics.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using QiuYudengMathematics.Comm;
using System;

namespace QiuYudengMathematics.Entity.Service
{
    public class AccountService
    {
        private readonly LogService logService;
        public AccountService()
        {
            logService = new LogService();
        }
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
                        PwdReset = item.PwdReset,
                        Subject = item.GroupGradeSubject.Select(y => new Subject()
                        {
                            ID = y.ID,
                            SubjectName = y.Subject,
                            GradeId = y.GradeID,
                            GradeName = y.GroupGrade.Grade,
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
            try
            {
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
                        Grade = model.Grade,
                        PwdReset = true
                    };
                    if (model.Subject != null)
                    {
                        var subjectList = db.GroupGradeSubject.Where(x => x.GradeID == model.Grade && x.Enable).ToList();
                        model.Subject.ForEach(x =>
                        {
                            if (subjectList.Where(y => y.ID == x).Any())
                                s.GroupGradeSubject.Add(subjectList.Where(y => y.ID == x).First());
                        });
                    }
                    db.Student.Add(s);
                    rtn.Success = db.SaveChanges() > 0;
                    rtn.Msg = rtn.Success ? "新增成功" : "新增失敗";

                    return rtn;
                }
            }
            catch (Exception e)
            {
                logService.Insert(e);
                return new RtnModel() { Success = false, Msg = "新增發生錯誤，請通知工程師" };
            }
        }
        public RtnModel Update(AccountModel model)
        {
            try
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
                        if (model.Subject != null)
                        {
                            var subjectList = db.GroupGradeSubject.Where(x => x.GradeID == model.Grade && x.Enable).ToList();
                            model.Subject.ForEach(x =>
                            {
                                if (subjectList.Where(y => y.ID == x).Any())
                                    StudentData.GroupGradeSubject.Add(subjectList.Where(y => y.ID == x).First());
                            });
                        }
                        rtn.Success = db.SaveChanges() > 0;
                        rtn.Msg = rtn.Success ? "更新成功" : "更新失敗";
                        return rtn;
                    }
                    else
                        return new RtnModel() { Success = false, Msg = "查無資料" };
                }
            }
            catch (Exception e)
            {
                logService.Insert(e);
                return new RtnModel() { Success = false, Msg = "更新發生錯誤，請通知工程師" };
            }
        }

        /// <summary>
        /// 重置密碼
        /// </summary>
        public RtnModel UpdatePwdReset(string AccountId)
        {
            try
            {
                RtnModel rtn = new RtnModel();
                using (var db = new QiuYudengMathematicsEntities())
                {
                    var StudentData = db.Student.Where(x => x.Account == AccountId).FirstOrDefault();
                    if (StudentData != null)
                    {
                        StudentData.Pwd = new AESComm().AES("12345", true);
                        StudentData.PwdReset = true;
                        db.SaveChanges();
                        return new RtnModel() { Success = true, Msg = "重置成功" }; ;
                    }
                    else
                        return new RtnModel() { Success = false, Msg = "查無資料" };
                }
            }
            catch (Exception e)
            {
                logService.Insert(e);
                return new RtnModel() { Success = false, Msg = "重置發生錯誤，請通知工程師" };
            }
        }
        /// <summary>
        /// 更新密碼
        /// </summary>
        public RtnModel UpdatePwd(string Pwd)
        {
            try
            {
                RtnModel rtn = new RtnModel();
                using (var db = new QiuYudengMathematicsEntities())
                {
                    var StudentData = db.Student.Where(x => x.Account == WebSiteComm.CurrentUserAccount).FirstOrDefault();
                    if (StudentData != null)
                    {
                        StudentData.Pwd = new AESComm().AES(Pwd, true);
                        StudentData.PwdReset = false;
                        rtn.Success = db.SaveChanges() > 0;
                        rtn.Msg = rtn.Success ? "更新成功" : "更新失敗";
                        return rtn;
                    }
                    else
                        return new RtnModel() { Success = false, Msg = "查無資料" };
                }
            }
            catch (Exception e)
            {
                logService.Insert(e);
                return new RtnModel() { Success = false, Msg = "更新發生錯誤，請通知管理人員" };
            }
        }
    }
}