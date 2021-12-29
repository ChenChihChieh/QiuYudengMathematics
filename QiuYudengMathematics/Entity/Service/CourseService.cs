using QiuYudengMathematics.Comm;
using QiuYudengMathematics.Models;
using QiuYudengMathematics.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace QiuYudengMathematics.Entity.Service
{
    public class CourseService
    {
        private readonly LogService logService;
        public CourseService()
        {
            logService = new LogService();
        }
        public List<CourseManagementViewModel> Query(CourseModel model)
        {
            using (var db = new QiuYudengMathematicsEntities())
            {
                var studentList = db.Student.Where(x => x.Enable).ToList();
                var data = db.CourseVideo.AsEnumerable()
                    .Select(item => new CourseManagementViewModel()
                    {
                        CourseSeq = item.CourseSeq,
                        CourseName = item.CourseName,
                        Url = item.Url,
                        SubjectId = item.SubjectId,
                        SubbjectInfo = new SubbjectInfo()
                        {
                            SubjectId = item.SubjectId,
                            SubjectGradeName = item.GroupGradeSubject.GroupGrade.Grade,
                            SubjectGradeId = item.GroupGradeSubject.GradeID,
                            SubjectName = item.GroupGradeSubject.Subject,
                        },
                        Enable = item.Enable,
                        Student = item.Student.Select(y => y.Account).ToList(),
                        CourseDate = item.CourseDate,
                        CourseDateStr = item.CourseDate.HasValue ? item.CourseDate.Value.ToString("yyyy/MM/dd") : string.Empty
                    }).ToList();

                if (model.SubjectId.HasValue)
                    data = data.Where(x => x.SubjectId == model.SubjectId.Value).ToList();
                if (model.Audition)
                    data = data.Where(x => x.Student.Contains(WebSiteComm.CurrentUserAccount)).ToList();
                if (model.Enable.HasValue)
                    data = data.Where(x => x.Enable == model.Enable.Value).ToList();

                return data.OrderBy(x => x.CourseDate).ThenBy(y => y.CourseSeq).ToList();
            }
        }
        public CourseManagementViewModel SingleQuery(int Seq)
        {
            using (var db = new QiuYudengMathematicsEntities())
                return db.CourseVideo.AsEnumerable().Where(x => x.CourseSeq == Seq)
                    .Select(item => new CourseManagementViewModel()
                    {
                        CourseSeq = item.CourseSeq,
                        CourseName = item.CourseName,
                        Url = item.Url,
                        SubjectId = item.SubjectId,
                        SubbjectInfo = new SubbjectInfo()
                        {
                            SubjectId = item.SubjectId,
                            SubjectName = item.GroupGradeSubject.Subject,
                            SubjectGradeId = item.GroupGradeSubject.GradeID,
                            SubjectGradeName = item.GroupGradeSubject.GroupGrade.Grade
                        },
                        Enable = item.Enable,
                        Student = item.Student.Select(y => y.Account).ToList(),
                        CourseDate = item.CourseDate,
                        CourseDateStr = item.CourseDate.HasValue ? item.CourseDate.Value.ToString("yyyy/MM/dd") : string.Empty
                    }).FirstOrDefault();
        }
        public List<string> QueryProgress(int CourseSeq)
        {
            using (var db = new QiuYudengMathematicsEntities())
            {
                List<string> str = new List<string>();
                var cv = db.CourseVideo.Where(x => x.CourseSeq == CourseSeq).FirstOrDefault();
                if (cv != null)
                {
                    var student = db.Student.Where(x => x.Enable && x.GroupGradeSubject.Where(y => y.ID == cv.SubjectId).Any()).ToList();
                    student.ForEach(x =>
                    {
                        var CourseVideoProgress = db.CourseVIdeoProgress.Where(y => y.Account == x.Account && y.CourseSeq == cv.CourseSeq).FirstOrDefault();
                        str.Add(string.Format("{0}-{1},進度:{2}", x.Account, x.Name, CourseVideoProgress == null ? "0%" : Math.Ceiling(CourseVideoProgress.Progress * 100).ToString() + "%"));
                    });
                    if (cv.Student.Count() > 0) str.Add("以下為試聽學生：");
                    //試聽的學生進度
                    cv.Student.ToList().ForEach(x =>
                    {
                        var CourseVideoProgress = db.CourseVIdeoProgress.Where(y => y.Account == x.Account && y.CourseSeq == cv.CourseSeq).FirstOrDefault();
                        str.Add(string.Format("{0}-{1},進度:{2}", x.Account, x.Name, CourseVideoProgress == null ? "0%" : Math.Ceiling(CourseVideoProgress.Progress * 100).ToString() + "%"));
                    });
                }
                return str;
            }
        }
        public RtnModel Insert(CourseManagementViewModel model)
        {
            try
            {
                RtnModel rtn = CheckField(model);
                if (!rtn.Success) return rtn;
                using (var db = new QiuYudengMathematicsEntities())
                {
                    CourseVideo cv = new CourseVideo()
                    {
                        CourseName = model.CourseName,
                        Url = model.Url,
                        SubjectId = model.SubjectId,
                        Enable = model.Enable,
                        CourseDate = model.CourseDate
                    };
                    db.CourseVideo.Add(cv);
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
        public RtnModel Update(CourseManagementViewModel model)
        {
            try
            {
                RtnModel rtn = CheckField(model);
                if (!rtn.Success) return rtn;
                using (var db = new QiuYudengMathematicsEntities())
                {
                    var Course = db.CourseVideo.Where(x => x.CourseSeq == model.CourseSeq).FirstOrDefault();
                    if (Course != null)
                    {
                        Course.CourseName = model.CourseName;
                        Course.SubjectId = model.SubjectId;
                        Course.Url = model.Url;
                        Course.Enable = model.Enable;
                        Course.CourseDate = model.CourseDate;
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
        public RtnModel UpdateAuditionStudent(CourseManagementViewModel model)
        {
            try
            {
                RtnModel rtn = new RtnModel();
                List<string> NoStudentData = new List<string>();
                using (var db = new QiuYudengMathematicsEntities())
                {
                    var Course = db.CourseVideo.Where(x => x.CourseSeq == model.CourseSeq).FirstOrDefault();
                    if (Course != null)
                    {
                        Course.Student.Clear();
                        if (model.Student != null)
                        {
                            var Student = db.Student.Where(x => x.Enable).ToList();
                            model.Student.ForEach(x =>
                            {
                                if (Student.Where(y => y.Account == x).Any())
                                    Course.Student.Add(Student.Where(y => y.Account == x).First());
                                else
                                    NoStudentData.Add(x);
                            });
                        }
                        db.SaveChanges();
                        rtn.Success = true;
                        rtn.Msg = "更新成功" + (NoStudentData.Count == 0 ? string.Empty : string.Format("\r\n{0},帳號無資料，不新增。", string.Join(",", NoStudentData)));
                        return rtn;
                    }
                    else
                        return new RtnModel() { Success = false, Msg = "查無資料" };
                }
            }
            catch (Exception e)
            {
                logService.Insert(e);
                return new RtnModel() { Success = false, Msg = "更新影片錯誤，請通知工程師" };
            }
        }
        private RtnModel CheckField(CourseManagementViewModel model)
        {
            if (string.IsNullOrEmpty(model.CourseName)) return new RtnModel() { Success = false, Msg = "請輸入名稱" };
            if (string.IsNullOrEmpty(model.Url)) return new RtnModel() { Success = false, Msg = "請輸入連結" };
            if (!Uri.TryCreate(model.Url, UriKind.Absolute, out var u)) return new RtnModel() { Success = false, Msg = "連結格式錯誤" };
            if (model.SubjectId == 0) return new RtnModel() { Success = false, Msg = "請選擇科目" };
            if (!model.CourseDate.HasValue) return new RtnModel() { Success = false, Msg = "請輸入日期" };
            return new RtnModel() { Success = true, Msg = string.Empty };
        }

        public RtnModel UpdateProgress(CourseProgressModel model)
        {
            try
            {
                RtnModel rtn = new RtnModel();
                using (var db = new QiuYudengMathematicsEntities())
                {
                    var VideoProgress = db.CourseVIdeoProgress.Where(x => x.Account == model.Account && x.CourseSeq == model.CourseSeq).FirstOrDefault();
                    if (VideoProgress == null)
                        db.CourseVIdeoProgress.Add(new CourseVIdeoProgress()
                        {
                            Account = model.Account,
                            CourseSeq = model.CourseSeq,
                            Progress = model.Progress
                        });
                    else
                    {
                        if (VideoProgress.Progress < model.Progress)
                            VideoProgress.Progress = model.Progress;
                    }
                    rtn.Success = db.SaveChanges() > 0;
                    rtn.Msg = rtn.Success ? "成功" : "失敗";
                    return rtn;
                }
            }
            catch (Exception e)
            {
                logService.Insert(e);
                return new RtnModel() { Success = false, Msg = "更新錯誤，請通知工程師" };
            }
        }
    }
}