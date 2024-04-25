using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Supermarket_Project.AppContext;
using Online_Supermarket_Project.Models;
using X.PagedList;

namespace Online_Supermarket_Project.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MyAppDbContext _db;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(MyAppDbContext db, ILogger<ProductsController> logger)
        {
            _db = db;
            _logger = logger;
        }
        [HttpGet("/sanpham")]
       public IActionResult Index(int? page,int sort=0)
        {
            int pageSize = 6;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<Product> lst;

            if (sort == 1)
            {
                var lstsanpham = _db.Product.AsNoTracking().OrderBy(x => x.Price);
                lst = new PagedList<Product>(lstsanpham, pageNumber, pageSize);


            }
            else if (sort == 2)
            {
                var lstsanpham = _db.Product.AsNoTracking().OrderByDescending(x => x.Price);
                lst = new PagedList<Product>(lstsanpham, pageNumber, pageSize);


            }
            else
            {
                var lstsanpham = _db.Product.AsNoTracking().OrderBy(x => x.ProductName);
                lst = new PagedList<Product>(lstsanpham, pageNumber, pageSize);
            }

            return View(lst);
        }
    }
}
