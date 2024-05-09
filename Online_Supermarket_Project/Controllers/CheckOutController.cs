using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Supermarket_Project.AppContext;
using Online_Supermarket_Project.Extension;
using Online_Supermarket_Project.Helpper;
using Online_Supermarket_Project.Models;
using Online_Supermarket_Project.ModelView;

namespace Online_Supermarket_Project.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly MyAppDbContext _context;
        public INotyfService _notyfService { get; set; }
        public CheckOutController(MyAppDbContext context, INotyfService notyfService)
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

        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index(string returnUrl = null)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanId = HttpContext.Session.GetString("CustomerId");
            MuaHangVM model = new MuaHangVM();
            if(taikhoanId != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CusId == Convert.ToInt32(taikhoanId));
                model.CusId = khachhang.CusId;
                model.FullName = khachhang.FullName;
                model.Address = khachhang.Address;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Phone;
            }
            ViewBag.GioHang = cart;
            ViewBag.listCate = _context.Category.AsNoTracking().ToList();
            return View(model);
        }

        [HttpPost]        
        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index(MuaHangVM muaHangVM)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanId = HttpContext.Session.GetString("CustomerId");
            MuaHangVM model = new MuaHangVM();
            if (taikhoanId != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CusId == Convert.ToInt32(taikhoanId));
                model.CusId = khachhang.CusId;
                model.FullName = khachhang.FullName;
                model.Address = khachhang.Address;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Phone;
                _context.Update(khachhang);
                _context.SaveChanges();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    // Khởi tạo đơn hàng
                    Order donhang = new Order();
                    donhang.CusId = model.CusId;
                    donhang.Address = model.Address;
                    donhang.OrderDate = DateTime.Now;
                    donhang.TransactStatusId = 1; //Đơn mới
                    donhang.Deleted = false;
                    donhang.Paid = false; // Chưa thanh toán
                    //donhang.Note = Utilities.UploadFile(model.Note);
                    donhang.TotalMonney = Convert.ToInt32(cart.Sum(x => x.TotalAmount));
                    _context.Add(donhang);
                    _context.SaveChanges();

                    foreach (var item in cart)
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrderId = donhang.OrderId;
                        orderDetail.ProductId = item.Product.ProductId;
                        orderDetail.Quantity = item.Amount;
                        orderDetail.Total = donhang.TotalMonney;
                        orderDetail.CreatedDate = DateTime.Now;
                        _context.Add(orderDetail);
                    }
                    _context.SaveChanges();
                    HttpContext.Session.Remove("GioHang");
                    _notyfService.Success("Đặt hàng thành công!");
                    return RedirectToAction("Success");
                }
            }catch (Exception ex)
            {
                ViewBag.listCate = _context.Category.AsNoTracking().ToList();
                ViewBag.GioHang = cart;
                return View(model);
            }
            ViewBag.listCate = _context.Category.AsNoTracking().ToList();
            ViewBag.GioHang = cart;
            return View(model);
        }

        [Route("dat-hang-tc.html", Name = "Success")]
        public IActionResult Success()
        {
            try
            {
                var taikhoanId = HttpContext.Session.GetString("CustomerId");
                if (string.IsNullOrEmpty(taikhoanId))
                {
                    return RedirectToAction("Login", "Accounts", new { returnUrl = "/dat-hang-tc.html" });
                }
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CusId == Convert.ToInt32(taikhoanId));
                var donhang = _context.Order.Where(x => x.CusId == Convert.ToInt32(taikhoanId)).OrderByDescending(x => x.OrderDate).AsNoTracking().FirstOrDefault();
                MuaHangSuccessVM muaSuccess = new MuaHangSuccessVM();
                muaSuccess.FullName = khachhang.FullName;
                muaSuccess.Address = khachhang.Address;
                muaSuccess.Email = khachhang.Email;
                muaSuccess.Phone = khachhang.Phone;
                muaSuccess.DonHangId = donhang.OrderId;
                muaSuccess.Note = donhang.Note;
                return View(muaSuccess);
            }
            catch
            {
                return View();
            }
        }
    }
}
