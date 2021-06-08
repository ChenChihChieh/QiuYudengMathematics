using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using QiuYudengMathematics.Comm;
using QiuYudengMathematics.Models;
using System.Configuration;
using QiuYudengMathematics.Entity.Service;
using Newtonsoft.Json;
using QiuYudengMathematics.Models.ViewModels;

namespace QiuYudengMathematics.Controllers
{
    public class LoginController : Controller
    {
        private AccountService AccountService;
        public LoginController()
        {
            AccountService = new AccountService();
        }
        public ActionResult Index()
        {
            FormsAuthentication.SignOut();
            return View();
        }
        public ActionResult Login(LoginModel model)
        {
            if (model.Account == ConfigurationManager.AppSettings["adminAccount"].ToString() && model.Password == ConfigurationManager.AppSettings["adminPwd"].ToString())
            {
                LoginProcess(model.Account, JsonConvert.SerializeObject(new AccountViewModel()
                {
                    Account = model.Account,
                    Name = "系統管理員"
                }));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var Student = AccountService.SingleQuery(model.Account);
                if (Student != null && model.Password == Student.Pwd && Student.Enable)
                {
                    LoginProcess(model.Account, JsonConvert.SerializeObject(Student));
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["Message"] = "帳號或密碼錯誤";
            return View("Index");
        }
        private void LoginProcess(string Account, string Data)
        {
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: Account,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddDays(1),
                isPersistent: false,
                userData: Data,
                cookiePath: FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(cookie);
        }
    }
}