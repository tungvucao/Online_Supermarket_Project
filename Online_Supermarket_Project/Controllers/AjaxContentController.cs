using Microsoft.AspNetCore.Mvc;

namespace Online_Supermarket_Project.Controllers
{
    public class AjaxContentController : Controller
    {
        public IActionResult HeaderCart()
        {
            return ViewComponent("HeaderCart");
        }

        public IActionResult HeaderFavourites()
        {
            return ViewComponent("NumberCart");
        }
    }
}
