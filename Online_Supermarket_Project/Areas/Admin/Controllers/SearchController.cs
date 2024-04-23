using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Supermarket_Project.AppContext;
using Online_Supermarket_Project.Models;

namespace Online_Supermarket_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly MyAppDbContext _context;

        public SearchController(MyAppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult FindProduct(string keyword)
        {
            List<Product> ls = new List<Product>();
            if (string.IsNullOrEmpty(keyword))
            {
                ls = _context.Product.AsNoTracking().Include(a => a.Cate).ToList();
                return PartialView("ListPoductsSearchPartial", ls);
            }
            else
            {
                ls = _context.Product.AsNoTracking().Include(a => a.Cate).Where(x => x.ProductName.Contains(keyword)).Take(20).ToList();
            }

            if (ls == null || ls.Count == 0)
            {
                return PartialView("ListPoductsSearchPartial", null);
            }
            else
            {
                return PartialView("ListPoductsSearchPartial", ls);
            }

        }

        public IActionResult FindCategory(string keyword)
        {
            List<Category> ls = new List<Category>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length == 0)
            {
                ls = _context.Category.AsNoTracking().ToList();
                return PartialView("ListCategoriesSearchPartial", null);
            }
            else
            {
                ls = _context.Category.AsNoTracking().Where(x => x.CateName.Contains(keyword)).Take(20).ToList();
            }

            if (ls == null || ls.Count == 0)
            {
                return PartialView("ListCategoriesSearchPartial", null);
            }
            else
            {
                return PartialView("ListCategoriesSearchPartial", ls);
            }

        }
    }
}
