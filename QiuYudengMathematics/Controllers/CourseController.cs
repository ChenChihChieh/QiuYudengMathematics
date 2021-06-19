using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QiuYudengMathematics.Comm;
using QiuYudengMathematics.Entity.Service;

namespace QiuYudengMathematics.Controllers
{
    public class CourseController : Controller
    {
        private readonly AccountService accountService;
        public CourseController()
        {
            accountService = new AccountService();
        }
        #region 課程管理
        public ActionResult CourseManagement() => View();

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