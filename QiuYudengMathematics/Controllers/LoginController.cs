using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using QiuYudengMathematics.Models;
using System.Configuration;
using QiuYudengMathematics.Entity.Service;
using Newtonsoft.Json;
using QiuYudengMathematics.Models.ViewModels;
using System.Net;

namespace QiuYudengMathematics.Controllers
{
    public class LoginController : Controller
    {
        private readonly AccountService AccountService;
        private readonly DeviceService DeviceService;
        public LoginController()
        {
            AccountService = new AccountService();
            DeviceService = new DeviceService();
        }
        public ActionResult Index()
        {
            FormsAuthentication.SignOut();
            return View();
        }
        public ActionResult Login(LoginModel model)
        {
            if (string.IsNullOrEmpty(model.Account))
            {
                TempData["Message"] = "請輸入帳號";
                return View("Index");
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                TempData["Message"] = "請輸入密碼";
                return View("Index");
            }
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
                    if (DeviceService.CheckDevice(Student.Account, GetHostName()))
                    {
                        LoginProcess(model.Account, JsonConvert.SerializeObject(Student));
                        if (Student.PwdReset) //首次登入，導到改密碼頁
                            return RedirectToAction("Index", "PwdReset");
                        else
                            return RedirectToAction("Index", "Home");
                    }
                    else
                        TempData["Message"] = "登入的裝置超過上限";
                }
                else
                    TempData["Message"] = "帳號或密碼錯誤";
                return View("Index");
            }         
        }

        public ActionResult LogoutForErrAccount(string ErrMsg)
        {
            TempData["Message"] = ErrMsg;
            FormsAuthentication.SignOut();
            return View("Index");
        }

        private string GetHostName()
        {
            try
            {
                return Dns.GetHostEntry(Request.UserHostAddress).HostName;
            }
            catch
            {
                return "Phone";
            }
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