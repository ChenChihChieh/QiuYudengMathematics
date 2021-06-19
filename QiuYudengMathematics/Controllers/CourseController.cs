﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QiuYudengMathematics.Comm;
using QiuYudengMathematics.Entity.Service;
using QiuYudengMathematics.Models;
using QiuYudengMathematics.Models.ViewModels;

namespace QiuYudengMathematics.Controllers
{
    public class CourseController : Controller
    {
        private readonly AccountService accountService;
        private readonly CourseService courseService;
        public CourseController()
        {
            accountService = new AccountService();
            courseService = new CourseService();
        }
        #region 課程管理
        public ActionResult CourseManagement() => View(accountService.getGrade());
        public ActionResult Query(int? SubjectId) => Json(new RtnModel() { Success = true, Data = courseService.Query(SubjectId) }, JsonRequestBehavior.AllowGet);
        public ActionResult SingleQuery(int Seq) => Json(new RtnModel() { Success = true, Data = courseService.SingleQuery(Seq) }, JsonRequestBehavior.AllowGet);
        public ActionResult Insert(CourseManagementViewModel model) => Json(courseService.Insert(model), JsonRequestBehavior.AllowGet);
        public ActionResult Update(CourseManagementViewModel model) => Json(courseService.Update(model), JsonRequestBehavior.AllowGet);
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

            return View();
        }
        #endregion
    }
}