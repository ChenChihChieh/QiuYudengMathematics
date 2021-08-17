using QiuYudengMathematics.Entity.Service;
using QiuYudengMathematics.Filter;
using QiuYudengMathematics.Models;
using System.Web.Mvc;

namespace QiuYudengMathematics.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly AccountService AccountService;
        private readonly SubjectService SubjectService;
        private readonly DeviceService DeviceService;
        public AccountController()
        {
            AccountService = new AccountService();
            SubjectService = new SubjectService();
            DeviceService = new DeviceService();
        }
        [RoleFilters]
        public ActionResult Index() => View(SubjectService.getGradeSubject());
        public ActionResult Query(AccountQueryModel model) => Json(new RtnModel() { Success = true, Data = AccountService.Query(model) }, JsonRequestBehavior.AllowGet);
        public ActionResult SingleQuery(string Id) => Json(new RtnModel() { Success = true, Data = AccountService.SingleQuery(Id) }, JsonRequestBehavior.AllowGet);
        public ActionResult GetGradeSubject() => Json(new RtnModel() { Success = true, Data = SubjectService.getGradeSubject() }, JsonRequestBehavior.AllowGet);
        public ActionResult Insert(AccountModel model) => Json(AccountService.Insert(model), JsonRequestBehavior.AllowGet);
        public ActionResult Update(AccountModel model) => Json(AccountService.Update(model), JsonRequestBehavior.AllowGet);
        public ActionResult PwdReset(string Id) => Json(AccountService.UpdatePwdReset(Id), JsonRequestBehavior.AllowGet);
        public ActionResult DeleteDevice(string Id) => Json(DeviceService.DeleteDevice(Id), JsonRequestBehavior.AllowGet);
    }
}