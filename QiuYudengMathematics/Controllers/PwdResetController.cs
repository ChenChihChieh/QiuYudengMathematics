using QiuYudengMathematics.Entity.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QiuYudengMathematics.Controllers
{
    [Authorize]
    public class PwdResetController : Controller
    {
        private readonly AccountService AccountService;
        public PwdResetController()
        {
            AccountService = new AccountService();
        }
        public ActionResult Index() => View();

        public ActionResult Reset(string Password)
        {
            AccountService.UpdatePwd(Password);
            return RedirectToAction("Index", "Home");
        }
    }
}