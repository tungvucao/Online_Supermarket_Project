using Microsoft.AspNetCore.Mvc;

namespace Online_Supermarket_Project.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
