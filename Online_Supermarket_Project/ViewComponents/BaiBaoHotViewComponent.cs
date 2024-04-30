using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Supermarket_Project.AppContext;
using Online_Supermarket_Project.Models;

namespace Online_Supermarket_Project.ViewComponents
{
    public class BaiBaoHotViewComponent : ViewComponent
    {
        private readonly MyAppDbContext _db;
        private readonly List<New> news;
        public BaiBaoHotViewComponent(MyAppDbContext db)
        {
            _db = db;
        }
        public IViewComponentResult Invoke()
        {
            var newestNews = _db.New.AsNoTracking()
                                        .OrderByDescending(x => x.IsHot)
                                        .Take(3)
                                        .ToList();
            return View("Default", newestNews);
        }
    }
}
