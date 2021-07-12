using QiuYudengMathematics.Entity.Service;
using QiuYudengMathematics.Models;
using System.Web.Mvc;
using QiuYudengMathematics.Comm;
using System.Configuration;
using System.Linq;
using System.Web;

namespace QiuYudengMathematics.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly BulletinBoardService bulletinBoardService;
        public HomeController()
        {
            bulletinBoardService = new BulletinBoardService();
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
        public ActionResult DownLoad(int Seq)
        {
            var FilePath = ConfigurationManager.AppSettings["BulletinBoardFilePath"].ToString() + bulletinBoardService.SingleQuery(Seq).FilePath;
            var FileName = System.IO.Path.GetFileName(FilePath);
            if (System.IO.File.Exists(FilePath))
                return File(FilePath, MimeMapping.GetMimeMapping(FileName), FileName);
            else
                return Content("查無檔案，請通知管理人員");
        }
    }
}