using System.Web.Mvc;

namespace MtHoodMiata.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult General()
        {
            Response.StatusCode = 500;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
    }
}
