using System.Web.Mvc;

namespace QiuYudengMathematics.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index() => View();
    }
}