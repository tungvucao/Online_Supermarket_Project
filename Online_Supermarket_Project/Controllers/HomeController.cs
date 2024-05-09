using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Supermarket_Project.AppContext;

namespace Online_Supermarket_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyAppDbContext _context;
        public HomeController(ILogger<HomeController> logger, MyAppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var listCate = _context.Category.AsNoTracking().ToList();
            ViewBag.listCate = listCate;
            ViewBag.listPro = _context.Product.AsNoTracking().Include(c => c.Cate).Where(p => p.Status == true && ((p.Price-p.Discount)/100)*100 > 30).ToList();
            ViewBag.listProNew = _context.Product.AsNoTracking().Include(c => c.Cate).Where(p => p.Status == true).OrderByDescending(p=>p.CreatedDate).ToList();
            return View();
        }

        public IActionResult Contact()
        {
            var listCate = _context.Category.AsNoTracking().ToList();
            ViewBag.listCate = listCate;
            return View();
        }
        public IActionResult About()
        {
            var listCate = _context.Category.AsNoTracking().ToList();
            ViewBag.listCate = listCate;
            return View();
        }
       
    }
}
