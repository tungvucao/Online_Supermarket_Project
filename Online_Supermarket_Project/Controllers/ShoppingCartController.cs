using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Supermarket_Project.AppContext;
using Online_Supermarket_Project.Extension;
using Online_Supermarket_Project.Models;
using Online_Supermarket_Project.ModelView;

namespace Online_Supermarket_Project.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly MyAppDbContext _context;

        public INotyfService _notyfService { get; set; }
        public ShoppingCartController(MyAppDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        
        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }

        [Route("cart.html", Name = "Cart")]
        public IActionResult Index()
        {
            ViewBag.listCate = _context.Category.AsNoTracking().ToList();
            return View(GioHang);
        }

        [HttpPost]
        [Route("api/cart/add")]
        public IActionResult AddToCart(int productID, int? amount)
        {
            try
            {
                List<CartItem> cart = GioHang;
                CartItem? itemt = cart.SingleOrDefault(p => p.Product.ProductId == productID);
                if (itemt != null)
                {
                    itemt.Amount = itemt.Amount + amount.Value;
                    HttpContext.Session.Set<List<CartItem>>("GioHang", cart);

                }
                else
                {
                    Product? hh = _context.Product.SingleOrDefault(x => x.ProductId == productID);
                    itemt = new CartItem
                    {
                        Amount = amount.HasValue ? amount.Value : 1,
                        Product = hh
                    };
                    cart.Add(itemt); 
                }
                HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                _notyfService.Success("Add To Cart Success");
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [Route("api/cart/update")]
        public IActionResult UpdateCart(int productID, int? amount)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            try
            {
                if (cart != null)
                {
                    CartItem? itemt = cart.SingleOrDefault(p => p.Product.ProductId == productID);
                    if(itemt != null && amount.HasValue)
                    {
                        itemt.Amount = amount.Value;
                    }
                    HttpContext.Session.Set<List<CartItem>>("GioHang", cart);

                }
                _notyfService.Success("Update Cart Success");
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [Route("api/cart/remove")]
        public IActionResult Remove(int productID)
        {
            try
            {
                List<CartItem> gioHang = GioHang;
                CartItem? itemt = gioHang.SingleOrDefault(p => p.Product.ProductId == productID);

                if (itemt != null)
                {
                    gioHang.Remove(itemt);
                }
                HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);

                _notyfService.Success("Delete Itemt Success");
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
    }
}
