using Microsoft.AspNetCore.Mvc;
using WebApplication1.Authorization;

namespace WebApplication1.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Permission("Test Permission")]
        public IActionResult TestAction()
        {
            return View();
        }
    }
}
