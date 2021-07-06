﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using QiuYudengMathematics.Comm;
using QiuYudengMathematics.Entity.Service;
using QiuYudengMathematics.Models;
using QiuYudengMathematics.Models.ViewModels;
using System.IO;
using System.Configuration;

namespace QiuYudengMathematics.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly AccountService accountService;
        private readonly CourseService courseService;
        private readonly LogService logService;
        public CourseController()
        {
            accountService = new AccountService();
            courseService = new CourseService();
            logService = new LogService();
        }
        #region 課程管理
        public ActionResult CourseManagement() => View(accountService.getGrade());
        public ActionResult Query(int? SubjectId) => Json(new RtnModel() { Success = true, Data = courseService.Query(new CourseModel() { SubjectId = SubjectId, Audition = false, Enable = null }) }, JsonRequestBehavior.AllowGet);
        public ActionResult SingleQuery(int Seq) => Json(new RtnModel() { Success = true, Data = courseService.SingleQuery(Seq) }, JsonRequestBehavior.AllowGet);
        public ActionResult Insert(CourseManagementViewModel model) => Json(courseService.Insert(model), JsonRequestBehavior.AllowGet);
        public ActionResult Update(CourseManagementViewModel model) => Json(courseService.Update(model), JsonRequestBehavior.AllowGet);
        public ActionResult UpdateAuditionStudent(CourseManagementViewModel model) => Json(courseService.UpdateAuditionStudent(model), JsonRequestBehavior.AllowGet);
        #endregion

        #region 課程
        public ActionResult CourseLearn(int SubjectId)
        {
            var Student = accountService.SingleQuery(WebSiteComm.CurrentUserAccount);
            if (Student == null)
                return RedirectToAction("LogoutForErrAccount", "Login", new { ErrMsg = "查無您的帳號，請通知管理人員" });
            if (!Student.Enable)
                return RedirectToAction("LogoutForErrAccount", "Login", new { ErrMsg = "您的帳號已停用" });
            if (!Student.Subject.Where(x => x.ID == SubjectId && x.Detriment).Any() && SubjectId != -1)
                return RedirectToAction("Index", "Home");
            return View(SubjectId);
        }

        public ActionResult CourseQuery(int SubjectId)
        {
            List<CourseManagementViewModel> models = new List<CourseManagementViewModel>();
            if (SubjectId == -1) //試聽課程
                models = courseService.Query(new CourseModel() { SubjectId = null, Audition = true, Enable = true });
            else
                models = courseService.Query(new CourseModel() { SubjectId = SubjectId, Audition = false, Enable = true });
            return Json(new RtnModel() { Data = models }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CourseVideo(int SeqId)
        {
            var Student = accountService.SingleQuery(WebSiteComm.CurrentUserAccount);
            if (Student == null)
                return RedirectToAction("LogoutForErrAccount", "Login", new { ErrMsg = "查無您的帳號，請通知管理人員" });
            if (!Student.Enable)
                return RedirectToAction("LogoutForErrAccount", "Login", new { ErrMsg = "您的帳號已停用" });
            var CourseVideo = courseService.SingleQuery(SeqId);
            if (CourseVideo == null)
                return RedirectToAction("Index", "Home");
            if (!CourseVideo.Enable)
                return RedirectToAction("Index", "Home");
            if (!WebSiteComm.CurrentUserName.Subject.Where(x => x.Detriment && x.ID == CourseVideo.SubjectId).Any() &&
               !CourseVideo.Student.Contains(WebSiteComm.CurrentUserAccount))
                return RedirectToAction("Index", "Home");
            CourseVideo.Url = ConfigurationManager.AppSettings["VideoUrl"].ToString() + CourseVideo.Url;
            return View(CourseVideo);
        }
        #endregion
    }
}