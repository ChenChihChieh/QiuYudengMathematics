using QiuYudengMathematics.Models;
using QiuYudengMathematics.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Configuration;

namespace QiuYudengMathematics.Entity.Service
{
    public class BulletinBoardService
    {
        private readonly LogService logService;
        public BulletinBoardService()
        {
            logService = new LogService();
        }
        public List<BulletinBoardViewModel> Query(BulletinBoardModel model)
        {
            using (var db = new QiuYudengMathematicsEntities())
            {
                var Students = db.Student.Where(x => x.Enable).ToList();
                var data = db.BulletinBoard
                    .AsEnumerable()
                    .Select(item => new BulletinBoardViewModel()
                    {
                        BulletinBoardSeq = item.BulletinBoardSeq,
                        Content = item.Content,
                        SubjectId = item.SubjectId,
                        Subject = new SubbjectInfo()
                        {
                            SubjectId = item.SubjectId,
                            SubjectGradeName = item.GroupGradeSubject.GroupGrade.Grade,
                            SubjectGradeId = item.GroupGradeSubject.GradeID,
                            SubjectName = item.GroupGradeSubject.Subject
                        },
                        Enable = item.Enable,
                        Comment = item.BoardComment.Select(c => new Comment()
                        {
                            Seq = c.Seq,
                            Account = c.Account,
                            AcoountName = ConfigurationManager.AppSettings["adminAccount"].ToString() == c.Account ?
                                                "管理員" :
                                                (Students.Where(s => s.Account == c.Account).Any() ? Students.Where(s => s.Account == c.Account).First().Name : ""),
                            Display = ConfigurationManager.AppSettings["adminAccount"].ToString() == c.Account || (Students.Where(s => s.Account == c.Account).Any() ? true : false),
                            Commentary = c.Comment,
                            SubComment = c.BoardSubComment.Select(cs => new SubComment()
                            {
                                Seq = cs.Seq,
                                Account = cs.Account,
                                AcoountName = ConfigurationManager.AppSettings["adminAccount"].ToString() == cs.Account ?
                                                "管理員" :
                                                (Students.Where(s => s.Account == cs.Account).Any() ? Students.Where(s => s.Account == cs.Account).First().Name : ""),
                                Display = ConfigurationManager.AppSettings["adminAccount"].ToString() == cs.Account || (Students.Where(s => s.Account == cs.Account).Any() ? true : false),
                                Commentary = cs.Comment
                            }).ToList()
                        }).ToList()
                    }).ToList();

                if (model.SubjectId != null && model.SubjectId.Count() > 0)
                    data = data.Where(x => model.SubjectId.Contains(x.SubjectId)).ToList();
                if (model.Enable.HasValue)
                    data = data.Where(x => x.Enable == model.Enable.Value).ToList();

                return data;
            }
        }
        public BulletinBoardViewModel SingleQuery(int Seq)
        {
            using (var db = new QiuYudengMathematicsEntities())
                return db.BulletinBoard
                    .Where(x => x.BulletinBoardSeq == Seq)
                    .AsEnumerable()
                    .Select(item => new BulletinBoardViewModel()
                    {
                        BulletinBoardSeq = item.BulletinBoardSeq,
                        Content = item.Content,
                        SubjectId = item.SubjectId,
                        Subject = new SubbjectInfo()
                        {
                            SubjectId = item.SubjectId,
                            SubjectGradeName = item.GroupGradeSubject.GroupGrade.Grade,
                            SubjectGradeId = item.GroupGradeSubject.GradeID,
                            SubjectName = item.GroupGradeSubject.Subject
                        },
                        Enable = item.Enable
                    }).FirstOrDefault();
        }
        public RtnModel Insert(BulletinBoardViewModel model)
        {
            try
            {
                RtnModel rtn = CheckField(model);
                if (!rtn.Success) return rtn;
                using (var db = new QiuYudengMathematicsEntities())
                {
                    db.BulletinBoard.Add(new BulletinBoard()
                    {
                        Content = model.Content,
                        SubjectId = model.SubjectId,
                        Enable = model.Enable
                    });
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
        public RtnModel Update(BulletinBoardViewModel model)
        {
            try
            {
                RtnModel rtn = CheckField(model);
                if (!rtn.Success) return rtn;
                using (var db = new QiuYudengMathematicsEntities())
                {
                    var data = db.BulletinBoard.Where(x => x.BulletinBoardSeq == model.BulletinBoardSeq).FirstOrDefault();
                    if (data != null)
                    {
                        data.Content = model.Content;
                        data.SubjectId = model.SubjectId;
                        data.Enable = model.Enable;
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
        private RtnModel CheckField(BulletinBoardViewModel model)
        {
            if (string.IsNullOrEmpty(model.Content)) return new RtnModel() { Success = false, Msg = "請輸入內容" };
            if (model.SubjectId == 0) return new RtnModel() { Success = false, Msg = "請選擇科目" };
            return new RtnModel() { Success = true, Msg = string.Empty };
        }
    }
}