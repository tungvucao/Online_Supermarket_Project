using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Supermarket_Project.AppContext;
using Online_Supermarket_Project.Models;

namespace Online_Supermarket_Project.Controllers
{
    public class CusSearchController : Controller
    {
        private readonly MyAppDbContext _context;
        public INotyfService _notyfService { get; set; }
        public CusSearchController(MyAppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult FindProduct(string keyword)
        {
            List<Product> ls = new List<Product>();
            if (string.IsNullOrEmpty(keyword))
            {
                ls = _context.Product.AsNoTracking().Include(a => a.Cate).Where(x=> x.Status == true).Take(3).ToList();
                return PartialView("CusListPoductsSearchPartial", ls);
            }

            ls = _context.Product.AsNoTracking().Include(a => a.Cate).Where(x => x.ProductName.Contains(keyword) && x.Status == true).Take(3).ToList();

            if (ls == null || ls.Count == 0)
            {
                return PartialView("CusListPoductsSearchPartial", null);
            }
            else
            {
                return PartialView("CusListPoductsSearchPartial", ls);
            }

        }

        [HttpPost]
        public IActionResult FindProductByCate(string keyword, int CateId)
        {
            List<Product> ls = new List<Product>();
            if (string.IsNullOrEmpty(keyword))
            {
                ls = _context.Product.AsNoTracking().Include(a => a.Cate).Where(x => x.Status == true && x.CateId == CateId).Take(3).ToList();
                return PartialView("CusListPoductsSearchPartial", ls);
            }

            ls = _context.Product.AsNoTracking().Include(a => a.Cate).Where(x => x.ProductName.Contains(keyword) && x.Status == true && x.CateId == CateId).Take(3).ToList();

            if (ls == null || ls.Count == 0)
            {
                return PartialView("CusListPoductsSearchPartial", null);
            }
            else
            {
                return PartialView("CusListPoductsSearchPartial", ls);
            }

        }
    }
}
