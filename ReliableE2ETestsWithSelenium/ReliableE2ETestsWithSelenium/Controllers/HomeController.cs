using System.Threading;
using System.Web.Mvc;
using ReliableE2ETestsWithSelenium.Infrastructure;

namespace ReliableE2ETestsWithSelenium.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetProducts()
        {
            Thread.Sleep(2000);
            return Json(DB.GetProducts(), JsonRequestBehavior.AllowGet);
        }
    }
}
