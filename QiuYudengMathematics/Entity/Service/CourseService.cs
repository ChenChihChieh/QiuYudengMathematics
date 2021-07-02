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
                        Student = item.Student.Select(y => y.Account).ToList()
                    }).ToList();

                if (model.SubjectId.HasValue)
                    data = data.Where(x => x.SubjectId == model.SubjectId.Value).ToList();
                if (model.Audition)
                    data = data.Where(x => x.Student.Contains(WebSiteComm.CurrentUserAccount)).ToList();
                if (model.Enable.HasValue)
                    data = data.Where(x => x.Enable == model.Enable.Value).ToList();

                return data;
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
                        Student = item.Student.Select(y => y.Account).ToList()
                    }).FirstOrDefault();
        }
        public RtnModel Insert(CourseManagementViewModel model)
        {
            try
            {
                RtnModel rtn = CheckField(model, true);
                if (!rtn.Success) return rtn;
                var FilePath = UploadVideo(model.Video);
                if (string.IsNullOrEmpty(FilePath)) return new RtnModel() { Success = false, Msg = "檔案上傳失敗，請通知工程師" };
                using (var db = new QiuYudengMathematicsEntities())
                {
                    CourseVideo cv = new CourseVideo()
                    {
                        CourseName = model.CourseName,
                        Url = FilePath,
                        SubjectId = model.SubjectId,
                        Enable = model.Enable
                    };
                    if (model.Student != null)
                    {
                        var Student = db.Student.Where(x => x.Enable).ToList();
                        model.Student.ForEach(x =>
                        {
                            if (Student.Where(y => y.Account == x).Any())
                                cv.Student.Add(Student.Where(y => y.Account == x).First());
                        });
                    }
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
                RtnModel rtn = CheckField(model, false);
                if (!rtn.Success) return rtn;
                using (var db = new QiuYudengMathematicsEntities())
                {
                    var Course = db.CourseVideo.Where(x => x.CourseSeq == model.CourseSeq).FirstOrDefault();
                    if (Course != null)
                    {
                        Course.CourseName = model.CourseName;
                        Course.SubjectId = model.SubjectId;
                        Course.Enable = model.Enable;
                        Course.Student.Clear();
                        if (model.Student != null)
                        {
                            var Student = db.Student.Where(x => x.Enable).ToList();
                            model.Student.ForEach(x =>
                            {
                                if (Student.Where(y => y.Account == x).Any())
                                    Course.Student.Add(Student.Where(y => y.Account == x).First());
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
        public RtnModel UpdateVideo(CourseManagementViewModel model)
        {
            try
            {
                RtnModel rtn = new RtnModel();
                if (model.Video.ContentLength == 0) return new RtnModel() { Success = false, Msg = "請上傳影片" };
                if (System.IO.Path.GetExtension(model.Video.FileName).ToUpper() != ".mp4".ToUpper()) return new RtnModel() { Success = false, Msg = "影片格式錯誤，請上傳mp4格式" };
                var FilePath = UploadVideo(model.Video);
                if (string.IsNullOrEmpty(FilePath)) return new RtnModel() { Success = false, Msg = "檔案上傳失敗，請通知工程師" };
                using (var db = new QiuYudengMathematicsEntities())
                {
                    var Course = db.CourseVideo.Where(x => x.CourseSeq == model.CourseSeq).FirstOrDefault();
                    if (Course != null)
                    {
                        Course.Url = FilePath;
                        rtn.Success = db.SaveChanges() > 0;
                        rtn.Msg = "更新成功";
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
        private RtnModel CheckField(CourseManagementViewModel model, bool CheckVedio)
        {
            if (string.IsNullOrEmpty(model.CourseName)) return new RtnModel() { Success = false, Msg = "請輸入名稱" };
            if (CheckVedio)
            {
                if (model.Video.ContentLength == 0) return new RtnModel() { Success = false, Msg = "請上傳影片" };
                if (System.IO.Path.GetExtension(model.Video.FileName).ToUpper() != ".mp4".ToUpper()) return new RtnModel() { Success = false, Msg = "影片格式錯誤，請上傳mp4格式" };
            }
            if (model.SubjectId == 0) return new RtnModel() { Success = false, Msg = "請選擇科目" };
            return new RtnModel() { Success = true, Msg = string.Empty };
        }
        private string UploadVideo(HttpPostedFileBase video)
        {
            try
            {
                var BasePath = ConfigurationManager.AppSettings["VideoPath"].ToString() + DateTime.Now.ToString("yyyyMM");
                if (!System.IO.Directory.Exists(BasePath))
                    System.IO.Directory.CreateDirectory(BasePath);
                var FilePath = DateTime.Now.ToString("yyyyMM") + "\\" + System.IO.Path.GetFileName(video.FileName);
                var FullPath = ConfigurationManager.AppSettings["VideoPath"].ToString() + FilePath;
                if (System.IO.File.Exists(FullPath))
                    System.IO.File.Delete(FullPath);
                video.SaveAs(FullPath);
                return FilePath;
            }
            catch (Exception e)
            {
                logService.Insert(e);
                return string.Empty;
            }
        }
    }
}