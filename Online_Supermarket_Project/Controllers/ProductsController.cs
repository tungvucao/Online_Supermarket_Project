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
        [HttpGet("/Products")]
        public IActionResult Index(int? page, int sort)
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
        public IActionResult TimKiem(string searchTerm, int? page)
        {
            int pageSize = 6;
            int pageNumber = page ?? 1;

            // Lấy danh sách sản phẩm từ cơ sở dữ liệu dựa trên từ khóa tìm kiếm
            var lstsanpham = _db.Product.AsNoTracking().Where(x => x.ProductName.Contains(searchTerm ?? "")).OrderBy(x => x.ProductName);

            // Phân trang cho danh sách sản phẩm
            var lst = new PagedList<Product>(lstsanpham, pageNumber, pageSize);

            return View("Index", lst); // Chuyển đến view Index và truyền danh sách sản phẩm đã tìm kiếm
        }
        
        public IActionResult SanPhamTheoLoai(int maloai, int? page)
        {
            int pageSize = 6;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstsanpham = _db.Product.AsNoTracking().Where(x => x.CateId == maloai).OrderBy(x => x.ProductName);
            PagedList<Product> lst = new PagedList<Product>(lstsanpham, pageNumber, pageSize);
            ViewBag.maloai = maloai;
            return View("Index",lst);
        }
    }
}
