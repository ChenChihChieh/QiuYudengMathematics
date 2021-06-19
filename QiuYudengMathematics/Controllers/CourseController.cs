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
        public ActionResult Index(int SubjectId)
        {
            var Student = accountService.SingleQuery(WebSiteComm.CurrentUserAccount);
            if (!Student.Enable)
                return RedirectToAction("LogoutForErrAccount", "Login", new { ErrMsg = "您的帳號已停用" });
            if (!Student.Subject.Where(x => x.ID == SubjectId && x.Detriment).Any())
                return RedirectToAction("Index", "Home");

            return View();
        }
    }
}