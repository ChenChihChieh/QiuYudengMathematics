using QiuYudengMathematics.Entity.Service;
using QiuYudengMathematics.Models;
using QiuYudengMathematics.Models.ViewModels;
using System.Web.Mvc;

namespace QiuYudengMathematics.Controllers
{
    [Authorize]
    public class BulletinBoardController : Controller
    {
        private readonly BulletinBoardService bulletinBoardService;
        public BulletinBoardController()
        {
            bulletinBoardService = new BulletinBoardService();
        }
        public ActionResult Index() => View();
        public ActionResult Query(BulletinBoardModel model) => Json(new RtnModel() { Success = true, Data = bulletinBoardService.Query(model) }, JsonRequestBehavior.AllowGet);
        public ActionResult SingleQuery(int Seq) => Json(new RtnModel() { Success = true, Data = bulletinBoardService.SingleQuery(Seq) }, JsonRequestBehavior.AllowGet);
        public ActionResult Insert(BulletinBoardViewModel model) => Json(bulletinBoardService.Insert(model), JsonRequestBehavior.AllowGet);
        public ActionResult Update(BulletinBoardViewModel model) => Json(bulletinBoardService.Update(model), JsonRequestBehavior.AllowGet);
    }
}