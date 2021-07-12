using QiuYudengMathematics.Entity.Service;
using QiuYudengMathematics.Models;
using System.Web.Mvc;
using QiuYudengMathematics.Comm;
using System.Configuration;
using System.Linq;

namespace QiuYudengMathematics.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly BulletinBoardService bulletinBoardService;
        private readonly CommentService commentService;
        public HomeController()
        {
            bulletinBoardService = new BulletinBoardService();
            commentService = new CommentService();
        }
        public ActionResult Index() => View();
        public ActionResult QueryBulletinBoard()
        {
            BulletinBoardModel model = new BulletinBoardModel() { Enable = true };
            if (WebSiteComm.CurrentUserAccount != ConfigurationManager.AppSettings["adminAccount"].ToString())
                model.SubjectId = WebSiteComm.CurrentUserName.Subject.Where(x => x.Detriment).Select(y => y.ID).ToList();
            return Json(new RtnModel()
            {
                Success = true,
                Data = bulletinBoardService.Query(model).OrderBy(x => x.SubjectId).ToList()
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InsertComment(CommentModel model) => Json(commentService.InsertComment(model), JsonRequestBehavior.AllowGet);
        public ActionResult InsertSubComment(CommentModel model) => Json(commentService.InsertSubComment(model), JsonRequestBehavior.AllowGet);
    }
}