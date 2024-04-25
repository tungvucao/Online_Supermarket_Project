using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Supermarket_Project.AppContext;
using Online_Supermarket_Project.Models;
using X.PagedList;

namespace Online_Supermarket_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyAppDbContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(MyAppDbContext db, ILogger<HomeController> logger)
        {
            _db = db;
            _logger = logger;
        }

        //public IActionResult Index(int? page)
        //{
        //    int pageSize = 6;
        //    int pageNumber=page==null||page<0?1:page.Value;
        //    var lstsanpham = _db.Product.AsNoTracking().OrderBy(x=>x.ProductName);
        //    PagedList<Product> lst = new PagedList<Product>(lstsanpham, pageNumber, pageSize);

        //    return View(lst);
        //}
        public IActionResult Index(int? page)
        {
            int pageSize = 4; // Số lượng sản phẩm trên mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại, mặc định là trang 1 nếu không có giá trị page

            var lstsanpham = _db.Product.AsNoTracking().OrderBy(x => x.ProductName);
            var pagedProduct = lstsanpham.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var totalProducts = lstsanpham.Count(); // Tổng số sản phẩm
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize); // Tổng số trang

            var lst = new StaticPagedList<Product>(pagedProduct, pageNumber, pageSize, totalProducts);

            return View(lst);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}

