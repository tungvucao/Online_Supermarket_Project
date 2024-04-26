using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Supermarket_Project.AppContext;
using Microsoft.AspNetCore.Mvc;
using Online_Supermarket_Project.Models;

namespace Online_Supermarket_Project.ViewComponents
{
    public class SanPhamMoiViewComponent : ViewComponent
    {
        private readonly MyAppDbContext _db;
        private readonly List<Product> products;
        public SanPhamMoiViewComponent(MyAppDbContext db)
        {
            _db = db;
        }
        public IViewComponentResult Invoke()
        {
            var newestProducts = _db.Product.AsNoTracking()
                                        .OrderByDescending(x => x.CreatedDate)
                                        .Take(3)
                                        .ToList();
            return View("Default",newestProducts);
        }
    }
}
