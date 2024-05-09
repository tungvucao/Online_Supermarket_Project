using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Supermarket_Project.AppContext;
using Online_Supermarket_Project.Models;
using PagedList.Core;
using static Azure.Core.HttpHeader;

namespace Online_Supermarket_Project.Controllers
{
    public class ProductController : Controller
    {
        private readonly MyAppDbContext _context;

        public ProductController(MyAppDbContext context)
        {
            _context = context;
        }
        [Route("/shop.html", Name = "ShopProduct")]

        public IActionResult Index(int? page, string sort = "")
        {
            try
            {
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = 3;
                List<Product> listPro = new List<Product>();
                if(!string.IsNullOrEmpty(sort) )
                {
                    switch (sort)
                    {
                        case "nameasc":
                            listPro = _context.Product.AsNoTracking().Include(c => c.Cate).Where(p => p.Status == true)
                                .OrderBy(p => p.ProductName).ToList();
                            break;
                        case "namedesc":
                            listPro = _context.Product.AsNoTracking().Include(c => c.Cate).Where(p => p.Status == true)
                                .OrderByDescending(p => p.ProductName).ToList();
                            break;
                        case "pricedesc":
                            listPro = _context.Product.AsNoTracking().Include(c => c.Cate).Where(p => p.Status == true)
                                .OrderByDescending(p => p.Price).ToList();
                            break;
                        case "priceasc":
                            listPro = _context.Product.AsNoTracking().Include(c => c.Cate).Where(p => p.Status == true)
                                .OrderBy(p => p.Price).ToList();
                            break;
                    }
                }
                else
                {
                    listPro = _context.Product.AsNoTracking().Include(c => c.Cate).Where(p => p.Status == true)
                                .OrderBy(p => p.CreatedDate).ToList();
                }

                var listCate = _context.Category.AsNoTracking().ToList();
                ViewBag.listCate = listCate;
                PagedList<Product> models = new PagedList<Product>(listPro.AsQueryable(), pageNumber, pageSize);
                ViewBag.totalPage = models.PageCount;
                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult Filter(string SelectedValue = "")
        {
            var url = $"/shop.html?sort={SelectedValue}";
            if (string.IsNullOrEmpty(SelectedValue))
            {
                url = $"/shop.html";
            }

            return Json(new { status = "success", redirectUrl = url });
        }

        [Route("/{Alias}", Name = "ListProductByCate")]
        public IActionResult ListByCate(string Alias, int? page)
        {
            try
            {
                var cate = _context.Category.AsNoTracking().SingleOrDefault(x => x.Alias == Alias);
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = 3;
                var listPro = _context.Product
                    .AsNoTracking()
                    .Where(x => x.CateId == cate.CateId)
                    .OrderBy(p => p.CreatedDate);
                var listCate = _context.Category.AsNoTracking().ToList();
                PagedList<Product> models = new PagedList<Product>(listPro, pageNumber, pageSize);
                ViewBag.totalPage = models.PageCount;
                ViewBag.listCate = listCate;
                ViewBag.CurrentPage = pageNumber;
                ViewBag.CurrentCate = cate;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("/{Alias}-{id}.html", Name = "ProductDetail")]
        public IActionResult Detail(int id)
        {
            try
            {
                var product = _context.Product.Include(p => p.Cate).FirstOrDefault(p => p.ProductId == id);
                if (product == null)
                {
                    return RedirectToAction("Index");
                }
                var listPro = _context.Product.AsNoTracking()
                    .Where(p => p.CateId == product.CateId && p.ProductId != id && p.Status == true)
                    .OrderBy(p => p.CateId).ToList();
                ViewBag.SamePro = listPro;
                var listCate = _context.Category.AsNoTracking().ToList();
                ViewBag.listCate = listCate;
                return View(product);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
    
}
