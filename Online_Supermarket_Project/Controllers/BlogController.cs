using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Supermarket_Project.AppContext;
using Online_Supermarket_Project.Models;
using PagedList.Core;

namespace Online_Supermarket_Project.Controllers
{
    public class BlogController : Controller
    {
        private readonly MyAppDbContext _context;

        public BlogController(MyAppDbContext context)
        {
            _context = context;
        }

        [Route("blog.html", Name = "Blog")]
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 3;
            var listNews = _context.New
            .AsNoTracking()
            .OrderBy(p => p.CreatedDate);
            PagedList<New> models = new PagedList<New>(listNews, pageNumber, pageSize);
            ViewBag.totalPage = models.PageCount;
            ViewBag.CurrentPage = pageNumber;
            var listCate = _context.Category.AsNoTracking().ToList();
            ViewBag.listCate = listCate;
            return View(models);
        }

        [Route("/tin-tuc/{Alias}-{id}.html", Name = "TinDetail")]
        public IActionResult Detail(int id)
        {
            var tin = _context.New.AsNoTracking().SingleOrDefault(x => x.NewId == id);
            var listCate = _context.Category.AsNoTracking().ToList();
            ViewBag.listCate = listCate;
            if (tin == null)
            {
                return RedirectToAction("Index");
            }
            return View(tin);
        }
    }
}
