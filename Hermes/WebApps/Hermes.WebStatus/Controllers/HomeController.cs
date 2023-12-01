using Microsoft.AspNetCore.Mvc;

namespace Hermes.WebStatus.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("/healthchecks-ui");
        }
    }
}