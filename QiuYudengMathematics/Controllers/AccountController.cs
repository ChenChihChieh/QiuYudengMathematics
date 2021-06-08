using QiuYudengMathematics.Entity.Service;
using QiuYudengMathematics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QiuYudengMathematics.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private AccountService AccountService;
        public AccountController()
        {
            AccountService = new AccountService();
        }
        public ActionResult Index() => View(AccountService.getGrade());

        public ActionResult Query(AccountQueryModel model) => Json(new RtnModel() { Success = true, Data = AccountService.Query(model) }, JsonRequestBehavior.AllowGet);

        public ActionResult SingleQuery(string Id) => Json(new RtnModel() { Success = true, Data = AccountService.SingleQuery(Id) }, JsonRequestBehavior.AllowGet);

        public ActionResult Insert(AccountModel model) => Json(AccountService.Insert(model), JsonRequestBehavior.AllowGet);

        public ActionResult Update(AccountModel model) => Json(AccountService.Update(model), JsonRequestBehavior.AllowGet);
    }
}