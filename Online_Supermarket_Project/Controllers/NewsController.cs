using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Supermarket_Project.AppContext;
using Online_Supermarket_Project.Models;
using X.PagedList; // Thêm namespace này
using System.Linq;

namespace Online_Supermarket_Project.Controllers
{
    public class NewsController : Controller
    {
        private readonly MyAppDbContext _db;
        private readonly ILogger<NewsController> _logger;
        public NewsController(MyAppDbContext db, ILogger<NewsController> logger)
        {
            _db = db;
            _logger = logger;
        }
        [HttpGet("/News")]
        public IActionResult Index(int? page)
        {
            int pageSize = 6; // Số lượng sản phẩm trên mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại, mặc định là trang 1 nếu không có giá trị page

            var lstnews = _db.New.AsNoTracking().OrderBy(x => x.Title);
            var pagedNews = lstnews.ToPagedList(pageNumber, pageSize); // Sử dụng ToPagedList() để tạo IPagedList<New>

            return View(pagedNews); // Trả về IPagedList<New> thay vì StaticPagedList<New>
        }
        public IActionResult ChiTietNews(int maNews)
        {
            var pr = _db.New.Where(x => x.NewId == maNews).FirstOrDefault();
            if (pr == null)
            {
                return View("Erron");
            }
            return View(pr);
        }
    }
}
