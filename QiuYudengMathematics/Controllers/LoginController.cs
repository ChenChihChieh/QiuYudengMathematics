using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using QiuYudengMathematics.Comm;
using QiuYudengMathematics.Models;
using System.Configuration;

namespace QiuYudengMathematics.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            FormsAuthentication.SignOut();
            return View();
        }
        public ActionResult Login(LoginModel model)
        {
            if (model.Account == ConfigurationManager.AppSettings["adminAccount"].ToString() && model.Password == ConfigurationManager.AppSettings["adminPwd"].ToString())
            {
                var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: model.Account,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddDays(1),
                isPersistent: false,
                userData: "",
                cookiePath: FormsAuthentication.FormsCookiePath);

                var encryptedTicket = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Message"] = "帳號或密碼錯誤";
                return View("Index");
            }
        }
    }
}